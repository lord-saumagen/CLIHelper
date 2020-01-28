using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;

namespace CommandLineHelper
{

  /// <summary>
  /// The 'DisplayHelper' class is a reference implementation of the 'IDisplayHelper'
  /// interface.
  /// <para>
  /// The purpose of the 'DisplayHelper' class is to simplify the screen rendering.
  /// The class offers functions to create the output strings for the help screen,
  /// usage screen, the validation summary screen and the version screen.
  /// </para>
  /// <para>
  ///   <see cref="IDisplayHelper" />
  /// </para>
  /// </summary>
  public class DisplayHelper : IDisplayHelper
  {

    const char CornerBottomLeft = '\u255A'; // ╚
    const char CornerBottomRight = '\u255D'; // ╝
    const char CornerTopLeft = '\u2554'; //╔
    const char CornerTopRight = '\u2557'; // ╗
    const char HorizontalLine = '\u2550'; // ═
    const char VerticalLine = '\u2551'; // ║
    const char TJunctionUp = '\u2566'; // ╦
    const char TJunctionDown = '\u2569'; // ╩

    const char TJunctionLeft = '\u2560'; // ╠
    const char TJunctionRight = '\u2563'; // ╣
    const char CrossJunction = '\u256C'; // ╬

    const char Asterisk = '*'; // *


    // ************************************************************************
    // IDisplayHelper implementation START
    // ************************************************************************

    /// <summary>
    /// Creates the help screen for the parameter object provided in argument 
    /// 'parameterObject'. 
    /// <para>
    /// The line length of the resulting help text will match with value provide in argument 'screenWidth'
    /// as long a the value is greater than the minimum screen line length. Otherwise the minimum screen
    /// line length will be used.
    /// </para>
    /// <para>
    /// The result string is supposed to be rendered on the command line.
    /// </para>
    /// </summary>
    /// <param name="parameterObject"></param>
    /// <param name="screenWidth"></param>
    /// <returns>The resulting string</returns>
    public virtual string CreateHelp(ParameterBase parameterObject, int screenWidth = 80)
    {
      string result;
      int longestArgumentLength;
      int longestTypeLength;
      int longestDefaultLength;
      string help;
      List<string> helpLines;

      result = string.Empty;
      longestArgumentLength = 0;

      if (parameterObject == null)
      {
        return result;
      }

      HelpAttribute? helpAttribute = (HelpAttribute?)parameterObject.GetType().GetCustomAttribute(typeof(HelpAttribute));

      //
      // If a help text is provided in the command attributes show that text first.
      //
      if ((helpAttribute != null) && !String.IsNullOrWhiteSpace(helpAttribute.Help))
      {
        help = helpAttribute.Help;
      }
      else
      {
        help = string.Empty;
      }


      if (parameterObject.PropertyMetaInfoList.Count == 0)
      {
        result = "The current parameter object has no parameter property!";
      }
      else
      {

        longestArgumentLength = parameterObject.PropertyMetaInfoList.Max(item => item.Name.Length);
        longestTypeLength = parameterObject.PropertyMetaInfoList.Max(item => item.Type.Length);
        longestDefaultLength = parameterObject.PropertyMetaInfoList.Max(item => (item.DefaultValue?.ToString()?.Length ?? 0));

        longestArgumentLength = Math.Max(longestArgumentLength, "[Parameter]".Length); // [Parameter] = 11 characters min
        longestTypeLength = Math.Max(longestTypeLength, "[Type]".Length); // [Type] = 6 characters min 
        longestDefaultLength = Math.Max(longestDefaultLength, "[Default]".Length); // [Default] = 9 characters min

        //
        // Expand the screen width if there isn't at least 20 characters left for the description.
        // Minimum length for the help screen is 11 + 6 + 9 + 20 for parameter, type default and description
        // plus additional 15 characters for spaces and borders. That is 61 characters minimum.
        //
        if((longestArgumentLength + longestTypeLength + longestDefaultLength + 20) > screenWidth)
        {
          screenWidth = (longestArgumentLength + longestTypeLength + longestDefaultLength + 20);
        }

        if(!String.IsNullOrWhiteSpace(help))
        {
          //
          // Format the help text from the help attribute.
          //
          helpLines = CreateWrappedLines(help, screenWidth, leadingSpaces:0, keepEmptyLines:true);
          result += "\r\n" + String.Join("\r\n", helpLines) + "\r\n\r\n";
        }
        

        result += CreateHelpHeader(screenWidth);

        result += CreateHelpTop(screenWidth, longestArgumentLength, longestTypeLength, longestDefaultLength);

        result += CreateHelpAndVersionArgumentHelpLines(parameterObject.HelpIndicatorList.First(), "Shows the help screen. (This screen).", screenWidth, longestArgumentLength, longestTypeLength, longestDefaultLength);
        result += CreateHelpSeparatorLine(screenWidth, longestArgumentLength, longestTypeLength, longestDefaultLength);
        result += CreateHelpAndVersionArgumentHelpLines(parameterObject.VersionIndicatorList.First(), "Shows the command version number if available or an empty string. (Nothing)", screenWidth, longestArgumentLength, longestTypeLength, longestDefaultLength);
        result += CreateHelpSeparatorLine(screenWidth, longestArgumentLength, longestTypeLength, longestDefaultLength);

        for (int index = 0; index < parameterObject.PropertyMetaInfoList.Count; index++)
        {
          result += CreateHelpLines(parameterObject.PropertyMetaInfoList[index], screenWidth, longestArgumentLength, longestTypeLength, longestDefaultLength);
          if (index < parameterObject.PropertyMetaInfoList.Count - 1)
          {
            result += CreateHelpSeparatorLine(screenWidth, longestArgumentLength, longestTypeLength, longestDefaultLength);
          }
        }

        result += CreateHelpBottom(screenWidth, longestArgumentLength, longestTypeLength, longestDefaultLength);
      }
      result += "\r\n\r\n";
      result += CreateUsage(parameterObject);

      return result;
    }


