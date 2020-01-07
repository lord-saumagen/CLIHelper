using System;

namespace CommandLineHelper
{

  /// <summary>
  /// The 'MandatoryAttribute' is a marker attribute. 
  /// Every parameter property which as a 'MandatoryAttribute' attribute 
  /// attached will result in a mandatory command line argument.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
  public class MandatoryAttribute : ParameterAttributeBase
  {

    public MandatoryAttribute()
    {
    }

  }// END class
}// END namespace
