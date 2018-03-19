using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Palindrome.Checker.Services.DTOS
{
    [Serializable]
    [DataContract]
    public class ApiResult<T>
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public int code { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public T Data { get; set; }
    }
}
