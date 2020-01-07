using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandLineHelper;
using System.Reflection;

namespace CommandLineHelper
{
  class ParameterDoubleOccurrence : CommandLineHelper.ParameterBase
  {

    public string StringParam
    {
      get;
      set;
    }

    [Internal]
    public int IntParam
    {
      get;
      set;
    }

    public ParameterDoubleOccurrence(string commandName, Assembly commandAssembly) : base(commandName,  commandAssembly, new DisplayHelper())
    { }

  }// END class

  [TestClass]
  [TestCategory("ParameterDoubleOccurrenceTest")]
  public class ParameterDoubleOccurrenceTest
  {
    [TestMethod]
    public void ParameterDoubleOccurrenceTest_ConstructorTest()
    {
      ParameterDoubleOccurrence parameterDoubleOccurrence = new ParameterDoubleOccurrence("ParameterDoubleOccurrenceTest", Assembly.GetExecutingAssembly());
      Assert.IsNotNull(parameterDoubleOccurrence, "The 'ParameterDoubleOccurrence' constructor should return a valid instance.");
    }

    [TestMethod]
    public void ParameterDoubleOccurrenceTest_ValidationTest()
    {
      string[] args;
      bool validationResult;
      ParameterDoubleOccurrence parameterDoubleOccurrence = new ParameterDoubleOccurrence("ParameterDoubleOccurrenceTest", Assembly.GetExecutingAssembly());

      args = new string[] {"StringParam=First string", "IntParam=42", "stringparam=Second string"};

      parameterDoubleOccurrence.Parse(args);
      validationResult = parameterDoubleOccurrence.Validate();
      Assert.IsTrue(validationResult, "The validation of 'ParameterDoubleOccurrence' should pass.");
      Assert.AreEqual("First string", parameterDoubleOccurrence.StringParam, "The value of parameter object 'StringParam' property should be from the first argument.");
    }

  }// END class
}// END namespace