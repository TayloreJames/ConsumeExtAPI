using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsumingAPI_Final.Service
{
    public interface IMUAPService
    {
        Task<MUAPResponse> GetMUAPResults(string query);
    }

    public class MUAPService : IMUAPService
    {

        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _options;


        public MUAPService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        public async Task<MUAPResponse> GetMUAPResults(string query)
        {
            //https: //services2.arcgis.com/qvkbeam7Wirps6zC/arcgis/rest/services/Medically_Underserved_Areas_Population/FeatureServer/0/query?where=ObjectId=1&outFields=muap_index,srvc_area,ObjectId&returnGeometry=true&orderByFields=muap_index&outSR=4326&f=json
            var response = await _httpClient.GetAsync($"query?where=ObjectId={query}&outFields=muap_index,srvc_area,ObjectId&returnGeometry=true&orderByFields=muap_index&outSR=4326&f=json");

            var jsonString = await response.Content.ReadAsStringAsync();
            //await response.Content.ReadAsAsync<SearchlyResponse>();

            var muapResponse = JsonSerializer.Deserialize<MUAPResponse>(jsonString, _options);

            return muapResponse;
        }
    }

    public class MUAPResponse
    {
        public string ObjectIdFieldName { get; set; }
        public UniqueIdField UniqueIdField { get; set; }
        public string? GlobalIdFieldName { get; set; }
        public GeometryProperties GeometryProperties { get; set; }
        public string GeometryType { get; set; }
        public SpatialReference SpatialReference { get; set; }
        public List<OutFields> Fields { get; set; }
        public List<FeatureSet> Features { get; set; }
        
    }

    public class UniqueIdField
    {
        public string Name { get; set; }
        public bool IsSystemMaintained { get; set; }
    }

    public class GeometryProperties
    {
        public string ShapeAreaFieldName { get; set; }
        public string ShapeLengthFieldName { get; set; }
        public string Units { get; set; }
    }
    public class SpatialReference
    {
        public int Wkid { get; set; }
        public int LatestWkid { get; set; }
    }

    public class OutFields
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Alias { get; set; }
        public string SQLType { get; set; }
        public string? Domain { get; set; }
        public string? DefaultValue { get; set; }
    }

    public class FeatureSet
    {
        public FeatureAttributes Attributes { get; set; }
        public FeatureGeometry Geometry { get; set; }
    }

    public class FeatureAttributes
    {
        public decimal MuapIndex { get; set; }
        public string ServiceArea { get; set; }
        public int ObjectId { get; set; }
    }

    public class FeatureGeometry
    {
        public List<RingPolygons> Rings { get; set; } 
    }

    public class RingPolygons
    {
        public List<CoordinatePair> Coordinates { get; set; }
    }

    public class CoordinatePair
    {
        public decimal[,] LongAndLat { get; set; }
    }
  
}
