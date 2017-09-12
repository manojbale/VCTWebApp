using System;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class Country
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
