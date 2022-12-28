using System;
using System.Collections.Generic;
using System.Text;

namespace ProductManagement.Data.Dtos
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
