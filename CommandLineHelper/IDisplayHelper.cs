using System;
using System.Reflection;

namespace CommandLineHelper
{

  public interface IDisplayHelper
  {
    /// <summary>
    /// Create the help for the parameter object provided in argument 
    /// 'parameterObject'. The line length of the resulting help text
    /// will match with value provide in argument 'screenWidth'.
    /// The help is supposed to be rendered on the command line.
    /// </summary>
    /// <param name="parameterObject"></param>
    /// <param name="screenWidth"></param>
    /// <returns>The resulting string</returns>
    string CreateHelp(ParameterBase parameterObject, int screenWidth = 80);

    /// <summary>
    /// Creates a usage description for the command associated with the provided
    /// 'parameterObject'. The usage description is either the one provided with
    /// the 'UsageAttribute' or a generic description created from the known 
    /// parameter attributes.
    /// </summary>
    /// <param name="parameterObject"></param>
    /// <returns>The resulting string</returns>
    string CreateUsage(ParameterBase parameterObject);

    /// <summary>
    /// Creates a summary of the validation errors of the 'ParameterBase' object 
    /// provided in 'parameterObject'.
    /// <para>
    /// The message provided in argument 'message' will be shown on top of the summary.
    /// </para>
    /// <para>
    /// The whole will be returned as string with a line length matching with the value
    /// of argument screenWidth.
    /// </para>
    /// </summary>
    /// <param name="parameterObject"></param>
    /// <param name="message"></param>
    /// <param name="screenWidth"></param>
    /// <returns>The resulting string</returns>
    string CreateValidationSummary(ParameterBase parameterObject, string message = "One ore more of the command line arguments are invalid.",  int screenWidth = 80);

    /// <summary>
    /// Returns the version of the provided 'commandObject' as string.
    /// </summary>
    /// <param name="commandObject"></param>
    /// <returns>The version as string or the message: 'No version info available.'</returns>
    string CreateVersion(Object commandObject);

    /// <summary>
    /// Returns the version of the provided 'commandAssembly' as string.
    /// </summary>
    /// <param name="commandObject"></param>
    /// <returns>The version as string or the message: 'No version info available.'</returns>
    string CreateVersion(Assembly commandAssembly);

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
    /// <param name="backgroundColor"></param>
    /// <param name="foregroundColor"></param>
    void PrintToConsole(string output, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black );
  }// END interface
}// END namespace