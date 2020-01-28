using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CommandLineHelperIntegrationTest
{
  public class BaseTest
  {
    protected string parameterObjectTop;
    protected string parameterObjectBottom;

    protected const string testCommand = "TestCommand";
    protected const string sourceFileName = "ParameterObject.cs";
    protected const string sourcePath = "./bin/Debug/netcoreapp3.1/ParameterObject.cs";
    protected const string testCommandProjectDir = "../../../../TestCommand/";
    protected const string testCommandPublishDir = "../../../../TestCommand/bin/Debug/netcoreapp3.1/publish/";

    public bool IsWindows
    {
      get
      {
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
      }
    }

    public bool IsLinux
    {
      get
      {
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
      }
    }

    public BaseTest()
    {
      using (var stream = System.IO.File.OpenRead("./ParameterObjectTop.txt"))
      {
        using (var strReader = new StreamReader(stream))
        {
          parameterObjectTop = strReader.ReadToEnd();
        }
      }

      using (var stream = System.IO.File.OpenRead("./ParameterObjectBottom.txt"))
      {
        using (var strReader = new StreamReader(stream))
        {
          parameterObjectBottom = strReader.ReadToEnd();
        }
      }
    }


    protected string CreateParameterObjectSource(List<string> parameterList, string usage = "")
    {
      string result;

      //
      // Create the import list,
      // the namespace ant the 
      // beginning of the class 
      // definition
      //
      result = string.Empty;
      result += parameterObjectTop;

      //
      // Add the property definitions
      // according to the 'parameterList'.
      //
      result += "\r\n\r\n";
      result += String.Join("\r\n", parameterList);
      result += "\r\n\r\n";

      //
      // Add the constructor. 
      // Close the class definition
      // and close the namespace.
      //
      result += parameterObjectBottom;

      //
      // If a 'usage' string was provided
      // replace the "UsageAttribute' comment
      // with the 'UsageAttribute".
      //
      if (!String.IsNullOrWhiteSpace(usage))
      {
        result = result.Replace("//UsageAttribute", "[Usage(\"" + usage + "\")]");
      }
      return result;
    }


    protected void ResetTestCommand()
    {
      string testCommandSource;
      string testCommandSourcePath;
      string testCommandTargetPath;


      testCommandSourcePath = Path.Combine(testCommandProjectDir, testCommand + ".cs.txt");
      testCommandTargetPath = testCommandSourcePath.Replace(".txt","");

      using (var stream = System.IO.File.OpenRead(testCommandSourcePath))
      {
        using (var strReader = new StreamReader(stream))
        {
          testCommandSource = strReader.ReadToEnd();
        }
      }

      System.IO.File.Delete(testCommandTargetPath);

      using (var stream = System.IO.File.OpenWrite(testCommandTargetPath))
      {
        using (var strWriter = new StreamWriter(stream))
        {
          strWriter.Write(testCommandSource);
          stream.Flush();
          strWriter.Close();
        }
      }

    }

    protected void SetTestCommandShowUsageOnEmptyArgs(bool showUsageOnEmptyArgs)
    {
      string testCommandSource;
      List<string> testCommandSourceLines;
      string processLine;
      int processLineIndex;
      string testCommandSourcePath;
      string testCommandTargetPath;


      testCommandSourcePath = Path.Combine(testCommandProjectDir, testCommand + ".cs.txt");
      testCommandTargetPath = testCommandSourcePath.Replace(".txt","");

      using (var stream = System.IO.File.OpenRead(testCommandSourcePath))
      {
        using (var strReader = new StreamReader(stream))
        {
          testCommandSource = strReader.ReadToEnd();
        }
      }

      testCommandSource = testCommandSource.Replace("\r","");
      testCommandSourceLines = testCommandSource.Split("\n", StringSplitOptions.None).ToList();

      processLineIndex = testCommandSourceLines.FindIndex(0, (line) =>  
                         { 
                           return line.IndexOf("IsValid = parameterObject.Process") > -1;
                         });

      processLine = testCommandSourceLines[processLineIndex];

      if(processLine != null)
      {
        processLine = $"      IsValid = parameterObject.Process(args, showUsageOnEmptyArgs : {showUsageOnEmptyArgs.ToString().ToLower()});";
        testCommandSourceLines[processLineIndex] = processLine;
      }

      testCommandSource = String.Join("\r\n",testCommandSourceLines);

      System.IO.File.Delete(testCommandTargetPath);

      using (var stream = System.IO.File.OpenWrite(testCommandTargetPath))
      {
        using (var strWriter = new StreamWriter(stream))
        {
          strWriter.Write(testCommandSource);
          stream.Flush();
          strWriter.Close();
        }
      }
    }


    protected bool BuildTarget()
    {
      ProcessExecute processExecute = new ProcessExecute();

      if(IsWindows)
      {
        processExecute.Start(testCommandProjectDir, "cmd", new string[] { "/C", "dotnet", "build", "--force" });
      }

      if(IsLinux)
      {
        processExecute.Start(testCommandProjectDir, "dotnet", new string[] {  "build", "--force" });
      }

      return processExecute.ProcessExitCode == 0;
    }


    protected bool PublishTarget()
    {
      ProcessExecute processExecute = new ProcessExecute();

      if(IsWindows)
      {
        processExecute.Start(testCommandProjectDir, "cmd", new string[] { "/C", "dotnet", "publish", "--force" });
      }

      if(IsLinux)
      {
        processExecute.Start(testCommandProjectDir, "dotnet", new string[] { "publish", "--force" });
      }

      return processExecute.ProcessExitCode == 0;
    }


  }// END class
}// END namespace
