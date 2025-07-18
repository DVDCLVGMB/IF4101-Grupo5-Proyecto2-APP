﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace Steady_Management_App.Models
{
    public class City
    {
        public int    CityId   { get; set; }
        public string CityName { get; set; } = string.Empty;
        public int    Province { get; set; }
        public string Country  { get; set; }

        public City() { }

        public City(int cityId, string cityName, int province, string country)
        {
            CityId   = cityId;
            CityName = cityName;
            Province = province;
            Country  = country;
        }
    }
}
