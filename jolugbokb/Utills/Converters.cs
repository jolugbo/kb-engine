using jolugbokb.Interface;
using jolugbokb.Models;
using Jose;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace jolugbokb.Utills
{
    public class Converters : IConverters
    {
        public Converters()
        {
            //_logger = logger;
            //_dbContext = dbContext;
        }
        public static IConfiguration Configuration { get; set; }

        //private readonly ILogger<Converters> _logger;

        public byte[] DecodeKeys(string MKeys)
        {
            byte[] a = new byte[32];
            try
            {
                string token = Jose.JWT.Decode(MKeys, null, JwsAlgorithm.none);
                var c = token.Split('#').ToList();
                IList<byte> byteArray = new List<byte>();
                foreach (var item in c)
                {
                    byte oneByte = Convert.ToByte(Convert.ToInt16(item));
                    byteArray.Add(oneByte);
                }
                byte[] secKey = byteArray.ToArray();
                return secKey;
            }
            catch (Exception Ex)
            {
                //_logger.LogError("method DecodeKeys: {0} " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return a;
        }

        public string UnPoisonInbase64(string a)
        {
            string b = null;
            try
            {
                string yt = a.Substring(0, 5);
                string ytt = a.Substring(15, a.Count() - 25);
                string y = yt + ytt;
                b = Base64Decode(y);
            }
            catch (Exception Ex)
            {

                //throw;
            }
            return b;
        }

        public virtual string PoisonInBase64(string a)
        {
            string b = null;
            try
            {
                string y = Base64Encode(a);
                string yt = y.Substring(0, 5);
                string ytt = y.Substring(5, y.Count() - 5);
                string aa = GetPoison();
                string bb = GetPoison();
                if (!string.IsNullOrEmpty(aa) && !string.IsNullOrEmpty(bb))
                {
                    b = yt + aa + ytt + bb;
                }
            }
            catch (Exception Ex)
            {
                //_logger.LogError("Method PoisonBase64 " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                // throw;
            }
            return b;
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public virtual string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public virtual string DecodeEncrypt(string b)
        {
            string a = null;
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                string timeZone = Configuration["TimeZone"].ToString();
                string MajorKeys = Configuration["MajorKeys"].ToString();
                string dfs = UnPoisonInbase64(b);
                a = Jose.JWT.Decode(dfs, DecodeKeys(MajorKeys), JwsAlgorithm.HS256);
            }
            catch (Exception Ex)
            {
                //_logger.LogError("Method DecodeEncrypt " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return a;
        }

        public virtual string GetPoison()
        {
            string a = null;
            try
            {
                int maxSize = 10;
                char[] chars = new char[62];
                chars =
                "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
                byte[] data = new byte[1];
                using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
                {
                    crypto.GetNonZeroBytes(data);
                    data = new byte[maxSize];
                    crypto.GetNonZeroBytes(data);
                }
                StringBuilder result = new StringBuilder(maxSize);
                foreach (byte ba in data)
                {
                    result.Append(chars[ba % (chars.Length)]);
                }
                a = (result.ToString().Count() == 10) ? result.ToString() : null;
            }
            catch (Exception Ex)
            {
                //_logger.LogError("Method GetPoison " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                // throw;
            }
            return a;
        }

        public virtual string SHA512Encryption(string a)
        {
            string b = null;
            try
            {
                var data = Encoding.UTF8.GetBytes(a);
                SHA512 shaM = new SHA512Managed();
                byte[] result = shaM.ComputeHash(data);
                StringBuilder hash = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    hash.Append(result[i].ToString("x2"));
                }
                b = hash.ToString();
            }
            catch (Exception Ex)
            {
                //_logger.LogError("Method SHA512Encryption " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return b;
        }

        public virtual string EncryptDecode(string b)
        {
            string a = null;
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                string timeZone = Configuration["TimeZone"].ToString();
                string MajorKeys = Configuration["MajorKeys"].ToString();
                string dfs = Jose.JWT.Encode(b, DecodeKeys(MajorKeys), JwsAlgorithm.HS256);
                a = PoisonInBase64(dfs);
            }
            catch (Exception Ex)
            {
                //_logger.LogError("Method EncryptDecode " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return a;
        }

        public virtual string GenToken()
        {
            string a = null;
            try
            {
                int maxSize = 6;
                char[] chars = new char[62];
                chars =
                "1234567890".ToCharArray();
                byte[] data = new byte[1];
                using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
                {
                    crypto.GetNonZeroBytes(data);
                    data = new byte[maxSize];
                    crypto.GetNonZeroBytes(data);
                }
                StringBuilder result = new StringBuilder(maxSize);
                foreach (byte ba in data)
                {
                    result.Append(chars[ba % (chars.Length)]);
                }
                a = result.ToString();

            }
            catch (Exception Ex)
            {
                //_logger.LogWarning("Error Ocurred while generating token: ");
                //_logger.LogError("Error on method GenToken" + Ex.InnerException + Ex.Message + Ex.StackTrace);
                // throw;
            }
            return a;
        }


    }
}