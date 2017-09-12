using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;
using System.Configuration;
using System;
using System.Threading;

namespace VCTWebApp.Shell.Views
{
    public class EParPlusInventoryAmountsPresenter : Presenter<IeParPlusInventoryAmountsReport>
    {
        #region Instance Variables
        private readonly CustomerRepository _customerRepositoryService;
        private readonly EParPlusRepository _eParPlusRepositoryService;
        private readonly ManualConsumptionRepository _manualConsumptionRepositoryService;
        private readonly Helper _helper = new Helper();
        readonly ProductLineRepository _productLineRepository;
        static List<Customer> _listCustomer = new List<Customer>();
        UserRepository userRepositoryService;
        #endregion

        #region Constructors

        public EParPlusInventoryAmountsPresenter()
            : this(new CustomerRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public EParPlusInventoryAmountsPresenter(CustomerRepository customerRepository)
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusInventoryAmountsPresenter", "Constructor is invoked.");
            _customerRepositoryService = customerRepository;
            _eParPlusRepositoryService = new EParPlusRepository();
            _productLineRepository = new ProductLineRepository();
            _manualConsumptionRepositoryService = new ManualConsumptionRepository();
            userRepositoryService = new UserRepository();
        }

        #endregion

        #region Public Overrides

        public override void OnViewInitialized()
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusInventoryAmountsPresenter", "OnViewInitialized() is invoked.");
            SetDefaultValues();
            _listCustomer = _customerRepositoryService.FetchAllCustomer(false, HttpContext.Current.User.Identity.Name);
            FillDropdowns(string.Empty, string.Empty);

            var lstProductLinePartDetail = _productLineRepository.FetchAllProductLine();
            View.ProductLineList = lstProductLinePartDetail;

            var lstCategory = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(string.Empty, string.Empty, string.Empty, string.Empty, 0);
            View.CategoryList = lstCategory;

            var lstSubCategory1 = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(string.Empty, string.Empty, string.Empty, string.Empty, 1);
            View.SubCategory1List = lstSubCategory1;

            var lstSubCategory2 = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(string.Empty, string.Empty, string.Empty, string.Empty, 2);
            View.SubCategory2List = lstSubCategory2;

            var lstSubCategory3 = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(string.Empty, string.Empty, string.Empty, string.Empty, 3);
            View.SubCategory3List = lstSubCategory3;
        }

        #endregion

        #region Member Methods

