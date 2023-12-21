using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TUSO.Domain.Dto
{
    public class ResponseDto
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }
        public dynamic Data { get; set; }
        public bool IsSuccess { get; set; }

        public ResponseDto(HttpStatusCode httpStatusCode, bool isSuccess, string message, dynamic data) {
        
            StatusCode = httpStatusCode;
            IsSuccess = IsSuccess;
            Message = message;
            Data = data;
        }


    }
}
