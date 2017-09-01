using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Member_MyStaffView : System.Web.UI.Page
{
	protected String[] MEMBERINFO;
	protected String BTNSUBMIT;
	private DBConn DB;
	protected StringBuilder StaffList;
	protected Int32 RowCount;
    protected void Page_Load(object sender, EventArgs e)
    {
		if (Session["MemberInfo"] == null || Session["MemberInfo"] + "" == "")
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "alertscript", "<script type='text/javascript'>alert('로그인 해주세요'); location.href='../Default.aspx';</script>", false);
		}

		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } }
		catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		if (Session["MemberInfo"] == null) { Response.Redirect("../Default.aspx"); }
		MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
		if (MEMBERINFO[4] == "President")
		{
			BTNSUBMIT = "		<input type=\"button\" value=\"수정\" style=\"width:150px; height:40px;\" onclick=\"BTN_Submit_Click();\" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
									"	<input type=\"button\" value=\"취소\" style=\"width:150px; height:40px;\" onclick=\"history.back();\" />";
		}
		else
		{
			BTNSUBMIT = "";
		}
		DB = new DBConn();
		LoadSubID(MEMBERINFO[1]);
    }
	private void LoadSubID(string CompanyPk)
	{
		StaffList = new StringBuilder();
		string EachStaffFormat = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width:850px;\">" +
			"<tr>" +
			"	<td class=\"Line1E8E8E8\" rowspan=\"4\" style=\"width:90px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px; font-weight:bold;\">{1}<input type=\"hidden\" id=\"Damdangja[{0}]Pk\" value=\"{2}\" /><input type=\"hidden\" id=\"Damdangja[{0}]Authority\" value=\"{11}\" /></td>" +
			"	<td class=\"Line1E8E8E8\" style=\"width:90px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;\">" + GetGlobalResourceObject("Member", "JikWi") + "</td>" +
			"	<td class=\"Line1E8E8E8\" style=\"width:140px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;\">" + GetGlobalResourceObject("Member", "Name") + "</td>" +
			"	<td class=\"Line1E8E8E8\" style=\"background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;\">ID</td>" +
			"	<td class=\"Line1E8E8E8\" style=\"width:160px; background-color:#f5f5f5; border-right:dotted 1px #BBBBBB; text-align:center; height:25px; padding:4px;\">Password</td></tr>" +
			"<tr>" +
			"	<td class=\"tdStaffBody\"><input type=\"text\" id=\"Damdangja[{0}]Duties\" value=\"{3}\"  style=\"width:70px;\" /></td>" +
			"	<td class=\"tdStaffBody\"><input type=\"text\" id=\"Damdangja[{0}]Name\" value=\"{4}\"  style=\"width:110px;\" /></td>" +
			"	<td class=\"tdStaffBody\">" +
			"		<input type=\"text\" id=\"Damdangja[{0}]ID\" value=\"{5}\"  style=\"width:110px;\" />" +
			"		<input type=\"hidden\" id=\"Damdangja[{0}]HID\" value=\"{5}\"  />" +
			"		<input type=\"hidden\" id=\"Damdangja[{0}]IsChecked\" value=\"0\"  />" +
			"		<input type=\"button\" value=\"ID Check\" onclick=\"IDCheck('{0}');\" /> </td>" +
			"	<td class=\"tdStaffBody\"><input type=\"password\" id=\"Damdangja[{0}]PWD\" style=\"width:130px;\" value=\"{10}\" /></td></tr>" +
			"<tr>" +
			"	<td class=\"tdStaffBody\" colspan=\"5\" style=\"text-align:left;\">" +
			"		<span style=\"padding-left:20px;\">TEL : <input type=\"text\" style=\"width:100px;\" id=\"Damdangja[{0}]TEL\" value=\"{6}\"  /></span>" +
			"		<span style=\"padding-left:40px;\"><select id=\"Damdangja[{0}]MSG\" value=\"{7}\"  ><option value=\"3\">All</option><option value=\"2\">Email</option><option value=\"1\">SMS</option><option value=\"0\">X</option> </select></span>" +
			"		<span style=\"padding-left:20px;\">Mobile : <input type=\"text\" style=\"width:110px;\" id=\"Damdangja[{0}]Mobile\" value=\"{8}\"  /></span>" +
			"		<span style=\"padding-left:20px;\">E-mail : <input type=\"text\" id=\"Damdangja[{0}]Email\" size=\"27\" value=\"{9}\"  /></span></td></tr></table>" +
			"	<div style=\"background-color:#999999; height:1px; font-size:1px; \"></div>";

		DB.SqlCmd.CommandText = "	SELECT AccountPk, AccountID, Password, Duties, Name, TEL, Mobile, Email, Authority, IsEmailNSMS " +
													"	FROM Account_ " +
													"	WHERE CompanyPk=" + CompanyPk + " and GubunCL=91 ;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		RowCount = 0;
		while (RS.Read()) {
			string[] eachrow = new string[] { 
				RowCount.ToString(), 
				RS["Authority"]+""=="0"?GetGlobalResourceObject("Member", "Staff_Accountancy").ToString() :GetGlobalResourceObject("Member", "Staff_FreightMan").ToString() , 
				RS["AccountPk"]+"", 
				RS["Duties"]+"", 
				RS["Name"]+"",	
				RS["AccountID"]+"", 
				RS["TEL"]+"", 
				RS["IsEmailNSMS"]+"", 
				RS["Mobile"]+"", 
				RS["Email"]+"", 
				RS["Password"]+""==""?"":"******", 
				RS["Authority"]+""
			};
			StaffList.Append(string.Format(EachStaffFormat, eachrow));
			RowCount++;
		}
	}
}