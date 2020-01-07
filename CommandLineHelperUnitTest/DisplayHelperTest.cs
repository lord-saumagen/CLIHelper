using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using CommandLineHelper;

namespace CommandLineHelper
{
  class TestCommand
  {
    public TestCommand()
    {

    }
  }

  [Usage("register_pet [help] [version] name=<pets name> age=<pets age in years> [gender=<'undefined'|'male'|'female'>]")]
  class ParameterDisplayHelper : CommandLineHelper.ParameterBase
  {
    [Name("name")]
    [Mandatory]
    [Description("The name of the pet you want to register.")]
    public string Name
    {
      get;
      set;
    }

    [Name("age")]
    [Mandatory]
    [Description("The age of the pet you want to register in years.")]
    public UInt32 Age
    {
      get;
      set;
    }

    [Name("gender")]
    [ValueSet(new object[] { "undefined", "male", "female" })]
    [DefaultValue("undefined")]
    [Description("The gender of the pet you want to register. Leave the default value 'undefined' if you don't know yet. The other valid values are: 'male' or 'female'.")]
    public string Gender
    {
      get;
      set;
    }


    public ParameterDisplayHelper(string command, Assembly commandAssembly) : base(command, commandAssembly, new DisplayHelper())
    { }
  }


  [TestClass]
  [TestCategory("DisplayHelperTest")]
  public class DisplayHelperTest
  {

    //
    // private static List<string> BreakLineAtMaxLength(string line, int maxLength, bool trimLines = true)
    //
    [TestMethod]
    public void DisplayHelper_BreakLineAtMaxLengthGracefullyTest()
    {
      DisplayHelper displayHelper;
      string testString;
      MethodInfo BreakLineAtMaxLengthMethodInfo;
      List<string> result;

      testString = "01234567890123456789\r\n0123456789 0123456789,0123456789;0123456789!012345679?0123456789 123456789";
      displayHelper = new DisplayHelper();
      BreakLineAtMaxLengthMethodInfo = displayHelper.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
      .Where(method => method.Name == "BreakLineAtMaxLength" && method.IsPrivate)
      .First();
      result = (List<string>)BreakLineAtMaxLengthMethodInfo.Invoke(displayHelper, new object[] { testString, 40, true });
      Assert.IsNotNull(result, "The 'BreakLineAtMaxLength' function should return a result.");
      Assert.AreEqual("01234567890123456789\r\n0123456789", result[0], "The lines should match the expectations.");
      Assert.AreEqual("0123456789,0123456789;0123456789!", result[1], "The lines should match the expectations.");
      Assert.AreEqual("012345679?0123456789 123456789", result[2], "The lines should match the expectations.");

      Debugger.Log(1, "BreakLineAtMaxLength", string.Join('\n', result) + "\r\n");
    }


    //
    // private static List<String> BreakLineAtLineBreak(string line, bool trimLines = true)
    // 
    [TestMethod]
    public void DisplayHelper_BreakLineAtLineBreak()
    {
      DisplayHelper displayHelper;
      string testString;
      MethodInfo BreakLineAtLineBreakMethodInfo;
      List<string> result;

      testString = "01234567890123456789\r\n0123456789 0123456789,0123456789\r0123456789!012345679?0123456789 \n123456789";
      displayHelper = new DisplayHelper();
      BreakLineAtLineBreakMethodInfo = displayHelper.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
      .Where(method => method.Name == "BreakLineAtLineBreak" && method.IsPrivate)
      .First();
      result = (List<string>)BreakLineAtLineBreakMethodInfo.Invoke(displayHelper, new object[] { testString, true });
      Assert.IsNotNull(result, "The function should return a result.");
      Assert.AreEqual(4, result.Count, "The result should have four entries.");
      Assert.AreEqual("01234567890123456789", result[0], "The lines should match with the expectation.");
      Assert.AreEqual("0123456789 0123456789,0123456789", result[1], "The lines should match with the expectation.");
      Assert.AreEqual("0123456789!012345679?0123456789", result[2], "The lines should match with the expectation.");
      Assert.AreEqual("123456789", result[3], "The lines should match with the expectation.");

      Debugger.Log(1, "BreakLineAtLineBreak", string.Join('\n', result) + "\r\n");
    }


