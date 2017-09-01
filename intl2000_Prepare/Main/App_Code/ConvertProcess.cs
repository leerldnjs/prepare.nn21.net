using Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.Services;

/// <summary>
/// ConvertProcess의 요약 설명입니다.
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
[System.Web.Script.Services.ScriptService]
public class ConvertProcess : System.Web.Services.WebService
{
	public ConvertProcess() {
		//디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
		//InitializeComponent(); 
	}

	[WebMethod]
	public string TostringTest() {
		for (int i = 0; i < 10000000; i++) {
			string a = i.ToString();
		}


		return "1";
	}

	[WebMethod]
	public string ConvertTransport_1_0() {
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"
			SELECT
				TBBH.[TransportBetweenBranchPk],[TransportCL],[BLNo],[FromBranchPk],[ToBranchPk]
				,[FromDateTime],[ToDateTime],[ArrivalDateTime],[Description]
				, ISNULL(Step.Step, 3) AS Step
			FROM [INTL2010].[dbo].[TransportBBHead] AS TBBH
				left join [INTL2010].[dbo].[TransportBBStep] AS Step ON TBBH.TransportBetweenBranchPk=Step.TransportBetweenBranchPk
			WHERE Description IS NOT NULL
			AND TBBH.[TransportBetweenBranchPk] >= 20000 
			ORDER BY TBBH.[TransportBetweenBranchPk];";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		List<sTransportHead> TransportHead = new List<sTransportHead>();

		List<string[]> TBBHPk_N_Step = new List<string[]>();
		while (RS.Read()) {
			var Temp = new sTransportHead();
			Temp.arrPacked = new List<sTransportPacked>();

			Temp.Transport_Head_Pk = RS["TransportBetweenBranchPk"].ToString();
			Temp.Transport_Status = RS["Step"].ToString();
			string TransportCL = RS["TransportCL"] + "";
			string[] arrDescription = (RS["Description"].ToString()).Split(Common.Splite321, StringSplitOptions.None);
			var TempPacked = new sTransportPacked();
			switch (TransportCL) {
				case "1":
					Temp.Transport_Way = "Air";
					Temp.Title = arrDescription[0];        //CompanyName
					Temp.Area_From = arrDescription[1];
					Temp.Area_To = arrDescription[2];
					Temp.VesselName = arrDescription[3];
					Temp.Value_String_1 = arrDescription[4];        //FromCompanyTEL
					Temp.Value_String_2 = arrDescription[5];        //ToCompanyTEL
					break;
				case "2":
					Temp.Transport_Way = "Car";
					Temp.Title = arrDescription[0];
					Temp.Area_From = arrDescription[1];
					Temp.Area_To = arrDescription[2];
					Temp.Voyage_No = arrDescription[3];
					Temp.Value_String_3 = arrDescription[4];
					Temp.Value_String_1 = arrDescription[5];
					Temp.VesselName = arrDescription[6];        //DriverName

					try {
						TempPacked = new sTransportPacked();
						TempPacked.RealPacked_Flag = "Y";
						TempPacked.Size = arrDescription[7];
						TempPacked.No = arrDescription[8];
						TempPacked.Seal_No = arrDescription[9];

						Temp.arrPacked.Add(TempPacked);
					}
					catch (Exception) {

					}
					
					break;
				case "3":
					Temp.Transport_Way = "Ship";
					Temp.Title = arrDescription[0];
					Temp.Area_From = arrDescription[1];
					Temp.Area_To = arrDescription[2];
					Temp.VesselName = arrDescription[3];
					Temp.Voyage_No = arrDescription[5];

					TempPacked = new sTransportPacked();
					TempPacked.RealPacked_Flag = "Y";
					TempPacked.Size = arrDescription[4];
					TempPacked.No = arrDescription[6];
					TempPacked.Seal_No = arrDescription[7];

					Temp.arrPacked.Add(TempPacked);
					break;
				case "4":
					Temp.Transport_Way = "Sub";
					Temp.Title = arrDescription[0];        //CompanyName
					Temp.Area_From = arrDescription[1];
					Temp.Area_To = arrDescription[2];
					Temp.Value_String_1 = arrDescription[3];        //FromStaff TEL
					Temp.Value_String_2 = arrDescription[4];        //ToStaff TEL
					break;
				case "5"://FCL
					Temp.Transport_Way = "Ship";
					Temp.Title = arrDescription[0];
					Temp.Area_From = arrDescription[1];
					Temp.Area_To = arrDescription[2];
					Temp.VesselName = arrDescription[3];
					Temp.Voyage_No = arrDescription[6];

					TempPacked = new sTransportPacked();
					TempPacked.RealPacked_Flag = "Y";
					TempPacked.Size = arrDescription[5];
					TempPacked.No = arrDescription[7];
					TempPacked.Seal_No = arrDescription[8];

					Temp.arrPacked.Add(TempPacked);
					break;
				case "6"://LCL
					Temp.Transport_Way = "Sub";
					Temp.Title = arrDescription[0];        //CompanyName
					Temp.Area_From = arrDescription[1];
					Temp.Area_To = arrDescription[2];
					Temp.VesselName = arrDescription[3];
					Temp.Value_String_1 = arrDescription[7];        //FromStaff TEL
					Temp.Value_String_2 = arrDescription[8];        //ToStaff TEL

					TempPacked = new sTransportPacked();
					TempPacked.RealPacked_Flag = "Y";
					TempPacked.Size = arrDescription[4];
					TempPacked.No = arrDescription[5];
					TempPacked.Seal_No = arrDescription[6];

					Temp.arrPacked.Add(TempPacked);
					break;
			}
			Temp.Value_String_0 = RS["BLNo"].ToString();
			Temp.BranchPk_From = RS["FromBranchPk"].ToString();
			Temp.BranchPk_To = RS["ToBranchPk"].ToString();
			Temp.DateTime_From = RS["FromDateTime"].ToString().Replace("/", "").Replace(".", "");
			Temp.DateTime_To = RS["ToDateTime"].ToString().Replace("/", "").Replace(".", "");
			TransportHead.Add(Temp);
			TBBHPk_N_Step.Add(new string[] { RS["TransportBetweenBranchPk"].ToString(), RS["Step"].ToString() });
		}
		RS.Close();

		List<sTransportHead> sumBody = new List<sTransportHead>();
		foreach (string[] row in TBBHPk_N_Step) {
			if (row[1] == "3") {
				DB.SqlCmd.CommandText = @"
	SELECT 
		Storage.BoxCount , R.RequestFormPk, R.ShipperPk, R.ConsigneePk 
		, C.CompanyCode AS ShipperCode, C.CompanyName AS ShipperName 
		, CC.CompanyCode AS ConsigneeCode, CC.CompanyName AS ConsigneeName
		, CalcH.PackingUnit, CalcH.TotalGrossWeight, CalcH.TotalVolume
	FROM  
		TransportBBHistory AS Storage 
		left join RequestForm AS R on Storage.RequestFormPk=R.RequestFormPk 
		Left join Company AS C on R.ShipperPk=C.CompanyPk 
		Left join Company AS CC on R.ConsigneePk=CC.CompanyPk 
		Left join RegionCode AS Departure on R.DepartureRegionCode=Departure.RegionCode 
		Left join RegionCode AS Arrival on R.ArrivalRegionCode=Arrival.RegionCode 
		left join RequestFormCalculateHead AS CalcH ON Storage.RequestFormPk=CalcH.RequestFormPk 
	WHERE Storage.TransportBetweenBranchPk=" + row[0] + @" 
	ORDER BY R.ConsigneeCode, R.RequestFormPk, R.ArrivalRegionCode;";
			} else {
				DB.SqlCmd.CommandText = @"
	SELECT 
		Storage.BoxCount , R.RequestFormPk, R.ShipperPk, R.ConsigneePk 
		, C.CompanyCode AS ShipperCode, C.CompanyName AS ShipperName 
		, CC.CompanyCode AS ConsigneeCode, CC.CompanyName AS ConsigneeName
		, CalcH.PackingUnit, CalcH.TotalGrossWeight, CalcH.TotalVolume
	FROM  
		OurBranchStorage AS Storage left join 
		RequestForm AS R on Storage.RequestFormPk=R.RequestFormPk Left join 
		Company AS C on R.ShipperPk=C.CompanyPk Left join 
		Company AS CC on R.ConsigneePk=CC.CompanyPk Left join 
		RequestFormCalculateHead AS CalcH ON Storage.RequestFormPk=CalcH.RequestFormPk 
	WHERE Storage.TransportBetweenBranchPk=" + row[0] + @" 
	AND Storage.[StatusCL] = 1;";
			}

			List<sTransportBody> TempBody = new List<sTransportBody>();
			RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				TempBody.Add(new sTransportBody() {
					Consignee_Company_Code = RS["ConsigneeCode"].ToString(),
					Consignee_Company_Name = RS["ConsigneeName"].ToString(),
					Consignee_Company_Pk = RS["ConsigneePk"].ToString(),
					Packed_Count = RS["BoxCount"].ToString(),
					Shipper_Company_Code = RS["ShipperCode"].ToString(),
					Shipper_Company_Name = RS["ShipperName"].ToString(),
					Packing_Unit = RS["PackingUnit"].ToString(),
					Request_Pk = RS["RequestFormPk"].ToString(),
					Shipper_Company_Pk = RS["ShipperPk"].ToString(),
					Transport_Head_Pk = row[0],
					Volume = RS["TotalVolume"].ToString(),
					Weight = RS["TotalGrossWeight"].ToString()
				});
			}
			RS.Dispose();
			sumBody.Add(new sTransportHead() {
				Transport_Head_Pk = row[0],
				arrBody = TempBody
			});
		}

