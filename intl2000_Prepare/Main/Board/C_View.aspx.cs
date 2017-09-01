using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Board_C_View : System.Web.UI.Page
{
	protected String Title;
	protected String BoardTitle;
	protected String Writer;
	protected String ReadCount;
	protected String RegisterdDate;
	protected String Contents;
	protected String Reply;
	protected String AccountID;
	protected String Name;
	protected String BoardContentsList;
	protected String BTNModify;
	protected String BTNDelete;
	protected String BTNReply;
	protected String AttachedFiles;
    protected String CompanyPk;

	private DBConn DB;

	protected void Page_Load(object sender, EventArgs e)
	{
		DB = new DBConn();
		AccountID = "";
		try {
			string[] Memberinfo = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
			Name = Memberinfo[3];
			AccountID = Memberinfo[2];
            CompanyPk = Memberinfo[1];
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}

		string ParentsPk = "";
		LoadPage(Request.Params["P"], Request.Params["C"], AccountID, ref ParentsPk);
		LoadContentsList(Request.Params["P"], Request.Params["C"], ParentsPk);
		LoadAttachedFiles(Request.Params["P"]);

		if (AccountID == "ilman" || AccountID == "ilic30" || AccountID == "ilogistics" || AccountID == "ilic00") {
			LoadBoardListForBoardMove(Request.Params["P"], Request.Params["C"], AccountID);
		}
	}
    
	private Boolean LoadPage(string Pk, string BoardCode, string AccountID, ref string parentsPk)
	{
		DB.SqlCmd.CommandText = @"
DECLARE @BoardPk int; 
DECLARE @BoardCode varchar(5); 
DECLARE @AccountID varchar(20); 
DECLARE @Permission char(1);
DECLARE @ReadCount int; 

SET @BoardPk=" + Pk + @";
SET @BoardCode='" + BoardCode + @"';
SET @AccountID='" + AccountID + @"';

SELECT @Permission=[PermissionType] FROM [BoardLibPermission] WHERE BoardCode=@BoardCode and TargetID=@AccountID;

if isnull(@Permission, '')=''
	BEGIN
		SELECT 'N';
	END 
ELSE
	BEGIN 
		SELECT @ReadCount=count(*) FROM [BoardReadHistory] WHERE AccountID=@AccountID and BoardPk=@BoardPk and [Registerd]>dateadd(mi,-10,getdate())
		IF @ReadCount=0
			BEGIN
				INSERT INTO [BoardReadHistory] ([BoardPk], [AccountID]) VALUES (@BoardPk, @AccountID);
				UPDATE [BoardContents] SET [ReadCount] = [ReadCount] +1 WHERE Pk=@BoardPk;
			END
		SELECT 
			C.[BoardCode], C.[Header], C.[Position], C.[ParentsPk], C.[Title], C.[Contents], C.[AccountID], C.[Name], C.[Registerd], C.[ReadCount], L.[Title] AS BoardTitle, @Permission  as Permission 
		FROM [BoardContents] AS C
			left join [BoardLibCode] AS L ON C.BoardCode=L.BoardCode
		WHERE C.Pk=@BoardPk and isnull(C.[Deleted], '')='' ;
	END;";

		DB.DBCon.Open();

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			if (RS[0] + "" == "N") {
				RS.Dispose();
				DB.DBCon.Close();
				return false;
			}

			parentsPk = RS["ParentsPk"] + "";
			BoardCode = RS["BoardCode"] + "";
			Title = RS["Title"] + "";
			BoardTitle = RS["BoardTitle"] + "";
			Writer = RS["AccountID"] + "<span style=\"color:black;\"> ["+RS["Name"]+"]</span>";
			ReadCount = RS["ReadCount"] + "";
			RegisterdDate = RS["Registerd"] + "";
			Contents = RS["Contents"] + "";
			if (RS["Permission"] + "" == "S" || RS["AccountID"]+"" == AccountID) {
				BTNDelete = "<input type=\"button\" onclick=\"Goto('Delete');\" value=\"Delete\" />";
				BTNModify = "<input type=\"button\" onclick=\"Goto('Modify');\" value=\"Modify\" />";
			} else {
				if (AccountID == "ilic30" || AccountID == "ilman") {
					BTNDelete = "<input type=\"button\" onclick=\"Goto('Delete');\" value=\"Delete\" />";
					BTNModify = "";
				} else {
					BTNDelete = "";
					BTNModify = "";
				}
			}
			BTNReply = "<input type=\"button\" onclick=\"Goto('Reply');\" value=\"Reply\" />";
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			return false;
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = @"
DECLARE @BoardPk int; 

SET @BoardPk=" + Pk + @";

SELECT [Pk], [Position], [Comment], [AccountID], [Name], [Registerd] 
FROM [BoardComment] 
WHERE [ContentsPk]=@BoardPk and isnull([Deleted], '')='' 
ORDER BY Position;";
		//Response.Write(DB.SqlCmd.CommandText);
		StringBuilder ReplyHtml = new StringBuilder();
		string ReplyFormatWithAddComment = "<tr><td colspan=\"2\" style=\"height:5px; background-repeat:repeat-x; \" background=\"../Images/Board/dot.gif\"></td></tr>" +
                "<tr><td style=\"width:700px; height:30px;\" bgcolor=\"#D1E3E5\">{0}</td><td align=\"center\" bgcolor=\"#D1E3E5\" style=\"width:290px;\" ><a onclick=\"OpenComment('{1}', '{3}');\"><img src=\"../Images/Board/reply.gif\"></a></td></tr>" +
				"<tr><td colspan='2' valign='top' style='height:50px; padding-left:10px;padding-right:10px; padding-top:5px;' bgcolor='#F0F5F6'>" +
                "<table align=\"center\" style=\"width:910px; margin-bottom:10px;\"><tr><td>{2}<div id=\"{3}\" style=\"width:0px;\"></div></td></tr></table></td></tr>";
		string ReplyFormatWithoutAddComment = "<tr><td colspan=\"2\" style=\"height:5px; background-repeat:repeat-x; \" background=\"../Images/Board/dot.gif\"></td></tr>" +
                "<tr><td style=\"width:700px; height:30px;\" bgcolor=\"#D1E3E5\">{0}</td><td align=\"center\" bgcolor=\"#D1E3E5\" style=\"width:290px;\">&nbsp;</td></tr>" +
				"<tr><td colspan='2' valign='top' style='height:50px; padding-left:10px;padding-right:10px; padding-top:5px;' bgcolor='#F0F5F6'>" +
                "<table align=\"center\" style=\"width:910px; margin-bottom:10px;\"><tr><td>{2}<div id=\"{3}\" style=\"width:0px;\"></div></td></tr></table></td></tr>";

		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string[] RowData = new string[4];
            RowData[0] = "";
			RowData[1] = RS["Position"] + "";
			RowData[2] = RS["Comment"] + "";
			RowData[3] = "PnComment" + RS["Position"];
			string position = RS["Position"] + "";

			if (position != "") {
				int underC = position.Length / 3 + 1;
				for (int i = 0; i < underC; i++) {
                    RowData[0] += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
				}
			}
            string Renewimg = "";
            if (RS["Registerd"] + "" != "")
            {
                DateTime Registerd = DateTime.Parse(RS["Registerd"] + "");

                if (DateTime.Now.AddDays(-1) < Registerd)
                {
                    Renewimg = " <img src=\"../Images/Board/Renew.gif\" align=\"absmiddle\">";
                }
            }
            RowData[0] += "→" + RS["AccountID"] + "[<span style=\"color:blue;\">" + RS["Name"] + "</span>] <span style=\"color:gray;\">" + RS["Registerd"] + Renewimg + "</span>";
			string ModifyBTN = "";
			if (RS["AccountID"] + "" == AccountID) {
                RowData[0] += "&nbsp;&nbsp;<span onclick=\"DeleteComment('" + RS["Pk"] + "');\" style=\"color:red;\"><img src=\"../Images/Board/delete.gif\" align=\"absmiddle\"></span>";
                ModifyBTN = "&nbsp;<a  onclick=\"OpenCommentForModify('" + RowData[1] + "', '" + RowData[3] + "', '" + RS["Pk"] + "', '" + RS["Comment"] + "');\" /><img src=\"../Images/Board/edit.gif\" align=\"absmiddle\"></a>";
			}

			RowData[0] += ModifyBTN;

			if (position.Length == 2) {
				ReplyHtml.Append(String.Format(ReplyFormatWithAddComment, RowData));
			} else {
				ReplyHtml.Append(String.Format(ReplyFormatWithoutAddComment, RowData));	
			}
		}

		RS.Dispose();
		DB.DBCon.Close();
        Reply = ReplyHtml + "<tr><td colspan=\"2\" bgcolor=\"#FFFFFF\"  style=\"height:20px;\"></td></tr>" +
            "<tr><td align=\"center\" bgcolor=\"#F4F3F3\" style=\"width:700px; height:90px; padding-left:20px;\"><textarea rows=\"4\" cols=\"120\" id=\"Comment\" style=\"ime-mode:active;\"></textarea></td>" +
            "<td align=\"left\"  bgcolor=\"#F4F3F3\">&nbsp;<input type=\"button\" style=\"width:60px; height:50px; \" value=\"Reply\" onclick=\"SaveReply('Comment', '');\" /></td></tr>";
		return true;
	}
	private Boolean LoadContentsList(string ContentsPk, string BoardCode, string ParentsPk)
	{
        string RowFormat = "<tr><td height=\"25\" align=\"center\">{0}</td><td align=\"center\">{1}</td><td style=\"padding-left:10px;\">{2}</td>" +
                                      "<td align=\"center\">{3}</td><td align=\"center\">{4}</td><td align=\"center\">{5}</td></tr>";
		StringBuilder TempReturnHTML = new StringBuilder();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"
DECLARE @Pk int; 
DECLARE @ParentsPk int; 
DECLARE @Position varchar(20); 
DECLARE @BoardCode char(5);
SET @Pk=" + ContentsPk + @"; 

SELECT @BoardCode=[BoardCode], @Position=[Position], @ParentsPk=[ParentsPk] FROM [BoardContents] WHERE Pk=@Pk

SELECT TOP 1 [Pk], [Header], [Position], [ParentsPk], [Title], [AccountID], [Name], [Registerd], [ReadCount], Comment.C ,Comment.lastReplyRegisterd
FROM [BoardContents] 
	left join (SELECT [ContentsPk] ,max(Registerd) AS lastReplyRegisterd, count(*) AS C FROM [BoardComment] WHERE isnull(Deleted, '')='' Group by ContentsPk) AS Comment ON Comment.ContentsPk=Pk
WHERE isnull(DeletedID, '')='' and [BoardCode]=@BoardCode and [ParentsPk] >@ParentsPk ORDER BY ParentsPk ASC; ";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string position = RS["Position"] + "";
			string[] RowData = new string[6];
			RowData[0] = RS["Pk"] + "";
			RowData[1] = RS["Header"] + "";

			if (position == "") {
				RowData[2] = RS["Title"] + "";
			} else {
				int underC = position.Length / 3 + 1;
				RowData[2] = "";
				for (int i = 0; i < underC; i++) {
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
            
            if (RS["lastReplyRegisterd"] + "" != "")
            {
                DateTime lastReplyRegisterd = DateTime.Parse(RS["lastReplyRegisterd"] + "");

                if (DateTime.Now.AddDays(-1) < lastReplyRegisterd)
                {
                    newimg += " <img src=\"../Images/Board/Renew.gif\" align=\"absmiddle\">";
                }
            }
            ///////////////////////////////


			string commentC = "";
			if (RS["C"] + "" != "") {
				commentC = " [" + RS["C"] + "]";
			}

			if (RS["Pk"] + "" == ContentsPk) {
                RowData[2] = "<strong>" + RowData[2] + commentC + newimg +  "</strong>";
			} else {
                RowData[2] = "<span onclick=\"GoView('" + BoardCode + "', '" + RS["Pk"] + "');\">" + RowData[2] + commentC + newimg +"</span>";
			}
			RowData[3] = RS["AccountID"] + " / <span style=\"color:blue;\">" + RS["Name"] + "</span>";
			RowData[4] = RS["Registerd"].ToString().Substring(5, 5) + " " + registerd.Hour + ":" + registerd.Minute;
			RowData[5] = RS["ReadCount"] + "";
			TempReturnHTML.Append(String.Format(RowFormat, RowData));
		}
		RS.Dispose();
		
		

		DB.SqlCmd.CommandText = @"
SELECT [Pk], [Header], [Position], [ParentsPk], [Title], [AccountID], [Name], [Registerd], [ReadCount] , Comment.C ,Comment.lastReplyRegisterd
FROM [BoardContents]
	left join (SELECT [ContentsPk] ,max(Registerd) AS lastReplyRegisterd, count(*) AS C FROM [BoardComment] WHERE isnull(Deleted, '')='' Group by ContentsPk) AS Comment ON Comment.ContentsPk=Pk

WHERE isnull(DeletedID, '')='' and [BoardCode]='" + BoardCode + @"' and ParentsPk=" + ParentsPk + @"
ORDER BY ParentsPk DESC, Position;";
		//Response.Write(DB.SqlCmd.CommandText);
		
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string position = RS["Position"] + "";
			string[] RowData = new string[6];
			RowData[0] = RS["Pk"] + "";
			RowData[1] = RS["Header"] + "";

			if (position == "") {
				RowData[2] = RS["Title"] + "";
			} else {
				int underC = position.Length / 3 + 1;
				RowData[2] = "";
				for (int i = 0; i < underC; i++) {
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
            
            if (RS["lastReplyRegisterd"] + "" != "")
            {
                DateTime lastReplyRegisterd = DateTime.Parse(RS["lastReplyRegisterd"] + "");

                if (DateTime.Now.AddDays(-1) < lastReplyRegisterd)
                {   
                    newimg += " <img src=\"../Images/Board/Renew.gif\" align=\"absmiddle\">";
                }
            }
            ///////////////////////////////
			string commentC = "";
			if (RS["C"] + "" != "") {
				commentC = " [" + RS["C"] + "]";
			}

			if (RS["Pk"] + "" == ContentsPk) {
                RowData[2] = "<strong>" + RowData[2] + commentC + newimg +  "</strong>";
			} else {
                RowData[2] = "<span onclick=\"GoView('" + BoardCode + "', '" + RS["Pk"] + "');\">" + RowData[2] + commentC + newimg + "</span>";
			}
			RowData[3] = RS["AccountID"] + " / <span style=\"color:blue;\">" + RS["Name"] + "</span>";
			RowData[4] = RS["Registerd"].ToString().Substring(5, 5) + " " + registerd.Hour + ":" + registerd.Minute;
			RowData[5] = RS["ReadCount"] + "";
			TempReturnHTML.Append(String.Format(RowFormat, RowData));
		}
		RS.Dispose();

		DB.SqlCmd.CommandText = @"
DECLARE @Pk int; 
DECLARE @ParentsPk int; 
DECLARE @Position varchar(20); 
DECLARE @BoardCode char(5);
SET @Pk=" + ContentsPk + @"; 

SELECT @BoardCode=[BoardCode], @Position=[Position], @ParentsPk=[ParentsPk] FROM [BoardContents] WHERE Pk=@Pk

SELECT TOP 1 [Pk], [Header], [Position], [ParentsPk], [Title], [AccountID], [Name], [Registerd], [ReadCount], Comment.C ,Comment.lastReplyRegisterd
FROM [BoardContents] 
	left join (SELECT [ContentsPk] ,max(Registerd) AS lastReplyRegisterd, count(*) AS C FROM [BoardComment] WHERE isnull(Deleted, '')='' Group by ContentsPk) AS Comment ON Comment.ContentsPk=Pk 
WHERE isnull(DeletedID, '')='' and [BoardCode]=@BoardCode and [ParentsPk] <@ParentsPk ORDER BY ParentsPk DESC;";
		RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string position = RS["Position"] + "";
			string[] RowData = new string[6];
			RowData[0] = RS["Pk"] + "";
			RowData[1] = RS["Header"] + "";

			if (position == "") {
				RowData[2] = RS["Title"] + "";
			} else {
				int underC = position.Length / 3 + 1;
				RowData[2] = "";
				for (int i = 0; i < underC; i++) {
					RowData[2] += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
				}
				RowData[2] += "→" + RS["Title"];
			}

			DateTime registerd = DateTime.Parse(RS["Registerd"] + "");
			string newimg = "";
			if (DateTime.Now.AddDays(-1) < registerd) {
				newimg += " <img src=\"../Images/Board/new.gif\" align=\"absmiddle\">";
			}

            
            
            if (RS["lastReplyRegisterd"] + "" != "")
            {
                DateTime lastReplyRegisterd = DateTime.Parse(RS["lastReplyRegisterd"] + "");

                if (DateTime.Now.AddDays(-1) < lastReplyRegisterd)
                {   
                    newimg += " <img src=\"../Images/Board/Renew.gif\" align=\"absmiddle\">";
                }
            }
            
			string commentC = "";
			if (RS["C"] + "" != "") {
				commentC = " [" + RS["C"] + "]";
			}

			if (RS["Pk"] + "" == ContentsPk) {
                RowData[2] = "<strong>" + RowData[2] + commentC + newimg +  "</strong>";
			} else {
				RowData[2] = "<span onclick=\"GoView('" + BoardCode + "', '" + RS["Pk"] + "');\">" + RowData[2] + commentC + newimg + "</span>";
			}
			RowData[3] = RS["AccountID"] + " / <span style=\"color:blue;\">" + RS["Name"] + "</span>";
			RowData[4] = RS["Registerd"].ToString().Substring(5, 5) + " " + registerd.Hour + ":" + registerd.Minute;
			RowData[5] = RS["ReadCount"] + "";
			TempReturnHTML.Append(String.Format(RowFormat, RowData));
		}
		RS.Dispose();


		DB.DBCon.Close();
		BoardContentsList = TempReturnHTML + "";
		return true;
	}
	private Boolean LoadAttachedFiles(string Pk)
	{
		DB.SqlCmd.CommandText = "SELECT [Pk], [FileTitle] FROM [BoardAttachedFile] WHERE ContentsPk=" + Pk + " and isnull(Deleted, '')='';";
		string eachformat = "<p><a href='../UploadedFiles/FileDownload.aspx?S={0}&T=Board' >{1}</a></p>";
		StringBuilder ReturnValue = new StringBuilder();
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append(string.Format(eachformat, RS["Pk"] + "", RS["FileTitle"] + ""));
		}
		RS.Dispose();
		DB.DBCon.Close();
		AttachedFiles = ReturnValue + "";
	   return true;
	}
	private Boolean LoadBoardListForBoardMove(string contentsPk, string boardcode, string accountid)
	{
		DB.SqlCmd.CommandText = @"
DECLARE @Count int;
DECLARE @Position varchar(20); 

SELECT @Count=count(*) 
FROM [BoardLibCode] AS C left join [BoardLibPermission] AS P On P.BoardCode=C.[BoardCode] 
WHERE P.[TargetID]='" + accountid+@"' and PermissionType='A' and C.BoardCode='"+boardcode+ @"'  ;
if @Count>0 BEGIN 
	SELECT @Position=Position From BoardContents WHERE Pk="+contentsPk+ @";
	if isnull( @Position, '')='' BEGIN
		SELECT C.[BoardCode], C.[Title] 
		FROM [BoardLibCode] AS C left join [BoardLibPermission] AS P On P.BoardCode=C.[BoardCode] 
		WHERE P.[TargetID]='" + accountid + @"' and PermissionType='A'
		ORDER BY C.OrderBy ;
	END
END ";
		//Response.Write(DB.SqlCmd.CommandText);
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		StringBuilder InnerOption = new StringBuilder();
		while (RS.Read()) {
			if (boardcode == RS[0] + "") {
				InnerOption.Append("<option value=\"" + RS[0] + "\" selected=\"selected\">" + RS[1] + "</option>");
			} else {
				InnerOption.Append("<option value=\"" + RS[0] + "\">" + RS[1] + "</option>");
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (InnerOption + "" != "") {
			BoardTitle = "<select id=\"BoardCodeForMove\">" + InnerOption + "</select>&nbsp;<input type=\"button\" value=\"이동\" onclick=\"MoveBoardCode();\" >";
		}
		//DB.DBCon.Open();

		return true;
	}
}