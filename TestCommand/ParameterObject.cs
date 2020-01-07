using System;
using System.Reflection;
using CommandLineHelper;

namespace TestCommand
{
  //UsageAttribute
  public class ParameterObject : ParameterBase
  {

[Mandatory]
[Name("number")]
public Int32? Int32Number { get; set; }


    public ParameterObject(string commandName, Assembly commandAssembly): base(commandName, commandAssembly, new DisplayHelper())
    {
      
    }
  }// END class
}// END namespace