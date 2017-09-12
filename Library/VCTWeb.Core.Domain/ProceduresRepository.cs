using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class ProceduresRepository
    {
        public List<Procedures> GetProceduresByProcedureName(string sProcedureName, string sPhysicianName)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Procedures> lstProcedures = new List<Procedures>();
            Procedures newProcedure = new Procedures();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETPROCEDURESBYPROCEDURENAME))
            {
                db.AddInParameter(cmd, "@Name", DbType.String, sProcedureName);
                db.AddInParameter(cmd, "@PhysicianName", DbType.String, sPhysicianName);

                
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newProcedure = Load(reader);
                        lstProcedures.Add(newProcedure);
                    }

                }
                return lstProcedures;
            }
        }



        public List<Physician> GetPhysicianByPartyName(string sPhysicianName,long lPartyId )
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Physician> lstProcedures = new List<Physician>();
            Physician newPhysician = new Physician();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETPPHYSICIANBYPARTYNAME))
            {
                db.AddInParameter(cmd, "@Name", DbType.String, sPhysicianName);
                db.AddInParameter(cmd, "@PartyId", DbType.Int64, lPartyId);            


                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newPhysician = LoadPhysician(reader);
                        lstProcedures.Add(newPhysician);
                    }

                }
                return lstProcedures;
            }
        }

        public List<Procedures> FetchAllProcedures()
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            List<Procedures> lstProcedures = new List<Procedures>();
            Procedures newProcedure = new Procedures();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GetAllProcedures))
            {
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newProcedure = Load(reader);
                        lstProcedures.Add(newProcedure);
                    }
                }
                return lstProcedures;
            }
        }

        private Procedures Load(SafeDataReader reader)
        {
            Procedures newProcedure = new Procedures();

            newProcedure.Name = reader.GetString("Name");
            newProcedure.Description = reader.GetString("Description");

            return newProcedure;
        }
        private Physician LoadPhysician(SafeDataReader reader)
        {
            Physician newPhysician = new Physician();

            newPhysician.PhysicianName = reader.GetString("PhysicianName");
            //newPhysician.PhysicianId = Convert.ToInt32(reader["PhysicianId"]);

            return newPhysician;
        }
    }
}