    //
    // private static List<string> CreateWrappedLines(string line, int maxLineLength, int leadingSpaces = 1)
    // 
    [TestMethod]
    public void DisplayHelper_CreateWrappedLinesTest()
    {
      DisplayHelper displayHelper;
      string testString;
      MethodInfo CreateWrappedLinesMethodInfo;
      List<string> result;

      int MaxWidth = 40;
      testString = string.Empty;
      testString += "01234567890123456789  \r\n";  // break at line break
      testString += " 0123456789012345678901234567890123456789012345\r"; // break at MaxWidth after trim, break at line break
      testString += " 67890123456789 012345678901234567890123456789 \n"; // break at MaxWidth after trim, break at line break
      testString += " 0123456789012345678901234567!890123 4567890123456789"; // break at space
      testString += "01234567890\r "; // break at line break
      testString += "123456789.012345678901234567890123456789012345678901234567890123456789"; // break at ., break at MaxWidth

      //
      // The expected result:
      //
      // 01234567890123456789 -> [SPACE] - 40
      // 0123456789012345678901234567890123456789
      // 67890123456789 0123456789012345678901234
      // 56789 -> [SPACE] - 40
      // 0123456789012345678901234567!890123 -> [SPACE] - 40
      // 4567890123456789 -> [SPACE] - 40
      // 01234567890 -> [SPACE] - 40
      // 123456789. -> [SPACE] - 40
      // 0123456789012345678901234567890123456789
      // 01234567890123456789 -> [SPACE] - 40
      //
      // 10 lines total. Each line with a length of 40 characters


      displayHelper = new DisplayHelper();
      CreateWrappedLinesMethodInfo = displayHelper.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
      .Where(method => method.Name == "CreateWrappedLines" && method.IsPrivate)
      .First();
      result = (List<String>)CreateWrappedLinesMethodInfo.Invoke(displayHelper, new object[] { testString, MaxWidth, 0 });
      Assert.IsNotNull(result);
      Assert.AreEqual(10, result.Count, "The function should return 10 lines as result.");
      foreach (var line in result)
      {
        Assert.AreEqual(40, line.Length, "Each result line should have the same length.");
      }
      Assert.AreEqual("01234567890123456789", result[0].Trim(), "The lines should match with the expectation.");
      Assert.AreEqual("0123456789012345678901234567890123456789", result[1].Trim(), "The lines should match with the expectation.");
      Assert.AreEqual("012345", result[2].Trim(), "The lines should match with the expectation.");
      Assert.AreEqual("0123456789012345678901234567!890123", result[5].Trim(), "The lines should match with the expectation.");
      Assert.AreEqual("123456789.", result[7].Trim(), "The lines should match with the expectation.");
      Assert.AreEqual("0123456789012345678901234567890123456789", result[8].Trim(), "The lines should match with the expectation.");

      Debugger.Log(1, "CreateWrappedDescription", string.Join('\n', result) + "\r\n");
    }


    //
    // private static string CreateKeyDescription(int screenWidth)
    //
    [TestMethod]
    public void DisplayHelper_CreateKeyDescriptionTest()
    {
      DisplayHelper displayHelper;
      MethodInfo CreateKeyDescriptionMethodInfo;
      string result;

      displayHelper = new DisplayHelper();
      CreateKeyDescriptionMethodInfo = displayHelper.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
      .Where(method => method.Name == "CreateKeyDescription" && method.IsPrivate)
      .First();
      result = (string)CreateKeyDescriptionMethodInfo.Invoke(displayHelper, new object[] { 80 });
      Assert.IsNotNull(result);
      StringAssert.Contains(result, "╔═╦════════════════════════════════════════════════════════════════════════════╗", "The returned string should show the expected message.");
      StringAssert.Contains(result, "║+║ Argument is mandatory.                                                     ║", "The returned string should show the expected message.");
      StringAssert.Contains(result, "║-║ Argument is optional.                                                      ║", "The returned string should show the expected message.");
      StringAssert.Contains(result, "╚═╩════════════════════════════════════════════════════════════════════════════╝", "The returned string should show the expected message.");

      Debugger.Log(1, "CreateKeyDescription", result);
    }


