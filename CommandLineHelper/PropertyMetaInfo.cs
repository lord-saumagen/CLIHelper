using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


namespace CommandLineHelper
{

  /// <summary>
  /// The 'PropertyMetaInfo' class is a facade class which combines the 
  /// parameter property 'PropertyInfo' with an interface to access the
  /// parse and validation results as well as the attached custom attributes
  /// of a parameter property.
  /// </summary>
  public class PropertyMetaInfo
  {
    /// <summary>
    /// Returns the 'PropertyInfo' which was provided as 'propertyInfo'
    /// parameter in the constructor.
    /// </summary>
    /// <value>PropertyInfo</value>
    public PropertyInfo PropertyInfo
    {
      get;
      private set;
    }

    /// <summary>
    /// Resturns the value of the 'NameAttribute.Name' value of the 'NameAttribute'
    /// attached to the parameter property.
    /// If there isn't a 'NameAttribute' attached to the parameter property,
    /// or if the 'NameAttribute.Name' property is an empty or whitespace string
    /// the parameter property name will be returned.
    /// </summary>
    /// <value>NameAttribute.Name | property.Name</value>
    public string Name
    {
      get
      {
        //
        // Try to get the 'NameAttribute' name.
        //
        if (this.PropertyInfo.GetCustomAttribute(typeof(NameAttribute)) != null)
        {
#pragma warning disable CS8602
          var result = (this.PropertyInfo.GetCustomAttribute(typeof(NameAttribute)) as NameAttribute).Name;
          if (!String.IsNullOrWhiteSpace(result))
          {
            return result;
          }
#pragma warning restore CS8602
        }

        //
        // Return the name of the property.
        //
        return this.PropertyInfo.Name;
      }
    }

    /// <summary>
    /// The type of the parameter property as string.
    /// </summary>
    /// <value></value>
    public string Type
    {
      get
      {
        if(Nullable.GetUnderlyingType(this.PropertyInfo.PropertyType) != null)
        {
          return Nullable.GetUnderlyingType(this.PropertyInfo.PropertyType)?.Name + "|" + "Null";
        }
        return this.PropertyInfo.PropertyType.Name;
      }
    }

    /// <summary>
    /// Returns true is the 'InternalAttribute' is attached to
    /// the parameter property. Otherwise false.
    /// </summary>
    /// <value></value>
    public bool IsInternal
    {
      get
      {
        return (this.PropertyInfo.GetCustomAttribute(typeof(InternalAttribute)) != null);
      }
    }

    /// <summary>
    /// Returns true if the 'MandatoryAttribute' is attached to
    /// the parameter property. Otherwise false.
    /// </summary>
    /// <value></value>
    public bool IsMandatory
    {
      get
      {
        return (this.PropertyInfo.GetCustomAttribute(typeof(MandatoryAttribute)) != null);
      }
    }


    /// <summary>
    /// Returns the 'ValueSet' of the attached 'ValueSetAttribute'. That list might
    /// be empty if no such attribute is attached.
    /// </summary>
    /// <value>List&lt;Object&gt;</value>
    public List<Object> ValueSet
    {
      get
      {
        return (this.PropertyInfo.GetCustomAttribute(typeof(ValueSetAttribute)) as ValueSetAttribute)?.ValueSet ?? new List<Object>();
      }
    }


    /// <summary>
    /// Returns the validation attributes which are attached to the property info.
    /// That list might be empty.
    /// </summary>
    /// <value>List&lt;ValidationAttributeBase&gt;</value>
    public IEnumerable<ValidationAttributeBase?> ValidationAttributeList
    {
      get
      {
        return this.PropertyInfo.GetCustomAttributes(typeof(ValidationAttributeBase)).Select(attr => attr as ValidationAttributeBase).ToList();
      }
    }


    /// <summary>
    /// Returns the 'DefaultValueAttribute.Value' of the assigned 'DefaultValueAttribute'.
    /// If no 'DefaultValueAttribute' has been assigned to the parameter property the returned
    /// value is:
    /// <para>
    /// null : for a nullable parameter property or reference parameter property.
    /// </para>
    /// <para>
    /// The Type default value: for a none nullable value type parameter property.
    /// </para>
    /// </summary>
    /// <value></value>
    public object? DefaultValue
    {
      get
      {
        //
        // The property has no 'DefaultValueAttribute' attribute assigned.
        //
        if (this.PropertyInfo.GetCustomAttribute(typeof(DefaultValueAttribute)) == null)
        {
          //
          // The property is not nullable.
          //
          if (Nullable.GetUnderlyingType(this.PropertyInfo.PropertyType) == null)
          {
            //
            // The property type is a value type. In that case the default
            // value will be the default value of the property type.
            //
            if (this.PropertyInfo.PropertyType.IsValueType)
            {
              return Activator.CreateInstance(this.PropertyInfo.PropertyType);
            }
          }
        }
        //
        // The property has a 'DefaultValueAttribute' attribute assigned.
        //
        else
        {
          return (this.PropertyInfo.GetCustomAttribute(typeof(DefaultValueAttribute)) as DefaultValueAttribute)?.Value;
        }
        //
        // All other types should be objects. Return a null in that case.
        //
        return null;
      }
    }


    /// <summary>
    /// Returns the 'DescriptionAttribute.Description' value of the applied
    /// 'DescriptionAttribute'. Returns null if there hasn't been a 'DescriptionAttribute' 
    /// applied to the parameter property. 
    /// </summary>
    /// <value></value>
    public string? Description
    {
      get
      {
        return ((this.PropertyInfo.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description ?? null);
      }
    }


    /// <summary>
    /// Holds the result of the last parse process.
    /// </summary>
    /// <value></value>
    public ParseResultEnum ParseResult
    {
      get;
      set;
    } = ParseResultEnum.NOT_PARSED;

    /// <summary>
    /// Returns true if the validation was successfull.
    /// </summary>
    /// <value></value>
    public bool IsValid
    {
      get
      {
        return this.ValidationError == null;
      }
    }

    /// <summary>
    /// Is either a validation error which has been created
    /// during the validaton process or null if the validation
    /// didn't happen yet or if the validation succeeded without
    /// an error.
    /// </summary>
    /// <value></value>
    public ValidationError? ValidationError
    {
      get;
      set;
    }

    /// <summary>
    /// Constructor of the 'PropertyMetaInfo' class
    /// </summary>
    /// <param name="propertyInfo">The property info of the corresponding parameter property.</param>
    /// <exception cref="System.ArgumentException"></exception>
    public PropertyMetaInfo(PropertyInfo propertyInfo)
    {
      MethodBase? methodBase = MethodBase.GetCurrentMethod();

      if (propertyInfo == null)
      {
        throw new ArgumentException($"Argument '{nameof(propertyInfo)}' must not be null in function '{methodBase?.ReflectedType?.Name}.ctor'.", $"{propertyInfo}");
      }
      this.PropertyInfo = propertyInfo;
      this.ValidationError = null;
    }

  }// END class
}// END namespace