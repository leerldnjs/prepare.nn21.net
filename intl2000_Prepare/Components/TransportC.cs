using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace Components
{
    public class TransportC
    {

        public TransportC() {

        }
		public string Set_TransportHead(sTransportHead THead, ref DBConn DB)
		{
			StringBuilder Query = new StringBuilder();
			GetQuery GQ = new GetQuery();
			string TransportHeadPk = "";

			Query.Append(GQ.TransportHead(THead));
			DB.SqlCmd.CommandText = Query + "";
			TransportHeadPk = DB.SqlCmd.ExecuteScalar() + "";
			return TransportHeadPk;
		}
        public string Set_TransportBody(sTransportBody TBody, ref DBConn DB) {
            return "1";
        }
        public string Set_TransportPacked(sTransportPacked TPacked, ref DBConn DB) {
			StringBuilder Query = new StringBuilder();
			GetQuery GQ = new GetQuery();
			Query.Append(GQ.TransportPacked(TPacked));
			DB.SqlCmd.CommandText = "" + Query;
			DB.SqlCmd.ExecuteNonQuery();

			return "1";
        }

        public sTransportBody Load_TransportBody(string TransportBodyPk, ref DBConn DB) {
            sTransportBody ReturnValue = new sTransportBody();
			DB.SqlCmd.CommandText = @"SELECT
				BODY.[TRANSPORT_BODY_PK]
				,BODY.[TRANSPORT_HEAD_PK]
				,BODY.[TRANSPORT_PACKED_PK]
				,BODY.[WAREHOUSE_PK_DEPARTURE]
				,BODY.[LAST_TRANSPORT_HEAD_PK]
				,BODY.[LAST_TRANSPORT_PACKED_PK]
				,BODY.[REQUEST_PK]
				,BODY.[SHIPPER_COMPANY_PK]
				,BODY.[CONSIGNEE_COMPANY_PK]
				,BODY.[SHIPPER_COMPANY_CODE]
				,BODY.[CONSIGNEE_COMPANY_CODE]
				,BODY.[SHIPPER_COMPANY_NAME]
				,BODY.[CONSIGNEE_COMPANY_NAME]
				,BODY.[PACKED_COUNT]
				,BODY.[PACKING_UNIT]
				,BODY.[DESCRIPTION]
				,BODY.[WEIGHT]
				,BODY.[VOLUME]
				,BODY.[STOCKED_DATE]
				,OBSC.[OurBranchCode]
			FROM [dbo].[TRANSPORT_BODY] AS BODY
			LEFT JOIN [dbo].[OurBranchStorageCode] AS OBSC ON BODY.[WAREHOUSE_PK_DEPARTURE] = OBSC.[OurBranchStoragePk]
			WHERE [TRANSPORT_BODY_PK] = " + TransportBodyPk;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Transport_Body_Pk = RS["TRANSPORT_BODY_PK"] + "";
				ReturnValue.Transport_Head_Pk = RS["TRANSPORT_HEAD_PK"] + "";
				ReturnValue.Transport_Packed_Pk = RS["TRANSPORT_PACKED_PK"] + "";
				ReturnValue.Warehouse_Pk_Departure = RS["WAREHOUSE_PK_DEPARTURE"] + "";
				ReturnValue.Last_Transport_Head_Pk = RS["LAST_TRANSPORT_HEAD_PK"] + "";
				ReturnValue.Last_Transport_Pacekd_Pk = RS["LAST_TRANSPORT_PACKED_PK"] + "";
				ReturnValue.Request_Pk = RS["REQUEST_PK"] + "";
				ReturnValue.Shipper_Company_Pk = RS["SHIPPER_COMPANY_PK"] + "";
				ReturnValue.Consignee_Company_Pk = RS["CONSIGNEE_COMPANY_PK"] + "";
				ReturnValue.Shipper_Company_Name = RS["SHIPPER_COMPANY_NAME"] + "";
				ReturnValue.Consignee_Company_Name = RS["CONSIGNEE_COMPANY_NAME"] + "";
				ReturnValue.Shipper_Company_Code = RS["SHIPPER_COMPANY_CODE"] + "";
				ReturnValue.Consignee_Company_Code = RS["CONSIGNEE_COMPANY_CODE"] + "";
				ReturnValue.Packed_Count = RS["PACKED_COUNT"] + "";
				ReturnValue.Packing_Unit = RS["PACKING_UNIT"] + "";
				ReturnValue.Description = RS["DESCRIPTION"] + "";
				ReturnValue.Weight = RS["WEIGHT"] + "";
				ReturnValue.Volume = RS["VOLUME"] + "";
				ReturnValue.Stocked_Date = RS["STOCKED_DATE"] + "";
				ReturnValue.Warehouse_Branch_Pk = RS["OurBranchCode"] + "";
			}
			RS.Close();

            return ReturnValue;
        }

		public sStorageItem Load_StorageItem(string StoragePk, ref DBConn DB) {
			sStorageItem ReturnValue = new sStorageItem();
			DB.SqlCmd.CommandText = @"SELECT
				WH.[STORAGE_PK]
				,WH.[TRANSPORT_HEAD_PK]
				,WH.[TRANSPORT_PACKED_PK]
				,WH.[WAREHOUSE_PK]
				,WH.[LAST_TRANSPORT_HEAD_PK]
				,WH.[LAST_TRANSPORT_PACKED_PK]
				,WH.[REQUEST_PK]
				,WH.[SHIPPER_COMPANY_PK]
				,WH.[CONSIGNEE_COMPANY_PK]
				,WH.[SHIPPER_COMPANY_CODE]
				,WH.[CONSIGNEE_COMPANY_CODE]
				,WH.[SHIPPER_COMPANY_NAME]
				,WH.[CONSIGNEE_COMPANY_NAME]
				,WH.[PACKED_COUNT]
				,WH.[PACKING_UNIT]
				,WH.[DESCRIPTION]
				,WH.[WEIGHT]
				,WH.[VOLUME]
				,WH.[STOCKED_DATE]
				,OBSC.[OurBranchCode]
			FROM [dbo].[STORAGE] AS WH
			LEFT JOIN [dbo].[OurBranchStorageCode] AS OBSC ON WH.[WAREHOUSE_PK] = OBSC.[OurBranchStoragePk]
			WHERE [STORAGE_PK] = " + StoragePk;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Storage_Pk = RS["STORAGE_PK"] + "";
				ReturnValue.Transport_Head_Pk = RS["TRANSPORT_HEAD_PK"] + "";
				ReturnValue.Transport_Packed_Pk = RS["TRANSPORT_PACKED_PK"] + "";
				ReturnValue.Warehouse_Pk = RS["WAREHOUSE_PK"] + "";
				ReturnValue.Last_Transport_Head_Pk = RS["LAST_TRANSPORT_HEAD_PK"] + "";
				ReturnValue.Last_Transport_Pacekd_Pk = RS["LAST_TRANSPORT_PACKED_PK"] + "";
				ReturnValue.Request_Pk = RS["REQUEST_PK"] + "";
				ReturnValue.Shipper_Company_Pk = RS["SHIPPER_COMPANY_PK"] + "";
				ReturnValue.Consignee_Company_Pk = RS["CONSIGNEE_COMPANY_PK"] + "";
				ReturnValue.Shipper_Company_Name = RS["SHIPPER_COMPANY_NAME"] + "";
				ReturnValue.Consignee_Company_Name = RS["CONSIGNEE_COMPANY_NAME"] + "";
				ReturnValue.Shipper_Company_Code = RS["SHIPPER_COMPANY_CODE"] + "";
				ReturnValue.Consignee_Company_Code = RS["CONSIGNEE_COMPANY_CODE"] + "";
				ReturnValue.Packed_Count = RS["PACKED_COUNT"] + "";
				ReturnValue.Packing_Unit = RS["PACKING_UNIT"] + "";
				ReturnValue.Description = RS["DESCRIPTION"] + "";
				ReturnValue.Weight = RS["WEIGHT"] + "";
				ReturnValue.Volume = RS["VOLUME"] + "";
				ReturnValue.Stocked_Date = RS["STOCKED_DATE"] + "";
				ReturnValue.Warehouse_Branch_Pk = RS["OurBranchCode"] + "";
			}
			RS.Close();

			return ReturnValue;
		}

		public sTransportPacked Load_TransportPackedOnly(string Transport_Packed_Pk, ref DBConn DB) {
			DB.SqlCmd.CommandText = @"SELECT
			[TRANSPORT_PACKED_PK]
			,[SEQ]
			,[TRANSPORT_HEAD_PK]
			,[COMPANY_PK_OWNER]
			,[CONTAINER_COMPANY]
			,[TYPE]
			,[NO]
			,[SIZE]
			,[SEAL_NO]
		FROM [dbo].[TRANSPORT_PACKED]
		WHERE [TRANSPORT_PACKED_PK] = " + Transport_Packed_Pk;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			sTransportPacked ReturnValue = new sTransportPacked();

			if (RS.Read()) {
				ReturnValue.Transport_Packed_Pk = RS["TRANSPORT_PACKED_PK"] + "";
				ReturnValue.Seq = RS["SEQ"] + "";
				ReturnValue.Transport_Head_Pk = RS["TRANSPORT_HEAD_PK"] + "";
				ReturnValue.Company_Pk_Owner = RS["COMPANY_PK_OWNER"] + "";
				ReturnValue.Container_Company = RS["CONTAINER_COMPANY"] + "";
				ReturnValue.Type = RS["TYPE"] + "";
				ReturnValue.No = RS["NO"] + "";
				ReturnValue.Size = RS["SIZE"] + "";
				ReturnValue.Seal_No = RS["SEAL_NO"] + "";
			}
			RS.Close();
			return ReturnValue;
		}

        public List<sTransportPacked> Load_TransportPacked(string Transport_Head_Pk, ref DBConn DB) {
            DB.SqlCmd.CommandText = @"
            SELECT TPacked.[TRANSPORT_PACKED_PK]
      ,TPacked.[SEQ]
      ,TPacked.[TRANSPORT_HEAD_PK]
      ,TPacked.[COMPANY_PK_OWNER]
      ,TPacked.[CONTAINER_COMPANY]
      ,TPacked.[TYPE]
      ,TPacked.[NO]
      ,TPacked.[SIZE]
      ,TPacked.[SEAL_NO]
	  ,TBody.[TRANSPORT_BODY_PK]
      ,TBody.[WAREHOUSE_PK_DEPARTURE]
      ,TBody.[REQUEST_PK]
      ,TBody.[SHIPPER_COMPANY_PK]
      ,TBody.[CONSIGNEE_COMPANY_PK]
      ,TBody.[SHIPPER_COMPANY_CODE]
      ,TBody.[CONSIGNEE_COMPANY_CODE]
      ,TBody.[SHIPPER_COMPANY_NAME]
      ,TBody.[CONSIGNEE_COMPANY_NAME]
      ,TBody.[PACKED_COUNT]
      ,TBody.[PACKING_UNIT]
      ,TBody.[DESCRIPTION]
      ,TBody.[WEIGHT]
      ,TBody.[VOLUME]
        FROM [dbo].[TRANSPORT_PACKED] AS TPacked
			left join [dbo].[TRANSPORT_BODY] AS TBody ON TPacked.TRANSPORT_PACKED_PK=TBody.TRANSPORT_PACKED_PK 
		WHERE TPacked.TRANSPORT_HEAD_PK= " + Transport_Head_Pk + " ORDER BY TPacked.[TRANSPORT_PACKED_PK];";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();

			List<sTransportPacked> ReturnValue = new List<sTransportPacked>();
			sTransportPacked temp = new sTransportPacked();
			string CurrentPackedPk = "";
			while (RS.Read()) {
				if (CurrentPackedPk != RS["TRANSPORT_PACKED_PK"] + "") {
					if (CurrentPackedPk != "") {
						ReturnValue.Add(temp);
					}
					temp = new sTransportPacked();
					temp.Transport_Packed_Pk = RS["TRANSPORT_PACKED_PK"] + "";
					temp.Seq = RS["SEQ"] + "";
					temp.Transport_Head_Pk = RS["TRANSPORT_HEAD_PK"] + "";
					temp.Company_Pk_Owner = RS["COMPANY_PK_OWNER"] + "";
					temp.Container_Company = RS["CONTAINER_COMPANY"] + "";
					temp.Type = RS["TYPE"] + "";
					temp.No = RS["NO"] + "";
					temp.Size = RS["SIZE"] + "";
					temp.Seal_No = RS["SEAL_NO"] + "";
					temp.arrBody = new List<sTransportBody>();
				}

				temp.arrBody.Add(new sTransportBody() {
					Transport_Body_Pk = RS["TRANSPORT_BODY_PK"] + "",
					Warehouse_Pk_Departure = RS["WAREHOUSE_PK_DEPARTURE"] + "",
					Request_Pk = RS["REQUEST_PK"] + "",
					Shipper_Company_Pk = RS["SHIPPER_COMPANY_PK"] + "",
					Consignee_Company_Pk = RS["CONSIGNEE_COMPANY_PK"] + "",
					Shipper_Company_Code = RS["SHIPPER_COMPANY_CODE"] + "",
					Consignee_Company_Code = RS["CONSIGNEE_COMPANY_CODE"] + "",
					Shipper_Company_Name = RS["SHIPPER_COMPANY_NAME"] + "",
					Consignee_Company_Name = RS["CONSIGNEE_COMPANY_NAME"] + "",
					Packed_Count = RS["PACKED_COUNT"] + "",
					Packing_Unit = RS["PACKING_UNIT"] + "",
					Description = RS["DESCRIPTION"] + "",
					Weight = RS["WEIGHT"] + "",
					Volume = RS["VOLUME"] + "",
					Transport_Head_Pk = temp.Transport_Head_Pk
				});
			}

			RS.Close();
			return ReturnValue;
		}

        public string Delete_TransportHead(string Transport_Pk, ref DBConn DB) {
            DB.SqlCmd.CommandText = @"
            DELETE FROM [dbo].[TRANSPORT_HEAD] WHERE [TRANSPORT_PK] = " + Transport_Pk + ";";
            DB.SqlCmd.ExecuteNonQuery();
            return "1";
        }
        public string Delete_TransportBody(string Transport_Body_Pk, ref DBConn DB) {
            return "1";
        }
        public string Delete_TransportPacked(string Transport_Packed_Pk, ref DBConn DB) {
            DB.SqlCmd.CommandText = @"
            DELETE FROM [dbo].[TRANSPORT_PACEKD] WHERE [TRANSPORT_PACKED_PK] = " + Transport_Packed_Pk + ";";
            DB.SqlCmd.ExecuteNonQuery();
            return "1";
        }
		public List<sTransportHead> LoadList_TransportHead_WithPacked(string BranchPk, string Status, ref DBConn DB) {
			DB.SqlCmd.CommandText = @"SELECT
			TH.[TRANSPORT_PK]
            ,TH.[TRANSPORT_WAY]
            ,TH.[TRANSPORT_STATUS]
            ,CP.[CompanyName] AS BRANCHPK_FROM
            ,TH.[BRANCHPK_TO]
            ,TH.[WAREHOUSE_PK_ARRIVAL]
            ,TH.[AREA_FROM]
            ,TH.[AREA_TO]
            ,TH.[DATETIME_FROM]
            ,TH.[DATETIME_TO]
            ,TH.[TITLE]
            ,TH.[VESSELNAME]
            ,TH.[VOYAGE_NO]
            ,TH.[VALUE_STRING_0]
            ,TH.[VALUE_STRING_1]
            ,TH.[VALUE_STRING_2]
            ,TH.[VALUE_STRING_3]
            ,TH.[VALUE_STRING_4]
            ,TH.[VALUE_STRING_5]
			,TP.[TRANSPORT_PACKED_PK]
			,TP.[SEQ]
			,TP.[TRANSPORT_HEAD_PK]
			,TP.[COMPANY_PK_OWNER]
			,CPTP.[CompanyCode]
			,LEFT(WHCD.[StorageName], 8) AS StorageName
			,TP.[CONTAINER_COMPANY]
			,TP.[TYPE]
			,TP.[NO]
			,TP.[SIZE]
			,TP.[SEAL_NO]
        FROM [dbo].[TRANSPORT_HEAD] AS TH 
		LEFT JOIN [dbo].[TRANSPORT_PACKED] AS TP ON TH.TRANSPORT_PK=TP.TRANSPORT_HEAD_PK 
		LEFT JOIN [dbo].[Company] AS CP ON TH.BRANCHPK_FROM = CP.CompanyPk
		LEFT JOIN [dbo].[Company] AS CPTP ON TP.[COMPANY_PK_OWNER] = CPTP.[CompanyPk]
		LEFT JOIN [dbo].[OurBranchStorageCode] AS WHCD ON TP.[WAREHOUSE_PK] = WHCD.[OurBranchStoragePk]
		WHERE TH.[BRANCHPK_FROM] = " + BranchPk + @"
		AND TH.[TRANSPORT_STATUS] = " + Status + @"
		ORDER BY [DATETIME_TO] DESC, [TRANSPORT_WAY], TH.[TRANSPORT_PK] ;";

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();

			List<sTransportHead> ReturnValue = new List<sTransportHead>();
			sTransportHead Current = new sTransportHead() {
				Transport_Head_Pk = "",
				arrPacked = new List<sTransportPacked>()
			};

			while (RS.Read()) {
				if (Current.Transport_Head_Pk != "" && Current.Transport_Head_Pk != RS["TRANSPORT_PK"] + "") {
					ReturnValue.Add(Current);
					Current = new sTransportHead() {
						Transport_Head_Pk = "",
						arrPacked = new List<sTransportPacked>()
					};
				}

				if (Current.Transport_Head_Pk == "") {
					Current.Transport_Head_Pk = RS["TRANSPORT_PK"] + "";
					Current.Transport_Way = RS["TRANSPORT_WAY"] + "";
					Current.Transport_Status = RS["TRANSPORT_STATUS"] + "";
					Current.BranchPk_From = RS["BRANCHPK_FROM"] + "";
					Current.BranchPk_To = RS["BRANCHPK_TO"] + "";
					Current.Warehouse_Pk_Arrival = RS["WAREHOUSE_PK_ARRIVAL"] + "";
					Current.Area_From = RS["AREA_FROM"] + "";
					Current.Area_To = RS["AREA_TO"] + "";
					Current.DateTime_From = RS["DATETIME_FROM"] + "";
					Current.DateTime_To = RS["DATETIME_TO"] + "";
					Current.Title = RS["TITLE"] + "";
					Current.VesselName = RS["VESSELNAME"] + "";
					Current.Voyage_No = RS["VOYAGE_NO"] + "";
					Current.Value_String_0 = RS["VALUE_STRING_0"] + "";
					Current.Value_String_1 = RS["VALUE_STRING_1"] + "";
					Current.Value_String_2 = RS["VALUE_STRING_2"] + "";
					Current.Value_String_3 = RS["VALUE_STRING_3"] + "";
					Current.Value_String_4 = RS["VALUE_STRING_4"] + "";
					Current.Value_String_5 = RS["VALUE_STRING_5"] + "";
				}

				if (RS["TRANSPORT_PACKED_PK"] + "" != "") {
					Current.arrPacked.Add(new sTransportPacked() {
						Transport_Packed_Pk = RS["TRANSPORT_PACKED_PK"] + "",
						Seq = RS["SEQ"] + "",
						Transport_Head_Pk = RS["TRANSPORT_HEAD_PK"] + "",
						Company_Pk_Owner = RS["COMPANY_PK_OWNER"] + "",
						Company_Code_Owner = RS["CompanyCode"] + "",
						WareHouse_Name = RS["StorageName"] + "",
						Container_Company = RS["CONTAINER_COMPANY"] + "",
						Type = RS["TYPE"] + "",
						No = RS["NO"] + "",
						Size = RS["SIZE"] + "",
						Seal_No = RS["SEAL_NO"] + ""
					});
				}
			}
			if (Current.Transport_Head_Pk != "") {
				ReturnValue.Add(Current);
			}
			RS.Close();
			return ReturnValue;
		}
		public List<sTransportHead> LoadList_TransportHead(string BranchPk, string Status, ref DBConn DB) {
			DB.SqlCmd.CommandText = @"SELECT
			[TRANSPORT_PK]
            ,[TRANSPORT_WAY]
            ,[TRANSPORT_STATUS]
            ,CP.[CompanyName] AS BRANCHPK_FROM
            ,[BRANCHPK_TO]
            ,[WAREHOUSE_PK_ARRIVAL]
            ,[AREA_FROM]
            ,[AREA_TO]
            ,[DATETIME_FROM]
            ,[DATETIME_TO]
            ,[TITLE]
            ,[VESSELNAME]
            ,[VOYAGE_NO]
            ,[VALUE_STRING_0]
            ,[VALUE_STRING_1]
            ,[VALUE_STRING_2]
            ,[VALUE_STRING_3]
            ,[VALUE_STRING_4]
            ,[VALUE_STRING_5]
        FROM [dbo].[TRANSPORT_HEAD] AS TH
		LEFT JOIN [dbo].[Company] AS CP ON TH.BRANCHPK_FROM = CP.CompanyPk
		WHERE [BRANCHPK_FROM] = " + BranchPk + @"
		AND [TRANSPORT_STATUS] = " + Status + @"
		ORDER BY [DATETIME_TO] DESC, [TRANSPORT_WAY] ;";

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();

			List<sTransportHead> ReturnValue = new List<sTransportHead>();
			while (RS.Read()) {
				sTransportHead temp = new sTransportHead();
				temp.Transport_Head_Pk = RS["TRANSPORT_PK"] + "";
				temp.Transport_Way = RS["TRANSPORT_WAY"] + "";
				temp.Transport_Status = RS["TRANSPORT_STATUS"] + "";
				temp.BranchPk_From = RS["BRANCHPK_FROM"] + "";
				temp.BranchPk_To = RS["BRANCHPK_TO"] + "";
				temp.Warehouse_Pk_Arrival = RS["WAREHOUSE_PK_ARRIVAL"] + "";
				temp.Area_From = RS["AREA_FROM"] + "";
				temp.Area_To = RS["AREA_TO"] + "";
				temp.DateTime_From = RS["DATETIME_FROM"] + "";
				temp.DateTime_To = RS["DATETIME_TO"] + "";
				temp.Title = RS["TITLE"] + "";
				temp.VesselName = RS["VESSELNAME"] + "";
				temp.Voyage_No = RS["VOYAGE_NO"] + "";
				temp.Value_String_0 = RS["VALUE_STRING_0"] + "";
				temp.Value_String_1 = RS["VALUE_STRING_1"] + "";
				temp.Value_String_2 = RS["VALUE_STRING_2"] + "";
				temp.Value_String_3 = RS["VALUE_STRING_3"] + "";
				temp.Value_String_4 = RS["VALUE_STRING_4"] + "";
				temp.Value_String_5 = RS["VALUE_STRING_5"] + "";
				ReturnValue.Add(temp);
			}
			RS.Close();
			return ReturnValue;
		}

		public List<sTransportPacked> LoadList_TransportPacked(string Type, string TypePk, string WareHousePk, ref DBConn DB) {
            List<sTransportPacked> ReturnValue = new List<sTransportPacked>();
			sTransportPacked temp = new sTransportPacked();
			string QueryWhere = "";

			switch (Type) {
				case "TransportHeadPk":
					QueryWhere = " WHERE PACKED.[TRANSPORT_HEAD_PK] = " + TypePk;
					break;
				case "BranchPk":
					if (WareHousePk == "") {
						QueryWhere = " WHERE PACKED.[TRANSPORT_HEAD_PK] IS NULL AND WHCD.[OurBranchCode] = " + TypePk;
					}
					else {
						QueryWhere = " WHERE PACKED.[TRANSPORT_HEAD_PK] IS NULL AND PACKED.[WAREHOUSE_PK] = " + WareHousePk;
					}
					break;
			}

			DB.SqlCmd.CommandText = @"SELECT 
				PACKED.[TRANSPORT_PACKED_PK]
				,PACKED.[SEQ]
				,PACKED.[WAREHOUSE_PK]
				,LEFT(WHCD.[StorageName], 8) AS StorageName
				,PACKED.[TRANSPORT_HEAD_PK]
				,PACKED.[COMPANY_PK_OWNER]
				,COM.[CompanyCode]
				,PACKED.[CONTAINER_COMPANY]
				,PACKED.[TYPE]
				,PACKED.[NO]
				,PACKED.[SIZE]
				,PACKED.[SEAL_NO]
			FROM [dbo].[TRANSPORT_PACKED] AS PACKED
			LEFT JOIN [dbo].[Company] AS COM ON PACKED.[COMPANY_PK_OWNER] = COM.[CompanyPk]
			LEFT JOIN [dbo].[OurBranchStorageCode] AS WHCD ON PACKED.[WAREHOUSE_PK] = WHCD.[OurBranchStoragePk] " + QueryWhere;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();

			while (RS.Read()) {
				temp.Transport_Packed_Pk = RS["TRANSPORT_PACKED_PK"] + "";
				temp.Seq = RS["SEQ"] + "";
				temp.WareHouse_Pk = RS["WAREHOUSE_PK"] + "";
				temp.WareHouse_Name = RS["StorageName"] + "";
				temp.Transport_Head_Pk = RS["TRANSPORT_HEAD_PK"] + "";
				temp.Company_Pk_Owner = RS["COMPANY_PK_OWNER"] + "";
				temp.Company_Code_Owner = RS["CompanyCode"] + "";
				temp.Container_Company = RS["CONTAINER_COMPANY"] + "";
				temp.Type = RS["TYPE"] + "";
				temp.No = RS["NO"] + "";
				temp.Size = RS["SIZE"] + "";
				temp.Seal_No = RS["SEAL_NO"] + "";
				ReturnValue.Add(temp);
			}
			RS.Close();
			return ReturnValue;
		}
		
        
        public List<sStorageItem> LoadList_StorageItem(string WarehousePk, ref DBConn DB) {
			DB.SqlCmd.CommandText = @"SELECT [STORAGE_PK]
				,[TRANSPORT_HEAD_PK]
				,[TRANSPORT_PACKED_PK]
				,[WAREHOUSE_PK]
				,WH.[StorageName] AS [WAREHOUSE_NAME]
				,[LAST_TRANSPORT_HEAD_PK]
				,[LAST_TRANSPORT_PACKED_PK]
				,[REQUEST_PK]
				,[SHIPPER_COMPANY_PK]
				,[CONSIGNEE_COMPANY_PK]
				,[SHIPPER_COMPANY_CODE]
				,[CONSIGNEE_COMPANY_CODE]
				,[SHIPPER_COMPANY_NAME]
				,[CONSIGNEE_COMPANY_NAME]
				,[PACKED_COUNT]
				,[PACKING_UNIT]
				,SR.[DESCRIPTION] AS [DESCRIPTION]
				,[WEIGHT]
				,[VOLUME]
				,RC.[Name] AS [AREA_FROM]
				,RF.[DepartureDate] AS [DEPATURE_DATE]
				,RF.[ArrivalDate] AS [ARRIVAL_DATE]
				,RF.[TransportWayCL] AS [TRANSPORT_WAY]
			FROM [dbo].[STORAGE] AS SR
			LEFT JOIN [dbo].[RequestForm] AS RF ON SR.REQUEST_PK = RF.RequestFormPk
			INNER JOIN [dbo].[RegionCode] AS RC ON RF.[DepartureRegionCode] = RC.[RegionCode]
			INNER JOIN [dbo].[OurBranchStorageCode] AS WH ON SR.[WAREHOUSE_PK] = WH.[OurBranchStoragePk]
			WHERE [WAREHOUSE_PK] = " + WarehousePk ;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			List<sStorageItem> ReturnValue = new List<sStorageItem>();

			while (RS.Read()) {
				sStorageItem temp = new sStorageItem();
				temp.Storage_Pk = RS["STORAGE_PK"] + "";
				temp.Transport_Head_Pk = RS["TRANSPORT_HEAD_PK"] + "";
				temp.Transport_Packed_Pk = RS["TRANSPORT_PACKED_PK"] + "";
				temp.Warehouse_Pk = RS["WAREHOUSE_PK"] + "";
				temp.Warehouse_Name = RS["WAREHOUSE_NAME"] + "";
				temp.Last_Transport_Head_Pk = RS["LAST_TRANSPORT_HEAD_PK"] + "";
				temp.Last_Transport_Pacekd_Pk = RS["LAST_TRANSPORT_PACKED_PK"] + "";
				temp.Request_Pk = RS["REQUEST_PK"] + "";
				temp.Shipper_Company_Pk = RS["SHIPPER_COMPANY_PK"] + "";
				temp.Consignee_Company_Pk = RS["CONSIGNEE_COMPANY_PK"] + "";
				temp.Shipper_Company_Code = RS["SHIPPER_COMPANY_CODE"] + "";
				temp.Consignee_Company_Code = RS["CONSIGNEE_COMPANY_CODE"] + "";
				temp.Shipper_Company_Name = RS["SHIPPER_COMPANY_NAME"] + "";
				temp.Consignee_Company_Name = RS["CONSIGNEE_COMPANY_NAME"] + "";
				temp.Packed_Count = RS["PACKED_COUNT"] + "";
				temp.Packing_Unit = RS["PACKING_UNIT"] + "";
				temp.Description = RS["DESCRIPTION"] + "";
				temp.Weight = RS["WEIGHT"] + "";
				temp.Volume = RS["VOLUME"] + "";
				temp.Req_Area_From = RS["AREA_FROM"] + "";
				temp.Req_DateTime_From = RS["DEPATURE_DATE"] + "";
				temp.Req_DateTime_To = RS["ARRIVAL_DATE"] + "";
				temp.Req_Transport_Way = RS["TRANSPORT_WAY"] + "";
				ReturnValue.Add(temp);
			}
			RS.Close();

            return ReturnValue;
        }

		public sTransportHead Load_TransportHead(string Transport_Pk, ref DBConn DB) {
			sTransportHead Head = new sTransportHead();
			DB.SqlCmd.CommandText = @"
            SELECT [TRANSPORT_PK]
            ,[TRANSPORT_WAY]
            ,[TRANSPORT_STATUS]
            ,[BRANCHPK_FROM]
            ,[BRANCHPK_TO]
			,[WAREHOUSE_PK_ARRIVAL]
            ,[AREA_FROM]
            ,[AREA_TO]
            ,[DATETIME_FROM]
            ,[DATETIME_TO]
            ,[TITLE]
            ,[VESSELNAME]
            ,[VOYAGE_NO]
            ,[VALUE_STRING_0]
            ,[VALUE_STRING_1]
            ,[VALUE_STRING_2]
            ,[VALUE_STRING_3]
            ,[VALUE_STRING_4]
            ,[VALUE_STRING_5]
        FROM [dbo].[TRANSPORT_HEAD]
        WHERE [TRANSPORT_PK] = " + Transport_Pk;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();

			if (RS.Read()) {
				Head.Transport_Head_Pk = Transport_Pk;
				Head.Transport_Way = RS["TRANSPORT_WAY"] + "";
				Head.Transport_Status = RS["TRANSPORT_STATUS"] + "";
				Head.BranchPk_From = RS["BRANCHPK_FROM"] + "";
				Head.BranchPk_To = RS["BRANCHPK_TO"] + "";
				Head.Warehouse_Pk_Arrival = RS["WAREHOUSE_PK_ARRIVAL"] + "";
				Head.Area_From = RS["AREA_FROM"] + "";
				Head.Area_To = RS["AREA_TO"] + "";
				Head.DateTime_From = RS["DATETIME_FROM"] + "";
				Head.DateTime_To = RS["DATETIME_TO"] + "";
				Head.Title = RS["TITLE"] + "";
				Head.VesselName = RS["VESSELNAME"] + "";
				Head.Voyage_No = RS["VOYAGE_NO"] + "";
				Head.Value_String_0 = RS["VALUE_STRING_0"] + "";
				Head.Value_String_1 = RS["VALUE_STRING_1"] + "";
				Head.Value_String_2 = RS["VALUE_STRING_2"] + "";
				Head.Value_String_3 = RS["VALUE_STRING_3"] + "";
				Head.Value_String_4 = RS["VALUE_STRING_4"] + "";
				Head.Value_String_5 = RS["VALUE_STRING_5"] + "";
			}
			RS.Close();

			DB.SqlCmd.CommandText = @"SELECT [TRANSPORT_PACKED_PK]
			,[SEQ]
			,[TRANSPORT_HEAD_PK]
			,[COMPANY_PK_OWNER]
			,CP.[CompanyCode]
			,[CONTAINER_COMPANY]
			,[TYPE]
			,[NO]
			,[SIZE]
			,[SEAL_NO]
		FROM [dbo].[TRANSPORT_PACKED] AS TP
		LEFT JOIN [dbo].[Company] AS CP ON TP.[COMPANY_PK_OWNER] = CP.[CompanyPk]
		WHERE [TRANSPORT_HEAD_PK] = " + Transport_Pk;

			sTransportPacked temp = new sTransportPacked();
			SqlDataReader RS2 = DB.SqlCmd.ExecuteReader();
			Head.arrPacked = new List<sTransportPacked>();
			while (RS2.Read()) {
				temp.Transport_Packed_Pk = RS2["TRANSPORT_PACKED_PK"] + "";
				temp.Seq = RS2["SEQ"] + "";
				temp.Transport_Head_Pk = RS2["TRANSPORT_HEAD_PK"] + "";
				temp.Company_Pk_Owner = RS2["COMPANY_PK_OWNER"] + "";
				temp.Company_Code_Owner = RS2["CompanyCode"] + "";
				temp.Container_Company = RS2["CONTAINER_COMPANY"] + "";
				temp.Type = RS2["TYPE"] + "";
				temp.No = RS2["NO"] + "";
				temp.Size = RS2["SIZE"] + "";
				temp.Seal_No = RS2["SEAL_NO"] + "";
				Head.arrPacked.Add(temp);
			}
			RS2.Close();

			//Head.arrPacked = Load_TransportPacked(Transport_Pk, ref DB);
			//Head.arrPacked = LoadList_TransportPacked("TransportHeadPk", Transport_Pk, ref DB);
			//Head.arrBody = LoadList_TransportBody("TransportHeadPk", Transport_Pk, ref DB);
			return Head;
		}

		public List<sTransportBody> LoadList_TransportBody(string Type, string TypePk, ref DBConn DB) {
			string QueryWhere;
			if (Type == "TransportHeadPk") {
				QueryWhere = "WHERE [TRANSPORT_HEAD_PK] = " + TypePk;
			}
			else {  //Type == "TransportPackedPk"
				QueryWhere = "WHERE [TRANSPORT_PACKED_PK] = " + TypePk;
			}

			sTransportBody temp = new sTransportBody();
			List<sTransportBody> ReturnValue = new List<sTransportBody>();
			DB.SqlCmd.CommandText = @"SELECT [TRANSPORT_BODY_PK]
			,[TRANSPORT_HEAD_PK]
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
			,[VOLUME]
			,RC.[Name] AS [AREA_FROM]
			,RF.[DepartureDate] AS [DEPARTURE_DATE]
			,RF.[ArrivalDate] AS [ARRIVAL_DATE]
			,RF.[TransportWayCL] AS [TRANSPORT_WAY]
		FROM [dbo].[TRANSPORT_BODY] AS TB
		LEFT JOIN [dbo].[RequestForm] AS RF ON TB.REQUEST_PK = RF.RequestFormPk
		INNER JOIN [dbo].[RegionCode] AS RC ON RF.[DepartureRegionCode] = RC.[RegionCode]" + QueryWhere;
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();

			while (RS.Read()) {
				temp.Transport_Body_Pk = RS["TRANSPORT_BODY_PK"] + "";
				temp.Transport_Head_Pk = RS["TRANSPORT_HEAD_PK"] + "";
				temp.Transport_Packed_Pk = RS["TRANSPORT_PACKED_PK"] + "";
				temp.Warehouse_Pk_Departure = RS["WAREHOUSE_PK_DEPARTURE"] + "";
				temp.Request_Pk = RS["REQUEST_PK"] + "";
				temp.Shipper_Company_Pk = RS["SHIPPER_COMPANY_PK"] + "";
				temp.Consignee_Company_Pk = RS["CONSIGNEE_COMPANY_PK"] + "";
				temp.Shipper_Company_Code = RS["SHIPPER_COMPANY_CODE"] + "";
				temp.Consignee_Company_Code = RS["CONSIGNEE_COMPANY_CODE"] + "";
				temp.Shipper_Company_Name = RS["SHIPPER_COMPANY_NAME"] + "";
				temp.Consignee_Company_Name = RS["CONSIGNEE_COMPANY_NAME"] + "";
				temp.Packed_Count = RS["PACKED_COUNT"] + "";
				temp.Packing_Unit = RS["PACKING_UNIT"] + "";
				temp.Description = RS["DESCRIPTION"] + "";
				temp.Weight = RS["WEIGHT"] + "";
				temp.Volume = RS["VOLUME"] + "";
				temp.Req_Area_From = RS["AREA_FROM"] + "";
				temp.Req_DateTime_From = RS["DEPARTURE_DATE"] + "";
				temp.Req_DateTime_To = RS["ARRIVAL_DATE"] + "";
				temp.Req_Transport_Way = RS["TRANSPORT_WAY"] + "";
				ReturnValue.Add(temp);
			}
			RS.Close();
			return ReturnValue;
		}

		public string StorageAddCount(string StoragePk, int Count, ref DBConn DB) {
			string Query = @"
DECLARE @LastCount int; 
DECLARE @ReturnValue int; 

SELECT @LastCount=[PACKED_COUNT] " + "+" + Count + @" 
  FROM [dbo].[STORAGE]
  WHERE STORAGE_PK=" + StoragePk + @";

IF (@LastCount>0)
BEGIN
	UPDATE [dbo].[STORAGE] SET [PACKED_COUNT]=@LastCount WHERE STORAGE_PK=" + StoragePk + @";
	SET @ReturnValue=1;
END
ELSE
BEGIN 
	IF( @LastCount=0)
	BEGIN
		DELETE [dbo].[STORAGE] WHERE STORAGE_PK=" + StoragePk + @";
		SET @ReturnValue=1;
	END
	ELSE 
	BEGIN
		SET @ReturnValue=0;
	END
END
SELECT @ReturnValue;";
			DB.SqlCmd.CommandText = Query;
			string ReturnValue = DB.SqlCmd.ExecuteScalar()+"";
			return ReturnValue;
		}

		public string BodyAddCount(string TransportBodyPk, int Count, ref DBConn DB) {
			string Query = @"
DECLARE @LastCount int; 
DECLARE @ReturnValue int; 

SELECT @LastCount=[PACKED_COUNT] " + "+" + Count + @" 
  FROM [dbo].[TRANSPORT_BODY]
  WHERE [TRANSPORT_BODY_PK]=" + TransportBodyPk + @";

IF (@LastCount>0)
BEGIN
	UPDATE [dbo].[TRANSPORT_BODY] SET [PACKED_COUNT]=@LastCount WHERE [TRANSPORT_BODY_PK]=" + TransportBodyPk + @";
	SET @ReturnValue=1;
END
ELSE
BEGIN 
	IF( @LastCount=0)
	BEGIN
		DELETE [dbo].[TRANSPORT_BODY] WHERE [TRANSPORT_BODY_PK]=" + TransportBodyPk + @";
		SET @ReturnValue=1;
	END
	ELSE 
	BEGIN
		SET @ReturnValue=0;
	END
END
SELECT @ReturnValue;";
			DB.SqlCmd.CommandText = Query;
			string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
			return ReturnValue;
        }


		public string Find_BodyPk(string StoragePk, string Type, string TypePk, ref DBConn DB) {
			DB.SqlCmd.CommandText = "SELECT [REQUEST_PK] FROM [dbo].[STORAGE] WHERE [STORAGE_PK] = " + StoragePk + ";";
			string RequestPk = DB.SqlCmd.ExecuteScalar() + "";

			if (Type == "Head") {
				DB.SqlCmd.CommandText = "SELECT [TRANSPORT_BODY_PK] FROM [dbo].[TRANSPORT_BODY] WHERE [TRANSPORT_HEAD_PK] = " + TypePk + " AND [REQUEST_PK] = " + RequestPk + ";";
			}
			else if (Type == "Packed") {
				DB.SqlCmd.CommandText = "SELECT [TRANSPORT_BODY_PK] FROM [dbo].[TRANSPORT_BODY] WHERE [TRANSPORT_PACKED_PK] = " + TypePk + " AND [REQUEST_PK] = " + RequestPk + ";";
			}
			string TransportBodyPk = DB.SqlCmd.ExecuteScalar() + "";

			if (TransportBodyPk == "") {
				string HeadPk = " NULL ";
				string PackedPk = " NULL ";
				if (Type == "Head") {
					HeadPk = TypePk;
				}
				else if (Type == "Packed") {
					PackedPk = TypePk;
				}

				DB.SqlCmd.CommandText = @"
				INSERT INTO [dbo].[TRANSPORT_BODY] 
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
				SELECT TOP (1)
				" + HeadPk + @"
				, " + PackedPk + @"
				,[WAREHOUSE_PK] 
				,[REQUEST_PK] 
				,[SHIPPER_COMPANY_PK]
				,[CONSIGNEE_COMPANY_PK]
				,[SHIPPER_COMPANY_CODE]
				,[CONSIGNEE_COMPANY_CODE]
				,[SHIPPER_COMPANY_NAME]
				,[CONSIGNEE_COMPANY_NAME]
				,0 AS [PACKED_COUNT]
				,[PACKING_UNIT]
				,[DESCRIPTION]
				,[WEIGHT]
				,[VOLUME]
				FROM [dbo].[STORAGE]
				WHERE [REQUEST_PK] = " + RequestPk + "; SELECT @@IDENTITY ;";
				TransportBodyPk = DB.SqlCmd.ExecuteScalar() + "";
			}
			return TransportBodyPk;
		}

		public string Find_StoragePk(string TransportBodyPk, string Type, string TypePk, ref DBConn DB) {
			string RequestPk = "";
			string StoragePk = "";

			if (Type == "Warehouse") {
				DB.SqlCmd.CommandText = "SELECT [REQUEST_PK] FROM [dbo].[TRANSPORT_BODY] WHERE [TRANSPORT_BODY_PK] = " + TransportBodyPk + ";";
				RequestPk = DB.SqlCmd.ExecuteScalar() + "";
			}
			else if (Type == "WarehouseToWarehouse") { // 창고이동 Warehouse1 Storage -> Warehouse2 Storage
				string FromStoragePk = TransportBodyPk;
				// TransportBodyPk Parameter 가 이 경우엔 StoragePk임 헷갈릴까바 변수에 담아놈 
				DB.SqlCmd.CommandText = "SELECT [REQUEST_PK] FROM [dbo].[STORAGE] WHERE [STORAGE_PK] = " + FromStoragePk + ";";
				RequestPk = DB.SqlCmd.ExecuteScalar() + "";
			}
			else { // Type == "RequestToWarehouse" // 접수증 입고확인 
				RequestPk = TransportBodyPk;
				// Request(접수증) -> Storage TransportBodyPk Parameter 가 이 경우엔 RequestPk임 헷갈릴까바 변수에 담아놈 
			}

			DB.SqlCmd.CommandText = "SELECT [STORAGE_PK] FROM [dbo].[STORAGE] WHERE [WAREHOUSE_PK] = " + TypePk + " AND [REQUEST_PK] = " + RequestPk + ";";
			StoragePk = DB.SqlCmd.ExecuteScalar() + "";

			if (StoragePk == "") {
				if(Type == "Warehouse" || Type == "WarehouseToWarehouse") {
					DB.SqlCmd.CommandText = @"
					INSERT INTO [dbo].[STORAGE] 
					([WAREHOUSE_PK]
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
					SELECT TOP (1)
					[WAREHOUSE_PK_DEPARTURE] 
					,[REQUEST_PK] 
					,[SHIPPER_COMPANY_PK]
					,[CONSIGNEE_COMPANY_PK]
					,[SHIPPER_COMPANY_CODE]
					,[CONSIGNEE_COMPANY_CODE]
					,[SHIPPER_COMPANY_NAME]
					,[CONSIGNEE_COMPANY_NAME]
					,0 AS [PACKED_COUNT]
					,[PACKING_UNIT]
					,[DESCRIPTION]
					,[WEIGHT]
					,[VOLUME]
					FROM [dbo].[TRANSPORT_BODY]
					WHERE [REQUEST_PK] = " + RequestPk + "; SELECT @@IDENTITY ;";
				}
				else { // Type == "RequestToWarehouse" // 접수증 입고확인 
					DB.SqlCmd.CommandText = @"
					INSERT INTO [dbo].[STORAGE] 
					([REQUEST_PK]
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
					SELECT TOP (1)
					[RequestFormPk]
					,[ShipperPk]
					,[ConsigneePk]
					,[ShipperCode]
					,[ConsigneeCode]
					,CP.[CompanyName] 
					,CP2.[CompanyName]
					,0 AS [PACKED_COUNT]
					,''
					,''
					,0
					,0
					FROM [dbo].[RequestForm] AS RF
					INNER JOIN [dbo].[Company] AS CP ON RF.ShipperPk = CP.CompanyPk
					INNER JOIN [dbo].[Company] AS CP2 ON RF.ConsigneePk = CP2.CompanyPk
					WHERE [RequestFormPk] = " + RequestPk + "; SELECT @@IDENTITY; "; ;
				}

				StoragePk = DB.SqlCmd.ExecuteScalar() + "";
			}
			return StoragePk;
		}

		public string Find_PackedPk(string TransportHeadPk, string TransportBodyPk, ref DBConn DB) {
			string TransportPackedPk = "";

			DB.SqlCmd.CommandText = @"SELECT [TRANSPORT_PACKED_PK] FROM [dbo].[TRANSPORT_PACKED] WHERE [REALPACKED_FLAG] = 'N' AND [TRANSPORT_HEAD_PK] = " + TransportHeadPk;
			TransportPackedPk = DB.SqlCmd.ExecuteScalar() + "";

			if (TransportPackedPk == "") {
				DB.SqlCmd.CommandText = @"
				INSERT INTO [dbo].[TRANSPORT_PACKED]
				([WAREHOUSE_PK]
				,[COMPANY_PK_OWNER]
				,[NO]
				,[SEAL_NO]
				,[REALPACKED_FLAG])
				SELECT TOP (1)
				[WAREHOUSE_PK_DEPARTURE]
				,0000
				,0000
				,0000
				,'N'
				FROM [dbo].[TRANSPORT_BODY]
				WHERE [TRANSPORT_BODY_PK] = " + TransportBodyPk + "; SELECT @@IDENTITY ;";

				TransportPackedPk = DB.SqlCmd.ExecuteScalar() + "";
			}

			return TransportPackedPk;
		}

		public string Check_TempPacked(string TransportBodyPk, ref DBConn DB) {
			DB.SqlCmd.CommandText = @"SELECT TOP (1)
			BODY.[TRANSPORT_PACKED_PK]
			FROM [dbo].[TRANSPORT_PACKED] AS PACKED
			LEFT JOIN [dbo].[TRANSPORT_BODY] AS BODY ON PACKED.[TRANSPORT_PACKED_PK] = BODY.[TRANSPORT_PACKED_PK]
			WHERE [REALPACKED_FLAG] = 'N'
			AND BODY.[TRANSPORT_BODY_PK] = " + TransportBodyPk;

			string PackedPk = DB.SqlCmd.ExecuteScalar() + "";

			if (PackedPk != "" && PackedPk != null) {
				return PackedPk;
			}
			else {
				return "-1";
			}
		}

		public string Load_TransportPackedSpace(string PackedPk, string PackedType, ref DBConn DB) {
			DB.SqlCmd.CommandText = @"SELECT SUM([VOLUME]) FROM [dbo].[TRANSPORT_BODY] WHERE [TRANSPORT_PACKED_PK] = " + PackedPk;
			string SumCbm = DB.SqlCmd.ExecuteScalar() + "";
			string PackedCbm = Common.GetPackedCbm(PackedType);
			if (SumCbm == "0.00" || SumCbm == "") {
				SumCbm = "0.01";
			}
			float ReturnValue = (float.Parse(SumCbm) / float.Parse(PackedCbm)) * 100;

			return ReturnValue + "";
		}

		public List<sCompanyWarehouse> LoadList_CompanyWarehouse(string ConsigneePk, ref DBConn DB) {
			List<sCompanyWarehouse> ReturnValue = new List<sCompanyWarehouse>();
			DB.SqlCmd.CommandText = @"SELECT
				[WarehousePk]
				,[CompanyPk]
				,[Title]
				,[Address]
				,[TEL]
				,[Staff]
				,[Mobile]
				,[Memo]
			FROM [dbo].[CompanyWarehouse]
			WHERE [CompanyPk] = " + ConsigneePk;
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				ReturnValue.Add(new sCompanyWarehouse() {
					WarehousePk = RS["WarehousePk"] + "",
					CompanyPk = RS["CompanyPk"] + "",
					Title = RS["Title"] + "",
					Address = RS["Address"] + "",
					Tel = RS["TEL"] + "",
					Staff = RS["Staff"] + "",
					Mobile = RS["Mobile"] + "",
					Memo = RS["Memo"] + ""
				});
			}
			RS.Close();

			return ReturnValue;
		}

		public sTransportHead Load_TransportHeadDelivery(string TransportHeadPk, ref DBConn DB) {
			sTransportHead ReturnValue = new sTransportHead();
			DB.SqlCmd.CommandText = @"SELECT 
				HEAD.[TRANSPORT_PK]
				,HEAD.[TRANSPORT_WAY]
				,HEAD.[TRANSPORT_STATUS]
				,HEAD.[BRANCHPK_FROM]
				,HEAD.[BRANCHPK_TO]
				,HEAD.[WAREHOUSE_PK_ARRIVAL]
				,HEAD.[AREA_FROM]
				,HEAD.[AREA_TO]
				,HEAD.[DATETIME_FROM]
				,HEAD.[DATETIME_TO]
				,HEAD.[TITLE]
				,HEAD.[VESSELNAME]
				,HEAD.[VOYAGE_NO]
				,HEAD.[VALUE_STRING_0]
				,HEAD.[VALUE_STRING_1]
				,HEAD.[VALUE_STRING_2]
				,HEAD.[VALUE_STRING_3]
				,HEAD.[VALUE_STRING_4]
				,HEAD.[VALUE_STRING_5]
				,CW.[WarehousePk]
				,CW.[Title] AS COMPANYWAREHOUSE_TITLE
				,CW.[Address]
				,CW.[TEL]
				,CW.[Staff]
				,CW.[Mobile]
			FROM [dbo].[TRANSPORT_HEAD] AS HEAD
			LEFT JOIN [dbo].[CompanyWarehouse] AS CW ON HEAD.AREA_TO = CAST(CW.[WarehousePk] AS VARCHAR)
			WHERE HEAD.[TRANSPORT_WAY] = 'Delivery'
			AND HEAD.[TRANSPORT_PK] = " + TransportHeadPk;

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				ReturnValue.DateTime_From = RS["DATETIME_FROM"] + "";
				ReturnValue.DateTime_To = RS["DATETIME_TO"] + "";
				ReturnValue.Title = RS["TITLE"] + "";
				ReturnValue.VesselName = RS["VESSELNAME"] + "";
				ReturnValue.Voyage_No = RS["VOYAGE_NO"] + "";
				ReturnValue.BranchPk_From = RS["BRANCHPK_FROM"] + "";
				ReturnValue.BranchPk_To = RS["BRANCHPK_TO"] + "";
				ReturnValue.Warehouse_Pk_Arrival = RS["WAREHOUSE_PK_ARRIVAL"] + "";
				ReturnValue.Area_From = RS["AREA_FROM"] + "";
				ReturnValue.Area_To = RS["AREA_TO"] + "";
				ReturnValue.Value_String_1 = RS["VALUE_STRING_1"] + "";
				ReturnValue.Value_String_2 = RS["VALUE_STRING_2"] + "";
				ReturnValue.Value_String_3 = RS["VALUE_STRING_3"] + "";
				ReturnValue.CompanyWarehouse_Pk = RS["WarehousePk"] + "";
				ReturnValue.CompanyWarehouse_Title = RS["COMPANYWAREHOUSE_TITLE"] + "";
				ReturnValue.CompanyWarehouse_Address = RS["Address"] + "";
				ReturnValue.CompanyWarehouse_Tel = RS["TEL"] + "";
				ReturnValue.CompanyWarehouse_Staff = RS["Staff"] + "";
				ReturnValue.CompanyWarehouse_Mobile = RS["Mobile"] + "";
			}
			RS.Close();
			return ReturnValue;
		}

		public List<sDeliveryListHead> LoadList_DeliveryList(string ResponsibleStaff, ref DBConn DB) {
			List<sDeliveryListHead> ReturnValue = new List<sDeliveryListHead>();
			sDeliveryListHead Head = new sDeliveryListHead();
			Head.arrBody = new List<sDeliveryListBody>();
			string QueryWhere = "";
			if (ResponsibleStaff != "ALL") {
				QueryWhere = "AND CCP.[ResponsibleStaff] = " + Common.StringToDB(ResponsibleStaff, true, false);
			}

			DB.SqlCmd.CommandText = @"
			SELECT 
				TH.[TRANSPORT_PK]
				, TH.[TRANSPORT_WAY]
				, TH.[TITLE]
				, TH.[VESSELNAME]
				, TH.[VOYAGE_NO]
				, TH.[AREA_FROM]
				, TH.[AREA_TO]
				, TP.[TRANSPORT_PACKED_PK]
				, TP.[NO]
				, TP.[SIZE]
				, TP.[SEAL_NO]
				, CD.[BLNo]
				, SR.[REQUEST_PK]
				, SR.[STORAGE_PK] AS STORAGE_PK
				, SR.[CONSIGNEE_COMPANY_PK]
				, SR.[CONSIGNEE_COMPANY_CODE]
				, SR.[CONSIGNEE_COMPANY_NAME]
				, SR.[PACKED_COUNT]
				, SR.[PACKING_UNIT]
				, SR.[VOLUME]
				, SR.[WEIGHT]
				, RF.[DocumentRequestCL]
				, RF.[ArrivalDate]
				, RFCH.[REQUESTFORMCALCULATE_HEAD_PK]
				, BCP.[CompanyCode] AS BCOMPANY_CODE
				, CCP.[CompanyCode] AS CCOMPANY_CODE
				, RFCH.[TOTAL_PRICE]
				, RFCH.[DEPOSITED_PRICE]
			FROM [dbo].[STORAGE] AS SR
			LEFT JOIN [dbo].[TRANSPORT_HEAD] AS TH ON SR.[LAST_TRANSPORT_HEAD_PK] = TH.[TRANSPORT_PK]
			LEFT JOIN [dbo].[TRANSPORT_PACKED] AS TP ON SR.[LAST_TRANSPORT_PACKED_PK] = TP.[TRANSPORT_PACKED_PK]
			LEFT JOIN [dbo].[RequestForm] AS RF ON SR.[REQUEST_PK] = RF.[RequestFormPk]
			LEFT JOIN [dbo].[CommerdialConnectionWithRequest] AS CCWR ON SR.[REQUEST_PK] = CCWR.[RequestFormPk]
			LEFT JOIN [dbo].[CommercialDocument] AS CD ON CCWR.[CommercialDocumentPk] = CD.[CommercialDocumentHeadPk]
			LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON SR.[REQUEST_PK] = RFCH.[TABLE_PK]
			LEFT JOIN [dbo].[Company] AS BCP ON RFCH.[BRANCH_COMPANY_PK] = BCP.[CompanyPk]
			LEFT JOIN [dbo].[Company] AS CCP ON RFCH.[CUSTOMER_COMPANY_PK] = CCP.[CompanyPk]
			WHERE ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm' 
			AND RF.[StepCL] < 65
			AND CD.[CommercialDocumentHeadPk] IS NOT NULL
			AND TH.[TRANSPORT_PK] IS NOT NULL " + QueryWhere + @" 
			UNION ALL
			SELECT top (50)
				TH.[TRANSPORT_PK]
				, TH.[TRANSPORT_WAY]
				, TH.[TITLE]
				, TH.[VESSELNAME]
				, TH.[VOYAGE_NO]
				, TH.[AREA_FROM]
				, TH.[AREA_TO]
				, TP.[TRANSPORT_PACKED_PK]
				, TP.[NO]
				, TP.[SIZE]
				, TP.[SEAL_NO]
				, CD.[BLNo]
				, SR.[REQUEST_PK]
				, SR.[TRANSPORT_BODY_PK] AS STORAGE_PK
				, SR.[CONSIGNEE_COMPANY_PK]
				, SR.[CONSIGNEE_COMPANY_CODE]
				, SR.[CONSIGNEE_COMPANY_NAME]
				, SR.[PACKED_COUNT]
				, SR.[PACKING_UNIT]
				, SR.[VOLUME]
				, SR.[WEIGHT]
				, RF.[DocumentRequestCL]
				, RF.[ArrivalDate]
				, RFCH.[REQUESTFORMCALCULATE_HEAD_PK]
				, BCP.[CompanyCode] AS BCOMPANY_CODE
				, CCP.[CompanyCode] AS CCOMPANY_CODE
				, RFCH.[TOTAL_PRICE]
				, RFCH.[DEPOSITED_PRICE]
			FROM [dbo].[TRANSPORT_BODY] AS SR
			LEFT JOIN [dbo].[TRANSPORT_HEAD] AS TH ON SR.[TRANSPORT_HEAD_PK] = TH.[TRANSPORT_PK]
			LEFT JOIN [dbo].[TRANSPORT_PACKED] AS TP ON SR.[TRANSPORT_PACKED_PK] = TP.[TRANSPORT_PACKED_PK]
			LEFT JOIN [dbo].[RequestForm] AS RF ON SR.[REQUEST_PK] = RF.[RequestFormPk]
			LEFT JOIN [dbo].[CommerdialConnectionWithRequest] AS CCWR ON SR.[REQUEST_PK] = CCWR.[RequestFormPk]
			LEFT JOIN [dbo].[CommercialDocument] AS CD ON CCWR.[CommercialDocumentPk] = CD.[CommercialDocumentHeadPk]
			LEFT JOIN [dbo].[REQUESTFORMCALCULATE_HEAD] AS RFCH ON SR.[REQUEST_PK] = RFCH.[TABLE_PK]
			LEFT JOIN [dbo].[Company] AS BCP ON RFCH.[BRANCH_COMPANY_PK] = BCP.[CompanyPk]
			LEFT JOIN [dbo].[Company] AS CCP ON RFCH.[CUSTOMER_COMPANY_PK] = CCP.[CompanyPk]
			WHERE ISNULL(RFCH.[TABLE_NAME], 'RequestForm') = 'RequestForm' 
			AND RF.[StepCL] < 65
			AND CD.[CommercialDocumentHeadPk] IS NOT NULL
			AND TH.[TRANSPORT_PK] IS NOT NULL " + QueryWhere + @" 
			ORDER BY RF.[ArrivalDate] DESC, TH.[TRANSPORT_PK], TP.[TRANSPORT_PACKED_PK]";

			string CurrPacked = "";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {

				if (CurrPacked != RS["TRANSPORT_PACKED_PK"].ToString() && CurrPacked != "") {
					ReturnValue.Add(Head);
				}

				if (CurrPacked != RS["TRANSPORT_PACKED_PK"].ToString()) {
					Head = new sDeliveryListHead();
					Head.TransportHeadPk = RS["TRANSPORT_PK"].ToString();
					Head.TransportWay = RS["TRANSPORT_WAY"].ToString();
					Head.TransportTitle = RS["TITLE"].ToString();
					Head.TransportVesselName = RS["VESSELNAME"].ToString();
					Head.TransportVoyageNo = RS["VOYAGE_NO"].ToString();
					Head.TransportAreaFrom = RS["AREA_FROM"].ToString();
					Head.TransportAreaTo= RS["AREA_TO"].ToString();
					Head.TransportPackedPk = RS["TRANSPORT_PACKED_PK"].ToString();
					Head.PackedNo = RS["NO"].ToString();
					Head.PackedSize = RS["SIZE"].ToString();
					Head.PackedSeal = RS["SEAL_NO"].ToString();

					Head.arrBody = new List<sDeliveryListBody>();
				}

				Head.arrBody.Add(new sDeliveryListBody() {
					RequestPk = RS["REQUEST_PK"].ToString(),
					StoragePk = RS["STORAGE_PK"].ToString(),
					BlNo = RS["BLNo"].ToString(),
					CompanyCode = RS["CONSIGNEE_COMPANY_CODE"].ToString(),
					CompanyName = RS["CONSIGNEE_COMPANY_NAME"].ToString(),
					PackedCount = RS["PACKED_COUNT"].ToString(),
					PackingUnit = RS["PACKING_UNIT"].ToString(),
					Volume = RS["VOLUME"].ToString(),
					Weight = RS["WEIGHT"].ToString(),
					DocumentRequest = RS["DocumentRequestCL"].ToString(),
					ArrivalDate = RS["ArrivalDate"].ToString(),
					CalculateHeadPk = RS["REQUESTFORMCALCULATE_HEAD_PK"].ToString(),
					CalculateBranchCode = RS["BCOMPANY_CODE"].ToString(),
					CalculateCustomerCode = RS["CCOMPANY_CODE"].ToString(),
					TotalPrice = RS["TOTAL_PRICE"].ToString(),
					DepositedPrice = RS["DEPOSITED_PRICE"].ToString()
				});

				CurrPacked = RS["TRANSPORT_PACKED_PK"].ToString();

			}
			RS.Close();

			return ReturnValue;
		}
		

	}

    public struct sTransportHead
    {
        public string Transport_Head_Pk;
        public string Transport_Way;
        public string Transport_Status;
        public string BranchPk_From;
        public string BranchPk_To;
		public string Warehouse_Pk_Arrival;
        public string Area_From;
        public string Area_To;
        public string DateTime_From;
        public string DateTime_To;
        public string Title;
        public string VesselName;
        public string Voyage_No;
        public string Value_String_0;
        public string Value_String_1;
        public string Value_String_2;
        public string Value_String_3;
        public string Value_String_4;
        public string Value_String_5;
		public string CompanyWarehouse_Pk;
		public string CompanyWarehouse_Title;
		public string CompanyWarehouse_Address;
		public string CompanyWarehouse_Tel;
		public string CompanyWarehouse_Staff;
		public string CompanyWarehouse_Mobile;
		public List<sTransportPacked> arrPacked;
        public List<sTransportBody> arrBody;
    }
    public struct sTransportBody
    {
        public string Transport_Body_Pk;
        public string Transport_Head_Pk;
        public string Transport_Packed_Pk;
        public string Warehouse_Pk_Departure;
		public string Warehouse_Branch_Pk;
		public string Last_Transport_Head_Pk;
		public string Last_Transport_Pacekd_Pk;
		public string Request_Pk;
        public string Shipper_Company_Pk;
        public string Consignee_Company_Pk;
        public string Shipper_Company_Code;
        public string Consignee_Company_Code;
        public string Shipper_Company_Name;
        public string Consignee_Company_Name;
        public string Packed_Count;
        public string Packing_Unit;
        public string Description;
        public string Weight;
        public string Volume;
		public string Stocked_Date;
		public string Req_Area_From;
		public string Req_DateTime_From;
		public string Req_DateTime_To;
		public string Req_Transport_Way;
    }
    public struct sTransportPacked
    {
        public string Transport_Packed_Pk;
		public string Seq;
		public string WareHouse_Pk;
		public string WareHouse_Name;
        public string Transport_Head_Pk;
        public string Company_Pk_Owner;
		public string Company_Code_Owner;
		public string Container_Company;
        public string Type;
        public string No;
        public string Size;
        public string Seal_No;
		public string RealPacked_Flag;
        public List<sTransportBody> arrBody;
    }

    public struct sStorageItem 
    {
        public string Storage_Pk;
        public string Transport_Head_Pk;
        public string Transport_Packed_Pk;
        public string Warehouse_Pk;
		public string Warehouse_Name;
		public string Warehouse_Branch_Pk;
        public string Last_Transport_Head_Pk;
        public string Last_Transport_Pacekd_Pk;
        public string Request_Pk;
        public string Shipper_Company_Pk;
        public string Consignee_Company_Pk;
        public string Shipper_Company_Code;
        public string Consignee_Company_Code;
        public string Shipper_Company_Name;
        public string Consignee_Company_Name;
        public string Packed_Count;
        public string Packing_Unit;
        public string Description;
        public string Weight;
        public string Volume;
		public string Stocked_Date;
		public string Req_Area_From;
		public string Req_DateTime_From;
		public string Req_DateTime_To;
		public string Req_Transport_Way;
	}

	public struct sCompanyWarehouse
	{
		public string WarehousePk;
		public string CompanyPk;
		public string Title;
		public string Address;
		public string Tel;
		public string Staff;
		public string Mobile;
		public string Memo;
	}

	public struct sDeliveryListHead
	{
		public string TransportHeadPk;
		public string TransportWay;
		public string TransportTitle;
		public string TransportVesselName;
		public string TransportVoyageNo;
		public string TransportAreaFrom;
		public string TransportAreaTo;
		public string TransportPackedPk;
		public string PackedNo;
		public string PackedSize;
		public string PackedSeal;
		public List<sDeliveryListBody> arrBody;
	}

	public struct sDeliveryListBody
	{
		public string StoragePk;
		public string RequestPk;
		public string BlNo;
		public string CompanyCode;
		public string CompanyName;
		public string PackedCount;
		public string PackingUnit;
		public string Volume;
		public string Weight;
		public string DocumentRequest;
		public string ArrivalDate;
		public string CalculateHeadPk;
		public string CalculateBranchCode;
		public string CalculateCustomerCode;
		public string TotalPrice;
		public string DepositedPrice;
		public string TransportStatus;
		public string ClearanceStatus;
		public string CollectStatus;
	}
}
