using System;
using System.Collections.Generic;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IConfigurationSettingView
    {
        List<Configuration> ConfigurationGroupList { set; }
        List<Configuration> ConfigurationsByGroupList { set; }
    }
}




