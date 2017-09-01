using Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Board_T_SetBoard : System.Web.UI.Page
{
    private DBConn DB;
    private StringBuilder ImgTag = new StringBuilder();
    protected void Page_Load(object sender, EventArgs e)
    {
        string Division = Request.Params["FileUpload_Type"];
        string AccountID = Request.Params["AccountID"];
        string Name = Request.Params["Name"];
        string Content = Request.Params["content"];
        string BoardCode = Request.Params["BoardCode"];
        string HTMLTitle = Request.Params["HTMLTitle"];
        string HTMLHeader = Request.Params["HTMLHeader"];
        string Pk = Request.Params["Pk"];
        DB = new DBConn();
        switch (Division)
        {
            case "New":
                string Position = "";
                string ParentsPk = "";
                DB.SqlCmd.CommandText = "INSERT INTO [BoardContents] ([BoardCode], [Header], [Position], [ParentsPk], [Title], [Contents], [AccountID], [Name]) VALUES (" +
                    Common.StringToDB(BoardCode, true, false) + ", " +
                    Common.StringToDB(HTMLHeader, true, false) + ", " +
                    Common.StringToDB(Position, true, false) + ", " +
                    Common.StringToDB(ParentsPk, false, false) + ", " +
                    Common.StringToDB(HTMLTitle, true, true) + ", " +
                    Common.StringToDB(Content, true, true) + ", " +
                    Common.StringToDB(AccountID, true, false) + ", " +
                    Common.StringToDB(Name, true, true) + "); SELECT @@IDENTITY; ";
                DB.DBCon.Open();
                string identity = DB.SqlCmd.ExecuteScalar() + "";
                DB.SqlCmd.CommandText = "UPDATE [BoardContents] SET [ParentsPk] = " + identity + " WHERE Pk=" + identity + ";";
                DB.SqlCmd.ExecuteNonQuery();

                foreach (string file in Request.Files)
                {
                    HttpPostedFile uploaded_file = Request.Files[file] as HttpPostedFile;
                    if (uploaded_file.ContentLength > 0)
                    {
                        SajinUpload(uploaded_file, identity);
                    }
                }
                if (ImgTag + "" != "")
                {

                    DB.SqlCmd.CommandText = "update [BoardContents] SET Contents=" + Common.StringToDB(ImgTag + Content, true, true) + "  WHERE Pk=" + identity + ";";
                    DB.SqlCmd.ExecuteNonQuery();
                }
                DB.DBCon.Close();
                Response.Redirect("~/Board/C_List.aspx?C=" + BoardCode);
                break;
            case "Modify":
                DB.SqlCmd.CommandText = "UPDATE [BoardContents] SET [Header] =" + Common.StringToDB(HTMLHeader, true, false) +
                    ", [Title] = " + Common.StringToDB(HTMLTitle, true, true) +
                    ", [Contents] = " + Common.StringToDB(Content, true, true) +
                    " WHERE Pk=" + Pk + ";";
                DB.DBCon.Open();
                DB.SqlCmd.ExecuteNonQuery();
                identity = Pk;

                foreach (string file in Request.Files)
                {
                    HttpPostedFile uploaded_file = Request.Files[file] as HttpPostedFile;
                    if (uploaded_file.ContentLength > 0)
                    {
                        SajinUpload(uploaded_file, identity);
                    }
                }
                if (ImgTag + "" != "")
                {
                    DB.SqlCmd.CommandText = "update [BoardContents] SET Contents=" + Common.StringToDB(Content + ImgTag, true, true) + "  WHERE Pk=" + identity + ";";
                    DB.SqlCmd.ExecuteNonQuery();
                }
                DB.DBCon.Close();
                Response.Redirect("~/Board/C_View.aspx?C=" + BoardCode + "&P=" + Pk);
                break;
            case "Reply":
                DB.SqlCmd.CommandText = @"
  DECLARE @Pk int; 
  DECLARE @ParentsPk int;
  DECLARE @Position varchar(20);
  SET @Pk=" + Pk + @";
  DECLARE @FrontPosition varchar(20);
  DECLARE @NextPosition varchar(20);
  DECLARE @TempINT int;
  SELECT @Position=Position, @ParentsPk=ParentsPk From BoardContents WHERE Pk=@Pk;
  if isnull(@Position, '')='' 
	BEGIN 
		SELECT TOP 1 @FrontPosition=Position FROM BoardContents WHERE len(Position)=2 and ParentsPk=@ParentsPk ;
		SET @TempINT=CAST (right(isnull(@FrontPosition, '00'), 2) AS INT)+1; 

		if (@TempINT<10) 
			BEGIN 
				SELECT @NextPosition='0'+CAST (@TempINT AS varchar); 
			END 
		else 
			BEGIN 
				SELECT @NextPosition=CAST (@TempINT AS varchar); 
			END 

	END 
else 
	BEGIN 
		SELECT TOP 1 @FrontPosition=Position FROM BoardContents WHERE len(Position)=len(@Position)+3 and ParentsPk=@ParentsPk and left(Position, 2)=@Position ; 
		SET @TempINT=CAST (right(isnull(@FrontPosition, '00'), 2) AS INT)+1; 
		if (@TempINT<10) 
			BEGIN SELECT @NextPosition=isnull(@Position, '')+'_0'+CAST (@TempINT AS varchar); END 
		else 
			BEGIN SELECT @NextPosition=isnull(@Position, '')+'_'+CAST (@TempINT AS varchar); END 
	END 

INSERT INTO [BoardContents] ([BoardCode], [Header], [Position], [ParentsPk], [Title], [Contents], [AccountID], [Name]) VALUES (" +
                   Common.StringToDB(BoardCode, true, false) + ", " +
                   Common.StringToDB(HTMLHeader, true, false) + ", " +
                   "@NextPosition, " +
                   "@ParentsPk, " +
                   Common.StringToDB(HTMLTitle, true, true) + ", " +
                   Common.StringToDB(Content, true, true) + ", " +
                   Common.StringToDB(AccountID, true, false) + ", " +
                   Common.StringToDB(Name, true, true) + "); SELECT @@IDENTITY; ";
                DB.DBCon.Open();
                identity = DB.SqlCmd.ExecuteScalar() + "";

                foreach (string file in Request.Files)
                {
                    HttpPostedFile uploaded_file = Request.Files[file] as HttpPostedFile;
                    if (uploaded_file.ContentLength > 0)
                    {
                        SajinUpload(uploaded_file, identity);
                    }
                }
                if (ImgTag + "" != "")
                {
                    DB.SqlCmd.CommandText = "update [BoardContents] SET Contents=" + Common.StringToDB(ImgTag + Content, true, true) + "  WHERE Pk=" + identity + ";";
                    DB.SqlCmd.ExecuteNonQuery();
                }
                DB.DBCon.Close();
                Response.Redirect("~/Board/C_List.aspx?C=" + BoardCode);
                break;
            default:
                break;
        }
    }
    private void UploadGetByteCount(string filename)
    {
        int count = Encoding.Default.GetByteCount(filename);
        if (count > 49) { Response.Write("<script type='text/javascript'>alert('파일의 이름이 너무 깁니다'); </script>"); }
    }
    private void SajinUpload(HttpPostedFile Sajin, string BoardPk)
    {
        string filename = Sajin.FileName.IndexOf("\\") == -1 ? Sajin.FileName : Sajin.FileName.Substring(Sajin.FileName.LastIndexOf("\\") + 1);

        DB.SqlCmd.CommandText = "INSERT INTO [BoardAttachedFile] ([ContentsPk], [FileTitle], [PhysicalPath]) VALUES (" + BoardPk + ", " + Common.StringToDB(filename, true, true) + ", NULL); SELECT @@IDENTITY;";
        string identity = DB.SqlCmd.ExecuteScalar() + "";
        DB.SqlCmd.CommandText = "UPDATE [BoardAttachedFile] SET [PhysicalPath] = " + Common.StringToDB("B/" + identity + "_" + filename, true, true) + " WHERE [Pk]=" + identity + ";";
        DB.SqlCmd.ExecuteNonQuery();
        string filepath = Server.MapPath("~/UploadedFiles/B/") + identity + "_" + filename;
        Sajin.SaveAs(filepath);

        if (filename.Substring(filename.Length - 3) == "jpg" || filename.Substring(filename.Length - 3) == "JPG" || filename.Substring(filename.Length - 3) == "gif" || filename.Substring(filename.Length - 3) == "GIF" || filename.Substring(filename.Length - 3) == "png" || filename.Substring(filename.Length - 3) == "PNG")
        {
            Bitmap img = new Bitmap(filepath);

            decimal width = img.Width;
            decimal height = img.Height;
            while (width > 850)
            {
                width *= 0.9M;
                height *= 0.9M;
            }
            ImgTag.Append("<div><img src=\"../UploadedFiles/B/" + identity + "_" + filename + "\" style=\"width:" + Math.Round(width).ToString() + "px; height:" + Math.Round(height).ToString() + "px;\" /></div>");
        }

    }
}