using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System;
using System.Linq;
using System.Reflection;

namespace CommandLineHelper
{
  class ParameterName : CommandLineHelper.ParameterBase
  {

    [Name("NumberName")]
    public int NumberProperty
    {
      get;
      set;
    }

    //
    // Empty name attribute should fall back to property name.
    // 
    [Name("")]
    public string StringProperty
    {
      get;
      set;
    }

    public ParameterName(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class


  [TestClass]
  [TestCategory("ParameterNameTest")]
  public class ParameterNameTest
  {
    [TestMethod]
    public void ParameterName_ConstructorTest()
    {
      ParameterName parameterName = new ParameterName("ParameterNameTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterName, "The 'ParameterName' constructor should return a valid instance.");
    }


    [TestMethod]
    public void ParameterName_NameTest()
    {
      ParameterName parameterName = new ParameterName("ParameterNameTest", Assembly.GetExecutingAssembly());
      //
      // The property meta info name for the 'StringProperty' should be the 
      // property name because of the empty string in the 'NameAttribute' attribute.
      //
      Assert.AreEqual(1, parameterName.PropertyMetaInfoList.Where(metaProp => metaProp.Name == "StringProperty").Count(), "The parameter object should have one parameter property named 'StringProperty'.");
      //
      // The property meta info name for the 'NumberProperty' should be the 
      // name attache in the 'NameAttribute' attribute.
      //
      Assert.AreEqual(1, parameterName.PropertyMetaInfoList.Where(metaProp => metaProp.Name == "NumberName").Count(), "The parameter object should have one parameter property named 'NumberName'.");
    }


    [TestMethod]
    public void ParameterName_ParseTest()
    {
      ParameterName parameterName = new ParameterName("ParameterNameTest", Assembly.GetExecutingAssembly());
      String[] args = new string[] {"StringProperty=asdf", "NumberName=99" };
      parameterName.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameterName.PropertyMetaInfoList.Where(metaInfo => metaInfo.Name == "StringProperty").Single().ParseResult);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameterName.PropertyMetaInfoList.Where(metaInfo => metaInfo.Name == "NumberName").Single().ParseResult);
    }


    [TestMethod]
    public void ParameterName_ValidationTest()
    {
      string[] args = new string[] {"NumberName=99", "StringProperty=asdf"};
      ParameterName parameterName = new ParameterName("ParameterNameTest", Assembly.GetExecutingAssembly());
      parameterName.Parse(args);
      var validationResult = parameterName.Validate();
      Assert.IsTrue(validationResult, "The validation of the 'parameterName' object should pass.");
    }


  }// END class
}// END namespace