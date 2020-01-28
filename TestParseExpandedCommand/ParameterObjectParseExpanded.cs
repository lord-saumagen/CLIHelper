

using System;
using System.Reflection;
using CommandLineHelper;
using System.Linq;

namespace TestCommand
{
  //UsageAttribute
  public class ParameterObjectParseExpanded : ParameterBase
  {

    [Mandatory]
    [Name("delivery-date")]
    public DateTime? DeliveryDate { get; set; }


    public ParameterObjectParseExpanded(string commandName, Assembly commandAssembly): base(commandName, commandAssembly, new DisplayHelper())
    {
    }

    public override void Parse(string[] args)
    {
      base.Parse(args);

      if(this.IsHelpRequest | this.IsVersionRequest)
      {
         return;
      }

      var metaInfo = this.PropertyMetaInfoList.Where(item => item.Name == "delivery-date").Single();

      if(metaInfo.ParseResult == ParseResultEnum.NOT_PARSED)
      {
         return;
      }

      var value = this.Arguments.Where(arg => arg.Key == metaInfo.Name).Single().Value;
      
      try
      {
        this.DeliveryDate = DateTime.Parse(value);
        metaInfo.ParseResult = ParseResultEnum.PARSE_SUCCEEDED;
      }
      catch
      {
      }
    }

  }// END class
}// END namespace



