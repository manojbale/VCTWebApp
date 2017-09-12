using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public class WebForm1Presenter : Presenter<IWebForm1>
    {
        #region Public Overrides
        public override void OnViewInitialized()
        {
            PopulateCaseTypeList();
        }

        private void PopulateCaseTypeList()
        {
            this.View.CaseTypeList = new CaseRepository().FetchAllCaseType();
        }

        public override void OnViewLoaded()
        {
            this.View.CaseType = this.View.SelectedCaseType;
        }

        public void Save()
        {
            string newcasetype = this.View.CaseType;
            OnViewInitialized();
        }


        #endregion
    }
}
