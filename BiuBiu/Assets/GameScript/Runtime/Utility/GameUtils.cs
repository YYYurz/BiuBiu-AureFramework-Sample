// using GameFramework;
// using Hr;
// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using System.Security.Cryptography;
// using System.Text;
// using System.Text.RegularExpressions;
// using UnityEngine;
// using UnityGameFramework.Runtime;
//
// namespace BB
// {
//     public class GameUtils
//     {
//         /// <summary>
//         /// Determines if is mobile platform.
//         /// </summary>
//         public static bool IsMobilePlatform()
//         {
// #if UNITY_IOS || UNITY_ANDROID
//             return true;
// #else
//             return false;
// #endif
//         }
//      
//         /// <summary>
//         /// 获取与目标的角度
//         /// </summary>
//         public static float GetAngleFromTwoPosition(Transform fromTrans, Transform toTrans)
//         {
//             if (fromTrans == null || toTrans == null)
//             {
//                 return 0f;
//             }
//
//             var pos = toTrans.position;
//             var xDistance = pos.x - pos.x;
//             var yDistance = pos.y - pos.y;
//             var angle = (Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg) - 90f;
//             angle = GetNormalizedAngle(angle);
//
//             return angle;
//         }
//
//         /// <summary>
//         /// Get 0 ~ 360 angle.
//         /// </summary>
//         public static float GetNormalizedAngle(float angle)
//         {
//             while (angle < 0f)
//             {
//                 angle += 360f;
//             }
//             while (360f < angle)
//             {
//                 angle -= 360f;
//             }
//             return angle;
//         }
//
//         /// <summary>
//         /// 反序列化json
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         /// <param name="json"></param>
//         /// <returns></returns>
//         public static T DeserializeObject<T>(string json)
//         {
//             if (Application.isPlaying)
//             {
//                 return Utility.Json.ToObject<T>(json);
//             }
//             else
//             {
//                 return UnityEngine.JsonUtility.FromJson<T>(json);
//             }
//         }
//
//         /// <summary>
//         /// 序列化json
//         /// </summary>
//         /// <typeparam name="T"></typeparam>
//         /// <param name="obj"></param>
//         /// <returns></returns>
//         public static string SerializeObject<T>(T obj)
//         {
//             if (Application.isPlaying)
//             {
//                 return Utility.Json.ToJson(obj);
//             }
//             else
//             {
//                 return UnityEngine.JsonUtility.ToJson(obj);
//             }
//             //return JsonConvert.SerializeObject(obj);
//         }
//
//         /// <summary>
//         /// 收集指定目录下指定后缀名的所有文件
//         /// </summary>
//         /// <param name="rootPath"></param>
//         /// <param name="suffix"></param>
//         /// <param name="fileList"></param>
//         public static void CollectFilesWithSuffix(string rootPath, string suffix, ref List<string> fileList)
//         {
//             string[] dirs = Directory.GetDirectories(rootPath);
//             foreach (string path in dirs)
//             {
//                 CollectFilesWithSuffix(path, suffix, ref fileList);
//             }
//
//             string[] files = Directory.GetFiles(rootPath);
//             foreach (string filePath in files)
//             {
//                 if (filePath.Substring(filePath.IndexOf(".")) == suffix)
//                 {
//                     fileList.Add(filePath);
//                 }
//             }
//         }
//
//         //fasthash64
//         public static unsafe ulong FastHash64(string str)
//         {
//             if (str == null) return 0;
//             fixed (void* buffer = str)
//             {
//                 return unchecked((ulong)FastHash64((byte*)buffer, (uint)(str.Length * sizeof(char)), 0UL));
//             }
//         }
//
//         /// <summary>
//         /// fasthash64x
//         /// </summary>
//
//         public static unsafe ulong FastHash64x(string str)
//         {
//             if (str == null) return 0;
//             fixed (void* buffer = str)
//             {
//                 return unchecked((ulong)FastHash64x((byte*)buffer, (uint)(str.Length * sizeof(char)), 0UL));
//             }
//         }
//
//         /// <summary>
//         /// fasthash64x
//         /// </summary>
//         public static unsafe ulong FastHash64x(byte* buffer, uint length, ulong seed = 0)
//         {
//             unchecked
//             {
//                 uint trueLength = length >> (sizeof(char) - 1);
//                 ulong* pos = (ulong*)buffer;
//                 const ulong m = 0x880355f21e6d1965UL;
//                 ulong* end = pos + (trueLength / 8);
//                 byte* pos2;
//                 ulong h = seed ^ (trueLength * m);
//
//                 for (int i = 1; i < trueLength; i++)
//                 {
//                     buffer[i] = buffer[i * sizeof(char)];
//                 }
//
//                 ulong v;
//                 while (pos != end)
//                 {
//                     v = *pos++;
//                     v ^= v >> 23;
//                     v *= 0x2127599bf4325c37UL;
//                     v ^= v >> 47;
//                     h ^= v;
//                     h *= m;
//                 }
//
//                 pos2 = (byte*)pos;
//                 v = 0;
//                 switch (trueLength & 7)
//                 {
//                     case 7:
//                         v ^= (ulong)pos2[6] << 48; goto case 6;
//                     case 6:
//                         v ^= (ulong)pos2[5] << 40; goto case 5;
//                     case 5:
//                         v ^= (ulong)pos2[4] << 32; goto case 4;
//                     case 4:
//                         v ^= (ulong)pos2[3] << 24; goto case 3;
//                     case 3:
//                         v ^= (ulong)pos2[2] << 16; goto case 2;
//                     case 2:
//                         v ^= (ulong)pos2[1] << 8; goto case 1;
//                     case 1:
//                         v ^= (ulong)pos2[0];
//                         v ^= v >> 23;
//                         v *= 0x2127599bf4325c37UL;
//                         v ^= v >> 47;
//                         h ^= v;
//                         h *= m;
//                         break;
//                 }
//
//                 h ^= h >> 23;
//                 h *= 0x2127599bf4325c37UL;
//                 h ^= h >> 47;
//
//                 return h;
//             }
//         }
//
//         //fasthash64
//         public static unsafe long FastHash64(byte* buffer, uint length, ulong seed = 0)
//         {
//             return unchecked((long)FastHash64x((byte*)buffer, length, seed));
//         }
//
//         #region URL IP
//         /// <summary>
//         /// 判断是否本游戏的代理 代理有一定规则 后面再写
//         /// </summary>
//         /// <param name="str"></param>
//         /// <returns></returns>
//         public static bool IsProxyUrl(string str)
//         {
//             return true;
//         }
//
//         /// 判断一个字符串是否为url
//         /// </summary>
//         /// <param name="str"></param>
//         /// <returns></returns>
//         public static bool IsUrl(string str)
//         {
//             try
//             {
//                 string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
//                 return Regex.IsMatch(str, Url);
//             }
//             catch (Exception ex)
//             {
//                 return false;
//             }
//         }
//
//         /// 验证IP
//         /// </summary>
//         /// <param name="ip"></param>
//         /// <returns></returns>
//         public static bool IsCorrectIP4(string ip)
//         {
//             System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^\d{1,3}(\.\d{1,3}){3}$",
//                 System.Text.RegularExpressions.RegexOptions.Singleline);
//             return reg.Match(ip).Success;
//         }
//
//         /// </summary>
//         /// <param name="ip"></param>
//         /// <returns></returns>
//         public static bool IsCorrectIP6(string ip)
//         {
//             System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^[A-F\d]{1,4}(\:[A-F\d]{1,4}){7}$",
//                 System.Text.RegularExpressions.RegexOptions.Singleline);
//             return reg.Match(ip).Success;
//         }
//         #endregion
//
//         #region 字符串和Byte之间的转化 以及编码格式
//         /// <summary>
//         /// 数字和字节之间互转
//         /// </summary>
//         /// <param name="num"></param>
//         /// <returns></returns>
//         public static int IntToBitConverter(int num)
//         {
//             int temp = 0;
//             byte[] bytes = BitConverter.GetBytes(num);//将int32转换为字节数组
//             temp = BitConverter.ToInt32(bytes, 0);//将字节数组内容再转成int32类型
//             return temp;
//         }
//
//         /// <summary>
//         /// 将字符串转为16进制字符，允许中文
//         /// </summary>
//         /// <param name="s"></param>
//         /// <param name="encode"></param>
//         /// <returns></returns>
//         public static string StringToHexString(string s, Encoding encode, string spanString)
//         {
//             byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
//             string result = string.Empty;
//             for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
//             {
//                 result += Convert.ToString(b[i], 16) + spanString;
//             }
//             return result;
//         }
//         /// <summary>
//         /// 将16进制字符串转为字符串
//         /// </summary>
//         /// <param name="hs"></param>
//         /// <param name="encode"></param>
//         /// <returns></returns>
//         public static string HexStringToString(string hs, Encoding encode)
//         {
//             string strTemp = "";
//             byte[] b = new byte[hs.Length / 2];
//             for (int i = 0; i < hs.Length / 2; i++)
//             {
//                 strTemp = hs.Substring(i * 2, 2);
//                 b[i] = Convert.ToByte(strTemp, 16);
//             }
//             //按照指定编码将字节数组变为字符串
//             return encode.GetString(b);
//         }
//         /// <summary>
//         /// byte[]转为16进制字符串
//         /// </summary>
//         /// <param name="bytes"></param>
//         /// <returns></returns>
//         public static string ByteToHexStr(byte[] bytes)
//         {
//             string returnStr = "";
//             if (bytes != null)
//             {
//                 for (int i = 0; i < bytes.Length; i++)
//                 {
//                     returnStr += bytes[i].ToString("X2");
//                 }
//             }
//             return returnStr;
//         }
//         /// <summary>
//         /// 将16进制的字符串转为byte[]
//         /// </summary>
//         /// <param name="hexString"></param>
//         /// <returns></returns>
//         public static byte[] StrToHexByte(string hexString)
//         {
//             hexString = hexString.Replace(" ", "");
//             if ((hexString.Length % 2) != 0)
//                 hexString += " ";
//             byte[] returnBytes = new byte[hexString.Length / 2];
//             for (int i = 0; i < returnBytes.Length; i++)
//                 returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
//             return returnBytes;
//         }
//
//         public static string ByteToASCII(byte[] bytes)
//         {
//             System.Text.ASCIIEncoding ASCII = new System.Text.ASCIIEncoding();
//             string strRet = ASCII.GetString(bytes);
//
//             return strRet;
//         }
//
//         public static string ByteToUtf8(byte[] bytes)
//         {
//             System.Text.UTF8Encoding ASCII = new System.Text.UTF8Encoding();
//             string strRet = ASCII.GetString(bytes);
//
//             return strRet;
//         }
//
//         /// <summary>
//         /// 判断是否存在中文字符
//         /// </summary>
//         /// <param name="input"></param>
//         /// <param name="index"></param>
//         /// <returns></returns>
//         protected bool IsChineseLetter(string input, int index)
//         {
//             int code = 0;
//             int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
//             int chend = Convert.ToInt32("9fff", 16);
//             if (input != "")
//             {
//                 code = Char.ConvertToUtf32(input, index);    //获得字符串input中指定索引index处字符unicode编码
//
//                 if (code >= chfrom && code <= chend)
//                 {
//                     return true;     //当code在中文范围内返回true
//
//                 }
//                 else
//                 {
//                     return false;    //当code不在中文范围内返回false
//                 }
//             }
//             return false;
//         }
//
//         /// <summary>
//         /// 把字符串转为utf-8
//         /// </summary>
//         /// <param name="unicodeString"></param>
//         /// <returns></returns>
//         public static string GetUtf8(string unicodeString)
//         {
//             UTF8Encoding utf8 = new UTF8Encoding();
//             byte[] encodedBytes = utf8.GetBytes(unicodeString);
//             string decodedString = utf8.GetString(encodedBytes);
//             return decodedString;
//         }
//
//         #endregion
//
//         #region 时间工具
//
//         /// <summary>
//         /// 获取当前北京时间时间戳 UTC+8
//         /// </summary>
//         /// <returns></returns>
//         public static long GetUTCAdd8Time()
//         {
//             DateTime utcStartTime = new DateTime(1970, 1, 1, 0, 0, 0);
//             DateTime utc8Time = DateTime.UtcNow + new TimeSpan(8, 0, 0);
//
//             return (long)(utc8Time - utcStartTime).TotalSeconds;
//         }
//         #endregion
//
//         /// <summary>
//         /// 生成MD5
//         /// </summary>
//         /// <param name="source"></param>
//         /// <returns></returns>
//         public static string EncryptWithMD5(string source)
//         {
//             byte[] sor = Encoding.UTF8.GetBytes(source);
//             MD5 md5 = MD5.Create();
//             byte[] result = md5.ComputeHash(sor);
//             StringBuilder strbul = new StringBuilder(40);
//             for (int i = 0; i < result.Length; i++)
//             {
//                 strbul.Append(result[i].ToString("x2"));//16进制 占两个占位符
//
//             }
//             return strbul.ToString();
//         }
//
//         /// <summary>
//         /// 解析URL参数
//         /// </summary>
//         /// <param name="url"></param>
//         /// <param name="nvc"></param>
//         static void ParseUrl(string url, Dictionary<string, string> nvc)
//         {
//             if (url == null)
//                 throw new ArgumentNullException("url");
//
//             if (url == "")
//                 return;
//
//             int questionMarkIndex = url.IndexOf('?');
//
//             if (questionMarkIndex == -1)
//             {
//                 questionMarkIndex = 0;
//             }
//
//             string ps = url.Substring(questionMarkIndex + 1);
//
//             // 开始分析参数对    
//             Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
//             MatchCollection mc = re.Matches(ps);
//
//             foreach (Match m in mc)
//             {
//                 nvc.Add(m.Result("$2"), m.Result("$3"));
//             }
//         }
//     }
// }
//
