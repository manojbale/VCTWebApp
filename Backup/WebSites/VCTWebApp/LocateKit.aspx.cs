using System;
using Microsoft.Practices.ObjectBuilder;

namespace VCTWebApp.Shell.Views
{
    public partial class LocateKit : Microsoft.Practices.CompositeWeb.Web.UI.Page, ILocateKitView
    {
        private LocateKitPresenter _presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["KitMapInfo"] != null)
            {
                string sKitMapInfo = "[" + Session["KitMapInfo"].ToString() + "]";
                hdKitData.Value = sKitMapInfo;

            }
            //if (!this.IsPostBack)
            //{
            //    this._presenter.OnViewInitialized();
            //}
            //this._presenter.OnViewLoaded();
        }

        [CreateNew]
        public LocateKitPresenter Presenter
        {
            get
            {
                return this._presenter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                this._presenter = value;
                this._presenter.View = this;
            }
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

    }
}

