﻿using System.ComponentModel.DataAnnotations;
using System.Data;
using FinanceManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace FinanceManager.Data.Models;


public class User : BaseModel
{
    [Required, StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
    
    public IEnumerable<Account>? Accounts { get; set; }
    
    public IEnumerable<UserRole>? Roles { get; set; }
}