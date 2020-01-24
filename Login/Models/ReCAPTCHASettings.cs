using System;
namespace Login.Models
{
    public class ReCAPTCHASettings
    {
        public ReCAPTCHASettings()
        {
        }
        public string ReCAPTCHA_Site_Key { get; set; }
        public string ReCAPTCHA_Secret_Key { get; set; }
    }
}
