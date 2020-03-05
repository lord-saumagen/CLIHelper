using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Collections.ObjectModel;

namespace CommandLineHelper
{

  /// <summary>
  /// Enumeration of console output streams
  /// </summary>
  public enum ConsoleOutputStream
  {
    /// <summary>
    ///  Identifies the standard console output stream.
    /// </summary>  
    STANDARD,
    /// <summary>
    ///  Identifies the error console output stream.
    /// </summary>  
    ERROR
  }

  /// <summary>
  /// <para>  
  /// The base class of all parameter classes. Implements the parse and validation function. 
  /// </para> 
  /// <para>
  /// In order to use the parameter class in your own code, you have to create a subclass 
  /// which inherits from 'ParameterBase' and add the parameter properties which are needed 
  /// in your  CLI application.
  /// </para>
  /// </summary>
  [Usage("A description of the command usage should be assigned to the parameter object.\r\nE.g.: command [help] [version] parameter=<parameterValue> ...")]
  public class ParameterBase
  {
    private IDisplayHelper? DisplayHelper
    {
      get;
      set;
    }

    /// <summary>
    /// Returns a dictionary which holds the command line arguments
    /// from the last parse operation.
    /// </summary>
    /// <value></value>
    public Dictionary<string, string> Arguments
    {
      get;
      private set;
    } = new Dictionary<string, string>();

    /// <summary>
    /// Returns true if a help request has been detected
    /// during parsing.
    /// </summary>
    /// <value></value>
    public virtual bool IsHelpRequest
    {
      get;
      private set;
    } = false;

    /// <summary>
    /// Returns true if a version request has been detected
    /// during parsing.
    /// </summary>
    /// <value></value>
    public virtual bool IsVersionRequest
    {
      get;
      private set;
    } = false;

    /// <summary>
    /// Returns true if a parse attempt has been made and either a help
    /// request or a version request has been detected or at least one
    ///  property has been successfully parsed.
    /// </summary>
    /// <value></value>
    public virtual bool IsParsed
    {
      get
      {

        if (this.IsHelpRequest)
        {
          return true;
        }

        if (this.IsVersionRequest)
        {
          return true;
        }

        if (this.PropertyMetaInfoList.Where(item => item.ParseResult != ParseResultEnum.NOT_PARSED).Any())
        {
          return true;
        }

        return false;
      }
    }

    /// <summary>
    /// Returns true if all parameter properties passed the validation.
    /// </summary>
    /// <value></value>
    public virtual bool IsValid
    {
      get
      {
        return this.PropertyMetaInfoList.All(item => item.IsValid);
      }
    }

    /// <summary>
    /// A read only collection of 'ValidationError' objects which shows 
    /// the validation errors from the last validation process.
    /// The list might be empty.
    /// </summary>
    /// <value></value>
    public virtual ReadOnlyCollection<ValidationError> ValidationErrorList
    {
      get
      {
        return (ReadOnlyCollection<ValidationError>)this.PropertyMetaInfoList.Select(item => item.ValidationError).Where(item => item != null).Cast<ValidationError>().ToList().AsReadOnly();
      }
    }


    /// <summary>
    /// A list of 'PropertyMetaInfo' objects which hold 
    /// additional information for each parameter property.
    /// </summary>
    /// <value></value>
    public virtual List<PropertyMetaInfo> PropertyMetaInfoList
    {
      get;
      private set;
    }

    /// <summary>
    /// The command name as used on the command line interface. That might not 
    /// necessarily be the same as your program name. The 'CommandName' is set
    /// in the constructor of the parameter class.
    /// </summary>
    /// <value></value>
    public String CommandName
    {
      get;
      private set;
    }

    /// <summary>
    /// The assembly of the attached command. The 'CommandAssembly' is set in the
    /// constructor of the parameter class.
    /// </summary>
    /// <value></value>
    public Assembly CommandAssembly
    {
      get;
      private set;
    }

    /// <summary>
    /// The 'HelpIndicatorList' is a list of command line arguments
    /// which are considered a help request when detected during parse.
    /// </summary>
    public List<string> HelpIndicatorList = new List<string> { "help", "/help", "-help", "--help", "/?" };

