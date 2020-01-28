using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLineHelper
{
  [TestClass]
  [TestCategory("ParameterBaseTest")]
  public class ParameterBaseTest
  {

    [TestMethod]
    public void ParameterBase_ConstructorTest()
    {
      ParameterBase parameter = new ParameterBase("ParameterBaseTest", Assembly.GetExecutingAssembly(), new DisplayHelper());
      Assert.IsNotNull(parameter, "The 'Parameter' constructor should return a valid instance.");
    }


    [TestMethod]
    public void ParameterBase_ValidationTest()
    {
      ParameterBase parameter = new CommandLineHelper.ParameterBase("ParameterBaseTest", Assembly.GetExecutingAssembly(), new DisplayHelper());

      bool validationResult = parameter.Validate();
      Assert.IsTrue(validationResult, "A parameter object without any parameter properties should always validate.");
      Assert.IsFalse(parameter.IsParsed, "The parameter should have the 'IsParsed' flag set to 'false' before parsing.");
      Assert.IsFalse(parameter.IsHelpRequest, "The parameter should have the 'IsHelpRequest' flag set to 'false' before parsing.");
    }


    [TestMethod]
    public void ParameterBase_CreateHelpTest()
    {
      bool validationResult;
      string helpStr;

      ParameterBase parameter = new CommandLineHelper.ParameterBase("ParameterBaseTest", Assembly.GetExecutingAssembly(), new DisplayHelper());
      validationResult = parameter.Validate();
      helpStr = parameter.CreateHelp();
      Assert.IsFalse(string.IsNullOrEmpty(helpStr), "The 'CreateHelp' function should return a none empty string.");
      StringAssert.Contains(helpStr, "has no parameter property", "The returned string should show the expected message when called on an object with no parameter property.");
    }


    [TestMethod]
    public void ParameterBase_ParseTest()
    {
      bool validationResult;
      ParameterBase parameter = new CommandLineHelper.ParameterBase("ParameterBaseTest", Assembly.GetExecutingAssembly(), new DisplayHelper());
      string[] args = new string[] { "help" };

      parameter.Parse(args);
      Assert.IsTrue(parameter.IsParsed, "The parameter should have the 'IsParsed' flag set to 'true' after parsing.");
      Assert.IsTrue(parameter.IsHelpRequest, "The parameter should have the 'IsHelpRequest' flag set to 'true' after parsing.");
      validationResult = parameter.Validate();
      Assert.IsFalse(validationResult, "The validation result of 'CommandLineHelper.Parameter' after parsing with a help request should be false.");
    }

    [TestMethod]
    public void ParameterBase_ProcessTest()
    {
      //bool validationResult;
      ParameterBase parameter = new CommandLineHelper.ParameterBase("ParameterBaseTest", Assembly.GetExecutingAssembly(), new DisplayHelper());
      string[] args = new string[] { "help" };

      parameter.Process(args);
    }


  }// END class
}// END namespace

