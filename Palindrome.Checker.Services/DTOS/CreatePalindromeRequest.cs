using System;
using System.Runtime.Serialization;

namespace Palindrome.Checker.Services.DTOS
{
    [Serializable]
    [DataContract]
    public class CreatePalindromeRequest
    {
        [DataMember]
        public string StringToSave { get; set; }
    }
}
