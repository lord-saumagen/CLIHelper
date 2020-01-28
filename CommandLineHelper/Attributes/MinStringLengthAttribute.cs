using System;
using System.Reflection;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'MinStringLengthAttribute' attribute constraints the input length
  /// of the parameter property it is attached to.
  /// If the parameter property fails validation because the string
  /// length undercut the minimum length, an error message is created
  /// which holds the provided message.
  /// <example>
  ///   <code>
  ///     [MinStringLength(5, "The value of 'FavoriteTVShow' must at least have a length of 5 characters.")]
  ///     public string FavoriteTVShow
  ///     {
  ///       get;
  ///       set;
  ///     }
  ///   </code>
  /// </example>
  /// </summary>
  public class MinStringLengthAttribute : ValidationAttributeBase
  {
    /// <summary>
    /// The length constraint for the string.
    /// </summary>
    /// <value></value>
    public int MinLength
    {
      get;
      private set;
    }


    /// <summary>
    /// Constructor of the 'MinStringLengthAttribute' class.
    /// </summary>
    /// <param name="minLength"></param>
    /// <param name="validationErrorMessage"></param>
    /// <returns></returns>
    public MinStringLengthAttribute(int minLength, string validationErrorMessage) : base(validationErrorMessage)
    {
      this.MinLength = minLength;
      this.ValidationErrorMessage = validationErrorMessage;
    }

    /// <summary>
    /// Implementation of the abstract base function 'Validate'.
    /// </summary>
    /// <param name="propertyMetaInfo"></param>
    /// <param name="parameterObject"></param>     
    public override void Validate(PropertyMetaInfo propertyMetaInfo, ParameterBase parameterObject)
    {
      MethodBase? methodBase = MethodBase.GetCurrentMethod();
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
      if (propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString()?.Length < this.MinLength)
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