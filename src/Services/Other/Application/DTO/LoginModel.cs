﻿using System.ComponentModel.DataAnnotations;

namespace Application.DTO;

[Serializable]
public class LoginModel
{
    [Required(ErrorMessage = "用户名不能为空")] 
    public string Username { get; set; }


    [Required(ErrorMessage = "密码不能为空")]
    public string Password { get; set; }
}