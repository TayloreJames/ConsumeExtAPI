using System;
using System.Linq;
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

        public bool IsPointInPolygon(CoordinatePoint p, RingPolygons polygon)
        {
            double minX = polygon.CoordinateSet.Select(_=>_.Coordinates.Min(x=>x.Longitude)).First();
            double maxX = polygon.CoordinateSet.Select(_ => _.Coordinates.Max(x => x.Longitude)).First();
            double minY = polygon.CoordinateSet.Select(_ => _.Coordinates.Min(x => x.Latitude)).First();
            double maxY = polygon.CoordinateSet.Select(_ => _.Coordinates.Max(x => x.Latitude)).First();
            for (int i = 1; i < polygon.CoordinateSet.Count; i++)
            {
                var q = polygon.CoordinateSet.ElementAt(i);

                for (int k = 0; k < q.Coordinates.Count; k++)
                {
                    minX = Math.Min(q.Coordinates.ElementAt(k).Longitude, minX);
                    maxX = Math.Max(q.Coordinates.ElementAt(k).Longitude, maxX);
                    minY = Math.Min(q.Coordinates.ElementAt(k).Latitude, minY);
                    maxY = Math.Max(q.Coordinates.ElementAt(k).Latitude, maxY);
                }

                
            }

            if (p.Longitude < minX || p.Longitude > maxX || p.Latitude < minY || p.Latitude > maxY)
            {
                return false;
            }

            // https://wrf.ecse.rpi.edu/Research/Short_Notes/pnpoly.html
            bool inside = false;
            for (int k = 0; k < polygon.CoordinateSet.Count; k++)
                
            {
                var q = polygon.CoordinateSet.ElementAt(k);
                for (int i = 0, j = q.Coordinates.Count - 1; i < q.Coordinates.Count; j = i++)
                {

                    if ((q.Coordinates.ElementAt(i).Latitude > p.Latitude) != (q.Coordinates.ElementAt(j).Latitude > p.Latitude) &&
                         p.Longitude < (q.Coordinates.ElementAt(j).Longitude - q.Coordinates.ElementAt(i).Longitude) * (p.Latitude - q.Coordinates.ElementAt(i).Latitude) / (q.Coordinates.ElementAt(j).Latitude - q.Coordinates.ElementAt(i).Latitude) + q.Coordinates.ElementAt(i).Longitude);
                    {
                        inside = !inside;
                    }
                }
            }

            return inside;
        }

    }

    public class CombinedAPIObject
    {
        public MUAPObject MUAPObject { get; set; }
        public GeocodingObject GeocodingObject { get; set; }
    }
    
}
