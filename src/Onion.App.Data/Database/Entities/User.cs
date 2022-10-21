﻿using Onion.App.Data.Database.Entities.Fields;

namespace Onion.App.Data.Database.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string GoogleSubjectId { get; set; }
    public UserRole Role { get; set; }
    public IEnumerable<RefreshToken> RefreshTokens { get; set; }

    public IEnumerable<TodoList> TodoLists { get; set; }
}