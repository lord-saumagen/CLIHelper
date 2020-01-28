using System;
using System.Reflection;
using CommandLineHelper;
using System.Linq;
using System.Collections.Generic;

namespace TestCommand
{
  [Usage("TestScreenCreationOverrideCommand [help] [version] [name=<string>] items-total=<UInt32>")]
  [Help("The TestScreenCreationOverrideCommand is a command which was created to test the screen override capabilities of the 'CommanLineHelper' library. The command itself does noting except writing to the output stream.")]
  public class ParameterObjectScreenOverride : ParameterBase
  {

    [Mandatory]
    [MinStringLength(5, "The value of the name argument must be a string with a minimal length of 5 characters.")]
    [Name("name")]
    public String RecipientName { get; set; }

    [Name("items-total")]
    [DefaultValue(1)]
    public UInt32 NumberOfItems {get; set;}

    public ParameterObjectScreenOverride(string commandName, Assembly commandAssembly): base(commandName, commandAssembly)
    {
    }

    public override string CreateHelp(int screenWidth = 80) 
    {
      HelpAttribute? helpAttribute = (HelpAttribute?) this.GetType().GetCustomAttribute(typeof(HelpAttribute));

      //
      // If a help text is provided show that text.
      //
      if ((helpAttribute != null) && !String.IsNullOrWhiteSpace(helpAttribute.Help))
      {
        return "Custom help screen\r\n\r\n" + helpAttribute.Help;
      }
      else
      {
        return "Custom help screen\r\n\r\n" + "No help available";
      }

    }
    public override string CreateUsage()
    {
      UsageAttribute? usageAttribute = (UsageAttribute?) this.GetType().GetCustomAttribute(typeof(UsageAttribute));

      //
      // If a usage text is provided show that text.
      //
      if ((usageAttribute != null) && !String.IsNullOrWhiteSpace(usageAttribute.Usage))
      {
        return "Custom usage screen\r\n\r\n" + usageAttribute.Usage;
      }
      else
      {
        return "Custom usage screen\r\n\r\n" + "No usage available";
      }
    }

    public override string CreateValidationSummary(int screenWidth = 80)
    {
      return CreateValidationSummary("",screenWidth);
    }

    public override string CreateValidationSummary(string message, int screenWidth = 80)
    {
      string result;
      List<String> lineList;

      result = string.Empty;

      result += "Custom validation screen\r\n\r\n";

      if(!String.IsNullOrWhiteSpace(message))
      {
        result += message + "\r\n\r\n";
      }

      foreach(var metaInfo in this.PropertyMetaInfoList)
      {
        result += metaInfo.Name + " : " + (metaInfo.IsValid ? "is valid" : "is invalid") + "\r\n";
      }

      lineList = CommandLineHelper.DisplayHelper.CreateWrappedLines(result, screenWidth);
      result = String.Join("\r\n", lineList);
      return result;
    }

    public override string CreateVersion(Object commandObject)
    {
      Assembly? commandObjectAssembly;

      if (commandObject == null)
      {
        return "Custom version screen\r\n\r\n" + "No version info available.";
      }

      commandObjectAssembly = Assembly.GetAssembly(commandObject.GetType());
#pragma warning disable CS8604      
      return "Custom version screen\r\n\r\n" + CreateVersion(commandObjectAssembly);
#pragma warning restore CS8604      
    }

    public override string CreateVersion(Assembly commandAssembly)
    {
      string? version;

      if (commandAssembly == null)
      {
        return "Custom version screen\r\n\r\n" + "";
      }

      version = "Custom version screen\r\n\r\n" + commandAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
      if (version == null)
      {
        return "Custom version screen\r\n\r\n" + "No version info available.";
      }
      return version;
    }

  }// END class
}// END namespace



