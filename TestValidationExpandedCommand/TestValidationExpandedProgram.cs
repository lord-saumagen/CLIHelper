using System;
using CommandLineHelper;
using System.Reflection;

namespace TestCommand
{
  class TestValidationExpandedCommand
  {
    static int Main(string[] args)
    {
      bool IsValid;
      ParameterObjectValidationExpanded parameterObjectParseExpanded = new ParameterObjectValidationExpanded("TestValidationExpandCommand", Assembly.GetExecutingAssembly());
      IsValid = parameterObjectParseExpanded.Process(args, showUsageOnEmptyArgs : false);
      return ((IsValid) ? 0 : 1);
    }
  }// END class
}// END namespace