using System;
using System.Collections.Generic;

namespace TBC.OpenAPI.SDK.Core.Tests.Models
{
    public class HttpTestResponseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; }
        public List<int>? Numbers { get; set; }
    }
}
