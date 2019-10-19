//  Copyright (C) 2019 Ben Staniford
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
