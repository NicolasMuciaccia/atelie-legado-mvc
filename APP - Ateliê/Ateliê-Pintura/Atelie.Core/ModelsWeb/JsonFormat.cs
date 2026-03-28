using System;
using System.Collections.Generic;

namespace Atelie.Core.JsonModel
{
    public class JsonFormat
    {
        public object Object { get; set; }
        public bool Success { get; set; }
        public List<String> MessageList { get; set; }
        public List<String> FieldList { get; set; }

        public JsonFormat()
        {
            MessageList = new List<string>();
            FieldList = new List<string>();
            Success = false;
        }
    }
}