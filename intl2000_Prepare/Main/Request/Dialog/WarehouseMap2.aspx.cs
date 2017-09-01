using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Request_Dialog_WarehouseMap2 : System.Web.UI.Page
{
	protected String Title, TEL, FAX, Address, NaverX, NaverY;
	protected void Page_Load(object sender, EventArgs e)
	{
		string WarehouseCode = Request.Params["S"] + "";
		DBConn DB = new DBConn();
		DB.SqlCmd.CommandText = @"SELECT [OurBranchStoragePk]
      ,[OurBranchCode]
      ,[StorageName]
      ,[StorageAddress]
      ,[TEL]
      ,[StaffName]
      ,[StaffMobile]
      ,[Description]
      ,[NaverMapPathX]
      ,[NaverMapPathY]
      ,[Memo]
  FROM [INTL2010].[dbo].[OurBranchStorageCode]
   WHERE OurBranchStoragePk=" + WarehouseCode + ";";
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		if (RS.Read()) {
			Title = RS["StorageName"] + "";
			Address = RS["StorageAddress"] + "";
			TEL = RS["TEL"] + "";
			NaverX = RS["NaverMapPathX"] + "";
			NaverY = RS["NaverMapPathY"] + "";
		}
		RS.Dispose();
		DB.DBCon.Close();
	}
}