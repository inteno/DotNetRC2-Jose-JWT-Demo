using System;
using System.Text;
using HelloMvc.Timestamp;
    
    public class TokenLogic
    {
        private const int secondsPerDay = 60 * 60 * 24;
        private byte[] _secretKeyByteArray = Encoding.UTF8.GetBytes("134j13;l12kjhdhausoqhwdre;khj31oyuewd9q0sujokj123ewdusxaph1;4eihdre");
        
    public void GenerateToken(int expireSeconds, string Id, string tokenType, out string newToken)
        {
            var token = new Document();
            var now = UnixTimestamp.ConvertToTimestamp(DateTime.UtcNow);

            token.typ = "JWT";
            token.iat = now;
            token.exp = now + expireSeconds;

            token.Id = Id;
            token.TokenType = tokenType;

            newToken = JWT.Encode(token, _secretKeyByteArray, JwsAlgorithm.HS256);
        }

        private class HeaderParsing
        {
            public string TypString { get; set; }
            public string AlgString { get; set; }

            public void parseHeader(string encodedJwtString)
            {
                object typObject = new object();
                object algObject = new object();
                var headers = JWT.Headers(encodedJwtString);

                if (headers.TryGetValue("typ", out typObject)) {
                    TypString = (string)typObject;
                }
                if (headers.TryGetValue("alg", out algObject)) {
                    AlgString = (string)algObject;
                }
            }
        }
        public bool ValidateToken(string encodedJwtString, out Document token)
        {
            HeaderParsing header = new HeaderParsing();

            header.parseHeader(encodedJwtString);

            if (String.IsNullOrWhiteSpace(header.TypString) || String.IsNullOrWhiteSpace(header.AlgString)) {
                token = null;
                throw new ArgumentNullException("Empty token.");
            }
            if (!String.Equals(header.TypString, "JWT") || !String.Equals(header.AlgString, "HS256")) {
                token = null;
                throw new Exception("Invalid token.");
            }
            try {
                token = JWT.Decode<Document>(encodedJwtString, _secretKeyByteArray);
            }
            catch (Exception) {
                token = null;
                throw new Exception();
            }
            if (UnixTimestamp.ConvertToTimestamp(DateTime.UtcNow) < token.exp) {
                    return true;
            }
            else {
                return false;
            }
        }
    }