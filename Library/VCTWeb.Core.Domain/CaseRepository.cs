using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Globalization;
using System.Web.Configuration;

namespace VCTWeb.Core.Domain
{
    public class CaseRepository
    {
        private string _user;

        public CaseRepository()
        {
        }

        public CaseRepository(string user)
        {
            _user = user;
        }

        public List<CaseStatusCls> FetchAllCaseStatus()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<CaseStatusCls> lstCaseStatus = new List<CaseStatusCls>();
            CaseStatusCls newCaseStatus = new CaseStatusCls();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCaseStatus))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCaseStatus = LoadCaseStatus(reader);
                        lstCaseStatus.Add(newCaseStatus);
                    }
                }
                return lstCaseStatus;
            }
        }

        private CaseStatusCls LoadCaseStatus(SafeDataReader reader)
        {
            CaseStatusCls newCaseStatus = new CaseStatusCls();
            newCaseStatus.CaseStatus = reader.GetString("CaseStatus");
            newCaseStatus.Description = reader.GetString("Description");
            return newCaseStatus;
        }

        public List<CaseType> FetchAllCaseType()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<CaseType> lstCaseType = new List<CaseType>();
            CaseType newCaseType = new CaseType();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCaseType))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCaseType = LoadCaseType(reader);
                        lstCaseType.Add(newCaseType);
                    }
                }
                return lstCaseType;
            }
        }

        public List<string> GetLocationTypes()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<string> lstLocationType = new List<string>();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLocationTypes))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstLocationType.Add(reader.GetString("Name"));
                    }
                }
            }

            return lstLocationType;
        }


        public List<string> GetLocationAndPartyTypes()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<string> lstLocationType = new List<string>();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLocationAndPartyTypes))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstLocationType.Add(reader.GetString("Name"));
                    }
                }
            }

            return lstLocationType;
        }

        public int GetGridPageSize()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            int PageSize = 0;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GET_DICTIONARY_RULE))
            {
                db.AddInParameter(cmd, "@KeyName", DbType.String, "GridPageSize");

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        PageSize = int.Parse(reader.GetString("KeyValue"));
                    }
                }
            }

            return PageSize;
        }

        public List<ReturnInventoryRMA> GetReturnInventoryRMADetailByCaseNum(string CaseNum, Int32 LocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ReturnInventoryRMA> lstRMA = new List<ReturnInventoryRMA>();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetRMAKitDetailsByCaseId))
            {
                db.AddInParameter(cmd, "@CaseNumber", DbType.String, CaseNum);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstRMA.Add(new ReturnInventoryRMA()
                        {
                            CaseId = reader.GetInt64("CaseId"),
                            CasePartId = reader.GetInt64("CasePartId"),
                            KitNumber = reader.GetString("KitNumber"),
                            PartNum = reader.GetString("PartNum"),
                            LotNum = reader.GetString("LotNum"),
                            Description = reader.GetString("Description"),
                            PartStatus = reader.GetString("PartStatus"),
                            LocationPartDetailId = reader.GetInt64("LocationPartDetailId"),
                            SeekReturn = reader.GetBoolean("SeekReturn"),
                            DispositionTypeId = reader.GetInt32("DispositionTypeId")
                        });
                    }
                }
                return lstRMA;
            }
        }

        //public List<ReturnInventoryRMA> GetReturnInventoryRMADetailByPartNum(string PartNum)
        //{
        //    SafeDataReader reader = null;
        //    Database db = DbHelper.CreateDatabase();
        //    List<ReturnInventoryRMA> lstRMA = new List<ReturnInventoryRMA>();

        //    using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetRMAKitDetailsByCaseId))
        //    {
        //        db.AddInParameter(cmd, "@CaseNumber", DbType.String, PartNum);
        //        using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
        //        {
        //            while (reader.Read())
        //            {
        //                lstRMA.Add(new ReturnInventoryRMA()
        //                {
        //                    CaseId = reader.GetInt64("CaseId"),
        //                    PartNum = reader.GetString("PartNum"),
        //                    LotNum = reader.GetString("LotNum"),
        //                    Description = reader.GetString("Description"),
        //                    PartStatus = reader.GetString("PartStatus"),
        //                    LocationPartDetailId = reader.GetInt64("LocationPartDetailId")
        //                });
        //            }
        //        }
        //        return lstRMA;
        //    }
        //}

        public List<DispositionType> GetDespositionTypeByCategory(string DispositionCategory)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<DispositionType> lstDispositionType = new List<DispositionType>();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetDispositionTypesByCategory))
            {
                db.AddInParameter(cmd, "@DispositionCategory", DbType.String, DispositionCategory);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstDispositionType.Add(new DispositionType()
                        {
                            DispositionTypeId = reader.GetInt32("DispositionTypeId"),
                            Disposition = reader.GetString("DispositionType")
                        });
                    }
                }
                return lstDispositionType;
            }
        }

        public List<KeyValuePair<string, string>> GetCasesListByPartAndLotNum(int LocationId, string PartNum, string LotNum, Int64 PartyId)
        {
            string Key;
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KeyValuePair<string, string>> lstCases = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> oModel;
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCaseListByPartAndLotNum))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@PartNum", DbType.String, PartNum);
                db.AddInParameter(cmd, "@LotNum", DbType.String, LotNum);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, PartyId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        Key = reader.GetInt64("CasePartId") + "_" + reader.GetInt64("LocationPartDetailId");
                        //Key = Convert.ToString(reader.GetInt64("CasePartId"));
                        oModel = new KeyValuePair<string, string>(Key, reader.GetString("CaseNumber"));
                        lstCases.Add(oModel);
                    }
                }
                return lstCases;
            }
        }

        public List<Party> GetPartyByLocation(Int32 LocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Party> lstParty = new List<Party>();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPartyByLocation))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstParty.Add(new Party()
                        {
                            PartyId = reader.GetInt64("PartyId"),
                            Name = reader.GetString("Name")
                        });
                    }
                }
                return lstParty;
            }
        }

        public bool SaveReturnInventoryRMA(string InventoryType, string CaseNum, string ReturnFrom, Int64 PartyId, Int32 ToLocationId, Int32 LocationId, string TableXML, string _user, out string ResultCaseNum)
        {
            bool result = false;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveReturnInventoryRMA))
            {
                db.AddInParameter(cmd, "@InventoryType", DbType.String, InventoryType);
                db.AddInParameter(cmd, "@CaseNumber", DbType.String, CaseNum);
                db.AddInParameter(cmd, "@ReturnFrom", DbType.String, ReturnFrom);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, PartyId);
                db.AddInParameter(cmd, "@ToLocationId", DbType.Int32, ToLocationId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@TableXML", DbType.String, TableXML);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddOutParameter(cmd, "@RecordStatus", DbType.Boolean, 1);

                ResultCaseNum = Convert.ToString(db.ExecuteScalar(cmd));

                result = Convert.ToBoolean(db.GetParameterValue(cmd, "@RecordStatus"));
                
            }

            return result;
        }

        public List<Region> GetRegionList(int LocationId)
        {
            SafeDataReader reader = null;            
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLocationsByLocationType))
            {
                db.AddInParameter(cmd, "@LocationType", DbType.String, "Region");
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                List<Region> lstRegion = new List<Region>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstRegion.Add(new Region()
                        {
                            RegionId = reader.GetInt32("LocationId"),
                            RegionName = reader.GetString("LocationName")
                        });
                    }

                }
                return lstRegion;
            }
        }

        public List<Corp> GetCorpList(int LocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLocationsByLocationType))
            {
                db.AddInParameter(cmd, "@LocationType", DbType.String, "Area");
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                List<Corp> lstModel = new List<Corp>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstModel.Add(new Corp()
                        {
                            LocationId = reader.GetInt32("LocationId"),
                            LocationName = reader.GetString("LocationName")
                        });
                    }

                }
                return lstModel;
            }
        }

        private CaseType LoadCaseType(SafeDataReader reader)
        {
            CaseType newCaseType = new CaseType();
            newCaseType.CaseTypeName = reader.GetString("CaseTypeName");
            return newCaseType;
        }

        public bool SaveCase(Cases CaseToBeSaved, string CaseKitFamilyDetailXml, string CasePartDetailXml)
        {
            bool returnvalue = false;
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveCase))
            {
                db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
                db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
                db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
                db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseToBeSaved.CaseId);
                db.AddInParameter(cmd, "@SurgeryDate", DbType.Date, CaseToBeSaved.SurgeryDate);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, CaseToBeSaved.PartyId);
                db.AddInParameter(cmd, "@ProcedureName", DbType.String, CaseToBeSaved.ProcedureName);
                //db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, CaseToBeSaved.KitFamilyId);
                db.AddInParameter(cmd, "@PatientName", DbType.String, CaseToBeSaved.PatientName);
                db.AddInParameter(cmd, "@Physician", DbType.String, CaseToBeSaved.Physician);
                db.AddInParameter(cmd, "@SpecialInstructions", DbType.String, CaseToBeSaved.SpecialInstructions);
                db.AddInParameter(cmd, "@SalesRep", DbType.String, CaseToBeSaved.SalesRep);
                //db.AddInParameter(cmd, "@Technicians", DbType.String, CaseToBeSaved.Technicians);
                db.AddInParameter(cmd, "@InventoryType", DbType.String, CaseToBeSaved.InventoryType);
                db.AddInParameter(cmd, "@CaseType", DbType.String, CaseToBeSaved.CaseType);
                db.AddInParameter(cmd, "@TotalPrice", DbType.Decimal, CaseToBeSaved.TotalPrice);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, CaseToBeSaved.LocationId);
                //db.AddInParameter(cmd, "@Quantity", DbType.Int32, CaseToBeSaved.Quantity);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.AddInParameter(cmd, "@CaseKitFamilyDetailXml", DbType.String, CaseKitFamilyDetailXml);
                db.AddInParameter(cmd, "@CasePartDetailXml", DbType.String, CasePartDetailXml);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }

        public Cases FetchCaseById(long CaseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            Cases newCase = new Cases();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCaseByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newCase = LoadCase(reader);
                    }

                }
                return newCase;
            }
        }

        private Cases LoadCase(SafeDataReader reader)
        {
            Cases newCase = new Cases();
            newCase.CaseId = reader.GetInt64("CaseId");
            newCase.SurgeryDate = reader.GetNullableDateTime("SurgeryDate");
            newCase.ShippingDate = reader.GetNullableDateTime("ShippingDate");
            newCase.RetrievalDate = reader.GetNullableDateTime("RetrievalDate");
            newCase.PartyId = reader.GetInt64("PartyId");
            newCase.ProcedureName = reader.GetString("ProcedureName");
            //newCase.KitFamilyId = reader.GetInt64("KitFamilyId");
            newCase.PatientName = reader.GetString("PatientName");
            newCase.Physician = reader.GetString("Physician");
            newCase.SpecialInstructions = reader.GetString("SpecialInstructions");
            newCase.SalesRep = reader.GetString("SalesRep");
            newCase.CaseStatus = reader.GetString("CaseStatus");
            newCase.CaseType = reader.GetString("CaseType");
            newCase.Technicians = reader.GetString("Technicians");
            decimal temp = reader.GetDecimal("TotalPrice");
            newCase.TotalPrice = Convert.ToDouble(temp);
            newCase.LocationId = reader.GetInt32("LocationId");
            newCase.PartyName = reader.GetString("PartyName");
            newCase.PartyType = reader.GetString("PartyType");
            newCase.ShipFromLocation = reader.GetString("ShipFromLocation");
            //newCase.KitFamilyName = reader.GetString("KitFamilyName");
            //newCase.KitFamilyDesc = reader.GetString("KitFamilyDescription");
            newCase.CaseNumber = reader.GetString("CaseNumber");
            newCase.InventoryType = reader.GetString("InventoryType");
            newCase.Quantity = reader.GetNullableInt32("Quantity");

            return newCase;
        }

        public List<CaseInvoiceAdvisory> GetInvoiceAdvisoryByCaseId(long CaseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<CaseInvoiceAdvisory> lstCaseInvoiceAdvisory = new List<CaseInvoiceAdvisory>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetInvoiceAdvisoryByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, CaseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstCaseInvoiceAdvisory.Add(new CaseInvoiceAdvisory()
                        {
                            InventoryType = reader.GetString("InventoryType"),
                            Particular = reader.GetString("Particular"),
                            Description = reader.GetString("Description"),
                            Status = reader.GetString("Status"),
                            Amount = reader.GetDecimal("Amount")
                        });
                    }

                }
                return lstCaseInvoiceAdvisory;
            }
        }

        public List<CaseMerge> FetchCasesByFilter(DateTime startDate, DateTime endDate, int locationId, string SalesRep, string Procedure, Int64? Party, string CaseStatus, string CaseType, string Physician, Int64 KitFamilyId)
        {
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            CaseMerge newCaseMerge = null;
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetListOfCasesByFilter))
            {
                List<CaseMerge> listOfCaseMerge = new List<CaseMerge>();
                db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
                db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
                db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
                db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
                db.AddInParameter(cmd, "@StartDate", DbType.Date, startDate);
                db.AddInParameter(cmd, "@EndDate", DbType.Date, endDate);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "@SalesRep", DbType.String, SalesRep);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, Party);
                db.AddInParameter(cmd, "@Procedure", DbType.String, Procedure);
                db.AddInParameter(cmd, "@Physician", DbType.String, Physician);
                db.AddInParameter(cmd, "@CaseStatus", DbType.String, CaseStatus);
                db.AddInParameter(cmd, "@CaseType", DbType.String, CaseType);
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, KitFamilyId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCaseMerge = LoadCaseMerge(reader);
                        listOfCaseMerge.Add(newCaseMerge);
                    }

                }
                return listOfCaseMerge;
            }
        }

        private CaseMerge LoadCaseMerge(SafeDataReader reader)
        {
            CaseMerge newCaseMerge = new CaseMerge();
            newCaseMerge.CaseValues = reader.GetString("CaseValues");
            newCaseMerge.SurgeryDate = reader.GetDateTime("SurgeryDate");

            return newCaseMerge;
        }

        private KitListing LoadCaseKits(SafeDataReader reader)
        {
            KitListing newCaseKit = new KitListing();
            newCaseKit.KitNumber = reader.GetString("KitNumber");
            newCaseKit.KitName = reader.GetString("KitName");
            return newCaseKit;
        }

        public List<Physician> FetchAllPhysician()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Physician> lstPhysician = new List<Physician>();
            Physician physician = new Physician();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPhysicians))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        physician = LoadPhysician(reader);
                        lstPhysician.Add(physician);
                    }
                }
                return lstPhysician;
            }
        }

        private Physician LoadPhysician(SafeDataReader reader)
        {
            Physician physician = new Physician();
            physician.PhysicianName = reader.GetString("PhysicianName");

            return physician;
        }


        public bool CancelCase(long caseId)
        {
            bool returnvalue = false;
            System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(Encryption.Decrypt(WebConfigurationManager.AppSettings["LinkedServerConnectionString"].ToString()));
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_CancelCase))
            {
                db.AddInParameter(cmd, "@ServerName", DbType.String, builder.DataSource);
                db.AddInParameter(cmd, "@UserName", DbType.String, builder.UserID);
                db.AddInParameter(cmd, "@Password", DbType.String, builder.Password);
                db.AddInParameter(cmd, "@InitialCatalog", DbType.String, builder.InitialCatalog);
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.ExecuteScalar(cmd);
                returnvalue = true;
            }
            return returnvalue;
        }

        public List<CaseSmall> FetchAllPendingCasesByCaseType(string caseType, int locationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<CaseSmall> PendingCasesList = new List<CaseSmall>();
            CaseSmall pendingCase = new CaseSmall();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_FetchAllPendingCasesByCaseType))
            {
                db.AddInParameter(cmd, "@CaseType", DbType.String, caseType);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        pendingCase = LoadCaseSmall(reader);
                        PendingCasesList.Add(pendingCase);
                    }
                }
                return PendingCasesList;
            }
        }

        public List<KitFamilSmall> GetKitFamilyByCaseId(long caseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitFamilSmall> KitFamilSmallList = new List<KitFamilSmall>();
            KitFamilSmall newKitFamilSmall = new KitFamilSmall();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newKitFamilSmall = LoadKitFamilSmall(reader);
                        KitFamilSmallList.Add(newKitFamilSmall);
                    }
                }
                return KitFamilSmallList;
            }
        }

        private KitFamilSmall LoadKitFamilSmall(SafeDataReader reader)
        {
            KitFamilSmall newKitFamilSmall = new KitFamilSmall();
            newKitFamilSmall.KitFamilyName = reader.GetString("KitFamilyName");
            newKitFamilSmall.KitNumber = reader.GetString("KitNumber");
            newKitFamilSmall.BuildKitId = reader.GetInt64("BuildKitId");
            newKitFamilSmall.KitFamilyId = reader.GetInt64("KitFamilyId");
            return newKitFamilSmall;
        }

        private CaseSmall LoadCaseSmall(SafeDataReader reader)
        {
            CaseSmall newCase = new CaseSmall();
            newCase.CaseId = reader.GetInt64("CaseId");
            newCase.CaseNumber = reader.GetString("CaseNumber");

            return newCase;
        }

        public string SendRequestForCase(long caseId, int requestedQuantity, int locationId)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SENDREQUESTFORCASE))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                db.AddInParameter(cmd, "@RequestedQuantity", DbType.Int32, requestedQuantity);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                return Convert.ToString(db.ExecuteScalar(cmd));
            }
        }

        public List<ItemDetail> GetCasePartDetailByCaseId(long caseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ItemDetail> lstItemDetail = new List<ItemDetail>();
            ItemDetail newItemDetail = new ItemDetail();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCasePartDetailByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newItemDetail = LoadItemDetail(reader);
                        lstItemDetail.Add(newItemDetail);
                    }

                }
                return lstItemDetail;
            }
        }

        public List<KitHistoryCaseDetail> GetKitHistoryByKitNumberAndLocationId(string kitNumber, int locationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitHistoryCaseDetail> lstKitHistoryCaseDetail = new List<KitHistoryCaseDetail>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitHistoryByKitNumberAndLocationId))
            {
                db.AddInParameter(cmd, "@KitNumber", DbType.String, kitNumber);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, locationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstKitHistoryCaseDetail.Add(new KitHistoryCaseDetail()
                        {
                            SurgeryDate = reader.GetDateTime("SurgeryDate"),
                            CaseNumber = reader.GetString("CaseNumber"),
                            PartyName = reader.GetString("PartyName"),
                            ProcedureName = reader.GetString("ProcedureName"),
                            SalesRep = reader.GetString("SalesRep"),
                            CaseStatus = reader.GetString("CaseStatus"),
                            PartNum = reader.GetString("PartNum"),
                            LotNum = reader.GetString("LotNum"),
                            Description = reader.GetString("Description"),
                            ExpiryDate = reader.GetDateTime("ExpiryDate")
                        });
                    }

                }
                return lstKitHistoryCaseDetail;
            }
        }

        private ItemDetail LoadItemDetail(SafeDataReader reader)
        {
            ItemDetail newItemDetail = new ItemDetail();

            newItemDetail.CasePartId = reader.GetInt64("CasePartId");
            newItemDetail.PartNum = reader.GetString("PartNum");
            newItemDetail.LotNum = reader.GetString("LotNum");
            newItemDetail.LocationPartDetailId = reader.GetInt64("LocationPartDetailId");
            return newItemDetail;
        }

        public List<CasePartDetailGroup> GetGroupedCasePartDetailByCaseId(long caseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<CasePartDetailGroup> lstCasePartDetailGroup = new List<CasePartDetailGroup>();
            CasePartDetailGroup newCasePartDetailGroup = new CasePartDetailGroup();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetGroupedCasePartDetailByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCasePartDetailGroup = LoadCasePartDetailGroup(reader);
                        lstCasePartDetailGroup.Add(newCasePartDetailGroup);
                    }

                }
                return lstCasePartDetailGroup;
            }
        }

        public List<CaseKitFamilyDetailGroup> GetGroupedCaseKitFamilyDetailByCaseId(long caseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<CaseKitFamilyDetailGroup> lstCaseKitFamilyDetailGroup = new List<CaseKitFamilyDetailGroup>();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetGroupedCaseKitFamilyDetailByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstCaseKitFamilyDetailGroup.Add(new CaseKitFamilyDetailGroup() {
                            KitFamilyId = reader.GetInt64("KitFamilyId"),
                            KitFamilyDescription = reader.GetString("KitFamilyDescription"),
                            KitFamilyName = reader.GetString("KitFamilyName"),
                            Quantity = reader.GetInt32("Quantity"),
                            Selected = reader.GetBoolean("Selected")
                        });
                    }

                }
                return lstCaseKitFamilyDetailGroup;
            }
        }

        public List<CasePartDetailGroup> GetKitFamilyItemsByCaseId(long KitFamilyId, long caseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<CasePartDetailGroup> lstCasePartDetailGroup = new List<CasePartDetailGroup>();
            CasePartDetailGroup newCasePartDetailGroup = new CasePartDetailGroup();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyItemsByCaseId))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, KitFamilyId);
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newCasePartDetailGroup = LoadCasePartDetailGroup(reader);
                        lstCasePartDetailGroup.Add(newCasePartDetailGroup);
                    }

                }
                return lstCasePartDetailGroup;
            }
        }

        private CasePartDetailGroup LoadCasePartDetailGroup(SafeDataReader reader)
        {
            CasePartDetailGroup newCasePartDetailGroup = new CasePartDetailGroup();

            newCasePartDetailGroup.PartNum = reader.GetString("PartNum");
            newCasePartDetailGroup.Description = reader.GetString("Description");
            newCasePartDetailGroup.Quantity = reader.GetInt32("Quantity");
            newCasePartDetailGroup.Selected = reader.GetBoolean("Selected");

            return newCasePartDetailGroup;
        }

        public bool SaveNewProductTransfer(Int64 KitFamilyId, int LocationId, DateTime TransDate, string CreateKitLocationXML, string CreateKitPartsXML, out string result)
        {
            bool returnValue = false;
            result = "";
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SaveNewProductTransfer))
            {
                db.AddInParameter(cmd, "@KitFamilyId", DbType.Int64, KitFamilyId);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@TransDate", DbType.DateTime, TransDate);
                db.AddInParameter(cmd, "@KitLocationXML", DbType.String, CreateKitLocationXML);
                db.AddInParameter(cmd, "@KitPartsXML", DbType.String, CreateKitPartsXML);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);

                result = Convert.ToString(db.ExecuteScalar(cmd));

                returnValue = true;
            }

            return returnValue;
        }


        public List<ViewCancelTransaction> GetCasesSummaryByCaseType(string CaseType, DateTime StartDate, DateTime EndDate, Int32 LocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ViewCancelTransaction> lstCaseTypes;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCasesSummaryByCaseType))
            {
                db.AddInParameter(cmd, "@CaseType", DbType.String, CaseType);
                db.AddInParameter(cmd, "@StartDate", DbType.DateTime, StartDate);
                db.AddInParameter(cmd, "@EndDate", DbType.DateTime, EndDate);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);


                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCaseTypes = new List<ViewCancelTransaction>();
                    while (reader.Read())
                    {
                        lstCaseTypes.Add(LoadCaseSummary(reader));
                    }

                }
                return lstCaseTypes;
            }
        }

        public ViewCancelTransaction LoadCaseSummary(SafeDataReader reader)
        {
            ViewCancelTransaction oCaseType = new ViewCancelTransaction();
            oCaseType.CaseStatus = reader.GetString("CaseStatus");
            oCaseType.Count = reader.GetInt32("Count");
            return oCaseType;
        }

        public List<ViewCancelTransaction> GetCaseItemsListByCaseId(Int64 CaseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ViewCancelTransaction> lstCaseTypes;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCaseItemsListByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.String, CaseId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCaseTypes = new List<ViewCancelTransaction>();
                    while (reader.Read())
                    {
                        lstCaseTypes.Add(LoadCaseItems(reader));
                    }

                }
                return lstCaseTypes;
            }
        }


        public ViewCancelTransaction LoadCaseItems(SafeDataReader reader)
        {
            ViewCancelTransaction oCaseType = new ViewCancelTransaction();
            oCaseType.CaseId = reader.GetInt64("CaseId");
            oCaseType.PartNum = reader.GetString("PartNum");
            oCaseType.Description = reader.GetString("Description");
            oCaseType.LotNum = reader.GetString("LotNum");
            oCaseType.ExpiryDate = reader.GetString("ExpiryDate");
            oCaseType.IsNearExpiry = reader.GetBoolean("IsNearExpiry");
            return oCaseType;
        }

        public List<ViewCancelTransaction> GetKitDetailByCaseId(Int64 CaseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ViewCancelTransaction> lstCaseTypes;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitDetailByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.String, CaseId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCaseTypes = new List<ViewCancelTransaction>();
                    while (reader.Read())
                    {
                        lstCaseTypes.Add(new ViewCancelTransaction()
                        {
                            CaseId = reader.GetInt64("CaseId"),
                            KitFamilyName = reader.GetString("KitFamilyName"),
                            Description = reader.GetString("KitFamilyDescription"),
                            Quantity = reader.GetInt32("Quantity"),
                            BuildKitId = reader.GetInt64("BuildKitId"),
                            CaseStatus = reader.GetString("CaseStatus")
                        });
                    }

                }
                return lstCaseTypes;
            }
        }

        public List<ViewCancelTransaction> GetKitFamilyDetailByCaseId(Int64 CaseId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ViewCancelTransaction> lstCaseTypes;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCaseKitDetailByCaseId))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.String, CaseId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCaseTypes = new List<ViewCancelTransaction>();
                    while (reader.Read())
                    {
                        lstCaseTypes.Add(new ViewCancelTransaction()
                        {
                            CaseId = reader.GetInt64("CaseId"),
                            KitNumber = reader.GetString("KitNumber"),
                        });
                    }

                }
                return lstCaseTypes;
            }
        }

        public List<DispositionType> DispositionTypeList()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<DispositionType> lstDispositionType;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetDispositionTypesByCategory))
            {
                db.AddInParameter(cmd, "@DispositionCategory", DbType.String, "CaseCancel");
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstDispositionType = new List<DispositionType>();
                    while (reader.Read())
                    {
                        lstDispositionType.Add(new DispositionType()
                        {
                            DispositionTypeId = reader.GetInt32("DispositionTypeId"),
                            Disposition = reader.GetString("DispositionType"),
                        });
                    }

                }
                return lstDispositionType;
            }
        }

        public List<ViewCancelTransaction> GetVCTLocationsByFilter(int LocationId, string LocationName)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ViewCancelTransaction> lstCaseTypes;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLocationDetailsByLocationId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@LocationName", DbType.String, LocationName);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCaseTypes = new List<ViewCancelTransaction>();
                    while (reader.Read())
                    {
                        lstCaseTypes.Add(new ViewCancelTransaction()
                        {
                            PartyName = reader.GetString("LocationName")
                        });
                    }

                }
                return lstCaseTypes;
            }
        }

        public List<ViewCancelTransaction> GetCasesListByCaseType(int LocationId, DateTime StartDate, DateTime EndDate, string CaseType, string CaseNumber, string InvType, string PartyName, string LocationType, string CaseStatus,int PageIndex,int PageSize)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ViewCancelTransaction> lstCaseTypes;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetCasesListByCaseType_Pagination))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@StartDate", DbType.DateTime, StartDate);
                db.AddInParameter(cmd, "@EndDate", DbType.DateTime, EndDate);

                if (CaseType.ToUpper() != "ALL")
                    db.AddInParameter(cmd, "@CaseType", DbType.String, CaseType);

                db.AddInParameter(cmd, "@CaseNumber", DbType.String, CaseNumber);

                if (InvType.ToUpper() != "ALL")
                    db.AddInParameter(cmd, "@InvType", DbType.String, InvType);

                db.AddInParameter(cmd, "@PartyName", DbType.String, PartyName);

                if (LocationType.ToUpper() != "ALL")
                    db.AddInParameter(cmd, "@LocationType", DbType.String, LocationType);

                if (CaseStatus.ToUpper() != "ALL")
                    db.AddInParameter(cmd, "@CaseStatus", DbType.String, CaseStatus);

                db.AddInParameter(cmd, "@PAGE_NO", DbType.Int32, PageIndex);
                db.AddInParameter(cmd, "@PAGE_SIZE", DbType.Int32, PageSize);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCaseTypes = new List<ViewCancelTransaction>();
                    while (reader.Read())
                    {
                        lstCaseTypes.Add(new ViewCancelTransaction()
                        {
                            CaseId = reader.GetInt64("CaseId"),
                            CaseType = reader.GetString("CaseType"),
                            InventoryType = reader.GetString("InventoryType"),
                            CaseNumber = reader.GetString("CaseNumber"),
                            RequestNumber = reader.GetString("RequestNumber"),
                            CaseStatus = reader.GetString("CaseStatus"),
                            SurgeryDate = reader.GetDateTime("SurgeryDate"),
                            //FromLocationName = reader.GetString("FromLocationName"),
                            UpdatedOn = reader.GetDateTime("UpdatedOn"),
                            UpdatedBy = reader.GetString("UpdatedBy"),
                            //LocationType = reader.GetString("LocationType"),    

                            PartyId = reader.GetInt64("PartyId"),
                            PartyName = reader.GetString("PartyName"),

                            LocationId = reader.GetInt32("LocationId"),
                            FromLocationName = reader.GetString("FromLocationName"),
                            ToLocationId = reader.GetInt32("ToLocationId"),
                            ToLocationName = reader.GetString("ToLocationName"),
                            LTParty = reader.GetString("LTParty"),
                            FromLTName = reader.GetString("FromLTName"),
                            ToLTName = reader.GetString("ToLTName"),

                            DispositionType = reader.GetString("DispositionType"),
                            Remarks = reader.GetString("Remarks"),
                            RowType = reader.GetString("RowType"),
                            TotalRecordCount = reader.GetInt32("TotalRecordCount")
                        });
                    }

                }
                return lstCaseTypes;
            }
        }



        public List<KitFamily> GetKitFamilyByNumber(string KitFamily)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitFamily> lstKitFamily = new List<KitFamily>();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyByNumber))
            {
                db.AddInParameter(cmd, "@KitFamily", DbType.String, KitFamily);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstKitFamily.Add(
                            new KitFamily()
                            {
                                KitFamilyId = reader.GetInt64("KitFamilyId"),
                                KitFamilyName = reader.GetString("KitFamilyName")
                            });
                    }
                }
            }

            return lstKitFamily;
        }


        public List<KitFamilyInventoryTransfer> GetKitFamilyByLocationAndNumber(string KitFamily, int LocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitFamilyInventoryTransfer> lstKitFamily = new List<KitFamilyInventoryTransfer>();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyByLocationAndNumber))
            {
                db.AddInParameter(cmd, "@KitFamily", DbType.String, KitFamily);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstKitFamily.Add(
                            new KitFamilyInventoryTransfer()
                            {
                                KitFamilyId = reader.GetInt64("KitFamilyId"),
                                KitFamilyName = reader.GetString("KitFamilyName"),
                                KitFamilyDescription = reader.GetString("KitFamilyDescription")
                                //AvailableQty = reader.GetInt32("AvailableQty"),
                                //LastUsage = reader.GetString("LastUsage")
                            });
                    }
                }
            }

            return lstKitFamily;
        }

        public List<KitFamily> GetKitFamilyByNumberAndLocation(string KitFamily, Int32 LocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<KitFamily> lstKitFamily = new List<KitFamily>();

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetKitFamilyByNumberAndLocation))
            {
                db.AddInParameter(cmd, "@KitFamily", DbType.String, KitFamily);
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        lstKitFamily.Add(
                            new KitFamily()
                            {
                                KitFamilyId = reader.GetInt64("KitFamilyId"),
                                KitFamilyName = reader.GetString("KitFamilyName")
                            });
                    }
                }
            }

            return lstKitFamily;
        }

        public bool CaseDetailsCancel(Int64 caseId, Int32 DispositionTypeId, string Remarks)
        {
            bool returnValue = false;

            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_CaseDetailsCancel))
            {
                db.AddInParameter(cmd, "@CaseId", DbType.Int64, caseId);
                db.AddInParameter(cmd, "@DispositionTypeId", DbType.Int32, DispositionTypeId);
                db.AddInParameter(cmd, "@Remarks", DbType.String, Remarks);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);

                returnValue = Convert.ToBoolean(db.ExecuteScalar(cmd));
                
            }

            return returnValue;
        }

        public List<Default> GetPartsHighOrderList(Int32 LocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Default> lstPartList;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPartsHighOrderList))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstPartList = new List<Default>();
                    while (reader.Read())
                    {
                        lstPartList.Add(new Default()
                        {
                            PartNum = reader.GetString("PartNumber"),
                            Description = reader.GetString("Description"),
                            Count = reader.GetInt32("Total"),
                            InventoryUtilizationDay = reader.GetInt32("InventoryUtilizationDays")
                        });
                    }

                }
                return lstPartList;
            }
        }

        public Default GetHomeMapBranchDetail(int LocationId)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            Default oModel;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetLocationDetailLocationId))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    oModel = new Default();
                    while (reader.Read())
                    {
                        oModel.BranchName = reader.GetString("LocationName");
                        oModel.Longitude = reader.GetDecimal("Longitude");
                        oModel.Latitude = reader.GetDecimal("Latitude");
                        oModel.Address1 = reader.GetString("Address1");
                        oModel.Address2 = reader.GetString("Address2");
                    }

                }
                return oModel;
            }
        }

        public List<Default> GetHomeMapCasesList(int LocationId, DateTime BusinessDate)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Default> lstCases;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetMapShippingDetail))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@ShippingDate", DbType.Date, BusinessDate);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCases = new List<Default>();
                    while (reader.Read())
                    {
                        lstCases.Add(LoadMapCases(reader));
                    }

                }
            }

            return lstCases;
        }

        private Default LoadMapCases(SafeDataReader reader)
        {
            Default oModel = new Default()
            {
                PartyId = reader.GetInt64("PartyId"),
                HospitalName = reader.GetString("HospitalName"),
                Longitude = reader.GetDecimal("Longitude"),
                Latitude = reader.GetDecimal("Latitude"),
                Address1 = reader.GetString("Address1"),
                Address2 = reader.GetString("Address2"),
                ShippingDate = reader.GetString("ShippingDate"),
                CaseNumber = reader.GetString("CaseNumber"),
                CaseType = reader.GetString("CaseType"),
                InventoryType = reader.GetString("InventoryType"),
                CaseStatus = reader.GetString("CaseStatus")
            };

            return oModel;
        }

        public List<Default> GetMapAgingOrdersList(int LocationId, DateTime BusinessDate)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Default> lstCases;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetMapAgingOrdersList))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@ShippingDate", DbType.Date, BusinessDate);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCases = new List<Default>();
                    while (reader.Read())
                    {
                        lstCases.Add(LoadMapCases(reader));
                    }

                }
            }

            return lstCases;
        }

        public List<ViewCancelTransaction> GetPendingCasesBySalesPerson(int LocationId, string SalesPerson)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<ViewCancelTransaction> lstCaseTypes;

            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetPendingCasesListBySalesPerson))
            {
                db.AddInParameter(cmd, "@LocationId", DbType.Int32, LocationId);
                db.AddInParameter(cmd, "@SalesRep", DbType.String, SalesPerson);

                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    lstCaseTypes = new List<ViewCancelTransaction>();
                    while (reader.Read())
                    {
                        lstCaseTypes.Add(LoadCaseList(reader));
                    }

                }
                return lstCaseTypes;
            }
        }

        public ViewCancelTransaction LoadCaseList(SafeDataReader reader)
        {
            ViewCancelTransaction oCaseType = new ViewCancelTransaction();

            oCaseType.CaseId = reader.GetInt64("CaseId");
            oCaseType.CaseType = reader.GetString("CaseType");
            oCaseType.InventoryType = reader.GetString("InventoryType");
            oCaseType.CaseNumber = reader.GetString("CaseNumber");
            oCaseType.RequestNumber = reader.GetString("RequestNumber");
            oCaseType.CaseStatus = reader.GetString("CaseStatus");
            oCaseType.SurgeryDate = reader.GetDateTime("SurgeryDate");
            oCaseType.PartyName = reader.GetString("PartyName");
            oCaseType.FromLocationName = reader.GetString("FromLocationName");
            oCaseType.UpdatedOn = reader.GetDateTime("UpdatedOn");
            oCaseType.UpdatedBy = reader.GetString("UpdatedBy");
            oCaseType.LocationType = reader.GetString("LocationType");
            oCaseType.LocationId = reader.GetInt32("LocationId");
            oCaseType.PartyId = reader.GetInt64("PartyId");
            oCaseType.DispositionType = reader.GetString("DispositionType");
            oCaseType.Remarks = reader.GetString("Remarks");

            return oCaseType;
        }

    }
}
