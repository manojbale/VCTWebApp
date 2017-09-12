using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface ICustomerView
    {
        string CustomerName { get; set; }
        string AccountNumber { get; set; }
        string StreetAddress { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }
        string OwnershipStructure { get; set; }
        string ManagementStructure { get; set; }
        string SpineOnlyMultiSpecialty { get; set; }
        string BranchAgency { get; set; }
        string Manager { get; set; }
        int? QtyOfORs { get; set; }
        string SalesRepresentative { get; set; }
        string SpecialistRep { get; set; }
        bool Active { get; set; }    
        int ConsumptionInterval{ get; set; }        
        int AssetNearExpiryDays { get; set; }

        [CLSCompliant(true)]
        List<Customer> CustomerList { set; }
        List<string> OwnershipStructureList { set; }
        List<string> ManagementStructureList { set; }
        List<string> SpineOnlyMultiSpecialtyList { set; }
        List<string> BranchAgencyList { set; }
        List<Users> SalesRepresentativeList { set; }
        List<Users> SpecialistRepList { set; }
        List<Users> ManagerList { set; }
        List<State> StateList { set; }

        List<CustomerProductLine> CustomerProductLineList { set; get; }

        string SelectedCustomerAccountNumber { get; }
        string SelectedCustomerNameFilter { get; }
        string SelectedAccountNumberFilter { get; }
        string SelectedOwnershipStructureFilter { get; }
        string SelectedManagementStructureFilter { get; }        
        string SelectedSpineonlyMultiSpecialtyFilter { get; }
        string SelectedBranchAgencyFilter { get; }
        string SelectedManagerFilter { get; }
        string SelectedSalesRepresentativeFilter { get; }
        bool SelectedActiveInactiveFilter { get; }
        string SelectedProductLines { get; }
    }
}
