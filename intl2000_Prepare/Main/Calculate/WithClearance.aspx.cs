using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;
using System.Text;

public partial class Calculate_WithClearance : System.Web.UI.Page
{
	private DBConn DB;
	protected String TBList;
	private String CompanyPk;
	protected void Page_Load(object sender, EventArgs e) {
		string[] arr_memberinfo;
		if (Session["MemberInfo"] == null) {
			Page.ClientScript.RegisterStartupScript(this.GetType(), "alertscript", "<script type='text/javascript'>alert('로그인 해주세요'); location.href='../Default.aspx';</script>", false);
			Response.Redirect("~/Default.aspx");
		} else {
			arr_memberinfo = Session["MemberInfo"].ToString().Split(new string[] { "!" }, StringSplitOptions.None);
			CompanyPk = arr_memberinfo[1];
		}

		DB = new DBConn();
		TBList = LoadClearanceList(CompanyPk, "20140000", "20150000");
	}
	private string LoadClearanceList(string CompanyPk, string StartDate, string EndDate) {
		string Tableformat = @"
<table border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:850px; padding-top:20px; "">
	<thead>
		<tr height=""30px"">	
			<td class=""THead1"" >BLNo</td>
			<td class=""THead1"" >Shipper<br />Consignee</td>
			<td class=""THead1"" >CT</td>
			<td class=""THead1"" >관세</td>
			<td class=""THead1"" >부가세</td>
			<td class=""THead1"" >I</td>
			<td class=""THead1"" >P</td>
			<td class=""THead1"" >BL</td>
			<td class=""THead1"" >File</td>
		</tr>
	</thead>
	<tbody>{0}<tbody>
</table>";
		string Rowformat =
@"<tr>
	<td class=""TBody1"">{1}</td>
	<td class=""TBody1"" style=""text-align:left;"">{2}<br />{3}</td>
	<td class=""TBody1"">{4}&nbsp;{5}</td>
	<td class=""TBody1"">{6}</td>
	<td class=""TBody1"">{7}</td>
	<td class=""TBody1""><input type='button' value='I' onclick=""Print('I', '{0}');"" /></td>
	<td class=""TBody1""><input type='button' value='P' onclick=""Print('P', '{0}');"" /></td>
	<td class=""TBody1""><input type='button' value='B' onclick=""Print('B', '{0}');"" /></td>
	<td class=""TBody1"">{8}</td>
</tr>";
		DB.SqlCmd.CommandText = @"
SELECT 
 RF.RequestFormPk, RF.ShipperPk, RF.ConsigneePk, RF.ShipperCode, RF.ConsigneeCode
 , RF.DepartureDate, RF.ArrivalDate, RF.TransportWayCL, RF.DocumentStepCL
 , CD.CommercialDocumentHeadPk, CD.BLNo, CD.Shipper, CD.Consignee
 , SC.companyname AS ShipperCompanyName
 , Departure.NameE
 , RF.TotalPackedCount, RF.PackingUnit, RF.TotalGrossWeight, RF.TotalVolume
 , T1.Value as T1V, T2.Value as T2V, T3.Value as T3V 
 , ClearanceFile0.ClearancedFilePk AS File0 
 , ClearanceFile1.ClearancedFilePk AS File1 
 , ClearanceFile2.ClearancedFilePk AS File2 
 , ClearanceFile3.ClearancedFilePk AS File3 
 , ClearanceFile4.ClearancedFilePk AS File4 
 , ClearanceFile100.ClearancedFilePk AS File100 
 , ClearanceFile101.ClearancedFilePk AS File101 
 , ClearanceFile102.ClearancedFilePk AS File102 
 , ClearanceFile103.ClearancedFilePk AS File103 
FROM RequestForm AS RF 
 inner join CommerdialConnectionWithRequest AS CCWR  ON RF.RequestFormPk=CCWR.RequestFormPk 
 left join CommercialDocument AS CD ON CD.CommercialDocumentHeadPk=CCWR.CommercialDocumentPk
 Left join Company AS SC on RF.ShipperPk=SC.CompanyPk 
 Left join RegionCode AS Departure on RF.DepartureRegionCode=Departure.RegionCode 
 Left join [dbo].[REQUESTFORMCALCULATE_HEAD] AS RCH on RF.RequestFormPk=RCH.[TABLE_PK] 
left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] AS Value FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세' 
			) AS T1 ON T1.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK] 
left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] AS Value FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='부가세'
			) AS T2 ON T2.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK]
