using System;
using System.Collections.Generic;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'DefaultValueAttribute' attribute allows to set default values for the
  /// parameter properties the attribute is attached to.
  /// <example>
  ///   <code>
  ///     [DefaultValue((Object) 42)]
  ///     public int NumberOfGuests
  ///     {
  ///       get;
  ///       set;
  ///     }
  ///   </code>
  /// </example>
  /// The value of the command line argument 'DefaultValue' will be 42 even if
  /// the command line argument wasn't provided.
  /// The value in the 'DefaultValueAttribute' must be cast to an object and the
  /// base type must match with the type of the property the attribute is attached to.
  /// </summary>
  public class DefaultValueAttribute : ParameterAttributeBase
  {

    /// <summary>
    /// The default value which will be use
    /// if there is no other value provided
    /// during parse. The 'Value' type must
    /// match with the type of the attached
    /// property.
    /// </summary>
    /// <value></value>
    public virtual Object Value
    {
      get;
      set;
    }

    /// <summary>
    /// Constructor of the 'DefaultValueAttribute' class.
    /// </summary>
    /// <param name="value"></param>
    public DefaultValueAttribute(Object value)
    {
      this.Value = value;
    }

  }// END class
}// END namespace
