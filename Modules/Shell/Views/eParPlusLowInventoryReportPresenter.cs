using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Threading;
using System.Globalization;

namespace VCTWebApp.Shell.Views
{
    public class eParPlusLowInventoryReportPresenter : Presenter<IeParPlusLowInventoryReport>
    {
        #region Instance Variables
        private CustomerRepository customerRepositoryService;
        private UserRepository userRepositoryService;
        private EParPlusRepository eParPlusRepositoryService;
        private Helper helper = new Helper();
        ProductLineRepository productLineRepository;
        static List<Customer> ListCustomer = new List<Customer>();
        #endregion

        #region Constructors

        public eParPlusLowInventoryReportPresenter()
            : this(new CustomerRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public eParPlusLowInventoryReportPresenter(CustomerRepository customerRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusLowInventoryReportPresenter", "Constructor is invoked.");
            this.customerRepositoryService = customerRepository;
            this.userRepositoryService = new UserRepository();
            eParPlusRepositoryService = new EParPlusRepository();
            productLineRepository = new ProductLineRepository();
            userRepositoryService = new UserRepository();
        }

        #endregion

        #region Public Overrides

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusLowInventoryReportPresenter", "OnViewInitialized() is invoked.");
            try
            {
                SetDefaultValues();
                ListCustomer = customerRepositoryService.FetchAllCustomer(false, HttpContext.Current.User.Identity.Name);
                FillDropdowns(string.Empty, string.Empty);

                List<ProductLine> lstProductLinePartDetail = this.productLineRepository.FetchAllProductLine();
                View.ProductLineList = lstProductLinePartDetail;

                List<string> lstCategory = this.eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(string.Empty, string.Empty, string.Empty, string.Empty, 0);
                View.CategoryList = lstCategory;

                List<string> lstSubCategory1 = this.eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(string.Empty, string.Empty, string.Empty, string.Empty, 1);
                View.SubCategory1List = lstSubCategory1;

                List<string> lstSubCategory2 = this.eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(string.Empty, string.Empty, string.Empty, string.Empty, 2);
                View.SubCategory2List = lstSubCategory2;

                List<string> lstSubCategory3 = this.eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(string.Empty, string.Empty, string.Empty, string.Empty, 3);
                View.SubCategory3List = lstSubCategory3;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Public Methods

        public void FillDropdowns(string SelectedDropdown, string SelectedValue)
        {
            List<string> list = new List<string>();
            List<VCTWeb.Core.Domain.Customer> lstCustomerTemp = new List<VCTWeb.Core.Domain.Customer>();
            string SelectedCustomer = View.SelectedCustomerAccountFilter;
            string SelectedBranchAgency = View.SelectedBranchAgencyFilter;
            string SelectedManager = View.SelectedManagerFilter;
            string SelectedSalesRepresentative = View.SelectedSalesRepresentativeFilter;
            string SelectedState = View.SelectedStateFilter;
            string SelectedOwnershipStructure = View.SelectedOwnershipStructureFilter;
            string SelectedManagementStructure = View.SelectedManagementStructureFilter;

            if (string.IsNullOrEmpty(SelectedDropdown))
            {
                //Fill Customer
                View.CustomerNameList = ListCustomer;

                //Fill Branch Agency
                list = ListCustomer.Select(i => i.BranchAgency).Distinct().ToList();
                View.BranchAgencyList = list;

                //Fill Manager
                list = ListCustomer.Select(i => i.Manager).Distinct().ToList();
                View.ManagerList = list;

                //Fill Sales Representative
                list = ListCustomer.Select(i => i.SalesRepresentative).Distinct().ToList();
                View.SalesRepresentativeList = list;

                //Fill State
                list = ListCustomer.Select(i => i.State).Distinct().ToList();
                View.StateList = list;

                //Fill Ownership Structure
                list = ListCustomer.Select(i => i.OwnershipStructure).Distinct().ToList();
                View.OwnershipStructureList = list;

                //Fill Management Structure
                list = ListCustomer.Select(i => i.ManagementStructure).Distinct().ToList();
                View.ManagementStructureList = list;
            }
            else
            {
                switch (SelectedDropdown.ToUpper())
                {
                    case "BRANCHAGENCY":

                        //Fill Customer
                        if (SelectedValue == "0")
                            lstCustomerTemp = ListCustomer;
                        else
                            lstCustomerTemp = ListCustomer.FindAll(i => i.BranchAgency == SelectedValue);
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
                        if (SelectedValue == "0")
                            lstCustomerTemp = ListCustomer;
                        else
                            lstCustomerTemp = ListCustomer.FindAll(i => i.Manager == SelectedValue);

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
                        if (SelectedValue == "0")
                            lstCustomerTemp = ListCustomer;
                        else
                            lstCustomerTemp = ListCustomer.FindAll(i => i.SalesRepresentative == SelectedValue);

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

                        if (SelectedValue == "0")
                            lstCustomerTemp = ListCustomer;
                        else
                            lstCustomerTemp = ListCustomer.FindAll(i => i.State == SelectedValue);

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

                        if (SelectedValue == "0")
                            lstCustomerTemp = ListCustomer;
                        else
                            lstCustomerTemp = ListCustomer.FindAll(i => i.OwnershipStructure == SelectedValue);

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

                        if (SelectedValue == "0")
                            lstCustomerTemp = ListCustomer;
                        else
                            lstCustomerTemp = ListCustomer.FindAll(i => i.ManagementStructure == SelectedValue);

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

                if (!string.IsNullOrEmpty(SelectedCustomer))
                    View.SelectedCustomerAccountFilter = SelectedCustomer;

                if (!string.IsNullOrEmpty(SelectedBranchAgency))
                    View.SelectedBranchAgencyFilter = SelectedBranchAgency;

                if (!string.IsNullOrEmpty(SelectedManager))
                    View.SelectedManagerFilter = SelectedManager;

                if (!string.IsNullOrEmpty(SelectedSalesRepresentative))
                    View.SelectedSalesRepresentativeFilter = SelectedSalesRepresentative;

                if (!string.IsNullOrEmpty(SelectedCustomer))
                    View.SelectedStateFilter = SelectedState;

                if (!string.IsNullOrEmpty(SelectedOwnershipStructure))
                    View.SelectedOwnershipStructureFilter = SelectedOwnershipStructure;

                if (!string.IsNullOrEmpty(SelectedManagementStructure))
                    View.SelectedManagementStructureFilter = SelectedManagementStructure;
            }
        }

        private void SetDefaultValues()
        {
            try
            {
                View.ListLowInventory = null;
                View.CategoryList = new List<string>();
                View.SubCategory1List = new List<string>();
                View.SubCategory2List = new List<string>();
                View.SubCategory3List = new List<string>();
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        public List<LowInventory> PopulateReport()
        {
            List<LowInventory> listReportData = eParPlusRepositoryService.FetchLowInventoryReport
                   (View.SelectedCustomerAccountFilter, View.SelectedStateFilter, View.SelectedOwnershipStructureFilter,
                       View.SelectedManagementStructureFilter, View.SelectedBranchAgencyFilter, View.SelectedManagerFilter, View.SelectedSalesRepresentativeFilter,
                       View.SelectedProductLineFilter, View.SelectedCategoryFilter, View.SelectedSubCategory1Filter, View.SelectedSubCategory2Filter, View.SelectedSubCategory3Filter, HttpContext.Current.User.Identity.Name);

            View.ListLowInventory = listReportData;
            return listReportData;
        }

        public void PopulateCategory()
        {
            View.CategoryList = new List<string>();
            View.SubCategory1List = new List<string>();
            View.SubCategory2List = new List<string>();
            View.SubCategory3List = new List<string>();
            List<string> lstCategory = this.eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, string.Empty, string.Empty, string.Empty, 0);
            View.CategoryList = lstCategory;
            PopulateSubCategory1();
            PopulateSubCategory2();
            PopulateSubCategory3();
        }

        public void PopulateSubCategory1()
        {
            View.SubCategory2List = new List<string>();
            View.SubCategory3List = new List<string>();
            List<string> lstSubCategory1 = this.eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, View.SelectedCategoryFilter, string.Empty, string.Empty, 1);
            View.SubCategory1List = lstSubCategory1;
            PopulateSubCategory2();
            PopulateSubCategory3();
        }

        public void PopulateSubCategory2()
        {
            View.SubCategory3List = new List<string>();
            List<string> lstSubCategory2 = this.eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, View.SelectedCategoryFilter, View.SelectedSubCategory1Filter, string.Empty, 2);
            View.SubCategory2List = lstSubCategory2;
            PopulateSubCategory3();
        }

        public void PopulateSubCategory3()
        {
            List<string> lstSubCategory3 = this.eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, View.SelectedCategoryFilter, View.SelectedSubCategory1Filter, View.SelectedSubCategory2Filter, 3);
            View.SubCategory3List = lstSubCategory3;
        }

        public bool SendLowInventoryNotificationEmail()
        {
            var isEmailSent = false;
            var emailBody = new EmailBody();
            var listLowInventory = View.ListLowInventory;
            try
            {
                if (listLowInventory != null && listLowInventory.Any())
                {
                    var currentUser = userRepositoryService.FetchUserByName(HttpContext.Current.User.Identity.Name);

                    if (!string.IsNullOrEmpty(currentUser.EmailID))
                    {
                        var distinctAccountNumber = listLowInventory.GroupBy(x => x.AccountNumber).Select(y => y.First());

                        if (distinctAccountNumber != null && distinctAccountNumber.Any())
                        {
                            foreach (var item in distinctAccountNumber)
                            {
                                var accountNumber = item.AccountNumber;
                                var listReportData = listLowInventory.FindAll(x => x.AccountNumber == accountNumber);
                                if (listReportData.Any())
                                {
                                    var emailServer = ConfigurationManager.AppSettings["EmailServer"];
                                    var emailPort = ConfigurationManager.AppSettings["EmailPort"];
                                    var senderEmailId = ConfigurationManager.AppSettings["SenderEmailId"];
                                    var senderName = Convert.ToString(ConfigurationManager.AppSettings["SenderName"]);
                                    var senderPassword = Convert.ToString(ConfigurationManager.AppSettings["SenderPassword"]);
                                    senderPassword = !string.IsNullOrEmpty(senderPassword) ? Encryption.Decrypt(senderPassword) : "";
                                    var mailUseDefaultCredentials = Convert.ToString(ConfigurationManager.AppSettings["MailUseDefaultCredentials"]);
                                    var mailEnableSsl = Convert.ToString(ConfigurationManager.AppSettings["MailEnableSsl"]);
                                    var message = emailBody.CreateLowInventoryReportBody(listLowInventory);
                                    var customerName = listReportData[0].CustomerName;
                                    using (var emailHelper = new EmailHelper())
                                    {
                                        emailHelper.To = currentUser.EmailID;
                                        emailHelper.From = senderEmailId;
                                        emailHelper.FromDisplayName = senderName;
                                        emailHelper.Cc = "";
                                        emailHelper.Bcc = "";
                                        emailHelper.Subject = "ACTION REQUIRED: " + customerName + ", Low Inventory Notification";
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

        #endregion Public Methods
    }
}
