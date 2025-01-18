using AutoMapper;
using Model;

namespace Dto
{

    public class User_Dto : Profile
    {
        public User_Dto()
        {
            CreateMap<User, User_Dto_For_ShowInformation>().ReverseMap();
            CreateMap<User, User_Dto_For_Login>().ReverseMap();
        }
    }

    public class User_Dto_For_ShowInformation
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }


    public class User_Dto_For_Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class User_Dto_For_PasswordRestore
    {
        public string Email { get; set; }
    }

}
