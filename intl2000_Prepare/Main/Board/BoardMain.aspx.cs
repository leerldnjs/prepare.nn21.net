using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Board_BoardMain : System.Web.UI.Page
{
	protected String BoardContentsList;
	private DBConn DB;
	protected String AccountID;
	protected String Name;
	protected String CompanyPk;
	private int pageLength = 7;
	private string[] DefaultLoadCode;
	protected void Page_Load(object sender, EventArgs e) {
		try {
			string[] Memberinfo = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
			Name = Memberinfo[3];
			AccountID = Memberinfo[2];
			CompanyPk = Memberinfo[1];
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}

		DB = new DBConn();
		if (CompanyPk == "10520") {
			DefaultLoadCode = new string[] { "80010", "80011", "80009" };
		} else {
			DefaultLoadCode = new string[] { "01002", "01001", "03001", "03005" };
		}
		LoadBoardContentsList(DefaultLoadCode);
	}


	private Boolean LoadBoardContentsList(string[] BoardCode) {
		string RowFormat = "<tr><td height=\"30\" align=\"center\">{0}</td><td align=\"center\">{1}</td><td style=\"padding-left:10px;\">{2}</td>" +
									  "<td align=\"center\">{3}</td><td align=\"center\">{4}</td><td align=\"center\">{5}</td></tr>";
		StringBuilder TempReturnHTML = new StringBuilder();
		StringBuilder TempTableSUM = new StringBuilder();
		DB.DBCon.Open();
		string BoardTitle = "";
		for (int B = 0; B < BoardCode.Length; B++) {
			DB.SqlCmd.CommandText = @"
DECLARE @BoardCode varchar(5); 
DECLARE @AccountID varchar(20);
DECLARE @BoardTitle nvarchar(20);

SET @BoardCode='" + BoardCode[B] + @"';
SET @AccountID='" + AccountID + @"';
DECLARE @Count int; 
SELECT @Count=Count(*)
  FROM [BoardLibPermission]
 WHERE [BoardCode]=@BoardCode and [TargetID]=@AccountID and PermissionType in ('A', 'R', 'W'); 

SELECT @BoardTitle=[Title] FROM [BoardLibCode] WHERE BoardCode=@BoardCode;
if (@Count>0)
	BEGIN
		SELECT TOP " + pageLength + @"  @BoardTitle AS BoardTitle, B.[Pk], B.[Header], B.[Position], B.[ParentsPk], B.[Title], B.[AccountID], B.[Name], B.[Registerd], B.[ReadCount] , Comment.C,Comment.lastReplyRegisterd
		FROM [BoardContents] AS B
			left join (SELECT [ContentsPk] ,max(Registerd) AS lastReplyRegisterd, count(*) AS C FROM [BoardComment] WHERE isnull(Deleted, '')='' Group by ContentsPk) AS Comment ON Comment.ContentsPk=Pk
		WHERE isnull(DeletedID, '')='' and [BoardCode]=@BoardCode ORDER BY ParentsPk DESC, Position; 
	END";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			//Response.Write(DB.SqlCmd.CommandText);
			while (RS.Read()) {
				//Response.Write("123");
				BoardTitle = RS["BoardTitle"] + "";
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

				DateTime registerd = DateTime.Parse(RS["Registerd"] + "");
				string newimg = "";
				if (DateTime.Now.AddDays(-1) < registerd) {
					newimg += " <img src=\"../Images/Board/new.gif\" align=\"absmiddle\">";
				}

				//Reply New 이미지///////////////

				if (RS["lastReplyRegisterd"] + "" != "") {
					DateTime lastReplyRegisterd = DateTime.Parse(RS["lastReplyRegisterd"] + "");

					if (DateTime.Now.AddDays(-1) < lastReplyRegisterd) {
						newimg += " <img src=\"../Images/Board/Renew.gif\" align=\"absmiddle\">";
					}
				}
				///////////////////////////////
				string commentC = "";
				if (RS["C"] + "" != "") {
					commentC = " [" + RS["C"] + "]";
				}

				RowData[2] = "<span onclick=\"GoView('" + BoardCode[B] + "', '" + RS["Pk"] + "');\">" + RowData[2] + "<font color=\"#0000FF\">" + commentC + "</font>" + newimg + "</span>";
				RowData[3] = RS["AccountID"] + " / <span style=\"color:blue;\">" + RS["Name"] + "</span>";
				RowData[4] = RS["Registerd"].ToString().Substring(5, 5) + " " + registerd.Hour + ":" + registerd.Minute;
				RowData[5] = RS["ReadCount"] + "";
				TempReturnHTML.Append(String.Format(RowFormat, RowData));
			}
			RS.Dispose();
			if (TempReturnHTML + "" != "") {
				TempTableSUM.Append("<tr><td align=\"left\" valign=\"bottom\" style=\"height:30px; padding-left:10px; padding-bottom:5px;\" ><img src=\"../Images/Board/arow01.gif\" align=\"absmiddle\">&nbsp;" +
					"<font style=\"font-family:Vernada; font-size:16px;font-weight:bold; \">" + BoardTitle + "</font></td></tr>" +
					"<tr><td><table cellpadding=\"0\" cellspacing=\"0\"  style=\"width:990px;\">" +
					"<tr><td colspan=\"6\" style=\"height:1px; background-color:#708090;\"></td></tr>" +
					"<tr><td colspan=\"6\" style=\"height:1px; background-color:#FFFFFF;\"></td></tr>" +
					"<tr><td align=\"center\" style=\"width:50px; height:30px; background-color:#D1E3E5; \">No</td>" +
					"<td align=\"center\" style=\"width:80px; background-color:#D1E3E5;\">Header</td>" +
					"<td align=\"center\" style=\"width:560px; background-color:#D1E3E5;\" >Title</td>" +
					"<td align=\"center\" style=\"width:100px; background-color:#D1E3E5;\">ID</td>" +
					"<td align=\"center\" style=\"width:150px; background-color:#D1E3E5;\">Registerd</td>" +
					"<td align=\"center\" style=\"width:50px; background-color:#D1E3E5;\">Hit</td>" +
					"</tr><tr><td colspan=\"6\" style=\"height:1px; background-color:#FFFFFF;\"></td></tr>" +
					"<tr><td colspan=\"6\" style=\"height:1px; background-color:#708090;\"></td></tr>" + TempReturnHTML + "<tr><td colspan=\"6\" style=\"height:1px; background-color:#FFFFFF;\"></td></tr><tr>" +
					"<td colspan=\"6\" style=\"height:1px; background-color:#708090;\"></td></tr></table></td></tr><tr><td>&nbsp;</td></tr>");
				TempReturnHTML = new StringBuilder();

			}
		}
		DB.DBCon.Close();
		BoardContentsList = TempTableSUM + "";
		return true;
	}
}