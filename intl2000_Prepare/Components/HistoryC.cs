using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
	public class HistoryC
	{
		public HistoryC()  {

		}

		public string Set_Comment(sComment Comment, ref DBConn DB) {
			StringBuilder Query = new StringBuilder();
			GetQuery GQ = new GetQuery();
			string[] Account = Load_AccountInfo(Comment.Account_Id, ref DB);
			Comment.Account_Pk = Account[0];
			Comment.Account_Name = Account[1];
			Query.Append(GQ.Comment(Comment));
			DB.SqlCmd.CommandText = "" + Query;
			DB.SqlCmd.ExecuteNonQuery();
			return "1";
		}

		public string[] Load_AccountInfo(string Account_Id, ref DBConn DB) {
			string[] Account = new string[2];
			string a = @"SELECT [AccountPk], [Name] FROM [dbo].[Account_] WHERE [AccountID] = '" + Account_Id + @"';";
			DB.SqlCmd.CommandText = @"SELECT [AccountPk], [Name] FROM [dbo].[Account_] WHERE [AccountID] = '" + Account_Id + @"';";
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				Account[0] = RS["AccountPk"] + "";
				Account[1] = RS["Name"] + "";
			}
			RS.Close();
			return Account;
		}

		public List<sComment> LoadList_Comment(string Table_Name, string Table_Pk, string Category, ref DBConn DB) {
			List<sComment> ReturnValue = new List<sComment>();
			sComment temp = new sComment();
			DB.SqlCmd.CommandText = @"SELECT [COMMENT_PK]
			,[TABLE_NAME]
			,[TABLE_PK]
			,[CATEGORY]
			,[CONTENTS]
			,[ACCOUNT_PK]
			,[ACCOUNT_ID]
			,[ACCOUNT_NAME]
			,[REGISTERD]
		FROM [dbo].[COMMENT]
		WHERE [TABLE_NAME] = '" + Table_Name + @"' 
		AND [TABLE_PK] = " + Table_Pk + @" 
		AND [CATEGORY] IN (" + Category + @") 		
		ORDER BY REGISTERD DESC; ";

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				temp.Comment_Pk = RS["COMMENT_PK"] + "";
				temp.Table_Name = RS["TABLE_NAME"] + "";
				temp.Table_Pk = RS["TABLE_PK"] + "";
				temp.Category = RS["CATEGORY"] + "";
				temp.Contents = RS["CONTENTS"] + "";
				temp.Account_Pk = RS["ACCOUNT_PK"] + "";
				temp.Account_Id = RS["ACCOUNT_ID"] + "";
				temp.Account_Name = RS["ACCOUNT_NAME"] + "";
				temp.Registerd = RS["REGISTERD"] + "";
				ReturnValue.Add(temp);
			}
			RS.Close();
			return ReturnValue;
		}
		
		public List<sHistory> LoadList_History(string Table_Name, string Table_Pk, string Code, ref DBConn DB) {
			List<sHistory> ReturnValue = new List<sHistory>();
			sHistory temp = new sHistory();
			DB.SqlCmd.CommandText = @"SELECT
			[HISTORY_PK]
			,[TABLE_NAME]
			,[TABLE_PK]
			,[CODE]
			,[DESCRIPTION]
			,[ACCOUNT_PK]
			,[ACCOUNT_NAME]
			,[ACCOUNT_ID]
			,[REGISTERD]
		FROM [dbo].[HISTORY]
		WHERE [TABLE_NAME] = '" + Table_Name + @"'
		AND [TABLE_PK] = " + Table_Pk + @"
		AND [CODE] = '" + Code + @"';";

			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			while (RS.Read()) {
				temp.History_Pk = RS["HISTORY_PK"] + "";
				temp.Table_Name = RS["TABLE_NAME"] + "";
				temp.Table_Pk = RS["TABLE_PK"] + "";
				temp.Code = RS["CODE"] + "";
				temp.Description = RS["DESCRIPTION"] + "";
				temp.Account_Pk = RS["ACCOUNT_PK"] + "";
				temp.Account_name = RS["ACCOUNT_NAME"] + "";
				temp.Account_Id = RS["ACCOUNT_ID"] + "";
				temp.Registerd = RS["REGISTERD"] + "";
				ReturnValue.Add(temp);
			}

			return ReturnValue;
		}

		public string Set_History(sHistory History, ref DBConn DB) {
			Setting setting = new Setting();
			string[] Account = Load_AccountInfo(History.Account_Id, ref DB);
			History.Account_Pk = Account[0];
			History.Account_name = Account[1];
			string Query = @"INSERT INTO [dbo].[HISTORY] 
            ([TABLE_NAME]
            ,[TABLE_PK]
            ,[CODE]
            ,[DESCRIPTION]
            ,[ACCOUNT_PK]
            ,[ACCOUNT_NAME]
            ,[ACCOUNT_ID]
            ,[BEFORE_DATA]
            ,[AFTER_DATA])
            VALUES
            (" + setting.ToDB(History.Table_Name, "varchar") + @"
            ," + setting.ToDB(History.Table_Pk, "int") + @"
            ," + setting.ToDB(History.Code, "varchar") + @"
            ," + setting.ToDB(History.Description, "nvarchar") + @"
            ," + setting.ToDB(History.Account_Pk, "int") + @"
            ," + setting.ToDB(History.Account_name, "nvarchar") + @"  
            ," + setting.ToDB(History.Account_Id, "varchar") + @"
            ," + setting.ToDB(History.Before_Data, "nvarchar") + @"
            ," + setting.ToDB(History.After_Data, "nvarchar") + @") ";
			DB.SqlCmd.CommandText = Query;
			DB.SqlCmd.ExecuteNonQuery();
			return "1";
		}

		public string ComputeChanges_Head(sTransportHead Head, ref DBConn DB) {
			TransportC TransC = new TransportC();
			sTransportHead Before = TransC.Load_TransportHead(Head.Transport_Head_Pk, ref DB);
			sTransportHead After = Head;
			StringBuilder Changes = new StringBuilder();

			if (Before.Transport_Status != After.Transport_Status) {
				Changes.Append("Status: " + Before.Transport_Status + "->" + After.Transport_Status + " || ");
			}
			if (Before.BranchPk_To != After.BranchPk_To) {
				Changes.Append("BranchPk_To: " + Before.BranchPk_To + "->" + After.BranchPk_To + " || ");
			}
			if (Before.Warehouse_Pk_Arrival != After.Warehouse_Pk_Arrival) {
				Changes.Append("Warehouse_Pk_Arrival: " + Before.Warehouse_Pk_Arrival + "->" + After.Warehouse_Pk_Arrival + " || ");
			}
			if (Before.Area_From != After.Area_From) {
				Changes.Append("Area_From: " + Before.Area_From + "->" + After.Area_From + " || ");
			}
			if (Before.DateTime_From != After.DateTime_From) {
				Changes.Append("DateTime_From: " + Before.DateTime_From + "->" + After.DateTime_From + " || ");
			}
			if (Before.DateTime_To != After.DateTime_To) {
				Changes.Append("DateTime_To: " + Before.DateTime_To + "->" + After.DateTime_To + " || ");
			}
			if (Before.VesselName != After.VesselName) {
				Changes.Append("VesselName: " + Before.VesselName + "->" + After.VesselName + " || ");
			}
			if (Before.Voyage_No != After.Voyage_No) {
				Changes.Append("Voyage_No: " + Before.Voyage_No + "->" + After.Voyage_No + " || ");
			}
			if (Before.Value_String_0 != After.Value_String_0) {
				Changes.Append("BLNo: " + Before.Value_String_0 + "->" + After.Value_String_0 + " || ");
			}
			if (Before.Value_String_1 != After.Value_String_1) {
				Changes.Append("Tel_From: " + Before.Value_String_1 + "->" + After.Value_String_1 + " || ");
			}
			if (Before.Value_String_2 != After.Value_String_2) {
				Changes.Append("Tel_To: " + Before.Value_String_2 + "->" + After.Value_String_2 + " || ");
			}
			if (Before.Value_String_3 != After.Value_String_3) {
				Changes.Append("Car_Size: " + Before.Value_String_3 + "->" + After.Value_String_3 + " || ");
			}
			return Changes + "";
		}

		public string ComputeChanges_Packed(sTransportPacked Packed, ref DBConn DB) {
			TransportC TransC = new TransportC();
			sTransportPacked Before = TransC.Load_TransportPackedOnly(Packed.Transport_Packed_Pk, ref DB);
			sTransportPacked After = Packed;
			StringBuilder Changes = new StringBuilder();

			if (Before.Transport_Head_Pk != After.Transport_Head_Pk) {
				Changes.Append("Transport_Head_Pk: " + Before.Transport_Head_Pk + "->" + After.Transport_Head_Pk + " || ");
			}
			if (Before.Company_Pk_Owner != After.Company_Pk_Owner) {
				Changes.Append("Company_Pk_Owner: " + Before.Company_Pk_Owner + "->" + After.Company_Pk_Owner + " || ");
			}
			if (Before.Container_Company != After.Container_Company) {
				Changes.Append("Container_Company: " + Before.Container_Company + "->" + After.Container_Company + " || ");
			}
			if (Before.Type != After.Type) {
				Changes.Append("Type: " + Before.Type + "->" + After.Type + " || ");
			}
			if (Before.No != After.No) {
				Changes.Append("No: " + Before.No + "->" + After.No + " || ");
			}
			if (Before.Size != After.Size) {
				Changes.Append("Size: " + Before.Size + "->" + After.Size + " || ");
			}
			if (Before.Seal_No != After.Seal_No) {
				Changes.Append("Seal_No: " + Before.Seal_No + "->" + After.Seal_No + " || ");
			}
			return Changes + "";
		}


	}

	public struct sComment
	{
		public string Comment_Pk;
		public string Table_Name;
		public string Table_Pk;
		public string Category;
		public string Contents;
		public string Account_Pk;
		public string Account_Id;
		public string Account_Name;
		public string Registerd;
	}

	public struct sHistory
	{
		public string History_Pk;
		public string Table_Name;
		public string Table_Pk;
		public string Code;
		public string Description;
		public string Account_Pk;
		public string Account_name;
		public string Account_Id;
		public string Registerd;
		public string Before_Data;
		public string After_Data;
	}
}