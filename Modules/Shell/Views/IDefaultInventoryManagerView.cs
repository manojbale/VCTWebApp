using System.Collections.Generic;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IDefaultInventoryManagerView
    {
        int InventoryUtilizationDay { set; get; }
        List<Default> PartOrderList { set; }
        List<ActiveKitFamily> ActiveKitFamilyList { set; }
        List<PendingBuildKit> PendingBuildKitList { set; }
        List<Default> CasesList { get; set; }
        Default BranchDetail { get; set; }

        int LocationId { get; }
        System.DateTime BusinessDate { get; }
    }
}




