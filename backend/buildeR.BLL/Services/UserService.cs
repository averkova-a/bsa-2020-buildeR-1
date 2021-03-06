using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using buildeR.BLL.Exceptions;
using buildeR.BLL.Interfaces;
using buildeR.Common.DTO;
using buildeR.Common.DTO.User;
using buildeR.Common.DTO.UserSocialNetwork;
using buildeR.DAL.Context;
using buildeR.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;

namespace buildeR.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly BuilderContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailBuilder _emailBuilder;
        private readonly IEmailService _emailService;
        private readonly IFileProvider _fileProvider;
        public UserService(BuilderContext context,
                           IMapper mapper,
                           IEmailService emailService,
                           IEmailBuilder emailBuilder,
                           IFileProvider fileProvider)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _emailBuilder = emailBuilder;
            _fileProvider = fileProvider;
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _context.Users.Include(u => u.UserSocialNetworks)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("user", id);
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> Login(string UId)
        {
            var user = await _context.Users
                .Include(u => u.UserSocialNetworks)
                .FirstOrDefaultAsync(u => u.UserSocialNetworks.Any(sn => sn.UId == UId));
            if (user != null)
            {
                return _mapper.Map<UserDTO>(user);
            }
            else
            {
                return null;
            }
        }

        public async Task<ICollection<UserDTO>> GetAll()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<ICollection<UserDTO>>(users);
        }

        public async Task<UserDTO> Register(NewUserDTO creatingUser)
        {

            var userSN = new NewUserSocialNetworkDTO()
            {
                UId = creatingUser.UId,
                ProviderName = creatingUser.ProviderName,
                SocialNetworkUrl = creatingUser.ProviderUrl,
            };

            var user = _mapper.Map<User>(creatingUser);
            user.CreatedAt = DateTime.Now;
            _context.Add(user);
            await _context.SaveChangesAsync();

            var userDto = _mapper.Map<UserDTO>(user);

            userSN.UserId = userDto.Id;
            var userSNEntity = _mapper.Map<UserSocialNetwork>(userSN);
            _context.Add(userSNEntity);
            await _context.SaveChangesAsync();

            var emailModel = _emailBuilder.GetSignUpLetter(creatingUser.Email, creatingUser.FirstName);
            await _emailService.SendEmailAsync(new List<string> { emailModel.Email }, emailModel.Subject, emailModel.Title, emailModel.Body);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new NotFoundException("user", id);
            }
            _context.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDTO> Update(UserDTO userDTO)
        {
            var user = _mapper.Map<User>(userDTO);
            var existing = await _context.Users
                .Include(u => u.UserSocialNetworks)
                .FirstOrDefaultAsync(x => x.Id == userDTO.Id);
            if (existing == null)
            {
                throw new NotFoundException("user", userDTO.Id);
            }
            _context.Entry(existing).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDTO>(existing);
        }
        public async Task<UserDTO> UpdateUserAvatar(IFormFile file, int userId)
        {
            string dbPath = await _fileProvider.UploadUserPhoto(file);
            var user = await GetUserById(userId);
            user.AvatarUrl = dbPath;
            return await Update(user);
        }

        public async Task<bool> ValidateUsername(ValidateUserDTO user)
        {
            if (user.Id != 0)
            {
                return await _context.Users.AnyAsync(x => x.Username.ToLower() == user.Username.ToLower() && x.Id != user.Id);
            }
            else
            {
                return await _context.Users.AnyAsync(x => x.Username.ToLower() == user.Username.ToLower());
            }
        }

        public async Task<UserDTO> LinkProvider(LinkProviderDTO userLink)
        {
            var isUserExist = await _context.Users.AnyAsync(u => u.Id == userLink.UserId);

            if (isUserExist)
            {

                var userSN = new NewUserSocialNetworkDTO()
                {
                    UId = userLink.UId,
                    ProviderName = userLink.ProviderName,
                    SocialNetworkUrl = userLink.ProviderUrl,
                };

                userSN.UserId = userLink.UserId;
                var userSNEntity = _mapper.Map<UserSocialNetwork>(userSN);
                _context.Add(userSNEntity);
                await _context.SaveChangesAsync();

                var updatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userLink.UserId);
                return _mapper.Map<UserDTO>(updatedUser);
            }
            else
            {
                throw new NotFoundException("user", userLink.UserId);
            }
        }

        public async Task AddUserLetter(UserLetterDTO newUserLetter)
        {
            var userLetter = _mapper.Map<UserLetter>(newUserLetter);
            await _context.Set<UserLetter>().AddAsync(userLetter);
            await _context.SaveChangesAsync();
            
            string strSubject = $"Feedback from {newUserLetter.UserName}: {newUserLetter.Subject}";
            await _emailService.SendEmailAsync(new List<string> {_emailService.SupportEmail},
                new EmailAddress(newUserLetter.UserEmail), strSubject, newUserLetter.Description);
            
            string strBody = @$"We want to thank you for your letter!<br><br>Each of your letters is very important to us! 
                          We have received your letter and will contact you as soon as possible.
                          <br><br>Cheers,<br>buildeR team";
            
            var emailModel = _emailBuilder.GetFeedbackLetter(newUserLetter.UserEmail, newUserLetter.UserName, newUserLetter.Subject, strBody);
            await _emailService.SendEmailAsync(new List<string> { emailModel.Email }, 
                emailModel.Subject, emailModel.Title, emailModel.Body);
        }

        public async Task<ICollection<UserLetterUserIdDTO>> GetAllUserLetters()
        {
            var users = await _context.Users.ToListAsync();
            var userLetters = await _context.UserLetters.ToListAsync();
            var userLettersUserIdDTO = users.Join(userLetters, user => user.Email,
                letter => letter.UserEmail,
                (user, letter) => new UserLetterUserIdDTO()
                {
                    Id = letter.Id,
                    UserEmail = letter.UserEmail,
                    UserName = letter.UserName,
                    Subject = letter.Subject,
                    Description = letter.Description,
                    IsRespond = letter.IsRespond,
                    UserId = user.Id
                }).OrderByDescending(l => l.Id);
            return _mapper.Map<ICollection<UserLetterUserIdDTO>>(userLettersUserIdDTO);
        }

        public async Task SendLetterToUser(UserLetterAnswerDTO userLetter)
        {
            var currentLetter = _mapper.Map<UserLetterDTO>(userLetter);
            
            string body = $"In this letter we try to help you.<br><br> We received your letter - \" {userLetter.Description}\" " +
                          $"<br><br>We have an answer to you question!<br><br> {userLetter.Answer}" +
                          "<br><br>We hope that we helped you!" +
                          "<br><br>Cheers,<br>buildeR team";
            
            var emailModel = _emailBuilder.GetFeedbackLetter(userLetter.UserEmail, userLetter.UserName, userLetter.Subject, body);
            await _emailService.SendEmailAsync(new List<string> { emailModel.Email }, 
                emailModel.Subject, emailModel.Title, emailModel.Body);
            
            currentLetter.IsRespond = true;
            await UpdateUserLetter(currentLetter);
        }

        public async Task<UserLetter> UpdateUserLetter(UserLetterDTO userLetter)
        {
            var letter = _mapper.Map<UserLetter>(userLetter);
            var existing = await _context.UserLetters
                .FirstOrDefaultAsync(x => x.Id == letter.Id);
            if (existing == null)
            {
                throw new NotFoundException("userLetter", letter.Id);
            }
            _context.Entry(existing).CurrentValues.SetValues(letter);
            await _context.SaveChangesAsync();
            return letter;
        }
        
        public async Task<ICollection<UserLetterDTO>> GetUserLettersCheckRespond(bool isRespond)
        {
            var userLetters = await _context.UserLetters.Where(l => l.IsRespond == isRespond).ToListAsync();
            return _mapper.Map<ICollection<UserLetterDTO>>(userLetters);
        }
    }
}