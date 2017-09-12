﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IeParPlusShipAndBillReport
    {
        List<OrderDetail> ListOrderDetail { set; }

        List<VCTWeb.Core.Domain.DispositionType> DispositionTypeList{ set; }

        List<Customer> CustomerNameList { set; }
        List<string> BranchAgencyList { set; }
        List<string> ManagerList { set; }
        List<string> SalesRepresentativeList { set; }
        List<string> StateList { set; }
        List<string> OwnershipStructureList { set; }
        List<string> ManagementStructureList { set; }


        List<ProductLine> ProductLineList { set; }
        List<string> CategoryList { set; }
        List<string> SubCategory1List { set; }
        List<string> SubCategory2List { set; }
        List<string> SubCategory3List { set; }


        string SelectedCustomerAccountFilter { get; set; }
        string SelectedStateFilter { get; set; }
        string SelectedOwnershipStructureFilter { get; set; }
        string SelectedManagementStructureFilter { get; set; }
        string SelectedBranchAgencyFilter { get; set; }
        string SelectedManagerFilter { get; set; }
        string SelectedSalesRepresentativeFilter { get; set; }

        string SelectedProductLineFilter { get; }
        string SelectedCategoryFilter { get; }
        string SelectedSubCategory1Filter { get; }
        string SelectedSubCategory2Filter { get; }
        string SelectedSubCategory3Filter { get; }




        //Int32 OrderId { get; set; }
        Int32 DispositionTypeId { get; }
        string Remarks { get; }
        Int32 Qty { get;  }


        DateTime? SelectedOrderStartDate { get; }
        DateTime? SelectedOrderEndDate { get; }

        DateTime? SelectedShippedStartDate { get; }
        DateTime? SelectedShippedEndDate { get; }    
    }
}
