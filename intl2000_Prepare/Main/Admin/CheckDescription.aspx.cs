using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;
using System.Text;

public partial class Admin_CheckDescription : System.Web.UI.Page
{
	protected String CommercialDocumentPk;
	protected String[] MemberInfo;
	protected String Gubun;
	protected String PageDate;
	protected String RequestList;
	protected String Onlyilic66BTN;
	private bool IsOnlyilic66;
	protected Int32 Count;
	private DBConn DB;
	protected String InputBTN;
	protected String BTN_SetClearance_Ready;
	protected String Comment;
	protected String List_RequestComment;
	protected String AttachedFiles;
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) {
			Response.Redirect("../Default.aspx");
		}
		try {
			if (Request["Language"].Length == 2) {
				Session["Language"] = Request["Language"];
			}
		} catch (Exception) {
		}
		switch (Session["Language"] + "") {
			case "en":
				Page.UICulture = "en";
				break;
			case "ko":
				Page.UICulture = "ko";
				break;
			case "zh":
				Page.UICulture = "zh-cn";
				break;
		}
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);

		CommercialDocumentPk = Request.Params["S"];

		string RequestPk = GetRequestPk(CommercialDocumentPk);
		IsOnlyilic66 = Onlyilic66(RequestPk);


		if (MemberInfo[0] == "Customs") {
			LogedWithoutRecentRequest111.Visible = false;
			Loged1.Visible = true;
			if (!IsOnlyilic66) {
				Onlyilic66BTN = "<input type=\"button\" value=\"관부가세 계산\" onclick=\"SetCalc();\" />&nbsp;" +
				"<input type=\"button\" value=\"저장\" onclick=\"SetSave();\" />&nbsp;";
				InputBTN = "<input type=\"button\" value=\"Ex\" style=\"width:20px; padding:0px;  \"  onclick=\"InvoiceExcelDown('" + CommercialDocumentPk + "');\" /><input type=\"button\" value=\"관부가세 확정\" onclick=\"Send('5');\" />";
			} else {
				Onlyilic66BTN = ""; InputBTN = "";
			}

		} else {
			if (!IsOnlyilic66) {
				InputBTN =
				//"<input type=\"button\" value=\"성심관세사로 전송\" onclick=\"SendSungSim('2');\" />" + 
				//"<input type=\"button\" value=\"관세사로 전송\" onclick=\"Send('2');\" />" +
				"<input type=\"button\" value=\"자가통관\" onclick=\"Send('3');\" />" +
				"<input type=\"button\" value=\"샘플\" onclick=\"Send('4');\" />" +
				"<br /><input type=\"button\" value=\"Ex\" style=\"width:20px; padding:0px;  \"  onclick=\"InvoiceExcelDown('" + CommercialDocumentPk + "');\" />&nbsp;&nbsp;<input type=\"button\" value=\"관부가세 확정\" onclick=\"Send('5');\" />";
				Onlyilic66BTN = "<input type=\"button\" value=\"관부가세 계산\" onclick=\"SetCalc();\" />&nbsp;" +
				"<input type=\"button\" value=\"저장\" onclick=\"SetSave();\" />&nbsp;";
			} else {
				InputBTN =

				  "<br /><input type=\"button\" value=\"Ex\" style=\"width:20px; padding:0px;  \"  onclick=\"InvoiceExcelDown('" + CommercialDocumentPk + "');\" />&nbsp;&nbsp;";
				Onlyilic66BTN = "";
			}


		}

		if (MemberInfo[2] == "ilic31" || MemberInfo[2] == "ilic30" || MemberInfo[2] == "ilic55" || MemberInfo[2] == "ilic66" || MemberInfo[2] == "ilic77") {
			InputBTN += "<p><input type=\"button\" value=\"한중FTA\" onclick=\"goto_hscode();\" /><input type=\"button\" value=\"관세사와 정산\" onclick=\"ModalOpenSetSettlementWithCustoms();\" /></p>";
		} else {
			InputBTN += "<p><input type=\"button\" value=\"한중FTA\" onclick=\"goto_hscode();\" /></p>";
		}

		DB = new DBConn();
		//20141230 성심관세사 임시처리
		//PageDate = MemberInfo[0] == "Customs" ? LoadPageDateForCustoms(MemberInfo[1]) : LoadPageDate(MemberInfo[1]);
		PageDate = MemberInfo[0] == "Customs" ? "" : LoadPageDate(MemberInfo[1]);
		Comment = LoadCommentList(CommercialDocumentPk, MemberInfo[2]);
		List_RequestComment = LoadRequestComment(CommercialDocumentPk, "");
		AttachedFiles = LoadFileList(CommercialDocumentPk);
		//string gubun=Request.Params["G"]+""==""?"O":Request.Params["G"]+"";
		//string date=Request.Params["D"]+""==""?"A":Request.Params["D"]+"";


		if (MemberInfo[1] == "3157" && MemberInfo[2] != "ilic55" && MemberInfo[2] != "ilic66" && MemberInfo[2] != "ilic77") {

			BTN_SetClearance_Ready = "<input type=\"button\" value=\"Ready_Go\" onclick=\"SetClearance_Ready();\" style=\"color:red;\"/>";
		}


	}
	private string GetRequestPk(string CommercialDocumentPk) {
		DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = string.Format(@"
SELECT  top 1 [RequestFormPk]      
  FROM [dbo].[CommerdialConnectionWithRequest]
  where  [CommercialDocumentPk] ='{0}'
  order by [CommercialDocumentPk]  desc", CommercialDocumentPk);
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string TempRequestPk = "";
		if (RS.Read()) {
			TempRequestPk = RS["RequestFormPk"].ToString();
		}
		return TempRequestPk;
	}
	private bool Onlyilic66(string RequestFormPk) {
		DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = string.Format(@"
SELECT [CODE]
  FROM [dbo].[HISTORY]
  WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]={0} AND [CODE] IN ('16', '17') ORDER BY [CODE];", RequestFormPk);
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int Count = 0;
		while (RS.Read()) {
			if (RS[0] + "" == "16") {
				Count++;
			} else {
				Count--;
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		Boolean ReturnValue = false;
		if (Count > 0) {
			ReturnValue = true;
		}
		return ReturnValue;
	}
	private string LoadFileList(string CommercialDocumentPk) {
		string fileROW = "<tr style=\"height:20px; \"><td class='{7}' >{1}</td><td class='{7}' style=\"text-align:left;\"><a href='../UploadedFiles/FileDownload.aspx?S={0}&T={8}' >{2}</a></td><td class='{7}' >{3}</td><td class='{7}' >{4}</td><td class='{7}' >{5}</td><td class='{7}' >{6}</td><td class='{7}' ><span onclick=\"FileDelete('{0}', '{8}');\" style='color:red;'>X</span></td></tr>";

		DB.SqlCmd.CommandText = @"SELECT [ClearancedFilePk], [GubunCL], [PhysicalPath] FROM ClearancedFile WHERE RequestFormPk in (
SELECT [RequestFormPk]
  FROM [dbo].[CommerdialConnectionWithRequest]
  WHERE CommercialDocumentPk=" + CommercialDocumentPk + @"
) ORDER BY GubunCL;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder filelist = new StringBuilder();
		while (RS.Read()) {
			string tempstring;
			switch (RS["GubunCL"] + "") {
				case "0":
					tempstring = "모음";
					break;
				case "1":
					tempstring = "수입신고필증";
					break;
				case "2":
					tempstring = "관부가세 납부영수증";
					break;
				case "3":
					tempstring = "관세사비 세금계산서";
					break;
				case "4":
					tempstring = "수입 세금계산서";
					break;
				case "100":
					tempstring = "모음";
					break;
				case "101":
					tempstring = "수입신고필증";
					break;
				case "102":
					tempstring = "관부가세 납부영수증";
					break;
				case "103":
					tempstring = "관세사비 세금계산서";
					break;
				default:
					tempstring = "기타";
					break;
			}
			int gubuncl = Int32.Parse(RS["GubunCL"] + "");
			string[] temp = new string[] {
				RS["ClearancedFilePk"]+"",
				"&nbsp;",
				tempstring,
				"&nbsp;",
				gubuncl<100?
					"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL2('"+RS["ClearancedFilePk"]+"', '"+(gubuncl+100)+"')\" />":
					"<img src=\"../Images/CheckFalse.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL2('"+RS["ClearancedFilePk"]+"', '"+(gubuncl-100)+"')\"  />",
				"&nbsp;",
				"&nbsp;",
				"TBody1G",
				"ClearancedFile"
			};
			filelist.Append(string.Format(fileROW, temp));
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"SELECT [FilePk], [Title], [GubunCL], [FileName], [AccountID], [Registerd] FROM [dbo].[File] WHERE GubunCL in (11, 13, 15, 17) and GubunPk in (
SELECT [RequestFormPk]
  FROM [dbo].[CommerdialConnectionWithRequest]
  WHERE CommercialDocumentPk=" + CommercialDocumentPk + @"
);";
		RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			int gubuncl = Int32.Parse(RS["GubunCL"] + "") - 10;
			string[] temp = new string[] {
				RS["FilePk"]+"",
				(RS["Title"]+""==""?"&nbsp;":RS["Title"]+""),
				RS["FileName"]+"", 
				//gubuncl%2==1?"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL('"+RS["FilePk"]+"', '')\" />":"<img src=\"../Images/CheckFalse.jpg\" width=\"15\" height=\"15\" />",
				(gubuncl/2)%2==1?
					"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL('"+RS["FilePk"]+"', '"+(gubuncl+8)+"')\"  />":
					"<img src=\"../Images/CheckFalse.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL('"+RS["FilePk"]+"', '"+(gubuncl+12)+"')\"  />",
				gubuncl>3?
					"<img src=\"../Images/CheckTrue.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL('"+RS["FilePk"]+"', '"+(gubuncl+6)+"')\" />":
					"<img src=\"../Images/CheckFalse.jpg\" width=\"15\" height=\"15\" onclick=\"SetFileGubunCL('"+RS["FilePk"]+"', '"+(gubuncl+14)+"')\"  />",
				RS["AccountID"]+"",
				(RS["Registerd"]+"").Substring(2, (RS["Registerd"]+"").Length-5),
				"TBody1",
				"File"
			};
			filelist.Append(string.Format(fileROW, temp));
			//filelist.Append(String.Format("<a href='../UploadedFiles/FileDownload.aspx?S={0}' >ㆍ{1} : {2} </a><span onclick=\"FileDelete('{0}');\" style='color:red;'>X</span><br />", RS[0] + "", RS[1] + "", RS[2] + ""));
		}
		RS.Dispose();
		DB.DBCon.Close();
		return filelist + "" == "" ? "" :
		   "	<div style=\"float:right; \">" +
		   "		<fieldset>" +
		   "			<legend style=\"text-align:left;\"><strong>접수증 첨부파일</strong></legend>" +
		   "			<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:500px;\">" +
		   "				<tr>" +
		   "					<td class='THead1' style=\"width:100px; text-align:center;\" colspan=\"2\">Attached File</td>" +
		   "					<td class='THead1' style=\"width:18px; text-align:center;\">S</td>" +
		   "					<td class='THead1' style=\"width:18px; text-align:center;\">C</td>" +
		   "					<td class='THead1' style=\"width:50px; text-align:center;\">ID</td>" +
		   "					<td class='THead1' style=\"width:130px; text-align:center;\">Registerd</td>" +
		   "					<td class='THead1' style=\"width:18px; text-align:center;\">D</td>" +
		   "				</tr>" +
		   "				<tr>" + filelist + "</table>				</fieldset></div>";
	}

	private String LoadRequestComment(string CommercialDocumentPk, string AccountID) {
		StringBuilder temp = new StringBuilder();

		DB.SqlCmd.CommandText = @"SELECT [COMMENT_PK], [CATEGORY], [ACCOUNT_ID], [CONTENTS], [REGISTERD] FROM [dbo].[COMMENT] WHERE [TABLE_PK] in (
SELECT [RequestFormPk]
  FROM [dbo].[CommerdialConnectionWithRequest]
  WHERE CommercialDocumentPk=" + CommercialDocumentPk + @"
) and [CATEGORY] in ('Request', 'Request_Confirm') ORDER BY [REGISTERD];";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string deleteButton = "";
			string AddCmt = "";
			if (RS["ACCOUNT_ID"] + "" == AccountID || AccountID == "ilman") {
				deleteButton = "<span style=\"color:red; cursor:pointer;\" onclick=\"CommentDelete('" + RS["COMMENT_PK"] + "')\">X</span>";
			}
			if (RS["CATEGORY"] + "" == "Request_Confirm") {
				AddCmt = "[입고확인 Comment] " + RS["CONTENTS"] + "";
			}
			else {
				AddCmt = RS["CONTENTS"] + "";
			}
			temp.Append("<div class=\"Line1E8E8E8\" style=\"padding:3px;\">" + (AddCmt).Replace("\r\n", "<br />") + " <span style=\"color:gray;\">" + (RS["REGISTERD"] + "").Substring(5, 5) + "</span> <span style=\"color:Blue;\">" + RS["ACCOUNT_ID"] + "</span>" + deleteButton + "</div>");
		}
		RS.Dispose();
		DB.DBCon.Close();
		return temp + "";
	}

	private String LoadPageDateForCustoms(string OurBranchPk) {
		List<int> NotCalculatedList = new List<int>();
		List<string> NotCalculatedCount = new List<string>();
		Int32 NotCalculatedTotal = 0;
		List<int> CalculatedList = new List<int>();
		List<string> CalculatedCount = new List<string>();
		Int32 CalculatedTotal = 0;

		DB.SqlCmd.CommandText = @"	SELECT isnull(RF.ArrivalDate, '20201231'), Count(*) AS DocumentCount 
															FROM RequestForm AS RF 
																Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
																inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
															WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and  RF.DocumentStepCL=2 and RF.StepCL<64 
															Group By RF.ArrivalDate 
															Order by RF.ArrivalDate ASC;";

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			NotCalculatedList.Add(Int32.Parse("" + RS[0]));
			NotCalculatedCount.Add("" + RS[1]);
			NotCalculatedTotal += Int32.Parse("" + RS[1]);
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"	SELECT isnull(RF.ArrivalDate, '20201231'), Count(*) AS DocumentCount 
															FROM RequestForm AS RF 
																Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
																inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
															WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and RF.DocumentStepCL=5 and RF.StepCL<64 
															Group By RF.ArrivalDate 
															Order by RF.ArrivalDate ASC;";
		RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			CalculatedList.Add(Int32.Parse("" + RS[0]));
			CalculatedCount.Add("" + RS[1]);
			CalculatedTotal += Int32.Parse("" + RS[1]);
		}
		RS.Dispose();
		StringBuilder ReturnValue = new StringBuilder();
		int tempJ = 0;
		string EachBox = "<div class=\"Date\"><div class=\"DateInnerDate\">{0}-{1}</div><a href='./CheckDescriptionList.aspx?D={4}&G=O'>{2}</a> / <a href='./CheckDescriptionList.aspx?D={4}&G=T'>{3}</a></div>";
		//15 23

		if (NotCalculatedTotal > 0 && CalculatedTotal > 0) {
			for (int i = 0; i < NotCalculatedList.Count; i++) {
				if (CalculatedList.Count == tempJ) {
					ReturnValue.Append(String.Format(EachBox, (NotCalculatedList[i] + "").Substring(4, 2), (NotCalculatedList[i] + "").Substring(6, 2), NotCalculatedCount[i], "0", NotCalculatedList[i]));
					continue;
				}
				if (NotCalculatedList[i] == CalculatedList[tempJ]) {
					ReturnValue.Append(String.Format(EachBox, (NotCalculatedList[i] + "").Substring(4, 2), (NotCalculatedList[i] + "").Substring(6, 2), NotCalculatedCount[i], CalculatedCount[tempJ], NotCalculatedList[i]));
					tempJ++;
					continue;
				}
				if (NotCalculatedList[i] < CalculatedList[tempJ]) {
					ReturnValue.Append(String.Format(EachBox, (NotCalculatedList[i] + "").Substring(4, 2), (NotCalculatedList[i] + "").Substring(6, 2), NotCalculatedCount[i], "0", NotCalculatedList[i]));
					continue;
				}
				bool iinc = false;
				while (CalculatedList.Count > tempJ && NotCalculatedList[i] > CalculatedList[tempJ]) {
					ReturnValue.Append(String.Format(EachBox, (CalculatedList[tempJ] + "").Substring(4, 2), (CalculatedList[tempJ] + "").Substring(6, 2), "0", CalculatedCount[tempJ], CalculatedList[tempJ]));
					tempJ++;
					iinc = true;
				}
				if (iinc) {
					i--;
				}
			}
			if (CalculatedList.Count > tempJ) {
				while (CalculatedList.Count == tempJ) {
					ReturnValue.Append(String.Format(EachBox, (CalculatedList[tempJ] + "").Substring(4, 2), (CalculatedList[tempJ] + "").Substring(6, 2), "0", CalculatedCount[tempJ], CalculatedList[tempJ]));
					tempJ++;
				}
			}
		} else if (NotCalculatedTotal == 0) {
			for (int i = 0; i < CalculatedList.Count; i++) {
				ReturnValue.Append(String.Format(EachBox, (CalculatedList[i] + "").Substring(4, 2), (CalculatedList[i] + "").Substring(6, 2), "0", CalculatedCount[i], CalculatedList[i]));
			}

		} else if (CalculatedTotal == 0) {
			for (int i = 0; i < NotCalculatedList.Count; i++) {
				ReturnValue.Append(String.Format(EachBox, (NotCalculatedList[i] + "").Substring(4, 2), (NotCalculatedList[i] + "").Substring(6, 2), NotCalculatedCount[i], "0", NotCalculatedList[i]));

			}
		}
		DB.DBCon.Close();
		return String.Format("<div class=\"Date\"><div class=\"DateInnerDate\">TOTAL</div><a href='./CheckDescriptionList.aspx?D=A&G=O'>{0}</a> / <a href='./CheckDescriptionList.aspx?D=A&G=T'>{1}</a></div>", NotCalculatedTotal, CalculatedTotal) + ReturnValue;
	}
	private String LoadPageDate(string OurBranchPk) {
		List<string> OwnDateList = new List<string>();
		List<string> OwnDateCount = new List<string>();
		DB.SqlCmd.CommandText = @"	SELECT RF.ArrivalDate, Count(*) AS DocumentCount 
															FROM RequestForm AS RF 
																Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
																inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
															WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and RF.StepCL<64 and RF.DocumentStepCL<2
															Group By RF.ArrivalDate 
															Order by RF.ArrivalDate ASC;";
		DB.DBCon.Open();

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			OwnDateList.Add("" + RS[0]);
			OwnDateCount.Add("" + RS[1]);
		}
		RS.Dispose();
		StringBuilder ReturnValue = new StringBuilder();
		DB.SqlCmd.CommandText = @"	SELECT RF.ArrivalDate, Count(*) AS DocumentCount 
															FROM RequestForm AS RF 
																Left join RegionCode AS Arrival on RF.ArrivalRegionCode=Arrival.RegionCode  
																inner join CommerdialConnectionWithRequest AS CCWR ON RF.RequestFormPk=CCWR.RequestFormPk 
															WHERE Arrival.OurBranchCode=" + OurBranchPk + @" and RF.StepCL<64 and RF.DocumentStepCL in (1, 2, 5, 6)
															Group By RF.ArrivalDate 
															Order by RF.ArrivalDate ASC;";
		RS = DB.SqlCmd.ExecuteReader();

		int k = OwnDateList.Count == 0 ? -1 : 0;
		int OwnTotalCount = 0;
		int OurBranchTotalCount = 0;
		while (RS.Read()) {
			string tempOwnCount = "0";
			if (k != -1 && RS[0] + "" == OwnDateList[k]) {
				tempOwnCount = OwnDateCount[k];
				OwnTotalCount += Int32.Parse(OwnDateCount[k]);
				k++;
				if (k == OwnDateCount.Count) {
					k = -1;
				}
			}
			//Response.Write(" " + k + " " + OwnDateCount.Count + "<br />");
			OurBranchTotalCount += Int32.Parse(RS[1] + "");
			if (RS[0] + "" != "") {
				ReturnValue.Append(String.Format("<div class=\"Date\"><div class=\"DateInnerDate\">{0}-{1}</div><a href='./CheckDescriptionList.aspx?D=" + RS[0] + "&G=O'>{2}</a> / <a href='./CheckDescriptionList.aspx?D=" + RS[0] + "&G=T'>{3}</a></div>", (RS[0] + "").Substring(4, 2), (RS[0] + "").Substring(6, 2), tempOwnCount, RS[1] + ""));
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		return String.Format("<div class=\"Date\"><div class=\"DateInnerDate\">TOTAL</div><a href='./CheckDescriptionList.aspx?D=A&G=O'>{0}</a> / <a href='./CheckDescriptionList.aspx?D=A&G=T'>{1}</a></div>", OwnTotalCount, OurBranchTotalCount) + ReturnValue;
	}

	private String LoadCommentList(string CDPk, string AccountID) {
		DB.DBCon.Open();
		StringBuilder CommentTemp = new StringBuilder();

		HistoryC HisC = new HistoryC();
		List<sComment> Comment = new List<sComment>();
		Comment = HisC.LoadList_Comment("CommercialDocument", CDPk, "'BL'", ref DB);
		for (int i = 0; i < Comment.Count; i++) {
			CommentTemp.Append(Comment[i].Contents + " : <strong>" + Comment[i].Account_Id + "</strong> " + (Comment[i].Registerd + "").Substring(5, (Comment[i].Registerd).Length - 8));
			if (Comment[i].Account_Id == AccountID) {
				CommentTemp.Append(" <span style=\"color:red; cursor:hand;\" onclick=\"DeleteComment('" + Comment[i].Comment_Pk + "')\">X</span><br />");
			}
			else {
				CommentTemp.Append("<br />");
			}
		}
		DB.DBCon.Close();
		return CommentTemp + "" == "" ?
		   "<fieldset style=\"width:400px; padding:10px;  \"><legend><strong>Comment</strong></legend><input type=\"text\" style=\"width:300px; \" id=\"TBContents\" ><input type=\"button\" value=\"등록\" onclick=\"InsertContents('" + CDPk + "', '" + AccountID + "');\" /></fieldset>" :
		   "<fieldset style=\"width:400px; padding:10px;  \"><legend><strong>Comment</strong></legend>" + CommentTemp + "<input type=\"text\" style=\"width:300px; \" id=\"TBContents\" ><input type=\"button\" value=\"등록\" onclick=\"InsertContents('" + CDPk + "', '" + AccountID + "');\" /></fieldset>";
	}
}