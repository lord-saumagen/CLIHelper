using System;
using CommandLineHelper;
using System.Reflection;

namespace TestCommand
{
  class TestCommand
  {
    static int Main(string[] args)
    {
      bool IsValid;
      ParameterObject parameterObject = new ParameterObject("TestCommand", Assembly.GetExecutingAssembly());
      IsValid = parameterObject.Process(args, showUsageOnEmptyArgs : false);
      return ((IsValid) ? 0 : 1);
    }
  }
}