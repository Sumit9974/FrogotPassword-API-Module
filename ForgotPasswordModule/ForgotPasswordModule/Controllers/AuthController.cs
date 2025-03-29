using ForgotPasswordModule.Models;
using ForgotPasswordModule.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace ForgotPasswordModule.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly EmailService emailService;
        private Timer timer;




        public AuthController(EmailService emailService)
        {
            this.emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> ValidateOTP(string otp)
        {
            if(emailService.otp == otp)
            {
                return Ok(new { otp = otp, message = "Your OTP is valid", isOtpValid = true });
            }
            return BadRequest(new { otp = otp, message = "Your OTP is invalid", isOtpValid = false});
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordDTO request)
        {
            string otp = new Random().Next(100000, 999999).ToString();
            DateTime expiryTime = DateTime.UtcNow.AddMinutes(10);

            // Store OTP in Database
            var otpRecord = new OtpRecord
            {
                Email = request.Email,
                Otp = otp,
                ExpiryTime = expiryTime
            };



            await emailService.sendEmailAsync(request.Email, "Password Reset Test OTP", $"Yout OTP is : {otp}");


            emailService.otp = otp;


            timer = new Timer((state) => { 
            
                if(emailService.otp != "")
                {
                    emailService.otp = "";
                    timer.Dispose();
                }
            
            }, null, TimeSpan.FromSeconds(50), Timeout.InfiniteTimeSpan);

            //await Task.Delay(TimeSpan.FromSeconds(50)).ContinueWith(_ =>
            //{
            //    lock (emailService.otp)
            //    {
            //        emailService.otp = "";
            //    }
            //});
             
            return Ok(otpRecord);

        }
    }
}
