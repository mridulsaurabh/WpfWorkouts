using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IWorkAsync
    {
        void RunAsync(Action longRunningProcessFunction, Action longRunningProcessCompleted, Action<Exception> handleError = null);
        void RunAsync<T>(Func<T> longRunningProcessFunction, Action<T> longRunningProcessCompleted, Action<Exception> handleError = null);
        void RunAsync(Func<Task> longRunningProcessFunction, Action longRunningProcessCompleted, Action<Exception> handleError = null);
        void RunAsync<T>(Func<Task<T>> longRunningProcessFunction, Action<T> longRunningProcessCompleted, Action<Exception> handleError = null);

        void RunAsync(Action longRunningProcessFunction, CancellationTokenSource cancellationTokenSource, Action longRunningProcessCompleted, Action<Exception> handleError = null);

    }
}
