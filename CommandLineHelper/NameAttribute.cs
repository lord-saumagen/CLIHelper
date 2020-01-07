using System;

namespace CommandLineHelper
{

  /// <summary>
  /// The 'NameAttribute' attribute allows to change the name of a parameter attribute
  /// for it's corresponding command line argument.
  /// This way it's possible to use easy to memorize argument names on the command line
  /// which otherwis would collide with your style guide naming rules.
  /// <example>For example:
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
  [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
  public class NameAttribute : ParameterAttributeBase
  {
    public virtual string Name
    {
      get;
      set;
    }


    public NameAttribute(string name)
    {
      this.Name = name;
    }

  }// END class
}// END namespace
