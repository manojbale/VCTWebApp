
/****************************************************************************  
 * KitDetail.cs                                                          
 *                                                                            
 * Description:     Describes business logic for KitDetail.  
 *  
 * Author:          Sanjay Kumar
 * Date:            May/20/2013
 *                                                                            
 ****************************************************************************/

using System;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class KitTable
    {

        public string KitNumber { get; set; }
        public int? ItemNumber { get; set; }
        public string Catalognumber { get; set; }
        public string Description { get; set; }
        public string CaseLotCode { get; set; }
        public int? Sent { get; set; }
        public int? Creturn { get; set; }
        public int? Group { get; set; }
        public int? BOQty { get; set; }
        public bool IsManuallyAdded { get; set; }
        public int? Quantity { get; set; }

    }

    [Serializable]
    public class Catalog
    {
        public Catalog()
        { }

        public Catalog(string catalogNumber, string description)
        {
            this.CatalogNumber = catalogNumber;
            this.Description = description;
        }

        public string CatalogNumber { get; set; }
        public string Description { get; set; }
        public string CatalogFull { get; set; }
        public Int64 LocationPartDetailId { get; set; }
        public int Total { get; set; }
    }

    [Serializable]
    public class PartyAvailableCatalog
    {
        public string CatalogNumber { get; set; }
        public string LotNum { get; set; }
        public string Description { get; set; }
        public string CatalogFull { get; set; }
        public int AvailableQty { get; set; }
        public int TransferQty { get; set; }
    }
}

