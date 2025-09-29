namespace Domain
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }

        public Book(string title, string author, string isbn, int quantity)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            Quantity = quantity;
        }
    }
}
