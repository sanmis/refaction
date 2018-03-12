using System;
using refactor_me.Data.Helpers;

namespace refactor_me.Data.Results
{
    public class CommitResult
    {
        public CommitResult(Exception e)
        {
            Error = e;
            Success = false;
        }

        public CommitResult()
        {
            Error = null;
            Success = true;
        }

        public bool Success { get; protected set; }
        public bool NotFound { get; protected set; }
        public Exception Error { get; set; }
        public bool HasError => Error != null;
        public string ErrorMessage => Error?.GetInnerMostException().Message ?? string.Empty;
    }
}