    /// <summary>
    /// Creates a usage description for the command associated with the provided
    /// 'parameterObject'. 
    /// <para>
    /// The usage description is either the one provided with
    /// the 'UsageAttribute' or a generic description created from the known 
    /// parameter attributes.
    /// </para>
    /// <para>
    /// The result string is supposed to be rendered on the command line.
    /// </para>
    /// </summary>
    /// <param name="parameterObject"></param>
    /// <param name="screenWidth"></param>
    /// <returns>The resulting string</returns>
    public virtual string CreateUsage(ParameterBase parameterObject, int screenWidth = 80)
    {
      string result;
      string usage;

      result = String.Empty;

      result += CreateUsageHeader(screenWidth);
      result += "Usage:\r\n\r\n";
      UsageAttribute? usageAttribute = (UsageAttribute?)parameterObject.GetType().GetCustomAttribute(typeof(UsageAttribute));

      //
      // If a usage description is provided in the command attributes return that description.
      //
      if ((usageAttribute != null) && !String.IsNullOrWhiteSpace(usageAttribute.Usage))
      {
        usage = usageAttribute.Usage;
      }
      //
      // Create a usage description from the known parameter attributes.
      //
      else
      {
        usage = parameterObject.CommandName + " ";
        usage += "[" + parameterObject.HelpIndicatorList.First() + "] ";
        usage += "[" + parameterObject.VersionIndicatorList.First() + "] ";
        foreach (var metaInfo in parameterObject.PropertyMetaInfoList)
        {
          if (metaInfo.IsMandatory)
          {
            usage += metaInfo.Name + "=" + "<" + metaInfo.Type + "> ";
          }
          else
          {
            usage += "[" + metaInfo.Name + "=" + "<" + metaInfo.Type + ">" + "] ";
          }
        }
      }
      
      List<string> lines = CreateWrappedLines(usage, screenWidth, 0);
      return result + String.Join("\r\n", lines);
    }


