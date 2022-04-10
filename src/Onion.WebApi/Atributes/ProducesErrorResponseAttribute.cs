using Microsoft.AspNetCore.Mvc;
using Onion.WebApi.Models;

namespace Onion.WebApi.Atributes;

public class ProducesErrorResponseAttribute : ProducesResponseTypeAttribute
{
    public ProducesErrorResponseAttribute(int errorStatus) : base(typeof(ErrorRes), errorStatus)
    {
    }
}
