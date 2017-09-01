using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Components;

public partial class Admin_Dialog_SetWarehouse : System.Web.UI.Page
{
	protected String S;

	protected String TITLE;
	protected String StorageCode;
	protected String Address;
	protected String TEL;
	protected String FAX;
	protected String X;
	protected String Y;
    protected void Page_Load(object sender, EventArgs e)
    {
		S = Request.Params["S"] + "";
		if (S == "" || S == "New") {
			TITLE = "";
			StorageCode = "";
			Address = "";
			TEL = "";
			FAX = "";
			X = "";
			Y = "";
		} else {
			DBConn DB = new DBConn();
			DB.SqlCmd.CommandText = @"SELECT [StorageName]
	  ,[StorageCode]
      ,[StorageAddress]
      ,[TEL]
      ,[FAX]
      ,[StaffName]
      ,[StaffMobile]
      ,[Description]
      ,[NaverMapPathX]
      ,[NaverMapPathY]
      ,[Memo]
      ,[IsUse]
  FROM [dbo].[OurBranchStorageCode]
   WHERE [OurBranchStoragePk]=" + S + "; ";
			DB.DBCon.Open();
			SqlDataReader RS = DB.SqlCmd.ExecuteReader();
			if (RS.Read()) {
				TITLE = RS["StorageName"] + "";
				StorageCode=RS["StorageCode"]+"";
				Address = RS["StorageAddress"] + "";
				TEL = RS["TEL"] + "";
				FAX = RS["FAX"] + "";
				X = RS["NaverMapPathX"] + "";
				Y = RS["NaverMapPathY"] + "";
			}
			RS.Dispose();
			DB.DBCon.Close();
		}
    }
}