    /// <summary>
    /// Creates a summary of the validation errors of the parameter object 
    /// provided in argument 'parameterObject'.
    /// <para>
    /// The message provided in argument 'message' will be shown on top of the summary.
    /// </para>
    /// <para>
    /// The summary will be returned as a string with a line length matching with the 
    /// value of argument screenWidth.
    /// </para>
    /// <para>
    /// The result string is supposed to be rendered on the command line.
    /// </para>
    /// </summary>
    /// <param name="parameterObject"></param>
    /// <param name="message"></param>
    /// <param name="screenWidth"></param>
    /// <returns>The resulting string</returns>
    public virtual String CreateValidationSummary(ParameterBase parameterObject, string message = "One or more of the command line arguments are invalid.", int screenWidth = 80)
    {
      string result;
      result = String.Empty;

      if (parameterObject == null)
      {
        return result;
      }

      result = CreateValidationSummaryMessage(message, screenWidth);
      result += CreateValidationSummaryBody(parameterObject, screenWidth);
      return result;
    }


    /// <summary>
    /// Returns the version of the provided 'commandObject' as string.
    /// <para>
    /// The command object should be the command line program who's version should be displayed.
    /// </para>
    /// <para>
    /// The result string is supposed to be rendered on the command line.
    /// </para>
    /// </summary>
    /// <param name="commandObject"></param>
    /// <returns>The version as string or the message: 'No version info available.'</returns>
    public virtual String CreateVersion(Object commandObject)
    {
      Assembly? commandObjectAssembly;

      if (commandObject == null)
      {
        return "";
      }

      commandObjectAssembly = Assembly.GetAssembly(commandObject.GetType());
      return CreateVersion(commandObjectAssembly);
    }

    /// <summary>
    /// Returns the version of the provided 'commandObjectAssembly' as string.
    /// <para>
    /// The command object should be the assembly of the command line program who's version should be displayed.
    /// </para>
    /// <para>
    /// The result string is supposed to be rendered on the command line.
    /// </para>
    /// </summary>
    /// <param name="commandObjectAssembly"></param>
    /// <returns>The version as string or the message: 'No version info available.'</returns>
    public virtual String CreateVersion(Assembly? commandObjectAssembly)
    {
      string? version;

      if (commandObjectAssembly == null)
      {
        return "";
      }

      version = commandObjectAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
      if (version == null)
      {
        return "No version info available.";
      }
      return version;
    }


    // ************************************************************************
    // IDisplayHelper implementation END
    // ************************************************************************

    private static string CreateValidationSummaryMessage(string message, int screenWidth)
    {
      string result;

      //╔═══════════════════════════════════════════════════════════════════════════════════╗
      //║_Message_..........................................................................║
      //╚═══════════════════════════════════════════════════════════════════════════════════╝

      result = string.Empty;
      message = message.Trim();
      List<string> messageLines;

      messageLines = CreateWrappedLines(message, screenWidth - 2);
      result += "" + CornerTopLeft + new string(HorizontalLine, screenWidth - 2) + CornerTopRight + "\r\n";
      foreach (string line in messageLines)
      {
        result += "" + VerticalLine + line + VerticalLine + "\r\n";
      }
      result += "" + CornerBottomLeft + new string(HorizontalLine, screenWidth - 2) + CornerBottomRight + "\r\n";

      return result;
    }

