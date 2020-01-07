using System;
using System.Collections.Generic;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'DefaultValueAttribute' attribute allows to set default values for the
  /// parameter properties the attribute is attached to.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
  public class DefaultValueAttribute : ParameterAttributeBase
  {

    public virtual Object Value
    {
      get;
      set;
    }


    public DefaultValueAttribute(Object value)
    {
      this.Value = value;
    }

  }// END class
}// END namespace
