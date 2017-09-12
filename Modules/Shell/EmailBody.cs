using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell
{
    public class EmailBody : IDisposable
    {
        public string CreateLowInventoryReportBody(List<LowInventory> listLowInventory)
        {
            int lineNumber = 0;
            var builder = new StringBuilder();

            #region Mail Format

            builder.Append("<html>");

            //************************************************************************************************************************************
            //Body Header..........
            //builder.Append("<table cellspacing =\"0\">");
            //builder.Append("<tr><td style=\"font-family: Calibri; font-size: 12pt; font-weight: normal;\">" + contact.FirstName.Trim() + " " + contact.LastName.Trim() + ",</td></tr>");
            //builder.Append("<tr><td style=\"font-family: Verdana; font-size: 10pt; font-weight: normal;\">Below is the Low Inventory report for surgical center '" + listLowInventory[0].LocationName + "':</td><br/></tr>");
            //builder.Append("</table>");
            //************************************************************************************************************************************

            var listProductLine = listLowInventory.Select(i => i.ProductLine).Distinct().ToList();

            if (listProductLine.Count > 0)
            {
                foreach (var productLine in listProductLine)
                {
                    var listLowInventoryForProductLine = listLowInventory.FindAll(p => p.ProductLine.Trim().ToUpper() == productLine.Trim().ToUpper());

                    if (!listLowInventoryForProductLine.Any()) continue;
                    //************************************************************************************************************************************
                    //For one Product Line Name and Description
                    builder.Append("<table cellspacing=\"4\" cellpadding=\"3\" bgcolor=\"#DBE5F1\">");
                    builder.Append("<tr><td style=\"font-family: Calibri; font-size: 12pt; font-weight: bold;\">" + listLowInventoryForProductLine[0].ProductLine + " : " + listLowInventoryForProductLine[0].ProductLineDesc + "</td></tr>");

                    builder.Append("<tr><td><table cellpadding=\"5\" cellspacing=\"0\">");
                    builder.Append("<tr>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 25px;\">Line</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 100px;\">Ref #</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 200px;\">Part</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 90px;\">Size</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #244061; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 75px; color: #FFFFFF;\">Required Order</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 75px;\">Already On Order</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 75px;\">Current Inventory</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 75px;\">Par Level</td>");
                    builder.Append("</tr>");


                    var iRowCount = 0;
                    foreach (var lowInventory in listLowInventoryForProductLine)
                    {
                        lineNumber++;
                        iRowCount++;
                        if (iRowCount % 2 == 0)
                        {
                            //For Odd Number of Row back color will be gray.
                            builder.Append("<tr>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 25px;\">" + Convert.ToString(lineNumber) + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 100px;\">" + lowInventory.RefNum + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 200px;\">" + lowInventory.PartDesc + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 90px;\">" + lowInventory.Size + "</td>");
                            if (lowInventory.BackOrderQty > 0)
                            {
                                builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 75px; color: #FF0000;\">" + Convert.ToString(lowInventory.LowInvQty) + "</td>");
                            }
                            else
                            {
                                builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 75px;\">" + Convert.ToString(lowInventory.LowInvQty) + "</td>");
                            }
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 75px;\">" + Convert.ToString(lowInventory.OrderedProductQty) + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 75px;\">" + Convert.ToString(lowInventory.InvLevelQty) + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 75px;\">" + Convert.ToString(lowInventory.PARLevelQty) + "</td>");
                            builder.Append("</tr>");
                        }
                        else
                        {
                            //For Even Number of Row back color will be white.
                            builder.Append("<tr>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 25px;\">" + Convert.ToString(lineNumber) + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 100px;\">" + lowInventory.RefNum + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 200px;\">" + lowInventory.PartDesc + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #0;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 90px;\">" + lowInventory.Size + "</td>");
                            if (lowInventory.BackOrderQty > 0)
                            {
                                builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 75px; color: #FF0000;\">" + Convert.ToString(lowInventory.LowInvQty) + "</td>");
                            }
                            else
                            {
                                builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 75px;\">" + Convert.ToString(lowInventory.LowInvQty) + "</td>");
                            }
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 75px;\">" + Convert.ToString(lowInventory.OrderedProductQty) + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 75px;\">" + Convert.ToString(lowInventory.InvLevelQty) + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 75px;\">" + Convert.ToString(lowInventory.PARLevelQty) + "</td>");
                            builder.Append("</tr>");
                        }
                    }
                    builder.Append(" </table></td></tr></table>");
                }

                builder.Append("<br /><table><tr><td style=\"font-family: Calibri; font-size: 12pt; color: #FF0000;\">* Quantities in red are on backorder.</td></tr></table>");
            }
            //************************************************************************************************************************************
            //Footer
            //builder.Append("<br /><table><tr><td style=\"font-family: Calibri; font-size: 12pt; font-weight: bold;\">Sincerely,</td></tr>");
            //builder.Append("<tr><td style=\"font-family: Calibri; font-size: 12pt; font-weight: normal;\">Stryker Spine Support Group</td></tr></table>");
            builder.Append("</html>");
            #endregion

            return builder.ToString();
        }

        public string CreateInventoryAmountReportBody(List<InventoryAmount> listInventoryAmount)
        {
            int lineNumber = 0;
            var builder = new StringBuilder();

            #region Mail Format

            builder.Append("<html>");

            //************************************************************************************************************************************
            //Body Header..........
            //builder.Append("<table cellspacing =\"0\">");
            //builder.Append("<tr><td style=\"font-family: Calibri; font-size: 12pt; font-weight: normal;\">" + contact.FirstName.Trim() + " " + contact.LastName.Trim() + ",</td></tr>");
            //builder.Append("<tr><td style=\"font-family: Verdana; font-size: 10pt; font-weight: normal;\">Below is the Low Inventory report for surgical center '" + listInventoryAmount[0].LocationName + "':</td><br/></tr>");
            //builder.Append("</table>");
            //************************************************************************************************************************************

            var listProductLine = listInventoryAmount.Select(i => i.ProductLine).Distinct().ToList();

            if (listProductLine.Count > 0)
            {
                foreach (var productLine in listProductLine)
                {
                    var listInventoryAmountForProductLine = listInventoryAmount.FindAll(p => p.ProductLine.Trim().ToUpper() == productLine.Trim().ToUpper());

                    if (!listInventoryAmountForProductLine.Any()) continue;
                    //************************************************************************************************************************************
                    //For one Product Line Name and Description
                    builder.Append("<table cellspacing=\"4\" cellpadding=\"3\" bgcolor=\"#DBE5F1\">");
                    builder.Append("<tr><td style=\"font-family: Calibri; font-size: 12pt; font-weight: bold;\">" + listInventoryAmountForProductLine[0].ProductLine + " : " + listInventoryAmountForProductLine[0].ProductLineDesc + "</td></tr>");

                    builder.Append("<tr><td><table cellpadding=\"5\" cellspacing=\"0\">");
                    builder.Append("<tr>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 25px;\">Line</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 100px;\">Ref #</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 300px;\">Part</td>");                    
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 100px;\">Off Cart</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #244061; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 100px; color: #FFFFFF;\">Current Inventory</td>");
                    builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; text-align: center; font-family: Calibri; font-size: 12pt;font-weight: bold; height: 20px; width: 100px;\">Par Level</td>");
                    builder.Append("</tr>");


                    var iRowCount = 0;
                    foreach (var inventoryAmount in listInventoryAmountForProductLine)
                    {
                        lineNumber++;
                        iRowCount++;
                        if (iRowCount % 2 == 0)
                        {
                            //For Odd Number of Row back color will be gray.
                            builder.Append("<tr>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 25px;\">" + Convert.ToString(lineNumber) + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 100px;\">" + inventoryAmount.RefNum + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 300px;\">" + inventoryAmount.PartDesc + "</td>");                            
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 100px;\">" + inventoryAmount.OffCartQty + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 100px;\">" + inventoryAmount.Qty+ "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 100px;\">" + Convert.ToString(inventoryAmount.PARLevelQty) + "</td>");                            
                            builder.Append("</tr>");
                        }
                        else
                        {
                            //For Even Number of Row back color will be white.
                            builder.Append("<tr>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 25px;\">" + Convert.ToString(lineNumber) + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 100px;\">" + inventoryAmount.RefNum + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 300px;\">" + inventoryAmount.PartDesc + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #0;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 100px;\">" + inventoryAmount.OffCartQty+ "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #0;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 100px;\">" + inventoryAmount.Qty + "</td>");
                            builder.Append("<td style=\"border: thin solid #000000; background-color: #FFFFFF; border-color: #000000;text-align: center; font-family: Calibri; font-size: 12pt; font-weight: normal;height: 20px; width: 100px;\">" + Convert.ToString(inventoryAmount.PARLevelQty) + "</td>");
                            builder.Append("</tr>");
                        }
                    }
                    builder.Append(" </table></td></tr></table>");
                }

                //builder.Append("<br /><table><tr><td style=\"font-family: Calibri; font-size: 12pt; color: #FF0000;\">* Quantity highlighted in red represents Back Order existense</td></tr></table>");
            }
            //************************************************************************************************************************************
            //Footer
            //builder.Append("<br /><table><tr><td style=\"font-family: Calibri; font-size: 12pt; font-weight: bold;\">Sincerely,</td></tr>");
            //builder.Append("<tr><td style=\"font-family: Calibri; font-size: 12pt; font-weight: normal;\">Stryker Spine Support Group</td></tr></table>");
            builder.Append("</html>");
            #endregion

            return builder.ToString();
        }

        public void Dispose()
        {

        }

        
    }
}
