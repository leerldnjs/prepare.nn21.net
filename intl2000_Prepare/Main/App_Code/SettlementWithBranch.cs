using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// SettlementWithBranch의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class SettlementWithBranch : System.Web.Services.WebService
{

	public SettlementWithBranch() {

		//디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
		//InitializeComponent(); 
	}

	[WebMethod]
	public string SetDelete_SettlementContainer(string TBBHPk) {
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"
			UPDATE [INTL2010].[dbo].[TransportBBHead] 
			SET SettlementStatus=1 
			WHERE TransportBetweenBranchPk=" + TBBHPk + @"; 

			DELETE FROM[INTL2010].[dbo].[Document]
			WHERE TypePk = " + TBBHPk + @" and Type = 'SettlementWithBranch';

			DELETE FROM[INTL2010].[dbo].[DocumentBody]
			WHERE DocumentBodyPk in (
				SELECT DB.DocumentBodyPk 
				FROM[INTL2010].[dbo].[DocumentBody] AS DB 
					left join Document AS D ON DB.DocumentPk= D.DocumentPk 
				WHERE D.DocumentPk is null
			 );";

		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	[WebMethod]
	public string SetDocumentBody_BranchPk(string DocumentBodyPk, string BranchPk) {
		string query = @"
			UPDATE [dbo].[DocumentBody] 
			SET [ValueInt0] = " + BranchPk + @" 
			WHERE [DocumentBodyPk]= " + DocumentBodyPk;
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = query;
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string SetSave_SettlementContainer(string FormData) {
		Dictionary<string, string> RS = new Utility().ConvertGetParam(FormData);

		SettlementContainer ReturnValue = new SettlementContainer();

		ReturnValue.DocumentPk = RS["DocumentPk"];
		ReturnValue.TBBHPk = RS["TBBHPk"] + "";
		ReturnValue.Status = RS["Status"] + "";
		ReturnValue.ContainerNo = RS["ContainerNo"] + "";
		ReturnValue.BLNo = RS["BLNo"] + "";
		ReturnValue.FromRegion = RS["FromRegion"] + "";
		ReturnValue.ToRegion = RS["ToRegion"] + "";
		ReturnValue.FromDate = RS["FromDate"] + "";
		ReturnValue.ToDate = RS["ToDate"] + "";
		ReturnValue.ExchangeRateDate = RS["ExchangeRateDate"] + "";
		ReturnValue.LastWarehouse = RS["LastWarehouse"] + "";
		ReturnValue.MonetaryYW = RS["MonetaryYW"] + "";
		ReturnValue.BranchPk_Own = RS["BranchPk_Own"] + "";

		ReturnValue.ArrEach = new List<SettlementEach>();
		SettlementEach TempSettlementEach;

		if (RS.ContainsKey("ArrEach_DocumentBodyPk[]")) {
			TempSettlementEach = new SettlementEach();
			TempSettlementEach.DocumentPk = RS["ArrEach_DocumentBodyPk[]"];
			TempSettlementEach.Description = RS["ArrEach_Description[]"] + "";
			TempSettlementEach.MonetaryUnitCL = RS["ArrEach_MonetaryUnitCL[]"] + "";
			TempSettlementEach.MonetaryUnit = RS["ArrEach_MonetaryUnit[]"] + "";
			TempSettlementEach.Price = decimal.Parse(RS["ArrEach_Price[]"] + "");
			TempSettlementEach.Exchanged_MonetaryUnitCL = RS["ArrEach_Exchanged_MonetaryUnitCL[]"] + "";
			TempSettlementEach.Exchanged_MonetaryUnit = RS["ArrEach_ExchangedMonetaryUnit[]"] + "";
			TempSettlementEach.Type = RS["ArrEach_Type[]"] + "";
			TempSettlementEach.TypePk = RS["ArrEach_TypePk[]"] + "";
			TempSettlementEach.BranchPk_Own = RS["ArrEach_BranchPk_Own[]"] + "";
			TempSettlementEach.Exchanged_Price = decimal.Parse(RS["ArrEach_ExchangedPrice[]"] + "");
			ReturnValue.ArrEach.Add(TempSettlementEach);

			var i = 0;
			while (RS.ContainsKey("ArrEach_Description[]" + i)) {
				TempSettlementEach = new SettlementEach();
				TempSettlementEach.DocumentPk = RS["ArrEach_DocumentBodyPk[]" + i];
				TempSettlementEach.Description = RS["ArrEach_Description[]" + i] + "";
				TempSettlementEach.MonetaryUnitCL = RS["ArrEach_MonetaryUnitCL[]" + i] + "";
				TempSettlementEach.MonetaryUnit = RS["ArrEach_MonetaryUnit[]" + i] + "";
				TempSettlementEach.Price = decimal.Parse(RS["ArrEach_Price[]" + i] + "");
				TempSettlementEach.Exchanged_MonetaryUnitCL = RS["ArrEach_Exchanged_MonetaryUnitCL[]" + i] + "";
				TempSettlementEach.Exchanged_MonetaryUnit = RS["ArrEach_ExchangedMonetaryUnit[]" + i] + "";
				TempSettlementEach.Type = RS["ArrEach_Type[]" + i] + "";
				TempSettlementEach.TypePk = RS["ArrEach_TypePk[]" + i] + "";
				TempSettlementEach.BranchPk_Own = RS["ArrEach_BranchPk_Own[]" + i] + "";
				TempSettlementEach.Exchanged_Price = decimal.Parse(RS["ArrEach_ExchangedPrice[]" + i] + "");
				ReturnValue.ArrEach.Add(TempSettlementEach);

				i++;
			}
		}

		List<SettlementRequest> Temp = new List<SettlementRequest>();
		SettlementRequest TempSettlementRequest;
		if (RS.ContainsKey("ArrRequestEach_DocumentBodyPk[]")) {
			TempSettlementRequest = new SettlementRequest();
			TempSettlementRequest.DocumentPk = ReturnValue.DocumentPk;
			TempSettlementRequest.FromDate = RS["ArrRequestEach_FromDate[]"];
			TempSettlementRequest.ToDate = RS["ArrRequestEach_ToDate[]"];
			TempSettlementRequest.FromRegion = RS["ArrRequestEach_FromRegion[]"];
			TempSettlementRequest.ToRegion = RS["ArrRequestEach_ToRegion[]"];
			TempSettlementRequest.TransportDescription = RS["ArrRequestEach_TransportDescription[]"];
			TempSettlementRequest.ConsigneeCode = RS["ArrRequestEach_ConsigneeCode[]"];
			TempSettlementRequest.CT = RS["ArrRequestEach_CT[]"];
			TempSettlementRequest.PackingUnit = RS["ArrRequestEach_PackingUnit[]"];
			TempSettlementRequest.Kg = RS["ArrRequestEach_Kg[]"];
			TempSettlementRequest.CBM = RS["ArrRequestEach_CBM[]"];
			TempSettlementRequest.RequestPk = RS["ArrRequestEach_RequestPk[]"];
			TempSettlementRequest.BranchPk_Own = RS["ArrRequest_BranchPk_Own[]"];

			TempSettlementRequest.ArrEach = new List<SettlementEach>();
			TempSettlementEach = new SettlementEach();
			TempSettlementEach.DocumentPk = RS["ArrRequestEach_DocumentBodyPk[]"];
			TempSettlementEach.Description = RS["ArrRequestEach_Description[]"] + "";
			TempSettlementEach.MonetaryUnitCL = RS["ArrRequestEach_MonetaryUnitCL[]"] + "";
			TempSettlementEach.MonetaryUnit = RS["ArrRequestEach_MonetaryUnit[]"] + "";
			TempSettlementEach.Price = decimal.Parse(RS["ArrRequestEach_Price[]"] + "");
			TempSettlementEach.Exchanged_MonetaryUnitCL = RS["ArrRequestEach_Exchanged_MonetaryUnitCL[]"] + "";
			TempSettlementEach.Exchanged_MonetaryUnit = RS["ArrRequestEach_ExchangedMonetaryUnit[]"] + "";
			TempSettlementEach.Type = RS["ArrRequestEach_Type[]"] + "";
			TempSettlementEach.TypePk = RS["ArrRequestEach_TypePk[]"] + "";
			TempSettlementEach.BranchPk_Own = RS["ArrRequestEach_BranchPk_Own[]"] + "";
			TempSettlementEach.Exchanged_Price = decimal.Parse(RS["ArrRequestEach_ExchangedPrice[]"] + "");

			TempSettlementRequest.ArrEach.Add(TempSettlementEach);
			Temp.Add(TempSettlementRequest);

			var i = 0;
			while (RS.ContainsKey("ArrRequestEach_Description[]" + i)) {
				TempSettlementRequest = new SettlementRequest();
				TempSettlementRequest.DocumentPk = ReturnValue.DocumentPk;
				TempSettlementRequest.FromDate = RS["ArrRequestEach_FromDate[]" + i];
				TempSettlementRequest.ToDate = RS["ArrRequestEach_ToDate[]" + i];
				TempSettlementRequest.FromRegion = RS["ArrRequestEach_FromRegion[]" + i];
				TempSettlementRequest.ToRegion = RS["ArrRequestEach_ToRegion[]" + i];
				TempSettlementRequest.TransportDescription = RS["ArrRequestEach_TransportDescription[]" + i];
				TempSettlementRequest.ConsigneeCode = RS["ArrRequestEach_ConsigneeCode[]" + i];
				TempSettlementRequest.CT = RS["ArrRequestEach_CT[]" + i];
				TempSettlementRequest.PackingUnit = RS["ArrRequestEach_PackingUnit[]" + i];
				TempSettlementRequest.Kg = RS["ArrRequestEach_Kg[]" + i];
				TempSettlementRequest.CBM = RS["ArrRequestEach_CBM[]" + i];
				TempSettlementRequest.RequestPk = RS["ArrRequestEach_RequestPk[]" + i];
				TempSettlementRequest.BranchPk_Own = RS["ArrRequestEach_BranchPk_Own[]" + i];

				TempSettlementRequest.ArrEach = new List<SettlementEach>();
				TempSettlementEach = new SettlementEach();
				TempSettlementEach.DocumentPk = RS["ArrRequestEach_DocumentBodyPk[]" + i];
				TempSettlementEach.Description = RS["ArrRequestEach_Description[]" + i] + "";
				TempSettlementEach.MonetaryUnitCL = RS["ArrRequestEach_MonetaryUnitCL[]" + i] + "";
				TempSettlementEach.MonetaryUnit = RS["ArrRequestEach_MonetaryUnit[]" + i] + "";
				TempSettlementEach.Price = decimal.Parse(RS["ArrRequestEach_Price[]" + i] + "");
				TempSettlementEach.Exchanged_MonetaryUnitCL = RS["ArrRequestEach_Exchanged_MonetaryUnitCL[]" + i] + "";
				TempSettlementEach.Exchanged_MonetaryUnit = RS["ArrRequestEach_ExchangedMonetaryUnit[]" + i] + "";
				TempSettlementEach.Type = RS["ArrRequestEach_Type[]" + i] + "";
				TempSettlementEach.TypePk = RS["ArrRequestEach_TypePk[]" + i] + "";
				TempSettlementEach.BranchPk_Own = RS["ArrRequestEach_BranchPk_Own[]" + i] + "";
				TempSettlementEach.Exchanged_Price = decimal.Parse(RS["ArrRequestEach_ExchangedPrice[]" + i] + "");

				TempSettlementRequest.ArrEach.Add(TempSettlementEach);
				Temp.Add(TempSettlementRequest);
				i++;
			}
		}

		ReturnValue.ArrRequest = new List<SettlementRequest>();
		foreach (SettlementRequest each in Temp) {
			bool isin = false;
			var i = 0;
			for (i = 0; i < ReturnValue.ArrRequest.Count; i++) {
				if (each.RequestPk == ReturnValue.ArrRequest[i].RequestPk) {
					isin = true;
					break;
				}
			}
			if (isin) {
				ReturnValue.ArrRequest[i].ArrEach.Add(each.ArrEach[0]);
			} else {
				ReturnValue.ArrRequest.Add(each);
			}
		}

		decimal TotalPrice = 0;

		for (var i = 0; i < ReturnValue.ArrEach.Count; i++) {
			if (ReturnValue.ArrEach[i].BranchPk_Own == "2888") {
				TotalPrice += ReturnValue.ArrEach[i].Exchanged_Price;
				//Response.Write(Current.ArrEach[i].Exchanged_Price + "____" + TotalPrice);
			}
		}
		for (var h = 0; h < ReturnValue.ArrRequest.Count; h++) {
			for (var i = 0; i < ReturnValue.ArrRequest[h].ArrEach.Count; i++) {
				if (ReturnValue.ArrRequest[h].ArrEach[i].BranchPk_Own == "2888") {
					TotalPrice += ReturnValue.ArrRequest[h].ArrEach[i].Exchanged_Price;
					//Response.Write(Current.ArrRequest[h].ArrEach[i].Exchanged_Price + "____" + TotalPrice);
				}
			}
		}
		ReturnValue.AmountYW = TotalPrice;
		
		Components.SettlementWithBranch SwB = new Components.SettlementWithBranch();
		SwB.SetSave(ref ReturnValue);
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE [INTL2010].[dbo].[TransportBBHead] SET SettlementStatus=2 WHERE TransportBetweenBranchPk=" + ReturnValue.TBBHPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string Change_RequestForm_Owner(string RequestFormPk, string BranchPk) {
		string query = @"
			SELECT [TransportBetweenBranchPk], COUNT(*)
			  FROM [INTL2010].[dbo].[TransportBBHistory] 
			  WHERE TransportBetweenBranchPk =(
			  SELECT TBBH.TransportBetweenBranchPk 
			  FROM [INTL2010].[dbo].[TransportBBHistory] AS TBBHistory
				left join TransportBBHead AS TBBH ON TBBHistory.TransportBetweenBranchPk=TBBH.TransportBetweenBranchPk 
			  WHERE RequestFormPk=47365 and ToBranchPk=3157
			  ) 
			  GROUP BY [TransportBetweenBranchPk]
			";
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = query;
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		string TBBHPk = "";
		if (RS.Read()) {
			if (RS[1] + "" == "1") {
				TBBHPk = RS[0] + "";
			}
		}
		RS.Close();
		if (TBBHPk != "") {
			query = @"
			UPDATE [dbo].[RequestForm] SET [BranchPk_Own] = " + BranchPk + @" WHERE RequestFormPk=" + RequestFormPk + @";
			UPDATE [INTL2010].[dbo].[TransportBBHead] SET [BranchPk_Own]=" + BranchPk + @" WHERE [TransportBetweenBranchPk]=" + TBBHPk + ";";
			DB.SqlCmd.CommandText = query;
			DB.SqlCmd.ExecuteNonQuery();
		}
		DB.DBCon.Close();
		if (TBBHPk != "") {
			return "1";
		} else {
			return "0";
		}
	}

	[WebMethod]
	public string SetTransportBBHead_BranchPk_Own(string TBBHPk, string Value) {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = "UPDATE [dbo].[TransportBBHead] SET BranchPk_Own=" + Value + " WHERE [TransportBetweenBranchPk] =" + TBBHPk + ";";
		DB.DBCon.Open();
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
		return "1";
	}
	
}