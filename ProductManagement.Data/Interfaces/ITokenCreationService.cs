using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Data.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManagement.Data.Interfaces
{
    public interface ITokenCreationService
    {
        AuthenticationResponse CreateToken(IdentityUser user);
    }
}
