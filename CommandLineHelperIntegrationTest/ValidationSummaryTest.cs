using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLineHelperIntegrationTest
{
  [TestClass]
  public class ValidationSummaryTest : BaseTest
  {
    public ValidationSummaryTest() : base()
    {

    }

    [TestMethod]
    public void ValidatonSummary_Int32FailTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>() { "[Name(\"number\")]", "public Int32 Int32Number { get; set; }\r\n" };
      parameterObjectSource = CreateParameterObjectSource(parameterList);

      System.IO.File.Delete(System.IO.Path.Combine(testCommandProjectDir, sourceFileName));
      using (var stream = System.IO.File.OpenWrite(System.IO.Path.Combine(testCommandProjectDir, sourceFileName)))
      {
        using (var strWriter = new StreamWriter(stream))
        {
          strWriter.Write(parameterObjectSource);
          strWriter.Flush();
          strWriter.Close();
        }
      }

      ResetTestCommand();

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] { "number=2.5" });
          Assert.AreNotEqual(0, processExecute.ProcessExitCode, "Validation of an invalid argument should result in a return code other than 0.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "║ number     ║ The value of command line argument 'number' is invalid.         ║", "The returned string should show the expected message.");

          Debugger.Log(1, "ValidationSummaryTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'Int32FailTest'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'Int32FailTest'.");
      }
    }


    public void ValidatonSummary_Int32PassTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>() { "[Name(\"number\")]", "public Int32 Int32Number { get; set; }\r\n" };
      parameterObjectSource = CreateParameterObjectSource(parameterList);

      System.IO.File.Delete(System.IO.Path.Combine(testCommandProjectDir, sourceFileName));
      using (var stream = System.IO.File.OpenWrite(System.IO.Path.Combine(testCommandProjectDir, sourceFileName)))
      {
        using (var strWriter = new StreamWriter(stream))
        {
          strWriter.Write(parameterObjectSource);
          strWriter.Flush();
          strWriter.Close();
        }
      }

      ResetTestCommand();

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] { "number=42" });
          Assert.AreEqual(0, processExecute.ProcessExitCode, "Validation should pass and result in a return code equal to 0.");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'Int32PassTest'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'Int32PassTest'.");
      }
    }


    [TestMethod]
    public void ValidatonSummary_ValueSetInt32FailTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>() { "[ValueSet( new object[] { 5, 10, 15, 20})]", "[Name(\"number\")]", "public Int32 Int32Number { get; set; }\r\n" };
      parameterObjectSource = CreateParameterObjectSource(parameterList);

      System.IO.File.Delete(System.IO.Path.Combine(testCommandProjectDir, sourceFileName));
      using (var stream = System.IO.File.OpenWrite(System.IO.Path.Combine(testCommandProjectDir, sourceFileName)))
      {
        using (var strWriter = new StreamWriter(stream))
        {
          strWriter.Write(parameterObjectSource);
          strWriter.Flush();
          strWriter.Close();
        }
      }

      ResetTestCommand();

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] { "number=6" });
          Assert.AreNotEqual(0, processExecute.ProcessExitCode, "Validation of an invalid argument should result in a return code other than 0.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "║ number     ║ The value of command line argument 'number' is not in the set   ║", "The returned string should show the expected message.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "[5, 10, 15, 20]", "The returned string should show the expected message.");

          Debugger.Log(1, "ValidationSummaryTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'ValueSetInt32FailTest'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'ValueSetInt32FailTest'.");
      }
    }


    [TestMethod]
    public void ValidatonSummary_ValueSetInt32PassTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>() { "[ValueSet( new object[] { 5, 10, 15, 20})]", "[Name(\"number\")]", "public Int32 Int32Number { get; set; }\r\n" };
      parameterObjectSource = CreateParameterObjectSource(parameterList);

      System.IO.File.Delete(System.IO.Path.Combine(testCommandProjectDir, sourceFileName));
      using (var stream = System.IO.File.OpenWrite(System.IO.Path.Combine(testCommandProjectDir, sourceFileName)))
      {
        using (var strWriter = new StreamWriter(stream))
        {
          strWriter.Write(parameterObjectSource);
          strWriter.Flush();
          strWriter.Close();
        }
      }

      ResetTestCommand();
      
      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] { "number=15" });
          Assert.AreEqual(0, processExecute.ProcessExitCode, "Validation should pass and result in a return code equal to 0.");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'ValueSetInt32PassTest'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'ValueSetInt32PassTest'.");
      }
    }


    [TestMethod]
    public void ValidatonSummary_DefaultValueInt32FailTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>() { "[DefaultValue((object) \"abc\")]", "[Name(\"number\")]", "public Int32 Int32Number { get; set; }\r\n" };
      parameterObjectSource = CreateParameterObjectSource(parameterList);

      System.IO.File.Delete(System.IO.Path.Combine(testCommandProjectDir, sourceFileName));
      using (var stream = System.IO.File.OpenWrite(System.IO.Path.Combine(testCommandProjectDir, sourceFileName)))
      {
        using (var strWriter = new StreamWriter(stream))
        {
          strWriter.Write(parameterObjectSource);
          strWriter.Flush();
          strWriter.Close();
        }
      }

      SetTestCommandShowUsageOnEmptyArgs(false);

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] {  });
          Assert.AreNotEqual(0, processExecute.ProcessExitCode, "Validation of an invalid argument should result in a return code other than 0.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "║ One or more of the command line arguments are invalid.                       ║", "The returned string should show the expected message.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "║ number     ║ The default value for the command line argument 'number' is     ║", "The returned string should show the expected message.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "The exceptions message is", "The returned string should show the expected message.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "║            ║ String' cannot be converted to type 'System.Int32'.'            ║", "The returned string should show the expected message.");
          
          Debugger.Log(1, "ValidationSummaryTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'DefaultValueInt32FailTest'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'DefaultValueInt32FailTest'.");
      }
    }


    [TestMethod]
    public void ValidatonSummary_DefaultValueInt32PassTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>() { "[DefaultValue((object) 42)]", "[Name(\"number\")]", "public Int32 Int32Number { get; set; }\r\n" };
      parameterObjectSource = CreateParameterObjectSource(parameterList);

      System.IO.File.Delete(System.IO.Path.Combine(testCommandProjectDir, sourceFileName));
      using (var stream = System.IO.File.OpenWrite(System.IO.Path.Combine(testCommandProjectDir, sourceFileName)))
      {
        using (var strWriter = new StreamWriter(stream))
        {
          strWriter.Write(parameterObjectSource);
          strWriter.Flush();
          strWriter.Close();
        }
      }

      SetTestCommandShowUsageOnEmptyArgs(false);

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] {  });
          Assert.AreEqual(0, processExecute.ProcessExitCode, "Validation should pass and result in a return code equal to 0.");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'DefaultValueInt32PassTest'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'DefaultValueInt32PassTest'.");
      }
    }


    [TestMethod]
    public void ValidatonSummary_MandatoryNullableInt32FailTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>() {"[Mandatory]",  "[Name(\"number\")]", "public Int32? Int32Number { get; set; }\r\n" };
      parameterObjectSource = CreateParameterObjectSource(parameterList);

      System.IO.File.Delete(System.IO.Path.Combine(testCommandProjectDir, sourceFileName));
      using (var stream = System.IO.File.OpenWrite(System.IO.Path.Combine(testCommandProjectDir, sourceFileName)))
      {
        using (var strWriter = new StreamWriter(stream))
        {
          strWriter.Write(parameterObjectSource);
          strWriter.Flush();
          strWriter.Close();
        }
      }

      SetTestCommandShowUsageOnEmptyArgs(false);

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] {  });
          Assert.AreNotEqual(0, processExecute.ProcessExitCode, "Validation of a missing mandatory argument should result in a return code other than 0.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "║ One or more of the command line arguments are invalid.                       ║", "The returned string should show the expected message.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "║ number     ║ The mandatory command line argument 'number' is missing or the  ║", "The returned string should show the expected message.");
          
          Debugger.Log(1, "ValidationSummaryTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'MandatoryNullableInt32FailTest'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'MandatoryNullableInt32FailTest'.");
      }
    }


    [TestMethod]
    public void ValidatonSummary_MandatoryNullableInt32PassTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>() {"[Mandatory]",  "[Name(\"number\")]", "public Int32? Int32Number { get; set; }\r\n" };
      parameterObjectSource = CreateParameterObjectSource(parameterList);

      System.IO.File.Delete(System.IO.Path.Combine(testCommandProjectDir, sourceFileName));
      using (var stream = System.IO.File.OpenWrite(System.IO.Path.Combine(testCommandProjectDir, sourceFileName)))
      {
        using (var strWriter = new StreamWriter(stream))
        {
          strWriter.Write(parameterObjectSource);
          strWriter.Flush();
          strWriter.Close();
        }
      }

      SetTestCommandShowUsageOnEmptyArgs(false);
      
      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] { "number=42" });
          Assert.AreEqual(0, processExecute.ProcessExitCode, "Validation should pass and result in a return code equal to 0.");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'MandatoryNullableInt32PassTest'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'MandatoryNullableInt32PassTest'.");
      }
    }

  }// END class
}// END namespace
