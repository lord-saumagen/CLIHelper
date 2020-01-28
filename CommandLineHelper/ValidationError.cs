using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'ValidationError' class holds a reference to the 
  /// 'PropertyMetaInfo' which failed validation as well as
  /// the value which couldn't be validated and the error 
  /// message.
  /// </summary>
  public class ValidationError
  {
    /// <summary>
    /// Back reference to the 'PropertyMetaInfo' which was 
    /// provided in the constructor.
    /// </summary>
    /// <value></value>
    public PropertyMetaInfo PropertyMetaInfo
    {
      get;
      private set;
    }

    /// <summary>
    /// The value of the command line argument which was
    /// erroneous.
    /// </summary>
    /// <value></value>
    public string Value
    {
      get;
      private set;
    }

    /// <summary>
    /// The error message created during validation.
    /// </summary>
    /// <value></value>
    public string Message
    {
      get;
      private set;
    }

    /// <summary>
    /// Constructor of the 'ValidationError' class.
    /// </summary>
    /// <param name="propertyMetaInfo"></param>
    /// <param name="value"></param>
    /// <param name="message"></param>
    public ValidationError(PropertyMetaInfo propertyMetaInfo, string value, string message)
    {
      this.PropertyMetaInfo = propertyMetaInfo;
      this.Value = value;
      this.Message = message;
    }
    
  }// END class
}// END namespace