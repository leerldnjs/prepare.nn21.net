<%@ Page Language="C#" %>
<%
	Components.DBConn DB = new Components.DBConn();
	DB.SqlCmd.CommandText = "SELECT [ReceiveTime] FROM MsgSendedHistory WHERE [MsgSendHistoryPk]=" + Request.Params["S"] + ";";
	DB.DBCon.Open();
	string Temp = DB.SqlCmd.ExecuteScalar() + "";
	if (Temp == "")
	{
		DB.SqlCmd.CommandText = "UPDATE MsgSendedHistory SET ReceiveTime=getDate() WHERE [MsgSendHistoryPk]=" + Request.Params["S"] + ";";
		DB.SqlCmd.ExecuteNonQuery();
		DB.DBCon.Close();
	}
	else
	{
		DB.DBCon.Close();
	}

	string imagePath = Server.MapPath("./IL_Logo.jpg");
	Response.Clear();
	Response.ContentType = "Image/Jpeg";
	Response.WriteFile(imagePath);
	Response.End();
 %>