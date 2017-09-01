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
using Components;

public partial class Board_C_Write : System.Web.UI.Page
{

	private String BoardCode;
	protected String AccountID;
	protected String Name;
	private String Position;
	private DBConn DB;
	private String Mode;
	protected String AttachedFiles = "";
	private StringBuilder ImgTag = new StringBuilder();
    protected String CompanyPk;
	protected void Page_Load(object sender, EventArgs e)
	{
		BoardCode = Request.Params["C"] + "";
		try {
			string[] Memberinfo = Session["MemberInfo"].ToString().Split(new char[] { '!' }, StringSplitOptions.None);
			Name = Memberinfo[3];
			AccountID = Memberinfo[2];
            CompanyPk = Memberinfo[1];
		} catch (Exception) {
			Response.Redirect("../Default.aspx");
		}

		EditorDefaultSetting();
		LoadBoardHeaderHTML();
		Mode = Request.Params["M"] + "";
		if (!IsPostBack) {
			if (Mode == "Modify") {
				LoadModify(Request.Params["P"] + "");
				LoadAttachedFiles(Request.Params["P"] + "");
			}
			if (Mode == "Reply") {
				LoadReply(Request.Params["P"] + "");
			}
		}
	}
    
	private void EditorDefaultSetting()
	{
		CKEditor1.config.toolbar = new object[]
		{
			new object[] { "Source", "Undo", "Redo", "-", "Save", "NewPage", "Preview", "-", "Templates" },
			new object[] { "Cut", "Copy", "Paste", "PasteText", "PasteFromWord", "-", "Print"},
			new object[] { "Link"},
			"/",
			new object[] { "Bold", "Italic", "Underline", "Strike", "-", "Subscript", "Superscript" },
			new object[] { "NumberedList", "BulletedList", "-", "Outdent", "Indent"},
			new object[] { "JustifyLeft", "JustifyCenter", "JustifyRight", "JustifyBlock" },
			new object[] { "Table", "HorizontalRule", "Smiley", "SpecialChar", "PageBreak"},
			"/",
			new object[] { "Styles", "Format", "Font", "FontSize" },
			new object[] { "TextColor", "BGColor" },
			new object[] { "Maximize", "ShowBlocks", "-", "About" }
		};
	}
	private Boolean LoadBoardHeaderHTML()
	{
		DB = new DBConn();
		DB.SqlCmd.CommandText = "SELECT [Pk], [Header] FROM [BoardLibHeader] WHERE BoardCode='" + BoardCode + "' ORDER BY OrderBy;";
		StringBuilder ReturnHTML = new StringBuilder();
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			HTMLHeader.Items.Add(RS["Header"] + "");
		}
		RS.Dispose();
		DB.DBCon.Close();
		if (HTMLHeader.Items.Count == 0) {
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
		if (RS.Read()) {
			HTMLTitle.Text = "[RE]" + RS["Title"] + "";
			HTMLHeader.SelectedValue = RS["Header"] + "";
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			return false;
		}
		RS.Dispose();
		DB.DBCon.Close();
		return true;
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
		if (RS.Read()) {
			HTMLTitle.Text = RS["Title"] + "";
			CKEditor1.Text = RS["Contents"] + "";
			HTMLHeader.SelectedValue = RS["Header"] + "";
		} else {
			RS.Dispose();
			DB.DBCon.Close();
			return false;
		}
		RS.Dispose();
		DB.DBCon.Close();
		return true;
	}
	protected void BTN_Save(object sender, EventArgs e)
	{
		if (HTMLTitle.Text == "") {
			string javascript = "<script>" +
				"alert(\"저장할수 없습니다.\");" +
				"</script>";
			Response.Write(javascript);
		}
		if (File0.PostedFile.FileName != "") {UploadGetByteCount(File0);}
		if (File1.PostedFile.FileName != "") {UploadGetByteCount(File1);}
		if (File2.PostedFile.FileName != "") {UploadGetByteCount(File2);}
		if (File3.PostedFile.FileName != "") {UploadGetByteCount(File3);}
		if (File4.PostedFile.FileName != "") {UploadGetByteCount(File4);}
		if (File5.PostedFile.FileName != "") {UploadGetByteCount(File5);}
		if (File6.PostedFile.FileName != "") {UploadGetByteCount(File6);}
		if (File7.PostedFile.FileName != "") {UploadGetByteCount(File7);}
		if (File8.PostedFile.FileName != "") {UploadGetByteCount(File8);}
		if (File9.PostedFile.FileName != "") {UploadGetByteCount(File9);}
		else {
			if (Mode == "Modify") {
				//Response.Write(HTMLTitle.Text);
				DB.SqlCmd.CommandText = "UPDATE [BoardContents] SET [Header] =" + Common.StringToDB(HTMLHeader.SelectedItem.Text, true, false) +
					", [Title] = " + Common.StringToDB(HTMLTitle.Text, true, true) +
					", [Contents] = " + Common.StringToDB(CKEditor1.Text, true, true) +
					" WHERE Pk=" + Request.Params["P"] + ";";
				//Response.Write(DB.SqlCmd.CommandText);
				DB.DBCon.Open();
				DB.SqlCmd.ExecuteNonQuery();
				string identity = Request.Params["P"] + "";

				if (File0.PostedFile.FileName != "") { SajinUpload(File0, identity); }
				if (File1.PostedFile.FileName != "") { SajinUpload(File1, identity); }
				if (File2.PostedFile.FileName != "") { SajinUpload(File2, identity); }
				if (File3.PostedFile.FileName != "") { SajinUpload(File3, identity); }
				if (File4.PostedFile.FileName != "") { SajinUpload(File4, identity); }
				if (File5.PostedFile.FileName != "") { SajinUpload(File5, identity); }
				if (File6.PostedFile.FileName != "") { SajinUpload(File6, identity); }
				if (File7.PostedFile.FileName != "") { SajinUpload(File7, identity); }
				if (File8.PostedFile.FileName != "") { SajinUpload(File8, identity); }
				if (File9.PostedFile.FileName != "") { SajinUpload(File9, identity); }
				if (ImgTag + "" != "") {
					DB.SqlCmd.CommandText = "update [BoardContents] SET Contents=" + Common.StringToDB(CKEditor1.Text + ImgTag, true, true) + "  WHERE Pk=" + identity + ";";
					DB.SqlCmd.ExecuteNonQuery();
				}
				DB.DBCon.Close();
				Response.Redirect("C_View.aspx?C=" + BoardCode + "&P=" + Request.Params["P"]);
			} else if (Mode == "Reply") {
				DB.SqlCmd.CommandText = @"
  DECLARE @Pk int; 
  DECLARE @ParentsPk int;
  DECLARE @Position varchar(20);
  SET @Pk=" + Request.Params["P"] + @";
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
					Common.StringToDB(HTMLHeader.SelectedItem.Text, true, false) + ", " +
					"@NextPosition, " +
					"@ParentsPk, " +
					Common.StringToDB(HTMLTitle.Text, true, true) + ", " +
					Common.StringToDB(CKEditor1.Text, true, true) + ", " +
					Common.StringToDB(AccountID, true, false) + ", " +
					Common.StringToDB(Name, true, true) + "); SELECT @@IDENTITY; ";
				//Response.Write(DB.SqlCmd.CommandText);
				DB.DBCon.Open();
				string identity = DB.SqlCmd.ExecuteScalar() + "";

				if (File0.PostedFile.FileName != "") { SajinUpload(File0, identity); }
				if (File1.PostedFile.FileName != "") { SajinUpload(File1, identity); }
				if (File2.PostedFile.FileName != "") { SajinUpload(File2, identity); }
				if (File3.PostedFile.FileName != "") { SajinUpload(File3, identity); }
				if (File4.PostedFile.FileName != "") { SajinUpload(File4, identity); }
				if (File5.PostedFile.FileName != "") { SajinUpload(File5, identity); }
				if (File6.PostedFile.FileName != "") { SajinUpload(File6, identity); }
				if (File7.PostedFile.FileName != "") { SajinUpload(File7, identity); }
				if (File8.PostedFile.FileName != "") { SajinUpload(File8, identity); }
				if (File9.PostedFile.FileName != "") { SajinUpload(File9, identity); }
				if (ImgTag + "" != "") {
					DB.SqlCmd.CommandText = "update [BoardContents] SET Contents=" + Common.StringToDB(ImgTag + CKEditor1.Text, true, true) + "  WHERE Pk=" + identity + ";";
					DB.SqlCmd.ExecuteNonQuery();
				}
				DB.DBCon.Close();
				Response.Redirect("C_List.aspx?C=" + BoardCode);
			} else {
				Position = "";
				string ParentsPk = "";
				DB.SqlCmd.CommandText = "INSERT INTO [BoardContents] ([BoardCode], [Header], [Position], [ParentsPk], [Title], [Contents], [AccountID], [Name]) VALUES (" +
					Common.StringToDB(BoardCode, true, false) + ", " +
					Common.StringToDB(HTMLHeader.SelectedItem.Text, true, false) + ", " +
					Common.StringToDB(Position, true, false) + ", " +
					Common.StringToDB(ParentsPk, false, false) + ", " +
					Common.StringToDB(HTMLTitle.Text, true, true) + ", " +
					Common.StringToDB(CKEditor1.Text, true, true) + ", " +
					Common.StringToDB(AccountID, true, false) + ", " +
					Common.StringToDB(Name, true, true) + "); SELECT @@IDENTITY; ";
				DB.DBCon.Open();
				string identity = DB.SqlCmd.ExecuteScalar() + "";
				DB.SqlCmd.CommandText = "UPDATE [BoardContents] SET [ParentsPk] = " + identity + " WHERE Pk=" + identity + ";";
				DB.SqlCmd.ExecuteNonQuery();

				if (File0.PostedFile.FileName != "") { SajinUpload(File0, identity); }
				if (File1.PostedFile.FileName != "") { SajinUpload(File1, identity); }
				if (File2.PostedFile.FileName != "") { SajinUpload(File2, identity); }
				if (File3.PostedFile.FileName != "") { SajinUpload(File3, identity); }
				if (File4.PostedFile.FileName != "") { SajinUpload(File4, identity); }
				if (File5.PostedFile.FileName != "") { SajinUpload(File5, identity); }
				if (File6.PostedFile.FileName != "") { SajinUpload(File6, identity); }
				if (File7.PostedFile.FileName != "") { SajinUpload(File7, identity); }
				if (File8.PostedFile.FileName != "") { SajinUpload(File8, identity); }
				if (File9.PostedFile.FileName != "") { SajinUpload(File9, identity); }

				if (ImgTag + "" != "") {
					DB.SqlCmd.CommandText = "update [BoardContents] SET Contents=" + Common.StringToDB(ImgTag + CKEditor1.Text, true, true) + "  WHERE Pk=" + identity + ";";
					DB.SqlCmd.ExecuteNonQuery();
				}
				DB.DBCon.Close();
				Response.Redirect("C_List.aspx?C=" + BoardCode);
			}
		}
	}
	private void UploadGetByteCount(HtmlInputFile Sajin)
	{
		string filename = Sajin.PostedFile.FileName.IndexOf("\\") == -1 ? Sajin.PostedFile.FileName : Sajin.PostedFile.FileName.Substring(Sajin.PostedFile.FileName.LastIndexOf("\\") + 1);
		int count = Encoding.Default.GetByteCount(filename);
		if (count > 49) { Response.Write("<script type='text/javascript'>alert('파일의 이름이 너무 깁니다'); </script>"); }
	}
	private void SajinUpload(HtmlInputFile Sajin, string BoardPk)
	{
		string filename = Sajin.PostedFile.FileName.IndexOf("\\") == -1 ? Sajin.PostedFile.FileName : Sajin.PostedFile.FileName.Substring(Sajin.PostedFile.FileName.LastIndexOf("\\") + 1);

		DB.SqlCmd.CommandText = "INSERT INTO [BoardAttachedFile] ([ContentsPk], [FileTitle], [PhysicalPath]) VALUES (" + BoardPk + ", " + Common.StringToDB(filename, true, true) + ", NULL); SELECT @@IDENTITY;";
		string identity = DB.SqlCmd.ExecuteScalar() + "";
		DB.SqlCmd.CommandText = "UPDATE [BoardAttachedFile] SET [PhysicalPath] = " + Common.StringToDB("B/" + identity + "_" + filename, true, true) + " WHERE [Pk]=" + identity + ";";
		DB.SqlCmd.ExecuteNonQuery();
		string filepath = Server.MapPath("~/UploadedFiles/B/") + identity + "_" + filename;
		Sajin.PostedFile.SaveAs(filepath);
        if (filename.Substring(filename.Length - 3) == "jpg" || filename.Substring(filename.Length - 3) == "JPG" || filename.Substring(filename.Length - 3) == "gif" || filename.Substring(filename.Length - 3) == "GIF" || filename.Substring(filename.Length - 3) == "png" || filename.Substring(filename.Length - 3) == "PNG")
        {
			Bitmap img = new Bitmap(filepath);

			decimal width = img.Width;
			decimal height = img.Height;
			while (width > 850) {
				width *= 0.9M;
				height *= 0.9M;
			}
			ImgTag.Append("<div><img src=\"../UploadedFiles/B/" + identity + "_" + filename + "\" style=\"width:" + Math.Round(width).ToString() + "px; height:" + Math.Round(height).ToString() + "px;\" /></div>");
		}

	}
	private Boolean LoadAttachedFiles(string Pk)
	{
		DB.SqlCmd.CommandText = "SELECT [Pk], [FileTitle] FROM [BoardAttachedFile] WHERE ContentsPk=" + Pk + " and isnull(Deleted, '')='';";
		string eachformat = "<p><a href='../UploadedFiles/FileDownload.aspx?S={0}&T=Board' >{1}</a> <span style=\"color:red;\" onclick=\"FileDelete('{0}');\">X</span></p>";
		StringBuilder ReturnValue = new StringBuilder();
		DB.DBCon.Open();
		SqlDataReader RS = DB.SqlCmd.ExecuteReader();
		while (RS.Read()) {
			ReturnValue.Append(string.Format(eachformat, RS["Pk"] + "", RS["FileTitle"] + ""));
		}
		RS.Dispose();
		DB.DBCon.Close();
		AttachedFiles = ReturnValue + "";
		return true;
	}

	protected void Button2_Click(object sender, EventArgs e)
	{
		HtmlInputFile Sajin = File0;
		string filepath = Server.MapPath("~/UploadedFiles/B/") + "201207291012.jpg";
		string filename = "201207291012.jpg";
		if (filename.Substring(filename.Length - 3) == "jpg" || filename.Substring(filename.Length - 3) == "gif") {
			Bitmap img = new Bitmap(filepath);
			decimal width = decimal.Parse(img.Width.ToString());
			decimal height = img.Height;

			while (width > 850) {
				width *= 0.9M;
				height *= 0.9M;
			}

		}

	}
}