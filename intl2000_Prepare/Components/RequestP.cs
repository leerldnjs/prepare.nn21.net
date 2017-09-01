using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Components
{
	public class RequestP : DBConn
	{
		StringBuilder Query;
		public String IntroRecentRequestList(string pk) {
			Query = new StringBuilder();
			SqlCmd.CommandText = "SELECT" +
										" R.RequestFormPk, R.DepartureDate, R.ArrivalDate, R.TransportWayCL, R.StepCL, " +
										" R.RequestDate, RC.Name as DepartureRC, RCC.Name as ArrivalRC, C.CompanyName, C.CompanyTEL, " +
										" R.ShipperPk, R.ShipperCode ,TBBHead.Description" +
									" FROM RequestForm as R " +
										" left join RegionCode as RC on R.DepartureRegionCode=RC.RegionCode " +
										" left join RegionCode as RCC on R.ArrivalRegionCode=RCC.RegionCode " +
										" left join Company as C on R.ConsigneePk=C.CompanyPk " +
										" left join [Account_] AS A ON R.[AccountID]=A.[AccountID] " +
										" left join (select RequestFormpk,TransportBetweenBranchPk from TransportBBHistory " +
													" union all " +
													" select [REQUEST_PK],[TRANSPORT_HEAD_PK] from [dbo].[STORAGE]) as TBBPk on R.RequestFormPk=TBBPk.RequestFormpk " +
										" left join (select * from [TransportBBHead] where ToBranchPk=3157) AS TBBHead ON TBBPk.[TransportBetweenBranchPk]=TBBHead.[TransportBetweenBranchPk] " +
									" WHERE ( StepCL>49 and R.ShipperPk=" + pk + "  ) or (( StepCL>52 and R.ConsigneePk=" + pk + " )  OR (A.CompanyPk=" + pk + ") ) " +
									"ORDER BY R.RequestFormPk desc,TBBPk.TransportBetweenBranchPk desc";

			//Shipper가 예약한것은 Consignee가 보지못하게변경 SP_RequestListByEachCompanyOver77로 프로시저 수정 (12.05.04)
			DBCon.Open();
			SqlDataReader returnData = SqlCmd.ExecuteReader();
			string TempRequestPk = "";
			string[] Description;
			string TempArrivalPort = "";
			decimal Count = 0;
			while (returnData.Read()) {
				if (TempRequestPk != returnData["RequestFormPk"].ToString()) {
					TempRequestPk = returnData["RequestFormPk"].ToString();
					Description = (returnData["Description"] + "").Split(Common.Splite321, StringSplitOptions.None);
					if (Description[0].ToString() != "") {
						if (Description[2].Length > 6) {
							if (Description[2].ToString().Trim().Substring(0, 6) == "PYONGT" || Description[2].ToString().Trim().Substring(0, 4) == "INCH") {
								TempArrivalPort = Description[2].ToString();
							} else {
								TempArrivalPort = "";
							}
						} else {
							TempArrivalPort = "";
						}
					}
					Count++;
					Query.Append(returnData["RequestFormPk"] + "@@" +
						(returnData["DepartureDate"] + "" == "" ? "" : returnData["DepartureDate"].ToString().Substring(2, 6)) + "@@" +
						returnData["DepartureRC"] + "@@" +
						(returnData["ArrivalDate"] + "" == "" ? "" : returnData["ArrivalDate"].ToString().Substring(2, 6)) + "@@" + returnData["ArrivalRC"] + "@@" + returnData["TransportWayCL"] + "@@" + returnData["CompanyName"] + "@@" + returnData["CompanyTEL"] + "@@" + returnData["RequestDate"].ToString().Substring(2, 8) + "@@" + returnData["StepCL"] + "@@" + returnData["ShipperPk"] + "@@" + returnData["ShipperCode"] + "@@" + TempArrivalPort + "####");
				}
			}
			returnData.Dispose();
			DBCon.Close();
			return Query + "";
		}       //개인회원 처음화면에 리스트 뽑는거
		public String ItemModifyList(string pk, string cl) {
			SqlCmd.CommandText = "SELECT [CODE], [ACCOUNT_ID], [DESCRIPTION], [REGISTERD] FROM [dbo].[HISTORY] WHERE [TABLE_NAME] = 'RequestForm' AND [TABLE_PK]=" + pk + " and ([CODE]='70' or [CODE]='71') Order By [REGISTERD] Desc";
			Query = new StringBuilder();
			DBCon.Open();
			SqlDataReader RS = SqlCmd.ExecuteReader();
			while (RS.Read()) {
				if (RS[0] + "" == "71" && cl == "70") { Query.Append("<tr><td class=\"ItemTableIn\" align='center' ><span style=\"color:gray;\">Admin</span></td>"); } else { Query.Append("<tr><td class=\"ItemTableIn\" align='center' ><span style=\"color:Blue;\"> " + RS[1] + "</span></td>"); }
				Query.Append("<td class=\"ItemTableIn\" >" + RS[2].ToString().Substring(1, RS[2].ToString().Length - 1).Replace("*", "<br />") + "</td>");
				Query.Append("<td class=\"ItemTableIn\" align='center' >" + RS[3].ToString().Substring(0, RS[3].ToString().Length - 3) + "</td></tr>");
			}
			RS.Dispose();
			DBCon.Close();
			return Query.ToString();
		}
		public String CompanyNameInDocumentList(string WriterPk) {
			SqlCmd.CommandText = "SELECT CompanyInDocumentPk, Name, Address FROM CompanyInDocument WHERE GubunPk=" + WriterPk + " and GubunCL=0";
			Query = new StringBuilder();
			DBCon.Open();
			SqlDataReader RS = SqlCmd.ExecuteReader();
			while (RS.Read()) { Query.Append(RS[0] + "!!" + RS[1] + "@@" + RS[2] + "####"); }
			RS.Dispose();
			DBCon.Close();
			return Query.ToString();
		}
		public String HTMLClearanceItemHistory(string ShipperPk, string ConsigneePk, int DateCount) {
			if (ShipperPk == "" || ConsigneePk == "") {
				return "";
			}
			SqlCmd.CommandText = @"
SELECT TOP 30 CICK.[Description], CICK.[Material], R.[RequestFormPk], R.[ShipperCode], R.[ConsigneeCode], R.[DepartureDate], R.[ArrivalDate], R.[TransportWayCL] 
FROM [RequestForm] AS R 
	inner join [CommerdialConnectionWithRequest] AS CCWR ON R.RequestFormPk=CCWR.RequestFormPk 
	left join [RequestFormItems] AS RFI ON R.RequestFormPk=RFI.RequestFormPk 
	left join [ClearanceItemCodeKOR] AS CICK ON RFI.ItemCode=CICK.[ItemCode] 
WHERE ShipperPk=" + ShipperPk + " and ConsigneePk=" + ConsigneePk + @" and isnull(CICK.[Description], '')<>'' 
ORDER BY DepartureDate DESC, RequestFormPk ASC, Description ASC, Material ASC ;";
			StringBuilder HTML = new StringBuilder();
			StringBuilder InnerTemp = new StringBuilder();

			string TempRequestFormPk = "";
			string TempDescription = "";
			string TempMaterial = "";

			DBCon.Open();
			SqlDataReader RS = SqlCmd.ExecuteReader();

			while (RS.Read()) {
				if (DateCount < 1) {
					break;
				}
				if (TempRequestFormPk != RS["RequestFormPk"] + "") {
					if (InnerTemp + "" != "") {
						HTML.Append(InnerTemp + "</li></ul></ul>");
						InnerTemp = new StringBuilder();
						DateCount--;
					}
					InnerTemp.Append("<ul style=\"list-style-type:none;\"><li><strong>" + RS["DepartureDate"] + " ~ " + RS["ArrivalDate"] + "</strong></li><ul style=\"list-style-type:disc;\"><li>" + RS["Description"] + " <em>" + RS["Material"] + "</em></li>");
					TempDescription = RS["Description"] + "";
					TempMaterial = RS["Material"] + "";
					TempRequestFormPk = RS["RequestFormPk"] + "";
				} else {
					if (TempDescription != RS["Description"] + "" || TempMaterial != RS["Material"] + "") {
						InnerTemp.Append("<li>" + RS["Description"] + " <em>" + RS["Material"] + "</em></li>");
						TempDescription = RS["Description"] + "";
						TempMaterial = RS["Material"] + "";
					}
				}
			}
			RS.Dispose();
			DBCon.Close();
			return HTML + "";
		}
	}
	
	public struct sRequestForm
	{
		public string RequestFormPk;
		public string ShipperPk;
		public string ConsigneePk;
		public string AccountID;
		public string ConsigneeCCLPk;
		public string ShipperCode;
		public string CompanyInDocumentPk;
		public string ConsigneeCode;
		public string ShipperClearanceNamePk;
		public string ConsigneeClearanceNamePk;
		public string DepartureDat; 
		public string ArrivalDate;
		public string DepartureRegionCode;
		public string ArrivalRegionCode;
		public string DepartureBranchPk;
		public string ArrivalBranchPk;
		public string DepartureAreaBranchCode;
		public string ShipperStaffName;
		public string ShipperStaffTEL;
		public string ShipperStaffMobile;
		public string TransportWayCL;
		public string JubsuWayCL;
		public string PaymentWayCL;
		public string PaymentWhoCL;
		public string DocumentRequestCL;
		public string MonetaryUnitCL;
		public string StepCL;
		public string DocumentStepCL;
		public string PickupRequestDate;
		public string NotifyPartyName;
		public string NotifyPartyAddress;
		public string Memo;
		public string RequestDate;
		public string StockedDate;
		public string ShipperSignID;
		public string ShipperSignDate;
		public string ConsigneeSignID;
		public string ConsigneeSignDate;
		public string ShipmentDate;
		public string GubunCL;
		public string BranchPk_Own;
		public string SettlementBranchPk;
		public string ItemSummary;
	}
}