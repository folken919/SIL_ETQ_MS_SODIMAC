using ApesPrinterTools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Sodimac.Cedis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        #region instanciar controlador
        IConfiguration configuration;
        public TestController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        #endregion

        #region metodos 
        [HttpGet("test")]
        public async Task<IActionResult> test()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"Ultima entrega para ambiente {this.configuration["environment"]} : {this.configuration["lastEdition"]}");
            result.AppendLine($"Responsable del desarrollo: {this.configuration["developer"]}");
            return Ok(result.ToString());
        }

        [HttpGet("impresion")]
        public IActionResult ValidarImpresion(string impresora)
        {
            try
            {
                PrinterTools printer;
                printer = new PrinterTools(impresora);
                return Ok(printer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }                 
        }

        #endregion
    }
}
