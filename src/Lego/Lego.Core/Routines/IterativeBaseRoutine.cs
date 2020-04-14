using System;
using System.Threading.Tasks;

namespace Lego.Core
{
    public abstract class IterativeBaseRoutine<T> : BaseRoutine<T> where T : IDevice
    {
        protected int Iterations { get; set; }

        public IterativeBaseRoutine(int iterations = 1)
        {
            Iterations = Math.Max(1, iterations);
        }

        public async override Task Run(T device)
        {
            do
            {
                await base.Run(device);

                Iterations -= 1;
            }
            while (Iterations > 0);
        }
    }
}
