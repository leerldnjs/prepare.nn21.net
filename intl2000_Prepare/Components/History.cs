using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components
{
	public class History
	{

		public string Set_History(string Table_Name, string Table_Pk, string Code, string Description, string Account_Pk, string Account_Name, string Account_Id, ref DBConn DB) {
			Setting setting = new Setting();
			string Before_Data = "";
			string After_Data = "";
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
            (" + setting.ToDB(Table_Name, "varchar") + @"
            ," + setting.ToDB(Table_Pk, "int") + @"
            ," + setting.ToDB(Code, "varchar") + @"
            ," + setting.ToDB(Description, "nvarchar") + @"
            ," + setting.ToDB(Account_Pk, "int") + @"
            ," + setting.ToDB(Account_Name, "nvarchar") + @"  
            ," + setting.ToDB(Account_Id, "varchar") + @"
            ," + setting.ToDB(Before_Data, "nvarchar") + @"
            ," + setting.ToDB(After_Data, "nvarchar") + @") ";
			DB.SqlCmd.CommandText = Query;
			DB.SqlCmd.ExecuteNonQuery();
			return "1";
		}
	}
}