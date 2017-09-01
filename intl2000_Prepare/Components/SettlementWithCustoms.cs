using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
	public class SettlementWithCustoms
	{
		DBConn DB;
		public SettlementWithCustoms()
		{
			DB = new DBConn();
		}

		public String Query_SetSettlement(string BLNo, string Date, string Price, string Description, string AccountId)
		{
			string Format_Query = @"INSERT INTO [dbo].[SettlementWithCustoms] ([BLNo],[Date],[Price],[Description],[AccountId]) VALUES ({0}, {1}, {2}, {3}, {4});";
			return string.Format(Format_Query,
				Common.StringToDB(BLNo, true, false),
				Common.StringToDB(Date, true, false),
				Common.StringToDB(Price, false, false),
				Common.StringToDB(Description, true, false),
				Common.StringToDB(AccountId, true, false));
		}
	}
}
