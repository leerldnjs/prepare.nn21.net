using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;

public partial class Admin_Dialog_RequestStorcedIn : System.Web.UI.Page
{
	private DBConn DB;
	protected String RequestFormPk;
	protected String TITLE;
	protected String StorcedIn;
    protected void Page_Load(object sender, EventArgs e)
    {
		RequestFormPk = Request.Params["S"];
		LoadRequest(RequestFormPk, Session["CompanyPk"] + "");
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
    }
	private Boolean LoadRequest(string RequestFormPk, string CompanyPk)
	{
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT R.[ShipperCode], R.[ConsigneeCode], R.[DepartureDate], R.[ArrivalDate]
	, DRC.Name AS DRCName, ARC.Name AS ARCName, TWCLD.[Description]
	, R.RequestFormPk, R.TotalPackedCount, R.TotalGrossWeight, R.TotalVolume 
  FROM [RequestForm] AS R
	left join RegionCode AS DRC ON R.[DepartureRegionCode]=DRC.RegionCode 
	left join RegionCode AS ARC ON R.[ArrivalRegionCode]=ARC.RegionCode
	left join TransportWayCLDescription AS TWCLD ON R.[TransportWayCL]=TWCLD.[TransportWayCL]
 WHERE R.RequestFormPk=" + RequestFormPk + ";";
		string Calculated = "";
		bool IsCalculated = true;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			TITLE = (RS["DepartureDate"] + "" == "" ? "" : (RS["DepartureDate"] + "").Substring(4)) +
				RS["DRCName"] + " <strong>[" + RS["ShipperCode"] + "]</strong> ~ " +
				(RS["ArrivalDate"] + "" == "" ? "" : (RS["ArrivalDate"] + "").Substring(4)) +
				RS["ARCName"] + " <strong>[" + RS["ConsigneeCode"] + "]</strong> : <span style='color:blue;'>" + RS["Description"] + "</span>";
			Calculated = "<input type=\"hidden\" id=\"HTotalPackedCount\" value=\"" + RS["TotalPackedCount"] + "\" />" +
								"<input type=\"hidden\" id=\"HTotalGrossWeight\" value=\"" + RS["TotalGrossWeight"] + "\" />" +
								"<input type=\"hidden\" id=\"HTotalVolume\" value=\"" + RS["TotalVolume"] + "\" />";
			if (RS["TotalPackedCount"] + "" == "") {
				IsCalculated = false;
			}
		}
		RS.Dispose();

		string packedcount = "";
		string grossweight = "";
		string volume = "";
		if (!IsCalculated) {
			DB.SqlCmd.CommandText = @"
SELECT SUM([PackedCount]), SUM([GrossWeight]), SUM([Volume]) 
  FROM [RequestFormItems]
  WHERE RequestFormPk=" + RequestFormPk + @"
 Group By RequestFormPk ;";
			RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				packedcount = RS[0] + "";
				grossweight = RS[1] + "";
				volume = RS[2] + "";
			}
			RS.Dispose();
		}
		DB.DBCon.Close();
		if (CompanyPk != "") {
			StorcedIn = "<div>Stored time: <input id=\"StorcedD\" type=\"text\" size=\"8\" value=\"" + DateTime.Now.Date.ToString("yyyyMMdd") + "\" />&nbsp;<input type=\"text\" id=\"StorcedH\" style=\"width:20px;\" maxlength=\"2\" /> : <input type=\"text\" id=\"StorcedM\" style=\"width:20px;\" maxlength=\"2\" />&nbsp;&nbsp;&nbsp;" + new Request().LoadBranchStorage(CompanyPk) + "</div>" +
			"<p>Box Count : <input type=\"text\" id=\"PackedCount\" style=\"width:50px; \" value=\"" + packedcount + "\" /> <select id='PackingUnit' size='1'><option value='15'>CT</option><option value='16'>RO</option><option value='17'>PA</option></select>&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"Weight\" style=\"width:50px; \"  value=\"" + grossweight + "\" /> K&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"Volume\" style=\"width:50px; \" value=\"" + volume + "\" /> CBM</p>" + Calculated;
		}
		return true;
	}
}