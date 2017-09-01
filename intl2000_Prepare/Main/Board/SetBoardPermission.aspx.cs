using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Board_SetBoardPermission : System.Web.UI.Page
{
	protected String TBBody;
	protected void Page_Load(object sender, EventArgs e)
	{
		LoadPermissionList(Request.Params["C"].ToString());
	}
	private Boolean LoadPermissionList(string BoardCode)
	{
        string RowFormat = "<tr><td align=\"center\" style=\"height:25px;\" >{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td><td align=\"center\">{4}</td><td align=\"center\"><a onclick=\"DeleteThis('{5}')\" onfocus=\"this.blur()\"><img src=\"../Images/Board/del.gif\"  border=\"0\"></a></td></tr>";
		StringBuilder TempBody = new StringBuilder();
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT BP.[Pk]
	, BP.[PermissionType]
    , BP.[TargetID]
    , A.[Duties]
    , A.[Name]
    , C.CompanyName 
FROM [BoardLibPermission] AS BP 
	left join Account_ AS A ON BP.TargetID=A.AccountID 
	left join Company AS C ON A.[CompanyPk]=C.CompanyPk 
WHERE BP.BoardCode='" + BoardCode + "' ORDER BY C.CompanyName DESC, BP.TargetID ASC;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			string PermissionCode = RS["PermissionType"] + "";
			string PermissionText;
			switch (PermissionCode) {
				case "A":
					PermissionText = "읽기 / 쓰기";
					break;
				case "R":
					PermissionText = "읽기";
					break;
				case "W":
					PermissionText = "쓰기";
					break;
				default:
					PermissionText = "??";
					break;

			}
			TempBody.Append(String.Format(RowFormat, RS["CompanyName"] + "", RS["Duties"] + "", RS["Name"] + "", RS["TargetID"] + "", PermissionText, RS["Pk"]+""));
		}
		RS.Dispose();
		DB.DBCon.Close();
		TBBody = TempBody.ToString();
		return true;
	}
}