using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Components;
using System.Data.SqlClient;

public partial class Request_OfferFormList : System.Web.UI.Page
{
	protected StringBuilder OfferList;
	protected String[] MemberInfo;
	private int PageNo;
	private int PageLength;
	protected String PageNoHtml;
	protected void Page_Load(object sender, EventArgs e)
	{
		try { MemberInfo = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None); }
		catch ( Exception ) { Response.Redirect("../Default.aspx"); }

		try { PageNo = Int32.Parse(Request["PageNo"]); }
		catch ( Exception ) { PageNo = 1; }

		try { if (Request["Language"].Length == 2) { Session["Language"] = Request["Language"]; } }
		catch (Exception) { }
		switch (Session["Language"] + "") { case "en": Page.UICulture = "en"; break; case "ko": Page.UICulture = "ko"; break; case "zh": Page.UICulture = "zh-cn"; break; }

		PageLength = 15;
		SetOfferList();
		//PageNoHtml=new Common().SetPageListByNo(PageLength, PageNo, totlaRecord, "RequestList.aspx", "?G=5051&")
	}
	private bool SetOfferList()
	{
		OfferList = new StringBuilder();
		Int32 StepCL;
		string BTNInvoice;
		string BTNPackingList;
		string BTNOfferSheet;
		string GoOfferWrite;
		string Description;
		string ShipperName;
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"SELECT 
		RF.RequestFormPk, RF.ConsigneePk, RF.ShipperStaffName, RF.StepCL, CCL.CompanyName, RF.RequestDate, 
		CID.Name, RI.Description, RII.itemCount 
	FROM RequestForm as RF 
		left join Company as CCL on RF.ConsigneePk=CCL.CompanyPk 
		left join (Select [TABLE_PK], [REGISTERD] as ActDate  From [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [CODE]='2') AS RFAI 
			ON RFAI.[TABLE_PK]=RF.RequestFormPk 
		LEFT JOIN CompanyInDocument as CID on RF.CompanyInDocumentPk=CID.CompanyInDocumentPk 
		LEFT OUTER JOIN RequestFormItems as RI on RF.RequestFormPk=RI.RequestFormPk
		LEFT JOIN ( SELECT S.RequestFormPk, Count(*) as itemCount FROM RequestFormItems as S group by RequestFormPk )as RII 
			on RF.RequestFormPk=RII.RequestFormPk 
	WHERE RF.ShipperPk=" + MemberInfo[1] + @" and RF.StepCL>0 and RF.StepCL<50 
	ORDER BY RFAI.ActDate DESC;";
		//Response.Write(DB.SqlCmd.CommandText);
		//return true;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		/*		0		RF.RequestFormPk,		RF.ConsigneePk,		RF.StepCL,		CCL.TargetCompanyName,		CID.Name			*/

		String TempRequestFormPk = string.Empty;
		for ( int i = 0; i < (PageNo - 1) * PageLength; i++ )
		{
			if ( RS.Read() )
			{
				if ( TempRequestFormPk != RS[0] + "" ) { TempRequestFormPk = RS[0] + ""; continue; }
				else { i--; continue; }
			}
			else { break; }
		}
		
		for ( int i = 0; i < PageLength; i++ )
		{
			/*		0		RF.RequestFormPk,		RF.ConsigneePk,			RF.ShipperStaffName,		RF.StepCL,		CCL.TargetCompanyName, 
			 *		5		RF.RequestDate,			CID.Name,					RI.Description, RII.itemCount			*/
			if ( RS.Read() )
			{
				if ( TempRequestFormPk != RS[0] + "" )
				{
					TempRequestFormPk = RS[0] + "";
					StepCL = Int32.Parse(RS["StepCL"] + "");

					ShipperName = (RS["Name"] + "").Length > 10 ? ShipperName = (RS["Name"] + "").Substring(0, 10) + "..." : ShipperName = RS["Name"] + "";
					BTNInvoice = StepCL % 2 == 1 ? "<input type=\"button\" value=\"View\" onclick=\"ViewDocu('I', '" + RS[0] + "')\" />" : "<input type=\"button\" value=\"Write\" onclick=\"WriteDocu('I', '" + RS[0] + "')\" />";
					BTNPackingList = (StepCL / 2) % 2 == 1 ? "<input type=\"button\" value=\"View\" onclick=\"ViewDocu('P', '" + RS[0] + "')\" />" : "<input type=\"button\" value=\"Write\" onclick=\"WriteDocu('P', '" + RS[0] + "')\" />";
					BTNOfferSheet = (StepCL / 4) % 2 == 1 ? "<input type=\"button\" value=\"View\" onclick=\"ViewDocu('O', '" + RS[0] + "')\" />" : "<input type=\"button\" value=\"Write\" onclick=\"WriteDocu('O', '" + RS[0] + "')\" />";
					GoOfferWrite = "<input type=\"button\" value=\"View\" onclick=\"ViewDocu('D', '" + RS[0] + "')\" />";
					Description = RS["Description"] + (RS["itemCount"] + "" == "" ? "" : " (" + RS["itemCount"] + ")");
					OfferList.Append("<tr style=\"height:25px;\">" +
													"<td align=\"center\" class=\"Line1E8E8E8\" >" + RS["RequestDate"].ToString().Substring(0, 10) + "</td>" +
													"<td align=\"center\" class=\"Line1E8E8E8\" >" + RS["ShipperStaffName"] + "</td>" +
													"<td align=\"center\" class=\"Line1E8E8E8\" >" + ShipperName + "</td>" +
													"<td align=\"center\" class=\"Line1E8E8E8\" >" + RS["CompanyName"] + "</td >" +
													"<td align=\"center\" class=\"Line1E8E8E8\" >" + Description + "</td>" +
													"<td align=\"center\" class=\"Line1E8E8E8\" >" + BTNInvoice + "</td>" +
													"<td align=\"center\" class=\"Line1E8E8E8\" >" + BTNPackingList + "</td>" +
													//"<td align=\"center\" class=\"Line1E8E8E8\" >" + BTNOfferSheet + "</td>" +
													"<td align=\"center\" class=\"Line1E8E8E8\" >" + GoOfferWrite + "</td>" +
													"<td align=\"center\" class=\"Line1E8E8E8\" ><input type=\"button\" value=\"DEL\" onclick=\"DeleteDocu('" + RS[0] + "')\" /></td>" +
												"</tr>");
					continue;
				}
				else { i--; continue; }
			}
			else { break; }
		}
		RS.Dispose();
		DB.SqlCmd.CommandText = "SELECT Count(*) FROM RequestForm WHERE ShipperPk=" + MemberInfo[1] + " and StepCL>0 and StepCL<50 ";
		string TotalRecord = DB.SqlCmd.ExecuteScalar() + "";
		PageNoHtml = new Common().SetPageListByNo(PageLength, PageNo, Int32.Parse(TotalRecord), "OfferFormList.aspx", "?");
		DB.DBCon.Close();
		return true;
	}
}