using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System;
using System.Linq;
using System.Reflection;

namespace CommandLineHelper
{
  class ParameterValueSet : CommandLineHelper.ParameterBase
  {

    [Name("StringSet")]
    [ValueSet(new object[] {"Left", "Right", "Top", "Down"})]
    public string StringSet
    {
      get;
      set;
    }

    [Name("UInt32Set")]
    [ValueSet(new Object[] {1, 2, 3})]
    [DefaultValue(1)]
    public UInt32 UInt32Set
    {
      get;
      set;
    }


    [Name("SingleSet")]
    [ValueSet(new Object[] {1.23, 1.25, 1.27})]
    [Mandatory]
    public Single SingleSet
    {
      get;
      set;
    }

    [Name("Int64NullSet")]
    [ValueSet(new Object[] {-2, -1, 0, 1, 2})]
    [Mandatory]
    public Int64? Int64Null
    {
      get;
      set;
    } 

    [Name("DecimalNullSet")]
    [ValueSet(new Object[] {-2.05, -1.025, 0, 1.025, 2.05, null})]
    public Decimal? DecimalNull
    {
      get;
      set;
    } 

    public ParameterValueSet(string commandName, Assembly commandAssembly) : base(commandName, commandAssembly, new DisplayHelper())
    { }

  }// END class


  [TestClass]
  [TestCategory("ParameterValueSetTest")]
  public class ParameterValueSetTest
  {
    [TestMethod]
    public void ParameterValueSet_ConstructorTest()
    {
      ParameterValueSet parameterValueSet = new ParameterValueSet("ParameterValueSetTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterValueSet, "The 'ParameterValueSet' constructor should return a valid instance.");
    }


    [TestMethod]
    public void ParameterValueSet__ParseTest()
    {
      ParameterValueSet parameterValueSet = new ParameterValueSet("ParameterValueSetTest", Assembly.GetExecutingAssembly());
      String[] args = new string[] {"StringSet=Left", "UInt32Set=3", "SingleSet=1.25", "Int64NullSet=5" };
      parameterValueSet.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameterValueSet.PropertyMetaInfoList.Where(metaInfo => metaInfo.Name == "StringSet").Single().ParseResult);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameterValueSet.PropertyMetaInfoList.Where(metaInfo => metaInfo.Name == "UInt32Set").Single().ParseResult);
    }


    [TestMethod]
    public void ParameterValueSet__ValidationTest()
    {
      string[] args = new string[] {"Int64NullSet=-1", "StringSet=Left", "UInt32Set=3", "SingleSet=1.25" };
      ParameterValueSet parameterValueSet = new ParameterValueSet("ParameterValueSetTest", Assembly.GetExecutingAssembly());
      parameterValueSet.Parse(args);
      var validationResult = parameterValueSet.Validate();
      Assert.IsTrue(validationResult, "The validation of the 'parameterValueSet' object should pass.");
    }


