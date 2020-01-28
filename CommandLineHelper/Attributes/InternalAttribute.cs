using System;

namespace CommandLineHelper
{

  /// <summary>
  /// The 'InternalAttribute' attribute is a marker attribute. 
  /// Every public property with a public get and set accessor of the 'ParameterBase' class 
  /// or it's derived classes is considered a parameter property.
  /// Those parameter properties are subjects of the parse and validation process.
  /// Properties which have the 'InternalAttribute' attribute attached will never 
  /// be processed during the parse or validation process.
  /// <example>
  ///   <code>
  ///     [Internal]
  ///     public int LoopCount
  ///     {
  ///       get;
  ///       set;
  ///     }
  ///   </code>
  /// </example>
  /// </summary>
  public class InternalAttribute : ParameterAttributeBase
  {
    /// <summary>
    /// Constructor of the 'InternalAttribute' class.
    /// </summary>
    public InternalAttribute() : base()
    {
    }
    
  }// END class
}// END namespace
