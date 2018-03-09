using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace KeyFresh
{
    /// <summary>
    /// Refresh policy definition for actions to take on exception before a retry 
    /// </summary>
    public class RefreshPolicy
    {
        private RetryPolicy _retryPolicy;
        private RetryPolicy _asyncRetryPolicy;

        private RefreshPolicy(RetryPolicy retryPolicy, RetryPolicy asyncRetryPolicy)
        {
            _retryPolicy = retryPolicy;
            _asyncRetryPolicy = asyncRetryPolicy;
        }

        public static RefreshPolicy HandleException<TException>(Action onException) where TException : Exception
        {
            return HandleException<TException>(onException, async () => { onException.Invoke(); await Task.FromResult(0); });
        }

        public static RefreshPolicy AsyncHandleException<TException>(Func<Task> onExceptionAsync) where TException : Exception
        {
            return HandleException<TException>(() => onExceptionAsync.Invoke(), onExceptionAsync);
        }

        public static RefreshPolicy HandleException<TException>(Action onException, Func<Task> onExceptionAsync) where TException : Exception
        {
            return HandleException<TException>(x => true, onException, onExceptionAsync);
        }

        public static RefreshPolicy HandleException<TException>(Func<TException, bool> exceptionPredicate, Action onException) where TException : Exception
        {
            return HandleException(exceptionPredicate, onException, async () => { onException.Invoke(); await Task.FromResult(0); });
        }

        public static RefreshPolicy AsyncHandleException<TException>(Func<TException, bool> exceptionPredicate, Func<Task> onExceptionAsync) where TException : Exception
        {
            return HandleException(exceptionPredicate, () => onExceptionAsync.Invoke(), onExceptionAsync);
        }

        public static RefreshPolicy HandleException<TException>(Func<TException, bool> exceptionPredicate, Action onException, Func<Task> onExceptionAsync)
            where TException : Exception
        {
            return new RefreshPolicy(
                Policy.Handle(exceptionPredicate).Retry(
                    1, 
                    (_, __) =>
                    {
                        onException();
                    }),
                 Policy.Handle(exceptionPredicate).RetryAsync(
                    1,
                    async (_, __) => {
                        await onExceptionAsync().ConfigureAwait(false);
                    }));
        }

        public Task<TResult> ExcecuteAsync<TResult>(Func<Task<TResult>> action)
        {
            return _asyncRetryPolicy.ExecuteAsync(action);
        }

        public Task ExcecuteAsync(Func<Task> action)
        {
            return _asyncRetryPolicy.ExecuteAsync(action);
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
