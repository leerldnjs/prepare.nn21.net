using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_ManagementStorage : System.Web.UI.Page
{
	protected String Html_Warehouse;
	protected String[] MemberInfo;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["MemberInfo"] + "" == "" || Session["MemberInfo"] + "" == null) { Response.Redirect("../Default.aspx"); }
		MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None);
		Html_Warehouse = LoadList(Request.Params["S"]);
	}
	private string LoadList(string BranchPk)
	{
		string FormatTable = "<table border='0' cellpadding='0' cellspacing='0' style='width:850px;' ><thead><tr height='30px'>" +
						"<td class='THead1' style='width:180px;' >Title</td>" +
						"<td class='THead1' style='width:80px; ' >Code</td>" +
						"<td class='THead1' >Address</td>" +
						"<td class='THead1' style='width:100px; ' >TEL</td>" +
						"<td class='THead1' style='width:100px; ' >FAX</td>" +
						"<td class='THead1'style='width:190px;' >Button</td>" +
					"</tr></thead>" +
					"{0}" +
					"<TR><td colspan='3' style='background-color:#F5F5F5; text-align:center; padding:20px; '>&nbsp;</TD></TR></Table>";

		string TableRowFormat = "	<tr height='30px; '><td class='{0}' >{1}</td>" +
										"		<td class='{0}' style='text-align:left;' >{6}</td>" +
										"		<td class='{0}' style='text-align:left;' >{2}</td>" +
										"		<td class='{0}' style='text-align:left;' >{3}</td>" +
										"		<td class='{0}' style='text-align:left;' >{4}</td>" +
										"		<td class='{0}' style='text-align:right;' >{5}</td></tr>";
		StringBuilder Inner = new StringBuilder();
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT [OurBranchStoragePk], [OurBranchCode], [StorageName],[StorageCode], [StorageAddress], [TEL], [FAX]
	, [StaffName], [StaffMobile], [Description], [NaverMapPathX], [NaverMapPathY], [Memo] 
FROM [dbo].[OurBranchStorageCode] 
WHERE OurBranchCode=" + BranchPk + @" and IsUse is null order by [StorageName];";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
         string BTN = "<input type='button' value='수정' onclick=\"PopWarehouseModify('" + RS["OurBranchStoragePk"] + "');\" /><input type='button' value='사용안함' onclick=\"WarehouseUseNo('" + RS["OurBranchStoragePk"] + "')\"/>";
			if (RS["NaverMapPathX"] + "" != "" && RS["NaverMapPathY"] + "" != "") {
				BTN = "<input type=\"button\" onclick=\"PopWarehouseMap('" + RS["OurBranchStoragePk"] + "');\" value=\"약도\">" + BTN;
			}
			Inner.Append(string.Format(TableRowFormat, "TBody1", RS["StorageName"] + "", RS["StorageAddress"] + "", RS["TEL"] + "", RS["FAX"] + "", BTN, RS["StorageCode"]+""));
		}
		RS.Dispose();
		DB.DBCon.Close();

		return string.Format(FormatTable, Inner.ToString());
	}
}