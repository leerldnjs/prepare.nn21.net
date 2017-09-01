using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
	public class Setting
	{
		public Setting() {

		}

		public const string DBNotChangedString = "&N%tCh@n(ed!";

		public static String SelectOurBranch = @"<option value='3157'>KRIC 인천</option>																			
																		<option value='3095'>JPOK Osaka</option>
																		<option value='3843'>CNGZ 广州</option>
																		<option value='2886'>CNYT 烟台</option>
																		<option value='2887'>CNSY 瀋陽</option>
																		<option value='2888'>CNYW 义乌</option>
																		<option value='3388'>CNQD 青岛</option>
																		<option value='11456'>CNHZ 杭州</option>
																		<option value='7898'>CNSX 绍兴</option>                                                                        
																		<option value='3798'>OtherLocation</option>";

		public static string SelectTransportWay = @"<option value=''>선택하세요</option>
                                                    <option value='Air'>항공</option>
                                                    <option value='Car'>차량</option>
                                                    <option value='Ship'>선박</option>
                                                    <option value='Sub'>외주</option>";
		public string ToDB(string Value, string DataType) {
			if (Value == "" || Value == null) {
				return "NULL";
			} else if (DataType == "nvarchar" || DataType == "nchar") {
				return "N'" + Value + "'";
			} else if (DataType == "string" || DataType == "varchar" || DataType == "char" || DataType == "datetime") {
				return "'" + Value + "'";
			} else if (DataType == "int" || DataType == "decimal" || DataType == "smallint" || DataType == "tinyint" || DataType == "money") {
				return Value.Replace(",", "");
			} else {
				return Value;
			}
		}

		public SqlDataReader Load_Saved(string Branch_Pk, string Code, ref DBConn DB) {
			List<string[]> ReturnValue = new List<string[]>();
			string Query = @"SELECT 
				MAX([SAVED_PK]) AS SAVED_PK
				,MAX([BRANCH_PK]) AS BRANCH_PK
				,MAX([CODE]) AS CODE
				,[VALUE_0]
				,MAX([VALUE_1]) AS VALUE_1
				,MAX([VALUE_2]) AS VALUE_2
				,MAX([VALUE_3]) AS VALUE_3
				,MAX([VALUE_4]) AS VALUE_4
				,MAX([VALUE_INT_0]) AS VALUE_INT_0
			FROM [dbo].[SAVED] 
            WHERE [BRANCH_PK] = " + Branch_Pk + @"
            AND [CODE] = " + ToDB(Code, "varchar") + @"
			AND [VALUE_0] IS NOT NULL
			GROUP BY [VALUE_0]";

			DB.SqlCmd.CommandText = Query;
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();

			return RS;
		}

		public string Set_Saved(string Branch_Pk, string Code, string Value_0, string Value_1, string Value_2, string Value_3, string Value_4, string Value_Int_0, ref DBConn DB) {
			string Query = @"INSERT INTO [dbo].[SAVED]
           ([BRANCH_PK]
           ,[CODE]
           ,[VALUE_0]
           ,[VALUE_1]
           ,[VALUE_2]
           ,[VALUE_3]
           ,[VALUE_4]
           ,[VALUE_INT_0])
     VALUES
           (" + ToDB(Branch_Pk, "int") + @"
           ," + ToDB(Code, "varchar") + @"
           ," + ToDB(Value_0, "nvarchar") + @"
           ," + ToDB(Value_1, "nvarchar") + @"
           ," + ToDB(Value_2, "nvarchar") + @"
           ," + ToDB(Value_3, "nvarchar") + @"
           ," + ToDB(Value_4, "nvarchar") + @"
           ," + ToDB(Value_Int_0, "int") + @") ";

			DB.SqlCmd.CommandText = Query;
			DB.SqlCmd.ExecuteNonQuery();
			return "1";
		}

		public string Delete_Saved(string Saved_Pk, ref DBConn DB) {
			string Query = @"DELETE FROM [dbo].[SAVED] WHERE [SAVED_PK] = " + ToDB(Saved_Pk, "int") + ";";
			DB.SqlCmd.CommandText = Query;
			DB.SqlCmd.ExecuteNonQuery();
			return "1";
		}
	}
}