    private static string CreateValidationSummaryBody(ParameterBase parameterObject, int screenWidth)
    {
      string result;
      int longestArgumentLength;
      int errorMessageSpace;

      //╔═[Argument]═╦═[Error message]════════════════════════════════════════════════════════╗
      //║_abcdefg_.  ║_abcdefghijklmnop_......................................................║
      //╚════════════╩════════════════════════════════════════════════════════════════════════╝
      //             |--------------------     errorMessageSpace     -------------------------|

      //
      // Create top with column names
      //
      longestArgumentLength = parameterObject.ValidationErrorList.Max(item => item.PropertyMetaInfo.Name.Length);
      longestArgumentLength = Math.Max(longestArgumentLength, "[Argument]".Length);
      errorMessageSpace = screenWidth - longestArgumentLength - 5; // screenWidth - longestArgumentLength - 2xSPACE -3x║
      result = string.Empty;
      result += CornerTopLeft;

      result += HorizontalLine + "[Argument]" + new string(HorizontalLine, longestArgumentLength - "[Argument]".Length) + HorizontalLine;
      result += TJunctionUp;
      result += HorizontalLine + "[Error message]" + new string(HorizontalLine, errorMessageSpace - "[Error message]".Length - 1);
      result += CornerTopRight + "\r\n";

      for (int index = 0; index < parameterObject.ValidationErrorList.Count; index++)
      {
        //
        // Create line
        //
        string argumentName = parameterObject.ValidationErrorList[index].PropertyMetaInfo.Name;
        string errorMessage = parameterObject.ValidationErrorList[index].PropertyMetaInfo.ValidationError?.Message ?? "";
        List<string> errorMessageLines = CreateWrappedLines(errorMessage, errorMessageSpace - 1);
        result += VerticalLine;
        result += " " + argumentName + new string(' ', longestArgumentLength - argumentName.Length) + " ";
        result += VerticalLine;
        result += errorMessageLines[0] + " ";
        result += VerticalLine + "\r\n";

        if (errorMessageLines.Count > 1)
        {
          for (int linesIndex = 1; linesIndex < errorMessageLines.Count; linesIndex++)
          {
            result += VerticalLine;
            result += " " + new string(' ', longestArgumentLength) + " ";
            result += VerticalLine;
            result += errorMessageLines[linesIndex] + " ";
            result += VerticalLine + "\r\n";
          }
        }

        if (index < parameterObject.ValidationErrorList.Count - 1)
        {
          //
          // Create separator
          //
          result += TJunctionLeft;
          result += new string(HorizontalLine, longestArgumentLength + 2);
          result += CrossJunction;
          result += new string(HorizontalLine, errorMessageSpace);
          result += TJunctionRight + "\r\n";
        }
      }

      //
      // Create bottom
      //
      result += CornerBottomLeft;
      result += new string(HorizontalLine, longestArgumentLength + 2);
      result += TJunctionDown;
      result += new string(HorizontalLine, errorMessageSpace);
      result += CornerBottomRight + "\r\n";

      return result;
    }


    /// <summary>
    /// The function creates the top of the help screen.
    /// </summary>
    /// <param name="screenWidth"></param>
    /// <param name="longestArgumentLength"></param>
    /// <param name="longestTypeLength"></param>
    /// <param name="longestDefaultLength"></param>
    /// <returns></returns>
    private static string CreateHelpTop(int screenWidth, int longestArgumentLength, int longestTypeLength, int longestDefaultLength)
    {
      string result;

      longestArgumentLength = Math.Max(longestArgumentLength, "[Parameter]".Length);
      longestTypeLength = Math.Max(longestTypeLength, "[Type]".Length);
      longestDefaultLength = Math.Max(longestDefaultLength, "[Default]".Length);

      //╔═╦═[Parameter]═══════╦═[Type]═╦═[Default]═╦═[Description]═════════════════════════════════════╗
      //║X║_longestArgument.._║_bool.._║_default.._║_description......................................_║
      result = string.Empty;
      result += CornerTopLeft;
      result += HorizontalLine;
      result += TJunctionUp;


      result += HorizontalLine + "[Parameter]" + new string(HorizontalLine, longestArgumentLength - "[Parameter]".Length) + HorizontalLine;
      result += TJunctionUp;
      result += HorizontalLine + "[Type]" + new string(HorizontalLine, longestTypeLength - "[Type]".Length) + HorizontalLine;
      result += TJunctionUp;
      result += HorizontalLine + "[Default]" + new string(HorizontalLine, longestDefaultLength - "[Default]".Length) + HorizontalLine;
      result += TJunctionUp;
      result += HorizontalLine + "[Description]";
      result += new string(HorizontalLine, screenWidth - result.Length - 1);
      result += CornerTopRight;
      return result + "\r\n";
    }