    [TestMethod]
    public void ParameterValueSet__ValidationTestFailInvalidArgument()
    {
      string[] args;
      bool validationResult;

      ParameterValueSet parameterValueSet = new ParameterValueSet("ParameterValueSetTest", Assembly.GetExecutingAssembly());

      args = new string[] {"DecimalNullSet=null", "StringSet=Left", "UInt32Set=3",  "SingleSet=1.25", "Int64NullSet=-1" };

      parameterValueSet.Parse(args);
      validationResult = parameterValueSet.Validate();
      Assert.IsTrue(validationResult, "The validation of the 'parameterValueSet' object should pass.");
      Assert.AreEqual(null, parameterValueSet.DecimalNull);
      Assert.AreEqual("Left", parameterValueSet.StringSet);
      Assert.AreEqual((UInt32)  3, parameterValueSet.UInt32Set);
      Assert.AreEqual(1.25, parameterValueSet.SingleSet);
      Assert.AreEqual((Int64) (-1), parameterValueSet.Int64Null);

      args = new string[] {"Int64NullSet=-1", "StringSet=Left", "UInt32Set=3", "SingleSet=nop" };

      parameterValueSet.Parse(args);
      validationResult = parameterValueSet.Validate();
      Assert.IsFalse(validationResult, "The validation of the 'parameterValueSet' object should fail because of the invalid 'SingleSet' argument value.");
      Assert.IsTrue(parameterValueSet.ValidationErrorList.Count == 1, "A validation error should be set for the failed validation.");
      Assert.AreEqual("SingleSet", parameterValueSet.ValidationErrorList[0].PropertyMetaInfo.Name, "The validation error should be an error regarding  the 'SingleSet' argument.");
      Assert.IsTrue(parameterValueSet.ValidationErrorList[0].Message.IndexOf("is missing or the value is invalid") >  -1, "The validation error should reveal that an argument is missing or invalid.");

      args = new string[] {"StringSet=Left", "UInt32Set=3",  "SingleSet=1.25" };

      parameterValueSet.Parse(args);
      validationResult = parameterValueSet.Validate();
      Assert.IsFalse(validationResult, "The validation of the 'parameterValueSet' object should fail because of the missing mandatory 'Int64NullSet' argument.");
      Assert.IsTrue(parameterValueSet.ValidationErrorList.Count == 1, "A validation error should be set for the failed validation.");
      Assert.AreEqual("Int64NullSet", parameterValueSet.ValidationErrorList[0].PropertyMetaInfo.Name, "The validation error should be an error regarding  the 'Int64NullSet' argument.");
      Assert.IsTrue(parameterValueSet.ValidationErrorList[0].Message.IndexOf("is missing or the value is invalid") >  -1, "The validation error should reveal that an argument is missing or invalid.");

    }


    [TestMethod]
    public void ParameterValueSet__ValidationTestFailOutOfSetArgument()
    {
      string[] args;
      bool validationPasses;

      args = new string[] {"StringSet=Left", "UInt32Set=3", "SingleSet=1.29" ,"Int64NullSet=-2"};
      ParameterValueSet parameterValueSet = new ParameterValueSet("ParameterValueSetTest", Assembly.GetExecutingAssembly());
      parameterValueSet.Parse(args);
      validationPasses = parameterValueSet.Validate();
      Assert.IsFalse(validationPasses, "The validation of the 'parameterValueSet' object should fail because of the value of the 'SingleSet' argument is not an element of the attached 'ValueSetAttribute'.");
      Assert.IsTrue(parameterValueSet.ValidationErrorList.Count == 1, "A validation error should be set for the failed validation.");
      Assert.IsTrue(parameterValueSet.ValidationErrorList[0].Message.IndexOf("not in the set of allowed values") >  -1, "The validation error should be an error regarding  the 'ParameterValueSet'.");

      args = new string[] {"StringSet=Left", "UInt32Set=3", "SingleSet=1.27"};
      parameterValueSet.Parse(args);
      validationPasses = parameterValueSet.Validate();
      Assert.IsFalse(validationPasses, "The validation of the 'parameterValueSet' object should fail because of the value of the 'SingleSet' argument is not an element of the attached 'ValueSetAttribute'.");
      Assert.IsTrue(parameterValueSet.ValidationErrorList.Count == 1, "A validation error should be set for the failed validation.");
      Assert.AreEqual("Int64NullSet", parameterValueSet.ValidationErrorList[0].PropertyMetaInfo.Name);
      Assert.IsTrue(parameterValueSet.ValidationErrorList[0].Message.IndexOf("mandatory") >  -1, "The validation error should be an error regarding  the mandatory 'Int64NullSet' parameter.");
    }

    [TestMethod]
    public void ParameterValueSet__ValidationTestFailInvalidDefaultValue()
    {
    }

    [TestMethod]
    public void ParameterValueSet__ValidationTestFailInvalidValueSet()
    {
    }
  }// END class
}// END namespace