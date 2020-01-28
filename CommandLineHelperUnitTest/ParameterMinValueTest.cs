using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System.Reflection;
using System.Linq;
using System;

namespace CommandLineHelper
{
  class ParameterMinValue : CommandLineHelper.ParameterBase
  {
    [MinValue(40, "The value of 'TestInt16' must be greater or equal 40.")]
    public Int16 TestInt16
    {
      get;
      set;
    }

    [MinValue(41, "The value of 'TestInt' must be greater or equal 41.")]
    public int TestInt
    {
      get;
      set;
    }

    [MinValue(42, "The value of 'TestInt64' must be greater or equal 42.")]
    public Int64 TestInt64
    {
      get;
      set;
    }


    [MinValue(43.0, "The value of 'TestDouble' must be greater or equal 43.")]
    public double TestDouble
    {
      get;
      set;
    }

    [MinValue(44.4, "The value of 'TestDecimal' must be greater or equal 44.4.")]
    public decimal TestDecimal
    {
      get;
      set;
    }

    [MinValue(45, "The value of 'TestNullInt' must be greater or equal 45.")]
    public int? TestNullInt
    {
      get;
      set;
    }

    [MinValue(46.6, "The value of 'TestNullDouble' must be greater or equal 46.6.")]
    public double? TestNullDouble
    {
      get;
      set;
    }

    [MinValue(47.7, "The value of 'TestNullDecimal' must be greater or equal 47.7.")]
    public decimal? TestNullDecimal
    {
      get;
      set;
    }

    [MinValue(50.1, "The value of 'TestSingle' must be greater or equal 50.1.")]
    public Single TestSingle
    {
      get;
      set;
    }

    [MinValue(51, "The value of 'TestNullSingle' must be greater or equal 51.")]
    public Single? TestNullSingle
    {
      get;
      set;
    }