    //
    // private static string CreateTop(int screenWidth, int longestArgumentLength, int longestTypeLength, int longesDefaultLength)
    //
    [TestMethod]
    public void DisplayHelper_CreateHelpTopTest()
    {
      DisplayHelper displayHelper;
      MethodInfo CreateTopMethodInfo;
      string result;

      displayHelper = new DisplayHelper();
      CreateTopMethodInfo = displayHelper.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
      .Where(method => method.Name == "CreateHelpTop" && method.IsPrivate)
      .First();
      result = (string)CreateTopMethodInfo.Invoke(displayHelper, new object[] { 80, 15, 10, 10 });
      Assert.IsNotNull(result);
      //
      // Two characters longer than the required width
      // because of the trailing line break '\r\n'.
      //
      Assert.AreEqual(82, result.Length);
      StringAssert.Contains(result, "╔═╦═[Parameter]═════╦═[Type]═════╦═[Default]══╦═[Description]══════════════════╗", "The returned string should show the expected message.");

      Debugger.Log(1, "CreateHelpTop", result);
    }

    //
    // private static string CreateSeparatorLine(int screenWidth, int longestArgumentLength, int longestTypeLength, int longestDefaultLength)
    //
    [TestMethod]
    public void DisplayHelper_CreateHelpSeparatorLineTest()
    {
      DisplayHelper displayHelper;
      MethodInfo CreateBottomMethodInfo;
      string result;

      displayHelper = new DisplayHelper();
      CreateBottomMethodInfo = displayHelper.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
      .Where(method => method.Name == "CreateHelpSeparatorLine" && method.IsPrivate)
      .First();
      result = (string)CreateBottomMethodInfo.Invoke(displayHelper, new object[] { 80, 15, 10, 10 });
      Assert.IsNotNull(result);
      //
      // Two characters longer than the required width
      // because of the trailing line break '\r\n'.
      //
      Assert.AreEqual(82, result.Length);
      StringAssert.Contains(result, "╠═╬═════════════════╬════════════╬════════════╬════════════════════════════════╣", "The returned string should show the expected message.");

      Debugger.Log(1, "CreateHelpSeparatorLine", result);
    }


    //
    // public static string CreateBottom(int screenWidth, int longestArgumentLength, int longestTypeLength, int longesDefaultLength)
    //
    [TestMethod]
    public void DisplayHelper_CreateHelpBottomTest()
    {
      DisplayHelper displayHelper;
      MethodInfo CreateBottomMethodInfo;
      string result;

      displayHelper = new DisplayHelper();
      CreateBottomMethodInfo = displayHelper.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
      .Where(method => method.Name == "CreateHelpBottom" && method.IsPrivate)
      .First();
      result = (string)CreateBottomMethodInfo.Invoke(displayHelper, new object[] { 80, 15, 10, 10 });
      Assert.IsNotNull(result);
      //
      // Two characters longer than the required width
      // because of the trailing line break '\r\n'.
      //
      Assert.AreEqual(82, result.Length);
      StringAssert.Contains(result, "╚═╩═════════════════╩════════════╩════════════╩════════════════════════════════╝", "The returned string should show the expected message.");
      Debugger.Log(1, "CreateHelpBottom", result);
    }