    /// <summary>
    /// The 'VersionIndicatorList' is a list of command line arguments
    /// which are considered a version request when detected during parse.
    /// </summary>
    public List<string> VersionIndicatorList = new List<string> { "version", "/version", "-version", "--version" };

    /// <summary>
    /// The constructor of the 'ParameterBase' class. 
    /// </summary>
    /// <param name="commandName">The command as used on the command line.</param>
    /// <param name="commandAssembly">The assembly which implements the command.</param>
    /// <param name="displayHelper">A displayHelper for the message rendering.</param>
    /// <exception cref="System.ArgumentException"></exception>
    public ParameterBase(String commandName, Assembly commandAssembly, IDisplayHelper? displayHelper = null)
    {
      MethodBase? methodBase = MethodBase.GetCurrentMethod();

      if (commandAssembly == null)
      {
        throw new System.ArgumentException($"The argument '{nameof(commandAssembly)}' must not be null in function '{methodBase?.ReflectedType?.Name}.ctor'");
      }
      this.CommandAssembly = commandAssembly;

      if (String.IsNullOrWhiteSpace(commandName))
      {
        throw new System.ArgumentException($"The argument '{nameof(commandName)}' must not be null or empty in function '{methodBase?.ReflectedType?.Name}.ctor'");
      }
      this.CommandName = commandName;

      this.DisplayHelper = displayHelper;

      //
      // Fill the 'PropertyMetaInfoList' with each property that qualifies as 
      // parameter property and isn't marked as internal.
      //
      this.PropertyMetaInfoList = this.GetType().GetProperties()
      .Where(prop => prop.CanRead && prop.CanWrite)
      .Where(prop => prop.GetGetMethod() != null)
      .Where(prop => prop.GetSetMethod() != null)
      .Select(prop => new PropertyMetaInfo(prop))
      .Where(propMetaInfo => !propMetaInfo.IsInternal).ToList();
    }


    /// <summary>
    /// Processes the command line by calling the parse and validation
    /// function. 
    /// <para>
    /// If the 'showUsageOnEmptyArgs' argument is true and the 'args' 
    /// argument has no argument which qualifies as parameter argument, 
    /// the usage screen will be rendered to the console error stream
    /// and the return value will be false;
    /// </para>
    /// <para>
    /// If the validation fails because it was a help request, the
    /// help screen will be rendered to the console standard stream
    /// and the return value will be false.
    /// </para>
    /// <para>
    /// If the validation fails because it was a version request, the
    /// version screen will be rendered to the console standard stream
    /// and the return value will be false.
    /// </para>
    /// <para>
    /// If the validation fails because of invalid arguments, the
    /// validation summary screen will be rendered to the console error
    /// stream the return value will be false.
    /// </para>
    /// <para>
    /// The function returns true if the process was successful and 
    /// no screen was rendered.
    /// </para>
    /// </summary>
    /// <param name="args">
    /// The command line arguments which will be processed.
    /// </param>
    /// <param name="showUsageOnEmptyArgs">
    /// Determines whether the usage screen will be rendered if the 
    /// args array has no argument which qualifies as parameter argument.
    /// </param>
    /// <returns>true, if the process succeeded, otherwise false</returns>
    public virtual bool Process(String[] args, bool showUsageOnEmptyArgs = true)
    {
      bool IsValid;
      string output;

      this.Parse(args);

      //
      // The usage screen will be rendered to the 
      // console error stream.
      //
      if (!this.IsHelpRequest && !this.IsVersionRequest && showUsageOnEmptyArgs && this.Arguments.Count() == 0)
      {
        output = this.CreateUsage();
        ParameterBase.PrintToConsole(output, consoleOutputStream : ConsoleOutputStream.ERROR);
        return false;
      }

      IsValid = this.Validate();

      if (!IsValid)
      {
        if (this.IsHelpRequest)
        {
          output = this.CreateHelp();
          Console.Write(output);
          return IsValid;
        }
        if (this.IsVersionRequest)
        {
          output = this.CreateVersion(this);
          Console.Write(output);
          return IsValid;
        }
        output = this.CreateValidationSummary();
        //ParameterBase.PrintToConsole(output, ConsoleColor.Red);

        //
        // The validation summary screen will be rendered to the 
        // console error stream.
        //
        ParameterBase.PrintToConsole(output, consoleOutputStream: ConsoleOutputStream.ERROR, foregroundColor: ConsoleColor.Red);
        return IsValid;
      }
      return IsValid;
    }