        public void FillDropdowns(string selectedDropdown, string selectedValue)
        {
            List<string> list;
            var selectedCustomer = View.SelectedCustomerAccountFilter;
            var selectedBranchAgency = View.SelectedBranchAgencyFilter;
            var selectedManager = View.SelectedManagerFilter;
            var selectedSalesRepresentative = View.SelectedSalesRepresentativeFilter;
            var selectedState = View.SelectedStateFilter;
            var selectedOwnershipStructure = View.SelectedOwnershipStructureFilter;
            var selectedManagementStructure = View.SelectedManagementStructureFilter;

            if (string.IsNullOrEmpty(selectedDropdown))
            {
                //Fill Customer
                View.CustomerNameList = _listCustomer;

                //Fill Branch Agency
                list = _listCustomer.Select(i => i.BranchAgency).Distinct().ToList();
                View.BranchAgencyList = list;

                //Fill Manager
                list = _listCustomer.Select(i => i.Manager).Distinct().ToList();
                View.ManagerList = list;

                //Fill Sales Representative
                list = _listCustomer.Select(i => i.SalesRepresentative).Distinct().ToList();
                View.SalesRepresentativeList = list;

                //Fill State
                list = _listCustomer.Select(i => i.State).Distinct().ToList();
                View.StateList = list;

                //Fill Ownership Structure
                list = _listCustomer.Select(i => i.OwnershipStructure).Distinct().ToList();
                View.OwnershipStructureList = list;

                //Fill Management Structure
                list = _listCustomer.Select(i => i.ManagementStructure).Distinct().ToList();
                View.ManagementStructureList = list;
            }
            else
            {
                List<Customer> lstCustomerTemp;
                switch (selectedDropdown.ToUpper())
                {
                    case "BRANCHAGENCY":

                        //Fill Customer
                        if (selectedValue == "0")
                            lstCustomerTemp = _listCustomer;
                        else
                            lstCustomerTemp = _listCustomer.FindAll(i => i.BranchAgency == selectedValue);
                        View.CustomerNameList = lstCustomerTemp;

                        //Fill Manager
                        list = lstCustomerTemp.Select(i => i.Manager).Distinct().ToList();
                        View.ManagerList = list;

                        //Fill Sales Representative
                        list = lstCustomerTemp.Select(i => i.SalesRepresentative).Distinct().ToList();
                        View.SalesRepresentativeList = list;

                        //Fill State
                        list = lstCustomerTemp.Select(i => i.State).Distinct().ToList();
                        View.StateList = list;

                        //Fill Ownership Structure
                        list = lstCustomerTemp.Select(i => i.OwnershipStructure).Distinct().ToList();
                        View.OwnershipStructureList = list;

                        //Fill Management Structure
                        list = lstCustomerTemp.Select(i => i.ManagementStructure).Distinct().ToList();
                        View.ManagementStructureList = list;

                        break;


                    case "MANAGER":
                        if (selectedValue == "0")
                            lstCustomerTemp = _listCustomer;
                        else
                            lstCustomerTemp = _listCustomer.FindAll(i => i.Manager == selectedValue);

                        //Fill Customer
                        View.CustomerNameList = lstCustomerTemp;

                        //Fill Branch Agency
                        list = lstCustomerTemp.Select(i => i.BranchAgency).Distinct().ToList();
                        View.BranchAgencyList = list;

                        //Fill Sales Representative
                        list = lstCustomerTemp.Select(i => i.SalesRepresentative).Distinct().ToList();
                        View.SalesRepresentativeList = list;

                        //Fill State
                        list = lstCustomerTemp.Select(i => i.State).Distinct().ToList();
                        View.StateList = list;

                        //Fill Ownership Structure
                        list = lstCustomerTemp.Select(i => i.OwnershipStructure).Distinct().ToList();
                        View.OwnershipStructureList = list;

                        //Fill Management Structure
                        list = lstCustomerTemp.Select(i => i.ManagementStructure).Distinct().ToList();
                        View.ManagementStructureList = list;

                        break;

                    case "SALESREPRESENTATIVE":
                        if (selectedValue == "0")
                            lstCustomerTemp = _listCustomer;
                        else
                            lstCustomerTemp = _listCustomer.FindAll(i => i.SalesRepresentative == selectedValue);

                        //Fill Customer
                        View.CustomerNameList = lstCustomerTemp;

                        //Fill Branch Agency
                        list = lstCustomerTemp.Select(i => i.BranchAgency).Distinct().ToList();
                        View.BranchAgencyList = list;

                        //Fill Manager
                        list = lstCustomerTemp.Select(i => i.Manager).Distinct().ToList();
                        View.ManagerList = list;

                        ////Fill Sales Representative
                        //list = lstCustomerTemp.Select(i => i.SalesRepresentative).Distinct().ToList();
                        //View.SalesRepresentativeList = list;

                        //Fill State
                        list = lstCustomerTemp.Select(i => i.State).Distinct().ToList();
                        View.StateList = list;

                        //Fill Ownership Structure
                        list = lstCustomerTemp.Select(i => i.OwnershipStructure).Distinct().ToList();
                        View.OwnershipStructureList = list;

                        //Fill Management Structure
                        list = lstCustomerTemp.Select(i => i.ManagementStructure).Distinct().ToList();
                        View.ManagementStructureList = list;

                        break;

                    case "STATE":

                        if (selectedValue == "0")
                            lstCustomerTemp = _listCustomer;
                        else
                            lstCustomerTemp = _listCustomer.FindAll(i => i.State == selectedValue);

                        //Fill Customer
                        View.CustomerNameList = lstCustomerTemp;

                        //Fill Branch Agency
                        list = lstCustomerTemp.Select(i => i.BranchAgency).Distinct().ToList();
                        View.BranchAgencyList = list;

                        //Fill Manager
                        list = lstCustomerTemp.Select(i => i.Manager).Distinct().ToList();
                        View.ManagerList = list;

                        //Fill Sales Representative
                        list = lstCustomerTemp.Select(i => i.SalesRepresentative).Distinct().ToList();
                        View.SalesRepresentativeList = list;

                        ////Fill State
                        //list = lstCustomerTemp.Select(i => i.State).Distinct().ToList();
                        //View.StateList = list;

                        //Fill Ownership Structure
                        list = lstCustomerTemp.Select(i => i.OwnershipStructure).Distinct().ToList();
                        View.OwnershipStructureList = list;

                        //Fill Management Structure
                        list = lstCustomerTemp.Select(i => i.ManagementStructure).Distinct().ToList();
                        View.ManagementStructureList = list;

                        break;

                    case "OWNERSHIPSTRUCTURE":

                        if (selectedValue == "0")
                            lstCustomerTemp = _listCustomer;
                        else
                            lstCustomerTemp = _listCustomer.FindAll(i => i.OwnershipStructure == selectedValue);

                        //Fill Customer
                        View.CustomerNameList = lstCustomerTemp;

                        //Fill Branch Agency
                        list = lstCustomerTemp.Select(i => i.BranchAgency).Distinct().ToList();
                        View.BranchAgencyList = list;

                        //Fill Manager
                        list = lstCustomerTemp.Select(i => i.Manager).Distinct().ToList();
                        View.ManagerList = list;

                        //Fill Sales Representative
                        list = lstCustomerTemp.Select(i => i.SalesRepresentative).Distinct().ToList();
                        View.SalesRepresentativeList = list;

                        //Fill State
                        list = lstCustomerTemp.Select(i => i.State).Distinct().ToList();
                        View.StateList = list;

                        //Fill Management Structure
                        list = lstCustomerTemp.Select(i => i.ManagementStructure).Distinct().ToList();
                        View.ManagementStructureList = list;

                        break;

                    case "MANAGEMENTSTRUCTURE":

                        if (selectedValue == "0")
                            lstCustomerTemp = _listCustomer;
                        else
                            lstCustomerTemp = _listCustomer.FindAll(i => i.ManagementStructure == selectedValue);

                        //Fill Customer
                        View.CustomerNameList = lstCustomerTemp;

                        //Fill Branch Agency
                        list = lstCustomerTemp.Select(i => i.BranchAgency).Distinct().ToList();
                        View.BranchAgencyList = list;

                        //Fill Manager
                        list = lstCustomerTemp.Select(i => i.Manager).Distinct().ToList();
                        View.ManagerList = list;

                        //Fill Sales Representative
                        list = lstCustomerTemp.Select(i => i.SalesRepresentative).Distinct().ToList();
                        View.SalesRepresentativeList = list;

                        //Fill State
                        list = lstCustomerTemp.Select(i => i.State).Distinct().ToList();
                        View.StateList = list;

                        //Fill Ownership Structure
                        list = lstCustomerTemp.Select(i => i.OwnershipStructure).Distinct().ToList();
                        View.OwnershipStructureList = list;

                        break;
                }

                if (!string.IsNullOrEmpty(selectedCustomer))
                    View.SelectedCustomerAccountFilter = selectedCustomer;

                if (!string.IsNullOrEmpty(selectedBranchAgency))
                    View.SelectedBranchAgencyFilter = selectedBranchAgency;

                if (!string.IsNullOrEmpty(selectedManager))
                    View.SelectedManagerFilter = selectedManager;

                if (!string.IsNullOrEmpty(selectedSalesRepresentative))
                    View.SelectedSalesRepresentativeFilter = selectedSalesRepresentative;

                if (!string.IsNullOrEmpty(selectedCustomer))
                    View.SelectedStateFilter = selectedState;

                if (!string.IsNullOrEmpty(selectedOwnershipStructure))
                    View.SelectedOwnershipStructureFilter = selectedOwnershipStructure;

                if (!string.IsNullOrEmpty(selectedManagementStructure))
                    View.SelectedManagementStructureFilter = selectedManagementStructure;
            }
        }

