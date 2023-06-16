using Library.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class LoginController : BaseController
    {
        #region 登入頁面
        [IgnoreSessionTimeout]
        public ActionResult Index()
        {
            //若是因為Timeout回首頁的,要Alert訊息
            if (TempData["SessionTimeout"] != null && (string)TempData["SessionTimeout"] == "NoSession")
                ViewBag.SessionTimeout = "NoSession";

            Session.Clear();

            User_VM Model = new User_VM();
            return View(Model);
        }
        #endregion

        #region 註冊頁面
        [IgnoreSessionTimeout]
        public ActionResult Register()
        {
            User_VM Model = new User_VM();
            return View(Model);
        }
        #endregion

        #region 執行登入/註冊
        [IgnoreSessionTimeout]
        [HttpPost]
        public JsonResult Action(User_VM Model)
        {
            string error = "";
            try
            {
                if (Model.Action == "Login")
                {
                    if (String.IsNullOrEmpty(Model.PhoneNumber?.Trim()) || 
                        String.IsNullOrEmpty(Model.Password?.Trim()))
                    {
                        error = "請輸入帳號及密碼";
                    }
                    else
                    {
                        IEnumerable<User_VM> User = db.Database.SqlQuery<User_VM>("EXEC GetUser @PhoneNumber, @Password",
                                                    new SqlParameter("@PhoneNumber", Model.PhoneNumber),
                                                    new SqlParameter("@Password", Encrypt(Model.Password))).ToList();

                        if (User.Any())
                        {
                            int UserId = User.FirstOrDefault()?.UserId ?? 0;
                            Session["UserId"] = UserId;
                            Session["UserName"] = User.FirstOrDefault()?.UserName;
                            db.Database.ExecuteSqlCommand("EXEC UpdateUserLastLoginTime @UserId ",
                                                           new SqlParameter("@UserId", UserId));
                            return Json(new { url = Url.Action("Index", "Library"), error });
                        }
                        else
                        {
                            error = "帳號或密碼錯誤";
                        }
                    }
                    
                }
                else //註冊
                {
                    IEnumerable<User_VM> User = db.Database.SqlQuery<User_VM>("EXEC GetUser @PhoneNumber, @Password",
                                                new SqlParameter("@PhoneNumber", Model.PhoneNumber),
                                                new SqlParameter("@Password", DBNull.Value)).ToList();
                    if (User.Count() > 0)
                    {
                        error = "此手機號碼已註冊過";
                    }
                    else if (Model.Password != Model.PasswordConfirm)
                    {
                        error = "兩次密碼輸入不同";
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("EXEC InsertUser @PhoneNumber, @Password, @UserName",
                                                       new SqlParameter("@PhoneNumber", Model.PhoneNumber),
                                                       new SqlParameter("@Password", Encrypt(Model.Password)),
                                                       new SqlParameter("@UserName", Model.UserName));
                        return Json(new { url = Url.Action("Index", "Login"), error });
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message.ToString();
            }

            return Json(new { url = "", error});
        }
        #endregion



        #region 密碼加密
        public static string Encrypt(string input)
        {
            string data = input;
            //將加密內文進行UTF8編碼為byte[]
            byte[] utfdata = UTF8Encoding.UTF8.GetBytes(data);

            //Key
            byte[] saltBytes = UTF8Encoding.UTF8.GetBytes("123456789ABCDEFG");

            //使用AES演算法
            AesManaged aes = new AesManaged();

            //RFC 2898 包含從密碼和 Salt 建立金鑰和初始化向量 (IV) 的方法。您可以使用 PBKDF2 (密碼式的金鑰衍生函式) 來衍生金鑰，其方法為使用虛擬亂數函式，以產生幾乎無限長度的金鑰
            //Rfc2898DeriveBytes 類別可以用來從基底金鑰與其他參數中產生衍生金鑰。在密碼式的金鑰衍生函式中，基底金鑰是密碼，而其他參數則是 Salt 值和反覆計數。
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes("123456", saltBytes);

            // Setting our parameters
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;

            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);

            // 產生加密器
            ICryptoTransform encryptTransf = aes.CreateEncryptor();

            // 輸出至MemoryStrem中進行加密
            MemoryStream encryptStream = new MemoryStream();
            CryptoStream encryptor = new CryptoStream(encryptStream, encryptTransf, CryptoStreamMode.Write);

            encryptor.Write(utfdata, 0, utfdata.Length);
            encryptor.Flush();
            encryptor.Close();

            // 加密完成後的byte[]
            byte[] encryptBytes = encryptStream.ToArray();

            //轉成Base64後輸出
            string encryptedString = Convert.ToBase64String(encryptBytes);

            return encryptedString;
        }
        #endregion

        #region 密碼解密
        public static string Decrypt(string base64Input)
        {
            byte[] encryptBytes = Convert.FromBase64String(base64Input);
            byte[] saltBytes = Encoding.UTF8.GetBytes("123456789ABCDEFG");

            AesManaged aes = new AesManaged();

            //RFC 2898 包含從密碼和 Salt 建立金鑰和初始化向量 (IV) 的方法。您可以使用 PBKDF2 (密碼式的金鑰衍生函式) 來衍生金鑰，其方法為使用虛擬亂數函式，以產生幾乎無限長度的金鑰
            //Rfc2898DeriveBytes 類別可以用來從基底金鑰與其他參數中產生衍生金鑰。在密碼式的金鑰衍生函式中，基底金鑰是密碼，而其他參數則是 Salt 值和反覆計數。
            //這也像是以前使用.net framework 中的Key and IV 的概念
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes("123456", saltBytes);

            // Setting our parameters
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;

            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);

            //改成解密
            ICryptoTransform decryptTrans = aes.CreateDecryptor();
            MemoryStream decryptStream = new MemoryStream();
            CryptoStream decryptor = new CryptoStream(decryptStream, decryptTrans, CryptoStreamMode.Write);

            decryptor.Write(encryptBytes, 0, encryptBytes.Length);
            decryptor.Flush();
            decryptor.Close();

            //解密後的byte[]
            byte[] decryptBytes = decryptStream.ToArray();
            string decryptedString = UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);

            return decryptedString;
        }
        #endregion
    }
}