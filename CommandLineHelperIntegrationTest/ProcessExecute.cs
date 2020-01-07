using System;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CommandLineHelperIntegrationTest
{
  public class ProcessExecute
  {
    public int ProcessExitCode
    {
      get;
      private set;
    } = 0;

    public string ProcessStandardOutput
    {
      get;
      private set;
    } = String.Empty;

    public string ProcessErrorOutput
    {
      get;
      private set;
    } = String.Empty;

    public void Start(string executionPath, string command, string[] args)
    {
      Process process;
      MethodBase methodBase;
      string currentPath;

      methodBase = MethodBase.GetCurrentMethod();

      if (String.IsNullOrWhiteSpace(executionPath))
      {
        throw new System.ArgumentException($"Argument '{executionPath}' must be a valid path in function '{methodBase.ReflectedType.Name}.{methodBase.Name}'.");
      }

      if (!Directory.Exists(executionPath))
      {
        throw new System.ArgumentException($"Argument '{executionPath}' must be a valid path in function '{methodBase.ReflectedType.Name}.{methodBase.Name}'.");
      }

      if (!Directory.Exists(executionPath))
      {
        throw new System.ArgumentException($"Argument '{command}' must be a valid command in function '{methodBase.ReflectedType.Name}.{methodBase.Name}'.");
      }

      currentPath = Directory.GetCurrentDirectory();
      Directory.SetCurrentDirectory(executionPath);
      process = new Process();
      process.StartInfo.FileName = command;

      foreach(string arg in args)
      {
        process.StartInfo.ArgumentList.Add(arg);
      }
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = false;
      process.StartInfo.RedirectStandardOutput = true;
      process.StartInfo.RedirectStandardError = true;
      process.EnableRaisingEvents = true; 
      process.Exited += new EventHandler(ProcessExitedEventHandler);
      process.Start();
      process.WaitForExit();

      Directory.SetCurrentDirectory(currentPath);
    }

    public void ProcessExitedEventHandler(object sender, EventArgs eventArgs)
    {
      Process process;

      process = (Process)sender;
      process.StandardOutput.BaseStream.Flush();
      this.ProcessStandardOutput = process.StandardOutput.ReadToEnd();
      process.StandardError.BaseStream.Flush();
      this.ProcessErrorOutput = process.StandardError.ReadToEnd();
      this.ProcessExitCode = process.ExitCode;
    }
  }

}