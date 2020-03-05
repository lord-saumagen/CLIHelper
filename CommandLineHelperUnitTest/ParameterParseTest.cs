using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System;
using System.Linq;
using System.Reflection;

namespace CommandLineHelper
{

  [TestClass]
  [TestCategory("ParameterParseTest")]
  public class ParameterParseTest
  {
    private string[] argsNull = new String[] {};

    [TestMethod]
    public void ParameterParseTest_ParseNullChar()
    {
      string[] args;
      ParameterComplex parameter = new ParameterComplex("ParameterParseTest" , Assembly.GetExecutingAssembly());

      parameter.Parse(argsNull);
      Assert.AreEqual(ParseResultEnum.NOT_PARSED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullChar").Single().ParseResult);

      args = new string[] {"NullChar="};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullChar").Single().ParseResult);

      args = new string[] {"NullChar=abcd"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullChar").Single().ParseResult);

      args = new string[] {"NullChar=' '"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullChar").Single().ParseResult);

      args = new string[] {"NullChar='@'"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullChar").Single().ParseResult);

      args = new string[] {"NullChar=X"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullChar").Single().ParseResult);

      args = new string[] {"NullChar=null"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullChar").Single().ParseResult);

    }


    [TestMethod]
    public void ParameterParseTest_ParseString()
    {
      string[] args;
      ParameterComplex parameter = new ParameterComplex("ParameterParseTest", Assembly.GetExecutingAssembly());

      parameter.Parse(argsNull);
      Assert.AreEqual(ParseResultEnum.NOT_PARSED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "String").Single().ParseResult);

      args = new string[] {"String=\"\t  \""};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "String").Single().ParseResult);

      args = new string[] {"String=\"\r\n\""};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "String").Single().ParseResult);

      args = new string[] {"String=\"String parse\""};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "String").Single().ParseResult);

      args = new string[] {"String=a b c d"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "String").Single().ParseResult);

      args = new string[] {"String=\"User Id=User;Password=P@ssword;POOLING=True;MIN POOL SIZE=1;MAX POOL SIZE=100;Incr Pool Size=5;DECR POOL SIZE=1;CONNECTION LIFETIME=60;VALIDATE CONNECTION=True;Data Source=ABCDEFG.RZ\""};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "String").Single().ParseResult);

      args = new string[] {"String=\"Server=ABCDEFG,1435;Database=Test;Trusted_Connection=yes;\""};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "String").Single().ParseResult);
    }

    [TestMethod]
    public void ParameterParseTest_ParseUInt32()
    {
      string[] args;
      ParameterComplex parameter = new ParameterComplex("ParameterParseTest", Assembly.GetExecutingAssembly());

      parameter.Parse(argsNull);
      Assert.AreEqual(ParseResultEnum.NOT_PARSED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "UInt32").Single().ParseResult);

      args = new string[] {"UInt32="};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "UInt32").Single().ParseResult);

      args = new string[] {"UInt32=Null"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "UInt32").Single().ParseResult);

      args = new string[] {"UInt32=-13"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "UInt32").Single().ParseResult);

      args = new string[] {"UInt32=13"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "UInt32").Single().ParseResult);
    }

    [TestMethod]
    public void ParameterParseTest_ParseNullDouble()
    {
      string[] args;
      ParameterComplex parameter = new ParameterComplex("ParameterParseTest", Assembly.GetExecutingAssembly());

      parameter.Parse(argsNull);
      Assert.AreEqual(ParseResultEnum.NOT_PARSED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullDouble").Single().ParseResult);

      args = new string[] {"NullDouble="};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullDouble").Single().ParseResult);

      args = new string[] {"NullDouble=true"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullDouble").Single().ParseResult);

      args = new string[] {"NullDouble=0.123456"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullDouble").Single().ParseResult);

      args = new string[] {"NullDouble=Null"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullDouble").Single().ParseResult);
    }

    [TestMethod]
    public void ParameterParseTest_ParseNullBool()
    {
      string[] args;
      ParameterComplex parameter = new ParameterComplex("ParameterParseTest", Assembly.GetExecutingAssembly());

      parameter.Parse(argsNull);
      Assert.AreEqual(ParseResultEnum.NOT_PARSED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullBool").Single().ParseResult);

      args = new string[] {"NullBool="};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullBool").Single().ParseResult);

      args = new string[] {"NullBool=0.123456"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_FAILED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullBool").Single().ParseResult);

      args = new string[] {"NullBool=true"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullBool").Single().ParseResult);

      args = new string[] {"NullBool=Yes"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullBool").Single().ParseResult);

      args = new string[] {"NullBool=False"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullBool").Single().ParseResult);

      args = new string[] {"NullBool=no"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullBool").Single().ParseResult);

      args = new string[] {"NullBool=Null"};
      parameter.Parse(args);
      Assert.AreEqual(ParseResultEnum.PARSE_SUCCEEDED, parameter.PropertyMetaInfoList.Where(propInfo => propInfo.Name == "NullBool").Single().ParseResult);
    }
    
  }// END class
}// END namespace