    [TestMethod]
    public void DisplayHelper_CreateHelpTest()
    {
      ParameterDisplayHelper parameterDisplayHelper;
      string result;

      parameterDisplayHelper = new ParameterDisplayHelper("register_pet", Assembly.GetExecutingAssembly());
      result = parameterDisplayHelper.CreateHelp();
      Assert.IsTrue(!String.IsNullOrWhiteSpace(result));
      StringAssert.Contains(result, "╔═╦═[Parameter]═╦═[Type]═╦═[Default]═╦═[Description]═══════════════════════════╗", "The returned string should show the expected message.");
      StringAssert.Contains(result, "║+║ name        ║ String ║           ║ The name of the pet you want to         ║", "The returned string should show the expected message.");
      StringAssert.Contains(result, "║+║ age         ║ UInt32 ║ 0         ║ The age of the pet you want to          ║", "The returned string should show the expected message.");
      StringAssert.Contains(result, "║-║ gender      ║ String ║ undefined ║ The gender of the pet you want to       ║", "The returned string should show the expected message.");
      StringAssert.Contains(result, "register_pet [help] [version] name=<pets name> age=<pets age in years> [gender=<'undefined'|'male'|'female'>]", "The returned string should show the expected message.");

      Debugger.Log(1, "CreateHelp", result);
    }

    [TestMethod]
    public void DisplayHelper_CreateVersionTest()
    {
      DisplayHelper displayHelper;
      string result;

      displayHelper = new DisplayHelper();
      //
      // Create version uses a dummy command which is defined in this help project.
      // For that reason the version string should reflect the version as specified
      // in the *.csproj file of this project. (Currently '1.2.3.4")
      //
      result = displayHelper.CreateVersion(new TestCommand());
      Assert.IsTrue(!String.IsNullOrEmpty(result));
      Assert.AreEqual("1.2.3.4", result, "The result string should match the expectation.");

      Debugger.Log(1, "CreateVersion", result);
    }

    [TestMethod]
    public void DisplayHelper_CreateValidationSummaryTest()
    {
      string[] args;
      ParameterDisplayHelper parameterDisplayHelper;
      string result;
      bool validationPassed;

      parameterDisplayHelper = new ParameterDisplayHelper("register_pet", Assembly.GetExecutingAssembly());
      args = new string[] { "age=-3", "gender=null" };
      parameterDisplayHelper.Parse(args);
      validationPassed = parameterDisplayHelper.Validate();
      result = parameterDisplayHelper.CreateValidationSummary();
      Assert.IsFalse(validationPassed);
      Assert.IsTrue(!String.IsNullOrEmpty(result));
      StringAssert.Contains(result, "║ One ore more of the command line arguments are invalid.                      ║", "The returned string should show the expected message.");

      Debugger.Log(1, "CreateValidationSummary", "VALIDATION SUMMARY WITH STD MESSAGE\r\n\r\n");
      Debugger.Log(1, "CreateValidationSummary", result + "\r\n\r\n");

      result = parameterDisplayHelper.CreateValidationSummary("This is a very large message which should break at the maximum line length and continue on the next line.");
      StringAssert.Contains(result, "║ This is a very large message which should break at the maximum line length   ║", "The returned string should show the expected message.");
      StringAssert.Contains(result, "║ and continue on the next line.                                               ║", "The returned string should show the expected message.");

      Debugger.Log(1, "CreateValidationSummary", "VALIDATION SUMMARY  WITH CUSTOM MESSAGE\r\n\r\n");
      Debugger.Log(1, "CreateValidationSummary", result + "\r\n\r\n");
    }

    //[ExpectedException(typeof(System.ArgumentException), "The expected argument exception wasn't raised.")]
    [TestMethod]
    public void DisplayHelper_CreateValidationSummaryFailTest()
    {
      string[] args;
      ParameterDisplayHelper parameterDisplayHelper;

      parameterDisplayHelper = new ParameterDisplayHelper("register_pet", Assembly.GetExecutingAssembly());
      args = new string[] { "age=-3", "gender=null" };
      parameterDisplayHelper.Parse(args);
      parameterDisplayHelper.Validate();
      //
      // Calling 'CreateValidationSummary' with an empty message should raise an exception
      //
      Assert.ThrowsException<System.ArgumentException>( () => { parameterDisplayHelper.CreateValidationSummary("");});
    }

  }// END class

}// END namespace