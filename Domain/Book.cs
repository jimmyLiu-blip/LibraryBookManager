namespace Domain
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int Quantity { get; set; }
        public int AvailableQuantity {  get; set; }

        public Book(string title, string author, string isbn, int quantity,int availableQuantity)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            Quantity = quantity;
            AvailableQuantity = quantity;
        }

        public override string ToString()
        {
            return $"書名：{PadRightMix(Title,20)} 作者：{PadRightMix(Author,16)} ISBN：{ISBN,-12} 總數量：{Quantity,3} 可借數量：{AvailableQuantity,3}";
        }

        private string PadRightMix(string input, int totalLength)
        {
            int realLength = 0;
            foreach (var c in input)
            {
                realLength += c > 127 ? 2 : 1;
            }

            int padding = totalLength - realLength;
            if (padding > 0)
            {
                return input + new string(' ', padding);
            }
            return input;
        }
    }
}
