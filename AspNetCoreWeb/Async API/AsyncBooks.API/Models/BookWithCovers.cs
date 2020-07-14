using System.Collections.Generic;

namespace AsyncBooks.API.Models
{
    public class BookWithCovers : Book
    {
        public IEnumerable<BookCover> BookCovers { get; set; } = new List<BookCover>();
    }
}