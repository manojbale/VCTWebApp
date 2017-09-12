using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class ReturnInventoryRMAPresenter : Presenter<IReturnInventoryRMAView>
    {
        private InventoryStockRepository InventoryStockRepositoryService;        
        private Helper helper = new Helper();

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        // private IShellController _controller;
        // public KitListingPresenter([CreateNew] IShellController controller)
        // {
        // 		_controller = controller;
        // }

        #region Constructors

        public ReturnInventoryRMAPresenter()
            : this(new InventoryStockRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public ReturnInventoryRMAPresenter(InventoryStockRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "VirtualCheckOutPresenter", "Constructor is invoked.");

            this.InventoryStockRepositoryService = new InventoryStockRepository();            
        }

        #endregion

        #region Public Overrides
        public override void OnViewInitialized()
        {            
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ReturnInventoryRMAPresenter", "OnViewInitialized() is invoked.");
            try
            {
                //PopulateHospital();
                PopulateReturnInventoryList();
                View.DispositionList = PopulateDispositionType();
            }
            catch
            {
                throw;
            }

            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ReturnInventoryRMAPresenter", "OnViewInitialized() is completed");
        }

        public override void OnViewLoaded()
        {            
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ReturnInventoryRMAPresenter", "OnViewLoaded() is invoked.");

            try
            {
                PopulateReturnInventoryList();
                
                helper.LogInformation(HttpContext.Current.User.Identity.Name, "ReturnInventoryRMAPresenter", "OnViewLoaded() is completed");

            }
            catch
            {
                throw;
            }

        }     
        #endregion

        #region Public Methods
        public bool Save(out string resultCaseNum)
        {
            //Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
            bool result = false;
            try
            {

                result = new CaseRepository().SaveReturnInventoryRMA(View.InventoryType, View.CaseNum, View.ReturnFrom, View.PartyId, View.ToLocationId, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]), View.TableXml, HttpContext.Current.User.Identity.Name, out resultCaseNum);
                              
            }
            catch
            {
                throw;
            }

            return result;
        }

        public List<DispositionType> PopulateDispositionType()
        {
            return (new CaseRepository().GetDespositionTypeByCategory("ReturnInventoryRMA"));
        }

        public List<KeyValuePair<string, string>> GetCasesListByPartAndLotNum(string PartNum, string LotNum, Int64 PartyId)
        {
            return (new CaseRepository().GetCasesListByPartAndLotNum(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]), PartNum, LotNum, PartyId));
        }

        public List<ReturnInventoryRMA> PopulateEmptyInventoryReturnRMADetail()
        {
            List<ReturnInventoryRMA> lstRMA = new List<ReturnInventoryRMA>();
            lstRMA.Add(new ReturnInventoryRMA());
            return lstRMA;
        }
        #endregion

        #region Private Methods
        public void PopulateReturnInventoryList()
        {
            if (View.InventoryType.ToLower() == "case")
            {
                if (string.IsNullOrEmpty(View.CaseNum))
                {
                    View.ReturnInventoryKitList = PopulateEmptyInventoryReturnRMADetail();
                }
                else
                {
                    View.ReturnInventoryKitList = new CaseRepository().GetReturnInventoryRMADetailByCaseNum(View.CaseNum, Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
                }
            }
            else
            {
                View.ReturnInventoryPartList = PopulateEmptyInventoryReturnRMADetail();
            }
            
        }

        public void PopulateHospital()
        {
            View.PartyList = new CaseRepository().GetPartyByLocation(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        public void PopulateRegion()
        {
            View.RegionList = new CaseRepository().GetRegionList(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }

        public void PopulateCorp()
        {
            View.CorpList = new CaseRepository().GetCorpList(Convert.ToInt32(HttpContext.Current.Session["LoggedInLocationId"]));
        }
              
        #endregion

    }

}