    /// <summary>
    /// Parses the arguments provided in argument 'args' and assigns the
    /// values of those arguments, which have corresponding parameter 
    /// properties, to the properties.
    /// <param>
    /// The parse function handles the parsing for the following 
    /// types only:
    /// </param>
    /// <list type="table">  
    ///   <listheader>  
    ///     <term>Type</term>
    ///     <description>Description</description>  
    ///   </listheader>  
    ///   <item>  
    ///     <term>Boolean &amp; Boolean?</term>
    ///     <description>Parses the strings: true, false, yes, no. (Not case sensitive)</description>  
    ///   </item>  
    ///   <item>
    ///     <term>Char &amp; Char?</term>
    ///     <description>Parses a single character</description>
    ///   </item>
    ///   <item>  
    ///     <term>Number types &amp; nullable number types</term>
    ///     <description>Parses number strings for the types: 
    ///      Int16, UInt16, Int32, UInt32, Int64, UInt64, Single, Double, Decimal and their 
    ///      nullable counterparts.
    ///    </description>  
    ///   </item>  
    ///    <item>String</item>
    ///    <descriptions>Any given string.</descriptions>
    ///   </list>  
    /// </summary>
    /// <param name="args"></param>
    /// <para>
    /// If you need parse support for other types, override the 'Parse'
    /// function in your subclass and add the required parse logic.
    /// </para>
    public virtual void Parse(string[] args)
    {
      Type parameterType;
      string value;

      //
      // Reset the properties.
      //
      ResetValidationError();
      ResetParseResult();
      this.IsHelpRequest = false;
      this.Arguments.Clear();

      parameterType = this.GetType();

      //
      // Check if the arguments contain a help request.
      //
      if (args.Where(arg => HelpIndicatorList.Contains(arg.Trim().ToLower())).Count() > 0)
      {
        this.IsHelpRequest = true;
        //
        // No need for further parsing since help was requested.
        //
        return;
      }

      //
      // Check if the arguments contain a version request.
      //
      if (args.Where(arg => VersionIndicatorList.Contains(arg.Trim().ToLower())).Count() > 0)
      {
        this.IsVersionRequest = true;
        //
        // No need for further parsing since the version was requested.
        //
        return;
      }


      //
      // Fill the arguments dictionary.
      //
      foreach (var arg in args)
      {
        string[] splitArg = arg.Split(new char[] {'='}, 2);

        if (splitArg.Length == 2)
        {
          if (!this.Arguments.ContainsKey(splitArg[0].Trim().ToLower()))
          {
            this.Arguments.Add(splitArg[0].Trim().ToLower(), splitArg[1].Trim());
          }
        }
      }

      //
      // Iterate over all properties and look for matching command line
      // arguments to set the propertie values.
      // 
      foreach (var item in PropertyMetaInfoList)
      {
        KeyValuePair<string, string> argument;
        try
        {
          argument = Arguments.Where(KeyValue => KeyValue.Key == item.Name.ToLower()).Single();
        }
        catch
        {
          //
          // No matching command line argument for the current 
          // parameter property. Set the parse result to not parsed.
          //
          item.ParseResult = ParseResultEnum.NOT_PARSED;
          continue;
        }

        value = argument.Value;

        //
        // Remove leading and trailing quotes from the argument value.
        //
        if (value.StartsWith("\"") && value.EndsWith("\""))
        {
          value = value.Substring(0, value.LastIndexOf("\""));
          value = value.Substring(value.IndexOf("\"") + 1);
        }

        if (!String.IsNullOrWhiteSpace(value))
        {
          try
          {
            switch (item.Type)
            {
              case "String":
                {
                  item.PropertyInfo.SetValue(this, value);
                  item.ParseResult = ParseResultEnum.PARSE_SUCCEEDED;
                  break;
                }
              case "Boolean|Null":
              case "Boolean":
                {
                  if ((value.ToLower() == "null") && (Nullable.GetUnderlyingType(item.PropertyInfo.PropertyType) != null))
                  {
                    item.PropertyInfo.SetValue(this, null);
                  }
                  else
                  {
                    if (value.Trim().ToLower() == "yes")
                    {
                      item.PropertyInfo.SetValue(this, true);
                    }
                    else if (value.Trim().ToLower() == "no")
                    {
                      item.PropertyInfo.SetValue(this, false);
                    }
                    else
                    {
                      item.PropertyInfo.SetValue(this, Boolean.Parse(value));
                    }
                  }
                  item.ParseResult = ParseResultEnum.PARSE_SUCCEEDED;
                  break;
                }
              case "Char|Null":
              case "Char":
                {
                  if ((value.ToLower() == "null") && (Nullable.GetUnderlyingType(item.PropertyInfo.PropertyType) != null))
                  {
                    item.PropertyInfo.SetValue(this, null);
                  }
                  else
                  {
                    Type? currentType;
                    if (Nullable.GetUnderlyingType(item.PropertyInfo.PropertyType) != null)
                    {
                      currentType = Nullable.GetUnderlyingType(item.PropertyInfo.PropertyType);
                    }
                    else
                    {
                      currentType = item.PropertyInfo.PropertyType;
                    }

                    if (currentType?.Name.ToLower().IndexOf("char") > -1)
                    {
                      value = value.Replace("'", "");
                    }

                    var parseMethod = currentType?.GetMethod("Parse");
                    var parsedValue = parseMethod?.Invoke(null, new object[] { value });
                    item.PropertyInfo.SetValue(this, parsedValue);
                  }
                  item.ParseResult = ParseResultEnum.PARSE_SUCCEEDED;
                  break;
                }
              case "Int16|Null":
              case "Int16":
              case "UInt16|Null":
              case "UInt16":
              case "Int32|Null":
              case "Int32":
              case "UInt32|Null":
              case "UInt32":
              case "Int64|Null":
              case "Int64":
              case "UInt64|Null":
              case "UInt64":
              case "Single|Null":
              case "Single":
              case "Double|Null":
              case "Double":
              case "Decimal|Null":
              case "Decimal":
                {
                  if ((value.ToLower() == "null") && (Nullable.GetUnderlyingType(item.PropertyInfo.PropertyType) != null))
                  {
                    item.PropertyInfo.SetValue(this, null);
                  }
                  else
                  {
                    Type? currentType;
                    if (Nullable.GetUnderlyingType(item.PropertyInfo.PropertyType) != null)
                    {
                      currentType = Nullable.GetUnderlyingType(item.PropertyInfo.PropertyType);
                    }
                    else
                    {
                      currentType = item.PropertyInfo.PropertyType;
                    }

                    var parseMethod = currentType?.GetMethod("Parse", new Type[] { typeof(String), typeof(NumberStyles) });
                    var parsedValue = parseMethod?.Invoke(null, new object[] { value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands });
                    item.PropertyInfo.SetValue(this, parsedValue);
                  }
                  item.ParseResult = ParseResultEnum.PARSE_SUCCEEDED;
                  break;
                }
              default:
                {
                  //
                  // Parsing failed because the type isn't supported.
                  //
                  item.ParseResult = ParseResultEnum.PARSE_FAILED;
                  break;
                }
            }
          }
          catch
          {
            //
            // Assigning the command line argument value to the current 
            // parameter property failed. Set the parse result to parse failed.
            //
            item.ParseResult = ParseResultEnum.PARSE_FAILED;
          }
        }
        else
        {
          //
          // The command line argument value was an empty string
          // null or whitespace. Set the parse result to parse failed.
          //
          item.ParseResult = ParseResultEnum.PARSE_FAILED;
        }
      }
    }


