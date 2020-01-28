using System;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'DescriptionAttribute' attribute should be used to add a meaningful description
  /// to a parameter attribute and in consequence to the corresponding command
  /// line argument.
  /// The description should be shown on the command line after a help request.
  /// <example>
  ///   <code>
  ///     [Description("The 'Email' argument value must be a valid e-mail address.")]
  ///     public string Email
  ///     {
  ///       get;
  ///       set;
  ///     }
  ///   </code>
  /// </example>
  /// </summary>
  public class DescriptionAttribute : ParameterAttributeBase
  {

    /// <summary>
    /// The description string which will be displayed
    /// in the help screen.
    /// </summary>
    /// <value></value>
    public virtual string Description
    {
      get;
      set;
    }

    /// <summary>
    /// Constructor of the 'DescriptionAttribute' class.
    /// </summary>
    /// <param name="description"></param>
    public DescriptionAttribute(string description) : base()
    {
      this.Description = description;
    }

  }// END class
}// END namespace
