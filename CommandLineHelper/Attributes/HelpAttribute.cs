using System;
using System.Reflection;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'HelpAttribute' attribute provides information which should be 
  /// shown on a help request.
  /// <example>
  ///   <code>
  ///  [Help(@"Write any information which will help a user to understand your
  ///  command line application. It can be anything from 'intended use',
  ///  'examples', 'common misconceptions' or any other helpful information
  ///  you are able to provide.")]
  ///  class ParameterExtended : ParameterBase
  ///  {
  ///  }  
  ///   </code>
  /// </example>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public class HelpAttribute : Attribute
  {

    /// <summary>
    /// The help string which will be used in the 
    /// help screen of the attached parameter object.
    /// </summary>
    /// <value></value>
    public string Help
    {
      get;
      set;
    }


    /// <summary>
    /// Constructor of the 'HelpAttribute' class.
    /// </summary>
    /// <param name="help"></param>
    /// <exception cref="System.ArgumentException"></exception>
    public HelpAttribute(string help)
    {
      MethodBase? methodBase = MethodBase.GetCurrentMethod();

      if (String.IsNullOrWhiteSpace(help))
      {
        throw new ArgumentException($"Argument '{nameof(help)}' must not be null or empty in function '{methodBase?.ReflectedType?.Name}.ctor'.", $"{nameof(help)}");
      }
      this.Help = help;
    }

  }// END class
}// END namespace