    /// <summary>
    /// Resets the 'ValidationError' property of each parameter property.
    /// </summary>
    private void ResetValidationError()
    {
      foreach (var item in this.PropertyMetaInfoList)
      {
        item.ValidationError = null;
      }
    }


    /// <summary>
    /// Resets the 'ParseResult' property of each parameter property.
    /// </summary>
    private void ResetParseResult()
    {
      foreach (var item in this.PropertyMetaInfoList)
      {
        item.ParseResult = ParseResultEnum.NOT_PARSED;
      }
    }


    /// <summary>
    /// Validates all parmeter properties of the current 
    /// 'ParameterBase' class and returns true if the validation
    /// for all properties succeeded, otherwise false.
    /// </summary>
    /// <returns>Returns true if the validation passed, otherwise false.</returns>
    public virtual bool Validate()
    {
      //
      // Clear the validation error list.
      // 
      ResetValidationError();

      //
      // It's a help request. No need to validate.
      //
      if (this.IsHelpRequest)
      {
        return false;
      }

      //
      // It's a version request. No need to validate.
      //
      if (this.IsVersionRequest)
      {
        return false;
      }

      foreach (var propMetaInfo in this.PropertyMetaInfoList)
      {
        //
        // Validate each parameter property and set the 'IsValid' flag
        // of that property according to the validaton result.
        //
        ValidateProperty(propMetaInfo);
      }


      //
      // Return true if all parameter properties passed the validation.
      //
      return this.IsValid;
    }