left join ( SELECT [REQUESTFORMCALCULATE_HEAD_PK], [ORIGINAL_PRICE] AS Value FROM [dbo].[REQUESTFORMCALCULATE_BODY] WHERE Title='관세사비'
			) AS T3 ON T3.[REQUESTFORMCALCULATE_HEAD_PK]=RCH.[REQUESTFORMCALCULATE_HEAD_PK]  
 left join ( SELECT [RequestFormPk], [PhysicalPath],[ClearancedFilePk] FROM [dbo].[ClearancedFile] WHERE GubunCL=0 ) AS ClearanceFile0 ON ClearanceFile0.RequestFormPk=RF.RequestFormPk 
 left join ( SELECT [RequestFormPk], [PhysicalPath],[ClearancedFilePk] FROM [dbo].[ClearancedFile] WHERE GubunCL=1 ) AS ClearanceFile1 ON ClearanceFile1.RequestFormPk=RF.RequestFormPk 
 left join ( SELECT [RequestFormPk], [PhysicalPath],[ClearancedFilePk] FROM [dbo].[ClearancedFile] WHERE GubunCL=2 ) AS ClearanceFile2 ON ClearanceFile2.RequestFormPk=RF.RequestFormPk 
 left join ( SELECT [RequestFormPk], [PhysicalPath],[ClearancedFilePk] FROM [dbo].[ClearancedFile] WHERE GubunCL=3 ) AS ClearanceFile3 ON ClearanceFile3.RequestFormPk=RF.RequestFormPk 
left join ( SELECT [RequestFormPk], [PhysicalPath],[ClearancedFilePk] FROM [dbo].[ClearancedFile] WHERE GubunCL=4 ) AS ClearanceFile4 ON ClearanceFile3.RequestFormPk=RF.RequestFormPk 
 left join ( SELECT [RequestFormPk], [PhysicalPath],[ClearancedFilePk] FROM [dbo].[ClearancedFile] WHERE GubunCL=100 ) AS ClearanceFile100 ON ClearanceFile100.RequestFormPk=RF.RequestFormPk 
 left join ( SELECT [RequestFormPk], [PhysicalPath],[ClearancedFilePk] FROM [dbo].[ClearancedFile] WHERE GubunCL=101 ) AS ClearanceFile101 ON ClearanceFile101.RequestFormPk=RF.RequestFormPk 
 left join ( SELECT [RequestFormPk], [PhysicalPath],[ClearancedFilePk] FROM [dbo].[ClearancedFile] WHERE GubunCL=102 ) AS ClearanceFile102 ON ClearanceFile102.RequestFormPk=RF.RequestFormPk 
 left join ( SELECT [RequestFormPk], [PhysicalPath],[ClearancedFilePk] FROM [dbo].[ClearancedFile] WHERE GubunCL=103 ) AS ClearanceFile103 ON ClearanceFile103.RequestFormPk=RF.RequestFormPk 
WHERE  RF.ConsigneePk=" + CompanyPk + " and RF.ArrivalDate>='" + StartDate + "'  and RF.ArrivalDate<='" + EndDate + @"'
AND ISNULL(RCH.[TABLE_NAME], 'RequestForm') = 'RequestForm'
       AND RF.stepcl > 63 
