using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System.Reflection;
using System.Linq;
using System;

namespace CommandLineHelper
{
  class ParameterMaxValue : CommandLineHelper.ParameterBase
  {
    [MaxValue(100, "The value of 'TestInt16' must be less or equal 100.")]
    public Int16 TestInt16
    {
      get;
      set;
    }

    [MaxValue(101, "The value of 'TestNullInt16' must be less or equal 101.")]
    public Int16? TestNullInt16
    {
      get;
      set;
    }

    [MaxValue(102, "The value of 'TestInt' must be less or equal 102.")]
    public int TestInt
    {
      get;
      set;
    }

    [MaxValue(103, "The value of 'TestNullInt' must be less or equal 103.")]
    public int? TestNullInt
    {
      get;
      set;
    }

    [MaxValue(104, "The value of 'TestInt64' must be less or equal 104.")]
    public Int64 TestInt64
    {
      get;
      set;
    }

    [MaxValue(105, "The value of 'TestNullInt64' must be less or equal 105.")]
    public Int64? TestNullInt64
    {
      get;
      set;
    }

    [MaxValue(106.5, "The value of 'TestSingle' must be less or equal 106.5.")]
    public Single TestSingle
    {
      get;
      set;
    }

    [MaxValue(107.5, "The value of 'TestNullSingle' must be less or equal 107.5.")]
    public Single? TestNullSingle
    {
      get;
      set;
    }

    [MaxValue(108.5, "The value of 'TestDouble' must be less or equal 108.5.")]
    public double TestDouble
    {
      get;
      set;
    }

    [MaxValue(109.5, "The value of 'TestNullDouble' must be less or equal 109.5.")]
    public double? TestNullDouble
    {
      get;
      set;
    }

    [MaxValue(110.5, "The value of 'TestDecimal' must be less or equal 110.5.")]
    public decimal TestDecimal
    {
      get;
      set;
    }


    [MaxValue(111.5, "The value of 'TestNullDecimal' must be less or equal 111.5.")]
    public decimal? TestNullDecimal
    {
      get;
      set;
    }


    public ParameterMaxValue(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class

  class ParameterMaxValueFail : CommandLineHelper.ParameterBase
  {
    [MaxValue(42, "The value of 'TestStr' must be less or equal 42.")]
    public string TestStr
    {
      get;
      set;
    }

    public ParameterMaxValueFail(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class


  [TestClass]
  [TestCategory("parameterMaxValueTest")]
  public class ParameterMaxValueTest
  {
    [TestMethod]
    public void ParameterMaxValueTest_ConstructorTest()
    {
      ParameterMaxValue parameterMaxValue = new ParameterMaxValue("ParameterMaxValueTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterMaxValue, "The 'ParameterMaxValue' constructor should return a valid instance.");
    }


    [TestMethod]
    public void ParameterMaxValueTest_WrongTypeValidationFailTest()
    {
      string[] args = new string[] { "TestStr=\"abcdef\"" };
      ParameterMaxValueFail parameterMaxValueFail = new ParameterMaxValueFail("ParameterMaxValueTest", Assembly.GetExecutingAssembly());
      parameterMaxValueFail.Parse(args);
      var validationResult = parameterMaxValueFail.Validate();
      Assert.IsFalse(validationResult, "The validation result of 'parameterMaxValueFail' attached to an invalid type should fail.");
      var validationError = parameterMaxValueFail.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestStr").SingleOrDefault();
      StringAssert.Contains(validationError.Message, "MaxValueAttribute", "The error message should match with the expected value.");
      StringAssert.Contains(validationError.Message, "is not allowed on properties of type", "The error message should match with the expected value.");
    }


    [TestMethod]
    public void ParameterMaxValueTest_ValidationFailTest()
    {
      ParameterMaxValue parameterMaxValue;
      ValidationError validationError;
      bool isValid;
      string[] args;
      
      //
      // MAX VALUES
      // 100, 101, 102, 103, 104, 105, 106.5, 107.5, 108.5, 109.5, 110.5, 111.5 
      //

      parameterMaxValue = new ParameterMaxValue("ParameterMaxValueTest", Assembly.GetExecutingAssembly());
      
      args = new string[] { "TestInt16=101", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestInt16").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestInt16'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=102", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullInt16").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullInt16'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=103", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestInt").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestInt'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=104", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullInt").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullInt'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=105", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestInt64").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestInt64'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=106",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullInt64").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullInt64'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.7", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestSingle").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestSingle'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.7", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullSingle").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullSingle'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.6", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestDouble").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestDouble'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.53", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullDouble").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullDouble'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.57", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestDecimal").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestDecimal'.");

      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.59" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullDecimal").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullDecimal'.");

      //
      // NULL TEST
      //

      parameterMaxValue = new ParameterMaxValue("ParameterMaxValueTest", Assembly.GetExecutingAssembly());
      args = new string[] { "TestInt16=100",  "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullInt16").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullInt16'.");

      parameterMaxValue = new ParameterMaxValue("ParameterMaxValueTest", Assembly.GetExecutingAssembly());
      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102",  "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullInt").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullInt'.");

      parameterMaxValue = new ParameterMaxValue("ParameterMaxValueTest", Assembly.GetExecutingAssembly());
      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullInt64").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullInt64'.");

      parameterMaxValue = new ParameterMaxValue("ParameterMaxValueTest", Assembly.GetExecutingAssembly());
      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestInt64=104",
                            "TestSingle=106.5",              "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullSingle").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullSingle'.");

      parameterMaxValue = new ParameterMaxValue("ParameterMaxValueTest", Assembly.GetExecutingAssembly());
      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestInt64=104",
                            "TestSingle=106.5",  "TestNullSingle=107.5", "TestDouble=108.5"                  , "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullDouble").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullDouble'.");

      parameterMaxValue = new ParameterMaxValue("ParameterMaxValueTest", Assembly.GetExecutingAssembly());
      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestInt64=104",
                            "TestSingle=106.5",  "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5"                };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMaxValueTest' should fail for an invalid value.");
      validationError = parameterMaxValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullDecimal").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullDecimal'.");

    }


    [TestMethod]
    public void ParameterMaxValueTest_ValidationPassTest()
    {
      bool isValid;
      string[] args;
      
      //
      // MAX VALUES
      // 100, 101, 102, 103, 104, 105, 106.5, 107.5, 108.5, 109.5, 110.5, 111.5 
      //

      ParameterMaxValue parameterMaxValue = new ParameterMaxValue("ParameterMaxValueTest", Assembly.GetExecutingAssembly());
      
      args = new string[] { "TestInt16=100", "TestNullInt16=101", "TestInt=102", "TestNullInt=103", "TestInt64=104", "TestNullInt64=105",
                            "TestSingle=106.5", "TestNullSingle=107.5", "TestDouble=108.5", "TestNullDouble=109.5", "TestDecimal=110.5", "TestNullDecimal=111.5" };

      parameterMaxValue.Parse(args);
      isValid = parameterMaxValue.Validate();
      Assert.IsTrue(isValid, "The validation of 'ParameterMaxValue' should pass.");
    }

  }// END class
}// END namespace