    /// <summary>
    /// Validates a single parameter property.
    /// </summary>
    /// <param name="propertyMetaInfo"></param>
    private void ValidateProperty(PropertyMetaInfo propertyMetaInfo)
    {
      //
      // The argument is mandatory
      //
      if (propertyMetaInfo.IsMandatory)
      {
        //
        // Parsing failed because the argument was missing.
        //
        if (propertyMetaInfo.ParseResult == ParseResultEnum.NOT_PARSED)
        {
          //
          // The argument has a default value attached.
          //
          if (propertyMetaInfo.DefaultValue != null)
          {
            //
            // Try using the default value.
            //
            try
            {
              propertyMetaInfo.PropertyInfo.SetValue(this, propertyMetaInfo.DefaultValue);
            }
            catch (System.Exception ex)
            {
              propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, "", $"Setting the default value for the mandatory command line argument '{propertyMetaInfo.Name}' if invalid. The exceptions message is: '{ex.Message}'");
              return;
            }
          }
          //
          // Not parsed and no default value.
          // Create a validation error for the missing mandatroy argument.
          //
          else
          {
            propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, "", $"The mandatory command line argument '{propertyMetaInfo.Name}' is missing or the value is invalid.");
            return;
          }
        }
        //
        // Parsing failed because the command line argument value was invalid.
        //
        else if (propertyMetaInfo.ParseResult == ParseResultEnum.PARSE_FAILED)
        {
          propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, "", $"The mandatory command line argument '{propertyMetaInfo.Name}' is missing or the value is invalid.");
          return;
        }
      }
      //
      // The argument is optional
      //
      else
      {
        //
        // Parsing the argument failed
        //
        if (propertyMetaInfo.ParseResult == ParseResultEnum.NOT_PARSED)
        {
          //
          // The argument has a default value attached.
          //
          if (propertyMetaInfo.DefaultValue != null)
          {
            //
            // Try using the default value.
            //
            try
            {
              propertyMetaInfo.PropertyInfo.SetValue(this, propertyMetaInfo.DefaultValue);
            }
            catch (System.Exception ex)
            {
              propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, "", $"The default value for the command line argument '{propertyMetaInfo.Name}' is invalid. The exceptions message is: '{ex.Message}'");
              return;
            }
          }
        }
        if (propertyMetaInfo.ParseResult == ParseResultEnum.PARSE_FAILED)
        {
          propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, "", $"The value of command line argument '{propertyMetaInfo.Name}' is invalid.");
          return;
        }
      }

      //
      // Check the value against the value set.
      //
      if (propertyMetaInfo.ValueSet.Count > 0)
      {

        Type targetType = propertyMetaInfo.PropertyInfo.PropertyType;
        Type underlyingType;

        List<object> compareSet = new List<object>();

        try
        {
          foreach (var item in propertyMetaInfo.ValueSet)
          {
#pragma warning disable CS8600, CS8604
            if (Nullable.GetUnderlyingType(targetType) != null)
            {
              if (item == null)
              {
                compareSet.Add(item);
              }
              else
              {
                underlyingType = Nullable.GetUnderlyingType(targetType);
                compareSet.Add(Convert.ChangeType(item, underlyingType));
              }
            }
            else
            {
              compareSet.Add(Convert.ChangeType(item, targetType));
            }
#pragma warning restore CS8600, CS8604
          }
        }
        catch
        {
          propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, "", $"The value set attached to '{propertyMetaInfo.Name}' is not compatible with the parameter property type.");
          return;
        }

        //
        // Compare string values in lower case.
        //
