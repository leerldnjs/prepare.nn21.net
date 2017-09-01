using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Components
{
	public class DBConn
	{
        public static DBConn DB;
        public SqlConnection DBCon;
		public SqlCommand SqlCmd;
		public DBConn() {
			DBCon = new SqlConnection();
			//DBCon.ConnectionString = "Data Source=192.168.77.23; Initial Catalog=INTL2010;Persist Security Info=True;User ID=intl2000;Password=nn@!il;";
			//DBCon.ConnectionString = "Data Source=192.168.88.5; Initial Catalog=INTL2010;Persist Security Info=True;User ID=intl2000;Password=nn@!il;";
			DBCon.ConnectionString = "Data Source=192.168.88.7; Initial Catalog=INTL2010;Persist Security Info=True;User ID=intl2000;Password=nn@!il;";
			SqlCmd = new SqlCommand();
			SqlCmd.Connection = DBCon;
		}

		public DBConn(string Value) {
			DBCon = new SqlConnection();
			switch (Value) {
				case "ReadyKorea":
					//DBCon.ConnectionString = "Data Source=192.168.77.23; Initial Catalog=edicus;Persist Security Info=True;User ID=edicus;Password=fpelchlrh;Pooling=false;";
					//DBCon.ConnectionString = "Data Source=192.168.88.5; Initial Catalog=edicus;Persist Security Info=True;User ID=edicus;Password=fpelchlrh;Pooling=false;";
					DBCon.ConnectionString = "Data Source=192.168.88.7; Initial Catalog=edicus;Persist Security Info=True;User ID=edicus;Password=fpelchlrh;Pooling=false;";
					break;
				case "YuhanN":
					//DBCon.ConnectionString = "Data Source=192.168.77.25,6277;Initial Catalog=INTL_uFMS;Persist Security Info=True;User ID=SA;Password=!Q@W3e4r;Pooling=false;";
					//DBCon.ConnectionString = "Data Source=192.168.88.11,6277;Initial Catalog=INTL_uFMS;Persist Security Info=True;User ID=SA;Password=!Q@W3e4r;Pooling=false;";
					break;
				case "PoolingNo":
					//DBCon.ConnectionString = "Data Source=192.168.77.23; Initial Catalog=INTL2010;Persist Security Info=True;User ID=intl2000;Password=nn@!il;Pooling=false;";
					//DBCon.ConnectionString = "Data Source=192.168.88.5; Initial Catalog=INTL2010;Persist Security Info=True;User ID=intl2000;Password=nn@!il;Pooling=false;";
					DBCon.ConnectionString = "Data Source=192.168.88.7; Initial Catalog=INTL2010;Persist Security Info=True;User ID=intl2000;Password=nn@!il;Pooling=false;";
					break;
				default:
					//DBCon.ConnectionString = "Data Source=192.168.77.23; Initial Catalog=INTL2010;Persist Security Info=True;User ID=intl2000;Password=nn@!il;";
					//DBCon.ConnectionString = "Data Source=192.168.88.5; Initial Catalog=INTL2010;Persist Security Info=True;User ID=intl2000;Password=nn@!il;";
					DBCon.ConnectionString = "Data Source=192.168.88.7; Initial Catalog=INTL2010;Persist Security Info=True;User ID=intl2000;Password=nn@!il;";

					break;
			}
			SqlCmd = new SqlCommand();
			SqlCmd.Connection = DBCon;
		}
	}
}