
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;

namespace CommandLineHelper
{


  [Usage(@"
  parameter_with_usage [number=<Int32 number>] name=<String>
  
  The 'number' value must be in the range [0..100]. The default value is 0.
  The 'name' must not be an empty string and the length must be less or equal 50.")]
  class ParameterWithUsage : ParameterBase
  {
    [Name("number")]
    public Int32 Int32Param
    {
      get;
      set;
    }

    [Mandatory]
    [Name("name")]
    public String StringParam
    {
      get;
      set;
    }

    public ParameterWithUsage(String commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    {

    }
  }// END class

  class ParameterWithoutUsage : ParameterBase
  {
    [Name("number")]
    public Int32 Int32Param
    {
      get;
      set;
    }

    [Mandatory]
    [Name("name")]
    public String StringParam
    {
      get;
      set;
    }

    public ParameterWithoutUsage(String commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    {

    }
  }// END class

  [TestClass]
  [TestCategory("ParameterUsageTest")]
  public class ParameterUsageTest
  {

    [TestMethod]
    public void ParameterUsageTest_UsageTestWithUsage()
    {
      string result;
      ParameterWithUsage parameterWithUsage = new ParameterWithUsage("ParameterUsageTest", Assembly.GetExecutingAssembly());
      result = parameterWithUsage.CreateUsage();
      Assert.IsNotNull(result);

      Debugger.Log(1, "ParameterUsageTest", result + "\r\n");
    }

    [TestMethod]
    public void ParameterUsageTest_UsageTestWithoutUsage()
    {
      string result;
      ParameterWithoutUsage parameterWithoutUsage = new ParameterWithoutUsage("ParameterUsageTest", Assembly.GetExecutingAssembly());
      result = parameterWithoutUsage.CreateUsage();
      Assert.IsNotNull(result);

      Debugger.Log(1, "ParameterUsageTest", result + "\r\n");
    }

  }// END class
}// END namespace