#pragma warning disable CS8602
        if (targetType.ToString().ToLower().IndexOf("string") > -1)
        {
          for (int index = 0; index < compareSet.Count; index++)
          {
            compareSet[index] = compareSet[index].ToString().ToLower();
          }

          if (!compareSet.Contains((propertyMetaInfo.PropertyInfo.GetValue(this) as String).ToLower()))
          {
#pragma warning restore CS8602
            string errorString = $"The value of command line argument '{propertyMetaInfo.Name}' is not in the set of allowed values.\r\nAllowed values are:\r\n[";
            foreach (var item in compareSet)
            {
              errorString += item.ToString() + ", ";
            }
            errorString = errorString.TrimEnd(new char[] { ',', ' ' });
            errorString += "]";
            propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, "", errorString);
            return;
          }
        }
        //
        // Not a string value.
        //
        else
        {
#pragma warning disable CS8604
          if (!compareSet.Contains(propertyMetaInfo.PropertyInfo.GetValue(this)))
          {
            string errorString = $"The value of command line argument '{propertyMetaInfo.Name}' is not in the set of allowed values.\r\nAllowed values are:\r\n[";
            foreach (var item in compareSet)
            {
              errorString += item.ToString() + ", ";
            }
            errorString = errorString.TrimEnd(new char[] { ',', ' ' });
            errorString += "]";
            propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, "", errorString);
            return;
          }
