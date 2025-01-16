using AutoMapper;

namespace Dto
{
    public class UserDocumentType_Dto:Profile
    {
        public UserDocumentType_Dto()
        {
            CreateMap<Model.UserDocumentType, UserDocumentType_Dto_For_OnlyActives>().ReverseMap();
        }
    }

    public class UserDocumentType_Dto_For_OnlyActives
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
