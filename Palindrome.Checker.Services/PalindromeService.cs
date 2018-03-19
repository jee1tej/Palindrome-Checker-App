using Microsoft.Extensions.Logging;
using Palindrome.Checker.Services.Common;
using Palindrome.Checker.Services.DTOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Palindrome.Checker.Services
{
    public interface IPalindromeService
    {
        HttpStatusCode CreatePalindrome(CreatePalindromeRequest createTranscationRequest, out PalindromeApiResult palindromeApiResult);
        HttpStatusCode GetPalindromes(out PalindromeApiResult palindromeApiResult);
    }

    public class PalindromeService : IPalindromeService
    {
        readonly ILogger<PalindromeService> _log;
        public PalindromeService(ILogger<PalindromeService> log)
        {
            _log = log;
        }
        public HttpStatusCode CreatePalindrome(CreatePalindromeRequest createTranscationRequest, out PalindromeApiResult palindromeApiResult)
        {
            _log.LogInformation("\nPalindromeService.CreateTransaction - Started. - " + DateTime.Now);

            palindromeApiResult = WriteTransaction(createTranscationRequest.StringToSave);

            if (palindromeApiResult.Status != ApiStatusCode.Success)
            {
                _log.LogError(new EventId(), (Exception)palindromeApiResult.Data, "\nPalindromeService.CreateTransaction - Failed. - " + DateTime.Now);

                palindromeApiResult.Data = new CreatePalindromeResponse
                {
                    IsPalindrome = false
                };

                return HttpStatusCode.InternalServerError;
            }

            _log.LogInformation("\nPalindromeService.CreateTransaction - Ended. - " + DateTime.Now);
            return HttpStatusCode.OK;
        }

        public HttpStatusCode GetPalindromes(out PalindromeApiResult palindromeApiResult)
        {
            _log.LogInformation("\nPalindromeService.GetPalindromes - Started. - " + DateTime.Now);

            palindromeApiResult = new PalindromeApiResult
            {
                Data = new GetPalindromeResponse
                {
                    Palindromes = File.ReadLines(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\..\")) + "Logs" + @"\Palindromes.txt").ToList<string>()
                }
            };
            
            _log.LogInformation("\nPalindromeService.GetPalindromes - Ended. - " + DateTime.Now);
            return HttpStatusCode.OK;
        }

        private PalindromeApiResult WriteTransaction(string StringToSave)
        {
            try
            {
                
                var original = Regex.Replace(StringToSave, "[^0-9a-zA-Z]+", "");
                var reversed = new string(original.Reverse().ToArray());
                var palindrome = original.ToUpper() == reversed.ToUpper();

                if (palindrome)
                {
                    if (!PalindromeExists(StringToSave))
                    {
                        using (StreamWriter outputFile = File.AppendText(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\..\")) + "Logs" + @"\Palindromes.txt"))
                        {
                            outputFile.WriteLine(StringToSave);
                        }
                    }
                }

                return new PalindromeApiResult
                {
                    Status = ApiStatusCode.Success,
                    code = 30,
                    Message = palindrome ? "String is a Palindrome" : "String isn't a Palindrome",
                    Data = new CreatePalindromeResponse
                    {
                        IsPalindrome = palindrome
                    }
                };

            }
            catch (Exception ex)
            {
                return new PalindromeApiResult
                {
                    Status = ApiStatusCode.Fail,
                    code = 20,
                    Message = "Exception encountered!!",
                    Data = ex
                };
            }
        }




        private bool PalindromeExists(string StringToSave)
        {
            return File.ReadLines(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\..\")) + "Logs" + @"\Palindromes.txt").Contains(StringToSave);

        }
    }
}
