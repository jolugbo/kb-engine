using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using jolugbokb.Data;
using jolugbokb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace jolugbokb.Utills
{
    public class OTPUtills
    {
        public static IConfiguration Configuration { get; set; }
        private readonly KBDBContext _dbContext;
        public string GenOTP(int maxSize)
        {
            string a = null;
            try
            {
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
                //throw;
            }
            return a;
        }

        public bool ValidateOTP(OTP a, string OTP)
        {
            bool b = false;
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                string timeZone = Configuration["TimeZone"].ToString();
                var abcv = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone));
                string TokenExpire = Configuration["TokenExpire"].ToString();
                TimeSpan tpin = abcv - a.RecCreDate;
                var tpinn = tpin.TotalSeconds;
                if (a.Otp.Trim() == OTP.Trim() && tpinn <= Convert.ToInt64(TokenExpire))
                {
                    b = true;
                }
            }
            catch (Exception Ex)
            {
                //throw;
            }
            return b;
        }

        public virtual async Task<SessionKey> GetKey(string UserID)
        {
            SessionKey a = new SessionKey();
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                string timeZone = Configuration["TimeZone"].ToString();
                var abcv = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone));
                a = await _dbContext.SessionKey.FirstOrDefaultAsync(z => z.Userid.Trim() == UserID.Trim() && z.CreatedOn.Value.Date == abcv.Date);
            }
            catch (Exception Ex)
            {
                //todo catch exception logging
                //throw;
            }
            return a;
        }

        public virtual async Task<bool> CreateKey(SessionKey NewUserSession)
        {
            bool a = false;
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                string timeZone = Configuration["TimeZone"].ToString();
                var abcv = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone));
                SessionKey b = await _dbContext.SessionKey.FirstOrDefaultAsync(z => z.Userid.Trim() == NewUserSession.Userid && z.IsValid == true);
                if (b == null)
                {
                    _dbContext.SessionKey.Add(NewUserSession);
                    await _dbContext.SaveChangesAsync();
                    a = true;
                }
                else {
                    await ExpireKey(NewUserSession.Userid);
                    _dbContext.SessionKey.Add(NewUserSession);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception Ex)
            {
                //todo catch exception logging
                //throw;
            }
            return a;
        }

        public virtual async Task<bool> ExpireKey(string UserID)
        {
            bool a = false;
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                string timeZone = Configuration["TimeZone"].ToString();
                var abcv = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone));
                SessionKey b = await _dbContext.SessionKey.FirstOrDefaultAsync(z => z.Userid.Trim() == UserID && z.IsValid == true);
                if (b != null)
                {
                    b.IsValid = false;
                    await _dbContext.SaveChangesAsync();
                    a = true;
                }
            }
            catch (Exception Ex)
            {
                //todo catch exception logging
                //throw;
            }
            return a;
        }
    }
}