        private void SetDefaultValues()
        {
            View.ListInventoryAmount = null;
            View.CategoryList = new List<string>();
            View.SubCategory1List = new List<string>();
            View.SubCategory2List = new List<string>();
            View.SubCategory3List = new List<string>();
        }

        public List<InventoryAmount> PopulateReport()
        {
            var listReportData = _eParPlusRepositoryService.FetchInventoryAmountReport
                   (View.SelectedCustomerAccountFilter, View.SelectedStateFilter, View.SelectedOwnershipStructureFilter,
                       View.SelectedManagementStructureFilter, View.SelectedBranchAgencyFilter, View.SelectedManagerFilter, View.SelectedSalesRepresentativeFilter,
                       View.SelectedProductLineFilter, View.SelectedCategoryFilter, View.SelectedSubCategory1Filter, View.SelectedSubCategory2Filter, View.SelectedSubCategory3Filter, View.ExpirationDaysFilter, View.SelectedItemStatus, HttpContext.Current.User.Identity.Name);
            View.ListInventoryAmount = listReportData;
            return listReportData;
        }

        public void PopulateCategory()
        {
            View.CategoryList = new List<string>();
            View.SubCategory1List = new List<string>();
            View.SubCategory2List = new List<string>();
            View.SubCategory3List = new List<string>();
            List<string> lstCategory = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, string.Empty, string.Empty, string.Empty, 0);
            View.CategoryList = lstCategory;
            PopulateSubCategory1();
            PopulateSubCategory2();
            PopulateSubCategory3();
        }

        public void PopulateSubCategory1()
        {
            View.SubCategory2List = new List<string>();
            View.SubCategory3List = new List<string>();
            List<string> lstSubCategory1 = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, View.SelectedCategoryFilter, string.Empty, string.Empty, 1);
            View.SubCategory1List = lstSubCategory1;
            PopulateSubCategory2();
            PopulateSubCategory3();
        }

        public void PopulateSubCategory2()
        {
            View.SubCategory3List = new List<string>();
            List<string> lstSubCategory2 = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, View.SelectedCategoryFilter, View.SelectedSubCategory1Filter, string.Empty, 2);
            View.SubCategory2List = lstSubCategory2;
            PopulateSubCategory3();
        }

        public void PopulateSubCategory3()
        {
            List<string> lstSubCategory3 = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, View.SelectedCategoryFilter, View.SelectedSubCategory1Filter, View.SelectedSubCategory2Filter, 3);
            View.SubCategory3List = lstSubCategory3;
        }

        public int InitiateEppManualScan(string accountNumber)
        {
            int numberOfShelves = _eParPlusRepositoryService.InitiateEppManualScan(accountNumber);
            return numberOfShelves;
        }

        public bool SendInventoryAmountNotificationEmail()
        {
            bool isEmailSent = false;
            EmailBody emailBody = new EmailBody();
            List<InventoryAmount> listInventoryAmount = View.ListInventoryAmount;
            try
            {
                if (listInventoryAmount != null && listInventoryAmount.Any())
                {
                    Users currentUser = userRepositoryService.FetchUserByName(HttpContext.Current.User.Identity.Name);
                    if (!string.IsNullOrEmpty(currentUser.EmailID))
                    {
                        var distinctAccountNumber = listInventoryAmount.GroupBy(x => x.AccountNumber).Select(y => y.First());
                        if (distinctAccountNumber != null && distinctAccountNumber.Any())
                        {
                            foreach (var item in distinctAccountNumber)
                            {
                                List<InventoryAmount> lstInventoryAmountSummary = new List<InventoryAmount>();

                                string accountNumber = item.AccountNumber;
                                string customerName = item.CustomerName;
                                List<InventoryAmount> distinctCutomerData = listInventoryAmount.FindAll(s => s.AccountNumber == accountNumber);
                                var lstRefNum = distinctCutomerData.Select(i => i.RefNum).Distinct();

                                foreach (var RefNum in lstRefNum)
                                {
                                    InventoryAmount inventoryAmount = new InventoryAmount();
                                    inventoryAmount.CustomerName = customerName;
                                    inventoryAmount.AccountNumber = accountNumber;
                                    inventoryAmount.RefNum = RefNum;
                                    inventoryAmount.ProductLine = distinctCutomerData.First(f => f.CustomerName == customerName && f.RefNum == RefNum).ProductLine;
                                    inventoryAmount.ProductLineDesc = distinctCutomerData.First(f => f.CustomerName == customerName && f.RefNum == RefNum).ProductLineDesc;

                                    inventoryAmount.LastScanned = distinctCutomerData.First(f => f.CustomerName == customerName && f.RefNum == RefNum).LastScanned;
                                    inventoryAmount.PartDesc = distinctCutomerData.First(f => f.CustomerName == customerName && f.RefNum == RefNum).PartDesc;
                                    inventoryAmount.IsNearExpiry = distinctCutomerData.Any(a => a.CustomerName == customerName && a.RefNum == RefNum && a.IsNearExpiry == true);
                                    inventoryAmount.Qty = distinctCutomerData.Where(w => w.CustomerName == customerName && w.RefNum == RefNum).Count();
                                    inventoryAmount.PARLevelQty = distinctCutomerData.First(f => f.CustomerName == customerName && f.RefNum == RefNum).PARLevelQty;
                                    int offcartQty = 0;
                                    offcartQty = distinctCutomerData.Where(w => w.CustomerName == customerName && w.RefNum == RefNum && w.ItemStatus == "OFFCART").Count();
                                    inventoryAmount.OffCartQty = offcartQty;
                                    lstInventoryAmountSummary.Add(inventoryAmount);
                                }

                                if (lstInventoryAmountSummary.Any())
                                {
                                    string emailServer = System.Configuration.ConfigurationManager.AppSettings["EmailServer"];
                                    string emailPort = System.Configuration.ConfigurationManager.AppSettings["EmailPort"];
                                    string senderEmailId = System.Configuration.ConfigurationManager.AppSettings["SenderEmailId"];
                                    var senderName = Convert.ToString(ConfigurationManager.AppSettings["SenderName"]);
                                    var senderPassword = Convert.ToString(ConfigurationManager.AppSettings["SenderPassword"]);
                                    senderPassword = !string.IsNullOrEmpty(senderPassword) ? Encryption.Decrypt(senderPassword) : "";
                                    var mailUseDefaultCredentials = Convert.ToString(ConfigurationManager.AppSettings["MailUseDefaultCredentials"]);
                                    var mailEnableSsl = Convert.ToString(ConfigurationManager.AppSettings["MailEnableSsl"]);
                                    var message = emailBody.CreateInventoryAmountReportBody(lstInventoryAmountSummary);


                                    using (var emailHelper = new EmailHelper())
                                    {
                                        emailHelper.To = currentUser.EmailID;
                                        emailHelper.From = senderEmailId;
                                        emailHelper.FromDisplayName = senderName;
                                        emailHelper.Cc = "";
                                        emailHelper.Bcc = "";
                                        emailHelper.Subject = customerName + " Inventory Audit";
                                        emailHelper.Message = message;
                                        isEmailSent = emailHelper.SendMail(emailServer.Trim(), emailPort.Trim(), senderEmailId.Trim(), senderPassword.Trim(), mailUseDefaultCredentials, mailEnableSsl);
                                        Thread.Sleep(500);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("NO EMAIL CONFIGURED");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isEmailSent;
        }

        #endregion Methods


        public bool SaveManualConsumption(string accountNumber, string tagId)
        {
            var manualConsumption = new ManualConsumption
            {
                AccountNumber = accountNumber,
                TagId = tagId,
                IsActive = true,
                UpdatedBy = HttpContext.Current.User.Identity.Name
            };
            var isSaved = _manualConsumptionRepositoryService.SaveManualConsumption(manualConsumption);
            return isSaved;
        }

        public bool IsManaulScanCompleted(string accountNumber, System.DateTime manualScanInitiatedAt)
        {
            return _manualConsumptionRepositoryService.IsManaulScanCompleted(accountNumber, manualScanInitiatedAt);
        }
    }
}
