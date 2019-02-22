using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class WorkAsync : IWorkAsync
    {       
        public async void RunAsync(Action longRunningProcessFunction, Action longRunningProcessCompleted, Action<Exception> handleError = null)
        {
            try
            {
                await Task.Factory.StartNew(longRunningProcessFunction);
                longRunningProcessCompleted();
            }
            catch (Exception ex)
            {
                Exception baseException = ex.GetBaseException();
                if (handleError == null)
                {
                    throw new Exception(baseException.Message, baseException);
                }
                else
                {
                    handleError(baseException);
                }
            }
        }

        public async void RunAsync<T>(Func<T> longRunningProcessFunction, Action<T> longRunningProcessCompleted, Action<Exception> handleError = null)
        {
            try
            {
                T result = await Task.Factory.StartNew<T>(longRunningProcessFunction);
                longRunningProcessCompleted(result);
            }
            catch (Exception ex)
            {
                Exception baseException = ex.GetBaseException();
                if (handleError == null)
                {
                    throw new Exception(baseException.Message, baseException);
                }
                else
                {
                    handleError(baseException);
                }
            }
        }

        public async void RunAsync(Func<Task> longRunningProcessFunction, Action longRunningProcessCompleted, Action<Exception> handleError = null)
        {
            try
            {
                await longRunningProcessFunction();
                longRunningProcessCompleted();
            }
            catch (Exception ex)
            {
                Exception baseException = ex.GetBaseException();
                if (handleError == null)
                {
                    throw new Exception(baseException.Message, baseException);
                }
                else
                {
                    handleError(baseException);
                }
            }
        }

        public async void RunAsync<T>(Func<Task<T>> longRunningProcessFunction, Action<T> longRunningProcessCompleted, Action<Exception> handleError = null)
        {
            try
            {
                T result = await longRunningProcessFunction();
                longRunningProcessCompleted(result);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                if (handleError == null)
                {
                    throw new Exception(baseException.Message, baseException);
                }
                else
                {
                    handleError(baseException);
                }
            }
        }

        public async void RunAsync(Action longRunningProcessFunction, CancellationTokenSource cancellationTokenSource, Action longRunningProcessCompleted, Action<Exception> handleError = null)
        {
            try
            {
                if (cancellationTokenSource.Token.IsCancellationRequested)
                {
                    // request cancellation before we started ?
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                }
                await Task.Factory.StartNew(longRunningProcessFunction);
                longRunningProcessCompleted();
            }
            catch (OperationCanceledException ex)
            {
                Exception baseException = ex.GetBaseException();
                if (handleError == null)
                {
                    throw new Exception(baseException.Message, baseException);
                }
                else
                {
                    handleError(baseException);
                }
            }
            catch (Exception ex)
            {
                Exception baseException = ex.GetBaseException();
                if (handleError == null)
                {
                    throw new Exception(baseException.Message, baseException);
                }
                else
                {
                    handleError(baseException);
                }
            }
            finally
            {
                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }
    }

}