#pragma warning restore CS8604
        }
      }

      //
      // Check all validaton attributes
      //
      foreach (ValidationAttributeBase? validationAttribute in propertyMetaInfo.ValidationAttributeList)
      {
        if (validationAttribute != null)
        {
          validationAttribute.Validate(propertyMetaInfo, this);
          if (propertyMetaInfo.ValidationError != null)
          {
            //
            // Stop execution on the first validation error.
            //
            return;
          }
        }
      }
    }


    /// <summary>
    /// The function will write the provided 'output' to the
    /// specified console stream using the provided 'foregroundColor' and 
    /// 'backgroundColor'. 
    /// After printing the original console colors get restored.
    /// <para>
    ///   The default colors are:
    /// </para>
    /// <para>
    ///   The output stream to use. Default is the 
    ///   standard output.
    /// </para>
    /// <para>
    ///   The backgroundColor default is ConsoleColor.Black.
    /// </para>
    /// <para>
    ///   The foregroundColor default is ConsoleColor.White.
    /// </para>
    /// </summary>
    /// <param name="output"></param>
    /// <param name="consoleOutputStream">The output stream to use. Default is the ConsoleOutputStream.STANDARD.</param>
    /// <param name="foregroundColor"></param>
    /// <param name="backgroundColor"></param>
    public static void PrintToConsole(string output, ConsoleOutputStream consoleOutputStream = ConsoleOutputStream.STANDARD, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
      ConsoleColor savedBackgroundColor;
      ConsoleColor savedForegroundColor;

      if (!String.IsNullOrWhiteSpace(output))
      {
        savedBackgroundColor = Console.BackgroundColor;
        savedForegroundColor = Console.ForegroundColor;

        Console.ForegroundColor = foregroundColor;
        Console.BackgroundColor = backgroundColor;

        if(consoleOutputStream == ConsoleOutputStream.STANDARD)
        {
          Console.Write(output);
        }
        else
        {
          Console.Error.Write(output);
          Console.Error.Flush();
        }

        Console.BackgroundColor = savedBackgroundColor;
        Console.ForegroundColor = savedForegroundColor;
      }
    }


    // ************************************************************************
    // IDisplayHelper mapping
    // ************************************************************************

    /// <summary>
    /// If a 'DisplayHelper' was provided during construction,
    /// the help screen for this parameter object will be created.
    /// Otherwise an empty string will be returned.
    /// </summary>
    /// <param name="screenWidth"></param>
    /// <returns>The resulting string</returns>
    public virtual string CreateHelp(int screenWidth = 80)
    {
      if (this.DisplayHelper != null)
      {
        return this.DisplayHelper.CreateHelp(this, screenWidth: screenWidth);
      }
      return string.Empty;
    }

    /// <summary>
    /// If a 'DisplayHelper' was provided during construction,
    /// a usage screen for this parameter object will be created.
    /// Otherwise an empty string will be returned.
    /// </summary>
    /// <returns>The resulting string</returns>
    public virtual string CreateUsage()
    {
      if (this.DisplayHelper != null)
      {
        return this.DisplayHelper.CreateUsage(this);
      }
      return string.Empty;
    }

    /// <summary>
    /// If a 'DisplayHelper' was provided during construction,
    /// a validation summary screen for this parameter object will be created.
    /// Otherwise an empty string will be returned.
    /// <para>
    /// The message provided in argument 'message' will be shown on top of the summary.
    /// </para>
    /// <para>
    /// The summary will be returned as a string with a line length matching with the 
    /// value of argument screenWidth.
    /// </para>
    /// </summary>
    /// <param name="screenWidth"></param>
    /// <returns>The resulting string</returns>
    public virtual string CreateValidationSummary(int screenWidth = 80)
    {
      if (this.DisplayHelper != null)
      {
        return this.DisplayHelper.CreateValidationSummary(this, screenWidth: screenWidth);
      }
      return string.Empty;
    }

    /// <summary>
    /// If a 'DisplayHelper' was provided during construction,
    /// a validation summary screen for this parameter object will be created.
    /// Otherwise an empty string will be returned.
    /// <para>
    /// The message provided in argument 'message' will be shown on top of the summary.
    /// </para>
    /// <para>
    /// The summary will be returned as a string with a line length matching with the 
    /// value of argument screenWidth.
    /// </para>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="screenWidth"></param>
    /// <returns>The resulting string</returns>
    public virtual string CreateValidationSummary(string message, int screenWidth = 80)
    {
      MethodBase? methodBase = MethodBase.GetCurrentMethod();

      if (String.IsNullOrWhiteSpace(message))
      {
        throw new System.ArgumentException($"Argument '{nameof(message)}' must not be null or empty in function '{methodBase?.ReflectedType?.Name}.{methodBase?.Name}'.");
      }
      if (this.DisplayHelper != null)
      {
        return this.DisplayHelper.CreateValidationSummary(this, message, screenWidth);
      }
      return string.Empty;
    }

    /// <summary>
    /// If a 'DisplayHelper' was provided during construction,
    /// a version screen for the provided 'commandObject' will be returned.
    /// Otherwise an empty string will be returned.
    /// <para>
    /// The command object should be the command line program who's version should be displayed.
    /// </para>
    /// </summary>
    /// <param name="commandObject"></param>
    /// <returns>The version as string or the message: 'No version info available.'</returns>
    public virtual string CreateVersion(Object commandObject)
    {
      if (this.DisplayHelper != null)
      {
        return this.DisplayHelper.CreateVersion(commandObject);
      }
      return string.Empty;
    }

    /// <summary>
    /// If a 'DisplayHelper' was provided during construction,
    /// a version screen for the provided 'commandAssembly' will be returned.
    /// Otherwise an empty string will be returned.
    /// <para>
    /// The command object should be the command line program who's version should be displayed.
    /// </para>
    /// </summary>
    /// <param name="commandAssembly"></param>
    /// <returns>The version as string or the message: 'No version info available.'</returns>
    public virtual string CreateVersion(Assembly commandAssembly)
    {
      if (this.DisplayHelper != null)
      {
        return this.DisplayHelper.CreateVersion(commandAssembly);
      }
      return string.Empty;
    }

  }// END class
}// END namespace