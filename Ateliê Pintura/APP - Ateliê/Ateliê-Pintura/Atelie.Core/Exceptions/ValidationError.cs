using System.Collections.Generic;

namespace Atelie.Core.Exceptions
{
    public class ValidationError
    {
        public Dictionary<string, List<string>> Errors { get; private set; }

        public ValidationError()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public void AddError(string field, string message)
        {
            if (!Errors.ContainsKey(field))
                Errors[field] = new List<string>();

            Errors[field].Add(message);
        }
    }
}
