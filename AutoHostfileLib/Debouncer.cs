using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AutoHostfileLib
{
    public delegate void DebounceFiredEventHandler();

    public class Debouncer
    {
        /// <summary>
        /// This event handler will fire when the debounce period expires
        /// </summary>
        public event DebounceFiredEventHandler DebounceFiredEvent;

        private Stopwatch debounceElapsedTime = new Stopwatch();
        private Task UpdateTask = null;

        private int DebouncePeriod;

        /// <summary>
        /// Creates the debouncer and sets the period which the debouncer will wait before triggering
        /// </summary>
        /// <param name="debouncePeriod">How long events must have ceased, before the DebounceFireEvent handler is called</param>
        public Debouncer(int debouncePeriod)
        {
            DebouncePeriod = debouncePeriod;
        }

        /// <summary>
        /// Indicates that an event of some kind has occured, resetting the debouncer
        /// </summary>
        public void Trigger()
        {
            lock (this)
            {
                debounceElapsedTime.Reset();
                debounceElapsedTime.Start();

                if (UpdateTask == null)
                {
                    UpdateTask = Task.Run(() =>
                    {
                        var elapsed = debounceElapsedTime.ElapsedMilliseconds;
                        while (elapsed < DebouncePeriod)
                        {
                            Thread.Sleep((int)(DebouncePeriod - elapsed));
                            elapsed = debounceElapsedTime.ElapsedMilliseconds;
                        }

                        DebounceFiredEvent.Invoke();

                        lock (this)
                        {
                            UpdateTask = null;
                        }
                    });
                }
            }
        }
    }
}