    /// <summary>
    /// The function creates a separator line which should be placed between
    /// two entries in the help screen.
    /// </summary>
    /// <param name="screenWidth"></param>
    /// <param name="longestArgumentLength"></param>
    /// <param name="longestTypeLength"></param>
    /// <param name="longestDefaultLength"></param>
    /// <returns></returns>
    private static string CreateHelpSeparatorLine(int screenWidth, int longestArgumentLength, int longestTypeLength, int longestDefaultLength)
    {
      string result;

      result = string.Empty;

      result += TJunctionLeft;
      result += HorizontalLine;
      result += CrossJunction;
      result += new String(HorizontalLine, longestArgumentLength + 2);
      result += CrossJunction;
      result += new String(HorizontalLine, longestTypeLength + 2);
      result += CrossJunction;
      result += new String(HorizontalLine, longestDefaultLength + 2);
      result += CrossJunction;
      result += new String(HorizontalLine, screenWidth - result.Length - 1);
      result += TJunctionRight + "\r\n";

      return result;
    }


    /// <summary>
    /// The function creates the help lines for the 'version' or 'help' indicator which are
    /// part of the help screen.
    /// </summary>
    /// <param name="indicator">One of the elements from the version/help indicator list</param>
    /// <param name="description">The description to show in the help screen</param>
    /// <param name="screenWidth"></param>
    /// <param name="longestArgumentLength"></param>
    /// <param name="longestTypeLength"></param>
    /// <param name="longestDefaultLength"></param>
    /// <returns></returns>
    private static string CreateHelpAndVersionArgumentHelpLines(string indicator, string description, int screenWidth, int longestArgumentLength, int longestTypeLength, int longestDefaultLength)
    {
      List<String> descriptionLines;

      descriptionLines = CreateWrappedLines(description, screenWidth - longestArgumentLength - longestTypeLength - longestDefaultLength - 14);

      for (int index = 0; index < descriptionLines.Count; index++)
      {
        string leadLine = string.Empty;
        string defaultValueStr = string.Empty;
        if (index == 0)
        {
          leadLine += VerticalLine;
          leadLine += "-";
          leadLine += VerticalLine + " ";
          leadLine += indicator + new String(' ', longestArgumentLength - indicator.Length) + " ";
        }
        else
        {
          leadLine += VerticalLine + " " + VerticalLine + " ";
          leadLine += new String(' ', longestArgumentLength) + " ";
        }
        leadLine += VerticalLine + " ";
        leadLine += new String(' ', longestTypeLength) + " ";
        leadLine += VerticalLine + " ";
        leadLine += new String(' ', longestDefaultLength) + " ";
        leadLine += VerticalLine;

        descriptionLines[index] = leadLine + descriptionLines[index] + " " + VerticalLine + "\r\n";
      }

      return String.Concat(descriptionLines);
    }

