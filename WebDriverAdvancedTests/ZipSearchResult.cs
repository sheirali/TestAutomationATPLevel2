using System;
using System.Collections.Generic;
using System.Text;

namespace WebDriverAdvancedTests
{
    public class ZipSearchResult
    {
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string MapUrl => $"https://www.google.com/maps/place/{Latitude}+{this.Longitude}";

        public override string ToString()
        {
           return $"City='{City}'  State='{State}'  Zip='{ZipCode}'  Latitude='{Latitude}'  Longitude='{Longitude}'  mapUrl='{MapUrl}'";
        }
    }
}
