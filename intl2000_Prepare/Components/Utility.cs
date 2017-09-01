using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
	public class Utility
	{
		public string GetQuery(string Table, Dictionary<string, string> Data) {
			string[] ArrColume = new string[] { };
			string[] ArrDataType = new string[] { };
			switch (Table) {
				case "Account_":
					ArrColume = new string[] { "AccountPk", "AccountID", "Password", "GubunCL", "CompanyPk", "Duties", "Name", "TEL", "Mobile", "Email", "Authority", "IsEmailNSMS", "LastVisit" };
					ArrDataType = new string[] { "int", "varchar", "varchar", "tinyint", "int", "nvarchar", "nvarchar", "varchar", "varchar", "varchar", "varchar", "smallint", "datetime" };
					break;
				case "Board_Code":
					ArrColume = new string[] { "BoardCodeId", "CompanyId", "BoardCode", "Title", "SumHeader", "OrderBy" };
					ArrDataType = new string[] { "int", "int", "varchar", "nvarchar", "nvarchar", "smallint" };
					break;
				case "Board_Head":
					ArrColume = new string[] { "BoardHeadId", "BoardCode", "Header", "Position", "No", "BoardHeadId_Parents", "Title", "AccountId_Writer", "SignId", "Name" };
					ArrDataType = new string[] { "int", "varchar", "nvarchar", "varchar", "int", "int", "nvarchar", "int", "varchar", "nvarchar" };
					break;
				case "Board_Body":
					ArrColume = new string[] { "BoardBodyId", "BoardHeadId", "Position", "ContentsType", "Contents", "AccountId_Writer", "SignId", "Name" };
					ArrDataType = new string[] { "int", "int", "varchar", "varchar", "nvarchar", "int", "varchar", "nvarchar" };
					break;
				case "Company":
					ArrColume = new string[] { "CompanyPk", "GubunCL", "CompanyCode", "CompanyName", "RegionCode", "CompanyAddress", "CompanyTEL", "CompanyFAX", "PresidentName", "PresidentEmail", "CompanyNo", "CompanyNameE", "CompanyAddressE", "LastRequestDate", "ResponsibleStaff", "Memo" };
					ArrDataType = new string[] { "int", "smallint", "varchar", "nvarchar", "varchar", "nvarchar", "varchar", "varchar", "nvarchar", "varchar", "varchar", "varchar", "varchar", "datetime", "varchar", "nvarchar" };
					break;
				case "RequestFormItems":
					ArrColume = new string[] { "RequestFormItemsPk", "RequestFormPk", "ItemCode", "MarkNNumber", "Description", "Label", "Material", "Quantity", "QuantityUnit", "PackedCount", "PackingUnit", "GrossWeight", "NetWeight", "Volume", "UnitPrice", "Amount", "RAN", "HSCodeForCO" };
					ArrDataType = new string[] { "int", "int", "int", "varchar", "nvarchar", "nvarchar", "nvarchar", "money", "smallint", "int", "smallint", "money", "money", "money", "money", "money", "tinyint", "varchar" };
					break;
				case "TRANSPORT_HEAD":
					ArrColume = new string[] { "TRANSPORT_PK", "TRANSPORT_WAY", "TRANSPORT_STATUS", "BRANCHPK_FROM", "BRANCHPK_TO", "WAREHOUSE_PK_ARRIVAL", "AREA_FROM", "AREA_TO", "DATETIME_FROM", "DATETIME_TO", "TITLE", "VESSELNAME", "VOYAGE_NO", "VALUE_STRING_0", "VALUE_STRING_1", "VALUE_STRING_2", "VALUE_STRING_3", "VALUE_STRING_4", "VALUE_STRING_5" };
					ArrDataType = new string[] { "int", "varchar", "tinyint", "int", "int", "int", "nvarchar", "nvarchar", "varchar", "varchar", "nvarchar", "nvarchar", "varchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar" };
					break;
				case "BANK_DEPOSIT":
					ArrColume = new string[] { "BANK_DEPOSIT_PK", "BANK_PK", "CATEGORY", "TYPE", "TYPE_PK", "DESCRIPTION", "MONETARY_UNIT", "PRICE", "PRICE_REMAIN", "DATETIME" };
					ArrDataType = new string[] { "int", "int", "nvarchar", "varchar", "int", "nvarchar", "varchar", "money", "money", "varchar" };
					break;
				case "TRANSPORT_PACKED":
					ArrColume = new string[] { "TRANSPORT_PACKED_PK", "SEQ", "WAREHOUSE_PK", "TRANSPORT_HEAD_PK", "COMPANY_PK_OWNER", "CONTAINER_COMPANY", "TYPE", "NO", "SIZE", "SEAL_NO", "REALPACKED_FLAG" };
					ArrDataType = new string[] { "int", "int", "int", "int", "int", "varchar", "varchar", "varchar", "varchar", "varchar", "varchar" };
					break;
				case "COMMENT":
					ArrColume = new string[] { "COMMENT_PK", "TABLE_NAME", "TABLE_PK", "CATEGORY", "CONTENTS", "ACCOUNT_PK", "ACCOUNT_ID", "ACCOUNT_NAME" };
					ArrDataType = new string[] { "int", "varchar", "int", "varchar", "nvarchar", "int", "varchar", "nvarchar" };
					break;
				case "Document":
					ArrColume = new string[] { "DocumentPk", "Type", "TypePk", "Status", "Value0", "Value1", "Value2", "Value3", "Value4", "Value5", "Value6", "Value7", "Value8", "Value9", "Value10", "Value11", "Value12", "Value13", "Value14", "Value15", "Value16", "Value17", "Value18", "Value19", "ValueInt0", "ValueDecimal0", "ValueDecimal1", "ValueDecimal2", "ParentsType", "ParentsId" };
					ArrDataType = new string[] { "int", "varchar", "int", "tinyint", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "nvarchar", "int", "decimal", "decimal", "decimal", "varchar", "int" };
					break;
			}

			string ReturnValue = "";

			if (Data[ArrColume[0]] == "" || Data[ArrColume[0]] == null) {
				StringBuilder queryLeft = new StringBuilder();
				StringBuilder queryElse = new StringBuilder();
				for (var i = 1; i < ArrColume.Length; i++) {
					if (Data.ContainsKey(ArrColume[i]) && Data[ArrColume[i]] != Setting.DBNotChangedString) {
						queryLeft.Append(",[" + ArrColume[i] + "]");
						queryElse.Append("," + ToDB(Data[ArrColume[i]], ArrDataType[i]));
					}
				}
				if (queryLeft.ToString() != "") {
					ReturnValue = "INSERT INTO [dbo].[" + Table + "] (" + queryLeft.ToString().Substring(1) + ") VALUES (" + queryElse.ToString().Substring(1) + ");" +
						"SELECT @@IDENTITY;";
				}
			} else {
				StringBuilder query = new StringBuilder();
				for (var i = 1; i < ArrColume.Length; i++) {
					if (Data.ContainsKey(ArrColume[i]) && Data[ArrColume[i]] != Setting.DBNotChangedString) {
						query.Append(",[" + ArrColume[i] + "]=" + ToDB(Data[ArrColume[i]], ArrDataType[i]));
					}
				}
				if (query.ToString() != "") {
					ReturnValue = "UPDATE [dbo].[" + Table + "] SET " + query.ToString().Substring(1) + " WHERE " + ArrColume[0] + "=" + ToDB(Data[ArrColume[0]], ArrDataType[0]) + ";" +
						"SELECT " + ToDB(Data[ArrColume[0]], ArrDataType[0]);
				}
			}
			return ReturnValue;
		}



		public Dictionary<string, string> ConvertGetParam(string Data) {
			Dictionary<string, string> ReturnValue = new Dictionary<string, string>();
			string[] ArrData = Data.Split(new string[] {
				"&"
			}, StringSplitOptions.RemoveEmptyEntries);
			foreach (string Each in ArrData) {
				int equal = Each.IndexOf("=");
				string Key = System.Uri.UnescapeDataString(Each.Substring(0, equal)).Replace("+", " ");
				string Value = System.Uri.UnescapeDataString(Each.Substring(equal + 1)).Replace("+", " ");

				if (ReturnValue.ContainsKey(Key)) {
					var i = 0;
					string checkKey = Key + i;
					while (ReturnValue.ContainsKey(checkKey)) {
						i++;
						checkKey = Key + i;
					}
					Key = checkKey;
					ReturnValue.Add(Key, Value);
				} else {
					ReturnValue.Add(Key, Value);
				}
			}
			return ReturnValue;
		}
		public static String NumberFormat(string Number, string Format = "###,###,###,###,###,##0.####") {
			try {
				return Decimal.Parse(Number).ToString(Format);
			} catch (Exception) {
				return "";
			}
		}
		public static String NumberFormat(decimal Number, string Format = "###,###,###,###,###,##0.####") {
			try {
				return Number.ToString(Format);
			} catch (Exception) {
				return "";
			}
		}

		public static string ToDB(string Value, string DataType) {
			if (Value == "" || Value == null) {
				return "NULL";
			} else if (DataType == "nvarchar" || DataType == "nchar") {
				return "N'" + Value + "'";
			} else if (DataType == "string" || DataType == "varchar" || DataType == "char") {
				return "'" + Value + "'";
			} else if (DataType == "int" || DataType == "decimal" || DataType == "smallint" || DataType == "tinyint" || DataType == "money") {
				return Value.Replace(",", "");
			} else {
				return Value;
			}
		}
		public static String ToDB(string TableName, List<string[]> Values, string[] FinalSelect = null) {
			if (FinalSelect == null) {
				FinalSelect = new string[] {
					"SELECT @@IDENTITY;",
					"SELECT " + Values[0][1] + ";"
				};
			}
			string Mode = Values[0][1] + "" == "" ? "INSERT" : "UPDATE";
			if (Values[0][1] == "0") {
				Mode = "INSERT";
			}
			if (Mode == "INSERT") {
				StringBuilder Left = new StringBuilder();
				StringBuilder Right = new StringBuilder();

				for (var i = 0; i < Values.Count; i++) {
					if (i == 0) {
						continue;
					}

					string DataType = "string";
					if (Values[i].Length > 2) {
						DataType = "Number";
						if (Values[i][2] == "char" || Values[i][2] == "nchar" || Values[i][2] == "nvarchar" || Values[i][2] == "varbinary" || Values[i][2] == "varchar") {
							DataType = "string";
						}
					}

					if (DataType == "Number") {
						if (Values[i][1] != Setting.DBNotChangedString && Values[i][1] != "" && Values[i][1] != null) {
							Left.Append(", [" + Values[i][0] + "]");
							Right.Append(", " + Values[i][1]);
						}
					} else {
						if (Values[i][1] != Setting.DBNotChangedString && Values[i][1] != "" && Values[i][1] != null) {
							Left.Append(", [" + Values[i][0] + "]");
							Right.Append(", N'" + Values[i][1].Replace("'", "''").Replace(@"""", "&quot;") + "'");
						}
					}
				}
				if (Values.Count > 1) {
					return "INSERT INTO " + TableName + " (" + Left.ToString().Substring(1) + ") VALUES (" +
							Right.ToString().Substring(1) + "); " +
							FinalSelect[0];
				} else {
					return "";
				}
			} else {
				StringBuilder Left = new StringBuilder();
				StringBuilder Right = new StringBuilder();
				for (var i = 0; i < Values.Count; i++) {
					if (i == 0) {
						Right.Append("[" + Values[i][0] + "]=" + Values[i][1].Replace("'", "''").Replace(@"""", "&quot;") + "");
						continue;
					}


					string DataType = "string";
					if (Values[i].Length > 2) {
						DataType = "Number";
						if (Values[i][2] == "char" || Values[i][2] == "nchar" || Values[i][2] == "nvarchar" || Values[i][2] == "varbinary" || Values[i][2] == "varchar") {
							DataType = "string";
						}
					}

					if (DataType == "Number") {
						if (Values[i][1] == Setting.DBNotChangedString) {
							continue;
						} else if (Values[i][1] + "" == "") {
							Left.Append(", [" + Values[i][0] + "]=NULL");
						} else {
							Left.Append(", [" + Values[i][0] + "]=" + Values[i][1].Replace("'", "''").Replace(@"""", "&quot;") + "");
						}
					} else {
						if (Values[i][1] == Setting.DBNotChangedString) {
							continue;
						} else if (Values[i][1] + "" == "") {
							Left.Append(", [" + Values[i][0] + "]=NULL");
						} else {
							Left.Append(", [" + Values[i][0] + "]=N'" + Values[i][1].Replace("'", "''").Replace(@"""", "&quot;") + "'");
						}
					}
				}
				if (Values.Count > 1) {
					return "	UPDATE " + TableName + " SET " + Left.ToString().Substring(1) + " WHERE " + Right + "; " +
							FinalSelect[1];
				} else {
					return "";
				}
			}
		}
		public static void Excute(string Query, DBConn DB = null) {
			if (DB == null) {
				DB = new DBConn();
			}
			bool isOpen = false;
			if (DB.DBCon.State != System.Data.ConnectionState.Open) {
				DB.DBCon.Open();
				isOpen = true;
			}
			DB.SqlCmd.CommandText = Query;
			DB.SqlCmd.ExecuteScalar();
			if (isOpen) {
				DB.DBCon.Close();
			}
		}
		public static string ExecuteScalar(string Query, DBConn DB = null) {
			if (DB == null) {
				DB = new DBConn();
			}
			bool isOpen = false;
			if (DB.DBCon.State != System.Data.ConnectionState.Open) {
				DB.DBCon.Open();
				isOpen = true;
			}
			DB.SqlCmd.CommandText = Query;
			string ReturnValue = DB.SqlCmd.ExecuteScalar() + "";
			if (isOpen) {
				DB.DBCon.Close();
			}
			return ReturnValue;
		}
		public static DataTable SelectAnyQuery(string Query, DBConn DB = null) {
			if (DB == null) {
				DB = new DBConn();
			}
			bool isOpen = false;
			if (DB.DBCon.State != System.Data.ConnectionState.Open) {
				DB.DBCon.Open();
				isOpen = true;
			}
			DataTable ReturnValue = new DataTable();
			DB.SqlCmd.CommandText = Query;
			SqlDataAdapter DA = new SqlDataAdapter(DB.SqlCmd);
			DA.Fill(ReturnValue);
			if (isOpen) {
				DB.DBCon.Close();
			}
			return ReturnValue;
		}
	}
}