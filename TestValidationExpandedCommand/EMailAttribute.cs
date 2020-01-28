using System;
using System.Reflection;
using CommandLineHelper;

namespace TestCommand
{
  /// <summary>
  /// The 'EmailAttribute' validates the value of the parameter property it is 
  /// attached to against the formal requirements of an email address.
  /// If the parameter property fails validation, an error message is created which 
  /// shows the message provided in the attribute constructor.
  /// <example>
  ///   <code>
  ///     [Email("The value of 'Email' must be a valid email address.")]
  ///     public string Email
  ///     {
  ///       get;
  ///       set;
  ///     }
  ///   </code>
  /// </example>
  /// </summary>
  public class EmailAttribute : ValidationAttributeBase
  {
    /// <summary>
    /// Constructor of the 'EmailAttribute' class.
    /// </summary>
    /// <param name="validationErrorMessage"></param>
    /// <returns></returns>
    public EmailAttribute(string validationErrorMessage) : base(validationErrorMessage)
    {
    }

    /// <summary>
    /// Implementation of the abstract base function 'Validate'.
    /// </summary>
    /// <param name="propertyMetaInfo"></param>
    /// <param name="parameterObject"></param>     
    public override void Validate(PropertyMetaInfo propertyMetaInfo, ParameterBase parameterObject)
    {
      MethodBase? methodBase = MethodBase.GetCurrentMethod();
      string? value;

      //
      // Make sure the 'ValidationAttribute' is assigned to the right 
      // property type. (A string in this case)
      //
#pragma warning disable CS8604
      if (propertyMetaInfo.Type.ToLower().IndexOf("string") < 0)
      {
        propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString(), $"The attribute '{methodBase?.DeclaringType}' is not allowed on properties of type: '{propertyMetaInfo.Type}'.");
        return;
      }

      //
      // The actual validation.
      //
      value = propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString();
      try
      {
        var mailAddress = new System.Net.Mail.MailAddress(value);
        if(mailAddress.Address.LastIndexOf('.') < mailAddress.Address.LastIndexOf('@') + 2)
        {
          throw new Exception();
        }
      }
      catch
      {
        propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString(), $"{ValidationErrorMessage}");
        return;
      }
  #pragma warning restore CS8604

      //
      // The validation passed.
      //
      propertyMetaInfo.ValidationError = null;
    }

  }// END class
}// END namespace