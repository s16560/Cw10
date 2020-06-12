using System;

namespace Cw5.DTOs.Responses
{
    public class Response
    {
        
        public string ResultCode { get; set; }
        public string Message { get; set; }
        public Object Obj { get; set; }
      
        public Response(string ResultCode, string Message, Object Obj)
        {
            this.ResultCode = ResultCode;
            this.Message = Message;
            this.Obj = Obj;
        }


    }
}
