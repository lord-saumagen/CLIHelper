using System;

namespace CommandLineHelper
{

  /// <summary>
  /// The 'InternalAttribute' is a marker attribute. 
  /// Every public property with a pubic get and set accessor of the 'ParameterBase' class 
  /// or it's derived class is considered a parameter property.
  /// Those parameter properties are subjects of the parse and validation process.
  /// Properties which have the 'InternalAttribute' attribute attached will never 
  /// be processed during the pares or validation process.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
  public class InternalAttribute : ParameterAttributeBase
  {
    public InternalAttribute()
    {
    }
    
  }// END class
}// END namespace
