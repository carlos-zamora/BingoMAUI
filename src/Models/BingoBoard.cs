namespace BingoMAUI.Models
{
    public class BingoBoard
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public int Size { get; set; }
        public string[] Content { get; set; } = Array.Empty<string>();
        public bool[] Marked { get; set; } = Array.Empty<bool>();

        public BingoBoard()
        {
        }

        public BingoBoard(string id)
        {
            Id = id;
        }

        public BingoBoard(string name, int size, string[] content)
        {
            Name = name;
            Size = size;
            Content = content;
            Marked = new bool[size * size];
        }

        public static BingoBoard CreateNewBingoBoard()
        {
            return new BingoBoard();
        }
    }
}
