<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" Debug="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<link href="Common/IntlWeb.css" rel="stylesheet" type="text/css" />
	<title>NN21.net</title>
	<style type="text/css">
		.textstyle {
			border-color: #cbd5dd;
			border-radius: 2px;
			display: block;
			width: 100%;
			padding: 3px 3px;
			border: 1px solid #ccc;
		}
		.btnstyle {
			font-weight: 500;
			border-radius: 2px;
			color: #fff !important;
			background-color: #177bbb;
			border-color: #177bbb;
			display: inline-block;
			white-space: nowrap;
		}
	</style>
	<script type="text/javascript">
		function LanguageSet(Lang) {
			form1.Tb_LanguageSetInfo.value = Lang;
			__doPostBack('Btn_ChangeLanguage', '');
		}
	</script>
</head>
<body style="background-color: #ffffff;">
	<form id="form1" runat="server">
		<div style="margin: 0 auto; background: url(./Images/nn21_system_login.jpg); background-repeat: no-repeat; background-size:100% auto; width:1200px;" >
			<input type="hidden" id="Tb_LanguageSetInfo" name="Tb_LanguageSetInfo" />
			<div style="width: 500px; height: 280px; padding-top: 460px; padding-left: 450px; padding-right: 50px; margin: 0 auto;">
				<table border="0" cellpadding="0" cellspacing="0">
					<tr>
						<td colspan="3">
							<p style="text-align: right; padding-right: 10px;">
								<span style="cursor: pointer;" onclick="LanguageSet('en')">English</span>&nbsp;&nbsp;||&nbsp;&nbsp;
								<span style="cursor: pointer;" onclick="LanguageSet('ko')">한글</span>&nbsp;&nbsp;||&nbsp;&nbsp;
								<span style="cursor: pointer;" onclick="LanguageSet('zh')">中文</span>
							</p>
						</td>
					</tr>
					<tr>
						<td style="width: 80px; text-align: right; padding: 5px;"><%=GetGlobalResourceObject("Member", "ID") %> : </td>
						<td style="width: 150px;">
							<asp:TextBox CssClass="textstyle" ID="TB_ID" runat="server" TabIndex="1"></asp:TextBox>
						</td>
						<td rowspan="2" style="width: 110px; text-align: right; padding-right: 10px;">
							<asp:Button CssClass="btnstyle" ID="Btn_Login" runat="server" OnClick="Btn_Login_Click" Height="30px" TabIndex="3" />
							<br />
							<asp:CheckBox ID="CheckBox1" runat="server" /><label for="CheckBox1">Save Account</label>
						</td>
					</tr>
					<tr>
						<td style="text-align: right; padding-right: 5px;"><%=GetGlobalResourceObject("Member", "Password") %> : </td>
						<td><asp:TextBox CssClass="textstyle" ID="TB_PWD" runat="server" TextMode="Password" TabIndex="2"></asp:TextBox></td>
					</tr>
					<tr>
						<td colspan="3">
							<p style="text-align: right; padding-right: 10px;">
								<asp:Button ID="Btn_MemberJoin" CssClass="btnstyle" Style="width: 64px; height: 30px;" runat="server" CausesValidation="False" OnClick="Btn_MemberJoin_Click" TabIndex="99" UseSubmitBehavior="False" Width="70" Height="25" />
							</p>
						</td>
					</tr>
				</table>
				<span style="visibility: hidden;">
					<asp:Button runat="server" ID="Btn_ChangeLanguage" OnClick="Btn_ChangeLanguage_Click" />
				</span>
			</div>
		</div>
		<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TB_ID" Display="None" SetFocusOnError="True" meta:resourcekey="RequiredFieldValidator1Resource1"></asp:RequiredFieldValidator>
		<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TB_PWD" Display="None" SetFocusOnError="True" meta:resourcekey="RequiredFieldValidator2Resource1"></asp:RequiredFieldValidator>
		<asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="True" ShowSummary="False" meta:resourcekey="ValidationSummary1Resource1" />
	</form>
</body>
</html>