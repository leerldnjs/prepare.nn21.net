using Components;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Board_T_Write : System.Web.UI.Page
{
    public String BoardCode;
    public String Mode;
    public String Pk;
    protected String CompanyPk, content, file_image, file_file,AccountID, Name;
    private DBConn DB;    
    private StringBuilder ImgTag = new StringBuilder();
    
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
WHERE C.Pk=@BoardPk and isnull(C.[Deleted], '')='';";
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
        DB.SqlCmd.CommandText = "SELECT [Pk],[FileTitle],[PhysicalPath] FROM [BoardAttachedFile] WHERE ContentsPk=" + Pk + " and isnull(Deleted, '')='';";
        string image_Format = @"
            attachments['image'].push({{
				'attacher': 'image',
				'data': {{
					'imageurl': '{0}',
					'filename': '{1}',					                    
					'originalurl': '{0}',
					'thumburl': '{0}' , 
					'UploadedFilePk' : '{2}'
                    'filesize': 0,
				}}
			}});";

        string file_Format = @"
			attachments['file'].push({{
				'attacher': 'file',
				'data': {{
					'attachurl': '{0}',
					'filemime': '{3}',
					'filename': '{1}',					
					'UploadedFilePk' : '{2}'
                    'filesize': 0,
				}}
			}});";
        StringBuilder ReturnValue = new StringBuilder();
        StringBuilder image_SB = new StringBuilder();
        StringBuilder file_SB = new StringBuilder();
        DB.DBCon.Open();
        SqlDataReader RS = DB.SqlCmd.ExecuteReader();
        string Ext = "";
        while (RS.Read())
        {
            Ext=RS["FileTitle"].ToString().Substring(RS["FileTitle"].ToString().LastIndexOf(".")+1).ToLower();
            if (Ext == "jpg" || Ext == "gif" ||Ext == "png")
            {
                image_SB.Append(string.Format(image_Format, "/UploadedFiles/"+RS["PhysicalPath"].ToString(), RS["FileTitle"].ToString(), RS["Pk"]));
            }
            else {
                file_SB.Append(string.Format(file_Format,"/UploadedFiles/"+ RS["PhysicalPath"].ToString(), RS["FileTitle"].ToString(), RS["Pk"].ToString(),Ext));
            } 
        }
        RS.Dispose();
        DB.DBCon.Close();
        file_file = file_SB.ToString();
        file_image = image_SB.ToString();
        return true;
    }


}