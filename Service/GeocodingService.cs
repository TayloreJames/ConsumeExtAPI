using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsumingAPI_Final.Service
{
    public interface IGeocodingService
    {
        Task<GeocodingObject> GetGeocodingResults(string address);
    }

    public class GeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private JsonSerializerOptions _options;
        private const string _key = "AIzaSyAnFtWq1VKpckVtV3i665mgdfzf20m6y6g";
        //private string 

        public GeocodingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        public async Task<GeocodingObject> GetGeocodingResults(string address)
        {
            //https: //maps.googleapis.com/maps/api/geocode/json?address=1600+Amphitheatre+Parkway,+Mountain+View,+CA&key=AIzaSyAnFtWq1VKpckVtV3i665mgdfzf20m6y6&libraries=geometry            
            var response = await _httpClient.GetAsync($"json?address={address}&key={_key}&libraries=geometry");

            var jsonString = await response.Content.ReadAsStringAsync();


            var geocodingResponse = JsonSerializer.Deserialize<GeocodingObject>(jsonString, _options);

            return geocodingResponse;
        }
    }

    public class GeocodingObject
    {
        public Results[] Results { get; set; }
        public string Status { get; set; }
    }

    public class Results
    {
        public AddressComponent[] Address_Components { get; set; }
        public string Formatted_Address { get; set; }
        public GeometryData Geometry { get; set; }
        public string Place_Id { get; set; }
        public PlusCode PlusCode { get; set; }
        public string[] Types { get; set; }

    }

    public class AddressComponent
    {
        public string Long_Name { get; set; }
        public string Short_Name { get; set; }
        public string[] Types { get; set; }
    }

    public class GeometryData
    {
        public Location Location { get; set; }
        public string LocationType { get; set; }
        public Viewport Viewport { get; set; }
    }

    public class Location
    {
        public float Lat { get; set; }
        public float Lng { get; set; }
    }

    public class Viewport
    {
        public NorthEast NorthEast { get; set; }
        public SouthWest SouthWest { get; set; }
    }

    public class NorthEast
    {
        public float Lat { get; set; }
        public float Lng { get; set; }
    }

    public class SouthWest
    {
        public float Lat { get; set; }
        public float Lng { get; set; }
    }

    public class PlusCode
    {
        public string CompoundCode { get; set; }
        public string GlobalCode { get; set; }
    }




}
