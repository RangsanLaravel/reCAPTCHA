using System;
using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    public class UserModel
    {
        public UserModel()
        {
        }
        [Required]
        [Display(Name ="ชื่อผู้ใช้",Prompt = "ชื่อผู้ใช้",Description = "ชื่อผู้ใช้")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Display(Name ="รหัสผ่าน",Prompt = "รหัสผ่าน",Description = "รหัสผ่าน")]
        public string PassWord { get; set; }
        public string Token { get; set; }
    }
}
