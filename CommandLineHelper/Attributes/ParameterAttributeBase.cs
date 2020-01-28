using System;

namespace CommandLineHelper
{

  /// <summary>
  /// The base class of all parameter attribute classes. 
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
  public class ParameterAttributeBase : Attribute
  {

    /// <summary>
    /// Constructor of the 'ParameterAttributeBase' class.
    /// </summary>
    public ParameterAttributeBase()
    {
    }

  }// END class
}// END namespace
