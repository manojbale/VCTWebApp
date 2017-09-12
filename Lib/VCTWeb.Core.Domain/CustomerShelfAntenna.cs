using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    public class CustomerShelfAntenna
    {
        #region "Instance variables"

        private int _customerShelfAntennaId;
        private int _customerShelfId;
        private string _antennaName;
        private string _antennaDescription;

        #endregion

        #region "public Properties"

        public int CustomerShelfAntennaId
        {
            get
            {
                return _customerShelfAntennaId;
            }
            set
            {
                if (_customerShelfAntennaId != value)
                {
                    _customerShelfAntennaId = value;

                }
            }
        }

        public int CustomerShelfId
        {
            get
            {
                return _customerShelfId;
            }
            set
            {
                if (_customerShelfId != value)
                {
                    _customerShelfId = value;

                }
            }
        }

        public string AntennaName
        {
            get
            {
                return _antennaName;
            }
            set
            {
                if (_antennaName != value)
                {
                    _antennaName = value;

                }
            }
        }

        public string AntennaDescription
        {
            get
            {
                return _antennaDescription;
            }
            set
            {
                if (_antennaDescription != value)
                {
                    _antennaDescription = value;

                }
            }
        }

        #endregion
    }
}
