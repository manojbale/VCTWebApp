using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using System.Web;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public class TagHistoryPresenter : Presenter<ITagHistoryView>
    {
        private Helper helper = new Helper();

        #region Constructors

        public TagHistoryPresenter()
        {

        }

        #endregion

        public override void OnViewLoaded()
        {

        }

        public void PopulateData(string accountNumber, string tagId)
        {
            var listEppTransaction = new EParPlusRepository().FetchTagHistoryByTagId(accountNumber, tagId);
            View.TagHistoryList = listEppTransaction;
            if (listEppTransaction != null && listEppTransaction.Any())
            {
                View.RefNum = listEppTransaction[0].RefNum;
                View.LotNum = listEppTransaction[0].LotNum;
                View.TagId = listEppTransaction[0].TagId;
            }
        }
    }
}
