using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models
{
    public class BaseResponse
    {
        public bool IsSucces { get; set; }
        public string ErrorMessage { get; set; }
        public BaseResponse()
        {
            IsSucces = true;
        }
        public BaseResponse(string errorMessage)
        {
            IsSucces = false;
            ErrorMessage = errorMessage;
        }
    }
        public class BaseResponse<T>:BaseResponse
        {
        /// <summary>
        /// Все данные ответа
        /// </summary>
            public T Data { get; set; }
            public BaseResponse(T data):base ()

            {
                Data = data;
            }
            public BaseResponse(string errorMessage) : base(errorMessage)
            {

            }
        }
    
}
