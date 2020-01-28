using System;
using CommandLineHelper;
using System.Reflection;
using System.Linq;

namespace TestCommand
{
  class TestScreenCreationOverrideCommand
  {
    static int Main(string[] args)
    {
      bool IsValid;
      bool showUsageOnEmptyArgs;

      showUsageOnEmptyArgs = args.Where(item => item.IndexOf("showUsageOnEmptyArgs") > -1).Count() > 0;
      ParameterObjectScreenOverride parameterObjectScreenOverride = new ParameterObjectScreenOverride("TestScreenCreationOverrideCommand", Assembly.GetExecutingAssembly());
      IsValid = parameterObjectScreenOverride.Process(args, showUsageOnEmptyArgs);
      return ((IsValid) ? 0 : 1);
    }
  }// END class
}// END namespace
