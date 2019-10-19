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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoHostfileLib;
using System.Threading;

namespace AutoHostfileTests
{
    [TestClass]
    public class DebouncerTests
    {
        [TestMethod]
        public void Debouncer_Trigger_FiredOnce_DebounceOccursOnce()
        {
            var debouncer = new Debouncer(0);
            var signal = new AutoResetEvent(false);
            bool result = false;
            debouncer.DebounceFiredEvent += () =>
            {
                result = true;
                signal.Set();
            };
            debouncer.Trigger();

            signal.WaitOne();

            // The debounce event should have fired
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Debouncer_Trigger_Fired100_DebounceOccurs()
        {
            var debouncer = new Debouncer(100);
            var signal = new AutoResetEvent(false);
            int result = 0;
            debouncer.DebounceFiredEvent += () =>
            {
                result++;
                signal.Set();
            };

            for (int i = 0; i < 100; i++)
            {
                debouncer.Trigger();
            }

            signal.WaitOne();

            // Debounce should fire just once despite 100 events
            Assert.AreEqual(result, 1);
        }
    }
}
