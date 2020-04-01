using AutoMapper;

namespace Books.Api
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            this.CreateMap<Entities.Book, Models.Book>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => $"{src.Author.FirstName} {src.Author.LastName}"));

            this.CreateMap<Models.BookForCreation, Entities.Book>();
        }
    }
}