    private static string CreateHelpLines(PropertyMetaInfo propertyMetaInfo, int screenWidth, int longestArgumentLength, int longestTypeLength, int longestDefaultLength)
    {
      List<String> descriptionLines;
      string description;

      description = propertyMetaInfo.Description ?? propertyMetaInfo.Name + "<" + propertyMetaInfo.Type.Replace("|Null", "") + ">";
      descriptionLines = CreateWrappedLines(description, screenWidth - longestArgumentLength - longestTypeLength - longestDefaultLength - 14);

      for (int index = 0; index < descriptionLines.Count; index++)
      {
        string leadLine = string.Empty;
        string defaultValueStr = string.Empty;
        if (index == 0)
        {
          leadLine += VerticalLine;
          if (propertyMetaInfo.IsMandatory)
          {
            leadLine += "+";
          }
          else
          {
            leadLine += "-";
          }
          leadLine += VerticalLine + " ";
          leadLine += propertyMetaInfo.Name + new String(' ', longestArgumentLength - propertyMetaInfo.Name.Length) + " ";
          leadLine += VerticalLine + " ";
          leadLine += propertyMetaInfo.Type + new String(' ', longestTypeLength - propertyMetaInfo.Type.Length) + " ";
          leadLine += VerticalLine + " ";

          if (propertyMetaInfo.DefaultValue != null)
          {
            defaultValueStr = propertyMetaInfo.DefaultValue?.ToString() ?? "";
          }
          else if (propertyMetaInfo.Type.IndexOf("|Null") > -1)
          {
            defaultValueStr = "Null";
          }

          if((propertyMetaInfo.Type == "Char") && (propertyMetaInfo.DefaultValue != null) && ( ((Char) propertyMetaInfo.DefaultValue) == Char.MinValue))
          {
            leadLine += "\\0" + new String(' ', longestDefaultLength - ("\\0".Length)) + " ";
          }
          else
          {
            leadLine += defaultValueStr + new String(' ', longestDefaultLength - (defaultValueStr.Length)) + " ";
          }
          leadLine += VerticalLine;
        }
        else
        {
          leadLine += VerticalLine + " " + VerticalLine + " ";
          leadLine += new String(' ', longestArgumentLength) + " ";
          leadLine += VerticalLine + " ";
          leadLine += new String(' ', longestTypeLength) + " ";
          leadLine += VerticalLine + " ";
          leadLine += new String(' ', longestDefaultLength) + " ";
          leadLine += VerticalLine;
        }
        descriptionLines[index] = leadLine + descriptionLines[index] + " " + VerticalLine + "\r\n";
      }
      return String.Concat(descriptionLines);
    }


    /// <summary>
    /// The function creates the bottom of the help screen.
    /// </summary>
    /// <param name="screenWidth"></param>
    /// <param name="longestArgumentLength"></param>
    /// <param name="longestTypeLength"></param>
    /// <param name="longestDefaultLength"></param>
    /// <returns></returns>
    private static string CreateHelpBottom(int screenWidth, int longestArgumentLength, int longestTypeLength, int longestDefaultLength)
    {
      string result;

      longestArgumentLength = Math.Max(longestArgumentLength, "[Parameter]".Length);
      longestTypeLength = Math.Max(longestTypeLength, "[Type]".Length);
      longestDefaultLength = Math.Max(longestDefaultLength, "[Default]".Length);

      //║X║_longestArgument.._║_bool.._║_true   .._║_description....................................._║
      //╚═╩═══════════════════╩════════╩═══════════╩══════════════════════════════════════════════════╝

      result = string.Empty;
      result += CornerBottomLeft;
      result += HorizontalLine;
      result += TJunctionDown;

      result += new string(HorizontalLine, longestArgumentLength + 2);
      result += TJunctionDown;
      result += new string(HorizontalLine, longestTypeLength + 2);
      result += TJunctionDown;
      result += new string(HorizontalLine, longestDefaultLength + 2);
      result += TJunctionDown;

      result += new string(HorizontalLine, screenWidth - result.Length - 1);
      result += CornerBottomRight;
      return result + "\r\n";
    }


    private static string CreateHelpHeader(int screenWidth)
    {
      string result;

      //|1|-                                    76                                    -| 
      //|0|_01234567890123456789012345678901234567890123456789012345678901234567890123_|
      //╔═╦════════════════════════════════════════════════════════════════════════════╗
      //║X║_Key description............................... ..........................._║
      //╚═╩════════════════════════════════════════════════════════════════════════════╝

      result = string.Empty;
      result +=  "" + CornerTopLeft + HorizontalLine + TJunctionUp + new string(HorizontalLine, screenWidth - 4) + CornerTopRight + "\r\n";
      result +=  "" + VerticalLine + "+" + VerticalLine + (" Argument is mandatory." + new string(' ', screenWidth)).Substring(0, screenWidth - 4) + VerticalLine + "\r\n";
      result +=  "" + VerticalLine + "-" + VerticalLine + (" Argument is optional." + new string(' ', screenWidth)).Substring(0, screenWidth - 4) + VerticalLine + "\r\n";
      result +=  "" + CornerBottomLeft + HorizontalLine + TJunctionDown + new string(HorizontalLine, screenWidth - 4) + CornerBottomRight + "\r\n";
      result += "\r\n";

      return result;
    }


