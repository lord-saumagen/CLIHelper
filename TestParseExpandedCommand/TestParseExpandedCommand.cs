using System;
using CommandLineHelper;
using System.Reflection;

namespace TestCommand
{
  class TestParseExpandedCommand
  {
    static int Main(string[] args)
    {
      bool IsValid;
      ParameterObjectParseExpanded parameterObjectParseExpanded = new ParameterObjectParseExpanded("TestParseExpandedCommand", Assembly.GetExecutingAssembly());
      IsValid = parameterObjectParseExpanded.Process(args, showUsageOnEmptyArgs : false);
      return ((IsValid) ? 0 : 1);
    }
  }
}