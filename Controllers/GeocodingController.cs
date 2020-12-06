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
    public class GeocodingController : Controller
    {
        private readonly IGeocodingService _geocodingService;

        public GeocodingController(IGeocodingService geocodingService)
        {
            _geocodingService = geocodingService;
        }
        // GET: api/Geocoding/query       
        [HttpGet("{address}")]
        public async Task<ActionResult<List<LatAndLong>>> GetGeocodingReponse(string address)
        {
            var latAndLong = new List<LatAndLong>();
            var result = await _geocodingService.GetGeocodingResults(address);

            foreach (var response in result.Results)
            {
                latAndLong.Add(new LatAndLong()
                {
                    Latitude = response.Geometry.Location.Lat,
                    Longitude = response.Geometry.Location.Lng
                });
            }

            return latAndLong;
        }
    }
    public class LatAndLong
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
