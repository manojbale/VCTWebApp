using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;
using System.Web.UI.WebControls;

namespace VCTWebApp.Shell.Views
{
    public interface IProductLinePartDetail
    {
        List<ProductLinePartDetail> ListProductLinePartDetail { set; }
        TreeNodeCollection ProductLinePartDetailNodeList { get; set; }

        int SelectedNodeLevel { get; set; }     
        string SelectedProductLine { get; set; }        
        string SelectedCategory { get; set; }
        string SelectedSubCategory1 { get; set; }
        string SelectedSubCategory2 { get; set; }
        string SelectedSubCategory3 { get; set; }
    }
}
