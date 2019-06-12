using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;

namespace Nop.Plugin.Widgets.Campgrounds.Mapping
{
    public partial class MapPoints
    {
        public MapPoints(string Id,
        string Name,
        string Description,
        GeoCoordinate CampgroundLocation)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.CampgroundLocation = CampgroundLocation;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public GeoCoordinate CampgroundLocation { get; set; }


    }

}
