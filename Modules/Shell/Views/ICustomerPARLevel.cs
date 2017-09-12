using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface ICustomerPARLevel
    {
        List<CustomerPARLevel> CustomerPARLevelList { set; get; }
        List<Customer> CustomerList { set; }
        string SelectedAccountNumber { get; }
        int PageSize { get; set; }
    }
}
