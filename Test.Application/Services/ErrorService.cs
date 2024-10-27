using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Application.Abstraction;
using Test.Domain.Entities;

namespace Test.Application.Services
{
    public class ErrorService : IErrorService
    {
        private readonly ILogger<ErrorService> _logger;

        public ErrorService(ILogger<ErrorService> logger)
        {
            _logger = logger;
        }

        public void AddErrorRecord(string message, Exception exception)
        {
            _logger.LogError(exception, "An error occurred: {Message}", message);
        }
    }
}
