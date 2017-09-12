using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	ProductLinePartDetail  	
    /// Purpose		:	Domain class for table ProductLinePartDetail  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	Apr 28 2015  1:53PM  		

    [Serializable]
    public class ProductLinePartDetail
    {
        #region "Instance variables"

        private string _productLineName;
        private string _productLineDesc;
        private string _refNum;
        private string _category;
        private string _subCategory1;
        private string _subCategory2;
        private string _subCategory3;
        private string _size;
        private Int16 _defaultPARLevel;
        private string _updatedBy;
        private DateTime _updatedOn;

        private string _partType;

        #endregion

        #region "ctors"

        public ProductLinePartDetail()
        {
            //write constructor logic here
        }
        #endregion


        #region "public Properties"

        public string ProductLineName
        {
            get
            {
                return _productLineName;
            }
            set
            {
                if (_productLineName != value)
                {
                    _productLineName = value;

                }
            }
        }

        public string ProductLineDesc
        {
            get
            {
                return _productLineDesc;
            }
            set
            {
                if (_productLineDesc != value)
                {
                    _productLineDesc = value;

                }
            }
        }

        public string RefNum
        {
            get
            {
                return _refNum;
            }
            set
            {
                if (_refNum != value)
                {
                    _refNum = value;

                }
            }
        }

        public string Category
        {
            get
            {
                return _category;
            }
            set
            {
                if (_category != value)
                {
                    _category = value;

                }
            }
        }

        public string SubCategory1
        {
            get
            {
                return _subCategory1;
            }
            set
            {
                if (_subCategory1 != value)
                {
                    _subCategory1 = value;

                }
            }
        }

        public string SubCategory2
        {
            get
            {
                return _subCategory2;
            }
            set
            {
                if (_subCategory2 != value)
                {
                    _subCategory2 = value;

                }
            }
        }

        public string SubCategory3
        {
            get
            {
                return _subCategory3;
            }
            set
            {
                if (_subCategory3 != value)
                {
                    _subCategory3 = value;

                }
            }
        }

        public string Size
        {
            get
            {
                return _size;
            }
            set
            {
                if (_size != value)
                {
                    _size = value;

                }
            }
        }

        public Int16 DefaultPARLevel
        {
            get
            {
                return _defaultPARLevel;
            }
            set
            {
                if (_defaultPARLevel != value)
                {
                    _defaultPARLevel = value;

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

        public string PartType
        {
            get
            {
                return _partType;
            }
            set
            {
                if (_partType != value)
                {
                    _partType = value;

                }
            }
        }

        #endregion
    }

    [Serializable]
    public class PartCatalog
    {
        public string RefNum { get; set; }
        public string Description { get; set; }
        public string CatalogFull { get; set; }
    }
}






