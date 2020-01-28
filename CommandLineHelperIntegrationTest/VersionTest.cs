using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandLineHelperIntegrationTest
{
  [TestClass]
  public class VersionTest : BaseTest
  {
    public VersionTest() : base()
    {
    }

    [TestMethod]
    public void VersionTest_PassTest()
    {
      ProcessExecute processExecute;

      ResetTestCommand();

      processExecute = new ProcessExecute();
      processExecute.Start(testCommandPublishDir, testCommand, new string[] { "version" });
      Assert.AreNotEqual(0, processExecute.ProcessExitCode, "A version request should always result in an invalid parameter object.");
      StringAssert.Contains(processExecute.ProcessStandardOutput, "1.2.3.4", "The returned string should show expected version.");
    }
  }// END class
}// END namespace
