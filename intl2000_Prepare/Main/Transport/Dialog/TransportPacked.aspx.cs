using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_Dialog_TransportPacked : System.Web.UI.Page
{
	protected string Seq = "";
	protected sTransportPacked TPacked;
	protected string pType = "";
	protected string pTypePk = "";
	protected string pBranchPk = "";

	protected void Page_Load(object sender, EventArgs e) {
		try { switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; } } catch (Exception) { }

		DBConn DB = new DBConn();
		DB.DBCon.Open();

		pType = Request.Params["Type"];
		pTypePk = Request.Params["TypePk"];
		pBranchPk = Request.Params["CompanyPk"];

		if (pTypePk != null) {
			TransportC TransC = new TransportC();
			TPacked = TransC.Load_TransportPackedOnly(pTypePk, ref DB);
		} else {
			TPacked = new sTransportPacked() {
				Transport_Packed_Pk = "",
				Seq = "",
				Transport_Head_Pk = "",
				Company_Pk_Owner = "",
				Container_Company = "",
				Type = "",
				No = "",
				Size = "",
				Seal_No = ""
			};
		}

		if (TPacked.Transport_Packed_Pk == "" || TPacked.Transport_Packed_Pk == null) {
			DB.SqlCmd.CommandText = @"SELECT MAX(SEQ) + 1 AS SEQ FROM [dbo].[TRANSPORT_PACKED]";
			TPacked.Seq = DB.SqlCmd.ExecuteScalar() + "";
			if (TPacked.Seq == "" || TPacked.Seq == null) {
				TPacked.Seq = "1";
			}
		}
		
		DB.DBCon.Close();

    }
}