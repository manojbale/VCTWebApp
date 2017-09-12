using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface ITagHistoryView
    {
        List<EPPTransaction> TagHistoryList { set; }
        string TagId { set; }
        string RefNum { set; }
        string LotNum { set; }
    }
}
