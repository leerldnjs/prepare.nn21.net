using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Calculate_WithCustomer : System.Web.UI.Page
{
	protected String Summary;
	private DBConn DB;
	protected String TBList;
	private String CompanyPk;
	protected void Page_Load(object sender, EventArgs e)
	{
		string[] arr_memberinfo;
		if (Session["MemberInfo"] == null) {
			Page.ClientScript.RegisterStartupScript(this.GetType(), "alertscript", "<script type='text/javascript'>alert('로그인 해주세요'); location.href='../Default.aspx';</script>", false);
			Response.Redirect("~/Default.aspx");
		} else {
			arr_memberinfo = Session["MemberInfo"].ToString().Split(new string[] { "!" }, StringSplitOptions.None);
			CompanyPk = arr_memberinfo[1];
		}

		DB = new DBConn();
		TBList = LoadClearanceListASCompanyCode(CompanyPk, "20140000", "20150000");
	}

	private String LoadClearanceListASCompanyCode(string CompanyPk, string StartDate, string EndDate)
	{
		DB.SqlCmd.CommandText = @"
SELECT * FROM (
SELECT 
	'Out' AS InOrOut, 
	R.[RequestFormPk], R.[DepartureDate], R.[ArrivalDate]
    , R.[TransportWayCL]
    , R.[MonetaryUnitCL]
    , R.[DocumentStepCL]
	, Total.Amount, Total.PackedCount, Total.Volume, Total.GrossWeight 
	, TargetCompany.CompanyPk
	, TargetCompany.CompanyCode, TargetCompany.CompanyName 
	, TargetRegion.Name 
	, RFI.[Description] 
	   , R.PackingUnit
  FROM [dbo].[RequestForm] AS R 
	left join (
		SELECT RFI.RequestFormPk, SUM(RFI.Amount) AS Amount, SUM(RF.TotalPackedCount) as PackedCount, SUM(RF.TotalGrossWeight) AS GrossWeight, SUM(RF.TotalVolume ) AS Volume
		FROM RequestFormItems AS RFI	
			left join [dbo].[RequestForm] AS RF ON RF.RequestFormPk=RFI.RequestFormPk 
		WHERE ISNULL(RFI.Amount, 0)>0
		Group By RFI.RequestFormPk
	) AS Total ON Total.RequestFormPk= R.RequestFormPk 
	left join [dbo].[Company] AS TargetCompany ON TargetCompany.CompanyPk=R.ConsigneePk 
	left join [dbo].[RegionCode] AS TargetRegion ON R.ArrivalRegionCode=TargetRegion.RegionCode 
	left join [dbo].[RequestFormItems] AS RFI ON R.RequestFormPk=RFI.RequestFormPk 
  WHERE R.ShipperPk=" + CompanyPk + " and R.ArrivalDate>='" + StartDate + "' and R.ArrivalDate<='" + EndDate + @"'  
 UNION 
SELECT 
	'In' AS InOrOut, 
	R.[RequestFormPk], R.[DepartureDate], R.[ArrivalDate]
    , R.[TransportWayCL]
    , R.[MonetaryUnitCL]
    , R.[DocumentStepCL]
	, Total.Amount, Total.PackedCount, Total.Volume, Total.GrossWeight 
	, TargetCompany.CompanyPk
	, TargetCompany.CompanyCode, TargetCompany.CompanyName 
	, TargetRegion.Name 
	, RFI.[Description] 
	   , R.PackingUnit
  FROM [dbo].[RequestForm] AS R 
	left join (
		SELECT RFI.RequestFormPk, SUM(RFI.Amount) AS Amount, SUM(RF.TotalPackedCount) as PackedCount, SUM(RF.TotalGrossWeight) AS GrossWeight, SUM(RF.TotalVolume ) AS Volume
		FROM RequestFormItems AS RFI	
			left join [dbo].[RequestForm] AS RF ON RF.RequestFormPk=RFI.RequestFormPk 
		WHERE ISNULL(RFI.Amount, 0)>0
		Group By RFI.RequestFormPk
	) AS Total ON Total.RequestFormPk= R.RequestFormPk 
	left join [dbo].[Company] AS TargetCompany ON TargetCompany.CompanyPk=R.ShipperPk 
	left join [dbo].[RegionCode] AS TargetRegion ON R.DepartureRegionCode=TargetRegion.RegionCode 
	left join [dbo].[RequestFormItems] AS RFI ON R.RequestFormPk=RFI.RequestFormPk 
  WHERE R.ConsigneePk=" + CompanyPk + " and R.ArrivalDate>='" + StartDate + "' and R.ArrivalDate<='" + EndDate + @"'  
  ) AS TT 
  ORDER BY TT.CompanyPk ASC,  TT.ArrivalDate ASC ; ";
		//Response.Write(DB.SqlCmd.CommandText);
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		List<string[]> InnerData = new List<string[]>();
		string[] tempInner = null;
		List<string> tempItem = new List<string>();
		string tempRequestFormPk = "";
		while (RS.Read()) {
			if (RS["RequestFormPk"] + "" != tempRequestFormPk) {
				tempRequestFormPk = RS["RequestFormPk"] + "";
				if (tempInner != null) {
					StringBuilder sumDescription = new StringBuilder();
					foreach (string eachDescription in tempItem) {
						sumDescription.Append(", " + eachDescription);
					}
					tempItem = new List<string>();
					tempInner[16] = sumDescription.ToString().Substring(2);
					InnerData.Add(tempInner);
				}
				tempInner = new string[17];
				tempInner[0] = RS["InOrOut"] + "";
				tempInner[1] = RS["RequestFormPk"] + "";
				tempInner[2] = RS["DepartureDate"] + "";
				tempInner[3] = RS["ArrivalDate"] + "";
				tempInner[4] = RS["TransportWayCL"] + "";
				tempInner[5] = RS["MonetaryUnitCL"] + "";
				tempInner[6] = RS["DocumentStepCL"] + "";
				tempInner[7] = RS["Amount"] + "";
				tempInner[8] = RS["PackedCount"] + "";
				tempInner[9] = RS["Volume"] + "";
				tempInner[10] = RS["GrossWeight"] + "";
				tempInner[11] = RS["CompanyCode"] + "";
				tempInner[12] = RS["CompanyName"] + "";
				tempInner[13] = RS["Name"] + "";
				tempInner[14] = RS["PackingUnit"] + "";
				tempInner[15] = RS["MonetaryUnitCL"] + "";
			}

			bool isIn = false;
			foreach (string eachDescription in tempItem) {
				if (eachDescription == RS["Description"] + "") {
					isIn = true;
					break;
				}
			}
			if (!isIn) {
				tempItem.Add(RS["Description"] + "");
			}
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (tempInner != null) {
			StringBuilder sumDescription = new StringBuilder();
			foreach (string eachDescription in tempItem) {
				sumDescription.Append(", " + eachDescription);
			}
			tempInner[14] = sumDescription.ToString().Substring(2);
			InnerData.Add(tempInner);
		}
		StringBuilder ReturnValue = new StringBuilder();
		string TempCompanyCode = "";
		int TempCountIn = 0;
		int TempCountOut = 0;
		string tempFooter = "";

		List<string[]> TempTotalAmount = new List<string[]>();
		foreach (string[] row in InnerData) {
			if (TempCompanyCode != row[11]) {
				if (ReturnValue.ToString() != "") {
					tempFooter = "";
					if (TempCountIn != 0) {
						string tempString = "";
						for (var i = 0; i < TempTotalAmount.Count; i++) {
							if (tempString != "") {
								tempString += "<br />";
							}
							tempString += Common.GetMonetaryUnit(TempTotalAmount[i][0]) + " " + Common.NumberFormat(TempTotalAmount[i][1]);
						}
						tempFooter += @"	<tfoot>
																		<tr height=""30px"">
																			<td class=""THead1"" colspan='5' style=""border-bottom:0px;  width:160px; font-weight:bold; text-align:right; padding-right:20px; "">수입 " + TempCountIn + @"건</td>
																			<td class=""THead1"" colspan='2' style='border-bottom:0px; text-align:right; padding-right:10px; ' >" + tempString + @"</td>
																		</tr>
																	</tfoot>";
					}
					if (TempCountOut != 0) {
						string tempString = "";
						for (var i = 0; i < TempTotalAmount.Count; i++) {
							if (tempString != "") {
								tempString += "<br />";
							}
							tempString += Common.GetMonetaryUnit(TempTotalAmount[i][0]) + " " + Common.NumberFormat(TempTotalAmount[i][2]);
						}
						tempFooter += @"	<tfoot>
																		<tr height=""30px"">
																			<td class=""THead1"" colspan='5' style=""border-bottom:0px;  width:160px; font-weight:bold; text-align:right; padding-right:20px; "">수입 " + TempCountIn + @"건</td>
																			<td class=""THead1"" colspan='2' style='border-bottom:0px; text-align:right; padding-right:10px; ' >" + tempString + @"</td>
																		</tr>
																	</tfoot>";
					}
					ReturnValue.Append("</tbody>" + tempFooter + "</table>");
					TempCountIn = 0;
					TempCountOut = 0;
					TempTotalAmount = new List<string[]>();
				}
				ReturnValue.Append(@"
					<table border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width:850px; padding-top:20px; "">
						<thead>
							<tr height=""30px"">	
								<td class=""THead1"" colspan='3' style=""width:160px; font-weight:bold; text-align:left; padding-left:20px; "">" + row[11] + " " + row[12] + @"</td>
								<!--td class=""THead1"" style=""width:130px;"">&nbsp;</td>
								<td class=""THead1"" >Box</td-->
								<td class=""THead1"" style=""width:60px;"">Box</td>
								<td class=""THead1"" style=""width:60px;"">CBM</td>
								<td class=""THead1"" style=""width:60px;"">Kg</td>
								<td class=""THead1"" style=""width:85px;"">금액</td>
							</tr>
						</thead>
						<tbody>");
				TempCompanyCode = row[11];
			}

			int tempCursor = -1;
			for (var i = 0; i < TempTotalAmount.Count; i++) {
				if (TempTotalAmount[i][0] == row[15]) {
					tempCursor = i;
					break;
				}
			}
			if (tempCursor == -1) {
				if (row[0] == "In") {
					TempTotalAmount.Add(new string[] { row[15], row[7], "0" });
					TempCountIn++;
				} else {
					TempTotalAmount.Add(new string[] { row[15], "0", row[7] });
					TempCountOut++;
				}
			} else {
				if (row[0] == "In") {
					TempTotalAmount[tempCursor][1] = (decimal.Parse((TempTotalAmount[tempCursor][1] + "" == "" ? "0" : TempTotalAmount[tempCursor][1] + "")) + (row[7] + "" == "" ? 0 : decimal.Parse(row[7]))).ToString();
					TempCountIn++;
				} else {
					TempTotalAmount[tempCursor][2] = (decimal.Parse((TempTotalAmount[tempCursor][1] + "" == "" ? "0" : TempTotalAmount[tempCursor][1] + "")) + (row[7] + "" == "" ? 0 : decimal.Parse(row[7]))).ToString();
					TempCountOut++;
				}
			}





			ReturnValue.Append(@"
					<tr style=""height:28px; "">
						<td class=""TBody1"" style='width:160px; '><a href=""/Request/RequestFormView.aspx?pk=" + row[1] + @""">" + row[2] + " ~ " + row[3] + @"</a></td>
						<td class=""TBody1"" style=""text-align: left; width:85px; padding-left:10px;  "">" +
																																		(row[0] == "In" ? @"<span style=""color: blue;"">From</span> " : @"<span style=""color: red;"">To</span> ") +
																																		row[13] + @"</td>
						<td class=""TBody1"" style='text-align:left; padding-left:10px; '><a href=""/Request/RequestFormView.aspx?pk=" + row[1] + @""">" + row[16] + @"</a></td>
						<td class=""TBody1"" style='text-align:right; ' >" + row[8] + " " + Common.GetPackingUnit(row[14]) + @"</td>
						<td class=""TBody1"" style='text-align:right; ' >" + Common.NumberFormat(row[9]) + @"</td>
						<td class=""TBody1"" style='text-align:right;' >" + Common.NumberFormat(row[10]) + @"Kg</td>
						<td class=""TBody1"" style='text-align:right;' >" + Common.GetMonetaryUnit(row[15]) + @" " + Common.NumberFormat(row[7]) + @"</td>
					</tr>");
		}

		if (ReturnValue.ToString() != "") {
			tempFooter = "";
			if (TempCountIn != 0) {
				string tempString = "";
				for (var i = 0; i < TempTotalAmount.Count; i++) {
					if (tempString != "") {
						tempString += "<br />";
					}
					tempString += Common.GetMonetaryUnit(TempTotalAmount[i][0]) + " " + Common.NumberFormat(TempTotalAmount[i][1]);
				}
				tempFooter += @"	<tfoot>
																		<tr height=""30px"">
																			<td class=""THead1"" colspan='5' style=""border-bottom:0px;  width:160px; font-weight:bold; text-align:right; padding-right:20px; "">수입 " + TempCountIn + @"건</td>
																			<td class=""THead1"" colspan='2' style='border-bottom:0px; text-align:right; padding-right:10px; ' >" + tempString + @"</td>
																		</tr>
																	</tfoot>";
			}
			if (TempCountOut != 0) {
				string tempString = "";
				for (var i = 0; i < TempTotalAmount.Count; i++) {
					if (tempString != "") {
						tempString += "<br />";
					}
					tempString += Common.GetMonetaryUnit(TempTotalAmount[i][0]) + " " + Common.NumberFormat(TempTotalAmount[i][2]);
				}
				tempFooter += @"	<tfoot>
																		<tr height=""30px"">
																			<td class=""THead1"" colspan='5' style=""border-bottom:0px;  width:160px; font-weight:bold; text-align:right; padding-right:20px; "">수출 " + TempCountOut + @"건</td>
																			<td class=""THead1"" colspan='2' style='border-bottom:0px; text-align:right; padding-right:10px; ' >" + tempString + @"</td>
																		</tr>
																	</tfoot>";
			}

			ReturnValue.Append("</tbody>" + tempFooter + "</table>");
		}

		return ReturnValue.ToString();
	}
}