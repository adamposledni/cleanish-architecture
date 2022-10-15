using Microsoft.AspNetCore.Mvc;
using Onion.Pres.WebApi.Models.Common;

namespace Onion.Pres.WebApi.Atributes;

public class ProducesErrorResponseAttribute : ProducesResponseTypeAttribute
{
    public ProducesErrorResponseAttribute(int errorStatus) : base(typeof(ErrorRes), errorStatus)
    {
    }
}
