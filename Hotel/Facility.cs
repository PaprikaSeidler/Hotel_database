﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hotel
{
    public class Facility
    {
        public int Facility_ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"ID: {Facility_ID}, Name: {Name}";
        }
    }
}
