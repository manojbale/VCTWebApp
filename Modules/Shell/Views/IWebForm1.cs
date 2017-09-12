using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;


namespace VCTWebApp.Shell.Views
{
    public interface IWebForm1
    {
        public List<CaseType> CaseTypeList { set; }

        public string SelectedCaseType { get; }
        public string CaseType { get; set; }

    }
}
