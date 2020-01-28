using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System.Reflection;

namespace CommandLineHelper
{
  class ParameterMandatory : CommandLineHelper.ParameterBase
  {
    [Mandatory]
    public string TestStr
    {
      get;
      set;
    }

    [Mandatory]
    public int? TestNullInt
    {
      get;
      set;
    }

    public ParameterMandatory(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class


  [TestClass]
  [TestCategory("ParameterMandatoryTest")]
  public class ParameterMandatoryTest
  {
    [TestMethod]
    public void ParameterMandatory_ConstructorTest()
    {
      ParameterMandatory parameterMandatory = new ParameterMandatory("ParameterMandatoryTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterMandatory, "The 'ParameterMandatory' constructor should return a valid instance.");
    }

    [TestMethod]
    public void ParameterMandatory_ValidationFailTest()
    {
      ParameterMandatory parameterMandatory = new ParameterMandatory("ParameterMandatoryTest", Assembly.GetExecutingAssembly());
      var validationResult = parameterMandatory.Validate();
      Assert.IsFalse(validationResult, "The validation result of 'ParameterMandatory' with no arguments should fail.");
    }

    [TestMethod]
    public void ParameterMandatory_ValidationNoTestStrFailTest()
    {
      string[] args = new string[] { "TestNullInt=42" };
      ParameterMandatory parameterMandatory = new ParameterMandatory("ParameterMandatoryTest", Assembly.GetExecutingAssembly());
      parameterMandatory.Parse(args);
      var validationResult = parameterMandatory.Validate();
      Assert.IsFalse(validationResult, "The validation result of 'ParameterMandatory' with a missing 'TestStr' argument should fail.");
    }

    [TestMethod]
    public void ParameterMandatory_ValidationNoTestNullIntFailTest()
    {
      string[] args = new string[] { "TestStr=\"Test Name\"" };
      ParameterMandatory parameterMandatory = new ParameterMandatory("ParameterMandatoryTest", Assembly.GetExecutingAssembly());
      parameterMandatory.Parse(args);
      var validationResult = parameterMandatory.Validate();
      Assert.IsFalse(validationResult, "The validation result of 'ParameterMandatory' with a missing 'TestNullInt' argument should fail.");
    }

    [TestMethod]
    public void ParameterMandatory_ValidationPassTest()
    {
      string[] args = new string[] { "TestStr=\"Test Name\"", "TestNullInt=42" };
      ParameterMandatory parameterMandatory = new ParameterMandatory("ParameterMandatoryTest", Assembly.GetExecutingAssembly());
      parameterMandatory.Parse(args);
      var validationResult = parameterMandatory.Validate();
      Assert.IsTrue(validationResult, "The validation result of 'ParameterMandatory' with a valid 'TestStr' property should be true.");
      Assert.AreEqual("Test Name", parameterMandatory.TestStr, "The parameter property value should match with the parsed value.");
    }

  }// END class
}// END namespace