using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IeParPlusReaderDetailView
    {
        int CustomerShelfId { get; set; }
        string AccountNumber { set; get; }
        string ShelfName { set; get; }
        string ShelfCode { set; get; }
        string ReaderHealthLastUpdatedOn { set; }
        string ReaderIP { set; }
        string TotalAntenna { set; }

        List<CustomerShelfProperty> ListOfCustomerShelfProperty { set; get; }
        List<CustomerShelfAntennaProperty> ListOfCustomerShelfAntennaProperty { set; get; }

        List<CustomerShelfAntenna> ListOfDistinctAntenna { set; get; }
        int SelectedCustomerShelfAntennaId { get; set; }
    }
}
