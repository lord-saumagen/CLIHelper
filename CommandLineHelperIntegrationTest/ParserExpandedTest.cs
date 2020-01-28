using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLineHelperIntegrationTest
{


  [TestClass]
  public class ParserExpandedTest
  {
    public static bool IsWindows
    {
      get
      {
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
      }
    }

    public static bool IsLinux
    {
      get
      {
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
      }
    }

    [ClassInitialize()]
    public static void ClassInit(TestContext context)
    {
      ProcessExecute processExecute;
      string executionPath;

      processExecute = new ProcessExecute();
      executionPath = Path.GetFullPath("../../../../TestParseExpandedCommand");

      if (IsWindows)
      {
        processExecute.Start(executionPath, "cmd", new string[] { "/C", "dotnet", "build", "--force" });
      }

      if (IsLinux)
      {
        processExecute.Start(executionPath, "dotnet", new string[] { "build", "--force" });
      }
    }



    [TestMethod]
    public void ParserExpanded_NoArgumentTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestParseExpandedCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestParseExpandedCommand", new string[] { });
      Assert.AreNotEqual(0, processExecute.ProcessExitCode, "The command should fail because of the missing argument.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "One or more of the command line arguments are invalid.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "The mandatory command line argument 'delivery-date'");
      Debugger.Log(1, "ParserExpandedTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
    }


    [TestMethod]
    public void ParserExpanded_InvalidArgumentTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestParseExpandedCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestParseExpandedCommand", new string[] { "delivery-date=12345" });
      Assert.AreNotEqual(0, processExecute.ProcessExitCode, "The command should fail because of the invalid argument.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "One or more of the command line arguments are invalid.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "The mandatory command line argument 'delivery-date'");
      Debugger.Log(1, "ParserExpandedTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
    }


    [TestMethod]
    public void ParserExpanded_ValidArgumentTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestParseExpandedCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestParseExpandedCommand", new string[] { "delivery-date=2030-01-01" });
      Assert.AreEqual(0, processExecute.ProcessExitCode, "The command should succeed with a valid argument.");
    }


  }// END class
}// END namespace