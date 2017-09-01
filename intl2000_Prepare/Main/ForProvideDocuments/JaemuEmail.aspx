<%@ Page Language="C#" %>
<%
	Components.DBConn DB = new Components.DBConn("1");
	DB.SqlCmd.CommandText = "use JaemuImage; SELECT [IsRead] FROM EmailHistory WHERE EmailHistoryPk=" + Request.Params["S"] + ";";
	DB.DBCon.Open();
	string Temp = DB.SqlCmd.ExecuteScalar() + "";
	if (Temp != "1")
	{
		DB.SqlCmd.CommandText = "use JaemuImage; UPDATE EmailHistory SET IsRead=1, ReadDate=getDate() WHERE EmailHistoryPk=" + Request.Params["S"] + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
	}
	else
	{
		DB.DBCon.Close();
	}

	string imagePath = Server.MapPath("./JaemuEmail.JPG");
	Response.Clear();
	Response.ContentType = "Image/Jpeg";
	Response.WriteFile(imagePath);
	Response.End();
 %>