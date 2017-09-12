using System;

using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public abstract class BaseEntityClass
    {
        private bool _isModified = false;
        private bool _isNew = true;

        public bool IsModified
        {
            get { return _isModified; }
            set { _isModified = value; }

        }

        public bool IsNew
        {
            get { return _isNew; }
            set { _isNew = value; }
        }
    }
}
