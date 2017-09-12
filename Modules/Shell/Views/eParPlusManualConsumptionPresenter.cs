using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class EParPlusManualConsumptionPresenter : Presenter<IeParPlusManualConsumption>
    {
        #region Instance Variables
        private readonly CustomerRepository _customerRepositoryService;
        private readonly EParPlusRepository _eParPlusRepositoryService;
        private readonly ManualConsumptionRepository _manualConsumptionRepositoryService;
        private readonly Helper _helper = new Helper();
        readonly ProductLineRepository _productLineRepository;
        static List<Customer> _listCustomer = new List<Customer>();
        #endregion

        #region Constructors

        public EParPlusManualConsumptionPresenter()
            : this(new CustomerRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public EParPlusManualConsumptionPresenter(CustomerRepository customerRepository)
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusManualConsumptionPresenter", "Constructor is invoked.");
            _customerRepositoryService = customerRepository;
            _eParPlusRepositoryService = new EParPlusRepository();
            _productLineRepository = new ProductLineRepository();
            _manualConsumptionRepositoryService = new ManualConsumptionRepository();
        }

        #endregion

        #region Public Overrides

        public override void OnViewInitialized()
        {
            _helper.LogInformation(HttpContext.Current.User.Identity.Name, "eParPlusLowInventoryReportPresenter", "OnViewInitialized() is invoked.");
            
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

        #region Public Methods

        public void FillDropdowns(string selectedDropdown, string selectedValue)
        {
            List<string> list;
            string selectedCustomer = View.SelectedCustomerAccountFilter;
            string selectedBranchAgency = View.SelectedBranchAgencyFilter;
            string selectedManager = View.SelectedManagerFilter;
            string selectedSalesRepresentative = View.SelectedSalesRepresentativeFilter;
            string selectedState = View.SelectedStateFilter;
            string selectedOwnershipStructure = View.SelectedOwnershipStructureFilter;
            string selectedManagementStructure = View.SelectedManagementStructureFilter;

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
                        lstCustomerTemp = selectedValue == "0" ? _listCustomer : _listCustomer.FindAll(i => i.BranchAgency == selectedValue);
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
                        lstCustomerTemp = selectedValue == "0" ? _listCustomer : _listCustomer.FindAll(i => i.Manager == selectedValue);

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
                        lstCustomerTemp = selectedValue == "0" ? _listCustomer : _listCustomer.FindAll(i => i.SalesRepresentative == selectedValue);

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

                        lstCustomerTemp = selectedValue == "0" ? _listCustomer : _listCustomer.FindAll(i => i.State == selectedValue);

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

                        lstCustomerTemp = selectedValue == "0" ? _listCustomer : _listCustomer.FindAll(i => i.OwnershipStructure == selectedValue);

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

                        lstCustomerTemp = selectedValue == "0" ? _listCustomer : _listCustomer.FindAll(i => i.ManagementStructure == selectedValue);

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
            View.ListManualConsumptionReport = null;
            View.CategoryList = new List<string>();
            View.SubCategory1List = new List<string>();
            View.SubCategory2List = new List<string>();
            View.SubCategory3List = new List<string>();
        }

        public List<ManualConsumptionReport> PopulateReport()
        {
            var listReportData = _eParPlusRepositoryService.FetchManualConsumptionReport
                   (View.SelectedCustomerAccountFilter, View.SelectedStateFilter, View.SelectedOwnershipStructureFilter,
                       View.SelectedManagementStructureFilter, View.SelectedBranchAgencyFilter, View.SelectedManagerFilter, View.SelectedSalesRepresentativeFilter,
                       View.SelectedProductLineFilter, View.SelectedCategoryFilter, View.SelectedSubCategory1Filter, View.SelectedSubCategory2Filter, View.SelectedSubCategory3Filter, View.SelectedStartDate, View.SelectedEndDate, HttpContext.Current.User.Identity.Name);
            View.ListManualConsumptionReport = listReportData;
            return listReportData;
        }

        public void PopulateCategory()
        {
            View.CategoryList = new List<string>();
            View.SubCategory1List = new List<string>();
            View.SubCategory2List = new List<string>();
            View.SubCategory3List = new List<string>();
            var lstCategory = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, string.Empty, string.Empty, string.Empty, 0);
            View.CategoryList = lstCategory;
            PopulateSubCategory1();
            PopulateSubCategory2();
            PopulateSubCategory3();
        }

        public void PopulateSubCategory1()
        {
            View.SubCategory2List = new List<string>();
            View.SubCategory3List = new List<string>();
            var lstSubCategory1 = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, View.SelectedCategoryFilter, string.Empty, string.Empty, 1);
            View.SubCategory1List = lstSubCategory1;
            PopulateSubCategory2();
            PopulateSubCategory3();
        }

        public void PopulateSubCategory2()
        {
            View.SubCategory3List = new List<string>();
            var lstSubCategory2 = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, View.SelectedCategoryFilter, View.SelectedSubCategory1Filter, string.Empty, 2);
            View.SubCategory2List = lstSubCategory2;
            PopulateSubCategory3();
        }

        public void PopulateSubCategory3()
        {
            View.SubCategory3List = _eParPlusRepositoryService.FetchProductLinePartCategoryHierarchy(View.SelectedProductLineFilter, View.SelectedCategoryFilter, View.SelectedSubCategory1Filter, View.SelectedSubCategory2Filter, 3);
        }

        public bool RevertManualConsumption(string accountNumber, string tagId)
        {
            return _manualConsumptionRepositoryService.RevertManualConsumption(accountNumber, tagId, HttpContext.Current.User.Identity.Name);
        }

        #endregion Public Methods
    }
}
