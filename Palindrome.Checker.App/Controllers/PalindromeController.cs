using System;
using System.Collections.Generic;
using Lychee.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Palindrome.Checker.App.Models;
using Palindrome.Checker.Services;
using Palindrome.Checker.Services.Common;
using Palindrome.Checker.Services.DTOS;

namespace Palindrome.Checker.App.Controllers
{
    [Produces("application/json")]
    [Route("api/Palindrome")]
    public class PalindromeController : Controller
    {
        private IPalindromeService _palindromeService;
        readonly ILogger<PalindromeController> _log;

        public PalindromeController(IPalindromeService palindromeService, ILogger<PalindromeController> log)
        {
            _palindromeService = palindromeService;
            _log = log;
        }

        [HttpPost("{CheckPalindrome}")]
        public IActionResult Create([FromBody] CreatePalindromeRequestModel request)
        {
            if (request == null)
            {
                return BadRequest(GetInvalidRequestResponse(new Exception("Request is empty.")));
            }

            try
            {
                Precondition.CheckArgument(!string.IsNullOrEmpty(request.StringToSave), "AccountNumber", "Account Number cannot be empty.");
                
                _log.LogInformation("\nPalindromeController.Create - Started. - " + DateTime.Now);

                PalindromeApiResult palindromeApiResult;
                var httpStatusCode = _palindromeService.CreatePalindrome(
                    new CreatePalindromeRequest
                    {
                        StringToSave = request.StringToSave
                    }, out palindromeApiResult);

                _log.LogInformation("\nPalindromeController.Create - Ended. - " + DateTime.Now);
                return StatusCode(httpStatusCode.GetHashCode(), palindromeApiResult);
            }
            catch (ArgumentException ArgEx)
            {
                _log.LogError(ArgEx, "\nPalindromeController.Create failed. - " + DateTime.Now);
                return BadRequest(new PalindromeApiResult
                {
                    Status = ApiStatusCode.Error,
                    code = 10,
                    Message = ArgEx.Message,
                    Data = ArgEx.InnerException
                });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "PalindromeController.Create failed.");
                return StatusCode(500, GetInternalServerErrorResponse(ex));
            }
        }

        [HttpGet]
        public IActionResult GetPalindromes()
        {
            try
            {
                _log.LogInformation("\nPalindromeController.Get - Started. - " + DateTime.Now);

                PalindromeApiResult palindromeApiResult;
                var httpStatusCode = _palindromeService.GetPalindromes(out palindromeApiResult);

                _log.LogInformation("\nPalindromeController.Get - Ended. - " + DateTime.Now);
                return StatusCode(httpStatusCode.GetHashCode(), palindromeApiResult);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "PalindromeController.Get failed.");
                return StatusCode(500, GetInternalServerErrorResponse(ex));
            }
        }

        private PalindromeApiResult GetInvalidRequestResponse(Exception ex)
        {
            var errors = new List<string> { ex.Message };

            if (ex.InnerException != null)
            {
                errors.Add(ex.InnerException.Message);
            }

            return new PalindromeApiResult
            {
                Status = ApiStatusCode.Error,
                code = 10,
                Message = "Invalid Request.",
                Data = errors
            };
        }

        private PalindromeApiResult GetInternalServerErrorResponse(Exception ex)
        {
            var errors = new List<string> { ex.Message };

            if (ex.InnerException != null)
            {
                errors.Add(ex.InnerException.Message);
            }

            return new PalindromeApiResult
            {
                Status = ApiStatusCode.Error,
                code = 20,
                Message = "Unexpected Error.",
                Data = errors
            };
        }
    }
}