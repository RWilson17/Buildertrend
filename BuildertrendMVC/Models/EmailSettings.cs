namespace BuildertrendMVC.Models
{
    public class EmailSettings
    {
        public string SmtpHost { get; set; } = "smtp.tu-servidor.com";
        public int SmtpPort { get; set; } = 587;
        public string SmtpUser { get; set; } = "usuario@tu-servidor.com";
        public string SmtpPass { get; set; } = "password";
        public string From { get; set; } = "no-reply@tu-servidor.com";
    }
}
