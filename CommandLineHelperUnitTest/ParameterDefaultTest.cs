using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System.Reflection;

namespace CommandLineHelper
{
  class ParameterDefault : CommandLineHelper.ParameterBase
  {

    [DefaultValue(25)]
    public int? NumberValue
    {
      get;
      set;
    }

    [DefaultValue("Test Value")]
    public string StringValue
    {
      get;
      set;
    }

    [DefaultValue(true)]
    public bool? BooleanValue
    {
      get;
      set;
    }

    public ParameterDefault(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class

  [TestClass]
  [TestCategory("ParameterDefaultTest")]
  public class ParameterDefaultValueTest
  {
    
    [TestMethod]
    public void ParameterDefault_ConstructorTest()
    {
      ParameterDefault parameterDefault = new ParameterDefault("ParameterDefaultTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterDefault, "The 'ParameterDefault' constructor should return a valid instance.");
    }


    [TestMethod]
    public void ParameterDefault_ParameterNotSetValidationTest()
    {
      ParameterDefault parameterDefault = new ParameterDefault("ParameterDefaultTest", Assembly.GetExecutingAssembly());
      var validationResult = parameterDefault.Validate();

      //
      // The validation process sets the default value for every
      // parameter property which hasn't a parsed value assigned.
      //
      Assert.IsTrue(validationResult, "The validation result of 'ParameterDefault' should pass with the default value.");
      Assert.IsNotNull(parameterDefault.NumberValue, "The property with the default attribute assigned should not longer be null.");
      Assert.AreEqual(25, parameterDefault.NumberValue, "The current value should match with the default value.");

      Assert.IsTrue(validationResult, "The validation result of 'ParameterDefault' should pass with the default value.");
      Assert.IsNotNull(parameterDefault.StringValue, "The property with the default attribute assigned should not longer be null.");
      Assert.AreEqual("Test Value", parameterDefault.StringValue, "The current value should match with the default value.");

      Assert.IsTrue(validationResult, "The validation result of 'ParameterDefault' should pass with the default value.");
      Assert.IsNotNull(parameterDefault.BooleanValue, "The property with the default attribute assigned should not longer be null.");
      Assert.AreEqual(true, parameterDefault.BooleanValue, "The current value should match with the default value.");

      parameterDefault.NumberValue = 12345;
      parameterDefault.StringValue = "A new test string.";
      parameterDefault.BooleanValue = true;
    }


    [TestMethod]
    public void ParameterDefault_ParameterSetValidationTest()
    {
      ParameterDefault parameterDefault = new ParameterDefault("ParameterDefaultTest", Assembly.GetExecutingAssembly());

      //
      // The validation process sets the default value for every
      // parameter property which hasn't a parsed value assigned.
      // That means that the validation process will overide the 
      // assigned values as long as the meta info 'IsParsed' flag
      // for the given parameter property is false.
      //
      parameterDefault.NumberValue = 12345;
      parameterDefault.StringValue = "A new test string.";
      parameterDefault.BooleanValue = true;

      var validationResult = parameterDefault.Validate();

      Assert.IsTrue(validationResult, "The validation result of 'ParameterDefault' should pass with the default value.");
      Assert.IsNotNull(parameterDefault.NumberValue, "The property with the default attribute assigned should not longer be null.");
      Assert.AreEqual(25, parameterDefault.NumberValue, "The current value should match with the default value.");

      Assert.IsTrue(validationResult, "The validation result of 'ParameterDefault' should pass with the default value.");
      Assert.IsNotNull(parameterDefault.StringValue, "The property with the default attribute assigned should not longer be null.");
      Assert.AreEqual("Test Value", parameterDefault.StringValue, "The current value should match with the default value.");

      Assert.IsTrue(validationResult, "The validation result of 'ParameterDefault' should pass with the default value.");
      Assert.IsNotNull(parameterDefault.BooleanValue, "The property with the default attribute assigned should not longer be null.");
      Assert.AreEqual(true, parameterDefault.BooleanValue, "The current value should match with the default value.");
    }


    [TestMethod]
    public void ParameterDefault_ParameterParsedValidationTest()
    {
      string[] args = new string[] { "NumberValue=12345", "StringValue=\"A new test string\"", "BooleanValue=true" };
      ParameterDefault parameterDefault = new ParameterDefault("ParameterDefaultTest", Assembly.GetExecutingAssembly());

      //
      // If a parameter property has a parsed value assigned
      // the default value will be ignored.
      //

      parameterDefault.Parse(args);
      var validationResult = parameterDefault.Validate();

      Assert.IsTrue(validationResult, "The validation result of 'ParameterDefault' should pass with the parsed values.");

      Assert.AreEqual(12345, parameterDefault.NumberValue, "The current value should match with the parsed value.");
      Assert.AreEqual("A new test string", parameterDefault.StringValue, "The current value should match with the parsed value.");
      Assert.AreEqual(true, parameterDefault.BooleanValue, "The current value should match with the parsed value.");
    }

  }// END class
}// END namespace