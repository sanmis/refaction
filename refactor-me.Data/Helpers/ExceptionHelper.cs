using System;

namespace refactor_me.Data.Helpers
{
    public static class ExceptionHelper
    {
        public static Exception GetInnerMostException(this Exception exception)
        {
            var innerMostexception = exception;

            while (innerMostexception.InnerException != null)
                innerMostexception = innerMostexception.InnerException;

            return innerMostexception;
        }
    }
}
