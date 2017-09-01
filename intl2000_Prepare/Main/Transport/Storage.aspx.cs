using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transport_Storage : System.Web.UI.Page
{
	protected string Html_TransportHead;
	protected string Html_TransportHead_WithPacked;
	protected string Html_StorageItem;
	protected string Html_TransportBody;
	protected string Html_TransportPacked;
	protected string Html_StorageTab;
	protected static string BranchPk;
	protected static string AccountID;
	protected static string[] MemberInformation;

	protected string TransportHeadPk = "Wait";

	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("/Default.aspx");
		}
		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		BranchPk = MemberInformation[1];
		AccountID = MemberInformation[2];

		TransportC T = new TransportC();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		List<sTransportHead> arrTHead = T.LoadList_TransportHead_WithPacked(BranchPk, "0", ref DB);
		DB.DBCon.Close();

		Html_TransportHead_WithPacked = MakeHtml_TransportHead_WithPacked(arrTHead);
		Html_StorageTab = MakeHtml_StorageTab();
	}

	private string MakeHtml_TransportHead_WithPacked(List<sTransportHead> arrTHead) {
		StringBuilder ReturnValue = new StringBuilder();
		HistoryC HisC = new HistoryC();
		TransportC TransC = new TransportC();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		foreach (sTransportHead THead in arrTHead) {
			int i = 0;

			ReturnValue.Append("<div class=\"col-xs-6\"><section class=\"panel b-primary\">");
			ReturnValue.Append("<div class=\"panel-heading b-b\"><a href=\"#\" class=\"font-bold\">" + THead.Transport_Way + " (" + THead.BranchPk_From + ") B/L: " + THead.Value_String_0 + "</a></div>");
			ReturnValue.Append("<ul class=\"list-group list-group-lg no-bg auto\">");
			ReturnValue.Append("<span class=\"list-group-item clearfix\">" + THead.VesselName + " (" + THead.DateTime_From + " - " + THead.DateTime_To + ")" + "</span>");
			ReturnValue.Append("<span class=\"list-group-item clearfix\">" + THead.Area_From + " - " + THead.Area_To + "</span>");

			ReturnValue.Append("<span class=\"list-group-item clearfix\">");
			List <sComment> Comment = HisC.LoadList_Comment("TRANSPORT_HEAD", THead.Transport_Head_Pk, "'TransportHead'", ref DB);
			for(int j = 0; j < Comment.Count; j++) {
				ReturnValue.Append("<p>" + Comment[j].Contents + "</p>");
				ReturnValue.Append("<small class=\"block text-muted\"><i class=\"fa fa-clock-o\"></i>" + Comment[j].Registerd + "&nbsp &nbsp &nbsp &nbsp &nbsp" + Comment[j].Account_Id + "</small>");
			}
			ReturnValue.Append("</span>");

			ReturnValue.Append("<span class=\"list-group-item clearfix\"><div class=\"col-xs-9\"><input type=\"text\" class=\"form-control\" id=\"Comment_" + THead.Transport_Head_Pk + "\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"button\" class=\"btn btn-primary btn-sm\" value=\"입력\" id=\"Btn_SetComment_" + THead.Transport_Head_Pk + "\" onclick=\"Comment('" + THead.Transport_Head_Pk + "')\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"button\" class=\"btn btn-info btn-sm\" value=\"발송\" id=\"Btn_TransportSend" + THead.Transport_Head_Pk + "\" onclick=\"Transport_Send('" + THead.Transport_Head_Pk + "')\" /></div>");
			ReturnValue.Append("<div class=\"col-xs-1\"><input type=\"button\" class=\"btn btn-danger btn-sm\" value=\"취소\" id=\"Btn_DeleteTransport_" + THead.Transport_Head_Pk + "\" onclick=\"Transport_Cancle('" + THead.Transport_Head_Pk + "')\" /></div></span>");
			ReturnValue.Append("<span class=\"list-group-item clearfix\" id=\"\">");
			foreach (sTransportPacked TPacked in THead.arrPacked) {
				ReturnValue.Append("<div class=\"col-xs-2 text-xs\" style=\"border:dotted; border-width:1px; border-color:coral; height:115px; width:150px; background-color:antiquewhite;\">");
				ReturnValue.Append("<label class=\"checkbox m-n i-checks\"><input type=\"Checkbox\" id=\"ChkHeadPacked_" + i + "\" name=\"ChkHeadPacked_" + i + "\" onclick=\"Chk_Onclick((this).id)\" value=\"" + TPacked.Transport_Packed_Pk + "\" /><i></i></label></br>");
				ReturnValue.Append("<table class=\"table b-t b-light text-xs\">");
				ReturnValue.Append("<tr><td style=\"padding:0px;\">지사</td><td style=\"padding:0px;\">" + TPacked.Company_Code_Owner + "</td></tr>");
				ReturnValue.Append("<tr><td style=\"padding:0px;\">창고</td><td style=\"padding:0px;\">" + TPacked.WareHouse_Name + "</td></tr>");
				ReturnValue.Append("<tr><td style=\"padding:0px;\">NO</td><td style=\"padding:0px;\">" + TPacked.No + "</td></tr>");
				ReturnValue.Append("<tr><td style=\"padding:0px;\">SEAL</td><td style=\"padding:0px;\">" + TPacked.Seal_No + "</td></tr></table>");
				/*
				ReturnValue.Append("Owner:  " + TPacked.Company_Pk_Owner + "</br>");
				ReturnValue.Append("No:  " + TPacked.No + "</br>");
				ReturnValue.Append("Seal:  " + TPacked.Seal_No + "<br>");
				*/
				ReturnValue.Append("</div>");
				ReturnValue.Append("<div class=\"progress progress-xs m-t-xs m-b-none\" style=\"width:150px; margin-top:120px;\"><div class=\"progress-bar bg-danger\" style=\"width:" + TransC.Load_TransportPackedSpace(TPacked.Transport_Packed_Pk, TPacked.Size, ref DB) + "%;\"></div></div>");
				ReturnValue.Append("<div class=\"panel-body form-horizontal\" id=\"Body_" + TPacked.Transport_Packed_Pk + "\"></div>");
				i++;
			}
			ReturnValue.Append("</span>");
			ReturnValue.Append("</ul></section></div>");
		}
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public static string MakeHtml_TransportBody(string TransportPackedPk, string Type) {
		List<sTransportBody> TBody = new List<sTransportBody>();
		StringBuilder ReturnValue = new StringBuilder();
		TransportC TranC = new TransportC();
		string ChkName;
		if (Type == "ChkHeadPacked") {
			ChkName = "ChkHeadBody";
		}
		else { //(Type == "ChkWaitPacked")
			ChkName = "ChkPackedBody";
		}

		DBConn DB = new DBConn();
		DB.DBCon.Open();
		TBody = TranC.LoadList_TransportBody("TransportPackedPk", TransportPackedPk, ref DB);

		string TheadElement = "<th style=\"text-align:left\">CBM</th><th style=\"text-align:left\">Kg</th>";
		if (ChkName == "ChkHeadBody") {
			TheadElement = "";
		}
		ReturnValue.Append("<table class=\"table text-xs\" style=\"margin-bottom:0px; \" ><thead class=\"bg-primary\"><tr><th></th><th style=\"text-align:left\">CT</th><th style=\"text-align:left\">고객</th>" + TheadElement + "<th style=\"text-align:left\">출발지</th><th style=\"text-align:left\">Shipper</th><th style=\"text-align:left\">일정</th><th style=\"text-align:left\">운송</th></tr></thead><tbody>");
		for (int i = 0; i < TBody.Count; i++) {
			ReturnValue.Append("<tr><td>" + "<label class=\"checkbox m-n i-checks\"><input type=\"Checkbox\" id=\"" + ChkName + "_" + i + "\" name=\"" + ChkName + "\" onclick=\"Chk_Onclick((this).id)\" value=\"" + TBody[i].Transport_Body_Pk + "\" />" + "<i></i></label></td>");
			ReturnValue.Append("<td>" + "<input type=\"text\" class=\"form-control\" style=\"width:30px;\" id=\"" + ChkName + "_" + i + "_Count" + "\" value=\"" + TBody[i].Packed_Count + "\" />/" + TBody[i].Packed_Count + "</td>");
			ReturnValue.Append("<td>" + TBody[i].Consignee_Company_Code + "</td>");
			if(ChkName == "ChkHeadBody") {
				ReturnValue.Append("<td style=\"display:none;\" id=\"" + ChkName + "_" + i + "_Volume" + "\">" + TBody[i].Volume + "</td>");
				ReturnValue.Append("<td style=\"display:none;\" id=\"" + ChkName + "_" + i + "_Weight" + "\">" + TBody[i].Weight + "</td>");
			}
			else {
				ReturnValue.Append("<td id=\"" + ChkName + "_" + i + "_Volume" + "\">" + TBody[i].Volume + "</td>");
				ReturnValue.Append("<td id=\"" + ChkName + "_" + i + "_Weight" + "\">" + TBody[i].Weight + "</td>");
			}
			ReturnValue.Append("<td>" + TBody[i].Req_Area_From + "</td>");
			ReturnValue.Append("<td>" + TBody[i].Shipper_Company_Name + "</td>");
			string DateFrom = TBody[i].Req_DateTime_From == "" ? "0000" : TBody[i].Req_DateTime_From.Substring(4, 4);
			string DateTo = TBody[i].Req_DateTime_To == "" ? "0000" : TBody[i].Req_DateTime_To.Substring(4, 4);
			ReturnValue.Append("<td>" + DateFrom + " - " + DateTo + "</td>");
			ReturnValue.Append("<td>" + Common.GetTransportWay(TBody[i].Req_Transport_Way) + "</td></tr>");
		}
		ReturnValue.Append("</tbody></table>");

		DB.DBCon.Close();
		return ReturnValue + "";
	}	

	private static string MakeHtml_TransportPacked(string TransportHeadPk, string WareHousePk) {
		List<sTransportPacked> Packed = new List<sTransportPacked>();
		StringBuilder ReturnValue = new StringBuilder();
		TransportC TranC = new TransportC();

		DBConn DB = new DBConn();
		DB.DBCon.Open();

		Packed = TranC.LoadList_TransportPacked("BranchPk", "", WareHousePk, ref DB);

		for (int i = 0; i < Packed.Count; i++) {
			ReturnValue.Append("<div class=\"col-xs-2\">");
			ReturnValue.Append("<div class=\"form-control text-xs\" style=\"border:dotted; border-width:1px; border-color: coral; height:120px; width:150px; background-color:antiquewhite; \">");
			ReturnValue.Append("<label class=\"checkbox m-n i-checks\"><input type=\"Checkbox\" id=\"ChkWaitPacked_" + i + "\" name=\"ChkWaitPacked_" + i + "\" onclick=\"Chk_Onclick((this).id)\" value=\"" + Packed[i].Transport_Packed_Pk + "\" /><i></i></label><label class=\"control-label col-xs-offset-11 text-danger\" onclick=\"Delete_Packed('" + Packed[i].Transport_Packed_Pk + "')\">X</label><br />");
			ReturnValue.Append("<table class=\"table b-t b-light text-xs\" style=\"border:none;\">");
			ReturnValue.Append("<tr><td style=\"padding:0px;\">지사</td><td style=\"padding:0px;\">" + Packed[i].Company_Code_Owner + "</td></tr>");
			ReturnValue.Append("<tr><td style=\"padding:0px;\">창고</td><td style=\"padding:0px;\">" + Packed[i].WareHouse_Name + "</td></tr>");
			ReturnValue.Append("<tr><td style=\"padding:0px;\">NO</td><td style=\"padding:0px;\">" + Packed[i].No + "</td></tr>");
			ReturnValue.Append("<tr><td style=\"padding:0px;\">SEAL</td><td style=\"padding:0px;\">" + Packed[i].Seal_No + "</td></tr></table>");
			/*
			ReturnValue.Append("Owner:  " + Packed[i].Company_Pk_Owner + "</br>");
			ReturnValue.Append("No:  " + Packed[i].No + "</br>");
			ReturnValue.Append("Seal:  " + Packed[i].Seal_No + "<br>");
			*/
			ReturnValue.Append("</div><div class=\"progress progress-xs m-t-xs m-b-none\" style=\"width:150px; margin-bottom:6px;\"><div class=\"progress-bar bg-danger\" style=\"width:" + TranC.Load_TransportPackedSpace(Packed[i].Transport_Packed_Pk, Packed[i].Size, ref DB) + "%\"></div></div>");
			ReturnValue.Append("</div>");
		}
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	private string MakeHtml_StorageTab() {
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"SELECT 
			TI.[OurBranchStoragePk]
			, TI.[StorageName]
			, SUM([STORAGE_COUNT]) AS SORAGE_COUNT
			, SUM([STORAGE_PACKEDCOUNT]) AS STORAGE_PACKEDCOUNT
			, SUM([PACKED_COUNT]) AS PACEKD_COUNT
		FROM (
				SELECT [OurBranchStoragePk], [StorageName], COUNT(*) AS STORAGE_COUNT, SUM(SR.[PACKED_COUNT]) AS STORAGE_PACKEDCOUNT, '0' AS PACKED_COUNT
				FROM [dbo].[OurBranchStorageCode] AS WH
				INNER JOIN [dbo].[STORAGE] AS SR ON WH.[OurBranchStoragePk] = SR.[WAREHOUSE_PK]
				WHERE SR.[WAREHOUSE_PK] IS NOT NULL
				AND WH.[OurBranchCode] = " + BranchPk + @"
				GROUP BY WH.[OurBranchStoragePk], WH.[StorageName]
				UNION 
				SELECT [OurBranchStoragePk], [StorageName], '0' AS STORAGE_COUNT, '0' AS STORAGE_PACKEDCOUNT, COUNT(*) AS PACKED_COUNT 
				FROM [dbo].[OurBranchStorageCode] AS WH
				INNER JOIN [dbo].[TRANSPORT_PACKED] AS TP ON WH.[OurBranchStoragePk] = TP.[WAREHOUSE_PK]
				WHERE TP.[WAREHOUSE_PK] IS NOT NULL 
				AND TP.[TRANSPORT_HEAD_PK] IS NULL
				AND WH.[OurBranchCode] = " + BranchPk + @"
				GROUP BY WH.[OurBranchStoragePk], WH.[StorageName]
			) AS TI
		GROUP BY TI.[OurBranchStoragePk], TI.[StorageName]
		ORDER BY TI.[StorageName]";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		ReturnValue.Append("<ul class=\"nav nav-tabs\">");
		while (RS.Read()) {
			ReturnValue.Append("<li onclick=\"RetrieveItem('" + RS["OurBranchStoragePk"] + "')\"><a data-toggle=\"tab\" href=\"#tab_" + RS["OurBranchStoragePk"] + "\" >");
			ReturnValue.Append(RS["StorageName"] + "(" + RS["PACEKD_COUNT"] + "/" + RS["SORAGE_COUNT"] + "/" + RS["STORAGE_PACKEDCOUNT"] + ")</a></li>");
		}
		ReturnValue.Append("</ul>");

		RS.Close();
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public static string MakeHtml_StorageItem(string WarehousePk) {
		List<sStorageItem> Item = new List<sStorageItem>();
		StringBuilder ReturnValue = new StringBuilder();
		TransportC TranC = new TransportC();

		DBConn DB = new DBConn();
		DB.DBCon.Open();
		Item = TranC.LoadList_StorageItem(WarehousePk, ref DB);

		ReturnValue.Append("<div class=\"tab-pane active text-xs\" id=\"tab_" + WarehousePk + "\">");
		ReturnValue.Append("<div style=\"outline-width; margin-top:20px; margin-bottom:20px;\" id=\"Pn_PackedBody\"></div>");
		ReturnValue.Append(MakeHtml_TransportPacked("BranchPk", WarehousePk));
		ReturnValue.Append("<div class=\"table-responsive\"><table class=\"table b-t b-light\">");
		ReturnValue.Append("<thead class=\"bg-success\"><tr><th></th><th style=\"text-align:left\">CT</th><th style=\"text-align:left\">고객</th><th style=\"text-align:left\">CBM</th><th style=\"text-align:left\">Kg</th><th style=\"text-align:left\">출발지</th><th style=\"text-align:left\">Shipper</th><th style=\"text-align:left\">일정</th><th style=\"text-align:left\">운송</th></tr></thead>");
		for (int i = 0; i < Item.Count; i++) {
			ReturnValue.Append("<tr><td>" + "<label class=\"checkbox m-n i-checks\"><input type=\"Checkbox\" id=\"ChkStorage_" + i + "\" name=\"ChkStorage\" onclick=\"Chk_Onclick((this).id);\" value=\"" + Item[i].Storage_Pk + "\" />" + "<i></i></label></td>");
			ReturnValue.Append("<td>" + "<input type=\"text\" class=\"form-control\" style=\"width:30px;\" id=\"" + "ChkStorage_" + i + "_Count" + "\" value=\"" + Item[i].Packed_Count + "\" />/" + Item[i].Packed_Count + "</td>");
			ReturnValue.Append("<td>" + Item[i].Consignee_Company_Code + "</td>");
			ReturnValue.Append("<td id=\"ChkStorage" + "_" + i + "_Volume" + "\">" + Item[i].Volume + "</td>");
			ReturnValue.Append("<td id=\"ChkStorage" + "_" + i + "_Weight" + "\">" + Item[i].Weight + "</td>");
			ReturnValue.Append("<td>" + Item[i].Req_Area_From + "</td>");
			ReturnValue.Append("<td>" + Item[i].Shipper_Company_Name + "</td>");
			string DateFrom = Item[i].Req_DateTime_From == "" ? "0000" : Item[i].Req_DateTime_From.Substring(4, 4);
			string DateTo = Item[i].Req_DateTime_To == "" ? "0000" : Item[i].Req_DateTime_To.Substring(4, 4);
			ReturnValue.Append("<td>" + DateFrom + " - " + DateTo + "</td>");
			ReturnValue.Append("<td>" + Common.GetTransportWay(Item[i].Req_Transport_Way) + "</td></tr>");
		}
		ReturnValue.Append("</table></div></div>");
		
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public static string MakeHtml_Modal(string Type) {
		StringBuilder ReturnValue = new StringBuilder();
		TransportC TranC = new TransportC();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		
		switch (Type) {
			case "DialogHead":
				List<sTransportHead> Head = new List<sTransportHead>();
				
				Head = TranC.LoadList_TransportHead(BranchPk, "0", ref DB);

				ReturnValue.Append("<table class=\"table table-striped b-t b-light\"><tbody>");
				for (int i = 0; i < Head.Count; i++) {
					ReturnValue.Append("<tr onclick=\"Click_Dialog('ToHead', " + "'" + Head[i].Transport_Head_Pk + "')\"><td>" + Head[i].Transport_Way + "</td>");
					//ReturnValue.Append("<td>" + Head[i].BranchPk_From + "</td>");
					//ReturnValue.Append("<td>" + Head[i].VesselName + "</td>");
					ReturnValue.Append("<td>" + Head[i].Value_String_0 + "</td>");
					ReturnValue.Append("<td>" + Head[i].DateTime_From + " - " + Head[i].DateTime_To + "</td>");
					ReturnValue.Append("<td>" + Head[i].Area_From + "</td>");
					ReturnValue.Append("<td>" + Head[i].Area_To + "</td></tr>");
				}
				ReturnValue.Append("</tbody>");
				break;
			case "DialogPacked":
				List<sTransportPacked> Packed = new List<sTransportPacked>();
				Packed = TranC.LoadList_TransportPacked("BranchPk", BranchPk, "", ref DB);
				for (int i = 0; i < Packed.Count; i++) {
					ReturnValue.Append("<div class=\"col-xs-6\"><div class=\"form-control text-xs\" onclick=\"Click_Dialog('ToPacked', " + "'" + Packed[i].Transport_Packed_Pk + "')\" style=\"border:dotted; border-width:1px; border-color: coral; height:80px; width:150px; background-color:antiquewhite; \">");
					ReturnValue.Append("<table class=\"table b-t b-light text-xs\" style=\"border:none;\">");
					ReturnValue.Append("<tr><td style=\"padding:0px;\">지사</td><td style=\"padding:0px;\">" + Packed[i].Company_Code_Owner + "</td></tr>");
					ReturnValue.Append("<tr><td style=\"padding:0px;\">창고</td><td style=\"padding:0px;\">" + Packed[i].WareHouse_Name + "</td></tr>");
					ReturnValue.Append("<tr><td style=\"padding:0px;\">NO</td><td style=\"padding:0px;\">" + Packed[i].No + "</td></tr>");
					ReturnValue.Append("<tr><td style=\"padding:0px;\">SEAL</td><td style=\"padding:0px;\">" + Packed[i].Seal_No + "</td></tr></table>");
					/*
					ReturnValue.Append("Owner:  " + Packed[i].Company_Pk_Owner + "</br>");
					ReturnValue.Append("No:  " + Packed[i].No + "</br>");
					ReturnValue.Append("Seal:  " + Packed[i].Seal_No + "<br>");
					*/
					ReturnValue.Append("</div><div class=\"progress progress-xs m-t-xs m-b-none\" style=\"width:150px; margin-bottom:20px;\"><div class=\"progress-bar bg-danger\" style=\"width:" + TranC.Load_TransportPackedSpace(Packed[i].Transport_Packed_Pk, Packed[i].Size, ref DB) + "%\"></div></div></div>");
				}
				break;
			case "DialogStorage":
				TransportP TransP = new TransportP();
				ReturnValue.Append(TransP.LoadBranchStorage(BranchPk));
				ReturnValue.Append("<input type=\"button\" class=\"btn btn-default\" style=\"align:right;\" value=\"창고선택\"  onclick=\"Click_Dialog('ToStorage', " + "" + "$('#St_Storage').val()" + ")\"/>");
				break;
		}

		DB.DBCon.Close();	
		return ReturnValue + "";
	}

	[WebMethod]
	public static string Transport_Send(string TransportHeadPk) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"UPDATE [dbo].[TRANSPORT_HEAD] SET [TRANSPORT_STATUS] = 1 WHERE [TRANSPORT_PK] = " + TransportHeadPk;
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public static string Transport_Cancle(string TransportHeadPk) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT COUNT(*) FROM [dbo].[TRANSPORT_PACKED] WHERE [TRANSPORT_HEAD_PK] = " + TransportHeadPk;
		string Count = DB.SqlCmd.ExecuteScalar() + "";
		
		if (Count == "0") {
			DB.SqlCmd.CommandText = @"DELETE FROM [dbo].[TRANSPORT_HEAD] WHERE [TRANSPORT_PK] = " + TransportHeadPk + @";
										DELETE FROM [dbo].[File] WHERE [GubunCL] = 0 and [GubunPk] = " + TransportHeadPk + @";";
			DB.SqlCmd.ExecuteNonQuery();
			DB.DBCon.Close();
			return "1";
		}
		else {
			DB.DBCon.Close();
			return "-1";
		}
	}

}