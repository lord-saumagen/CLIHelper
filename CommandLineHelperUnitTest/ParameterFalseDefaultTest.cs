using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System.Reflection;

namespace CommandLineHelper
{
  class ParameterFalseDefault : CommandLineHelper.ParameterBase
  {

    [DefaultValue((object)-25)]
    public UInt32 UInt32Number
    {
      get;
      set;
    }

    [DefaultValue((object)true)]
    public string StringValue
    {
      get;
      set;
    }

    [DefaultValue((object)"Hello")]
    public bool? BooleanValue
    {
      get;
      set;
    }

    public ParameterFalseDefault(string commandName, Assembly commandAssembly) : base(commandName,  commandAssembly, new DisplayHelper())
    { }

  }// END class


  [TestClass]
  [TestCategory("ParameterFalseDefaultTest")]
  public class ParameterFalseDefaultTest
  {
    [TestMethod]
    public void ParameterFalse_ConstructorTest()
    {
      ParameterFalseDefault parameterFalseDefault = new ParameterFalseDefault("ParameterFalseDefault_ConstructorTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterFalseDefault, "The 'ParameterFalseDefault' constructor should return a valid instance.");
    }

    [TestMethod]
    public void ParameterFalse_ValidationTestParameterNotSet()
    {
      ParameterFalseDefault parameterFalseDefault = new ParameterFalseDefault("ParameterFalseDefault_ValidationTestParameterNotSet", Assembly.GetExecutingAssembly());
      var validationResult = parameterFalseDefault.Validate();
      //      Assert.IsTrue(validationResult, "The validation result of 'ParameterDefault' should pass with the default value.");
      //      Assert.IsNotNull(parameterDefault.NumberValue, "The property with the default attribute assigned should not longer be null.");
      //      Assert.AreEqual(25, parameterDefault.NumberValue, "The current value should match with the default value.");
      //
      //      Assert.IsTrue(validationResult, "The validation result of 'ParameterDefault' should pass with the default value.");
      //      Assert.IsNotNull(parameterDefault.StringValue, "The property with the default attribute assigned should not longer be null.");
      //      Assert.AreEqual("Test Value", parameterDefault.StringValue, "The current value should match with the default value.");
      //
      //      Assert.IsTrue(validationResult, "The validation result of 'ParameterDefault' should pass with the default value.");
      //      Assert.IsNotNull(parameterDefault.BooleanValue, "The property with the default attribute assigned should not longer be null.");
      //      Assert.AreEqual(true, parameterDefault.BooleanValue, "The current value should match with the default value.");
    }
    
  }// END class
}// END namespace