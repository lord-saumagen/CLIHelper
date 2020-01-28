using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLineHelperIntegrationTest
{
  [TestClass]
  public class UsageTest : BaseTest
  {
    public UsageTest() : base()
    {
    }

    [TestMethod]
    public void UsageTest_CustomPassTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;
      string customUsage;

      customUsage = "testcommand [-help] [-version] number<string in the range -10 - 10>";
      parameterList = new List<string>() { "[Name(\"number\")]", "public Int32 Int32Number { get; set; }\r\n" };
      parameterObjectSource = CreateParameterObjectSource(parameterList, usage: customUsage);

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

      SetTestCommandShowUsageOnEmptyArgs(true);

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] {});
          Assert.AreNotEqual(0, processExecute.ProcessExitCode, "Validation of the missing arguments should result in a return code other than 0.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, customUsage, "The returned string should show the expected message.");
          Debugger.Log(1, "UsageTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
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

    [TestMethod]
    public void UsageTest_StandardPassTest()
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

      SetTestCommandShowUsageOnEmptyArgs(true);

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] {});
          Assert.AreNotEqual(0, processExecute.ProcessExitCode, "Validation of the missing argument should result in a return code other than 0.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "Usage:", "The returned string should show the expected message.");
          StringAssert.Contains(processExecute.ProcessErrorOutput, "[help] [version]", "The returned string should show the expected message.");
          Debugger.Log(1, "UsageTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
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

  }// END class
}// END namespace
