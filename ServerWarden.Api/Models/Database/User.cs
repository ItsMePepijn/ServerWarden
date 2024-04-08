﻿namespace ServerWarden.Api.Models.Database
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsSuperAdmin { get; set; }
    }
}
