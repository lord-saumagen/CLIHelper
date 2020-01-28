using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'MinValueAttribute' attribute constraints the value of 
  /// the parameter property it is attached to, to a minimum numeric value.
  /// If the parameter property fails validation because the value undercuts
  /// the minimum value, an error message is created which holds the 
  /// provided message.
  /// <example>
  ///   <code>
  ///     [MinValue( 2, "The value of 'SeatsPerCar' must be a value greater or equal 2.")]
  ///     public int SeatsPerCar
  ///     {
  ///       get;
  ///       set;
  ///     }
  ///   </code>
  /// </example>
  /// </summary>
  public class MinValueAttribute : ValidationAttributeBase
  {
    /// <summary>
    /// The value constraint for the number.
    /// </summary>
    /// <value></value>
    public object MinValue
    {
      get;
      private set;
    }

    /// <summary>
    /// Constructor of the 'MinValueAttribute' class.
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="validationErrorMessage"></param>
    /// <returns></returns>
    public MinValueAttribute(object minValue, string validationErrorMessage) : base(validationErrorMessage)
    {
      this.MinValue = minValue;
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
            if(Convert.ToInt16(this.MinValue) <= ((Int16) propertyValue))
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
            if(((Int32) this.MinValue) <= ((Int32) propertyValue))
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
            if(Convert.ToInt64(this.MinValue) <= ((Int64) propertyValue))
            {
              propertyMetaInfo.ValidationError = null;
              return;
            }
            break;
          }
          case "Single|Null":
          case "Single":
          {
            if(Convert.ToSingle(this.MinValue) <= ((Single) propertyValue))
            {
              propertyMetaInfo.ValidationError = null;
              return;
            }
            break;
          }
          case "Double|Null":
          case "Double":
          {
            if(((Double) this.MinValue) <= ((Double) propertyValue))
            {
              propertyMetaInfo.ValidationError = null;
              return;
            }
            break;
          }
          case "Decimal|Null":
          case "Decimal":
          {
            if(Convert.ToDecimal(this.MinValue) <= ((Decimal) propertyValue))
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