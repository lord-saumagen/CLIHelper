using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLineHelper
{
  /// <summary>
  /// <param>  
  /// The 'ValueSetAttribute' constraints the valid command line attribute
  /// values to the elements in the 'ValueSet'.
  /// </param>
  /// <param>
  /// Make sure the 'ValueSet' holds only objects which are assignable to
  /// the target parameter property.
  /// </param>
  /// <param>
  /// If you intent to combine the 'ValueSetAttribute' and the 'DefaultValueAttribute'
  /// make sure the default value is one of the elements of the 'ValueSet'.
  /// </param>
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
  public class ValueSetAttribute : ParameterAttributeBase
  {

    public virtual List<Object> ValueSet
    {
      get;
      set;
    }


    public ValueSetAttribute(List<Object> valueSet)
    {
       MethodBase? methodBase = MethodBase.GetCurrentMethod();

      if(valueSet.Count == 0)
      {
        throw new ArgumentException($"Argument '{nameof(valueSet)}' must not be empty in function '{methodBase?.ReflectedType?.Name}.ctor'.");
      }
      this.ValueSet = valueSet;
    }

    public ValueSetAttribute(Object[] valueSet)
    {
      MethodBase? methodBase = MethodBase.GetCurrentMethod();

      if(valueSet.Length == 0)
      {
        throw new ArgumentException($"Argument '{nameof(valueSet)}' must not be empty in function '{methodBase?.ReflectedType?.Name}.ctor'.");
      }
      this.ValueSet = valueSet.ToList();
    }

  }// END class
}// END namespace
