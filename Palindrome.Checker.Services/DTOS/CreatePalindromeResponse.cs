using System;
using System.Runtime.Serialization;

namespace Palindrome.Checker.Services.DTOS
{
    [Serializable]
    [DataContract]
    public class CreatePalindromeResponse
    {
        [DataMember]
        public bool IsPalindrome { get; set; }
    }
}
