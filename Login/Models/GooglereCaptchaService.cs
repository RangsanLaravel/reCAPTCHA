using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Login.Models
{
    public class GooglereCaptchaService
    {
        private ReCAPTCHASettings _settings;
        public GooglereCaptchaService(IOptions<ReCAPTCHASettings> setings)
        {
            _settings = setings.Value;
        }
        public virtual async Task<GooglereREpo> VerifyreCaptcha(string _Token)
        {
            GooglereCaptchaData googlereCaptchaData = new GooglereCaptchaData
            {
                response = _Token,
                secret = _settings.ReCAPTCHA_Secret_Key
            };
            HttpClient client = new HttpClient();
            var response =await client.GetStringAsync($"https://www.google.com/recaptcha/api/siteverify?=secret{googlereCaptchaData.secret}&response={googlereCaptchaData.response}");
            var capresp = JsonConvert.DeserializeObject<GooglereREpo>(response);
            return capresp;
        } 
    }
    public class GooglereCaptchaData
    {
        public string response { get; set; }
        public string secret { get; set; }
    }
    public class GooglereREpo
    {
        public bool success { get; set; }
        public double  score { get; set; }
        public string acion { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
        
    }
}
