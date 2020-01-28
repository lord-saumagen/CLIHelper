using System;
using System.Reflection;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'UsageAttribute' attribute provides the usage information 
  /// for a given command. It should be assigned to the parameter property
  /// class. 
  /// <example>
  ///   <code>
  ///  [Usage(@"&lt;CommandName&gt; [number=&lt;Int32 number&gt;] name=&lt;String&gt;
  ///  The 'number' value must be in the range [0..100]. The default value is 0.
  ///  The 'name' must not be an empty string and the length must be less or equal 50.")]
  ///  class ParameterExtended : ParameterBase
  ///  {
  ///  }  
  ///   </code>
  /// </example>
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public class UsageAttribute : Attribute
  {

    /// <summary>
    /// The usage string which will be used in the 
    /// usage screen of the attached parameter object.
    /// </summary>
    /// <value></value>
    public string Usage
    {
      get;
      set;
    }


    /// <summary>
    /// Constructor of the 'UsageAttribute' class.
    /// </summary>
    /// <param name="usage"></param>
    /// <exception cref="System.ArgumentException"></exception>
    public UsageAttribute(string usage)
    {
      MethodBase? methodBase = MethodBase.GetCurrentMethod();

      if (String.IsNullOrWhiteSpace(usage))
      {
        throw new ArgumentException($"Argument '{nameof(usage)}' must not be null or empty in function '{methodBase?.ReflectedType?.Name}.ctor'.", $"{nameof(usage)}");
      }
      this.Usage = usage;
    }

  }// END class
}// END namespace