		List<sTransportHead> FinalData = new List<sTransportHead>();
		for (var i = 0; i < TransportHead.Count; i++) {
			sTransportHead current = TransportHead[i];
			for (var j = 0; j < sumBody.Count; j++) {
				if (current.Transport_Head_Pk == sumBody[j].Transport_Head_Pk) {
					if (current.arrPacked.Count > 0) {
						sTransportPacked currentPacked = current.arrPacked[0];
						currentPacked.arrBody = sumBody[j].arrBody;
						current.arrPacked[0] = currentPacked;
					} else {
						current.arrBody = sumBody[j].arrBody;
					}
					sumBody.RemoveAt(j);
					break;
				}
			}
			FinalData.Add(current);
		}

		StringBuilder query = new StringBuilder();
		/*
		query.Append(@"
			DELETE FROM [dbo].[TRANSPORT_HEAD];
			DELETE FROM [dbo].[TRANSPORT_PACKED];
			DELETE FROM [dbo].[TRANSPORT_BODY];
			DBCC CHECKIDENT(TRANSPORT_HEAD, RESEED, 0);
			DBCC CHECKIDENT(TRANSPORT_PACKED, RESEED, 0);
			DBCC CHECKIDENT(TRANSPORT_BODY, RESEED, 0);
		");
		*/
		query.Append(@"
			SET IDENTITY_INSERT [INTL2010].[dbo].[TRANSPORT_HEAD] ON; 
			DECLARE @HeadPk int; 
			DECLARE @PackedPk int; 
			DECLARE @BodyPk int; ");
		foreach (sTransportHead head in FinalData) {
			query.Append("INSERT INTO [dbo].[TRANSPORT_HEAD] ([TRANSPORT_PK], [TRANSPORT_WAY] ,[TRANSPORT_STATUS] ,[BRANCHPK_FROM] ,[BRANCHPK_TO] ,[WAREHOUSE_PK_ARRIVAL] ,[AREA_FROM] ,[AREA_TO] ,[DATETIME_FROM] ,[DATETIME_TO] ,[TITLE] ,[VESSELNAME] ,[VOYAGE_NO] ,[VALUE_STRING_0] ,[VALUE_STRING_1] ,[VALUE_STRING_2] ,[VALUE_STRING_3] ,[VALUE_STRING_4] ,[VALUE_STRING_5]) VALUES (");
			query.Append(Common.StringToDB(head.Transport_Head_Pk, false, false));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Transport_Way, true, false));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Transport_Status, false, false));
			query.Append(", ");
			query.Append(Common.StringToDB(head.BranchPk_From, false, false));
			query.Append(", ");
			query.Append(Common.StringToDB(head.BranchPk_To, false, false));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Warehouse_Pk_Arrival, false, false));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Area_From, true, true));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Area_To, true, true));
			query.Append(", ");
			query.Append(Common.StringToDB(head.DateTime_From, true, false));
			query.Append(", ");
			query.Append(Common.StringToDB(head.DateTime_To, true, false));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Title, true, true));
			query.Append(", ");
			query.Append(Common.StringToDB(head.VesselName, true, true));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Voyage_No, true, false));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Value_String_0, true, true));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Value_String_1, true, true));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Value_String_2, true, true));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Value_String_3, true, true));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Value_String_4, true, true));
			query.Append(", ");
			query.Append(Common.StringToDB(head.Value_String_5, true, true));
			query.Append(");");
			query.Append("SELECT @HeadPk=@@IDENTITY;");
			foreach (sTransportPacked packed in head.arrPacked) {
				query.Append("INSERT INTO [dbo].[TRANSPORT_PACKED] ([TRANSPORT_HEAD_PK], [COMPANY_PK_OWNER], [CONTAINER_COMPANY], [TYPE], [NO], [SIZE], [SEAL_NO],[REALPACKED_FLAG]) VALUES (@HeadPk");
				query.Append(", ");
				query.Append(Common.StringToDB(packed.Company_Pk_Owner, false, false));
				query.Append(", ");
				query.Append(Common.StringToDB(packed.Container_Company, true, false));
				query.Append(", ");
				query.Append(Common.StringToDB(packed.Type, true, false));
				query.Append(", ");
				query.Append(Common.StringToDB(packed.No, true, false));
				query.Append(", ");
				query.Append(Common.StringToDB(packed.Size, true, false));
				query.Append(", ");
				query.Append(Common.StringToDB(packed.Seal_No, true, false));
				query.Append(", ");
				query.Append(Common.StringToDB("Y", true, false));
				query.Append(");");
				query.Append("SELECT @PackedPk=@@IDENTITY;");
				foreach (sTransportBody body in packed.arrBody) {
					query.Append(@"INSERT INTO [dbo].[TRANSPORT_BODY]
           ([TRANSPORT_HEAD_PK]
           ,[TRANSPORT_PACKED_PK]
           ,[WAREHOUSE_PK_DEPARTURE]
           ,[REQUEST_PK]
           ,[SHIPPER_COMPANY_PK]
           ,[CONSIGNEE_COMPANY_PK]
           ,[SHIPPER_COMPANY_CODE]
           ,[CONSIGNEE_COMPANY_CODE]
           ,[SHIPPER_COMPANY_NAME]
           ,[CONSIGNEE_COMPANY_NAME]
           ,[PACKED_COUNT]
           ,[PACKING_UNIT]
           ,[DESCRIPTION]
           ,[WEIGHT]
           ,[VOLUME])
     VALUES
           (@HeadPk
           ,@PackedPk");
					query.Append(", ");
					query.Append(Common.StringToDB(body.Warehouse_Pk_Departure, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Request_Pk, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Shipper_Company_Pk, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Consignee_Company_Pk, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Shipper_Company_Code, true, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Consignee_Company_Code, true, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Shipper_Company_Name, true, true));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Consignee_Company_Name, true, true));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Packed_Count, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Packing_Unit, true, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Description, true, true));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Weight, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Volume, false, false));
					query.Append(");");
				}
			}
			query.Append("SET @PackedPk=null; ");
			if (head.arrBody != null) {
				foreach (sTransportBody body in head.arrBody) {
					query.Append(@"INSERT INTO [dbo].[TRANSPORT_BODY]
           ([TRANSPORT_HEAD_PK]
           ,[TRANSPORT_PACKED_PK]
           ,[WAREHOUSE_PK_DEPARTURE]
           ,[REQUEST_PK]
           ,[SHIPPER_COMPANY_PK]
           ,[CONSIGNEE_COMPANY_PK]
           ,[SHIPPER_COMPANY_CODE]
           ,[CONSIGNEE_COMPANY_CODE]
           ,[SHIPPER_COMPANY_NAME]
           ,[CONSIGNEE_COMPANY_NAME]
           ,[PACKED_COUNT]
           ,[PACKING_UNIT]
           ,[DESCRIPTION]
           ,[WEIGHT]
           ,[VOLUME])
     VALUES
           (@HeadPk
           ,@PackedPk");

					query.Append(", ");
					query.Append(Common.StringToDB(body.Warehouse_Pk_Departure, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Request_Pk, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Shipper_Company_Pk, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Consignee_Company_Pk, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Shipper_Company_Code, true, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Consignee_Company_Code, true, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Shipper_Company_Name, true, true));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Consignee_Company_Name, true, true));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Packed_Count, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Packing_Unit, true, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Description, true, true));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Weight, false, false));
					query.Append(", ");
					query.Append(Common.StringToDB(body.Volume, false, false));
					query.Append(");");
				}
			}

		}
		//query.Append(@"SET IDENTITY_INSERT [INTL2010].[dbo].[TRANSPORT_HEAD] OFF; ");
		
		DB.SqlCmd.CommandText = query.ToString();
		DB.SqlCmd.ExecuteNonQuery();
		//DBCC CHECKIDENT('테이블명',RESEED,0)

		DB.DBCon.Close();
		return "1";
	}

	[WebMethod]
	public string ConvertStorage_1_1() {
		StringBuilder Query = new StringBuilder();
		List<sStorageItem> Storage = new List<sStorageItem>();
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"
	SELECT 
		Storage.BoxCount , Storage.[StorageCode], R.RequestFormPk, R.ShipperPk, R.ConsigneePk 
		, C.CompanyCode AS ShipperCode, C.CompanyName AS ShipperName 
		, CC.CompanyCode AS ConsigneeCode, CC.CompanyName AS ConsigneeName
		, CalcH.PackingUnit, CalcH.TotalGrossWeight, CalcH.TotalVolume, TBBH.TransportBetweenBranchPk, TP.TRANSPORT_PACKED_PK
	FROM  
		OurBranchStorage AS Storage left join 
		RequestForm AS R on Storage.RequestFormPk=R.RequestFormPk Left join 
		Company AS C on R.ShipperPk=C.CompanyPk Left join 
		Company AS CC on R.ConsigneePk=CC.CompanyPk Left join 
		RequestFormCalculateHead AS CalcH ON Storage.RequestFormPk=CalcH.RequestFormPk Left join
		TransportBBHistory AS TBBH ON Storage.RequestFormPk=TBBH.RequestFormPk Left join
		TRANSPORT_PACKED AS TP ON TBBH.TransportBetweenBranchPk = TP.TRANSPORT_PACKED_PK
	WHERE Storage.[StatusCL] = 0;";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			Storage.Add(new sStorageItem {
				Warehouse_Pk = RS["StorageCode"].ToString(),
				Last_Transport_Head_Pk = RS["TransportBetweenBranchPk"].ToString(),
				Last_Transport_Pacekd_Pk = RS["TRANSPORT_PACKED_PK"].ToString(),
				Request_Pk = RS["RequestFormPk"].ToString(),
				Shipper_Company_Pk = RS["ShipperPk"].ToString(),
				Shipper_Company_Code = RS["ShipperCode"].ToString(),
				Shipper_Company_Name = RS["ShipperName"].ToString(),
				Consignee_Company_Pk = RS["ConsigneePk"].ToString(),
				Consignee_Company_Code = RS["ConsigneeCode"].ToString(),
				Consignee_Company_Name = RS["ConsigneeName"].ToString(),
				Packed_Count = RS["BoxCount"].ToString(),
				Packing_Unit = RS["PackingUnit"].ToString(),
				Weight = RS["TotalGrossWeight"].ToString(),
				Volume = RS["TotalVolume"].ToString()
			});
		}
		RS.Close();

		Query.Append(@"
			DELETE FROM [dbo].[STORAGE];
			DBCC CHECKIDENT(STORAGE, RESEED, 0);");

		foreach (sStorageItem S in Storage) {
			Query.Append(@"INSERT INTO [dbo].[STORAGE] 
			([REQUEST_PK]
		   ,[WAREHOUSE_PK]
		   ,[LAST_TRANSPORT_HEAD_PK]
		   ,[LAST_TRANSPORT_PACKED_PK]
           ,[SHIPPER_COMPANY_PK]
           ,[CONSIGNEE_COMPANY_PK]
           ,[SHIPPER_COMPANY_CODE]
           ,[CONSIGNEE_COMPANY_CODE]
           ,[SHIPPER_COMPANY_NAME]
           ,[CONSIGNEE_COMPANY_NAME]
           ,[PACKED_COUNT]
           ,[PACKING_UNIT]
           ,[WEIGHT]
           ,[VOLUME])
			VALUES (
			");
			Query.Append(Common.StringToDB(S.Request_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Warehouse_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Last_Transport_Head_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Last_Transport_Pacekd_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Shipper_Company_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Consignee_Company_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Shipper_Company_Code, true, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Consignee_Company_Code, true, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Shipper_Company_Name, true, true));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Consignee_Company_Name, true, true));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Packed_Count, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Packing_Unit, true, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Weight, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(S.Volume, false, false));
			Query.Append(");");
		}

		DB.DBCon.Close();
		return Query.ToString();
	}


	[WebMethod]
	public string ConvertRequestCharge_2() {
		StringBuilder Query = new StringBuilder();
		List<sRequestFormCalculateHead> HeadS = new List<sRequestFormCalculateHead>();
		List<sRequestFormCalculateHead> HeadC = new List<sRequestFormCalculateHead>();
		sRequestFormCalculateHead HShipper;
		sRequestFormCalculateHead HConsignee;

		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT 
			RFCH.[RequestFormPk]
			,ISNULL(RFCH.[TotalPackedCount], '0') AS TotalPackedCount
			,ISNULL(RFCH.[PackingUnit], '0') AS PackingUnit
			,ISNULL(RFCH.[TotalGrossWeight], '0') AS TotalGrossWeight
			,ISNULL(RFCH.[TotalVolume], '0') AS TotalVolume
			,RFCH.[StandardPriceHeadPk]
			,RFCH.[StandardPriceA]
			,SUBSTRING(RFCH.[CriterionValue], 1, CHARINDEX('!', RFCH.[CriterionValue]) - 1) AS CriterionValue
			,SUBSTRING(RFCH.[CriterionValue], CHARINDEX('!', RFCH.[CriterionValue]) + 1, 9) AS OverWeightValue
			,RFCH.[ExchangeRate]
			,ISNULL(RFCH.[ShipperMonetaryUnit], '0') AS ShipperMonetaryUnit
			,ISNULL(RFCH.[ShipperCharge], '0') + (CASE RFCH.[WillPayTariff] WHEN 'S' THEN ISNULL(TARIFF.[value], '0') ELSE 0 END) AS ShipperCharge
			,ISNULL(RFCH.[ConsigneeMonetaryUnit], '0') AS ConsigneeMonetaryUnit
			,ISNULL(RFCH.[ConsigneeCharge], '0') + (CASE RFCH.[WillPayTariff] WHEN 'C' THEN ISNULL(TARIFF.[value], '0') ELSE 0 END) AS ConsigneeCharge
			,ISNULL(CONVERT(CHAR(19), RFCH.[ShipperDepositedDate], 120), '20170101') AS ShipperDepositedDate
			,ISNULL(RFCH.[ShipperDeposited], '0') AS ShipperDeposited
			,RFCH.[ShipperBankAccountPk]
			,ISNULL(CONVERT(CHAR(19), RFCH.[ConsigneeDepositedDate], 120), '20170101') AS ConsigneeDepositedDate
			,ISNULL(RFCH.[ConsigneeDeposited], '0') AS ConsigneeDeposited
			,RFCH.[ConsigneeBankAccountPk]
			,RFCH.[WillPayTariff]
			,ISNULL(RF.[ShipperPk], '0') AS ShipperPk
			,ISNULL(RF.[DepartureBranchPk], '0') AS DepartureBranchPk
			,ISNULL(RF.[ConsigneePk], '0') AS ConsigneePk
			,ISNULL(RF.[ArrivalBranchPk], '0') AS ArrivalBranchPk
		FROM [INTL2010].[dbo].[RequestFormCalculateHead] AS RFCH
		LEFT JOIN [dbo].[RequestForm] AS RF ON RFCH.[RequestFormPk] = RF.[RequestFormPk]
		LEFT JOIN ( SELECT [GubunPk], MAX([MonetaryUnitCL]) AS MonetaryUnitCL, SUM([Value]) AS Value
					FROM [dbo].[CommercialDocumentTariff]
					GROUP BY [GubunPk]) AS TARIFF ON RFCH.[RequestFormPk] = TARIFF.[GubunPk] 
		WHERE RFCH.[RequestFormPk] >= 5000 AND  RFCH.[RequestFormPk] < 10000
		ORDER BY RFCH.[RequestFormPk]";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		int HeadPk = 1;
		while (RS.Read()) {
			string[] ExchangeRateSC = RS["ExchangeRate"].ToString().Split(new string[] { "@@" }, StringSplitOptions.None);
			string[] ExchangeRateS;
			try {
				ExchangeRateS = ExchangeRateSC[0].Split(new string[] { "!" }, StringSplitOptions.None);
			} catch (Exception) {
				ExchangeRateS = new string[] { "", "", "", "" };
			}
			
			string[] ExchangeRateC;
			try {
				ExchangeRateC = ExchangeRateSC[1].Split(new string[] { "!" }, StringSplitOptions.None);
			} catch (Exception) {
				ExchangeRateC = new string[] {"", "", "", "" };
			}

			HShipper = new sRequestFormCalculateHead();
			HShipper.RequestFormCalculate_Head_Pk = HeadPk.ToString();
			HShipper.Table_Name = "RequestForm";
			HShipper.Table_Pk = RS["RequestFormPk"].ToString();
			HShipper.Type = "Request";
			HShipper.Charge_No = "0";
			HShipper.Branch_Company_Pk = RS["DepartureBranchPk"].ToString();
			HShipper.Customer_Company_Pk = RS["ShipperPk"].ToString();
			try {
				HShipper.Charge_Date = ExchangeRateS[2].Replace("~", "");
				if (HShipper.Charge_Date == "" || HShipper.Charge_Date == null) {
					HShipper.Charge_Date = ExchangeRateC[2].Replace("~", "");
				}
			} catch (Exception) {
				HShipper.Charge_Date = "null";
			}
			
			HShipper.Monetary_Unit = RS["ShipperMonetaryUnit"].ToString();
			HShipper.Total_Price = RS["ShipperCharge"].ToString();
			HShipper.Deposited_Price = RS["ShipperDeposited"].ToString();
			HShipper.Last_Deposited_Date = RS["ShipperDepositedDate"].ToString();
			HShipper.Temp_ConverExchangedData = ExchangeRateS;
			HeadPk++;

			HConsignee = new sRequestFormCalculateHead();
			HConsignee.RequestFormCalculate_Head_Pk = HeadPk.ToString();
			HConsignee.Table_Name = "RequestForm";
			HConsignee.Table_Pk = RS["RequestFormPk"].ToString();
			HConsignee.Type = "Request";
			HConsignee.Charge_No = "0";
			HConsignee.Branch_Company_Pk = RS["ArrivalBranchPk"].ToString();
			HConsignee.Customer_Company_Pk = RS["ConsigneePk"].ToString();
			try {
				HConsignee.Charge_Date = ExchangeRateC[2].Replace("~", "");
				if (HConsignee.Charge_Date == "" || HConsignee.Charge_Date == null) {
					HConsignee.Charge_Date = ExchangeRateS[2].Replace("~", "");
				}
			} catch (Exception) {
				HConsignee.Charge_Date = "null";
			}
			HConsignee.Monetary_Unit = RS["ConsigneeMonetaryUnit"].ToString();
			HConsignee.Total_Price = RS["ConsigneeCharge"].ToString();
			HConsignee.Deposited_Price = RS["ConsigneeDeposited"].ToString();
			HConsignee.Last_Deposited_Date = RS["ConsigneeDepositedDate"].ToString();
			HConsignee.Temp_ConverExchangedData = ExchangeRateC;
			HeadPk++;

			HeadS.Add(HShipper);
			HeadC.Add(HConsignee);
			string Cri = RS["CriterionValue"].ToString() == "" ? "0" : RS["CriterionValue"].ToString();
			string Ove = RS["OverWeightValue"].ToString() == "" ? "0" : RS["OverWeightValue"].ToString();


			Query.Append("UPDATE [dbo].[RequestForm] SET [TotalPackedCount] = ");
			Query.Append(RS["TotalPackedCount"].ToString());
			Query.Append(", ");
			Query.Append("[PackingUnit] = ");
			Query.Append(RS["PackingUnit"].ToString());
			Query.Append(", ");
			Query.Append("[TotalGrossWeight] = ");
			Query.Append(RS["TotalGrossWeight"].ToString());
			Query.Append(", ");
			Query.Append("[TotalVolume] = ");
			Query.Append(RS["TotalVolume"].ToString());
			Query.Append(", ");
			Query.Append("[CriterionValue] = ");
			Query.Append(Common.StringToDB(Cri, true, false));
			Query.Append(" ,");
			Query.Append("[OverWeightValue] = ");
			Query.Append(Common.StringToDB(Ove, true, false));
			Query.Append(", ");
			Query.Append("[ExchangeDate] = ");
			Query.Append(Common.StringToDB(HShipper.Charge_Date, true, false));
			Query.Append(" WHERE [RequestFormPk] = ");
			Query.Append(RS["RequestFormPk"].ToString());
			Query.Append(";");
			
		}
		RS.Close();

		List<sRequestFormCalculateBody> Body = new List<sRequestFormCalculateBody>();
		sRequestFormCalculateBody BTemp;

		for (int x = 0; x < HeadS.Count; x++) {
			DB.SqlCmd.CommandText = @"SELECT 
				 RB.[RequestFormCalculateBodyPk]
				,RB.[RequestFormPk]
				,RB.[GubunCL]
				,RB.[Title]
				,RB.[Price]
				,RB.[MonetaryUnit]
				,RB.[StandardPriceHeadPkNColumn]
				,RB.[BranchPk_Own]
				,TH.[FromBranchPk]
				,TH.[ToBranchPk]
				,RH.[WillPayTariff]
				,TF1.TfTitle1
				,TF1.Value1
				,TF2.TfTitle2
				,TF2.Value2
				,TF3.TfTitle3
				,TF3.Value3
			FROM [dbo].[RequestFormCalculateBody] AS RB
			LEFT JOIN [dbo].[RequestFormCalculateHead] AS RH ON RB.[RequestFormPk] = RH.[RequestFormPk]
			LEFT JOIN ( 
						SELECT [GubunPk], [Title] AS TfTitle1, [Value] AS Value1
						FROM [dbo].[CommercialDocumentTariff]
						WHERE [Title]  = '관세'
						) AS TF1 ON RB.[RequestFormPk] = TF1.[GubunPk]
			LEFT JOIN ( 
						SELECT [GubunPk], [Title] AS TfTitle2, [Value] AS Value2
						FROM [dbo].[CommercialDocumentTariff]
						WHERE [Title]  = '부가세'
						) AS TF2 ON RB.[RequestFormPk] = TF2.[GubunPk]
			LEFT JOIN ( 
						SELECT [GubunPk], [Title] AS TfTitle3, [Value] AS Value3
						FROM [dbo].[CommercialDocumentTariff]
						WHERE [Title]  = '관세사비'
						) AS TF3 ON RB.[RequestFormPk] = TF3.[GubunPk]
			LEFT JOIN [dbo].[RequestForm] AS RF ON RB.[RequestFormPk] = RF.[RequestFormPk]
			LEFT JOIN (SELECT TOP (1) [RequestFormPk], [TransportBetweenBranchPk] FROM [dbo].[TransportBBHistory] WHERE [RequestFormPk] = " + HeadS[x].Table_Pk + @") AS TBH ON RF.[RequestFormPk] = TBH.[RequestFormPk]
			LEFT JOIN [dbo].[TransportBBHead] AS TH ON TBH.[TransportBetweenBranchPk] = TH.[TransportBetweenBranchPk]
			WHERE RB.[RequestFormPk] = " + HeadS[x].Table_Pk;

			RS = DB.SqlCmd.ExecuteReader();
			int i = 0;
			while (RS.Read()) {
				if (RS["GubunCL"] + "" == "200") {
					BTemp = new sRequestFormCalculateBody();
					BTemp.RequestFormCalculate_Head_Pk = HeadS[x].RequestFormCalculate_Head_Pk;
					BTemp.Settlement_Company_Pk = RS["FromBranchPk"].ToString();
					BTemp.Title = RS["Title"].ToString();
					BTemp.Original_Monetary_Unit = RS["MonetaryUnit"].ToString();
					BTemp.Exchanged_Monetary_Unit = HeadS[x].Monetary_Unit;
					BTemp.Original_Price = RS["Price"].ToString();
					try {
						BTemp.To_Exchange_Rate = HeadS[x].Temp_ConverExchangedData[3];
						if (BTemp.To_Exchange_Rate == "" || BTemp.To_Exchange_Rate == null) {
							BTemp.To_Exchange_Rate = "0";
						}
					} catch (Exception) {
						BTemp.To_Exchange_Rate = "0";
					}

					Body.Add(BTemp);

					if (i == 0 && RS["WillPayTariff"].ToString() == "S") {
						if (RS["TfTitle1"].ToString() != "") {
							BTemp = new sRequestFormCalculateBody();
							BTemp.RequestFormCalculate_Head_Pk = HeadS[x].RequestFormCalculate_Head_Pk;
							BTemp.Settlement_Company_Pk = RS["FromBranchPk"].ToString();
							BTemp.Title = RS["TfTitle1"].ToString();
							BTemp.Original_Monetary_Unit = "20";
							BTemp.Exchanged_Monetary_Unit = HeadS[x].Monetary_Unit;
							BTemp.Original_Price = RS["Value1"].ToString();
							Body.Add(BTemp);
						}
						if (RS["TfTitle2"].ToString() != "") {
							BTemp = new sRequestFormCalculateBody();
							BTemp.RequestFormCalculate_Head_Pk = HeadS[x].RequestFormCalculate_Head_Pk;
							BTemp.Settlement_Company_Pk = RS["FromBranchPk"].ToString();
							BTemp.Title = RS["TfTitle2"].ToString();
							BTemp.Original_Monetary_Unit = "20";
							BTemp.Exchanged_Monetary_Unit = HeadS[x].Monetary_Unit;
							BTemp.Original_Price = RS["Value2"].ToString();
							Body.Add(BTemp);
						}
						if (RS["TfTitle3"].ToString() != "") {
							BTemp = new sRequestFormCalculateBody();
							BTemp.RequestFormCalculate_Head_Pk = HeadS[x].RequestFormCalculate_Head_Pk;
							BTemp.Settlement_Company_Pk = RS["FromBranchPk"].ToString();
							BTemp.Title = RS["TfTitle3"].ToString();
							BTemp.Original_Monetary_Unit = "20";
							BTemp.Exchanged_Monetary_Unit = HeadS[x].Monetary_Unit;
							BTemp.Original_Price = RS["Value3"].ToString();
							Body.Add(BTemp);
						}
					}
				}
				else {
					BTemp = new sRequestFormCalculateBody();
					BTemp.RequestFormCalculate_Head_Pk = HeadC[x].RequestFormCalculate_Head_Pk;
					BTemp.Settlement_Company_Pk = RS["ToBranchPk"].ToString();
					BTemp.Title = RS["Title"].ToString();
					BTemp.Original_Monetary_Unit = RS["MonetaryUnit"].ToString();
					BTemp.Exchanged_Monetary_Unit = HeadC[x].Monetary_Unit;
					BTemp.Original_Price = RS["Price"].ToString();
					try {
						BTemp.To_Exchange_Rate = HeadC[x].Temp_ConverExchangedData[3];
						if (BTemp.To_Exchange_Rate == "" || BTemp.To_Exchange_Rate == null) {
							BTemp.To_Exchange_Rate = "0";
						}
					} catch (Exception) {
						BTemp.To_Exchange_Rate = "0";
					}

					Body.Add(BTemp);

					if (i == 0 && RS["WillPayTariff"].ToString() == "C") {
						if (RS["TfTitle1"].ToString() != "") {
							BTemp = new sRequestFormCalculateBody();
							BTemp.RequestFormCalculate_Head_Pk = HeadS[x].RequestFormCalculate_Head_Pk;
							BTemp.Settlement_Company_Pk = RS["ToBranchPk"].ToString();
							BTemp.Title = RS["TfTitle1"].ToString();
							BTemp.Original_Monetary_Unit = "20";
							BTemp.Exchanged_Monetary_Unit = HeadS[x].Monetary_Unit;
							BTemp.Original_Price = RS["Value1"].ToString();
							Body.Add(BTemp);
						}
						if (RS["TfTitle2"].ToString() != "") {
							BTemp = new sRequestFormCalculateBody();
							BTemp.RequestFormCalculate_Head_Pk = HeadS[x].RequestFormCalculate_Head_Pk;
							BTemp.Settlement_Company_Pk = RS["ToBranchPk"].ToString();
							BTemp.Title = RS["TfTitle2"].ToString();
							BTemp.Original_Monetary_Unit = "20";
							BTemp.Exchanged_Monetary_Unit = HeadS[x].Monetary_Unit;
							BTemp.Original_Price = RS["Value2"].ToString();
							Body.Add(BTemp);
						}
						if (RS["TfTitle3"].ToString() != "") {
							BTemp = new sRequestFormCalculateBody();
							BTemp.RequestFormCalculate_Head_Pk = HeadS[x].RequestFormCalculate_Head_Pk;
							BTemp.Settlement_Company_Pk = RS["ToBranchPk"].ToString();
							BTemp.Title = RS["TfTitle3"].ToString();
							BTemp.Original_Monetary_Unit = "20";
							BTemp.Exchanged_Monetary_Unit = HeadS[x].Monetary_Unit;
							BTemp.Original_Price = RS["Value3"].ToString();
							Body.Add(BTemp);
						}
					}

				}
				i++;
			}
			RS.Close();
		}

		
		Query.Append(@"
			DELETE FROM [dbo].[REQUESTFORMCALCULATE_HEAD];
			DELETE FROM [dbo].[REQUESTFORMCALCULATE_BODY];
			DBCC CHECKIDENT(REQUESTFORMCALCULATE_HEAD, RESEED, 0);
			DBCC CHECKIDENT(REQUESTFORMCALCULATE_BODY, RESEED, 0);");
			
		Query.Append(@"SET IDENTITY_INSERT [INTL2010].[dbo].[REQUESTFORMCALCULATE_HEAD] ON;");
		
		foreach(sRequestFormCalculateHead IHS in HeadS) {
			Query.Append(@"INSERT INTO [dbo].[REQUESTFORMCALCULATE_HEAD]
				([REQUESTFORMCALCULATE_HEAD_PK]
				,[TABLE_NAME]
				,[TABLE_PK]
				,[TYPE]
				,[CHARGE_NO]
				,[BRANCH_COMPANY_PK]
				,[BRANCH_BANK_PK]
				,[CUSTOMER_COMPANY_PK]
				,[CUSTOMER_BANK_PK]
				,[CHARGE_DATE]
				,[MONETARY_UNIT]
				,[TOTAL_PRICE]
				,[DEPOSITED_PRICE]
				,[LAST_DEPOSITED_DATE])
			VALUES
				( ");
			Query.Append(Common.StringToDB(IHS.RequestFormCalculate_Head_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Table_Name, true, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Table_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Type, true, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Charge_No, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Branch_Company_Pk, false, false));
			Query.Append(", ");
			Query.Append("NULL");
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Customer_Company_Pk, false, false));
			Query.Append(", ");
			Query.Append("NULL");
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Charge_Date, true, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Monetary_Unit, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Total_Price, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Deposited_Price, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHS.Last_Deposited_Date, true, false));
			Query.Append(");");
		}
		foreach (sRequestFormCalculateHead IHC in HeadC) {
			Query.Append(@"INSERT INTO [dbo].[REQUESTFORMCALCULATE_HEAD]
				([REQUESTFORMCALCULATE_HEAD_PK]
				,[TABLE_NAME]
				,[TABLE_PK]
				,[TYPE]
				,[CHARGE_NO]
				,[BRANCH_COMPANY_PK]
				,[BRANCH_BANK_PK]
				,[CUSTOMER_COMPANY_PK]
				,[CUSTOMER_BANK_PK]
				,[CHARGE_DATE]
				,[MONETARY_UNIT]
				,[TOTAL_PRICE]
				,[DEPOSITED_PRICE]
				,[LAST_DEPOSITED_DATE])
			VALUES
				( ");
			Query.Append(Common.StringToDB(IHC.RequestFormCalculate_Head_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Table_Name, true, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Table_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Type, true, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Charge_No, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Branch_Company_Pk, false, false));
			Query.Append(", ");
			Query.Append("NULL");
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Customer_Company_Pk, false, false));
			Query.Append(", "); 
			Query.Append("NULL");
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Charge_Date, true, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Monetary_Unit, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Total_Price, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Deposited_Price, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IHC.Last_Deposited_Date, true, false));
			Query.Append(");");
		}

		Query.Append(@"SET IDENTITY_INSERT [INTL2010].[dbo].[REQUESTFORMCALCULATE_HEAD] OFF;");

		foreach (sRequestFormCalculateBody IBS in Body) {
			Query.Append(@"INSERT INTO [dbo].[REQUESTFORMCALCULATE_BODY]
				(
					[REQUESTFORMCALCULATE_HEAD_PK]
					,[SETTLEMENT_COMPANY_PK]
					,[TITLE]
					,[ORIGINAL_MONETARY_UNIT]
					,[EXCHANGED_MONETARY_UNIT]
					,[TO_EXCHANGE_RATE]
					,[ORIGINAL_PRICE]
				)
			VALUES
				( ");
			Query.Append(Common.StringToDB(IBS.RequestFormCalculate_Head_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IBS.Settlement_Company_Pk, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IBS.Title, true, true));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IBS.Original_Monetary_Unit, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IBS.Exchanged_Monetary_Unit, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IBS.To_Exchange_Rate, false, false));
			Query.Append(", ");
			Query.Append(Common.StringToDB(IBS.Original_Price, false, false));
			Query.Append(");");
		}

		//DB.SqlCmd.CommandText = Query.ToString();
		//DB.SqlCmd.ExecuteNonQuery();

		DB.DBCon.Close();
		return Query.ToString();
	}

	[WebMethod]
	public string ConvertTransportCharge_6() {
		StringBuilder Query = new StringBuilder();
		List<sRequestFormCalculateHead> Head = new List<sRequestFormCalculateHead>();
		List<sRequestFormCalculateBody> Body = new List<sRequestFormCalculateBody>();
		DBConn DB = new DBConn();
		DB.DBCon.Open();



		DB.DBCon.Close();
		return Query.ToString();
	}

	[WebMethod]
	public string ConvertComment_3() {
		StringBuilder Query = new StringBuilder();
		List<sComment> Comment = new List<sComment>();
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"SELECT
			CASE TB.[GubunCL]
			WHEN 0 THEN 'Company'
			WHEN 1 THEN 'RequestForm'
			WHEN 2 THEN 'CommercialDocument'
			WHEN 4 THEN 'File'
			WHEN 10 THEN 'Company'
			WHEN 20 THEN 'RequestForm'
			WHEN 40 THEN 'Company'
			WHEN 41 THEN 'Company'
			WHEN 42 THEN 'Company'
			WHEN 52 THEN 'Company'
			END AS TABLE_NAME
			,[GubunPk] AS TABLE_PK
			,CASE TB.[GubunCL]
			WHEN 0 THEN 'Basic'
			WHEN 1 THEN 'Request'
			WHEN 2 THEN 'BL'
			WHEN 4 THEN 'File'
			WHEN 10 THEN 'Basic_Important'
			WHEN 20 THEN 'Request_Confirm'
			WHEN 40 THEN 'Delivery'
			WHEN 41 THEN 'ShipperSelection'
			WHEN 42 THEN 'RequestCharge'
			WHEN 52 THEN 'Company_Info'
			END AS CATEGORY
			,ISNULL(AC.[AccountPk], 0) AS AccountPk
			,TB.[AccountID]
			,ISNULL(AC.[Name], '') AS AccountName
			,[Contents]
			,ISNULL(CONVERT(CHAR(19), [Registerd], 120), '20170101') AS Registerd 
		FROM [dbo].[TalkBusiness] AS TB
		LEFT JOIN [dbo].[Account_] AS AC ON TB.[AccountID] = AC.[AccountID]
		WHERE TB.[GubunCL] IN (0, 1, 2, 4, 10, 20, 40, 41, 42, 52)";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			Comment.Add(new sComment() {
				Table_Name = RS["TABLE_NAME"].ToString(),
				Table_Pk = RS["TABLE_PK"].ToString(),
				Category = RS["CATEGORY"].ToString(),
				Contents = RS["Contents"].ToString(),
				Account_Pk = RS["AccountPk"].ToString(),
				Account_Id = RS["AccountID"].ToString(),
				Account_Name = RS["AccountName"].ToString(),
				Registerd = RS["Registerd"].ToString()
			});
		}
		RS.Close();

		DB.SqlCmd.CommandText = @"SELECT
			'TRANSPORT_HEAD' AS TABLE_NAME
			,[TransportBBPk] AS TABLE_PK
			,'TransportHead' AS CATEGORY
			,ISNULL(AC.AccountPk, 0) AS AccountPk
			,TC.[AccountID]
			,ISNULL(AC.Name, '') AS AccountName
			,[Comment] AS Contents
			,ISNULL(CONVERT(CHAR(19), [Registerd], 120), '20010101') AS Registerd 
		FROM [dbo].[TransportBBComment] AS TC
		LEFT JOIN [dbo].[Account_] AS AC ON TC.[AccountID] = AC.[AccountID]";
		RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			Comment.Add(new sComment() {
				Table_Name = RS["TABLE_NAME"].ToString(),
				Table_Pk = RS["TABLE_PK"].ToString(),
				Category = RS["CATEGORY"].ToString(),
				Contents = RS["Contents"].ToString(),
				Account_Pk = RS["AccountPk"].ToString(),
				Account_Id = RS["AccountID"].ToString(),
				Account_Name = RS["AccountName"].ToString(),
				Registerd = RS["Registerd"].ToString()
			});
		}
		RS.Close();

		DB.SqlCmd.CommandText = @"SELECT
			'RequestForm' AS TABLE_NAME
			,[RequestFormPk] AS TABLE_PK
			,RA.[GubunCL] AS CATEGORY
			,[Value] AS Contents
			,ISNULL(AC.AccountPk, 0) AS AccountPk
			,RA.[ActID] AS AccountID
			,ISNULL(AC.Name, '') AS AccountName
			,ISNULL(CONVERT(CHAR(19), [ActDate], 120), '20170101') AS Registerd 
		FROM [INTL2010].[dbo].[RequestFormAdditionalInfo] AS RA
		left join Account_ AS AC ON RA.ActID=AC.AccountID 
		WHERE RA.[ActID] IS NOT NULL";
		RS = DB.SqlCmd.ExecuteReader();

		while (RS.Read()) {
			Comment.Add(new sComment() {
				Table_Name = RS["TABLE_NAME"].ToString(),
				Table_Pk = RS["TABLE_PK"].ToString(),
				Category = RS["CATEGORY"].ToString(),
				Contents = RS["Contents"].ToString(),
				Account_Pk = RS["AccountPk"].ToString(),
				Account_Id = RS["AccountID"].ToString(),
				Account_Name = RS["AccountName"].ToString(),
				Registerd = RS["Registerd"].ToString()
			});
		}
		RS.Close();

		DB.DBCon.Close();

		Query.Append(@"DELETE FROM [dbo].[COMMENT];
						DBCC CHECKIDENT(COMMENT, RESEED, 0);");
		Setting ST = new Setting();
		foreach(sComment CM in Comment) {
			Query.Append(@"INSERT INTO [dbo].[COMMENT]
				([TABLE_NAME]
				,[TABLE_PK]
				,[CATEGORY]
				,[CONTENTS]
				,[ACCOUNT_PK]
				,[ACCOUNT_ID]
				,[ACCOUNT_NAME]
				,[REGISTERD])
			VALUES
				(");
			Query.Append(ST.ToDB(CM.Table_Name, "varchar"));
			Query.Append(", ");
			Query.Append(ST.ToDB(CM.Table_Pk, "int"));
			Query.Append(", ");
			Query.Append(ST.ToDB(CM.Category, "varchar"));
			Query.Append(", ");
			Query.Append(ST.ToDB(CM.Contents, "nvarchar"));
			Query.Append(", ");
			Query.Append(ST.ToDB(CM.Account_Pk, "int"));
			Query.Append(", ");
			Query.Append(ST.ToDB(CM.Account_Id, "varchar"));
			Query.Append(", ");
			Query.Append(ST.ToDB(CM.Account_Name, "nvarchar"));
			Query.Append(", ");
			Query.Append(ST.ToDB(CM.Registerd, "datetime"));
			Query.Append(");");
		}

		return Query.ToString();
	}

	[WebMethod]
	public string ConvertHistory_4() {
		StringBuilder Query = new StringBuilder();
		DBConn DB = new DBConn();
		DB.DBCon.Open();



		DB.DBCon.Close();
		return Query.ToString();
	}

	[WebMethod]
	public string ConvertStandardPrice_0() {
		StringBuilder Query = new StringBuilder();
		string[] Column = {"B", "C", "D", "E", "F", "G", "H", "I", "J" };
		DBConn DB = new DBConn();
		DB.DBCon.Open();
		DB.SqlCmd.CommandText = @"
			SELECT [StandardPriceListPk]
			  FROM [INTL2010].[dbo].[TransportWayCLSetting] 
			  WHERE StandardPriceListPk is not null ";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();

		List<string> StandardPriceListPk = new List<string>();

		while (RS.Read()) {
			string temp = RS[0] + "";
			StandardPriceListPk.AddRange(temp.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
		}
		RS.Close();

		List<string> ListPk = new List<string>();

		foreach (string each in StandardPriceListPk) {
			bool isin = false;
			foreach (string inner in ListPk) {
				if (each.Trim() == inner) {
					isin = true;
					break;
				}
			}
			if (!isin) {
				ListPk.Add(each.Trim());
			}
		}


		Query.Append(@"
				DELETE FROM [dbo].[STANDARDPRICE_LIST];
				DELETE FROM [dbo].[STANDARDPRICE_ITEM];
				DELETE FROM [dbo].[STANDARDPRICE_VALUE];
				DECLARE @ListPk int; 
				DECLARE @ItemPk int; ");
		foreach (string SListPk in ListPk) {
			string OurBranchPk = "";
			string Title = "";
			string TransportWayCL = "";
			string StandardGuide = "";
			string Role = "M";
			DB.SqlCmd.CommandText = @"
				SELECT [StandardPriceListPk]
						,[OurBranchPk]
						,[ArrivalBranchPk]
						,[TransportWayCL]
						,[Title]
						,[Guideline]
						,[FreightCharge]
						,[DepartureCharge]
						,[ArrivalCharge]
						,[BranchPk_Own]
				  FROM [INTL2010].[dbo].[StandardPrice] 
				 WHERE StandardPriceListPk=" + SListPk;
			RS = DB.SqlCmd.ExecuteReader();
			List<string> HeadPk = new List<string>();

			if (RS.Read()) {
				OurBranchPk = RS["OurBranchPk"].ToString();
				Title = RS["Title"].ToString();
				TransportWayCL = RS["TransportWayCL"].ToString();
				StandardGuide = RS["Guideline"].ToString();

				if (RS["FreightCharge"].ToString() != "") {
					HeadPk.Add(RS["FreightCharge"].ToString());
				}
				if (RS["DepartureCharge"].ToString() != "") {
					HeadPk.Add(RS["DepartureCharge"].ToString());
				}
				if (RS["ArrivalCharge"].ToString() != "") {
					HeadPk.Add(RS["ArrivalCharge"].ToString());
				}

				Query.Append(@"
				INSERT INTO [INTL2010].[dbo].[STANDARDPRICE_LIST] 
					([OURBRANCH_PK]
					 ,[TITLE]
					 ,[TRANSPORT_WAY_CL]
					 ,[STANDARD_GUIDE]
					 ,[ROLE]) 
				VALUES
					(" + OurBranchPk + @"
					,N'" + Title + @"'
					," + TransportWayCL + @"
					," + StandardGuide + @"
					,'" + Role + @"'); 
					SELECT @ListPk = @@IDENTITY;");

			}
			RS.Close();

			

			List<string[]> Item_Gubun = new List<string[]>();
			List<string[]> Item = new List<string[]>();
			foreach (string SHeadPk in HeadPk) {
				DB.SqlCmd.CommandText = @"SELECT 
					[StandardPriceHeadPk]
					,[Title]
					,[Length]
					,[OurBranchCode]
					,[A]
					,[B]
					,[C]
					,[D]
					,[E]
					,[F]
					,[G]
					,[H]
					,[I]
					,[J]
				 FROM [INTL2010].[dbo].[StandardPriceHead] 
				 WHERE StandardPriceHeadPk=" + SHeadPk;
				RS = DB.SqlCmd.ExecuteReader();
				string ItemTitle = "";
				string ItemMonetary = "";
				string ItemPayment = "";
				string ItemOverWeight = "";
				string EXW, DDP, CNF, FOB;
				if (RS.Read()) {
					for (int i = 0; i < Column.Length; i++) {
						if (RS[Column[i]] + "" != "") {
							ItemOverWeight = RS[Column[i]].ToString().Substring(0, 1) == "0" ? "N" : "Y";
							ItemPayment = RS[Column[i]].ToString().Substring(RS[Column[i]].ToString().IndexOf("~") + 1, RS[Column[i]].ToString().IndexOf("!") - RS[Column[i]].ToString().IndexOf("~") - 1);
							ItemMonetary = RS[Column[i]].ToString().Substring(RS[Column[i]].ToString().IndexOf("!") + 1, RS[Column[i]].ToString().IndexOf("@") - RS[Column[i]].ToString().IndexOf("!") - 1);
							ItemTitle = RS[Column[i]].ToString().Substring(RS[Column[i]].ToString().IndexOf("$") + 1, RS[Column[i]].ToString().IndexOf("%") - RS[Column[i]].ToString().IndexOf("$") - 1);
							switch (ItemPayment) {
								case "8":
									EXW = "C";
									DDP = "S";
									CNF = "S";
									FOB = "S";
									break;
								case "9":
									EXW = "C";
									DDP = "S";
									CNF = "S";
									FOB = "C";
									break;
								case "11":
									EXW = "C";
									DDP = "S";
									CNF = "C";
									FOB = "C";
									break;
								default:
									EXW = "";
									DDP = "";
									CNF = "";
									FOB = "";
									break;
							}

							Item_Gubun.Add(new string[] { SHeadPk, Column[i] });
							Item.Add(new string[] { "@ListPk", RS["OurBranchCode"] + "", ItemTitle, ItemMonetary, EXW, DDP, CNF, FOB, ItemOverWeight });
						}
					}
				}
				RS.Close();
			}
			

			for(int i = 0; i < Item_Gubun.Count; i++) {
				Query.Append(@"INSERT INTO [dbo].[STANDARDPRICE_ITEM]
					([STANDARDPRICE_PK]
					,[SETTLEMENT_BRANCH_PK]
					,[TITLE]
					,[MONETARY_UNIT]
					,[EXW]
					,[DDP]
					,[CNF]
					,[FOB]
					,[OVERWEIGHT_FLAG])
				VALUES
					(" + Item[i][0] + @"
					," + Item[i][1] + @"
					,N'" + Item[i][2] + @"'
					," + Item[i][3] + @"
					,'" + Item[i][4] + @"'
					,'" + Item[i][5] + @"'
					,'" + Item[i][6] + @"'
					,'" + Item[i][7] + @"'
					,'" + Item[i][8] + @"'); SELECT @ItemPk = @@IDENTITY;");

				DB.SqlCmd.CommandText = @"SELECT [A]
					,[" + Item_Gubun[i][1] + @"]
				 FROM [INTL2010].[dbo].[StandardPriceBody] 
				 WHERE [StandardPriceHeadPk]=" + Item_Gubun[i][0] + @"
				 AND [" + Item_Gubun[i][1] + @"] IS NOT NULL;";
				RS = DB.SqlCmd.ExecuteReader();
				while (RS.Read()) {
					Query.Append(@"INSERT INTO [dbo].[STANDARDPRICE_VALUE]
						([STANDARDPRICE_ITEM_PK]
						,[CRITERION]
						,[CRITERION_TEXT]
						,[PRICE])
					VALUES
						(@ItemPk
						," + RS[0] + @"
						,NULL
						," + RS[1] + @");");
				}
				RS.Close();

			}
		}


		DB.DBCon.Close();

		return Query.ToString(); ;
	}

	[WebMethod]
	public string ConvertStorageOut_5() {
		StringBuilder Query = new StringBuilder();
		List<sStorageItem> Storage = new List<sStorageItem>();
		List<sTransportBody> Body = new List<sTransportBody>();
		string MaxTransportBBPk = "";
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"SELECT MAX(TRANSPORT_PK) FROM [dbo].[TRANSPORT_HEAD];";
		MaxTransportBBPk = DB.SqlCmd.ExecuteScalar() + "";

		DB.SqlCmd.CommandText = @"SELECT top (10000)
			OBSO.[OurBranchStorageOutPk]
			,OBSO.[StorageCode]
			,OBSO.[RequestFormPk]
			,OBSO.[BoxCount]
			,RF.[PackingUnit]
			,RF.[TotalGrossWeight]
			,RF.[TotalVolume]
			,CONVERT(CHAR(19), OBSO.[StockedDate], 120) AS StockedDate
			,OBSO.[TransportBetweenBranchPk]
			,OBSO.[TransportBetweenCompanyPk]
			,OBSO.[StatusCL]
			,OBSO.[Comment]
			,RF.[ShipperPk]
			,RF.[ConsigneePk]
			,RF.[ShipperCode]
			,RF.[ConsigneeCode]
			,SCMP.[CompanyName] AS SCOMPANYNAME
			,CCMP.[CompanyName] AS CCOMPANYNAME
		FROM [dbo].[OurBranchStorageOut] AS OBSO
		LEFT JOIN [dbo].[RequestForm] AS RF ON OBSO.[RequestFormPk] = RF.[RequestFormPk]
		LEFT JOIN [dbo].[Company] AS SCMP ON RF.[ShipperPk] = SCMP.[CompanyPK]
		LEFT JOIN [dbo].[Company] AS CCMP ON RF.[ConsigneePk] = CCMP.[CompanyPk]
		ORDER BY OBSO.[OurBranchStorageOutPk]";

		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			if (RS["StatusCL"].ToString() != "6") {
				Storage.Add(new sStorageItem() {
					Warehouse_Pk = RS["StorageCode"].ToString(),
					Request_Pk = RS["RequestFormPk"].ToString(),
					Packed_Count = RS["BoxCount"].ToString(),
					Packing_Unit = RS["PackingUnit"].ToString(),
					Weight = RS["TotalGrossWeight"].ToString(),
					Volume = RS["TotalVolume"].ToString(),
					Stocked_Date = RS["StockedDate"].ToString(),
					Last_Transport_Head_Pk = RS["TransportBetweenBranchPk"].ToString(),
					Transport_Head_Pk = (Convert.ToInt32(RS["TransportBetweenCompanyPk"].ToString()) + Convert.ToInt32(MaxTransportBBPk)).ToString(),
					Shipper_Company_Pk = RS["ShipperPk"].ToString(),
					Shipper_Company_Code = RS["ShipperCode"].ToString(),
					Shipper_Company_Name = RS["SCOMPANYNAME"].ToString(),
					Consignee_Company_Pk = RS["ConsigneePk"].ToString(),
					Consignee_Company_Code = RS["ConsigneeCode"].ToString(),
					Consignee_Company_Name = RS["CCOMPANYNAME"].ToString()
				});
			}
			else {
				Body.Add(new sTransportBody() {
					Warehouse_Pk_Departure = RS["StorageCode"].ToString(),
					Request_Pk = RS["RequestFormPk"].ToString(),
					Packed_Count = RS["BoxCount"].ToString(),
					Packing_Unit = RS["PackingUnit"].ToString(),
					Weight = RS["TotalGrossWeight"].ToString(),
					Volume = RS["TotalVolume"].ToString(),
					Stocked_Date = RS["StockedDate"].ToString(),
					Last_Transport_Head_Pk = RS["TransportBetweenBranchPk"].ToString(),
					Transport_Head_Pk = (Convert.ToInt32(RS["TransportBetweenCompanyPk"].ToString()) + Convert.ToInt32(MaxTransportBBPk)).ToString(),
					Shipper_Company_Pk = RS["ShipperPk"].ToString(),
					Shipper_Company_Code = RS["ShipperCode"].ToString(),
					Shipper_Company_Name = RS["SCOMPANYNAME"].ToString(),
					Consignee_Company_Pk = RS["ConsigneePk"].ToString(),
					Consignee_Company_Code = RS["ConsigneeCode"].ToString(),
					Consignee_Company_Name = RS["CCOMPANYNAME"].ToString()
				});
			}
		}
		RS.Close();

		for (int i = 0; i < Storage.Count; i++) {
			Query.Append(@"INSERT INTO [dbo].[STORAGE]");
			Query.Append(@"([TRANSPORT_HEAD_PK]
			,[WAREHOUSE_PK]
			,[LAST_TRANSPORT_HEAD_PK]
			,[REQUEST_PK]
			,[SHIPPER_COMPANY_PK]
			,[CONSIGNEE_COMPANY_PK]
			,[SHIPPER_COMPANY_CODE]
			,[CONSIGNEE_COMPANY_CODE]
			,[SHIPPER_COMPANY_NAME]
			,[CONSIGNEE_COMPANY_NAME]
			,[PACKED_COUNT]
			,[PACKING_UNIT]
			,[WEIGHT]
			,[VOLUME]
			,[STOCKED_DATE])");
			Query.Append("VALUES");
			Query.Append("(");
			Query.Append(Common.StringToDB(Storage[i].Transport_Head_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Warehouse_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Last_Transport_Head_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Request_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Shipper_Company_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Consignee_Company_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Shipper_Company_Code, true, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Consignee_Company_Code, true, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Shipper_Company_Name, true, true));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Consignee_Company_Name, true, true));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Packed_Count, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Packing_Unit, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Weight, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Volume, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Storage[i].Stocked_Date, true, false));
			Query.Append(");");
		}

		for (int i = 0; i < Body.Count; i++) {
			Query.Append(@"INSERT INTO [dbo].[TRANSPORT_BODY]");
			Query.Append(@"([TRANSPORT_HEAD_PK]
			,[WAREHOUSE_PK_DEPARTURE]
			,[LAST_TRANSPORT_HEAD_PK]
			,[REQUEST_PK]
			,[SHIPPER_COMPANY_PK]
			,[CONSIGNEE_COMPANY_PK]
			,[SHIPPER_COMPANY_CODE]
			,[CONSIGNEE_COMPANY_CODE]
			,[SHIPPER_COMPANY_NAME]
			,[CONSIGNEE_COMPANY_NAME]
			,[PACKED_COUNT]
			,[PACKING_UNIT]
			,[WEIGHT]
			,[VOLUME]
			,[STOCKED_DATE])");
			Query.Append("VALUES");
			Query.Append("(");
			Query.Append(Common.StringToDB(Body[i].Transport_Head_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Warehouse_Pk_Departure, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Last_Transport_Head_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Request_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Shipper_Company_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Consignee_Company_Pk, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Shipper_Company_Code, true, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Consignee_Company_Code, true, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Shipper_Company_Name, true, true));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Consignee_Company_Name, true, true));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Packed_Count, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Packing_Unit, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Weight, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Volume, false, false));
			Query.Append(",");
			Query.Append(Common.StringToDB(Body[i].Stocked_Date, true, false));
			Query.Append(");");
		}

		DB.DBCon.Close();
		return Query.ToString();
	}

	[WebMethod]
	public string ConvertTransportBC_7() {
		StringBuilder Query = new StringBuilder();
		List<sTransportHead> Head = new List<sTransportHead>();
		List<string> RealAreaTo = new List<string>();
		string MaxTransportBBPk;
		DBConn DB = new DBConn();
		DB.DBCon.Open();

		DB.SqlCmd.CommandText = @"SELECT MAX(TRANSPORT_PK) FROM [dbo].[TRANSPORT_HEAD];";
		MaxTransportBBPk = DB.SqlCmd.ExecuteScalar() + "" ;

		DB.SqlCmd.CommandText = @"SELECT 
			BC.[TransportBetweenCompanyPk]
			,BC.[RequestFormPk]
			,BC.[TransportBBCLPk]
			,BC.[CompanyPk]
			,BC.[Type]
			,BC.[Title]
			,BC.[DriverName]
			,BC.[DriverTEL]
			,BC.[TEL]
			,BC.[CarSize]
			,BC.[FromDate]
			,BC.[ToDate]
			,BC.[WarehouseInfo]
			,BC.[WarehouseMobile]
			,BC.[PackedCount]
			,BC.[PackingUnit]
			,BC.[Weight]
			,BC.[Volume]
			,BC.[DepositWhere]
			,BC.[Price]
			,BC.[DeliveryPrice]
			,BC.[Memo]
			,BC.[StepCL]
			,RF.[ArrivalBranchPk]
			,OBSO.[StorageCode]
			,OBSO.[StatusCL]
		FROM [dbo].[TransportBC] AS BC
		LEFT JOIN [dbo].[RequestForm] AS RF ON BC.[RequestFormPk] = RF.[RequestFormPk]
		LEFT JOIN (SELECT [RequestFormPk], [StorageCode], [StatusCL] FROM [dbo].[OurBranchStorageOut]) AS OBSO ON BC.[RequestFormPk] = OBSO.[RequestFormPk]
		WHERE BC.[TransportBetweenCompanyPk] NOT IN (10, 12)
		ORDER BY BC.[TransportBetweenCompanyPk]";
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			Head.Add(new sTransportHead() {
				Transport_Head_Pk = (Int32.Parse(RS["TransportBetweenCompanyPk"].ToString()) + Int32.Parse(MaxTransportBBPk)).ToString(),
				Transport_Way = "Delivery",
				Transport_Status = RS["StatusCL"].ToString(),
				BranchPk_From = RS["ArrivalBranchPk"].ToString(),
				BranchPk_To = RS["CompanyPk"].ToString(),
				Warehouse_Pk_Arrival = RS["StorageCode"].ToString(),
				Area_From = RS["StorageCode"].ToString(),
				Area_To = RS["WarehouseInfo"].ToString(),
				DateTime_From = RS["FromDate"].ToString(),
				DateTime_To = RS["ToDate"].ToString(),
				Title = RS["Title"].ToString(),
				VesselName = RS["DriverName"].ToString(),
				Voyage_No = RS["Type"].ToString(),
				Value_String_1 = RS["DriverTEL"].ToString(),
				Value_String_2 = RS["TEL"].ToString(),
				Value_String_3 = RS["CarSize"].ToString(),
			});
		}
		RS.Close();

		for (int i = 0; i < Head.Count; i++) {
			string Address = "";
			try {
				Address = Head[i].Area_To.Split(new string[] { "@@" }, StringSplitOptions.None)[0].Split(new string[] { " : " }, StringSplitOptions.None)[1];
			} catch (Exception) {
				Address = Head[i].Area_To.Split(new string[] { "@@" }, StringSplitOptions.None)[0].Split(new string[] { " : " }, StringSplitOptions.None)[0];
			}
			DB.SqlCmd.CommandText = @"SELECT TOP (1) 
				[WarehousePk]
			FROM [dbo].[CompanyWarehouse]
			WHERE [Address] = '" + Address + "'";
			RealAreaTo.Add(DB.SqlCmd.ExecuteScalar().ToString());
		}





		DB.DBCon.Close();
		return Query.ToString();
	}






}