using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Name		:	OrderAdjustment  	
    /// Purpose		:	Domain class for table OrderAdjustment  	
    /// Created By	:	Suraj Namdeo
    /// Created On	:	May 28 2015  5:41PM  		
    /// </summary
    public class OrderAdjustment
    {
        #region "Instance variables"

        private Int64 _orderAdjustmentId;
        private int _orderId;
        private int _dispositionTypeId;
        private string _dispositionType;
        private int _qty;        
        private string _remarks;
        private string _updatedBy;
        private DateTime _updatedOn;

        #endregion

        #region "ctors"

        public OrderAdjustment()
        {
            //write constructor logic here
        }
        #endregion


        #region "public Properties"


        public Int64 OrderAdjustmentId
        {
            get
            {
                return _orderAdjustmentId;
            }
            set
            {
                if (_orderAdjustmentId != value)
                {
                    _orderAdjustmentId = value;

                }
            }
        }

        public int OrderId
        {
            get
            {
                return _orderId;
            }
            set
            {
                if (_orderId != value)
                {
                    _orderId = value;

                }
            }
        }

        public int DispositionTypeId
        {
            get
            {
                return _dispositionTypeId;
            }
            set
            {
                if (_dispositionTypeId != value)
                {
                    _dispositionTypeId = value;

                }
            }
        }

        public string DispositionType
        {
            get
            {
                return _dispositionType;
            }
            set
            {
                if (_dispositionType != value)
                {
                    _dispositionType = value;
                }
            }
        }

        public int Qty
        {
            get
            {
                return _qty;
            }
            set
            {
                if (_qty != value)
                {
                    _qty = value;

                }
            }
        }
        
        public string Remarks
        {
            get
            {
                return _remarks;
            }
            set
            {
                if (_remarks != value)
                {
                    _remarks = value;

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






