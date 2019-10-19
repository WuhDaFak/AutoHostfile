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
