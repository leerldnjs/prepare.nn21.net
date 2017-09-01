<%@ Page Language="C#" %>
<%
	Components.DBConn DB = new Components.DBConn();
	DB.SqlCmd.CommandText = "SELECT [IsRead] FROM EmailHistory WHERE EmailHistoryPk=" + Request.Params["S"] + ";";
	DB.DBCon.Open();
	string Temp = DB.SqlCmd.ExecuteScalar() + "";
	if (Temp != "1")
	{
		DB.SqlCmd.CommandText = "UPDATE EmailHistory SET IsRead=1, ReadDate=getDate() WHERE EmailHistoryPk=" + Request.Params["S"] + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
	}
	else
	{
		DB.DBCon.Close();
	}
	
	string imagePath = Server.MapPath("./JaemuIMGLogo.jpg");
	Response.Clear();
	Response.ContentType = "Image/Jpeg";
	Response.WriteFile(imagePath);
	Response.End();
 %>