using System;
using System.Collections.Generic;
using Atelie.Core.Resources;

namespace Atelie.Core.Exceptions
{
    public class ValidationRuleException : Exception
    {
        public readonly IReadOnlyDictionary<string, List<string>> ValidationErrors;

        public ValidationRuleException(string field, string message) : base(GlobalMessages.ErroValidacao)
        {
            ValidationErrors = new Dictionary<string, List<string>>
            {
                { field, new List<string> { message } }
            };
        }

        public ValidationRuleException(Dictionary<string, List<string>> validationErrors) : base(GlobalMessages.ErrosValidacao)
        {
            ValidationErrors = validationErrors;
        }
    }
}