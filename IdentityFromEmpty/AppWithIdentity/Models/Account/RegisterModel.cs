﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppWithIdentity.Models.Account
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "{0}を入力してください")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("メールアドレス")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0}を入力してください")]
        [DataType(DataType.Password)]
        [DisplayName("パスワード")]
        public string Password { get; set; }
    }
}
