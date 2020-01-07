using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System;
using System.Linq;
using System.Reflection;

namespace CommandLineHelper
{

  class ParameterComplex : CommandLineHelper.ParameterBase
  {
    //
    // Public property without a name and not market as internal.
    //
    [Name("String")]
    public string StringParam
    {
      get;
      set;
    }

    [Name("Char")]
    public char CharParam
    {
      get;
      set;
    }

    [Name("NullChar")]
    public char? NullCharParam
    {
      get;
      set;
    }

    [Name("Bool")]
    public bool BoolParam
    {
      get;
      set;
    }

    [Name("NullBool")]
    public bool? NullBoolParam
    {
      get;
      set;
    }

    [Name("Int16")]
    public Int16 Int16Param
    {
      get;
      set;
    }

    [Name("NullInt16")]
    public Int16? NullInt16Param
    {
      get;
      set;
    }

    [Name("UInt16")]
    public UInt16 UInt16Param
    {
      get;
      set;
    }

    [Name("NullUInt16")]
    public UInt16? NullUInt16Param
    {
      get;
      set;
    }

    [Name("Int32")]
    public Int32 Int32Param
    {
      get;
      set;
    }

    [Name("NullInt32")]
    public Int32? NullInt32Param
    {
      get;
      set;
    }

    [Name("UInt32")]
    public UInt32 UInt32Param
    {
      get;
      set;
    }

    [Name("NullUInt32")]
    public UInt32? NullUInt32Param
    {
      get;
      set;
    }

    [Name("Int64")]
    public Int64 Int64Param
    {
      get;
      set;
    }

    [Name("NullInt64")]
    public Int64? NullInt64Param
    {
      get;
      set;
    }

    [Name("UInt64")]
    public UInt64 UInt64Param
    {
      get;
      set;
    }

    [Name("NullUInt32")]
    public UInt64? NullUInt64Param
    {
      get;
      set;
    }

    [Name("Single")]
    public Single SingleParam
    {
      get;
      set;
    }

    [Name("NullSingle")]
    public Single? NullSingleParam
    {
      get;
      set;
    }

    [Name("Double")]
    public Double DoubleParam
    {
      get;
      set;
    }

    [Name("NullDouble")]
    public Double? NullDoubleParam
    {
      get;
      set;
    }

    [Name("Decimal")]
    public Decimal DecimalParam
    {
      get;
      set;
    }

    [Name("NullDecimal")]
    public Decimal? NullDecimalParam
    {
      get;
      set;
    }

    //
    // Public property marked as internal should be skipped during 
    // parse and validation.
    // 
    [Internal]
    [Name("Internal")]
    public String InternalParam
    {
      get;
      set;
    }

    //
    // Private property should be skipped during parse
    // and validation.
    //
    [Name("Foo")]
    private String FooParam
    {
      get;
      set;
    }

    //
    // Public property with a private set accessor should
    // be skipped during parse and validation.
    //
    [Name("Bar")]
    public String BarParam
    {
      get;
      private set;
    }

    [Mandatory]
    public string MStringParam
    {
      get;
      set;
    }

    [Mandatory]
    public Int32? MNullInt32Param
    {
      get;
      set;
    }

    [Mandatory]
    public Boolean? MNullBoolParam
    {
      get;
      set;
    }



    public ParameterComplex(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

    public override void Parse(string[] args)
    {
      base.Parse(args);
    }

    public override bool Validate()
    {
      return base.Validate();
    }

  }


  [TestClass]
  [TestCategory("ParameterComplexTest")]
  public class ParameterComplexTest
  {

