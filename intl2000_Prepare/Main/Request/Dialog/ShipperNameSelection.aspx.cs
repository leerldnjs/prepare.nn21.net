using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;
using System.Data.SqlClient;

public partial class Request_Dialog_ShipperNameSelection : System.Web.UI.Page
{
	protected String ShipperData;
	protected String DefaultConnection;
	private DBConn DB;
	protected String CompanyPk;
	protected String ShipperPk;
	protected String ShipperCode;
	protected String ConsigneePk;
	protected String ConsigneeCode;
	protected String Registerd, Contents;

	protected void Page_Load(object sender, EventArgs e)
	{
		try { switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; } } catch (Exception) { }

		CompanyPk = Request.Params["S"] + "";
		ShipperPk = Request.Params["Shipper"] + "";
		ConsigneePk = Request.Params["Consignee"] + "";
		ShipperData = HistoryOfShipperNameInDocument(CompanyPk);
		Html_Contents(CompanyPk, out Registerd, out Contents);

		Response.Expires = 0;
		Response.Cache.SetNoStore();
		Response.AppendHeader("Pragma", "no-cache");
	}
	private String Html_Contents(string CompanyPk, out string Registerd, out string Contents)
	{
		DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT [CompanyCode] FROM [dbo].[Company] WHERE [CompanyPk] = " + ShipperPk;
		ShipperCode = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = @"SELECT [CompanyCode] FROM [dbo].[Company] WHERE [CompanyPk] = " + ConsigneePk;
		ConsigneeCode = DB.SqlCmd.ExecuteScalar() + "";
		DB.DBCon.Close();

		string ReturnValue = new Admin().LoadTalkBusiness(CompanyPk, "ShipperSelection");
		if (ReturnValue == "") {
			var Each = ReturnValue.Split(new string[] { "@@" }, StringSplitOptions.None);
			Registerd = "";
			Contents = "";
		} else {
			var Each = ReturnValue.Split(new string[] { "@@" }, StringSplitOptions.None);
			Registerd = "Registerd Info: <strong>" + Each[0] + " / " + Each[2] + "</strong>";
			Contents = Each[1];
		}

		return "1";
	}
	private String HistoryOfShipperNameInDocument(string WriterPk)
	{
		DB = new DBConn();
		DB.SqlCmd.CommandText = @"
SELECT 
	CID.CompanyInDocumentPk, CID.Name, CID.Address, F.FilePk, F.FileName, F.[PhysicalPath]  ,CID.CompanyNo,CID.CustomsCode,CID.Title,CID.ZipCode1,CID.ZipCode2,CID.Name_KOR,CID.Address_KOR
FROM 
	CompanyInDocument AS CID 
	left join (
		SELECT FilePk, GubunPk, FileName, [PhysicalPath] FROM [File] WHERE isnull(GubunCL, 3)=3 ) AS F On CID.CompanyInDocumentPk=F.GubunPk  
WHERE CID.GubunPk=" + WriterPk + " and CID.GubunCL=0";
		StringBuilder ReturnValue = new StringBuilder();
		DB.DBCon.Open();
		string TRFormat = "	<tr><td style='text-align:center; border-bottom:dotted 1px #93A9B8; '>{0}</td>" +
										"	<td style='border-bottom:dotted 1px #93A9B8; '>&nbsp;&nbsp;<strong>CompanyCode:</strong>{7}<br />&nbsp;&nbsp;<strong>Name:</strong><strong>{1}</strong><br />&nbsp;&nbsp;<strong>Addr:</strong>{2}<br />&nbsp;&nbsp;<strong>Name_KOR:</strong>{10}<br />&nbsp;&nbsp;<strong>Address_KOR:</strong>{11}<br />&nbsp;&nbsp;<strong>CompanyNo:</strong>{5}<br />&nbsp;&nbsp;<strong>CustomsCode:</strong>{6}<br />&nbsp;&nbsp;<strong>ZipCode:</strong>{8}-{9}</td>" +
										"	<td style='text-align:center; border-bottom:dotted 1px #93A9B8; '><input type=\"button\" style='width:60px;' value=\"select\" onclick=\"InsertSuccess2('{3}')\" /><br />" +
										"	<input type=\"button\" style='width:60px;color:red;' value=\"delete\" onclick=\"Del('{4}')\" />" +
										"<br />" +
										"	<input type=\"button\" style='width:60px;color:#1DDB16;' value=\"modify\" onclick=\"modify('','{4}','{1}','{2}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')\" /></td></tr>";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			//CID.CompanyInDocumentPk, CID.Name, CID.Address, F.FilePk, F.FileName, F.[PhysicalPath
			string[] TRData = new string[] {
				RS["FilePk"]+""==""?
					"<input type=\"button\" value=\"img\" onclick=\"GoFileupload('" + RS["CompanyInDocumentPk"] + "')\" />":
					"<a href=\"../../UploadedFiles/FileDownload.aspx?S=" + RS["FilePk"] + "\" >"+
						"<img src='../../UploadedFiles/" + RS["PhysicalPath"]+"' style=\"border:0px; width:45px; height:45px;\" /></a>"+
						"&nbsp;<span onclick=\"FileDelete('" + RS["FilePk"] + "');\" style='color:red;'>X</span>", 
				RS["Name"]+"", 
				RS["Address"]+"", 
				RS[0] + "##" + RS[1] + "##" + RS[2] + "##" + RS[3] + "##" + RS[4], 
				RS["CompanyInDocumentPk"]+"",	
				RS["CompanyNo"]+"",
				RS["CustomsCode"]+"",
				RS["Title"]+"",
				RS["ZipCode1"]+"",
				RS["ZipCode2"]+"",
				RS["Name_KOR"]+"",
					RS["Address_KOR"]+""	
			};
			ReturnValue.Append(String.Format(TRFormat, TRData));
			//Query.Append("@@@@" + RS[0] + "##" + RS[1] + "##" + RS[2] + "##" + RS[3] + "##" + RS[4]);
		}
		RS.Dispose();
		DB.DBCon.Close();
		return "<table style=\"width:710px;\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" + ReturnValue +
			"<tr><td style='text-align:center;'><strong>PRIVATE</strong></td>" +
			"	<td>&nbsp;&nbsp;CompanyCode : <input type=\"text\" id=\"CustomerClearanceCompanyCode\" style=\"text-align:left; width:165px;\" /><br/>" +
				"&nbsp;&nbsp;Name : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"CustomerClearanceCompanyName\" style=\"text-align:left; width:450px;\" /><br/>" +
				"&nbsp;&nbsp;Address : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"CustomerClearanceAddress\" style=\"text-align:left; width:450px;\" /><br/>" +
				"&nbsp;&nbsp;Name_KOR: &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"CustomerClearanceName_KOR\" style=\"text-align:left; width:450px;\" /><br/>" +
				"&nbsp;&nbsp;Address_KOR : &nbsp;<input type=\"text\" id=\"CustomerClearanceAddress_KOR\" style=\"text-align:left; width:450px;\" /><br/>" +
				"&nbsp;&nbsp;CompanyNo : &nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"CustomerClearanceCompanyNo\" style=\"text-align:left; width:150px;\" />" +
				"&nbsp;&nbsp;CustomsCode : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"CustomerClearanceCustomsCode\" style=\"text-align:left; width:150px;\" /><br/>" +
				"&nbsp;&nbsp;ZipCode : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type=\"text\" id=\"CustomerClearanceZipCode1\" style=\"text-align:left; width:50px;\" />&nbsp;-&nbsp;<input type=\"text\" id=\"CustomerClearanceZipCode2\" style=\"text-align:left; width:50px;\" /></td>" +
			"<td ><input type=\"button\" style=\"width:70px;\" value=\"insert\" onclick=\"InsertShipperInDocument_CustomsCode('0', '" + Request.Params["S"] + "', form1.CustomerClearanceCompanyName.value, form1.CustomerClearanceAddress.value,form1.CustomerClearanceCompanyNo.value,form1.CustomerClearanceCustomsCode.value,form1.CustomerClearanceZipCode1.value,form1.CustomerClearanceZipCode2.value,form1.CustomerClearanceName_KOR.value,form1.CustomerClearanceAddress_KOR.value,form1.CustomerClearanceCompanyCode.value,form1.CustomerClearancePk.value);\" /></td>" +
			"</tr></table>";
	}
}