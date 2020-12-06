using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumingAPI_Final.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConsumingAPI_Final.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CombinedAPIController : Controller
    {
        private readonly ICombinedAPIService _combinedAPIService;
        

        public CombinedAPIController(ICombinedAPIService combinedAPIService)
        {
            _combinedAPIService = combinedAPIService;
        }
        // GET: api/CombinedAPI/query       
        [HttpGet("{address}")]
        public async Task<ActionResult<CombinedAPIObject>> GetCombinedReponse(string address)
        {
            
            var result = await _combinedAPIService.GetCombinedObject(address);

            return result;
            
        }
    }
}