    private static string CreateUsageHeader(int screenWidth)
    {
      string result;
      List<string> wrappedLines;
      List<string> printLines;


      //|-  7  -|-                                 70                                 -| 
      //|_01234_|_01234567890123456789012345678901234567890123456789012345678901234567_|
      //╔═══════╦══════════════════════════════════════════════════════════════════════╗
      //║_<...>-║_Argument value type and/or description.............................._║
      //║_     -║_E.g. age=<Uint32, value must be greater than 18>...................._║
      //║_<.|.>_║_Argument value type and/or description with alternative............._║
      //║_     _║_E.g. married=<Boolean | Null>......................................._║
      //║_[...]-║_Argument is optional..E.g. [email=<String>]........................._║
      //╚═══════╩══════════════════════════════════════════════════════════════════════╝

      printLines = new List<string>();

      printLines.Add("" + CornerTopLeft + new String(HorizontalLine, 7)  + TJunctionUp + new string(HorizontalLine, screenWidth - 10) + CornerTopRight + "\r\n");

      wrappedLines = CreateWrappedLines("Argument value type and/or description.\r\nE.g. \"age=<Uint32> Value must be greater than 18.\"", screenWidth - 10);
      printLines.AddRange( CreateHelpHeaderPrintLines(wrappedLines, " <...> "));

      wrappedLines = CreateWrappedLines("Argument value type and/or description with alternative.\r\nE.g. \"married=<Boolean|Null> Enter 'yes', 'no' or 'null' if unknown.\"", screenWidth - 10);
      printLines.AddRange( CreateHelpHeaderPrintLines(wrappedLines, " <.|.> "));

      wrappedLines = CreateWrappedLines("Argument is optional. E.g. \"[email=<String>]\"", screenWidth - 10);
      printLines.AddRange( CreateHelpHeaderPrintLines(wrappedLines, " [...] "));

      printLines.Add("" + CornerBottomLeft + new String(HorizontalLine, 7)  + TJunctionDown + new string(HorizontalLine, screenWidth - 10) + CornerBottomRight + "\r\n");

      result = String.Join(null, printLines);
      result += "\r\n";

      return result;
    }


    private static List<string> CreateHelpHeaderPrintLines(List<string> wrappedLines, string marker)
    {
      List<string> result;

      result = new List<string>();

      for(int index = 0; index < wrappedLines.Count; index ++)
      {
        string line = string.Empty;
        if(index == 0)
        {
           line += "" + VerticalLine + marker + VerticalLine;    
        }
        else
        {
           line += "" + VerticalLine + "       " + VerticalLine;    
        }
        line += wrappedLines[index] +  VerticalLine + "\r\n";
        result.Add(line);
      }

      return result;
    }


    /// <summary>
    /// Wraps the string provided in argument 'text' at least at the
    /// maximum line length provided in argument 'maxLineLength'.
    /// The function respects the line breaks which might already
    /// part of the text. 
    /// The function returns the result as a list of strings.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="maxLineLength"></param>
    /// <param name="leadingSpaces"></param>
    /// <param name="keepEmptyLines"></param>
    /// <returns></returns>
    public static List<string> CreateWrappedLines(string text, int maxLineLength, int leadingSpaces = 1, bool keepEmptyLines = false)
    {
      List<string> sourceLines;
      List<string> resultLines;

      sourceLines = new List<string>();
      resultLines = new List<string>();

      text = text.Trim();

      sourceLines = BreakTextAtLineBreak(text, keepEmptyLines:keepEmptyLines);

      //
      // Check that each source line fits into the available
      // space. Add line breaks if necessary.
      //
      foreach (var sourceLine in sourceLines)
      {
        var sourceLineTrimmed = sourceLine.Trim();
        if (sourceLineTrimmed.Length > (maxLineLength - leadingSpaces))
        {
          resultLines.AddRange(BreakLineAtMaxLength(sourceLineTrimmed, maxLineLength - leadingSpaces));
        }
        else
        {
          resultLines.Add(sourceLineTrimmed);
        }
      }

      //
      // Add leading and trailing spaces.
      //
      for (int index = 0; index < resultLines.Count; index++)
      {
        resultLines[index] = "" + new String(' ', leadingSpaces) + resultLines[index] + new String(' ', maxLineLength - resultLines[index].Length - leadingSpaces);
      }

      return resultLines;
    }


