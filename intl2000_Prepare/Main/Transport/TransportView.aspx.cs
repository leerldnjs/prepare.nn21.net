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

public partial class Transport_TransportView :System.Web.UI.Page
{
	protected string Html_TransportView;
	protected string Html_RetrieveBody;
	protected string TransportHeadPk;
	protected string pHeadPk;
	protected string pStatus;
	protected string pWarehouse;
	protected string pPackedCount;
	protected sTransportHead Head = new sTransportHead();
	protected string Html_Comment;
	protected string Html_ChargeList;
	protected string Html_FileList;
	protected string[] MemberInformation;
	protected static string Accountid;
	

	protected void Page_Load(object sender, EventArgs e) {
		pHeadPk = Request.Params["S"].ToString();
		Html_TransportView = MakeHtml_TransportView(pHeadPk);
		Html_Comment = MakeHtml_Comment();
		Html_ChargeList = MakeHtml_ChargeList();
		Html_FileList = MakeHtml_FileList();

		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("/Default.aspx");
		}
		MemberInformation = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		Accountid = MemberInformation[2];
	}

	private string MakeHtml_Comment() {
		StringBuilder ReturnValue = new StringBuilder();
		HistoryC HisC = new HistoryC();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		List<sComment> Comment = HisC.LoadList_Comment("TRANSPORT_HEAD", pHeadPk, "'HeadComment'", ref DB);
		for (int i = 0; i < Comment.Count; i++) {
			ReturnValue.Append("<li class=\"list-group-item\">");
			ReturnValue.Append("<p>" + Comment[i].Contents + "</p>");
			ReturnValue.Append("<small class=\"block text-muted\"><i class=\"fa fa-clock-o\"></i>" + Comment[i].Registerd + "&nbsp &nbsp &nbsp &nbsp &nbsp" + Comment[i].Account_Id + "</small></li>");
		}
		DB.DBCon.Close();

		return ReturnValue + "";
	}

	private string MakeHtml_TransportView(string TransportHeadPk) {
		TransportC TransC = new TransportC();
		StringBuilder ReturnValue = new StringBuilder();
		string MRN = "";
		string MSN = "";

		DBConn DB = new DBConn();
		DB.DBCon.Open();
		Head = TransC.Load_TransportHead(pHeadPk, ref DB);
		ReturnValue.Append("<div class=\"row\"><div class=\"col-xs-6\" style=\"margin-top:10px;\"><small class=\"text-danger\">Vessel Name :</small><div class=\"text-lg font-bold\">" + Head.VesselName + "</div></div>");
		ReturnValue.Append("<div class=\"col-xs-6\" style=\"margin-top:10px;\"><small class=\"text-danger\">Master B/L :</small><div class=\"text-lg font-bold\">" + Head.Value_String_0 + ""+ "</div></div></div>");
		ReturnValue.Append("<div class=\"row\"><div class=\"col-xs-6\" style=\"margin-top:10px;\"><small class=\"text-danger\">선적항 :</small><div class=\"text-lg font-bold\">" + Head.Area_From + "</div></div>");
		ReturnValue.Append("<div class=\"col-xs-6\" style=\"margin-top:10px;\"><small class=\"text-danger\">출발예정일 :</small><div class=\"text-lg font-bold\">" + Head.DateTime_From + "</div></div></div>");
		ReturnValue.Append("<div class=\"row\"><div class=\"col-xs-6\" style=\"margin-top:10px;\"><small class=\"text-danger\">도착항 :</small><div class=\"text-lg font-bold\">" + Head.Area_To + "</div></div>");
		ReturnValue.Append("<div class=\"col-xs-6\" style=\"margin-top:10px;\"><small class=\"text-danger\">도착예정일 :</small><div class=\"text-lg font-bold\">" + Head.DateTime_To + "</div></div></div>");
		ReturnValue.Append("<div class=\"row\"><div class=\"col-xs-6\" style=\"margin-top:10px;\"><small class=\"text-danger\">출발지연락처 :</small><div class=\"text-lg font-bold\">" + Head.Value_String_1 + "</div></div>");
		ReturnValue.Append("<div class=\"col-xs-6\" style=\"margin-top:10px;\"><small class=\"text-danger\">도착지연락처 :</small><div class=\"text-lg font-bold\">" + Head.Value_String_2 + "</div></div></div>");
		ReturnValue.Append("<div class=\"row\"><div class=\"col-xs-6\" style=\"margin-top:10px;\"><small class=\"text-danger\">차번호 :</small><div class=\"text-lg font-bold\">" + Head.Value_String_3 + "</div></div>");
		ReturnValue.Append("<div class=\"col-xs-6\" style=\"margin-top:10px;\"><small class=\"text-danger\">항차 :</small><div class=\"text-lg font-bold\">" + Head.Voyage_No + "</div></div></div>");
		DB.SqlCmd.CommandText = @"SELECT [TransportBBPk], [MRN], [MSN] FROM [dbo].[CommercialDocumentDO] WHERE [TransportBBPk] = " + pHeadPk;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string BtnDoFlag;
		if (RS.Read()) {
			MRN = RS["MRN"] + "";
			MSN = RS["MSN"] + "";
			BtnDoFlag = "<div class=\"col-xs-1\"><input id=\"Btn_DOUpdate\" type=\"button\" value=\"수정\" class=\"btn btn-primary btn-sm\" style=\"margin-top:30px;\" onclick=\"DO_Update()\" /></div></div>";
		}
		else {
			BtnDoFlag = "<div class=\"col-xs-1\"><input id=\"Btn_DOInsert\" type=\"button\" value=\"입력\" class=\"btn btn-primary btn-sm\" style=\"margin-top:30px;\" onclick=\"DO_Insert()\" /></div></div>";
		}
		ReturnValue.Append("<div class=\"row\"><div class=\"col-xs-4\" style=\"margin-top:10px;\"><small class=\"text-danger\">MRN :</small><div class=\"\"><input type=\"text\" id=\"MRN\" class=\"form-control\" value=\"" + MRN + "\"/></div></div>");
		ReturnValue.Append("<div class=\"col-xs-4 col-xs-offset-2\" style=\"margin-top:10px;\"><small class=\"text-danger\">MSN :</small><div class=\"\"><input type=\"text\" id=\"MSN\" class=\"form-control\" value=\"" + MSN + "\"/></div></div>");
		ReturnValue.Append(BtnDoFlag);

		pStatus = Head.Transport_Status;
		pWarehouse = Head.Warehouse_Pk_Arrival;
		pPackedCount = Head.arrPacked.Count + "";

		for (int i = 0; i < Head.arrPacked.Count; i++) {
			ReturnValue.Append("<div class=\"col-xs-6\" style=\"margin-top:15px;\">");
			ReturnValue.Append("<div class=\"text-xs\" style=\"border:dotted; border-width:1px; border-color:coral; height:80px; width:150px; background-color:antiquewhite; \" onclick=\"Retrieve_Body('" + Head.arrPacked[i].Transport_Packed_Pk + "')\">");
			ReturnValue.Append("<input type=\"hidden\" id=\"PackedPk_" + i + "\" value=\"" + Head.arrPacked[i].Transport_Packed_Pk + "\" />");
			ReturnValue.Append("지사:" + Head.arrPacked[i].Company_Code_Owner + "</br>");
			ReturnValue.Append("No:" + Head.arrPacked[i].No + "</br>");
			ReturnValue.Append("Seal:" + Head.arrPacked[i].Seal_No + "<br>");
			ReturnValue.Append("<input type=\"button\" class=\"btn btn-xs btn-warning\" value=\"조회\" style=\"margin-top:5px; margin-left:112px;\"/></div>");
			ReturnValue.Append("</div>");
		}

		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public static string MakeHtml_RetrieveBody(string TransportPackedPk) {
		TransportC TransC = new TransportC();
		List<sTransportBody> Body = new List<sTransportBody>();
		StringBuilder ReturnValue = new StringBuilder();

		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"SELECT 
			[TRANSPORT_BODY_PK]
			,[TRANSPORT_HEAD_PK]
			,[TRANSPORT_PACKED_PK]
			,[WAREHOUSE_PK_DEPARTURE]
			,[REQUEST_PK]
			,[SHIPPER_COMPANY_PK]
			,[CONSIGNEE_COMPANY_PK]
			,[SHIPPER_COMPANY_CODE]
			,[CONSIGNEE_COMPANY_CODE]
			,[SHIPPER_COMPANY_NAME]
			,[CONSIGNEE_COMPANY_NAME]
			,[PACKED_COUNT]
			,[PACKING_UNIT]
			,[DESCRIPTION]
			,[WEIGHT]
			,[VOLUME]
			,RC.[Name] AS [AREA_FROM]
			,RF.[DepartureDate] AS [DEPARTURE_DATE]
			,RF.[ArrivalDate] AS [ARRIVAL_DATE]
			,RF.[TransportWayCL] AS [TRANSPORT_WAY]
			,CR.[CommercialDocumentPk]
			,RF.[DocumentRequestCL]
			,OB.[FilePk]
		FROM [dbo].[TRANSPORT_BODY] AS TB
		LEFT JOIN [dbo].[RequestForm] AS RF ON TB.[REQUEST_PK] = RF.[RequestFormPk]
		LEFT JOIN [dbo].[RegionCode] AS RC ON RF.[DepartureRegionCode] = RC.[RegionCode]
		LEFT JOIN [dbo].[CommerdialConnectionWithRequest] AS CR ON RF.[RequestFormPk] = CR.RequestFormPk
		LEFT JOIN (SELECT [filePk],[GubunPk] FROM [dbo].[File] WHERE [gubuncl] = 18) AS OB ON TB.[REQUEST_PK] = OB.[GubunPk]
		WHERE [TRANSPORT_PACKED_PK] = " + TransportPackedPk;

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("<tr class=\"text-sm\"><td>" + RS["SHIPPER_COMPANY_NAME"] + "</td>");
			ReturnValue.Append("<td>" + RS["CONSIGNEE_COMPANY_NAME"] + "</td>");
			ReturnValue.Append("<td onclick=\"location.href='/Admin/RequestView.aspx?g=s&pk=" + RS["REQUEST_PK"] + "'\"><a>" + RS["DEPARTURE_DATE"] + "</a></td>");
			ReturnValue.Append("<td onclick=\"location.href='/Admin/RequestView.aspx?g=s&pk=" + RS["REQUEST_PK"] + "'\"><a>" + RS["ARRIVAL_DATE"] + "</a></td>");
			ReturnValue.Append("<td>" + RS["PACKED_COUNT"] + "</td>");
			ReturnValue.Append("<td>" + RS["VOLUME"] + "</td>");
			ReturnValue.Append("<td>" + RS["WEIGHT"] + "</td>");

			string InputBtn = "<input type=\"button\" class=\"btn btn-default btn-xs\" value=\"YT\" style=\"width:25px; padding:0px;  \" onclick=\"Print('B_YT', '" + RS["CommercialDocumentPk"] + "','');\" />&nbsp;" +
						"<input type=\"button\" class=\"btn btn-default btn-xs\" value=\"YT2\" style=\"width:25px; padding:0px;  \" onclick=\"Print('B_YT2', '" + RS["CommercialDocumentPk"] + "','');\" />&nbsp;" +
						"<input type=\"button\" class=\"btn btn-default btn-xs\" value=\"B\" style=\"width:20px; padding:0px;  \" onclick=\"Print('B', '" + RS["CommercialDocumentPk"] + "','" + RS["FilePk"] + "');\" />&nbsp;" +
						"<input type=\"button\" class=\"btn btn-default btn-xs\" value=\"I\" style=\"width:20px; padding:0px;  \"  onclick=\"Print('I', '" + RS["CommercialDocumentPk"] + "','');\" />&nbsp;" +
						"<input type=\"button\" class=\"btn btn-default btn-xs\" value=\"P\" style=\"width:20px; padding:0px;  \"  onclick=\"Print('P', '" + RS["CommercialDocumentPk"] + "','');\" />&nbsp;" +
						"<input type=\"button\" class=\"btn btn-default btn-xs\" value=\"DO\" style=\"width:25px; padding:0px;  \" onclick=\"Print('DO', '" + RS["CommercialDocumentPk"] + "','');\" />&nbsp;" +
						"<input type=\"button\" class=\"btn btn-default btn-xs\" value=\"D\" style=\"width:20px; padding:0px;  \" onclick=\"Print('D', '" + RS["REQUEST_PK"] + "','');\" />";
			/*
			HistoryC HisC = new HistoryC();
			List<sHistory> History = HisC.LoadList_History("RequestForm", RS["REQUEST_PK"] + "", "0", ref DB);
			StringBuilder HisString = new StringBuilder();
			for (int i = 0; i < History.Count; i++) {
				HisString.Append("<strong>" + History[i].Description + "</string>" + History[i].Account_Id + " - " + History[i].Registerd);
			}
			*/
			string[] Documents = RS["DocumentRequestCL"].ToString().Split(Common.Splite11, StringSplitOptions.RemoveEmptyEntries);
			string HtmlDocumentRequest = "";
			foreach (string T in Documents) {
				switch (T) {
					case "31":
						HtmlDocumentRequest += "화주원산지제공(客户提供产地证) ";
						break;
					case "32":
						HtmlDocumentRequest += "원산지신청(申请产地证) ";
						break;
					case "33":
						HtmlDocumentRequest += "단증보관(单证报关) ";
						break;
					case "34":
						HtmlDocumentRequest += "FTA원산지대리신청(FTA代理申请产地证) ";
						break;
				}
			}

			ReturnValue.Append("<td>" + InputBtn + RS["DESCRIPTION"] + /*HisString +*/ HtmlDocumentRequest + "</td></tr>");
		}

		RS.Close();
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	private string MakeHtml_ChargeList() {
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT TBBC.[TransportBBChargePk]
			,TBBC.[TransportBBHeadPk]
			,TBBC.[PaymentBranchPk]
			,TBBC.[Date]
			,TBBC.[Title]
			,TBBC.[MonetaryUnitCL]
			,TBBC.[Price]
			,TBBC.[Registerd]
			,TBBC.[Comment]
			, F.FilePk
		FROM [dbo].[TransportBBCharge] AS TBBC
		left join(
			select* FROM [dbo].[File] WHERE ISNULL(GubunCL, '31')='31'
			) AS F ON TBBC.[TransportBBChargePk]= F.GubunPk
		WHERE TBBC.TransportBBHeadPk = " + pHeadPk;

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("<li class=\"list-group-item col-xs-12\">");
			ReturnValue.Append("<span class=\"col-xs-4\">" + RS["Title"] + "<span style=\"color:red;\" onclick=\"DeleteTransportBBCharge('" + RS["TransportBBChargePk"] + "')\"> X</span></span>");
			ReturnValue.Append("<span class=\"col-xs-4\">" + Common.GetMonetaryUnit(RS["MonetaryUnitCL"] + "") + " " + Common.NumberFormat(RS["Price"].ToString()) + "</span>");
			ReturnValue.Append("<a href=\"/UploadedFiles/FileDownload.aspx?S=" + RS["FilePk"] + "\" class=\"col-xs-4\">File</a></li>");
		}
		RS.Close();
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	private string MakeHtml_FileList() {
		StringBuilder ReturnValue = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT [FilePk], [Title], [FileName] FROM [dbo].[File] WHERE GubunCL=0 and GubunPk=" + pHeadPk;
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append("<li class=\"list-group-item col-xs-12\">");
			ReturnValue.Append("<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["FilePk"] + ">" + RS["Title"] + ":" + RS["FileName"] + "</a>" + "<span style=\"color:red;\" onclick=\"DeleteTransportBBCharge('" + RS["FilePk"] + "')\"> X</span></span></li>");
		}
		RS.Close();
		DB.DBCon.Close();
		return ReturnValue + "";
	}

	[WebMethod]
	public static string Assign_Stroage(string TransportHeadPk, string WarehousePk) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = "UPDATE [dbo].[TRANSPORT_HEAD] SET [WAREHOUSE_PK_ARRIVAL] = " + WarehousePk + " WHERE [TRANSPORT_PK] = " + TransportHeadPk + " ;";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();

		return "1";
	}

	[WebMethod]
	public static string Transport_ToStep(string TransportHeadPk, string TransportStatus) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		switch (TransportStatus) {
			case "1":
				DB.SqlCmd.CommandText = "UPDATE [dbo].[TRANSPORT_HEAD] SET [TRANSPORT_STATUS] = 2 WHERE [TRANSPORT_PK] = " + TransportHeadPk + " ;";
				DB.SqlCmd.ExecuteNonQuery();
				break;
			case "2":
				DB.SqlCmd.CommandText = "UPDATE [dbo].[TRANSPORT_HEAD] SET [TRANSPORT_STATUS] = 3 WHERE [TRANSPORT_PK] = " + TransportHeadPk + " ;";
				DB.SqlCmd.ExecuteNonQuery();
				Copy_Table(TransportHeadPk, ref DB);
				Set_StorageOut(TransportHeadPk, ref DB);
				break;
		}
		DB.DBCon.Close();
		return "1";
	}

	private static string Copy_Table(string TransportHeadPk, ref DBConn DB) {
		DB.SqlCmd.CommandText = @"INSERT INTO [dbo].[TRANSPORT_PACKED] 
		([SEQ]
		,[WAREHOUSE_PK]
		,[COMPANY_PK_OWNER]
		,[CONTAINER_COMPANY]
		,[TYPE]
		,[NO]
		,[SIZE]
		,[SEAL_NO]
		,[REALPACKED_FLAG])
		(SELECT 
		[SEQ]
		,[WAREHOUSE_PK_ARRIVAL]
		,[COMPANY_PK_OWNER]
		,[CONTAINER_COMPANY]
		,[TYPE]
		,[NO]
		,[SIZE]
		,[SEAL_NO]
		,[REALPACKED_FLAG]	
		FROM [dbo].[TRANSPORT_PACKED] AS PACKED
		LEFT JOIN [dbo].[TRANSPORT_HEAD] AS HEAD ON HEAD.TRANSPORT_PK = PACKED.TRANSPORT_HEAD_PK
		WHERE [TRANSPORT_HEAD_PK] = " + TransportHeadPk + @"); SELECT @@IDENTITY";
		string PackedPk = DB.SqlCmd.ExecuteScalar() + "";

		DB.SqlCmd.CommandText = @"INSERT INTO [dbo].[TRANSPORT_BODY]
		([TRANSPORT_PACKED_PK]
		,[LAST_TRANSPORT_HEAD_PK]
		,[LAST_TRANSPORT_PACKED_PK]
		,[REQUEST_PK]
		,[SHIPPER_COMPANY_PK]
		,[CONSIGNEE_COMPANY_PK]
		,[SHIPPER_COMPANY_CODE]
		,[CONSIGNEE_COMPANY_CODE]
		,[SHIPPER_COMPANY_NAME]
		,[CONSIGNEE_COMPANY_NAME]
		,[PACKED_COUNT]
		,[PACKING_UNIT]
		,[DESCRIPTION]
		,[WEIGHT]
		,[VOLUME])
		(SELECT 
		" + PackedPk + @"
		,[TRANSPORT_HEAD_PK]
		,[TRANSPORT_PACKED_PK]
		,[REQUEST_PK]
		,[SHIPPER_COMPANY_PK]
		,[CONSIGNEE_COMPANY_PK]
		,[SHIPPER_COMPANY_CODE]
		,[CONSIGNEE_COMPANY_CODE]
		,[SHIPPER_COMPANY_NAME]
		,[CONSIGNEE_COMPANY_NAME]
		,[PACKED_COUNT]
		,[PACKING_UNIT]
		,[DESCRIPTION]
		,[WEIGHT]
		,[VOLUME]
		FROM [dbo].[TRANSPORT_BODY] AS BODY
		LEFT JOIN [dbo].[TRANSPORT_HEAD] AS HEAD ON HEAD.TRANSPORT_PK = BODY.TRANSPORT_HEAD_PK
		WHERE [TRANSPORT_HEAD_PK] = " + TransportHeadPk + @");";
		DB.SqlCmd.ExecuteNonQuery();

		return "1";
	}
	
	private static string Set_StorageOut(string TransportHeadPk, ref DBConn DB) {
		HistoryC HisC = new HistoryC();
		sHistory History;
		SqlDataReader RS;
		string WareHousePk = "";
		string WareHouseName = "";
		List<string> RequestPk = new List<string>();
		StringBuilder Query = new StringBuilder();
		DB.SqlCmd.CommandText = @"SELECT HEAD.[WAREHOUSE_PK_ARRIVAL], OBSC.[StorageName] 
		FROM [dbo].[TRANSPORT_HEAD] AS HEAD 
		LEFT JOIN [dbo].[OurBranchStorageCode] AS OBSC ON HEAD.[WAREHOUSE_PK_ARRIVAL] = OBSC.[OurBranchStoragePk]
		WHERE HEAD.[TRANSPORT_PK] = " + TransportHeadPk;
		RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			WareHousePk = RS["WAREHOUSE_PK_ARRIVAL"] + "";
			WareHouseName = RS["StorageName"] + "";
		}

		string Comment = "<a href=\"TransportView.aspx?S=" + TransportHeadPk + "\">To " + WareHouseName + "</a>";

		RS.Close();

		DB.SqlCmd.CommandText = @"SELECT HEAD.[WAREHOUSE_PK_ARRIVAL], BODY.[REQUEST_PK], BODY.[PACKED_COUNT], RO.[OurBranchStorageOutPk], RO.[BoxCount] AS [ALEADY_SET]
		FROM [dbo].[TRANSPORT_HEAD] AS HEAD
		LEFT JOIN [dbo].[TRANSPORT_BODY] AS BODY ON HEAD.[TRANSPORT_PK] = BODY.[TRANSPORT_HEAD_PK]
		LEFT JOIN [dbo].[RequestForm] AS RF ON BODY.[REQUEST_PK] = RF.[RequestFormPk]
		LEFT JOIN (SELECT [OurBranchStorageOutPk], [RequestFormPk], [BoxCount] FROM [dbo].[OurBranchStorageOut] WHERE isnull([TransportBetweenBranchPk], 0)=0 ) AS RO ON RO.[RequestFormPk]=BODY.[REQUEST_PK]
		WHERE HEAD.[TRANSPORT_PK] = " + TransportHeadPk;
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			RequestPk.Add(RS["REQUEST_PK"] + "");
			
			if (RS["PACKED_COUNT"] + "" == RS["ALEADY_SET"] + "") {
				Query.Append(@"UPDATE [dbo].[OurBranchStorageOut] SET 
					[StorageCode] = " + WareHousePk + @"
					, [StockedDate] = getDate()
					, [TransportBetweenBranchPk] = " + TransportHeadPk + @"
					, [StatusCL] = 5
				WHERE [OurBranchStorageOutPk] = " + RS["OurBranchStorageOutPk"] + "");
			}
			else if (RS["ALEADY_SET"] + "" == "") {
				Query.Append(@"INSERT INTO [dbo].[OurBranchStorageOut] ([StorageCode], [RequestFormPk], [BoxCount], [StockedDate], [TransportBetweenBranchPk], [StatusCL]) VALUES 
				(" + WareHousePk + ", " + RS["REQUEST_PK"] + ", " + RS["PACKED_COUNT"] + ", getDate(), " + TransportHeadPk + ", 4)");
			}
			else {
				int PackedCount = Int32.Parse(RS["PACKED_COUNT"] + "");
				int AleadySet = Int32.Parse(RS["ALEADY_SET"] + "");

				if (PackedCount < AleadySet) {
					Query.Append(@"UPDATE [dbo].[OurBranchStorageOut] SET
						[StorageCode] = " + WareHousePk + @"
						, [StockedDate] = getDate()
						, [TransportBetweenBranchPk] = " + TransportHeadPk + @"
						, [StatusCL] = 5
					WHERE [OurBranchStorageOutPk] = " + RS["OurBranchStorageOutPk"] + "");
				}
				else {
					Query.Append(@"UPDATE [dbo].[OurBranchStorageOut] SET
						[StorageCode] = " + WareHousePk + @"
						, [BoxCount] = " + AleadySet + @"
						, [StockedDate] = getDate()
						, [TransportBetweenBranchPk] = " + TransportHeadPk + @"
						, [StatusCL] = 5
					WHERE [OurBranchStorageOutPk] = " + RS["OurBranchStorageOutPk"] + "");
				}

			}
		}
		RS.Close();

		for(int i = 0; i < RequestPk.Count; i++) {
			History = new sHistory();
			History.Table_Name = "RequestForm";
			History.Table_Pk = RequestPk[i];
			History.Code = "66";
			History.Account_Id = Accountid;
			History.Description = Comment;
			HisC.Set_History(History, ref DB);
		}
		Query.Append(@"INSERT INTO [dbo].[TransportBBHistory] ([StorageCode], [RequestFormPk], [BoxCount], [TransportBetweenBranchPk])
		SELECT " + WareHousePk + @", [REQUEST_PK], [PACKED_COUNT], [TRANSPORT_HEAD_PK] FROM [dbo].[TRANSPORT_BODY] WHERE [TRANSPORT_HEAD_PK] = " + TransportHeadPk);
		DB.SqlCmd.CommandText = Query + "";
		DB.SqlCmd.ExecuteNonQuery();

		return "1";
	}



}