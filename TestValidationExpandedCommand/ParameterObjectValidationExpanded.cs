

using System;
using System.Reflection;
using CommandLineHelper;
using System.Linq;

namespace TestCommand
{
  //UsageAttribute
  public class ParameterObjectValidationExpanded : ParameterBase
  {

    [Mandatory]
    [Name("delivery-date")]
    public DateTime? DeliveryDate { get; set; }

    [Mandatory]
    [Name("Email")]
    [Email("The value of argument 'email' must be a valid email address.")]
    public string? EmailAddress {get; set; }

    public ParameterObjectValidationExpanded(string commandName, Assembly commandAssembly): base(commandName, commandAssembly, new DisplayHelper())
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


    public override bool Validate()
    {
      base.Validate();

      //
      // It's a help request. No need to validate.
      //
      if (this.IsHelpRequest)
      {
        return false;
      }

      //
      // It's a version request. No need to validate.
      //
      if (this.IsVersionRequest)
      {
        return false;
      }

      var deliveryDateMetaInfo = this.PropertyMetaInfoList.Where(metaInfo => metaInfo.Name == "delivery-date").Single();

      if(!deliveryDateMetaInfo.IsValid)
      {
        //
        // Another validator already marked the 
        // delivery-date as invalid. There is
        // nothing more to do.
        //
        return false;
      }

      if(deliveryDateMetaInfo.ParseResult != ParseResultEnum.PARSE_SUCCEEDED)
      {
        //
        // Nothing to do
        //
        return false;
      }


      //
      // Validate the delivery date. It must be 
      // a date at least 2 days in the future.
      //
      if(this.DeliveryDate < DateTime.Now.AddDays(2))
      {
        var parsedValue = this.Arguments.Where(item => item.Key == "delivery-date").Single().Value;
        deliveryDateMetaInfo.ValidationError = new ValidationError(deliveryDateMetaInfo, parsedValue, "The delivery-date must be a date at least 2 days in the future.");
      }
      else
      {
        deliveryDateMetaInfo.ValidationError = null;
      }

      return base.IsValid;
    }
  }// END class
}// END namespace