    /// <summary>
    /// The function breaks the text provided in argument 'text'
    /// at the line breaks found in that text and returns the 
    /// result as a list of strings.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="keepEmptyLines"></param>
    /// <param name="trimLines"></param>
    /// <returns></returns>
    public static List<String> BreakTextAtLineBreak(string text, bool keepEmptyLines = false, bool trimLines = true)
    {
      List<string> resultLines;

      resultLines = new List<string>();

      if (text.IndexOf("\r\n") > -1)
      {
        text = text.Replace("\r\n", "\r");
      }

      if ((text.IndexOf("\r") > -1) || (text.IndexOf("\n") > -1))
      {
        if(keepEmptyLines)
        {
          resultLines.AddRange(text.Split(new char[] { '\r', '\n' }, StringSplitOptions.None));
        }
        else
        {
          resultLines.AddRange(text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
        }
      }
      else
      {
        resultLines.Add(text);
      }

      if (trimLines)
      {
        for (int index = 0; index < resultLines.Count; index++)
        {
          resultLines[index] = resultLines[index].Trim();
        }
      }

      return resultLines;
    }


    /// <summary>
    /// Break the line at the last line break indicator at a position lower
    /// than 'maxLength' if possible. Otherwise breaks the line at 'maxLength'
    /// even if it breaks a word.
    /// </summary>
    /// <param name="line"></param>
    /// <param name="maxLength"></param>
    /// <param name="trimLines"></param>
    /// <returns></returns>
    private static List<string> BreakLineAtMaxLength(string line, int maxLength, bool trimLines = true)
    {
      List<string> resultLines;
      List<char> lineCharList;
      List<char> lineBreakIndicators = new List<char>() { ' ', '.', ':', '!', '?', ',' };
      resultLines = new List<string>();

      if (trimLines)
      {
        line = line.Trim();
      }

      if (line.Length <= maxLength)
      {
        resultLines.Add(line);
      }
      else
      {
        lineCharList = line.ToCharArray().ToList();
        var lineSegment = lineCharList.Take(maxLength);
        bool brokeOnIndicator = false;

        //
        // Search for a line break indicator backwards and break 
        // the line at that indicator position.
        //
        for (int index = maxLength - 1; index > 0; index--)
        {
          if (lineBreakIndicators.Contains(lineSegment.ElementAt(index)))
          {
            resultLines.Add(line.Substring(0, index + 1));
            brokeOnIndicator = true;
            resultLines.AddRange(BreakLineAtMaxLength(line.Trim().Substring(index + 1), maxLength, trimLines));
            break;
          }
        }

        //
        // No line break indicator available. Break the line at
        // the 'maxLength' position, even if it rips words apart.
        //
        if (!brokeOnIndicator)
        {
          resultLines.Add(line.Substring(0, maxLength));
          resultLines.AddRange(BreakLineAtMaxLength(line.Trim().Substring(maxLength), maxLength, trimLines));
        }
      }

      if (trimLines)
      {
        for (int index = 0; index < resultLines.Count; index++)
        {
          resultLines[index] = resultLines[index].Trim();
        }
      }

      return resultLines;
    }

  }// END class
}// END namespace