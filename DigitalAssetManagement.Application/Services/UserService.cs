using DigitalAssetManagement.Application.Dtos.Requests.Users;
using DigitalAssetManagement.Application.Dtos.Responses.Users;
﻿using DigitalAssetManagement.Domain.Entities;

namespace DigitalAssetManagement.Application.Services
{
    public interface UserService
    {
        Task<AuthResponse> LoginByEmailAndPassword(EmailPasswordAuthRequest request);
        Task Register(EmailPasswordRegistrationRequest request);
        Task<User?> GetById(int id);
        Task<User> GetByEmail(string email);
    }
}
