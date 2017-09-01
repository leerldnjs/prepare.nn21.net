using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_Dialog_TransportHead :System.Web.UI.Page
{
	protected sTransportHead Head;
	protected List<string> pDateTime_From = new List<string>();
	protected List<string> pDateTime_To = new List<string>();
	protected string pType = "";
	protected string pTypePk = "";

	protected void Page_Load(object sender, EventArgs e) {
		try { switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; } } catch (Exception) { }

		pType = Request.Params["Type"];
		pTypePk = Request.Params["TypePk"];

		TransportC TransC = new TransportC();
		if (pTypePk != null) {
			DBConn DB = new DBConn();
			DB.DBCon.Open();
			Head = TransC.Load_TransportHead(pTypePk, ref DB);
			DB.DBCon.Close();
			pDateTime_From = ToSubString("Date", Head.DateTime_From);
			pDateTime_To = ToSubString("Date", Head.DateTime_To);
		} else {
			Head = new sTransportHead() {
				Transport_Head_Pk = "",
				Transport_Way = "",
				Transport_Status = "0",
				BranchPk_From = "",
				BranchPk_To = "",
				Warehouse_Pk_Arrival = "",
				Area_From = "",
				Area_To = "",
				DateTime_From = "",
				DateTime_To = "",
				Title = "",
				VesselName = "",
				Voyage_No = "",
				Value_String_0 = "",
				Value_String_1 = "",
				Value_String_2 = "",
				Value_String_3 = "",
				Value_String_4 = "",
				Value_String_5 = ""
			};
			pDateTime_From.Add("");
			pDateTime_From.Add("");
			pDateTime_From.Add("");
			pDateTime_To.Add("");
			pDateTime_To.Add("");
			pDateTime_To.Add("");
		}
	}

	private List<string> ToSubString(string Type, string SplitReq) {
		List<string> ReturnValue = new List<string>();
		switch (Type) {
			case "Date":
				ReturnValue.Add(SplitReq.Substring(0, SplitReq.IndexOf(" ")));
				ReturnValue.Add(SplitReq.Substring(SplitReq.IndexOf(" "), SplitReq.IndexOf(":") - SplitReq.IndexOf(" ")));
				ReturnValue.Add(SplitReq.Substring(SplitReq.IndexOf(":") + 1));
				/*
				if (SplitReq.Length > 10) {
					ReturnValue.Add(SplitReq.Substring(0, 7));
					ReturnValue.Add(SplitReq.Substring(8, 2));
					ReturnValue.Add(SplitReq.Substring(9, 2));
				}
				else if (SplitReq.Length > 8 && SplitReq.Length < 10) {
					ReturnValue.Add(SplitReq.Substring(0, 7));
					ReturnValue.Add(SplitReq.Substring(8, 2));
					ReturnValue.Add("");
				}
				else { // SplitReq.Length < 8 
					ReturnValue.Add(SplitReq.Substring(0, 7));
					ReturnValue.Add("");
					ReturnValue.Add("");
				}
				*/
				break;
		}

		return ReturnValue;
	} 
	
}


