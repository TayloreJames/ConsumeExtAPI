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
    public class MUAPController : Controller
    {
        private readonly IMUAPService _muapService;

        public MUAPController(IMUAPService muapService)
        {
            _muapService = muapService;
        }


        // GET: api/MUAP/query
        [HttpGet("{query}")]
        public async Task<ActionResult<List<FeatureGeometry>>> GetObjectGeometry(string query)
        {
            var geometryPolygons = new List<FeatureGeometry>();
            var result = await _muapService.GetMUAPResults(query);

            foreach (var response in result.Features)
            {
                geometryPolygons.Add(new FeatureGeometry() { Rings = response.Geometry.Rings }); ;
            };

            return geometryPolygons;
        }



        //    public IActionResult Index()
        //{
        //    return View();
        //}
    }
    //public class FeatureGeometry
    //{
    //    public List<RingPolygons> Rings { get; set; }
    //}

    //public class RingPolygons
    //{
    //    public List<GeometryPoint> Coordinates { get; set; }
    //}

    //public class GeometryPoint
    //{
    //    public decimal[,] LongAndLat { get; set; }
    //}
}
