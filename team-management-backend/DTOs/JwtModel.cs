﻿/*
 * @author Aaron Pérez
 * @since 07/10/2024
 */

namespace team_management_backend.DTOs
{
    public class JwtModel
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
    }
}
