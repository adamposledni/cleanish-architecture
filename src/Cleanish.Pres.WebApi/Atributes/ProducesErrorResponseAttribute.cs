using Microsoft.AspNetCore.Mvc;
using Cleanish.Pres.WebApi.Models.Common;

namespace Cleanish.Pres.WebApi.Atributes;

internal class ProducesErrorResponseAttribute : ProducesResponseTypeAttribute
{
    public ProducesErrorResponseAttribute(int errorStatus) : base(typeof(ErrorRes), errorStatus)
    {
    }
}