AND RF.documentstepcl not IN ( 4 ) 
ORDER  BY 
 RF.ArrivalDate ASC, 
 CD.CommercialDocumentHeadPk  ASC, 
    RF.RequestFormPk  ASC;";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		List<string[]> InnerData = new List<string[]>();
		string[] tempInner = null;
		StringBuilder ReturnValue = new StringBuilder();
		//List<string> tempItem = new List<string>();
		//string tempRequestFormPk = "";

		string tempCommercialDocumentHeadPk = "";
		List<string[]> Sum = new List<string[]>();

		while (RS.Read()) {
			if (RS["CommercialDocumentHeadPk"] + "" != tempCommercialDocumentHeadPk) {
				if (tempInner != null) {
					int cursor = -1;
					for (var i = 0; i < Sum.Count; i++) {
						if (Sum[i][0] == tempInner[3]) {
							cursor = i;
							break;
						}
					}
					if (cursor == -1) {
						Sum.Add(new string[] { tempInner[3], "1", tempInner[4] });
					} else {
						Sum[cursor][1] = ( Int32.Parse(Sum[cursor][1]) + 1 ).ToString();
						Sum[cursor][2] = ( decimal.Parse(Sum[cursor][2]) + decimal.Parse(tempInner[4]) ).ToString();
					}
					ReturnValue.Append(String.Format(Rowformat, tempInner));
				}
				tempCommercialDocumentHeadPk = RS["CommercialDocumentHeadPk"] + "";
				tempInner = new string[9];
				tempInner[0] = tempCommercialDocumentHeadPk;
				tempInner[1] = RS["BLNo"] + "";
				tempInner[2] = RS["Shipper"] + "";
				tempInner[3] = RS["Consignee"] + "";
				tempInner[4] = "0";
				tempInner[5] = Common.GetPackingUnit(RS["PackingUnit"] + "") + "";
				tempInner[6] = Common.NumberFormat(RS["T1V"] + "");
				tempInner[7] = Common.NumberFormat(RS["T2V"] + "");
				tempInner[8] = "";
			}

			tempInner[4] = ( Int32.Parse(RS["TotalPackedCount"] + "") + Int32.Parse(tempInner[4]) ).ToString();
			if (RS["File0"] + "" != "") {
				if (tempInner[8] != "") {
					tempInner[8] += "<br />";
				}
				tempInner[8] += "<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["File0"].ToString() + "&T=ClearancedFile' >모음</a>";
			}
			if (RS["File1"] + "" != "") {
				if (tempInner[8] != "") {
					tempInner[8] += "<br />";
				}
				tempInner[8] += "<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["File1"].ToString() + "&T=ClearancedFile' >수입신고필증</a>";
			}
			if (RS["File2"] + "" != "") {
				if (tempInner[8] != "") {
					tempInner[8] += "<br />";
				}
				tempInner[8] += "<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["File2"].ToString() + "&T=ClearancedFile' >관부가세납부영수증</a>";
			}
			if (RS["File3"] + "" != "") {
				if (tempInner[8] != "") {
					tempInner[8] += "<br />";
				}
				tempInner[8] += "<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["File3"].ToString() + "&T=ClearancedFile' >관세사비세금계산서</a>";
			}
			if (RS["File4"] + "" != "") {
				if (tempInner[8] != "") {
					tempInner[8] += "<br />";
				}
				tempInner[8] += "<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["File4"].ToString() + "&T=ClearancedFile' >수입세금계산서</a>";
			}
			if (RS["File100"] + "" != "") {
				if (tempInner[8] != "") {
					tempInner[8] += "<br />";
				}
				tempInner[8] += "<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["File100"].ToString() + "&T=ClearancedFile' >모음</a>";
			}
			if (RS["File101"] + "" != "") {
				if (tempInner[8] != "") {
					tempInner[8] += "<br />";
				}
				tempInner[8] += "<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["File101"].ToString() + "&T=ClearancedFile' >수입신고필증</a>";
			}
			if (RS["File102"] + "" != "") {
				if (tempInner[8] != "") {
					tempInner[8] += "<br />";
				}
				tempInner[8] += "<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["File102"].ToString() + "&T=ClearancedFile' >관부가세납부영수증</a>";
			}
			if (RS["File103"] + "" != "") {
				if (tempInner[8] != "") {
					tempInner[8] += "<br />";
				}
				tempInner[8] += "<a href='../UploadedFiles/FileDownload.aspx?S=" + RS["File103"].ToString() + "&T=ClearancedFile' >관세사비세금계산서</a>";
			}
		}
		if (tempInner != null) {
			int cursor = -1;
			for (var i = 0; i < Sum.Count; i++) {
				if (Sum[i][0] == tempInner[3]) {
					cursor = i;
					break;
				}
			}
			if (cursor == -1) {
				Sum.Add(new string[] { tempInner[3], "1", tempInner[4] });
			} else {
				Sum[cursor][1] = ( Int32.Parse(Sum[cursor][1]) + 1 ).ToString();
				Sum[cursor][2] = ( decimal.Parse(Sum[cursor][2]) + decimal.Parse(tempInner[4]) ).ToString();
			}
			ReturnValue.Append(String.Format(Rowformat, tempInner));
		}

		StringBuilder HtmlFooter = new StringBuilder();
		foreach (string[] row in Sum) {
			HtmlFooter.Append(@"
				<tr height=""30px"">
					<td class=""THead1"" colspan='8' style=""border-bottom:0px;  width:160px; font-weight:bold; text-align:right; padding-right:20px; "">" + row[0] + @"</td>
					<td class=""THead1"" style='border-bottom:0px; text-align:right; padding-right:10px; ' >" + row[1] + @"건</td>
				</tr>");
		}

		ReturnValue.Append("	<tfoot>" + HtmlFooter + "</tfoot>");

		return string.Format(Tableformat, ReturnValue);

	}
}