using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Helpers
{
    public class JwtSettings
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int DurationInDays { get; set; }
    }
}
