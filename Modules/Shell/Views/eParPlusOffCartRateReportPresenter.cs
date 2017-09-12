using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class eParPlusOffCartRateReportPresenter : Presenter<IeParPlusOffCartRateReport>
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

        public eParPlusOffCartRateReportPresenter()
            : this(new CustomerRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public eParPlusOffCartRateReportPresenter(CustomerRepository customerRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusOffCartRateReportPresenter", "Constructor is invoked.");
            this.customerRepositoryService = customerRepository;
            this.userRepositoryService = new UserRepository();
            eParPlusRepositoryService = new EParPlusRepository();
            productLineRepository = new ProductLineRepository();
        }

        #endregion

        #region Public Overrides

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusOffCartRateReportPresenter", "OnViewInitialized() is invoked.");
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
                View.ListOffCartAmount = null;
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

        public List<InventoryOffCartRate> PopulateReport()
        {
            List<InventoryOffCartRate> listReportData = eParPlusRepositoryService.FetchInventoryOffCartRateReport
                   (View.SelectedCustomerAccountFilter, View.SelectedStateFilter, View.SelectedOwnershipStructureFilter,
                       View.SelectedManagementStructureFilter, View.SelectedBranchAgencyFilter, View.SelectedManagerFilter, View.SelectedSalesRepresentativeFilter,
                       View.SelectedProductLineFilter, View.SelectedCategoryFilter, View.SelectedSubCategory1Filter, View.SelectedSubCategory2Filter, View.SelectedSubCategory3Filter, View.SelectedStartDate, View.SelectedEndDate, HttpContext.Current.User.Identity.Name);

            View.ListOffCartAmount = listReportData;
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

        #endregion Public Methods
    }
}
