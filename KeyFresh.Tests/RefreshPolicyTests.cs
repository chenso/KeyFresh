using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KeyFresh.UnitTests
{
    public class RefreshPolicyTests
    {
        const string message = "message";

        private class Switch {
            bool on = false;

            public bool ThrowIfNotOn()
            {
                if (!on)
                {
                    throw new Exception(message);
                }
                return true;
            }

            public void ThrowIfNotOnVoid()
            {
                if (!on)
                {
                    throw new Exception(message);
                }
            }

            public Task<bool> ThrowIfNotOnAsync()
            {
                if (!on)
                {
                    throw new Exception(message);
                }
                return Task.FromResult(true);
            }

            public bool ThrowArgumentNullIfNotOn()
            {
                if (!on)
                {
                    throw new ArgumentNullException(message);
                }
                return true;
            }


            public Task<bool> ThrowArgumentNullIfNotOnAsync()
            {
                if (!on)
                {
                    throw new ArgumentNullException(message);
                }
                return Task.FromResult(true);
            }

            public Task ThrowArgumentExceptionIfNotOnAsync()
            {
                if (!on)
                {
                    throw new ArgumentException(message);
                }
                return Task.CompletedTask;
            }

            public void SwitchOn()
            {
                on = true;
            }

            public Task SwitchOnAsync()
            {
                on = true;
                return Task.CompletedTask;
            }
        }

        [Fact]
        public void RefreshPolicy_HandleException_SynchronousFunc_RetrySuccess()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleException<Exception>(x => x.Message == message, swt.SwitchOn);
            var success = policy.Excecute(swt.ThrowIfNotOn);
            Assert.True(success);
        }

        [Fact]
        public void RefreshPolicy_AsyncHandleException_SynchronousFunc_RetrySuccess()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleExceptionAsync<Exception>(swt.SwitchOnAsync);
            var success = policy.Excecute(swt.ThrowIfNotOn);
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
            var success = await policy.ExcecuteAsync(swt.ThrowIfNotOnAsync).ConfigureAwait(false);
            Assert.True(success);
        }

        [Fact]
        public async void RefreshPolicy_AsyncHandleExceptionWithPredicate_AsynchronousFunc_RetrySuccess()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleExceptionAsync<Exception>(x => x.Message == message, swt.SwitchOnAsync);
            var success = await policy.ExcecuteAsync(swt.ThrowIfNotOnAsync).ConfigureAwait(false);
            Assert.True(success);
        }

        [Fact]
        public async void RefreshPolicy_AsyncHandleExceptionWithPredicate_AsynchronousAction_RetrySuccess()
        {
            var swt = new Switch();
            RefreshPolicy policy = RefreshPolicy.HandleExceptionAsync<Exception>(x => x.Message == message, swt.SwitchOnAsync);
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
