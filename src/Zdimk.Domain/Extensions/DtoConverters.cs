using Zdimk.Domain.Dtos;
using Zdimk.Domain.Entities;

namespace Zdimk.Domain.Extensions
{
    public static class DtoConverters
    {
        public static UserPrivateDto ToUserPrivateDto(this User user)
        {
            return new UserPrivateDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                RegistrationDate = user.RegistrationDate,
                LastLoginDate = user.LastLoginDate
            };
        }
        
        public static UserPublicDto ToUserPublicDto(this User user)
        {
            return new UserPublicDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                BirthDate = user.BirthDate,
            };
        }
    }
}