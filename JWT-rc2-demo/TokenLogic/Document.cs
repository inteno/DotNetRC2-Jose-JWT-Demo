public class Document
    {
        //Standard claims:
        public int iat { get; set; }
        public int exp { get; set; }
        public string typ { get; set; }

        //Custom claim
        public string TokenType { get; set; }
        public string Id { get; set; }
    }