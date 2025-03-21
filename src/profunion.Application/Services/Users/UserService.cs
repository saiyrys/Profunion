﻿using AutoMapper;
using Microsoft.Extensions.Configuration;
using profunion.Applications.Interface.IFiles;
using profunion.Applications.Interface.IUser;

using profunion.Domain.Models.UserModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.Data;
using profunion.Infrastructure.SomeService.HashPassword;
using profunion.Shared.Common.dto;
using profunion.Shared.Common.Interfaces;
using profunion.Shared.Common.Service;
using profunion.Shared.Dto.Category;
using profunion.Shared.Dto.Events;
using profunion.Shared.Dto.Uploads;
using profunion.Shared.Dto.Users;

namespace profunion.Applications.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPagination _pagination;
        private readonly IUpdateMethods _update;
        private readonly ISortUser _sortUser;
        private readonly ApplicationDbContext _context;

        public UserService(IUserRepository userRepository, IMapper mapper, IPagination pagination, IUpdateMethods update, ISortUser sortUser, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _pagination = pagination;
            _update = update;
            _sortUser = sortUser;
            _context = context;
        }

        public async Task<(IEnumerable<GetUserDto> Users, int TotalPage)> GetUsersByAdmin(int page, UserQueryDto query, SortStateUser sort)
        {
            int pageSize = 12;

            var users = await _userRepository.GetAllAsync();

            var userDto = _mapper.Map<IEnumerable<GetUserDto>>(users);

            if (!string.IsNullOrEmpty(query.search))
            {
                userDto = await Search<GetUserDto>.SearchEntities(userDto, query.search);
            }

            if (!string.IsNullOrEmpty(query.role))
            {
                userDto = userDto.Where(u => u.role == query.role.ToUpper());
            }

            if (sort != SortStateUser.Current)
            {
                userDto = _sortUser.SortObject(userDto, sort);
            }

            if (query.created_at_start != null || query.created_at_end != null || query.updated_at_start != null || query.updated_at_end != null )
            {
                userDto = userDto.Where(n =>
                    (!string.IsNullOrEmpty(query.created_at_start) ? n.createdAt.Date >= DateTime.Parse(query.created_at_start).Date : true) &&
                    (!string.IsNullOrEmpty(query.created_at_end) ? n.createdAt.Date <= DateTime.Parse(query.created_at_end).Date : true) &&
                    (!string.IsNullOrEmpty(query.updated_at_start) ? n.updatedAt.Date >= DateTime.Parse(query.updated_at_start).Date  : true) &&
                    (!string.IsNullOrEmpty(query.updated_at_end) ? n.updatedAt.Date <= DateTime.Parse(query.updated_at_end).Date : true)
                ).ToList();
            }

            var pagination = await _pagination.Paginate(userDto, page);
            userDto = pagination.Items;

            var totalPages = pagination.TotalPages;

            return (userDto, totalPages);
        }

        public async Task<GetUserDto> GetUserById(long userName)
        {
            var user = await _userRepository.GetByIdAsync(userName);

            if (user == null)
                throw new KeyNotFoundException();

            var userDto = _mapper.Map<GetUserDto>(user);

            return userDto;
        }

        public async Task<bool> UpdateUsers(long userId, UpdateUserDto updateUser)
        {
            var currentUser = await _userRepository.GetByIdAsync(userId);

            if (currentUser == null)
                throw new UnauthorizedAccessException("Пользователь не найден");

            // Проверяем, может ли текущий пользователь обновлять данные
            if (currentUser.role != "ADMIN" && currentUser.userId != userId)
                throw new UnauthorizedAccessException("Недостаточно прав для изменения данных");

            if (string.IsNullOrWhiteSpace(updateUser.password))
            {
                updateUser.password = null; // Это решит проблему
            }

            await _update.UpdateEntity<User, UpdateUserDto, long>(userId, updateUser);

            if (!string.IsNullOrEmpty(updateUser.password) && !string.IsNullOrWhiteSpace(updateUser.password))
            {
                HashingPassword hashing = new();
                var (hashedPassword, salt) = await hashing.HashPassword(updateUser.password);

                currentUser.password = hashedPassword;
                currentUser.salt = Convert.ToBase64String(salt);
            }

            currentUser.updatedAt = DateTime.UtcNow;

            
            if (!await _userRepository.UpdateAsync(currentUser))
            {
                throw new ArgumentException("Ошибка при обновлении данных пользователя");
            }

            return true;
        }

        public async Task<bool> DeleteUser(long userId)
        {
            var userToDelete = await _userRepository.GetByIdAsync(userId);

            if (!await _userRepository.Delete(userToDelete))
            {
                throw new ArgumentException(" ", "Ошибка удаления пользователя");
            }

            return true;
        }

        
    }
}
