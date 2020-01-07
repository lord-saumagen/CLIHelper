using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System.Reflection;

namespace CommandLineHelper
{
  class ParameterInternal : CommandLineHelper.ParameterBase
  {

    [Internal]
    public string TestString
    {
      get;
      set;
    }

    [Internal]
    public int TestNumber
    {
      get;
      set;
    }

    public ParameterInternal(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class

  [TestClass]
  [TestCategory("ParameterInternalTest")]
  public class ParameterInternalTest
  {
    [TestMethod]
    public void ParameterInternal_ConstructorTest()
    {
      ParameterInternal parameterInternal = new ParameterInternal("ParameterInternalTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterInternal, "The 'ParameterInternal' constructor should return a valid instance.");
    }

    [TestMethod]
    public void ParameterInternal_ValidationTest()
    {
      ParameterInternal parameterInternal = new ParameterInternal("ParameterInternalTest", Assembly.GetExecutingAssembly());
      parameterInternal.TestString = "asdf";
      parameterInternal.TestNumber = 42;
      var validationResult = parameterInternal.Validate();
      Assert.IsTrue(validationResult, "The validation of 'ParameterInternal' should pass.");
    }

  }// END class
}// END namespace