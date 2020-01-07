using System;
using System.Reflection;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'UsageAttribute' attribute provides the usage information 
  /// for a given command.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public class UsageAttribute : Attribute
  {

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
      
      if(String.IsNullOrWhiteSpace(usage))
      {
        throw new ArgumentException($"Argument '{nameof(usage)}' must not be null or empty in function '{methodBase?.ReflectedType?.Name}.ctor'.", $"{nameof(usage)}");
      }
      this.Usage = usage;
    }

  }// END class
}// END namespace
