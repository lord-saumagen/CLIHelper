using System;

namespace CommandLineHelper
{

  /// <summary>
  /// The 'NameAttribute' attribute allows to change the name of a parameter attribute
  /// for it's corresponding command line argument.
  /// This way it's possible to use easy to memorize argument names on the command line
  /// which otherwis would collide with your style guide naming rules.
  /// <example>
  ///   <code>
  ///     [Name("email")]
  ///     public string RecipientEmailAddress
  ///     {
  ///       get;
  ///       set;
  ///     }
  /// </code>
  /// The command line argument would be <c>email</c> and corresponding property 
  /// name would be <c>RecipientEmailAddress</c>.
  /// </example>
  /// </summary>
  public class NameAttribute : ParameterAttributeBase
  {
    /// <summary>
    /// The name as use on the command line.
    /// </summary>
    /// <value></value>
    public virtual string Name
    {
      get;
      set;
    }


    /// <summary>
    /// Constructor of the 'NameAttribute' class
    /// </summary>
    /// <param name="name"></param>
    public NameAttribute(string name) : base()
    {
      this.Name = name;
    }

  }// END class
}// END namespace
