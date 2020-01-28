using System;

namespace CommandLineHelper
{

  /// <summary>
  /// The 'MandatoryAttribute' attribute is a marker attribute. 
  /// Every parameter property which has a 'MandatoryAttribute' attribute 
  /// attached will result in a mandatory command line argument.
  /// <example>
  ///   <code>
  ///     [Mandatory]
  ///     public int LoopCount
  ///     {
  ///       get;
  ///       set;
  ///     }
  ///   </code>
  /// </example>
  /// A mandatory parameter property will result in a command line argument
  /// which must be provided on the command line. Missing mandatory command
  /// line arguments will result in an error message.
  /// </summary>
  public class MandatoryAttribute : ParameterAttributeBase
  {

    /// <summary>
    /// Constructor of the 'MandatoryAttribute' class.
    /// </summary>
    public MandatoryAttribute()
    {
    }

  }// END class
}// END namespace
