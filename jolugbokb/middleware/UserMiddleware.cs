using jolugbokb.Data;
using jolugbokb.Models;
using jolugbokb.Utills;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace jolugbokb.middleware
{

    public class UserMiddleware
    {
        private readonly ILogger<UserMiddleware> _logger;
        private readonly KBDBContext _dbContext;
        private readonly Converters _converters;
        public static IConfiguration Configuration { get; set; }
        public UserMiddleware(ILogger<UserMiddleware> logger, KBDBContext dbContext, Converters converters)
        {
            _logger = logger;
            _dbContext = dbContext;
            _converters = converters;
        }

        #region Password

        public async Task<bool> CreatePassword(string Password, int UserID, string type, string source)
        {
            bool a = false;
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                string timeZone = Configuration["TimeZone"].ToString();
                var abcv = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone));
                Converters _conver = _converters;
                var b = _conver.EncryptDecode(Password);
                if (type == "create")
                {
                    UserPass pass = new UserPass
                    {
                        UserId = UserID,
                        DateCreated = abcv,
                        Pass = b,
                        RecCreSource = source,
                        valid = true
                    };
                    await _dbContext.UserSecure.AddAsync(pass);
                    await _dbContext.SaveChangesAsync();
                    a = true;
                }
                else if (type == "update")
                {
                    UserPass password = _dbContext.UserSecure.FirstOrDefault(z => z.UserId == UserID && z.valid == true);
                    if (password != null)
                    {
                        password.valid = false;
                        _dbContext.UserSecure.Attach(password);
                        _dbContext.Entry(password).Property(x => x.valid).IsModified = true;
                        await _dbContext.SaveChangesAsync();
                        UserPass passwordNew = new UserPass
                        {
                            UserId = UserID,
                            DateCreated = abcv,
                            RecCreSource = source,
                            Pass = b,
                            valid = true
                        };
                        await _dbContext.UserSecure.AddAsync(passwordNew);
                        await _dbContext.SaveChangesAsync();
                        a = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("Error occured during process of creating a password for user: " + UserID);
                _logger.LogError("method CreatePassword: {0} " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return a;
        }

        public async Task<GenericResponseMsg> ValidatePass (string Password, int UserID)
        {
            GenericResponseMsg output = new GenericResponseMsg();
            try
            {
                if (Password.Trim().Length < 8)
                {
                    output.a = false;
                    output.b = "Password must be of atleast 8 Characters";
                    return output;
                }
                else
                {
                    output.a = true;
                    output.b = "Password is Valid";
                    return output;
                }
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("Error occured during password validation: " + UserID);
                _logger.LogError("method ValidatePassword: {0} " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return output;
        }

        #endregion

        #region Create Customer details on the database

        public async Task<bool> CheckIfKBUserExist(string Email)
        {
            bool a = false;
            try
            {
                User d = await _dbContext.Users.FirstOrDefaultAsync(m => m.Email.Trim() == Email.Trim());
                if (d != null)
                {
                    return true;
                }

            }
            catch (Exception Ex)
            {
                _logger.LogWarning("Error occured while checking if customer exist: " + Email);
                _logger.LogError("method CheckIfCustomerExist: {0} " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return a;
        }

        public async Task<bool> CreateKBUser(User kbUser)
        {
            Console.WriteLine(kbUser.Email);
            bool a = false;
            try
            {
                User c = await _dbContext.Users.FirstOrDefaultAsync(m => m.Email.Trim() == kbUser.Email.Trim());
                if (!await CheckIfKBUserExist(kbUser.Email))
                {
                    await _dbContext.Users.AddAsync(kbUser);
                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Entry(kbUser).ReloadAsync();
                    a = true;
                }

            }
            catch (Exception Ex)
            {
                _logger.LogWarning("Error occured while creating App user record: " + kbUser.Email);
                _logger.LogError("method CreateAppuserRecord: {0} " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return a;
        }

        public async Task<bool> UpdateKBUser(User kbUser)
        {
            bool a = false;
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                string timeZone = Configuration["TimeZone"].ToString();
                var abcv = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone));
                _dbContext.Entry(kbUser).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                a = true;
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("Error occured while creating App user record: " + kbUser.Email);
                _logger.LogError("method CreateAppuserRecord: {0} " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return a;
        }

        public async Task<User> GetKBUser(string Email)
        {
            User KBUser = new User();
            try
            {
                KBUser = await _dbContext.Users.FirstOrDefaultAsync(z => z.Email == Email.Trim());
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("Error occured while getting App user  record: " + Email);
                _logger.LogError("method GetUserData: {0} " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return KBUser;
        }

        public async Task<IEnumerable<User>> GetAllKBUserAsync()
        {
            IEnumerable<User> KBUsers = new List<User>();
            try
            {
                KBUsers =  _dbContext.Users.Select(u => u).ToList();
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("Error occured while getting App users");
                _logger.LogError("method GetUserData: {0} " + Ex.InnerException + Ex.Message + Ex.StackTrace);
                //throw;
            }
            return KBUsers;
        }


        #endregion

    }

}
