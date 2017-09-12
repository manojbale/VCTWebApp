using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace VCTWebApp
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("CaseType", typeof(string));
            dt.Columns.Add("Case#", typeof(string));
            dt.Columns.Add("InvType", typeof(string));
            dt.Columns.Add("SurgeryDate", typeof(string));
            dt.Columns.Add("ShipToLocation", typeof(string));
            dt.Columns.Add("LocationType", typeof(string));
            dt.Columns.Add("RequestedBy", typeof(string));
            dt.Columns.Add("RequestedOn", typeof(string));
            dt.Columns.Add("Status", typeof(string));

            DataRow row1 = dt.NewRow();
            row1["CaseType"] = "InventoryTransfer";
            row1["Case#"] = "RC-200081";
            row1["InvType"] = "Kit";
            row1["SurgeryDate"] = "5/16/2014";
            row1["ShipToLocation"] = "Florida Hospital Altamonte";
            row1["LocationType"] = "HOSPITALS";
            row1["RequestedBy"] = "jains";
            row1["RequestedOn"] = "5/12/2014 10:48:03 AM";
            row1["Status"] = "New";
            dt.Rows.Add(row1);

            DataRow row2 = dt.NewRow();
            row2["CaseType"] = "NewProductTransfer";
            row2["Case#"] = "RC-200082";
            row2["InvType"] = "Kit";
            row2["SurgeryDate"] = "5/17/2014";
            row2["ShipToLocation"] = "Warren McClure";
            row2["LocationType"] = "HOSPITALS";
            row2["RequestedBy"] = "amit";
            row2["RequestedOn"] = "5/13/2014 10:48:03 AM";
            row2["Status"] = "New";
            dt.Rows.Add(row2);

            DataRow row3 = dt.NewRow();
            row3["CaseType"] = "ReplenishmentTransfer";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "InventoryAssigned";
            dt.Rows.Add(row3);
                        
            row3 = dt.NewRow();
            row3["CaseType"] = "ReturnInventoryRMATransfer";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "Cancelled";
            dt.Rows.Add(row3);

            row3 = dt.NewRow();
            row3["CaseType"] = "RoutineCase";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "Shipped";
            dt.Rows.Add(row3);

            row3 = dt.NewRow();
            row3["CaseType"] = "InternalRequest";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "Shipped";
            dt.Rows.Add(row3);

            row3 = dt.NewRow();
            row3["CaseType"] = "InternalRequest";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "Delivered";
            dt.Rows.Add(row3);

            row3 = dt.NewRow();
            row3["CaseType"] = "InventoryTransfer";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "Received";
            dt.Rows.Add(row3);

              row3 = dt.NewRow();
              row3["CaseType"] = "NewProductTransfer";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "Cancelled";
            dt.Rows.Add(row3);

            row3 = dt.NewRow();
            row3["CaseType"] = "ReplenishmentTransfer";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "Shipped";
            dt.Rows.Add(row3);

            row3 = dt.NewRow();
            row3["CaseType"] = "ReplenishmentTransfer";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "Shipped";
            dt.Rows.Add(row3);

            row3 = dt.NewRow();
            row3["CaseType"] = "InternalRequest";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "Delivered";
            dt.Rows.Add(row3);

            row3 = dt.NewRow();
            row3["CaseType"] = "InternalRequest";
            row3["Case#"] = "RC-200083";
            row3["InvType"] = "Parts";
            row3["SurgeryDate"] = "5/18/2014";
            row3["ShipToLocation"] = "Baylor Hospital";
            row3["LocationType"] = "HOSPITALS";
            row3["RequestedBy"] = "jains";
            row3["RequestedOn"] = "5/14/2014 10:48:03 AM";
            row3["Status"] = "Received";
            dt.Rows.Add(row3);

            //for (int i = 0; i < 12; i++)
            //{
            //    DataRow row = dt.NewRow();
            //    row["PONO"] = "P1000"+ i.ToString();
            //    row["Case#"] = "RC-200083"+ i.ToString();
            //    row["InvType"] = "Parts";
            //    row["SurgeryDate"] = "5/18/2014";
            //    row["ShipToLocation"] = "Martinez Convalescent Hospital";
            //    row["LocationType"] = "HOSPITALS";
            //    row["RequestedBy"] = "jains";
            //    row["RequestedOn"] = "5/14/2014 10:48:03 AM";
            //    row["Status"] = "Delivered";
            //    dt.Rows.Add(row);

            //}

                GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btn = e.Row.FindControl("ImgStatus") as ImageButton;
                btn.Height = 20;
                if (btn.CommandArgument == "New")
                {
                    btn.ImageUrl = "~/images/step1.png";                    
                    //btn.CssClass = "btn1";
                }
                else if (btn.CommandArgument == "InventoryAssigned")
                {
                    btn.ImageUrl = "~/images/step2.png";
                    //btn.CssClass = "btn2";
                }
                else if (btn.CommandArgument == "Cancelled")
                {
                    btn.ImageUrl = "~/images/step3.png";
                    //btn.CssClass = "btn3";
                }
                else if (btn.CommandArgument == "Shipped")
                {
                    btn.ImageUrl = "~/images/step4.png";
                    //btn.CssClass = "btn4";
                }
                else if (btn.CommandArgument == "Delivered")
                {
                    btn.ImageUrl = "~/images/step5.png";
                    //btn.CssClass = "btn5";
                }
                else if (btn.CommandArgument == "Received")
                {
                    btn.ImageUrl = "~/images/step6.png";
                    //btn.CssClass = "btn6";
                }

            }
        }
    }
}