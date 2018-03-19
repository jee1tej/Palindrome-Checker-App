using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Palindrome.Checker.Services.DTOS
{
    [Serializable]
    [DataContract]
    class GetPalindromeResponse
    {
        [DataMember]
        public List<string> Palindromes { get; set; }
    }
}
