using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Board_C_List : System.Web.UI.Page
{

	protected String BoardContentsList;
	protected String BTN_Write;
	private DBConn DB;
	protected String PageNoHTML;
	protected String AccountID;
	protected String Name;
	private int pageLength = 35;
	private int CurrentPageNo;
	protected String BoardTitle;
	protected String CompanyPk;

	protected void Page_Load(object sender, EventArgs e) {
		try {
			string[] Memberinfo = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
			Name = Memberinfo[3];
			AccountID = Memberinfo[2];
			CompanyPk = Memberinfo[1];
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}

		if (Request.Params["PageNo"] + "" == "") {
			CurrentPageNo = 1;
		} else {
			CurrentPageNo = Int32.Parse(Request.Params["PageNo"] + "");
		}

		DB = new DBConn();
		LoadBoardContentsList(Request.Params["C"] + "", Request.Params["SerchType"] + "", Request.Params["SerchValue"] + "");
		LoadBTNWrite(Request.Params["C"] + "", AccountID);
	}

	private Boolean LoadBTNWrite(string BoardCode, string AccountID) {
		DB.SqlCmd.CommandText = "SELECT [PermissionType] FROM [BoardLibPermission] WHERE BoardCode='" + BoardCode + "' and TargetID='" + AccountID + "';";
		DB.DBCon.Open();
		string Permission = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();

		if (Permission == "A" || Permission == "W") {
			BTN_Write = "<p><input type=\"button\" onclick=\"BTN_Write('" + BoardCode + "');\" value=\"글쓰기\"  style=\"width:80px; height:30px;\"/></p>";
		} else {
			BTN_Write = "";
		}
		return true;
	}
	private Boolean LoadBoardContentsList(string BoardCode, string SerchType, string SerchValue) {
		string RowFormat = "<tr><td height=\"25\" align=\"center\">{0}</td><td align=\"center\">{1}</td><td style=\"padding-left:10px;\">{2}</td>" +
									  "<td align=\"center\">{3}</td><td align=\"center\">{4}</td><td align=\"center\">{5}</td></tr>";
		StringBuilder TempReturnHTML = new StringBuilder();

		string SerchQuery = "";
		if (SerchValue != "") {
			switch (SerchType) {
				case "1":
					SerchQuery = " and Title like N'%" + SerchValue + "%' ";
					break;
				case "2":
					SerchQuery = " and Contents like N'%" + SerchValue + "%' ";
					break;
				case "3":
					SerchQuery = " and AccountID like '%" + SerchValue + "%' ";
					break;
				case "All":
					SerchQuery = " and (Title like N'%" + SerchValue + "%' OR Contents like N'%" + SerchValue + "%' OR AccountID like '%" + SerchValue + "%' OR  [Contents] like '%" + SerchValue + "%') ";
					break;
				default:
					break;
			}
		}

		DB.SqlCmd.CommandText = @"
SELECT [Pk], [Header], [Position], [ParentsPk], [Title], [AccountID], [Name], [Registerd], [ReadCount] , Comment.C ,Comment.lastReplyRegisterd
FROM [BoardContents] 
	left join (SELECT [ContentsPk] ,max(Registerd) AS lastReplyRegisterd, count(*) AS C FROM [BoardComment] WHERE isnull(Deleted, '')='' Group by ContentsPk) AS Comment ON Comment.ContentsPk=Pk
WHERE isnull(DeletedID, '')='' and [BoardCode]='" + BoardCode + @"' " + SerchQuery + @"
ORDER BY ParentsPk DESC, Position;";

		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		for (int i = 0; i < (CurrentPageNo - 1) * pageLength; i++) {
			if (RS.Read()) {
				continue;
			} else {
				break;
			}
		}
		for (int i = 0; i < pageLength; i++) {
			if (RS.Read()) {
				string position = RS["Position"] + "";
				string[] RowData = new string[6];
				RowData[0] = RS["Pk"] + "";
				RowData[1] = RS["Header"] + "";

				if (position == "") {
					RowData[2] = RS["Title"] + "";
				} else {
					int underC = position.Length / 3 + 1;
					RowData[2] = "";
					for (int j = 0; j < underC; j++) {
						RowData[2] += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
					}
					RowData[2] += "→" + RS["Title"];
				}
				string newimg = "";
				DateTime registerd = DateTime.Parse(RS["Registerd"] + "");
				if (DateTime.Now.AddDays(-1) < registerd) {
					newimg += " <img src=\"../Images/Board/new.gif\" align=\"absmiddle\">";
				}
				//Reply New 이미지

				if (RS["lastReplyRegisterd"] + "" != "") {
					DateTime lastReplyRegisterd = DateTime.Parse(RS["lastReplyRegisterd"] + "");

					if (DateTime.Now.AddDays(-1) < lastReplyRegisterd) {
						newimg += " <img src=\"../Images/Board/Renew.gif\" align=\"absmiddle\">";
					}
				}

				string commentC = "";
				if (RS["C"] + "" != "") {
					commentC = " [" + RS["C"] + "]";
				}

				RowData[2] = "<span onclick=\"GoView('" + BoardCode + "', '" + RS["Pk"] + "');\">" + RowData[2] + "<font color=\"#0000FF\">" + commentC + "</font>" + newimg + "</span>";
				RowData[3] = RS["AccountID"] + " / <span style=\"color:blue;\">" + RS["Name"] + "</span>";
				RowData[4] = RS["Registerd"].ToString().Substring(5, 5) + " " + registerd.Hour + ":" + registerd.Minute;
				RowData[5] = RS["ReadCount"] + "";
				TempReturnHTML.Append(String.Format(RowFormat, RowData));
			} else {
				break;
			}
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "SELECT [Title] FROM [BoardLibCode] WHERE BoardCode='" + BoardCode + "';";
		BoardTitle = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = @"
SELECT count(*) 
FROM [BoardContents] 
WHERE isnull(DeletedID, '')='' and [BoardCode]='" + BoardCode + @"' " + SerchQuery + @";";

		//WHERE isnull(DeletedID, '')='' and [BoardCode]='" + BoardCode + @"';";
		int totalRecord = Int32.Parse(DB.SqlCmd.ExecuteScalar() + "");

		DB.DBCon.Close();
		BoardContentsList = TempReturnHTML + "";

		//PageNoHTML = "<div>" + new Common().SetPageListByNo(pageLength, CurrentPageNo, totalRecord, "C_List.aspx", "?C=" + BoardCode + "&") + "</div>";
		PageNoHTML = "<div>" + new Common().SetPageListByNo(pageLength, CurrentPageNo, totalRecord, "C_List.aspx", "?C=" + BoardCode + "&SerchValue=" + SerchValue + "&SerchType=" + SerchType + "&") + "</div>";

		return true;
	}
}