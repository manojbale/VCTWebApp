using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.IO;
using System.Data;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using VCTWeb.Core.Domain;

namespace VCTWebApp
{
    /// <summary>
    /// Summary description for VCTWebService
    /// </summary>
    [WebService(Namespace = "http://VCTWebApp.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class VCTWebService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<KitListing> GetKitsByKitNumber(string sKitNumber)
        {
            VCTWeb.Core.Domain.KitListingRepository kitListingRepository = new VCTWeb.Core.Domain.KitListingRepository();
            List<VCTWeb.Core.Domain.KitListing> lstKitListing = new List<VCTWeb.Core.Domain.KitListing>();
            lstKitListing = kitListingRepository.GetKitsByKitNumber(sKitNumber);
            return lstKitListing;
            /*
             [{ "id": "1", "label": "MyFirstHotel", "value": "MyFirstHotel" },{ "id": "2", "label": "MySecondHotel", "value": "MySecondHotel" }]
             */


            //string s = "{ \"d\" : " + new JavaScriptSerializer().Serialize(lstKits) + "}";
            //string s = "{" + new JavaScriptSerializer().Serialize(lstKits) + "}";
            //return s;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<KitListing> GetKitsByKitNumberOrDesc(string sKitNumber)
        {
            VCTWeb.Core.Domain.KitListingRepository kitListingRepository = new VCTWeb.Core.Domain.KitListingRepository();
            List<VCTWeb.Core.Domain.KitListing> lstKitListing = new List<VCTWeb.Core.Domain.KitListing>();
            lstKitListing = kitListingRepository.GetKitsByKitNumberOrDesc(sKitNumber, Convert.ToInt32(Session["LoggedInLocationId"]));
            return lstKitListing;
            /*
             [{ "id": "1", "label": "MyFirstHotel", "value": "MyFirstHotel" },{ "id": "2", "label": "MySecondHotel", "value": "MySecondHotel" }]
             */


            //string s = "{ \"d\" : " + new JavaScriptSerializer().Serialize(lstKits) + "}";
            //string s = "{" + new JavaScriptSerializer().Serialize(lstKits) + "}";
            //return s;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<KitListing> GetMappedKitsByKitNumberOrDesc(string sKitNumber)
        {
            VCTWeb.Core.Domain.KitListingRepository kitListingRepository = new VCTWeb.Core.Domain.KitListingRepository();
            List<VCTWeb.Core.Domain.KitListing> lstKitListing = new List<VCTWeb.Core.Domain.KitListing>();
            lstKitListing = kitListingRepository.GetMappedKitsByKitNumber(sKitNumber, Convert.ToInt32(Session["LoggedInLocationId"]));
            return lstKitListing;            
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<KitListing> GetKitsByKitName(string sKitName)
        {
            VCTWeb.Core.Domain.KitListingRepository kitListingRepository = new VCTWeb.Core.Domain.KitListingRepository();
            List<VCTWeb.Core.Domain.KitListing> lstKitListing = new List<VCTWeb.Core.Domain.KitListing>();
            lstKitListing = kitListingRepository.GetKitsByKitName(sKitName);
            return lstKitListing;
        }

       

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Catalog> GetCatalogByCatalogNumber(string sCatalogNumber)
        {
            List<string> result = new List<string>();
            VCTWeb.Core.Domain.KitTableRepository kitTableRepository = new VCTWeb.Core.Domain.KitTableRepository();
            List<VCTWeb.Core.Domain.Catalog> lstCatalog = new List<VCTWeb.Core.Domain.Catalog>();
            lstCatalog = kitTableRepository.GetCatalogByCatalogNumber(sCatalogNumber);
            //foreach (VCTWeb.Core.Domain.Catalog catalog in lstCatalog)
            //    result.Add(catalog.CatalogNumber);
            //return result;
            return lstCatalog;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Catalog> GetRMAPartsByPartNum(string sCatalogNumber)
        {
            List<string> result = new List<string>();
            VCTWeb.Core.Domain.KitTableRepository kitTableRepository = new VCTWeb.Core.Domain.KitTableRepository();
            List<VCTWeb.Core.Domain.Catalog> lstCatalog = new List<VCTWeb.Core.Domain.Catalog>();

            Int64 PartyId = 0;
            Int32 LocationId = 0;
            string LocDetail = Convert.ToString(Session["RMALocationId"]);
            if (LocDetail.Contains("_"))
            {
                string[] arr = LocDetail.Split('_');
                if (arr[0].ToLower() == "party") PartyId = Convert.ToInt64(arr[1]); else LocationId = Convert.ToInt32(arr[1]);

                Int64 CaseShipFromLocationId = Convert.ToInt64(arr[2]);
                lstCatalog = kitTableRepository.GetRMAPartsByPartNum(sCatalogNumber, PartyId, LocationId, CaseShipFromLocationId);            
            }
         
            return lstCatalog;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Cases> GetRMACasesByCaseNum(string sCaseNumber)
        {            
            VCTWeb.Core.Domain.KitTableRepository kitTableRepository = new VCTWeb.Core.Domain.KitTableRepository();
            List<VCTWeb.Core.Domain.Cases> lstCases = new List<VCTWeb.Core.Domain.Cases>();

            Int64 PartyId = 0;
            Int32 LocationId = 0;
            string LocDetail = Convert.ToString(Session["RMALocationId"]);
            if (LocDetail.Contains("_"))
            {
                string[] arr = LocDetail.Split('_');
                if (arr[0].ToLower() == "party") PartyId = Convert.ToInt64(arr[1]); else LocationId = Convert.ToInt32(arr[1]);

                Int64 CaseShipFromLocationId = Convert.ToInt64(arr[2]);

                lstCases = kitTableRepository.GetRMACasesByCaseNum(sCaseNumber, PartyId, LocationId, CaseShipFromLocationId);
            }
            

            return lstCases;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Party> GetPartyByPartyName(string sPartyName)
        {
            List<string> result = new List<string>();
            VCTWeb.Core.Domain.PartyRepository partyRepository = new VCTWeb.Core.Domain.PartyRepository();
            List<VCTWeb.Core.Domain.Party> lstParty = new List<VCTWeb.Core.Domain.Party>();
            lstParty = partyRepository.GetPartyByPartyName(sPartyName);
            return lstParty;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Procedures> GetProceduresByProcedureName1(string sProcedureName, string sPhysicianName)
        {
            List<string> result = new List<string>();
            VCTWeb.Core.Domain.ProceduresRepository procedureRepository = new VCTWeb.Core.Domain.ProceduresRepository();
            List<VCTWeb.Core.Domain.Procedures> lstProcedure = new List<VCTWeb.Core.Domain.Procedures>();

            //int iPhysisicanId=0;
            //if (sPhysicianId != string.Empty)
            //{
            //    iPhysisicanId = Convert.ToInt32(sPhysicianId);
            //}

            lstProcedure = procedureRepository.GetProceduresByProcedureName(sProcedureName, sPhysicianName);
            return lstProcedure;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Procedures> GetProceduresByProcedureName(string sProcedureName)
        {
            List<string> result = new List<string>();
            VCTWeb.Core.Domain.ProceduresRepository procedureRepository = new VCTWeb.Core.Domain.ProceduresRepository();
            List<VCTWeb.Core.Domain.Procedures> lstProcedure = new List<VCTWeb.Core.Domain.Procedures>();
            //lstProcedure = procedureRepository.GetProceduresByProcedureName(sProcedureName, sSurgeonName);
            lstProcedure = procedureRepository.GetProceduresByProcedureName(sProcedureName,string.Empty);
            return lstProcedure;
        }



        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<Physician> GetPhysicianByPartyId(string sPhysicianName,string sPartyId)
        {
            List<string> result = new List<string>();
            VCTWeb.Core.Domain.ProceduresRepository procedureRepository = new VCTWeb.Core.Domain.ProceduresRepository();
            List<VCTWeb.Core.Domain.Physician> lstPhysician = new List<VCTWeb.Core.Domain.Physician>();
            long lPartyId=0;
            if (sPartyId != string.Empty)
            {
                lPartyId = Convert.ToInt64(sPartyId);
            }

            lstPhysician = procedureRepository.GetPhysicianByPartyName(sPhysicianName, lPartyId);
            return lstPhysician;
        }

        //public IEnumerable<VCTWeb.Core.Domain.Procedures> lstProcedure { get; set; }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<ViewCancelTransaction> GetTransactionsByFilter(string FilterBy, string CaseNumber, string StartDate, string EndDate, string CaseType, string InvType, string PartyName, string LocationType, string CaseStatus)
        {
            VCTWeb.Core.Domain.CaseRepository caseRepository = new VCTWeb.Core.Domain.CaseRepository();
            List<VCTWeb.Core.Domain.ViewCancelTransaction> lstTransactions = new List<VCTWeb.Core.Domain.ViewCancelTransaction>();
            DateTime SDate = Convert.ToDateTime(StartDate + " 00:00:00");
            DateTime EDate = Convert.ToDateTime(EndDate + " 23:59:59");
            lstTransactions = caseRepository.GetCasesListByCaseType(Convert.ToInt32(Session["LoggedInLocationId"]), SDate, EDate, CaseType, CaseNumber, InvType, PartyName, LocationType, CaseStatus,0,int.MaxValue);

            if (FilterBy == "ShipToLocation")
            {
                var lstbyShipToLocation = lstTransactions.GroupBy(x => x.PartyName, (key, group) => group.First()).ToList<ViewCancelTransaction>();
                return lstbyShipToLocation;
            }

            return lstTransactions;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<ViewCancelTransaction> GetVCTLocationsByFilter(string PartyName)
        {
            VCTWeb.Core.Domain.CaseRepository caseRepository = new VCTWeb.Core.Domain.CaseRepository();
            List<VCTWeb.Core.Domain.ViewCancelTransaction> lstTransactions = new List<VCTWeb.Core.Domain.ViewCancelTransaction>();

            lstTransactions = caseRepository.GetVCTLocationsByFilter(Convert.ToInt32(Session["LoggedInLocationId"]), PartyName);

            return lstTransactions;
        }        

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<VCTWeb.Core.Domain.KitFamily> GetKitFamilyByNumber(string sKitFamily)
        {
            VCTWeb.Core.Domain.CaseRepository caseRepository = new VCTWeb.Core.Domain.CaseRepository();
            List<VCTWeb.Core.Domain.KitFamily> KitFamilyList = new List<VCTWeb.Core.Domain.KitFamily>();

            if (Convert.ToString(HttpContext.Current.Session["LoggedInLocationType"]).ToUpper() == "AREA")
            {
                KitFamilyList = caseRepository.GetKitFamilyByNumber(sKitFamily);
            }
            else
            {
                KitFamilyList = caseRepository.GetKitFamilyByNumberAndLocation(sKitFamily, Convert.ToInt32(Session["LoggedInLocationId"]));
            }
            

            return KitFamilyList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<VCTWeb.Core.Domain.KitFamilyInventoryTransfer> GetKitFamilyByLocationAndNumber(string sKitFamily)
        {
            VCTWeb.Core.Domain.CaseRepository caseRepository = new VCTWeb.Core.Domain.CaseRepository();
            List<VCTWeb.Core.Domain.KitFamilyInventoryTransfer> KitFamilyList = new List<VCTWeb.Core.Domain.KitFamilyInventoryTransfer>();

            KitFamilyList = caseRepository.GetKitFamilyByLocationAndNumber(sKitFamily, Convert.ToInt32(Session["LoggedInLocationId"]));
            
            return KitFamilyList;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<PartyAvailableCatalog> GetCatalogCountByCatalogNumber(string sCatalogNumber)
        {
            List<string> result = new List<string>();
            VCTWeb.Core.Domain.KitTableRepository kitTableRepository = new VCTWeb.Core.Domain.KitTableRepository();
            List<VCTWeb.Core.Domain.PartyAvailableCatalog> lstCatalog = kitTableRepository.GetCatalogCountByCatalogNumber(Convert.ToInt32(Session["LoggedInLocationId"]), Convert.ToInt64(Session["FromPartyId"]), sCatalogNumber);            
            return lstCatalog;
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<PartCatalog> FetchPartCatalogByPartNum(string RefNum)
        {
            List<string> result = new List<string>();
            VCTWeb.Core.Domain.ProductLinePartDetailRepository ProductLinePartDetailRepository = new VCTWeb.Core.Domain.ProductLinePartDetailRepository();
            List<VCTWeb.Core.Domain.PartCatalog> lstPartCatalog = new List<VCTWeb.Core.Domain.PartCatalog>();
            lstPartCatalog = ProductLinePartDetailRepository.FetchPartCatalogByPartNum(RefNum);
            return lstPartCatalog;
        }

    }
}

