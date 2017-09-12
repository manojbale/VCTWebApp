using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	ProductLine  	
    /// Purpose		:	Domain class for table ProductLine  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	May 11 2015  3:50PM  		
    /// </summary>	
    [Serializable]
    public class ProductLine
    {
        #region "Instance variables"

        private string _productLineName;
        private string _productLineDesc;
        private string _updatedBy;
        private DateTime _updatedOn;

        #endregion

        #region "ctors"

        public ProductLine()
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

        #endregion
    }
}






