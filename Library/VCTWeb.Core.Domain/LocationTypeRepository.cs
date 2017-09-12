using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    public class LocationTypeRepository
    {
        private string _user;

        public LocationTypeRepository()
        {
        }

        public LocationTypeRepository(string user)
        {
            _user = user;
        }
    }
}
