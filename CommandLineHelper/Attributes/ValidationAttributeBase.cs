using System;

namespace CommandLineHelper
{

  /// <summary>
  /// The base class of all validation attributes. In order to create
  /// your own validaton attribute do the following.
  /// Create a subclass which inherits from 'ValidationAttributeBase'.
  /// Overwrite the 'Validate' function to implement your own validation
  /// logic.
  /// <para>
  /// See: <see cref="Validate"/>
  /// </para>
  /// </summary>
  public abstract class ValidationAttributeBase : ParameterAttributeBase
  {
    /// <summary>
    /// The validation error message which will be used
    /// if the validation failed.
    /// </summary>
    /// <value></value>
    public string ValidationErrorMessage
    {
      get;
      protected set;
    }

    /// <summary>
    /// Constructor of the 'ValidationAttributeBase' class.
    /// </summary>
    /// <param name="validationErrorMessage">The error message to show when the validation fails</param>
    public ValidationAttributeBase(string validationErrorMessage) : base()
    {
      this.ValidationErrorMessage = validationErrorMessage;
    }

    /// <summary>
    /// This function must be implemented by every subclass. 
    /// <para>
    /// The function must set the 'propertyMetaInfo.ValidationError' property if the validation failed.
    /// </para>
    /// <para>
    /// If the validation succeeded, the 'propertyMetaInfo.ValidationError' should be set to 'null'.
    /// </para>
    /// <example> <para>An example string length validation attribute implementation:</para>
    /// <code>
    ///  public class MaxStringLengthAttribute : ValidationAttributeBase
    ///  {
    ///    public int MaxLength
    ///    {
    ///      get;
    ///      private set;
    ///    }
    ///  
    ///    public MaxStringLengthAttribute(int maxLength, string validationErrorMessage) : base(validationErrorMessage)
    ///    {
    ///       this.MaxLength = maxLength;
    ///       this.ValidationErrorMessage = validationErrorMessage;
    ///    }
    ///    
    ///    //
    ///    // Create an implementation of the abstact base function 'Validate'
    ///    //
    ///    public override void Validate(PropertyMetaInfo propertyMetaInfo, ParameterBase parameterObject)
    ///    {
    ///      MethodBase? methodBase = MethodBase.GetCurrentMethod();
    ///      //
    ///      // Make sure the 'ValidationAttribute' is assigned to the right 
    ///      // property type. (A string in this case)
    ///      //
    ///      if(propertyMetaInfo.Type.ToLower().IndexOf("string") &lt; 0)
    ///      {
    ///        propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString(), $"The attribute '{methodBase?.DeclaringType}' is not allowed on properties of type: '{propertyMetaInfo.Type}'.");
    ///        return; 
    ///      }
    ///  
    ///      //
    ///      // The actual validation. Set the 'ValidationError' if the validation fails.
    ///      //
    ///      if(propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString()?.Length &gt; this.MaxLength)
    ///      {
    ///        propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString(), $"{ValidationErrorMessage}");
    ///        return; 
    ///      }
    ///  
    ///      //
    ///      // The validation passed. Clear the 'ValidationError'.
    ///      //
    ///      propertyMetaInfo.ValidationError = null;
    ///    }
    ///  
    ///  }// END class
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="propertyMetaInfo"></param>
    /// <param name="parameterObject"></param>
    public abstract void Validate(PropertyMetaInfo propertyMetaInfo, ParameterBase parameterObject);

  }// END class
}// END namespace
