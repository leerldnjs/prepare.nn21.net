using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Board_BoardList : System.Web.UI.UserControl
{
	protected String BoardListHTML;
	protected void Page_Load(object sender, EventArgs e)
	{
		String AccountID = "";
		try {
			AccountID = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None)[2];
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}
		String BoardCode = Request.Params["C"] + "";
		LoadBoardList(AccountID, BoardCode);
	}

	private Boolean LoadBoardList(string AccountID, string SelectedBoardCode)
	{
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT C.[BoardCode], C.[Title], C.[OrderBy], P.[PermissionType], New.C ,NewComment.RC
FROM [BoardLibCode] AS C 
	left join [BoardLibPermission] AS P On P.BoardCode=C.[BoardCode] 
	left join (
		SELECT [BoardCode], Count(*) AS C
		FROM [INTL2010].[dbo].[BoardContents]
		WHERE Registerd>getDate()-1 and isnull(DeletedID, '')='' 
		Group By [BoardCode]
	) AS New ON C.BoardCode=New.BoardCode 
    left join (
		SELECT [BoardCode], Count(*) AS RC
		FROM [INTL2010].[dbo].[BoardContents] contents
			left join [INTL2010].[dbo].[BoardComment] AS comment ON contents.Pk=comment.ContentsPk 
		WHERE comment.Registerd>getDate()-1 and isnull(DeletedID, '')='' and isnull(comment.Deleted, '')='' 
		Group By [BoardCode]
	) AS NewComment ON C.BoardCode=NewComment.BoardCode 
WHERE P.[TargetID]='" + AccountID + @"' or len(C.BoardCode)=2 
ORDER BY C.OrderBy ;";

		StringBuilder ReturnHtml = new StringBuilder();
		StringBuilder TempHtml = new StringBuilder();
		string TempHeaderTitle = "";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        string htmlFormat = " <tr><td valign=\"top\" style=\"height:20px; padding-left:5px; padding-right:5px; padding-top:15px;\"><img src=\"../Images/ico_arrow.gif\" />&nbsp;<b>{0}</b></td></tr>" +
                                     "{1}";
		while (RS.Read()) {
			string Title = RS["Title"] + "";
			if (RS["BoardCode"].ToString().Length == 2) {
				if (TempHtml + "" != "") {
					ReturnHtml.Append(string.Format(htmlFormat, TempHeaderTitle, TempHtml + ""));
					TempHtml = new StringBuilder();
				}
				TempHeaderTitle = Title;
				continue;
			}
			string newimg = "";
			if (RS["C"] + "" != "") {
				newimg += " <img src=\"../Images/Board/new.gif\" align=\"absmiddle\" style=\"border:0px; \" >";
			}
            
            if (RS["RC"] + "" != "")
            {
                newimg += " <img src=\"../Images/Board/Renew.gif\" align=\"absmiddle\" style=\"border:0px; \" >";
            }
			if (SelectedBoardCode == RS["BoardCode"] + "") {
				if (RS["BoardCode"] + "" == "21005") {
					TempHtml.Append("<tr><td style=\" height:20px; padding-left:10px; padding-right:5px;\"><img src=\"../Images/Board/icon.gif\" />" +
										"<a href=\"/Board/Logistics_Route.aspx\"><strong>" + RS["Title"] + newimg + "</strong></a></td></tr>");

				}
				else {
					TempHtml.Append("<tr><td style=\" height:20px; padding-left:10px; padding-right:5px;\"><img src=\"../Images/Board/icon.gif\" />" +
										"<a href=\"C_List.aspx?C=" + RS["BoardCode"] + "\"><strong>" + RS["Title"] + newimg + "</strong></a></td></tr>");

				}
			} else {
				if (RS["BoardCode"] + "" == "21005") {
					TempHtml.Append("<tr><td style=\" height:20px; padding-left:10px; padding-right:5px;\"><img src=\"../Images/Board/icon.gif\" />" +
												"<a href=\"/Board/Logistics_Route.aspx\">" + RS["Title"] + newimg + "</a></td></tr>");

				} else {
					TempHtml.Append("<tr><td style=\" height:20px; padding-left:10px; padding-right:5px;\"><img src=\"../Images/Board/icon.gif\" />" +
												"<a href=\"C_List.aspx?C=" + RS["BoardCode"] + "\">" + RS["Title"] + newimg + "</a></td></tr>");

				}
			}
		}
		RS.Dispose();
		if (TempHtml + "" != "") {
            ReturnHtml.Append(string.Format(htmlFormat, TempHeaderTitle, TempHtml + ""));
		}
		BoardListHTML = ReturnHtml + "";
		return true;
	}
}