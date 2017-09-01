using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Data.SqlClient;

public partial class Admin_Taxpaid : System.Web.UI.Page
{
	protected String ListHtml;
	protected String[] MEMBERINFO;
	private DBConn DB;
	private string AccountID;
	protected void Page_Load(object sender, EventArgs e) {

		try {
			MEMBERINFO = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}
		AccountID = MEMBERINFO[2] + "";
		ListHtml = MakeHtml();
	}
	private string MakeHtml() {
		DB = new DBConn();
		DBConn DB_Ready = new DBConn("ReadyKorea");
		 		DB.SqlCmd.CommandText = @"SELECT 
a.rece ,a.rpt_no,a.BlNo	  
,r.Consigneecode
,company.CompanyName as CompanyName
,R.TotalPackedCount,R.PackingUnit
,a.Tot_Gs,a.Tot_Vat
,ISNULL(Tariff1.TSum, 0) as Tariff1
,ISNULL(Tariff2.TSum, 0) as Tariff2
,ISNULL(RHead.[TOTAL_PRICE], 0) AS Charge
,ISNULL(RHead.[DEPOSITED_PRICE],0) AS Deposited
,R.RequestFormPk	  
,Radd761.[REGISTERD] as ActDate761,Radd761.[ACCOUNT_ID] as ActID761
,Radd762.[REGISTERD] as ActDate762,Radd762.[ACCOUNT_ID] as ActID762
,c.CommercialDocumentHeadPk
,R.DocumentStepCL AS DocumentStepCL
FROM   [edicus].[dbo].CUSDEC929A1  a
left join [INTL2010].[dbo].[CommercialDocument]  c on a.BlNo=c.BLNo
left join [INTL2010].[dbo].[CommerdialConnectionWithRequest] ccwr on c.CommercialDocumentHeadPk=ccwr.CommercialDocumentPk
left join [INTL2010].[dbo].[RequestForm] R on ccwr.RequestFormPk=r.RequestFormPk
left join [INTL2010].[dbo].[Company] Company on R.ConsigneePk=Company.CompanyPk 
Left join [dbo].[REQUESTFORMCALCULATE_HEAD] AS RHead on R.RequestFormPk=RHead.[TABLE_PK] 
left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) As TSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff1 ON RHead.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff1.[REQUESTFORMCALCULATE_HEAD_PK]
		left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) As TSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='부가세' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff2 ON RHead.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff2.[REQUESTFORMCALCULATE_HEAD_PK] 
		left join (
		SELECT [REQUESTFORMCALCULATE_HEAD_PK], SUM([ORIGINAL_PRICE]) As TSum FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세사비' GROUP BY [REQUESTFORMCALCULATE_HEAD_PK]
		) AS Tariff3 ON RHead.[REQUESTFORMCALCULATE_HEAD_PK]=Tariff3.[REQUESTFORMCALCULATE_HEAD_PK]
