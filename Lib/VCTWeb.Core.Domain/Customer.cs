using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	Customer  	
    /// Purpose		:	Domain class for table Customer  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	Apr 17 2015  2:07PM  		
    /// </summary>	

    [Serializable]    
    public class Customer
    {

        #region "Instance variables"

        private string _customerName;
        private string _accountNumber;
        private string _streetAddress;
        private string _city;
        private string _state;
        private string _zip;
        private string _ownershipStructure;
        private string _managementStructure;
        private string _spineOnlyMultiSpecialty;
        private int? _qtyOfORs;
        private string _branchAgency;
        private string _manager;
        private string _salesRepresentative;
        private string _specialistRep;
        private bool _isActive;
        private string _updatedBy;
        private DateTime _updatedOn;                
        private int _ConsumptionInterval;
        private int _AssetNearExpiryDays;
        private string _nameAccount;
        private string _productLines;
        #endregion

        #region "ctors"

        public Customer()
        {
            //write constructor logic here
        }
        #endregion

        #region "public Properties"

        [StringLengthValidator(1, 100, Ruleset = "Customer", MessageTemplate = "valCustomerName")]
        public string CustomerName
        {
            get
            {
                return _customerName;
            }
            set
            {
                if (_customerName != value)
                {
                    _customerName = value;

                }
            }
        }

        [StringLengthValidator(1, 100, Ruleset = "Customer", MessageTemplate = "valAccountNumber")]
        public string AccountNumber
        {
            get
            {
                return _accountNumber;
            }
            set
            {
                if (_accountNumber != value)
                {
                    _accountNumber = value;

                }
            }
        }

        [StringLengthValidator(1, 100, Ruleset = "Customer", MessageTemplate = "valStreetAddress")]
        public string StreetAddress
        {
            get
            {
                return _streetAddress;
            }
            set
            {
                if (_streetAddress != value)
                {
                    _streetAddress = value;

                }
            }
        }

        [StringLengthValidator(1, 100, Ruleset = "Customer", MessageTemplate = "valCity")]
        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                if (_city != value)
                {
                    _city = value;

                }
            }
        }

        [StringLengthValidator(1, 100, Ruleset = "Customer", MessageTemplate = "valState")]
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    _state = value;

                }
            }
        }

        [StringLengthValidator(1, 10, Ruleset = "Customer", MessageTemplate = "valZip")]
        public string Zip
        {
            get
            {
                return _zip;
            }
            set
            {
                if (_zip != value)
                {
                    _zip = value;

                }
            }
        }

        [StringLengthValidator(1, 010, Ruleset = "Customer", MessageTemplate = "valOwnershipStructure")]
        public string OwnershipStructure
        {
            get
            {
                return _ownershipStructure;
            }
            set
            {
                if (_ownershipStructure != value)
                {
                    _ownershipStructure = value;

                }
            }
        }

        [StringLengthValidator(1, 100, Ruleset = "Customer", MessageTemplate = "valManagementStructure")]
        public string ManagementStructure
        {
            get
            {
                return _managementStructure;
            }
            set
            {
                if (_managementStructure != value)
                {
                    _managementStructure = value;

                }
            }
        }

        public string SpineOnlyMultiSpecialty
        {
            get
            {
                return _spineOnlyMultiSpecialty;
            }
            set
            {
                if (_spineOnlyMultiSpecialty != value)
                {
                    _spineOnlyMultiSpecialty = value;

                }
            }
        }

        public int? QtyOfORs
        {
            get
            {
                return _qtyOfORs;
            }
            set
            {
                if (_qtyOfORs != value)
                {
                    _qtyOfORs = value;

                }
            }
        }

        [StringLengthValidator(1, 100, Ruleset = "Customer", MessageTemplate = "valBranchAgency")]
        public string BranchAgency
        {
            get
            {
                return _branchAgency;
            }
            set
            {
                if (_branchAgency != value)
                {
                    _branchAgency = value;

                }
            }
        }

        [StringLengthValidator(1, 100, Ruleset = "Customer", MessageTemplate = "valManager")]
        public string Manager
        {
            get
            {
                return _manager;
            }
            set
            {
                if (_manager != value)
                {
                    _manager = value;

                }
            }
        }

        [StringLengthValidator(1, 100, Ruleset = "Customer", MessageTemplate = "valSalesRepresentative")]
        public string SalesRepresentative
        {
            get
            {
                return _salesRepresentative;
            }
            set
            {
                if (_salesRepresentative != value)
                {
                    _salesRepresentative = value;
                }
            }
        }

        [StringLengthValidator(1, 100, Ruleset = "Customer", MessageTemplate = "valSpecialistRep")]
        public string SpecialistRep
        {
            get
            {
                return _specialistRep;
            }
            set
            {
                if (_specialistRep != value)
                {
                    _specialistRep = value;
                }
            }
        }
        
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;

                }
            }
        }

        public string UpdatedBy
        {
            get
            {
                return _updatedBy;
            }
            set
            {
                if (_updatedBy != value)
                {
                    _updatedBy = value;

                }
            }
        }

        public DateTime UpdatedOn
        {
            get
            {
                return _updatedOn;
            }
            set
            {
                if (_updatedOn != value)
                {
                    _updatedOn = value;

                }
            }
        }
              
        public int ConsumptionInterval
        {
            get
            {
                return _ConsumptionInterval;
            }
            set
            {
                if (_ConsumptionInterval != value)
                {
                    _ConsumptionInterval = value;

                }
            }
        }

        public int AssetNearExpiryDays
        {
            get
            {
                return _AssetNearExpiryDays;
            }
            set
            {
                if (_AssetNearExpiryDays != value)
                {
                    _AssetNearExpiryDays = value;

                }
            }
        }

        public string NameAccount
        {
            get
            {
                return _nameAccount;
            }
            set
            {
                if (_nameAccount != value)
                {
                    _nameAccount = value;

                }
            }
        }

        public string ProductLines
        {
            get
            {
                return _productLines;
            }
            set
            {
                if (_productLines != value)
                {
                    _productLines = value;
                }
            }
        }

        #endregion
    }
}


