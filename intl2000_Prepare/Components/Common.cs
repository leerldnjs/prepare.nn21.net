using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Components
{
	public class Common
	{
		private int TempInt32;
		public static String SelectOurBranch = @"<option value='3157'>KRIC 인천</option>																			
																		<option value='3095'>JPOK Osaka</option>
																		<option value='3843'>CNGZ 广州</option>
																		<option value='2886'>CNYT 烟台</option>
																		<option value='2887'>CNSY 瀋陽</option>
																		<option value='2888'>CNYW 义乌</option>
																		<option value='3388'>CNQD 青岛</option>
																		<option value='11456'>CNHZ 杭州</option>
																		<option value='7898'>CNSX 绍兴</option>   
																		<option value='12437'>VTHM Hochimin</option>
																		<option value='12438'>VTHN Hanoi</option>
																		<option value='12527'>VTDN Danang</option>
																		<option value='12464'>MMYG</option>
																		<option value='3798'>OtherLocation</option>";
		public static String SelectOurBranch_HAHAHA = @"<option value='3157'>KRIC 인천</option>																			
																		<option value='3095'>JPOK Osaka</option>
																		<option value='3843'>CNGZ 广州</option>
																		<option value='2886'>CNYT 烟台</option>
																		<option value='2887'>CNSY 瀋陽</option>
																		<option value='2888'>CNYW 义乌</option>
																		<option value='3388'>CNQD 青岛</option>
																		<option value='7898'>CNSX 绍兴</option>
																		<option value='11456'>CNHZ 杭州</option>
																		<option value='12437'>VTHM Hochimin</option>
																		<option value='12438'>VTHN Hanoi</option>
																		<option value='12527'>VTDN Danang</option>
																		<option value='12464'>MMYG</option>
                                                                        <option value='10520'>HAHAHA</option>
																		<option value='3798'>OtherLocation</option>";


		public String SetPageListByNo(int EachPageLimit, int NowPage, int TotalRecord, string URL, string Type) {
			int MaxPage;
			if (TotalRecord % EachPageLimit == 0) { MaxPage = TotalRecord / EachPageLimit; } else { MaxPage = TotalRecord / EachPageLimit + 1; }
			StringBuilder Query = new StringBuilder();
			if (MaxPage == 1) { Query.Append(" "); } else {
				if (MaxPage + 1 > 10) {
					int Group = (NowPage - 1) / 10;
					if (NowPage > 10) { Query.Append("<a href=" + URL + Type + "pageNo=" + (Group * 10 - 1) + "><<</a>"); }
					TempInt32 = NowPage > 10 ? -4 : 1;
					for (int i = Group * 10 + TempInt32; i < MaxPage + 1; i++) {
						if (i == NowPage) { Query.Append(" <span style='font-size:17px;font-weight:bold; padding-left:5px; padding-right:5px;  '>" + i + "</span> "); } else {
							Query.Append(" <a href=" + URL + Type + "pageNo=" + i + "><span style='font-size:15px; padding-left:5px; padding-right:5px; '>" + i + "</span></a> ");
						}
						if (i == Group * 10 + 15) {
							Query.Append("<a href=" + URL + Type + "pageNo=" + (Group * 10 + 11) + ">>></a>");
							break;
						}

					}
				} else {
					for (int i = 1; i < MaxPage + 1; i++) {
						if (i == NowPage) { Query.Append(" <span style='font-size:17px;font-weight:bold; padding-left:5px; padding-right:5px;  '>" + i + "</span> "); } else {
							Query.Append(" <a href=" + URL + Type + "pageNo=" + i + "><span style='font-size:15px; padding-left:5px; padding-right:5px; '>" + i + "</span></a> ");
						}
					}
				}
			}
			return Query + "";
		}
		public static readonly Char[] Splite11 = new Char[] { '!' };
		public static readonly String[] Splite22 = new String[] { "@@" };
		public static readonly String[] Splite34 = new String[] { "####" };
		public static readonly String[] Splite321 = new String[] { "#@!" };
		public static readonly String[] Splite51423 = new String[] { "%!$@#" };
		public static String[] SetPhoneNumberSplit(string Phone) {
			string[] TempStringArray = new string[3] { "", "", "" };
			if (Phone != "" && Phone.IndexOf('-') != -1) {
				TempStringArray[0] = Phone.Substring(0, Phone.IndexOf('-'));
				TempStringArray[1] = Phone.Substring(Phone.IndexOf('-') + 1, Phone.LastIndexOf('-') - Phone.IndexOf('-') - 1);
				TempStringArray[2] = Phone.Substring(Phone.LastIndexOf('-') + 1, Phone.Length - Phone.LastIndexOf('-') - 1);
			}
			return TempStringArray;
		}
		public static String NumberFormat(string Number) {
			try {
				return Decimal.Parse(Number).ToString("###,###,###,###,###,##0.####");
			} catch (Exception) {
				return "";
			}
		}
		public String CheckNull(string Value, bool IsString, bool isUnicode) {
			if (Value == "") { return "NULL"; }
			if (IsString & isUnicode) { return "N'" + Value + "'"; }
			if (IsString) { return "'" + Value + "'"; }
			return Value;
		}
		public static String StringToDB(string Value, bool isString, bool isUnicode)
		{
			if (Value == "" || Value == null) { return "NULL"; } else if (isString & isUnicode) { return "N'" + Value.Replace("'", "''") + "'"; } else if (isString) { return "'" + Value.Replace("'", "''") + "'"; } else { return Value.Replace(",", string.Empty); }
		}
		public static String GetPackedCbm(string ContainerSize) {
			switch (ContainerSize) {
				default:
					return "67";
				case "20GP":
					return "31";
				case "40GP":
					return "67";
				case "40HC":
					return "76";
				case "45GP":
					return "85";
			}
		}
		public static String GetBranchName(string BranchPk) {
			string ReturnValue = "";
			switch (BranchPk) {
				case "3157":
					ReturnValue = "KRIC";
					break;
				case "3095":
					ReturnValue = "JPOK";
					break;
				case "3843":
					ReturnValue = "CNGZ";
					break;
				case "2886":
					ReturnValue = "CNYT";
					break;
				case "2887":
					ReturnValue = "CNSY";
					break;
				case "2888":
					ReturnValue = "CNYW";
					break;
				case "3388":
					ReturnValue = "CNQD";
					break;
				case "11456":
					ReturnValue = "CNHZ";
					break;
				case "7898":
					ReturnValue = "CNSX";
					break;
				case "12437":
					ReturnValue = "VTHM";
					break;
				case "12438":
					ReturnValue = "VTHN";
					break;
				case "12527":
					ReturnValue = "VTDN";
					break;
				case "12464":
					ReturnValue = "MMYG";
					break;
				
			}
							
			return ReturnValue;
		}

		public static String GetMonetaryUnit(string MonetaryUnitCL) {
			switch (MonetaryUnitCL) {
				case "18":
					return "￥";//RMB
				case "19":
					return "$"; //USD
				case "20":
					return "￦";//KRW
				case "21":
					return "Y";//JPY
				case "22":
					return "HK$";//HKD
				case "23":
					return "€";//EUR
				default:
					return "";
			}
		}
		public static String GetPackingUnit(string PackingUnitCL) {
			switch (PackingUnitCL) {
				case "15":
					return "CT";
				case "16":
					return "RO";
				case "17":
					return "PA";
				default:
					return "GT";
			}
		}
		public static string GetTransportBCHistoryGubunCL(string GubunCL) {
			switch (GubunCL) {
				case "0":
					return "최초입력";
				case "1":
					return "정보입력";
				case "2":
					return "IL 문자발송";
				case "3":
					return "운송사 문자발송";
				case "10":
					return "출고완료";
				default:
					return "Not";
			}
		}
		public static String GetTransportWay(string TransportWayCL) {
			switch (TransportWayCL) {
				case "27":
					return "LCL";
				case "28":
					return "항공특송";
				case "29":
					return "항공운송";
				case "30":
					return "3 Days";
				case "31":
					return "FCL";
				case "32":
					return "4 Days";
				case "33":
					return "5 Days";
				case "35":
					return "7 Days";
				case "36":
					return "2 Days";
				case "37":
					return "6 Days";
				case "38":
					return "복합운송 항공";
				case "39":
					return "복합운송 차량";
				case "40":
					return "내륙 항공운송";
				case "41":
					return "내륙 해상운송";
				case "42":
					return "내륙 항공운송";
				case "43":
					return "내륙 육로운송";
				case "44":
					return "AIR HAND CARRY";
				case "45":
					return "SEA HAND CARRY";
				case "46":
					return "EMS";
				default:
					return "Not Fixed";
			}
		}
		public static String GetStepCL(string StepCL) {
			switch (StepCL) {
				case "50":
					return "<span style='color:green;'>접수예약 </span>";
				case "51":
					return "<span style='color:green;'>접수예약 완료</span>";
				case "52":
					return "<span style='color:green;'>접수보류</span>";
				case "53":
					return "<span style='color:green;'>픽업예약</span>";
				case "54":
					return "<span style='color:green;'>화물 접수 완료</span>";
				case "55":
					return "<span style='color:green;'>화물 계량 완료</span>";
				case "56":
					return "<span style='color:green;'>일정 확정</span>";
				case "57":
					return "<span style='color:green;'>화물접수 완료</span>";
				case "58":
					return "<span style='color:green;'>운송진행중 <br /> 운임확정 완료</span>";
				case "62":
					return "<span style='color:green;'>운송진행중</span>";
				case "64":
					return "<span style='color:black;'>배송완료</span>";
				case "65":
					return "<span style='color:black;'>출고완료</span>";
				default:
					return "??";
			}
		}
		public static String GetQuantityUnit(string QuantityUnitCL) {
			switch (QuantityUnitCL) {
				case "40":
					return "PCS";
				case "41":
					return "PRS";
				case "42":
					return "SET";
				case "43":
					return "S/F";
				case "44":
					return "YDS";
				case "45":
					return "M";
				case "46":
					return "KG";
				case "47":
					return "DZ";
				case "48":
					return "L";
				case "49":
					return "BOX";
				case "50":
					return "SQM";
				case "51":
					return "M2";
				case "52":
					return "RO";
				default:
					return "";
			}
		}
		public static string DateFormat(string Format, string Date, string Date2 = "") {
			string ReturnValue = "";
			switch (Format) {
				case "MD~D":
					if (Date.Length > 10) {
						ReturnValue = Date.Substring(5, 5);
					} else {
						ReturnValue = Date;
					}
					if (Date2 != "") {
						ReturnValue += " ~ ";
					}
					if (Date2.Length > 10) {
						ReturnValue += Date2.Substring(5, 5);
					} else {
						ReturnValue += Date2;
					}
					break;
				case "MD":
					if (Date.Length > 10) {
						ReturnValue = Date.Substring(5, 5);
					} else {
						ReturnValue = Date;
					}
					break;
			}
			return ReturnValue;
		}
		public static String GetBetweenBranchTransportWay(string TransportWayCL) {
			switch (TransportWayCL) {
				case "1":
					return "<span style=\"background-color:#FFA07A; \">AIR</span>";
				case "2":
					return "<span style=\"background-color:#98FB98; \">CAR</span>";
				case "3":
					return "<span style=\"background-color:#c0d4eE; \">SHIP</span>";
				case "4":
					return "<span style=\"background-color:gray; \">Hand carry</span>";
				case "5":
					return "<span style=\"background-color:#E0F4FE; \">FCL</span>";
				case "6":
					return "<span style=\"background-color:#F0G4FE; \">LCL</span>";
				case "99":
					return "<span style=\"background-color:#c0d4eE; \">SHIP Direct</span>";
				default:
					return "";
			}
		}
		public static String GetBetweenBranchTransportStepCL(string StepCL) {
			switch (StepCL) {
				case "1":
					return "<span style=\"color:blue;\" >예약</span>";
				case "2":
					return "<span style=\"color:red;\" >진행중</span>";
				default:
					return "<span style=\"color:green;\" >완료</span>";
			}
		}
		public static String DBToHTML(object value) {
			return value + "" == "" ? "&nbsp;" : value + "";
		}
		public String SetPageListByNoBranch(int EachPageLimit, int NowPage, int TotalRecord, string URL) {
			int MaxPage;
			if (TotalRecord % EachPageLimit == 0) {
				MaxPage = TotalRecord / EachPageLimit;
			} else {
				MaxPage = TotalRecord / EachPageLimit + 1;
			}
			StringBuilder Query = new StringBuilder();
			if (MaxPage == 1) {
				Query.Append(" ");
			} else {
				if (MaxPage + 1 > 10) {
					int Group = (NowPage - 1) / 10;
					if (NowPage > 10) {
						Query.Append("<a href=" + URL + "pageNo=" + (Group * 10 - 1) + "><<</a>");
					}
					TempInt32 = NowPage > 10 ? -4 : 1;
					for (int i = Group * 10 + TempInt32; i < MaxPage + 1; i++) {
						if (i == NowPage) {
							Query.Append(" <span style='font-size:17px;font-weight:bold; padding-left:5px; padding-right:5px;  '>" + i + "</span> ");
						} else {
							Query.Append(" <a href=" + URL + "pageNo=" + i + "><span style='font-size:15px; padding-left:5px; padding-right:5px; '>" + i + "</span></a> ");
						}
						if (i == Group * 10 + 15) {
							Query.Append("<a href=" + URL + "pageNo=" + (Group * 10 + 11) + ">>></a>");
							break;
						}

					}
				} else {
					for (int i = 1; i < MaxPage + 1; i++) {
						if (i == NowPage) {
							Query.Append(" <span style='font-size:17px;font-weight:bold; padding-left:5px; padding-right:5px;  '>" + i + "</span> ");
						} else {
							Query.Append(" <a href=" + URL + "pageNo=" + i + "><span style='font-size:15px; padding-left:5px; padding-right:5px; '>" + i + "</span></a> ");
						}
					}
				}
			}
			return Query + "";
		}
		public String LoadHtml_Paging_Post(int EachPageLimit, int NowPage, int TotalRecord, string URL) {
			if (URL.IndexOf("?") > 0) {
				if (URL.Substring(URL.Length - 1) != "?" && URL.Substring(URL.Length - 1) != "&") {
					URL += "&";
				}
			} else {
				URL += "?";
			}

			int MaxPage;
			int TempInt32;
			if (TotalRecord % EachPageLimit == 0) { MaxPage = TotalRecord / EachPageLimit; } else { MaxPage = TotalRecord / EachPageLimit + 1; }
			StringBuilder Query = new StringBuilder();
			if (MaxPage == 1) {
				Query.Append(" ");
			} else {
				if (MaxPage + 1 > 10) {
					int Group = (NowPage - 1) / 10;
					if (NowPage > 10) {
						Query.Append("<li style='cursor:pointer;'><a href=" + URL + "PageNo=" + (Group * 10 - 1) + "><i class=\"fa fa-chevron-left\"></i></a></li>");
					}
					TempInt32 = NowPage > 10 ? -4 : 1;
					for (int i = Group * 10 + TempInt32; i < MaxPage + 1; i++) {
						if (i == NowPage) {
							Query.Append("<li style='cursor:pointer;' class=\"active\"><a href=\"#\">" + i + "</a></li>");
						} else {
							Query.Append("<li style='cursor:pointer;'><a onclick=\"GoPage(" + i + ")\" >" + i + "</a></li>");
						}
						if (i == Group * 10 + 15) {
							Query.Append("<li style='cursor:pointer;'><a onclick=\"GoPage(" + (Group * 10 + 11) + ")\" ><i class=\"fa fa-chevron-right\"></i></a></li>");
							break;
						}
					}
				} else {
					for (int i = 1; i < MaxPage + 1; i++) {
						if (i == NowPage) {
							Query.Append("<li style='cursor:pointer;' class=\"active\"><a href=\"#\">" + i + "</a></li>");
						} else {
							Query.Append("<li style='cursor:pointer;'><a onclick=\"GoPage(" + i + ")\" >" + i + "</a></li>");
						}
					}
				}
			}
			return "<ul class=\"pagination pagination-md\">" + Query + "</ul>";
		}
		public String LoadHtml_Paging(int EachPageLimit, int NowPage, int TotalRecord, string URL) {
			if (URL.IndexOf("?") > 0) {
				if (URL.Substring(URL.Length - 1) != "?" && URL.Substring(URL.Length - 1) != "&") {
					URL += "&";
				}
			} else {
				URL += "?";
			}

			int MaxPage;
			int TempInt32;
			if (TotalRecord % EachPageLimit == 0) { MaxPage = TotalRecord / EachPageLimit; } else { MaxPage = TotalRecord / EachPageLimit + 1; }
			StringBuilder Query = new StringBuilder();
			if (MaxPage == 1) {
				Query.Append(" ");
			} else {
				if (MaxPage + 1 > 10) {
					int Group = (NowPage - 1) / 10;
					if (NowPage > 10) {
						Query.Append("<li><a href=" + URL + "PageNo=" + (Group * 10 - 1) + "><i class=\"fa fa-chevron-left\"></i></a></li>");
					}
					TempInt32 = NowPage > 10 ? -4 : 1;
					for (int i = Group * 10 + TempInt32; i < MaxPage + 1; i++) {
						if (i == NowPage) {
							Query.Append("<li class=\"active\"><a href=\"#\">" + i + "</a></li>");
						} else {
							Query.Append("<li><a href=" + URL + "PageNo=" + i + ">" + i + "</a></li>");
						}
						if (i == Group * 10 + 15) {
							Query.Append("<li><a href=" + URL + "PageNo=" + (Group * 10 + 11) + "><i class=\"fa fa-chevron-right\"></i></a></li>");
							break;
						}
					}
				} else {
					for (int i = 1; i < MaxPage + 1; i++) {
						if (i == NowPage) {
							Query.Append("<li class=\"active\"><a href=\"#\">" + i + "</a></li>");
						} else {
							Query.Append("<li><a href=" + URL + "PageNo=" + i + ">" + i + "</a></li>");
						}
					}
				}
			}
			return "<ul class=\"pagination pagination-md\">" + Query + "</ul>";
		}
		//AES_256 암호화
		public String AESEncrypt256(String InputText) {
			//절대 바꾸면 안됌
			string AESKey = "intl2014";
			//절대 바꾸면 안됌

			RijndaelManaged RijndaelCipher = new RijndaelManaged();

			// 입력받은 문자열을 바이트 배열로 변환  
			byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);

			// 딕셔너리 공격을 대비해서 키를 더 풀기 어렵게 만들기 위해서   
			// Salt를 사용한다.  
			byte[] Salt = Encoding.ASCII.GetBytes(AESKey.Length.ToString());

			PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(AESKey, Salt);

			// Create a encryptor from the existing SecretKey bytes.  
			// encryptor 객체를 SecretKey로부터 만든다.  
			// Secret Key에는 32바이트  
			// Initialization Vector로 16바이트를 사용  
			ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

			MemoryStream memoryStream = new MemoryStream();

			// CryptoStream객체를 암호화된 데이터를 쓰기 위한 용도로 선언  
			CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

			cryptoStream.Write(PlainText, 0, PlainText.Length);

			cryptoStream.FlushFinalBlock();

			byte[] CipherBytes = memoryStream.ToArray();

			memoryStream.Close();
			cryptoStream.Close();

			string EncryptedData = Convert.ToBase64String(CipherBytes);

			return EncryptedData;
		}

		//AES_256 복호화  
		public String AESDecrypt256(String InputText) {
			//절대 바꾸면 안됌
			string AESKey = "intl2014";
			//절대 바꾸면 안됌

			RijndaelManaged RijndaelCipher = new RijndaelManaged();

			byte[] EncryptedData = Convert.FromBase64String(InputText);
			byte[] Salt = Encoding.ASCII.GetBytes(AESKey.Length.ToString());

			PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(AESKey, Salt);

			// Decryptor 객체를 만든다.  
			ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

			MemoryStream memoryStream = new MemoryStream(EncryptedData);

			// 데이터 읽기 용도의 cryptoStream객체  
			CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

			// 복호화된 데이터를 담을 바이트 배열을 선언한다.  
			byte[] PlainText = new byte[EncryptedData.Length];



			int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

			memoryStream.Close();
			cryptoStream.Close();

			string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

			return DecryptedData;
		}
	}
}