using Sitecore.Diagnostics;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data.Linq;

namespace Sitecore.Hack.HI.Web.sitecore.admin.v2
{
    
    public partial class stats : Sitecore.sitecore.admin.AdminPage
    {
        #region Events
        protected override void OnInit(EventArgs e)
        {
            base.CheckSecurity(true); //Required!
            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs arguments)
        {
            try
            {
                Assert.ArgumentNotNull(sender, "sender");
                Assert.ArgumentNotNull(arguments, "arguments");
                //base.CheckSecurity(true);
                if (!IsPostBack)
                {
                    this.ShowSiteSelector();
                }
                //this.ShowRenderingStats(base.Request.QueryString["site"]);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error while loading stats v2  entries", ex, this);
                ShowMessage(ex.Message, "Error");
            }

        }

        private void Page_Error(object sender, EventArgs e)
        {
            // Get last error from the server
            Exception exc = Server.GetLastError();
            Response.Write("Oops something went wrong : " + exc.Message);
            Sitecore.Diagnostics.Log.Error("stats V2 : " + exc, this);
            Server.ClearError();

        }

        protected void ddlSites_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSites.SelectedIndex == 0)
                {
                    // Please select
                    ShowMessage("No rendering stats found, it seems that you haven't yet requested selected site or no site has been selected yet.",
                   "Error");
                    lblMessage.Visible = true;
                    tblStats.Visible = false;
                    phChart.Visible = false;
                }
                else
                {
                    // Load data
                    ShowRenderingStats(ddlSites.SelectedItem.Value);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error while loading stats v2 sites", ex, this);
                ShowMessage(ex.Message, "Error");
                tblStats.Visible = false;
            }
        }

        protected void chkIgnorePH_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlSites.SelectedIndex == 0)
                {
                    // Please select
                    ShowMessage("No rendering stats found, it seems that you haven't yet requested selected site or no site has been selected yet.",
                   "Error");
                    lblMessage.Visible = true;
                    tblStats.Visible = false;
                    phChart.Visible = false;
                }
                else
                {
                    // Load data
                    ShowRenderingStats(ddlSites.SelectedItem.Value);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error while loading stats v2 sites", ex, this);
                ShowMessage(ex.Message, "Error");
                tblStats.Visible = false;
            }
        }

        protected void btnReset_Click(object sender, EventArgs arguments)
        {
            try
            {
                if (ddlSites.SelectedIndex == 0)
                {
                    // Please select
                    ShowMessage("No rendering stats found, it seems that you haven't yet requested selected site or no site has been selected yet.",
                   "Error");
                    lblMessage.Visible = true;
                    tblStats.Visible = false;
                    phChart.Visible = false;
                }
                else
                {
                    // Reset data
                    Assert.ArgumentNotNull(sender, "sender");
                    Assert.ArgumentNotNull(arguments, "arguments");
                    Statistics.Clear();
                    ShowMessage("Statistics reset done",
                   string.Empty);
                    lblMessage.Visible = true;
                    tblStats.Visible = false;
                    phChart.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error while resetting stats v2 sites", ex, this);
                ShowMessage(ex.Message, "Error");
                tblStats.Visible = false;
            }
        }
        #endregion

        #region Private methods
        private static string[] GetSiteNames()
        {
            List<string> list = new List<string>();
            foreach (Statistics.RenderingData data in Statistics.RenderingStatistics)
            {
                if (!list.Contains(data.SiteName))
                {
                    list.Add(data.SiteName);
                }
            }
            return list.ToArray();
        }

        private void ShowRenderingStats(string siteName)
        {           
            if (Statistics.RenderingStatistics != null && Statistics.RenderingStatistics.Any())
            {
                SortedList<string, Statistics.RenderingData> list = new SortedList<string, Statistics.RenderingData>();
                foreach (Statistics.RenderingData data in Statistics.RenderingStatistics)
                {
                    if ((siteName == null) || data.SiteName.Equals(siteName, StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(data.SiteName + 0xff + data.TraceName, data);
                    }
                }

                int counter = 0;

                List<RenderingDataForChart> renderingDataForChartList = new List<RenderingDataForChart>();
                foreach (Statistics.RenderingData data2 in list.Values)
                {
                    if (chkIgnorePH.Checked && data2.TraceName.StartsWith("Placeholder:", StringComparison.InvariantCultureIgnoreCase))
                    {
                        // Ignore placeholder
                        continue;
                    }


                    renderingDataForChartList.Add(
                        new RenderingDataForChart
                        {
                            RenderingFullName = data2.TraceName,
                            RenderingShortName = data2.TraceName,
                            TimeTaken = data2.MaxTime.TotalMilliseconds.ToString(),
                            RawTimeTaken = data2.MaxTime
                        }
                        );

                    TableRow tableRow = new TableRow();
                    tableRow.ID = "row" + counter++;
                    tableRow.TableSection = TableRowSection.TableBody;

                    // Rendering
                    AddTableCell(tableRow, data2.TraceName, FieldTypes.Text);

                    // Site
                    AddTableCell(tableRow, data2.SiteName, FieldTypes.Text);

                    // Count
                    AddTableCell(tableRow, data2.RenderCount.ToString(), FieldTypes.Text);

                    // From cache
                    AddTableCell(tableRow, data2.UsedCache.ToString(), FieldTypes.Text);

                    // Avg. time (ms)
                    AddTableCell(tableRow, data2.AverageTime.TotalMilliseconds.ToString(), FieldTypes.Text);

                    // Avg. items
                    AddTableCell(tableRow, data2.AverageItemsAccessed.ToString(), FieldTypes.Text);


                    // Max. time
                    AddTableCell(tableRow, data2.MaxTime.TotalMilliseconds.ToString(), FieldTypes.Text);


                    // Max. items
                    AddTableCell(tableRow, data2.MaxItemsAccessed.ToString(), FieldTypes.Text);

                    // Total time
                    AddTableCell(tableRow, data2.TotalTime.ToString(), FieldTypes.Text);

                    // Total items
                    AddTableCell(tableRow, data2.TotalItemsAccessed.ToString(), FieldTypes.Text);


                    // Last run
                    AddTableCell(tableRow, data2.LastRendered.ToString(), FieldTypes.DateTime);

                    tblStats.Rows.Add(tableRow);

                }

                lblMessage.Visible = false;
                tblStats.Visible = true;
                phChart.Visible = true;

                renderingDataForChartList.Sort(delegate(RenderingDataForChart r1, RenderingDataForChart r2)
                {
                    return r2.RawTimeTaken.CompareTo(r1.RawTimeTaken);
                }
                );

                int count = renderingDataForChartList.Count > 10 ? 10 : renderingDataForChartList.Count;
                // Get only TOP 10 Records
                renderingDataForChartList = renderingDataForChartList.GetRange(0, count);
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                // http://stackoverflow.com/questions/2077265/json-in-c-sending-and-receiving-data                
                string scriptToLoad = string.Format("setTimeout(function () {{ LoadBarChart('{0}'); }}, 100);",
                    serializer.Serialize(renderingDataForChartList));

                Page.ClientScript.RegisterStartupScript(this.GetType(), "loadbarchart", scriptToLoad, true);

            }
            else
            {
                ShowMessage("No stats found", "Error");
            }
        }

        private void ShowSiteSelector()
        {
            string[] siteNames = GetSiteNames();
            Array.Sort<string>(siteNames);
            ddlSites.Items.Add("All Sites");
            foreach (string str in siteNames)
            {
                ddlSites.Items.Add(str);
            }
        }

        /// <summary>
        /// This function will be used
        /// to show message
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="messageType">Message type to show</param>
        private void ShowMessage(string message, string messageType)
        {
            lblMessage.Visible = true;
            lblMessage.Text = message;
            if (messageType == "Error")
            {
                lblMessage.CssClass = "alert alert-danger";

            }
            else
            {
                lblMessage.CssClass = "alert alert-success";
            }
        }

        /// <summary>
        /// This function will be used to add
        /// table cell
        /// </summary>
        /// <param name="tableRow">Table Row</param>
        /// <param name="aField">Field</param>
        /// <param name="fieldType">Type of field</param>
        /// <returns></returns>
        private static TableCell AddTableCell(TableRow tableRow,
            string value, FieldTypes fieldType)
        {
            TableCell tableCell1 = new TableCell();

            string valueToPrint = "NA";

            switch (fieldType)
            {
                case FieldTypes.DateTime:
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        DateTime createdDate = DateTime.Now;
                        createdDate = DateTime.Parse(value);
                        valueToPrint = TimeAgo(createdDate);
                    }
                    else
                    {
                        valueToPrint = "NA";
                    }
                    break;
                case FieldTypes.Text:
                    valueToPrint = !string.IsNullOrWhiteSpace(value) ? value : "NA";
                    break;
                default:
                    valueToPrint = !string.IsNullOrWhiteSpace(value) ? value : "NA";
                    break;
            }

            tableCell1.Text = valueToPrint;
            tableRow.Cells.Add(tableCell1);
            return tableCell1;
        }

        /// <summary>
        /// http://aeykay.blogspot.in/2012/10/facebook-like-time-ago-function.html
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string TimeAgo(DateTime date)
        {

            TimeSpan timeSince = DateTime.UtcNow.Subtract(date);
            if (timeSince.TotalMilliseconds < 1)
                return "not yet";
            if (timeSince.TotalMinutes < 1)
                return "just now";
            if (timeSince.TotalMinutes < 2)
                return "1 minute ago";
            if (timeSince.TotalMinutes < 60)
                return string.Format("{0} minutes ago", timeSince.Minutes);
            if (timeSince.TotalMinutes < 120)
                return "1 hour ago";
            if (timeSince.TotalHours < 24)
                return string.Format("{0} hours ago", timeSince.Hours);
            if (timeSince.TotalDays == 1)
                return "yesterday";
            if (timeSince.TotalDays < 7)
                return string.Format("{0} days ago", timeSince.Days);
            if (timeSince.TotalDays < 14)
                return "last week";
            if (timeSince.TotalDays < 21)
                return "2 weeks ago";
            if (timeSince.TotalDays < 28)
                return "3 weeks ago";
            if (timeSince.TotalDays < 60)
                return "last month";
            if (timeSince.TotalDays < 365)
                return string.Format("{0} months ago", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730)
                return "last year";

            //last but not least...
            return string.Format("{0} years ago", Math.Round(timeSince.TotalDays / 365));

        }


        #endregion

        enum FieldTypes
        {
            DateTime,
            Text
        }
    }

    public class RenderingDataForChart
    {
        public string RenderingShortName { get; set; }
        public string RenderingFullName { get; set; }
        public string TimeTaken { get; set; }
        public TimeSpan RawTimeTaken { get; set; }
    }
}