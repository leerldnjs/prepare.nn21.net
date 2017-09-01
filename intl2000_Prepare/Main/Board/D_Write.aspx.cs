using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Board_D_Write : System.Web.UI.Page
{
    public String BoardCode;
    protected String AccountID;
    protected String Name;    
    private DBConn DB;
    public String Mode;
    public String Pk;
    protected String AttachedFiles = "";
    private StringBuilder ImgTag = new StringBuilder();
    protected String CompanyPk;
    protected String content="";
    protected void Page_Load(object sender, EventArgs e)
    {
        BoardCode = Request.Params["C"] + "";
        try
        {
            string[] Memberinfo = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
            Name = Memberinfo[3];
            AccountID = Memberinfo[2];
            CompanyPk = Memberinfo[1];
        }
        catch (Exception)
        {
            Response.Redirect("../Default.aspx");
        }
        LoadBoardHeaderHTML();
        Mode = Request.Params["M"] + "" == "" ? "New" : Request.Params["M"] + "";
        Pk = Request.Params["P"] + "";
        if (!IsPostBack)
        {
            if (Mode == "Modify")
            {
                LoadModify(Request.Params["P"] + "");
                LoadAttachedFiles(Request.Params["P"] + "");
            }
            if (Mode == "Reply")
            {
                LoadReply(Request.Params["P"] + "");
            }
        }
    }


    private Boolean LoadModify(string ContentsPk)
    {
        DB.SqlCmd.CommandText = @"
DECLARE @BoardPk int; 
SET @BoardPk=" + ContentsPk + @";

SELECT C.[Header], C.[Title], C.[Contents] 
FROM [BoardContents] AS C
WHERE C.Pk=@BoardPk and isnull(C.[Deleted], '')='' ;
";
        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        if (RS.Read())
        {
            HTMLTitle.Text = RS["Title"] + "";

            content = RS["Contents"] + "";

            HTMLHeader.SelectedValue = RS["Header"] + "";
        }
        else
        {
            RS.Dispose();
            DB.DBCon.Close();
            return false;
        }
        RS.Dispose();
        DB.DBCon.Close();
        return true;
    }
    private Boolean LoadBoardHeaderHTML()
    {
        DB = new DBConn();
        DB.SqlCmd.CommandText = "SELECT [Pk], [Header] FROM [BoardLibHeader] WHERE BoardCode='" + BoardCode + "' ORDER BY OrderBy;";
        StringBuilder ReturnHTML = new StringBuilder();
        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        while (RS.Read())
        {
            HTMLHeader.Items.Add(RS["Header"] + "");
        }
        RS.Dispose();
        DB.DBCon.Close();
        if (HTMLHeader.Items.Count == 0)
        {
            HTMLHeader.Items.Add("");
        }

        return true;
    }
    private Boolean LoadReply(string ContentsPk)
    {
        DB.SqlCmd.CommandText = @"
DECLARE @BoardPk int; 
SET @BoardPk=" + ContentsPk + @";

SELECT C.[Header], C.[Title]
FROM [BoardContents] AS C
WHERE C.Pk=@BoardPk and isnull(C.[Deleted], '')='' ;
";
        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        if (RS.Read())
        {
            HTMLTitle.Text = "[RE]" + RS["Title"] + "";
            HTMLHeader.SelectedValue = RS["Header"] + "";
        }
        else
        {
            RS.Dispose();
            DB.DBCon.Close();
            return false;
        }
        RS.Dispose();
        DB.DBCon.Close();
        return true;
    }

    private Boolean LoadAttachedFiles(string Pk)
    {
        DB.SqlCmd.CommandText = "SELECT [Pk], [FileTitle] FROM [BoardAttachedFile] WHERE ContentsPk=" + Pk + " and isnull(Deleted, '')='';";
        string eachformat = "<p><a href='../UploadedFiles/FileDownload.aspx?S={0}&T=Board' >{1}</a> <span style=\"color:red;\" onclick=\"FileDelete('{0}');\">X</span></p>";
        StringBuilder ReturnValue = new StringBuilder();
        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        while (RS.Read())
        {
            ReturnValue.Append(string.Format(eachformat, RS["Pk"] + "", RS["FileTitle"] + ""));
        }
        RS.Dispose();
        DB.DBCon.Close();
        AttachedFiles = ReturnValue + "";
        return true;
    }


}