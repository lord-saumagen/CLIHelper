using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLineHelper
{
  /// <summary>
  /// <param>  
  /// The 'ValueSetAttribute' attribute constraints the input of the
  /// parameter property it is attached to. Only the values which are
  /// provided in the 'ValueSetAttribute' constructor will be considered
  /// valid parameter property values during validation.
  /// </param>
  /// <param>
  /// Make sure the 'ValueSet' holds only objects which are assignable to
  /// the target parameter property.
  /// </param>
  /// <param>
  /// If you intent to combine the 'ValueSetAttribute' and the 'DefaultValueAttribute'
  /// make sure the default value is one of the elements of the 'ValueSet'.
  /// </param>
  /// <example>
  ///   <code>
  ///     [ValueSet(new object[] {"Left", "Right", "Top", "Down"})]
  ///     public string NavigateDirection
  ///     {
  ///       get;
  ///       set;
  ///     }
  /// </code>
  /// </example>
  /// </summary>
  public class ValueSetAttribute : ParameterAttributeBase
  {

    /// <summary>
    /// The list of elements which form the ValueSet'. The type of
    /// the elements must match with the type of the attached
    /// parameter property.
    /// </summary>
    /// <value></value>
    public virtual List<Object> ValueSet
    {
      get;
      set;
    }

    /// <summary>
    /// Constructor of the 'ValueSetAttribute' class.
    /// </summary>
    /// <param name="valueSet">A list of objects which form the value set.</param>
    public ValueSetAttribute(List<Object> valueSet)
    {
       MethodBase? methodBase = MethodBase.GetCurrentMethod();

      if(valueSet.Count == 0)
      {
        throw new ArgumentException($"Argument '{nameof(valueSet)}' must not be empty in function '{methodBase?.ReflectedType?.Name}.ctor'.");
      }
      this.ValueSet = valueSet;
    }

    /// <summary>
    /// Constructor of the 'ValueSetAttribute' class.
    /// </summary>
    /// <param name="valueSet">An array of objects which form the value set.</param>
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