    public ParameterMinValue(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class

  class ParameterMinValueFail : CommandLineHelper.ParameterBase
  {
    [MinValue((object) 42, "The value of 'TestStr' must be greater or equal 42.")]
    public string TestStr
    {
      get;
      set;
    }

    public ParameterMinValueFail(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class


  [TestClass]
  [TestCategory("ParameterMinValueTest")]
  public class ParameterMinValueTest
  {
    [TestMethod]
    public void ParameterMinValueTest_ConstructorTest()
    {
      ParameterMinValue parameterMinValue = new ParameterMinValue("ParameterMinValueTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterMinValue, "The 'ParameterMinValue' constructor should return a valid instance.");
    }


    [TestMethod]
    public void ParameterMinValueTest_WrongTypeValidationFailTest()
    {
      string[] args = new string[] { "TestStr=\"abcdef\"" };
      ParameterMinValueFail parameterMinValueFail = new ParameterMinValueFail("ParameterMinValueTest", Assembly.GetExecutingAssembly());
      parameterMinValueFail.Parse(args);
      var validationResult = parameterMinValueFail.Validate();
      Assert.IsFalse(validationResult, "The validation result of 'ParameterMinValueTest' attached to an invalid type should fail.");
      var validationError = parameterMinValueFail.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestStr").SingleOrDefault();
      StringAssert.Contains(validationError.Message, "MinValueAttribute", "The error message should match with the expected value.");
      StringAssert.Contains(validationError.Message, "is not allowed on properties of type", "The error message should match with the expected value.");
    }


    [TestMethod]
    public void ParameterMinValueTest_ValidationFailTest()
    {
      ParameterMinValue parameterMinValue;
      ValidationError validationError;
      bool isValid;
      string[] args;
      
      //
      // MIN VALUES
      // 40, 41, 42, 43, 44.4, 45, 46.6, 47.7, 50.1, 51
      //

      parameterMinValue = new ParameterMinValue("ParameterMinValueTest", Assembly.GetExecutingAssembly());
      
      args = new string[] { "TestInt16=39", "TestInt=41", "TestInt64=42", "TestDouble=43", "TestDecimal=44.4", "TestNullInt=45", "TestNullDouble=46.6", "TestNullDecimal=47.7", "TestSingle=50.1", "TestNullSingle=51"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestInt16").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestInt16'.");

      args = new string[] { "TestInt16=40", "TestInt=40", "TestInt64=42", "TestDouble=43", "TestDecimal=44.4", "TestNullInt=45", "TestNullDouble=46.6", "TestNullDecimal=47.7", "TestSingle=50.1", "TestNullSingle=51"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestInt").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestInt'.");

      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=41", "TestDouble=43", "TestDecimal=44.4", "TestNullInt=45", "TestNullDouble=46.6", "TestNullDecimal=47.7", "TestSingle=50.1", "TestNullSingle=51"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestInt64").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestInt64'.");

      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=42", "TestDouble=42", "TestDecimal=44.4", "TestNullInt=45", "TestNullDouble=46.6", "TestNullDecimal=47.7", "TestSingle=50.1", "TestNullSingle=51"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestDouble").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestDouble'.");

      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=42", "TestDouble=43", "TestDecimal=44.3", "TestNullInt=45", "TestNullDouble=46.6", "TestNullDecimal=47.7", "TestSingle=50.1", "TestNullSingle=51"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestDecimal").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestDecimal'.");

      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=42", "TestDouble=43", "TestDecimal=44.4", "TestNullInt=44", "TestNullDouble=46.6", "TestNullDecimal=47.7", "TestSingle=50.1", "TestNullSingle=51"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullInt").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullInt'.");

      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=42", "TestDouble=43", "TestDecimal=44.4", "TestNullInt=45", "TestNullDouble=46.5", "TestNullDecimal=47.7", "TestSingle=50.1", "TestNullSingle=51"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullDouble").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullDouble'.");

      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=42", "TestDouble=43", "TestDecimal=44.4", "TestNullInt=45", "TestNullDouble=46.6", "TestNullDecimal=47.6", "TestSingle=50.1", "TestNullSingle=51"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullDecimal").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullDecimal'.");

      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=42", "TestDouble=43", "TestDecimal=44.4", "TestNullInt=45", "TestNullDouble=46.6", "TestNullDecimal=47.7", "TestSingle=50", "TestNullSingle=51"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestSingle").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestSingle'.");

      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=42", "TestDouble=43", "TestDecimal=44.4", "TestNullInt=45", "TestNullDouble=46.6", "TestNullDecimal=47.7", "TestSingle=50.1", "TestNullSingle=50.9"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullSingle").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullSingle'.");

      //
      // NULL TEST
      //

      parameterMinValue = new ParameterMinValue("ParameterMinValueTest", Assembly.GetExecutingAssembly());
      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=42", "TestDouble=43", "TestDecimal=44.4",                 "TestNullDouble=46.6", "TestNullDecimal=47.7", "TestSingle=50.1", "TestNullSingle=50.9"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullInt").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullInt'.");

      parameterMinValue = new ParameterMinValue("ParameterMinValueTest", Assembly.GetExecutingAssembly());
      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=42", "TestDouble=43", "TestDecimal=44.4", "TestNullInt=45", "TestNullDouble=46.6",                         "TestSingle=50.1", "TestNullSingle=50.9"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsFalse(isValid, "The validation of 'parameterMinValue' should fail for an invalid value.");
      validationError = parameterMinValue.ValidationErrorList.Where(err => err.PropertyMetaInfo.PropertyInfo.Name == "TestNullDecimal").SingleOrDefault();
      Assert.IsNotNull(validationError, "The validation should fail for the invalid value on 'TestNullDecimal'.");
    }


    [TestMethod]
    public void ParameterMinValueTest_ValidationPassTest()
    {
      bool isValid;
      string[] args;
      
      //
      // MIN VALUES
      // 40, 41, 42, 43, 44.4, 45, 46.6, 47.7, 50.1, 51
      //

      ParameterMinValue parameterMinValue = new ParameterMinValue("ParameterMinValueTest", Assembly.GetExecutingAssembly());
      
      args = new string[] { "TestInt16=40", "TestInt=41", "TestInt64=42", "TestDouble=43", "TestDecimal=44.4", "TestNullInt=45", "TestNullDouble=46.6", "TestNullDecimal=47.7", "TestSingle=50.1", "TestNullSingle=51"  };
      parameterMinValue.Parse(args);
      isValid = parameterMinValue.Validate();
      Assert.IsTrue(isValid, "The validation of 'parameterMinValue' should pass.");
    }

  }// END class
}// END namespace