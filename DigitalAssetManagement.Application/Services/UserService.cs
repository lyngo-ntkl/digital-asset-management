using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Dtos.Responses.Users;
﻿using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Application.Services
{
    public interface UserService
    {
        Task<AuthResponse> LoginWithEmailPassword(EmailPasswordAuthRequest request);
        Task Register(EmailPasswordRegistrationRequest request);
        Task<User?> Get(int id);
    }
}
