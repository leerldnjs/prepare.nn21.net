using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;

public partial class Request_Dialog_OfferShipperNameSelection : System.Web.UI.Page
{
	public String ShipperData;
	protected void Page_Load(object sender, EventArgs e)
	{
		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");

		try { switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; } }
		catch (Exception) { }

		string Data = new RequestP().CompanyNameInDocumentList(Request.Params["S"]);
		string[] ShipperSplit = Data.Split(Common.Splite34, StringSplitOptions.RemoveEmptyEntries);
		StringBuilder Html = new StringBuilder();

		foreach (String Shipper in ShipperSplit)
		{
			Html.Append("<input type=\"button\" value=\"" + GetGlobalResourceObject("Member", "Selection") + "\" onclick=\"InsertSuccess('" + Shipper + "')\" /><input type=\"button\" value=\"" + GetGlobalResourceObject("Member", "Delete") + "\" onclick=\"Del('" + Shipper.Replace("!!", "')\" />&nbsp;&nbsp;<strong>").Replace("@@", "</strong> ") + "<br />");
		}
		ShipperData = Html + "";
	}
}
