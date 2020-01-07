using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace CommandLineHelper
{
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

    public ValidationError(PropertyMetaInfo propertyMetaInfo, string value, string message)
    {
      this.PropertyMetaInfo = propertyMetaInfo;
      this.Value = value;
      this.Message = message;
    }
    
  }// END class
}// END namespace