using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HelloMvc
{
    public class HomeController : Controller
    {        
        [HttpPost("/Encrypt")]
        public string Encrypt(int ExpireDays, string Id, string TokenType)
        {
            TokenLogic tl = new TokenLogic();
            
            string newToken;
            
            tl.GenerateToken(ExpireDays, Id, TokenType, out newToken);
            
            EncryptResponse response = new EncryptResponse();
            response.Success = true;
            response.Token = newToken;
            
            return JsonConvert.SerializeObject(response);
        }
        
        [HttpPost("/Decrypt")]
        public string Decrypt(string Token)
        {
            TokenLogic tl = new TokenLogic();
            Document tokenDoc;
            DecryptResponse response = new DecryptResponse();
            
            response.Success = false;
            response.Token = null;
           
            if(tl.ValidateToken(Token, out tokenDoc))
            {
                response.Success = true;
                response.Token = tokenDoc;
            }
            return JsonConvert.SerializeObject(response);
        }
    }
    
    public class EncryptResponse
    {
        public bool Success {get;set;}
        public string Token {get;set;}      
    }
    
    public class DecryptResponse
    {
        public bool Success {get;set;}
        public Document Token {get;set;}      
    }
}