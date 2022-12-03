﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GraduateThesis.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderOptions
    {
        ASC, DESC
    }
}
