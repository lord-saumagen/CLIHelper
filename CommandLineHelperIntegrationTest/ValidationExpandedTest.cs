using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLineHelperIntegrationTest
{


  [TestClass]
  public class ValidationExpandedTest
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
      executionPath = Path.GetFullPath("../../../../TestValidationExpandedCommand");

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
    public void ValidationExpanded_NoArgumentTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestValidationExpandedCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestValidationExpandedCommand", new string[] { });
      Assert.AreNotEqual(0, processExecute.ProcessExitCode, "The command should fail because of the missing arguments.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "One or more of the command line arguments are invalid.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "The mandatory command line argument 'delivery-date'");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "The mandatory command line argument 'Email'");
      Debugger.Log(1, "ValidationExpandedTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
    }

    [TestMethod]
    public void ValidationExpanded_InvalidDateTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestValidationExpandedCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestValidationExpandedCommand", new string[] { "delivery-date=2020-01-01", "email=who@where.com" });
      Assert.AreNotEqual(0, processExecute.ProcessExitCode, "The command should fail because of the invalid 'delivery-date' argument.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "One or more of the command line arguments are invalid.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "The delivery-date must be a date at least 2 days");
      Debugger.Log(1, "ValidationExpandedTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
    }

    [TestMethod]
    public void ValidationExpanded_InvalidEmailTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestValidationExpandedCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestValidationExpandedCommand", new string[] { "delivery-date=2050-01-01", "email=blah@blah" });
      Assert.AreNotEqual(0, processExecute.ProcessExitCode, "The command should fail because of the invalid 'delivery-date' argument.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "One or more of the command line arguments are invalid.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "The value of argument 'email' must be a valid email address.");
      Debugger.Log(1, "ValidationExpandedTest", processExecute.ProcessErrorOutput + "\r\n\r\n");
    }

    [TestMethod]
    public void ParserExpanded_PassTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestParseExpandedCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestParseExpandedCommand", new string[] {  "delivery-date=2050-01-01", "email=who@where.com" });
      Assert.AreEqual(0, processExecute.ProcessExitCode, "The command should pass for valid arguments.");
    }


  }// END class
}// END namespace