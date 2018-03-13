using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KeyFresh.UnitTests
{
    public class RefreshPolicyTests
    {
        private const string Message = "message";

        private class Switch {
            private bool _on = false;

            public bool ThrowIfNotOn()
            {
                if (!_on)
                {
                    throw new Exception(Message);
                }
                return true;
            }

            public void ThrowIfNotOnVoid()
            {
                if (!_on)
                {
                    throw new Exception(Message);
                }
            }

            public Task<bool> ThrowIfNotOnAsync()
            {
                if (!_on)
                {
                    throw new Exception(Message);
                }
                return Task.FromResult(true);
            }

            public bool ThrowArgumentNullIfNotOn()
            {
                if (!_on)
                {
                    throw new ArgumentNullException(Message);
                }
                return true;
            }


            public Task<bool> ThrowArgumentNullIfNotOnAsync()
            {
                if (!_on)
                {
                    throw new ArgumentNullException(Message);
                }
                return Task.FromResult(true);
            }

            public Task ThrowArgumentExceptionIfNotOnAsync()
            {
                if (!_on)
                {
                    throw new ArgumentException(Message);
                }
                return Task.CompletedTask;
            }

            public void SwitchOn()
            {
                _on = true;
            }

            public Task SwitchOnAsync()
            {
                _on = true;
                return Task.CompletedTask;
            }
        }

        [Fact]
        public void RefreshPolicy_HandleException_SynchronousFunc_RetrySuccess()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleException<Exception>(x => x.Message == Message, swt.SwitchOn);
            bool success = policy.Excecute(swt.ThrowIfNotOn);
            Assert.True(success);
        }

        [Fact]
        public void RefreshPolicy_AsyncHandleException_SynchronousFunc_RetrySuccess()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleExceptionAsync<Exception>(swt.SwitchOnAsync);
            bool success = policy.Excecute(swt.ThrowIfNotOn);
            Assert.True(success);
        }

        [Fact]
        public void RefreshPolicy_AsyncHandleException_SynchronousAction_NoException()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleExceptionAsync<Exception>(swt.SwitchOnAsync);
            policy.Excecute(swt.ThrowIfNotOnVoid);
        }

        [Fact]
        public async void RefreshPolicy_AsyncHandleException_AsynchronousFunc_RetrySuccess()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleException<Exception>(swt.SwitchOn);
            bool success = await policy.ExcecuteAsync(swt.ThrowIfNotOnAsync).ConfigureAwait(false);
            Assert.True(success);
        }

        [Fact]
        public async void RefreshPolicy_AsyncHandleExceptionWithPredicate_AsynchronousFunc_RetrySuccess()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleExceptionAsync<Exception>(x => x.Message == Message, swt.SwitchOnAsync);
            bool success = await policy.ExcecuteAsync(swt.ThrowIfNotOnAsync).ConfigureAwait(false);
            Assert.True(success);
        }

        [Fact]
        public async void RefreshPolicy_AsyncHandleExceptionWithPredicate_AsynchronousAction_RetrySuccess()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleExceptionAsync<Exception>(x => x.Message == Message, swt.SwitchOnAsync);
            await policy.ExcecuteAsync(swt.ThrowArgumentExceptionIfNotOnAsync).ConfigureAwait(false);
        }

        [Fact]
        public void RefreshPolicy_AsyncHandleException_UncaughtException_RetryFail()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleExceptionAsync<ArgumentOutOfRangeException>(swt.SwitchOnAsync);
            Assert.ThrowsAsync<ArgumentNullException>(() => policy.ExcecuteAsync(swt.ThrowArgumentNullIfNotOnAsync));
        }
    }
}
