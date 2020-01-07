using System;

namespace CommandLineHelper
{
  /// <summary>
  /// The 'DescriptionAttribute' should be used to add a meaningful description
  /// to a parameter attribute and in consequence to the corresponding command
  /// line argument.
  /// The description should be shown on the command line after a help request.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
  public class DescriptionAttribute : ParameterAttributeBase
  {

    public virtual string Description
    {
      get;
      set;
    }


    public DescriptionAttribute(string description)
    {
      this.Description = description;
    }

  }// END class
}// END namespace
