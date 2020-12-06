using System;
using System.Threading.Tasks;

namespace ConsumingAPI_Final.Service
{
    public interface ICombinedAPIService
    {
        Task<CombinedAPIObject> GetCombinedObject(string address);
    }

    public class CombinedAPIService : ICombinedAPIService
    {
        private IMUAPService _muapService;
        private IGeocodingService _geocodingService;
        private const string _neighborhoods = "1%3D1";

        public CombinedAPIService(IMUAPService muapService, IGeocodingService geocodingService)
        {
            _muapService = muapService;
            _geocodingService = geocodingService;            
        }

        public async Task<CombinedAPIObject> GetCombinedObject(string address)
        {
            var muapResults = await _muapService.GetMUAPResults(_neighborhoods);
            var geocodingResults = await  _geocodingService.GetGeocodingResults(address);
            foreach (var feature in muapResults.Features)
            {
                feature.Geometry.FlattenRings();
            }
           
            var combinedAPIObject = new CombinedAPIObject() { MUAPObject = muapResults, GeocodingObject = geocodingResults };
            return combinedAPIObject;
        }
       
    }

    public class CombinedAPIObject
    {
        public MUAPObject MUAPObject { get; set; }
        public GeocodingObject GeocodingObject { get; set; }
    }
    
}
