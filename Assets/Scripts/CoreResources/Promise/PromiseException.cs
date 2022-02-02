using System;
using CoreResources.Pool;

namespace CoreResources.Promise
{
    public class PromiseException : Exception
    {
        public PromiseException(string message) : base(message)
        {}

        protected PromiseException(string message, Exception innerException) : base(message, innerException)
        {}

        public Exception GetBaseException(Type searchType = null)
        {
            Exception exception = this;
            Func<Exception, bool> stopSearch =
                searchType == null
                    ? new Func<Exception, bool>(_ => false)
                    : searchType.IsInstanceOfType;
            while (!stopSearch(exception) && exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            return exception;
        }
        
        public static PromiseException NonPending(string methodName, PromiseState state)
        {
            string stateName = Enum.GetName(typeof(PromiseState), state);

            return new PromiseException(
                string.Format(
                    "PromiseException | Calling '{0}()' on a non-pending promise in state: {1}",
                    methodName,
                    stateName));
        }

        public static PromiseException PooledInteraction(string methodName, PoolablePromise promise)
        {
            return new PromiseException(
                string.Format(
                    "PromiseException | Calling '{0}()' on a pooled instance of a promise: {1}",
                    methodName,
                    promise.ToString()));
        }
    }
}