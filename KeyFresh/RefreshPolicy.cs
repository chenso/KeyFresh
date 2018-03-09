using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace KeyFresh
{
    public class RefreshPolicy
    {
        private RetryPolicy _retryPolicy;

        private RefreshPolicy(RetryPolicy retryPolicy)
        {
            _retryPolicy = retryPolicy;
        }

        public static RefreshPolicy HandleException<TException>(Action action) where TException : Exception
        {
            return HandleException<TException>(x => true, action);
        }

        public static RefreshPolicy HandleException<TException>(Func<TException, bool> exceptionPredicate, Action onException)
            where TException : Exception
        {
            return new RefreshPolicy(
                Policy.Handle(exceptionPredicate).Retry(
                    1, 
                    (_, __) =>
                    {
                        onException();
                    }));
        }

        public static RefreshPolicy AsyncHandleException<TException>(Func<Task> func) where TException : Exception
        {
            return AsyncHandleException<TException>(x => true, func);
        }

        public static RefreshPolicy AsyncHandleException<TException>(Func<TException, bool> exceptionPredicate, Func<Task> onException) where TException : Exception
        {
            return new RefreshPolicy(
                Policy.Handle(exceptionPredicate).RetryAsync(
                    1, 
                    async (_, __) => {
                        await onException().ConfigureAwait(false);
                    }));
        }

        public Task<TResult> ExcecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            return _retryPolicy.ExecuteAsync(action);
        }

        public Task ExcecuteAsync(Func<Task> action)
        {
            return _retryPolicy.ExecuteAsync(action);
        }

        public TResult Excecute<TResult>(Func<TResult> action)
        {
            return _retryPolicy.Execute(action);
        }

        public void Excecute(Action action)
        {
            _retryPolicy.Execute(action);
        }
    }
}
