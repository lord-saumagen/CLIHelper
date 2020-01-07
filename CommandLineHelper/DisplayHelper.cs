using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;

namespace CommandLineHelper
{

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
    /// Create the help for the parameter object provided in argument 
    /// 'parameterObject'. The line length of the resulting help text
    /// will match with value provide in argument 'screenWidth'.
    /// The help is supposed to be rendered on the command line.
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

      result = string.Empty;
      longestArgumentLength = 0;

      if (parameterObject == null)
      {
        return result;
      }

#if !DEBUG
      if (Console.WindowWidth <= screenWidth)
      {
        Console.WindowWidth = screenWidth + 4;
      }
#endif

      if (parameterObject.PropertyMetaInfoList.Count == 0)
      {
        result = "The current parameter object has no parameter property!";
      }
      else
      {

        longestArgumentLength = parameterObject.PropertyMetaInfoList.Max(item => item.Name.Length);
        longestTypeLength = parameterObject.PropertyMetaInfoList.Max(item => item.Type.Length);
        longestDefaultLength = parameterObject.PropertyMetaInfoList.Max(item => (item.DefaultValue?.ToString()?.Length ?? 0));

        longestArgumentLength = (longestArgumentLength > "[Parameter]".Length) ? longestArgumentLength : "[Parameter]".Length;
        longestTypeLength = (longestTypeLength > "[Type]".Length) ? longestTypeLength : "[Type]".Length;
        longestDefaultLength = (longestDefaultLength > "[Default]".Length) ? longestDefaultLength : "[Default]".Length;


        result += CreateKeyDescription(screenWidth);

        result += CreateHelpTop(screenWidth, longestArgumentLength, longestTypeLength, longestDefaultLength);

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
    /// 'parameterObject'. The usage description is either the one provided with
    /// the 'UsageAttribute' or a generic description created from the known 
    /// parameter attributes.
    /// </summary>
    /// <param name="parameterObject"></param>
    /// <returns>The resulting string</returns>
    public virtual string CreateUsage(ParameterBase parameterObject)
    {
      string result;
      result = String.Empty;

      result += "Usage:\r\n\r\n";
      UsageAttribute? usageAttribute = (UsageAttribute?)parameterObject.GetType().GetCustomAttribute(typeof(UsageAttribute));

      //
      // If a usage description is provided in the command attributes use return this description.
      //
      if ((usageAttribute != null) && !String.IsNullOrWhiteSpace(usageAttribute.Usage))
      {
        result += usageAttribute.Usage;
      }
      //
      // Create a usage description from the known parameter attributes.
      //
      else
      {
        result += parameterObject.CommandName + " ";
        result += "[" + parameterObject.HelpIndicatorList.First() + "] ";
        result += "[" + parameterObject.VersionIndicatorList.First() + "] ";
        foreach (var metaInfo in parameterObject.PropertyMetaInfoList)
        {
          if (metaInfo.Mandatory)
          {
            result += metaInfo.Name + "=" + "<" + metaInfo.Type + "> ";
          }
          else
          {
            result += "[" + metaInfo.Name + "=" + "<" + metaInfo.Type + ">" + "] ";
          }
        }
      }
      return result + "\r\n";
    }


    /// <summary>
    /// Creates a summary of the validation errors of the 'ParameterBase' object 
    /// provided in 'parameterObject'.
    /// <para>
    /// The message provided in argument 'message' will be shown on top of the summary.
    /// </para>
    /// <para>
    /// The summary will be returned as a string with a line length matching with the 
    /// value of argument screenWidth.
    /// </para>
    /// </summary>
    /// <param name="parameterObject"></param>
    /// <param name="message"></param>
    /// <param name="screenWidth"></param>
    /// <returns>The resulting string</returns>
    public virtual String CreateValidationSummary(ParameterBase parameterObject, string message = "One ore more of the command line arguments are invalid.", int screenWidth = 80)
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
    /// The command object should be the command line program who's version should be displayed.
    /// </para>
    /// </summary>
    /// <param name="commandObject"></param>
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

    /// <summary>
    /// Prints the string provided in argument 'output' to the console using
    /// the provided background and foreground colors.
    /// After printing the original console colors get restored.
    /// <para>
    ///   The default colors are:
    /// </para>
    /// <para>
    ///   backgroundColor = ConsoleColor.Black
    /// </para>
    /// <para>
    ///   foregroundColor = ConsoleColor.White
    /// </para>
    /// </summary>
    /// <param name="output"></param>
    /// <param name="foregroundColor"></param>
    /// <param name="backgroundColor"></param>
    public virtual void PrintToConsole(string output, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
      ConsoleColor savedBackgroundColor;
      ConsoleColor savedForegroundColor;

      if (String.IsNullOrWhiteSpace(output))
      {
        return;
      }

      savedBackgroundColor = Console.BackgroundColor;
      savedForegroundColor = Console.ForegroundColor;

      Console.BackgroundColor = backgroundColor;
      Console.ForegroundColor = foregroundColor;

      Console.Write(output);

      Console.BackgroundColor = savedBackgroundColor;
      Console.ForegroundColor = savedForegroundColor;
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
          if (propertyMetaInfo.Mandatory)
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

          leadLine += defaultValueStr + new String(' ', longestDefaultLength - (defaultValueStr.Length)) + " ";
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


    private static string CreateKeyDescription(int screenWidth)
    {
      string result;

      //╔═╦═════════════════════════════════════════════════════════════════════════════════╗
      //║X║_Key description................................................................_║
      //╚═╩═════════════════════════════════════════════════════════════════════════════════╝

      result = "" + CornerTopLeft + HorizontalLine + TJunctionUp + new string(HorizontalLine, screenWidth - 4) + CornerTopRight + "\r\n";
      result += "" + VerticalLine + "+" + VerticalLine + (" Argument is mandatory." + new string(' ', screenWidth)).Substring(0, screenWidth - 4) + VerticalLine + "\r\n";
      result += "" + VerticalLine + "-" + VerticalLine + (" Argument is optional." + new string(' ', screenWidth)).Substring(0, screenWidth - 4) + VerticalLine + "\r\n";
      result += "" + CornerBottomLeft + HorizontalLine + TJunctionDown + new string(HorizontalLine, screenWidth - 4) + CornerBottomRight + "\r\n"; ;
      result += "\r\n";

      return result;
    }


    private static List<string> CreateWrappedLines(string line, int maxLineLength, int leadingSpaces = 1)
    {
      List<string> sourceLines;
      List<string> resultLines;

      sourceLines = new List<string>();
      resultLines = new List<string>();

      line = line.Trim();

      sourceLines = BreakLineAtLineBreak(line);

      //
      // Check that each source line fits into the available
      // descriptions space. Add line breaks if necessary.
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


    //
    // Break the line on all line breaks which are already 
    // part of the line.
    //
    private static List<String> BreakLineAtLineBreak(string line, bool trimLines = true)
    {
      List<string> resultLines;

      resultLines = new List<string>();

      if (line.IndexOf("\r\n") > -1)
      {
        line = line.Replace("\r\n", "\r");
      }

      if ((line.IndexOf("\r") > -1) || (line.IndexOf("\n") > -1))
      {
        resultLines.AddRange(line.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));
      }
      else
      {
        resultLines.Add(line);
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
    // Break the line at the last line break indicator at a position lower
    // than 'maxLength' if possible. Otherwise breaks the line at 'maxLength'
    // even if it breaks a word.
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