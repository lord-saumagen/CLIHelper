using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLineHelperIntegrationTest
{


  [TestClass]
  public class ScreenCreationOverrideTest
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
      executionPath = Path.GetFullPath("../../../../TestScreenCreationOverrideCommand");

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
    public void ScreenCreationOverride_NoArgumentUsageTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestScreenCreationOverrideCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestScreenCreationOverrideCommand", new string[] { "showUsageOnEmptyArgs" });
      Assert.AreNotEqual(0, processExecute.ProcessExitCode, "The command should fail because of the missing argument.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "Custom usage screen");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "TestScreenCreationOverrideCommand [help] [version]");
      Debugger.Log(1, "ScreenCreationOverride", processExecute.ProcessErrorOutput + "\r\n\r\n");
    }

    
    [TestMethod]
    public void ScreenCreationOverride_NoArgumentSummaryTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestScreenCreationOverrideCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestScreenCreationOverrideCommand", new string[] {  });
      Assert.AreNotEqual(0, processExecute.ProcessExitCode, "The command should fail because of the missing argument.");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "Custom validation screen");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "name");
      StringAssert.Contains(processExecute.ProcessErrorOutput, "items-total");
      Debugger.Log(1, "ScreenCreationOverride", processExecute.ProcessErrorOutput + "\r\n\r\n");
    }


    [TestMethod]
    public void ScreenCreationOverride_HelpTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestScreenCreationOverrideCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestScreenCreationOverrideCommand", new string[] { "help" });
      Assert.AreNotEqual(0, processExecute.ProcessExitCode, "The command should fail because of the help request.");
      StringAssert.Contains(processExecute.ProcessStandardOutput, "Custom help screen");
      StringAssert.Contains(processExecute.ProcessStandardOutput, "The TestScreenCreationOverrideCommand is a command");
      Debugger.Log(1, "ScreenCreationOverride", processExecute.ProcessStandardOutput + "\r\n\r\n");
    }


    [TestMethod]
    public void ScreenCreationOverride_VersionTest()
    {
      ProcessExecute processExecute;
      string executionPath;

      executionPath = Path.GetFullPath("../../../../TestScreenCreationOverrideCommand/bin/Debug/netcoreapp3.1");
      processExecute = new ProcessExecute();
      processExecute.Start(executionPath, "TestScreenCreationOverrideCommand", new string[] { "version" });
      Assert.AreNotEqual(0, processExecute.ProcessExitCode, "The command should fail because of the version request.");
      StringAssert.Contains(processExecute.ProcessStandardOutput, "Custom version screen");
      StringAssert.Contains(processExecute.ProcessStandardOutput, "5.5.5.5");
      Debugger.Log(1, "ScreenCreationOverride", processExecute.ProcessStandardOutput + "\r\n\r\n");
    }


    // [TestMethod]
    // public void ParserExpanded_ValidArgumentTest()
    // {
    //   ProcessExecute processExecute;
    //   string executionPath;

    //   executionPath = Path.GetFullPath("../../../../TestParseExpandedCommand/bin/Debug/netcoreapp3.1");
    //   processExecute = new ProcessExecute();
    //   processExecute.Start(executionPath, "TestParseExpandedCommand", new string[] { "delivery-date=2030-01-01" });
    //   Assert.AreEqual(0, processExecute.ProcessExitCode, "The command should succeed with a valid argument.");
    // }


  }// END class
}// END namespace