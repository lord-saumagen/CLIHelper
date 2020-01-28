using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System.Reflection;
using System.Linq;

namespace CommandLineHelper
{
  class ParameterMaxStringLength : CommandLineHelper.ParameterBase
  {
    [MaxStringLength(6, "The value of 'TestStr' must not be longer than 6 characters.")]
    public string TestStr
    {
      get;
      set;
    }

    public ParameterMaxStringLength(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class

  class ParameterMaxStringLengthFail : CommandLineHelper.ParameterBase
  {
    [MaxStringLength(6, "The value of 'TestStr' must not be longer than 6 characters.")]
    public int TestStr
    {
      get;
      set;
    }

    public ParameterMaxStringLengthFail(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class


  [TestClass]
  [TestCategory("ParameterMaxStringLengthTest")]
  public class ParameterMaxStringLengthTest
  {
    [TestMethod]
    public void ParameterMaxStringLength_ConstructorTest()
    {
      ParameterMaxStringLength parameterMaxStringLength = new ParameterMaxStringLength("ParameterMaxStringLengthTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterMaxStringLength, "The 'ParameterMaxStringLength' constructor should return a valid instance.");
    }


    [TestMethod]
    public void ParameterMaxStringLength_WrongTypeValidationFailTest()
    {
      string[] args = new string[] { "TestStr=\"012345\"" };
      ParameterMaxStringLengthFail parameterMaxStringLength = new ParameterMaxStringLengthFail("ParameterMaxStringLengthTest", Assembly.GetExecutingAssembly());
      parameterMaxStringLength.Parse(args);
      var validationResult = parameterMaxStringLength.Validate();
      Assert.IsFalse(validationResult, "The validation result of 'ParameterMaxStringLength' attached to an invalid type should fail.");
      var validationError = parameterMaxStringLength.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestStr").SingleOrDefault();
      StringAssert.Contains(validationError.Message, "MaxStringLengthAttribute",  "The error message should match with the expected value." );
      StringAssert.Contains(validationError.Message, "is not allowed on properties of type:", "The error message should match with the expected value.");
    }


    [TestMethod]
    public void ParameterMaxStringLength_ValidationFailTest()
    {
      string[] args = new string[] { "TestStr=\"0123456789\"" };
      ParameterMaxStringLength parameterMaxStringLength = new ParameterMaxStringLength("ParameterMaxStringLengthTest", Assembly.GetExecutingAssembly());
      parameterMaxStringLength.Parse(args);
      var validationResult = parameterMaxStringLength.Validate();
      Assert.IsFalse(validationResult, "The validation result of 'ParameterMaxStringLength' with an invalid 'TestStr' property should fail.");
      var validationError = parameterMaxStringLength.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestStr").SingleOrDefault();
      StringAssert.Contains(validationError.Message, "The value of",  "The error message should match with the expected value." );
      StringAssert.Contains(validationError.Message, "longer than 6 characters", "The error message should match with the expected value.");
    }


    [TestMethod]
    public void ParameterMaxStringLength_ValidationTest()
    {
      string[] args = new string[] { "TestStr=\"012345\"" };
      ParameterMaxStringLength parameterMaxStringLength = new ParameterMaxStringLength("ParameterMaxStringLengthTest", Assembly.GetExecutingAssembly());
      parameterMaxStringLength.Parse(args);
      var validationResult = parameterMaxStringLength.Validate();
      Assert.IsTrue(validationResult, "The validation result of 'ParameterMaxStringLength' with a valid 'TestStr' property should be true.");
      Assert.AreEqual("012345", parameterMaxStringLength.TestStr, "The parameter property value should match with the parsed value.");
    }

  }// END class
}// END namespace