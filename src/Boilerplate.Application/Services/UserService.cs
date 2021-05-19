﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Boilerplate.Application.DTOs.Hero;
using Boilerplate.Application.DTOs.User;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Auth;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;


namespace Boilerplate.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _userRepository.Dispose();
            }
        }

        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            if (user == null || !BC.Verify(password, user.Password))
            {
                return null;
            }

            return user;
        }

        public async Task<GetUserDto> CreateUser(CreateUserDto user)
        {
            var created = _userRepository.Create(_mapper.Map<User>(user));
            user.Password = BC.HashPassword(user.Password);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<GetUserDto>(created);
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            await _userRepository.Delete(id);
            return await _userRepository.SaveChangesAsync() > 0;
        }

        public async Task<GetUserDto> UpdatePassword(Guid id, UpdatePasswordDto passwordDto)
        {
            var originalUser = await _userRepository.GetById(id);
            if (originalUser == null) return null;

            originalUser.Password = BC.HashPassword(passwordDto.Password);
            _userRepository.Update(originalUser);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<GetUserDto>(originalUser);
        }
    }
}
