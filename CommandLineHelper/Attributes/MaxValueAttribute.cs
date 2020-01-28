using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'MaxValueAttribute' attribute constraints the value of 
  /// the parameter property it is attached to, to a maximum numeric value.
  /// If the parameter property fails validation because the value exceeds
  /// the maximum value, an error message is created which holds the 
  /// provided message.
  /// <example>
  ///   <code>
  ///     [MaxValue(10, "The value of 'SeatsPerCar' must be a value less or equal 10.")]
  ///     public int SeatsPerCar
  ///     {
  ///       get;
  ///       set;
  ///     }
  ///   </code>
  /// </example>
  /// </summary>
  public class MaxValueAttribute : ValidationAttributeBase
  {
    /// <summary>
    /// The value constraint for the number.
    /// </summary>
    /// <value></value>
    public object MaxValue
    {
      get;
      private set;
    }

    /// <summary>
    /// Constructor of the 'MaxValueAttribute' class.
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="validationErrorMessage"></param>
    /// <returns></returns>
    public MaxValueAttribute(object minValue, string validationErrorMessage) : base(validationErrorMessage)
    {
      this.MaxValue = minValue;
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

#pragma warning disable CS8600, CS8602, CS8604

      var propertyValue = propertyMetaInfo.PropertyInfo.GetValue(parameterObject);
      if(propertyValue == null)
      {
        propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString(), $"{ValidationErrorMessage}");
        return;
      }
      else
      {
      
        switch(propertyMetaInfo.Type)
        {
          case "UInt16|Null":
          case "UInt16":
          case "Int16|Null":
          case "Int16":
          {      
            if(Convert.ToInt16(this.MaxValue) >= ((Int16) propertyValue))
            {
              propertyMetaInfo.ValidationError = null;
              return;
            }
            break;
          }
          case "UInt32|Null":
          case "UInt32":
          case "Int32|Null":
          case "Int32":
          {
            if(((Int32) this.MaxValue) >= ((Int32) propertyValue))
            {
              propertyMetaInfo.ValidationError = null;
              return;
            }
            break;
          }
          case "UInt64|Null":
          case "UInt64":
          case "Int64|Null":
          case "Int64":
          {
            if(Convert.ToInt64(this.MaxValue) >= ((Int64) propertyValue))
            {
              propertyMetaInfo.ValidationError = null;
              return;
            }
            break;
          }
          case "Single|Null":
          case "Single":
          {
            if(Convert.ToSingle(this.MaxValue) >= ((Single) propertyValue))
            {
              propertyMetaInfo.ValidationError = null;
              return;
            }
            break;
          }
          case "Double|Null":
          case "Double":
          {
            if(((Double) this.MaxValue) >= ((Double) propertyValue))
            {
              propertyMetaInfo.ValidationError = null;
              return;
            }
            break;
          }
          case "Decimal|Null":
          case "Decimal":
          {
            if(Convert.ToDecimal(this.MaxValue) >= ((Decimal) propertyValue))
            {
              propertyMetaInfo.ValidationError = null;
              return;
            }
            break;
          }
          default:
          {
            //
            // The 'ValidationAttribute' is assigned to the wrong
            // property type. (Must be one of the number types above)
            //

            propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString(), $"The attribute '{methodBase?.DeclaringType}' is not allowed on properties of type: '{propertyMetaInfo.Type}'.");
            return;
          }
        }
        propertyMetaInfo.ValidationError =  new ValidationError(propertyMetaInfo, propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString(), $"{ValidationErrorMessage}");
      }

#pragma warning restore CS8600, CS8602, CS8604
    }

  }// END class
}// END namespace