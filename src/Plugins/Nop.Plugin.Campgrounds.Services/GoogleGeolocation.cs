using Newtonsoft.Json;
using System.Collections.Generic;

namespace Nop.Plugin.Campgrounds.Services
{
    // To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
    //
    //    using Nop.Plugin.Campgrounds.Services;
    //
    //    var data = GoogleGeocode.FromJson(jsonString);

    public partial class GoogleGeocode
    {
        [JsonProperty("results")]
        public List<Result> Results { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("address_components")]
        public List<AddressComponent> AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonProperty("geometry")]
        public Geometry Geometry { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }
    }

    public partial class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }
    }

    public partial class Geometry
    {
        [JsonProperty("bounds")]
        public Bounds Bounds { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("location_type")]
        public string LocationType { get; set; }

        [JsonProperty("viewport")]
        public Bounds Viewport { get; set; }
    }

    public partial class Bounds
    {
        [JsonProperty("northeast")]
        public Location Northeast { get; set; }

        [JsonProperty("southwest")]
        public Location Southwest { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("lat")]
        public decimal Lat { get; set; }

        [JsonProperty("lng")]
        public decimal Lng { get; set; }

        [JsonProperty("placeId")]
        public string PlaceId { get; set; }

        [JsonProperty("stateProvinceId")]
        public int StateProvinceId { get; set; }

        [JsonProperty("geocodeURL")]
        public string GeocodeURL { get; set; }

        [JsonProperty("found")]
        public bool Found { get; set; }

    }

    public partial class GoogleGeocode
    {
        public static GoogleGeocode FromJson(string json) => JsonConvert.DeserializeObject<GoogleGeocode>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GoogleGeocode self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}