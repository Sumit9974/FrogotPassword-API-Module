namespace ForgotPasswordModule.Models
{
    public class OtpRecord
    {
        public int Id { get; set; }
        public string Email { get; set; } = String.Empty;
        public string Otp { get; set; } = String.Empty;
        public DateTime ExpiryTime { get; set; }
    }

}
