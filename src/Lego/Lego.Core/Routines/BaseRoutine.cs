using Lego.Core.Models.Devices;
using System;
using System.Threading.Tasks;

namespace Lego.Core
{
    public abstract class BaseRoutine<T> where T : IDevice
    {
        public abstract Func<T, bool> StartCondition { get; }
        public abstract Func<T, bool> StopCondition { get; }

        public virtual async Task Run(T device)
        {
            while(!StartCondition.Invoke(device))
            {
                await Task.Delay(100);
            }

            Routine(device);

            while (!StopCondition.Invoke(device))
            {
                await Task.Delay(100);
            }
        }

        public abstract void Routine(T device);
    }
}
