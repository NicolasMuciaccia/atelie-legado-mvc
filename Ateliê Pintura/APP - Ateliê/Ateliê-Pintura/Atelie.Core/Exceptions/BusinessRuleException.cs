using System;
using System.Collections.Generic;
using System.Linq;
using Atelie.Core.Resources;

namespace Atelie.Core.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public readonly IReadOnlyList<string> BusinessErrors;

        public BusinessRuleException(string message) : base(message)
        {
            BusinessErrors = new List<string> { message };
        }

        public BusinessRuleException(List<string> errors) : base(errors?.FirstOrDefault() ?? GlobalMessages.MultiplosErrors)
        {
            BusinessErrors = errors ?? new List<string>();
        }

        public BusinessRuleException(string message, Exception innerException) : base(message, innerException)
        {
            BusinessErrors = new List<string> { message };
        }
    }
}