    [TestMethod]
    public void ParameterComplex_ConstructorTest()
    {
      ParameterComplex parameterComplex = new ParameterComplex("ParameterComplexTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterComplex, "The 'ParameterComplex' constructor should return a valid instance.");
    }

    [TestMethod]
    public void ParameterComplex_ValidationFailTest()
    {
      ParameterComplex parameterComplex = new ParameterComplex("ParameterComplexTest", Assembly.GetExecutingAssembly());
      string[] args = new String[] { "String=\"C://TEMP\"", "Single=1.5", "Double=2.5", "Decimal=25.123", "Int16=16", 
                                     "Int32=32", "Int64=64",  "Bool=true", 
                                     "Internal=\"Internal parsed value.\"", "Foo=123", "Bar=test", 
                                     "MNullBoolParam=true" };
      parameterComplex.Parse(args);

      Assert.AreEqual("C://TEMP", parameterComplex.StringParam, "The parameter property 'StringParam' value should match with the parsed argument value.");
      Assert.AreEqual(1.5, parameterComplex.SingleParam, "The parameter property 'SingleParam' value should match with the  parsed argument value.");
      Assert.AreEqual(2.5, parameterComplex.DoubleParam, "The parameter property 'DoubleParam' value should match with the  parsed argument value.");
      Assert.AreEqual(25.123m, parameterComplex.DecimalParam, "The parameter property 'DecimalParam' value should match with the  parsed argument value.");
      Assert.AreEqual(16, parameterComplex.Int16Param, "The parameter property 'Int16Param' value should match with the parsed argument value.");
      Assert.AreEqual(32, parameterComplex.Int32Param, "The parameter property 'Int32Param' value should match with the parsed argument value.");
      Assert.AreEqual(64, parameterComplex.Int64Param, "The parameter property 'Int64Param' value should match with the parsed argument value.");
      Assert.AreEqual(true, parameterComplex.BoolParam, "The parameter property 'BoolParam' value should match with the parsed argument value.");

      //
      // Excluded parameter
      //
      Assert.AreNotEqual("Internal parsed value.", parameterComplex.InternalParam, "The parameter property 'InternalParam' value should not match with the parsed argument value.");
      //Assert.AreNotEqual("123", parameterComplex.FooParam, "The parameter 'FooParam' property value should not match with the parsed argument value.");
      Assert.AreNotEqual("test", parameterComplex.BarParam, "The parameter property 'BarParam' should not match with the parsed argument value.");

      //
      // Mandatory parameter
      //
      Assert.AreEqual(null, parameterComplex.MStringParam, "The parameter property 'MStringParam' value should be null.");
      Assert.AreEqual(null, parameterComplex.MNullInt32Param, "The parameter property 'MNullInt32Param' should be null.");
      //Assert.AreEqual(true, parameterComplex.MNullBoolParam, "The parameter property 'MNullBoolParam' value should be null.");

      var IsValid = parameterComplex.Validate();
      Assert.IsFalse(IsValid, "The validation for object 'parameterComplex' should fail because of the missing mandatory arguments.");
      Assert.AreEqual(2, parameterComplex.ValidationErrorList.Count(), "There should be 2 validation errors.");

      var MStringParamErr = parameterComplex.ValidationErrorList.Where(err => err.PropertyMetaInfo.Name == "MStringParam").FirstOrDefault();
      var MNullInt32ParamErr = parameterComplex.ValidationErrorList.Where(err => err.PropertyMetaInfo.Name == "MNullInt32Param").FirstOrDefault();
      //varMNullBoolParamErr = parameterComplex.ValidationErrorList.Where(err => err.PropertyMetaInfo.Name == "MNullBoolParam").FirstOrDefault();

      Assert.IsNotNull(MStringParamErr, "There should be an error object for the mandatory 'MString' property which wasn't set.");
      Assert.IsNotNull(MNullInt32ParamErr, "There should be an error object for the mandatory 'MInt32Err' property which wasn't set.");
    }



    [TestMethod]
    public void ParameterComplex_ValidationFailMandatoryTest()
    {
      ParameterComplex parameterComplex = new ParameterComplex("ParameterComplexTest", Assembly.GetExecutingAssembly());
      string[] args = new String[] { "MNullBoolParam=true" };

      parameterComplex.Parse(args);

      //
      // Mandatory parameter
      //
      Assert.AreEqual(null, parameterComplex.MStringParam, "The parameter property 'MStringParam' value should be null.");
      Assert.AreEqual(null, parameterComplex.MNullInt32Param, "The parameter property 'MNullInt32Param' should be null.");
      //Assert.AreEqual(true, parameterComplex.MNullBoolParam, "The parameter property 'MNullBoolParam' value should be null.");

      var IsValid = parameterComplex.Validate();

      Assert.IsFalse(IsValid, "The validation for object 'parameterComplex' should fail because of the missing mandatory arguments.");
      Assert.AreEqual(2, parameterComplex.ValidationErrorList.Count(), "There should be 2 validation errors.");

      var MStringParamErr = parameterComplex.ValidationErrorList.Where(err => err.PropertyMetaInfo.Name == "MStringParam").FirstOrDefault();
      var MNullInt32ParamErr = parameterComplex.ValidationErrorList.Where(err => err.PropertyMetaInfo.Name == "MNullInt32Param").FirstOrDefault();
      //varMNullBoolParamErr = parameterComplex.ValidationErrorList.Where(err => err.PropertyMetaInfo.Name == "MNullBoolParam").FirstOrDefault();

      Assert.IsNotNull(MStringParamErr, "There should be an error object for the mandatory 'MStringParam' property which wasn't set.");
      Assert.IsNotNull(MNullInt32ParamErr, "There should be an error object for the mandatory 'MNullInt32Param' property which wasn't set.");
    }

    [TestMethod]
    public void ParameterComplex_ValidationFailInvalidArgumentTest()
    {
      ParameterComplex parameterComplex = new ParameterComplex("ParameterComplexTest", Assembly.GetExecutingAssembly());
      string[] args = new String[] { "String=\"C://TEMP\"", "Single=not a number"};

      parameterComplex.Parse(args);

      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameterComplex.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "String").Single().ParseResult, "The meta info parse result of parameter property 'StringParam' should be 'PARSE_SUCCEEDED'.");
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameterComplex.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "Single").Single().ParseResult, "The meta info of parameter property 'SingleParm' should be 'PARSE_FAILED'.");

      var IsValid = parameterComplex.Validate();

      Assert.IsFalse(IsValid, "The validation for object 'parameterComplex' should fail because of the invalid argument value and the missing mandatory arguments.");
      Assert.AreEqual(4, parameterComplex.ValidationErrorList.Count(), "There should be 4 validation errors.");

      var SingleParamErr = parameterComplex.ValidationErrorList.Where(err => err.PropertyMetaInfo.Name == "Single").FirstOrDefault();

      Assert.IsNotNull(SingleParamErr, "There should be an error object for the 'SingleParam' property which was assigned an invalid value.");
    }

    [TestMethod]
    public void ParameterComplex_HelpTest()
    {
      ParameterComplex parameterComplex = new ParameterComplex("ParameterComplexTest", Assembly.GetExecutingAssembly());
      string helpStr = parameterComplex.CreateHelp();
      Assert.IsNotNull(helpStr);
    }

  }// END class
}// END namespace
