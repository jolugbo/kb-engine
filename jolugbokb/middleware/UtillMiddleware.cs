using jolugbokb.Models;
using jolugbokb.Utills;
using Jose;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace jolugbokb.middleware
{

    public class UtillMiddleware
    {
        private readonly ILogger<UtillMiddleware> _logger;
        private readonly UserMiddleware _userMiddleware;
        private readonly Converters _converters;
        private readonly OTPUtills _OTPUtills;
        public static IConfiguration Configuration { get; set; }
        public UtillMiddleware(ILogger<UtillMiddleware> logger, UserMiddleware userMiddleware, Converters converters)
        {
            _logger = logger;
            _userMiddleware = userMiddleware;
            _converters = converters;
        }

        public async Task<OutputMessage> SignOut(string UserID)
        {
            OutputMessage res = new OutputMessage();
            try
            {
                SessionKey a = await _OTPUtills.GetKey(UserID);
                if (a != null)
                {
                    await _OTPUtills.ExpireKey(UserID);
                }
                res.Message = "User signed out succesfully";
                res.HttpStatusCode = 200;
                return res;
            }
            catch (Exception Ex)
            {
                _logger.LogWarning("Error occured while signing out client");
                _logger.LogError("method SignOut: {0} " + Ex.InnerException + Ex.Message + Ex.StackTrace);

                //throw;
            }
            res.Message = "Error occured kindly try again later";
            res.HttpStatusCode = 501;
            return res;
        }

        public async Task<OutputMessage> SendUserOTP(SendOTPData data)
        {
            OutputMessage res = new OutputMessage();
            try
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
                Configuration = builder.Build();
                string timeZone = Configuration["TimeZone"].ToString();
                var abcv = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(timeZone));
                User a = await _userMiddleware.GetKBUser(data.RecipientEmail);
                if (a == null)
                {
                    _logger.LogWarning("method SendUserOTP: Customers Email does not exist " + data.RecipientEmail);
                    res.Message = "Customer does not exist";
                    res.HttpStatusCode = 406;
                    return res;
                }
                if (a != null)
                {
                    string UserOTP = new OTPUtills().GenOTP(6);
                    if (data.Reason.Trim().ToLower() == "registration")
                    {
                        if (a.UserValidated)
                        {
                            _logger.LogWarning("method SendUserOTP: Customers Email  already exist " + data.RecipientEmail);
                            res.Message = "Customer already validated";
                            res.HttpStatusCode = 406;
                            return res;
                        }
                        //TODO SEND EMAIL NOTIFICATIONS
                        res.Message = "OTP successfuly sent to costomer";
                        res.HttpStatusCode = 200;
                        return res;
                    }
                    if (data.Reason.Trim().ToLower() == "ProfileEdit")
                    {
                        //TODO SEND EMAIL NOTIFICATIONS
                        res.Message = "OTP successfuly sent to costomer";
                        res.HttpStatusCode = 200;
                        return res;
                    }
                }

            }
            catch (Exception Ex)
            {
                _logger.LogWarning("Error Encountered for User:" + data.RecipientEmail);
                _logger.LogError("method SendUserOTP: {0} " + Ex.Message + Ex.StackTrace + Ex.InnerException);
                //throw;
            }
            return res;
        }
    }

}
