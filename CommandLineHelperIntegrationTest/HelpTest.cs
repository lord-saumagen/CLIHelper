using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLineHelperIntegrationTest
{
  
  [TestClass]
  public class HelpTest : BaseTest
  {
    public HelpTest() : base()
    {
    }


    [TestMethod]
    public void Help_NoParameterTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>();
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

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] { "help" });
          Assert.AreNotEqual(0, processExecute.ProcessExitCode, "A help request results in a validation fail. For that reason the return value should not be 0.");
          StringAssert.Contains(processExecute.ProcessStandardOutput, "The current parameter object has no parameter property!",  "The returned string should match the expectation.");
          StringAssert.Contains(processExecute.ProcessStandardOutput, "TestCommand [help] [version]", "The returned string should match the expectation.");

          Debugger.Log(1, "NoParameterTest", processExecute.ProcessStandardOutput+ "\r\n");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'NoParameter'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'NoParameter'.");
      }
    }


    [TestMethod]
    public void Help_MandatoryStringNameParameterTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>() { "[Mandatory]", "[Name(\"source\")]", "public string SourcePath { get; set; }\r\n" };
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

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] { "abcde", "help" });
          Assert.AreNotEqual(0, processExecute.ProcessExitCode, "A help request results in a validation fail. For that reason the return value should not be 0.");
          
          
          StringAssert.Contains(processExecute.ProcessStandardOutput, "║+║ source      ║ String ║           ║ source<String>                          ║", "The returned string should show the expected message.");
          StringAssert.Contains(processExecute.ProcessStandardOutput, "TestCommand [help] [version] source=<String>","The returned string should show the expected message.");

          Debugger.Log(1, "MandatoryStringNameParameterTest", processExecute.ProcessStandardOutput+ "\r\n");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'MandatoryStringNameParam'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'MandatoryStringNameParam'.");
      }
    }


    [TestMethod]
    public void Help_DefaultDescriptionNameUInt32ParameterTest()
    {
      string parameterObjectSource;
      List<string> parameterList;
      ProcessExecute processExecute;

      parameterList = new List<string>() { "[Description(\"The number of items from the last delivery. It must be a number greater or equal 0.\")]", "[Name(\"amount\")]", "[DefaultValue((object)42)]", "public UInt32 itemCount { get; set; }\r\n" };
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

      if (BuildTarget())
      {
        if (PublishTarget())
        {
          processExecute = new ProcessExecute();
          processExecute.Start(testCommandPublishDir, testCommand, new string[] { "abcde", "help" });
          Assert.AreNotEqual(0, processExecute.ProcessExitCode, "A help request results in a validation fail. For that reason the return value should not be 0.");
          StringAssert.Contains(processExecute.ProcessStandardOutput, "╔═╦═[Parameter]═╦═[Type]═╦═[Default]═╦═[Description]═══════════════════════════╗", "The returned string should show the expected message.");
          StringAssert.Contains(processExecute.ProcessStandardOutput, "║-║ amount      ║ UInt32 ║ 42        ║ The number of items from the last       ║", "The returned string should show the expected message.");
          StringAssert.Contains(processExecute.ProcessStandardOutput, "║ ║             ║        ║           ║ delivery. It must be a number greater   ║", "The returned string should show the expected message."); 
          StringAssert.Contains(processExecute.ProcessStandardOutput, "║ ║             ║        ║           ║ or equal 0.                             ║", "The returned string should show the expected message.");
          StringAssert.Contains(processExecute.ProcessStandardOutput, "TestCommand [help] [version] [amount=<UInt32>]","The returned string should show the expected message.");

          Debugger.Log(1, "DefaultDescriptionNameUInt32ParameterTest", processExecute.ProcessStandardOutput+ "\r\n");
        }
        else
        {
          Assert.Fail("Publishing the test target failed in function 'DefaultDescriptionNamedUInt32Param'.");
        }
      }
      else
      {
        Assert.Fail("Building the test target failed in function 'DefaultDescriptionNamedUInt32Param'.");
      }
    }

  }// END class
}// END namespace