left join (
SELECT * FROM [dbo].[HISTORY] WHERE [CODE]='761'
)AS Radd761 on R.RequestFormPk=Radd761.RequestFormPk 
left join (
SELECT * FROM [dbo].[HISTORY] WHERE [CODE]='762'
)AS Radd762 on R.RequestFormPk=Radd762.RequestFormPk 
WHERE a.rece in ('결재') 
AND ISNULL(RHead.[TABLE_NAME], 'RequestForm') = 'RequestForm'
AND a.Rpt_Seq='00'
ORDER BY rpt_day DESC, rpt_no DESC, rpt_seq";


		DB.DBCon.Open();
		StringBuilder ReturnValue = new StringBuilder();
		string RowFormat = "<tr>" +
			  "<td class='TBody1' style='text-align:left;' ><a href=\"RequestView.aspx?g=c&pk={11}\" >{0}</a></td>" +
			  "<td class='TBody1' style='text-align:left;' ><a href=\"CheckDescription.aspx?S={12}\" >{1}</td>" +
			  "<td class='TBody1' style='text-align:left;' >{2}</td>" +
			  "<td class='TBody1' style='text-align:center;' >{3}</td>" +
			  "<td class='TBody1' style='text-align:right; {16}' >{4}</td>" +
			  "<td class='TBody1' style='text-align:right; {17}' >{5}</td>" +
			  "<td class='TBody1' style='text-align:right; {16}' >{6}</td>" +
			  "<td class='TBody1' style='text-align:right;  {17}' >{7}</td>" +
			  "<td class='TBody1' style='text-align:right;' >{8}</td>" +
			  "<td class='TBody1' style='text-align:right;' >{9}</td>" +
			  "<td class='TBody1' style='text-align:center; {14}' >{10}</td>" +
			  "<td class='TBody1' style='text-align:center; {15}' >{13}</td></tr>";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			string Style1 = "";
			string Style2 = "";
			string Style3 = "";
			string Style4 = "";
			String[] RowData = new String[18];

			if (Common.NumberFormat(RS["Tot_Gs"].ToString()) != Common.NumberFormat(RS["Tariff1"].ToString())) {
				Style3 = "background-color:#FFA2A2;";
			}
			if (Common.NumberFormat(RS["Tot_Vat"].ToString()) != Common.NumberFormat(RS["Tariff2"].ToString())) {
				Style4 = "background-color:#FFA2A2;";
			}



			RowData[0] = RS["rpt_no"].ToString();
			RowData[1] = RS["BlNo"].ToString();
			RowData[2] = RS["Consigneecode"].ToString() + " <br /> " + RS["CompanyName"].ToString();
			RowData[3] = RS["TotalPackedCount"].ToString() + Common.GetPackingUnit(RS["PackingUnit"] + "");
			RowData[4] = Common.NumberFormat(RS["Tot_Gs"].ToString());
			RowData[5] = Common.NumberFormat(RS["Tot_Vat"].ToString());
			RowData[6] = Common.NumberFormat(RS["Tariff1"].ToString());
			RowData[7] = Common.NumberFormat(RS["Tariff2"].ToString());

			string Deposited = RS["Deposited"] + "" == "" ? "0" : RS["Deposited"] + "";
			string Charge = RS["Charge"] + "" == "" ? "0" : RS["Charge"] + "";
			decimal d_Deposited = decimal.Parse(Deposited);
			decimal d_Charge = decimal.Parse(Charge);
			RowData[8] = Common.NumberFormat(RS["Charge"].ToString());

			decimal calc = d_Deposited - d_Charge;
			if (calc == 0) {
				RowData[9] = "<span style='color:green;'>完</span>";
			} else if (calc > 0) {
				RowData[9] = "<span style='color:blue;'>" + Common.NumberFormat(calc.ToString()) + "</span>";
			} else {
				RowData[9] = "<span style='color:red;'>" + Common.NumberFormat(calc.ToString()) + "</span>";
			}
			if (RS["ActDate761"] + "" != "") {
				RowData[10] = RS["ActID761"] + " <br /> " + RS["ActDate761"].ToString().Substring(5, 5);
				Style1 = "background-color:#FFFACD;";
			} else {
				string TempDocumentStepCL = "";
				switch (RS["DocumentStepCL"] + "") {
					case "7":
						TempDocumentStepCL = "10";
						break;
					case "8":
						TempDocumentStepCL = "11";
						break;
					case "9":
						TempDocumentStepCL = "12";
						break;

				}
				RowData[10] = "<input type=\"button\" value=\"세납\" onclick=\"SetTaxpaid('" + RS["RequestFormPk"].ToString() + "','" + AccountID + "','761','" + TempDocumentStepCL + "');\" />";
			}
			if (RS["ActDate762"] + "" != "") {
				RowData[13] = RS["ActID762"] + " <br /> " + RS["ActDate762"].ToString().Substring(5, 5);
				Style2 = "background-color:skyblue;";
			}
			else {
				if (MEMBERINFO[2] == "ilic66" || MEMBERINFO[2] == "ilic77" || MEMBERINFO[2] == "ilic55") {
					RowData[13] = "<input type=\"button\" value=\"납부\" onclick=\"SetTaxpaid('" + RS["RequestFormPk"].ToString() + "','" + AccountID + "','762','');\" />";
				}
				else {
					RowData[13] = "총무전용";
				}
			}

			RowData[11] = RS["RequestFormPk"].ToString();
			RowData[12] = RS["CommercialDocumentHeadPk"].ToString();
			RowData[14] = Style1;
			RowData[15] = Style2;
			RowData[16] = Style3;
			RowData[17] = Style4;
			ReturnValue.Append(String.Format(RowFormat, RowData));
		}

		DB.DBCon.Close();
		return "<table border='0' cellpadding='0' cellspacing='0' style='width:1050px;' >" +
		"<thead><tr style=\"height:40px;\">" +
		"	<td class='THead1' style=\"width:120px; \" >신고번호</td>" +
		"	<td class='THead1' style=\"width:120px; \">BL번호</td>" +
		"	<td class='THead1'   >CompanyName</td>" +
		"	<td class='THead1' style=\"width:30px; \"  >CT</td>" +
		"	<td class='THead1' style=\"width:80px; \"  >R관세</td>" +
		"	<td class='THead1' style=\"width:80px; \"  >R부가세</td>" +
		"	<td class='THead1' style=\"width:80px; \"  >N관세</td>" +
		"	<td class='THead1' style=\"width:80px; \"  >N부가세</td>" +
		"	<td class='THead1' style=\"width:80px; \"  >총액</td>" +
		"	<td class='THead1' style=\"width:80px; \"  >차액</td>" +
		"	<td class='THead1' style=\"width:90px; \"  >업무</td>" +
		"	<td class='THead1' style=\"width:90px; \"  >재무</td>" +
		"	</tr></thead>" + ReturnValue +
		"	</table>";
	}
}