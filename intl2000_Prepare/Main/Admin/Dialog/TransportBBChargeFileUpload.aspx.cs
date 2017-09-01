using Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admin_Dialog_TransportBBChargeFileUpload : System.Web.UI.Page
{
   protected String IsUpload = "N";
   protected String GubunCL;
   protected String SessionConpanyPk;
   protected String FileList;
   private String AccountID;
   private DBConn DB;
    protected void Page_Load(object sender, EventArgs e)
    {
       try
       {
          AccountID = Session["MemberInfo"].ToString().Split(Common.Splite11, StringSplitOptions.None)[2];
       }
       catch (Exception)
       {
          Response.Redirect("../../Default.aspx");
       }
       GubunCL = Request.Params["G"] + "";
       SessionConpanyPk = Request.Params["P"] + "" == "" ? "3157" : Request.Params["P"] + "";
       Response.Expires = 0;
       Response.Cache.SetNoStore();
       Response.AppendHeader("Pragma", "no-cache");
       DB = new DBConn();    
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
       SetTransportBBCharge();
      
       IsUpload = "Y";
    }


    private string SetTransportBBCharge()
    {
       DB.SqlCmd.CommandText = "INSERT INTO [dbo].[TransportBBCharge]([TransportBBHeadPk],[PaymentBranchPk],[Date],[Title],[MonetaryUnitCL],[Price],[Comment],[Registerd])" +
     "VALUES(" + Request.Params["HeadPk"].ToString() + "," + SessionConpanyPk + ",'" + Request.Params["Date"].ToString() + "',N'" + Request.Params["Title"].ToString() + "','" + Request.Params["MonetaryUnit"].ToString() + "','" + Request.Params["Price"].ToString() + "',N'" + Request.Params["TB_Comment"].ToString() + "',getdate());SELECT @@IDENTITY;";
      DB.DBCon.Open();
      string Pk = DB.SqlCmd.ExecuteScalar() + "";
      DB.DBCon.Close();
      if (FILE0.PostedFile.FileName != "") { SajinUpload(FILE0, Pk, Request.Params["TB0"] + ""); }
      if (FILE1.PostedFile.FileName != "") { SajinUpload(FILE1, Pk, Request.Params["TB1"] + ""); }
      if (FILE2.PostedFile.FileName != "") { SajinUpload(FILE2, Pk, Request.Params["TB2"] + ""); }
      if (FILE3.PostedFile.FileName != "") { SajinUpload(FILE3, Pk, Request.Params["TB3"] + ""); }
      return "1";

}
    private void SajinUpload(HtmlInputFile Sajin, string GubunPk, string Title)
    {
      
       string filename;
       filename = Sajin.PostedFile.FileName.IndexOf("\\") == -1 ? Sajin.PostedFile.FileName : Sajin.PostedFile.FileName.Substring(Sajin.PostedFile.FileName.LastIndexOf("\\") + 1);

     
             DB.SqlCmd.CommandText = "	INSERT INTO [INTL2010].[dbo].[File] ([Title], [GubunCL], [GubunPk], [FileName], AccountID, [Registerd]) VALUES (" +
                                                    Common.StringToDB(Title, true, true) + ", " + GubunCL + ", " + GubunPk + ", " + Common.StringToDB(filename, true, true) + ", '" + AccountID + "' , getDate()); SELECT @@IDENTITY;";
       

       DB.DBCon.Open();
       string identity = DB.SqlCmd.ExecuteScalar() + "";
       DB.SqlCmd.CommandText = "Update [File] SET [PhysicalPath]=N'" + GubunCL + "/" + GubunPk + "_" + identity + "_" + filename + "' WHERE [FilePk]=" + identity + ";";
       DB.SqlCmd.ExecuteNonQuery();
       DB.DBCon.Close();
       Sajin.PostedFile.SaveAs(Server.MapPath("~/UploadedFiles/" + GubunCL + "/") + GubunPk + "_" + identity + "_" + filename);
    }
}