using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IeParPlusReaderDashboard
    {
        int CustomerShelfId { get; set; }
        List<CustomerShelf> ListCustomerShelf { get; set; }
        List<CustomerShelfProperty> ListCustomerShelfProperty { get; set; }
        List<CustomerShelfAntennaProperty> ListCustomerShelfAntennaProperty { get; set; }
        int ColumnPerRowInDashboard { get; set; }
    }
}
