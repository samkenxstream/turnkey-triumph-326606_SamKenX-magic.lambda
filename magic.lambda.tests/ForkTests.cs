/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using Xunit;
using magic.node;
using magic.signals.contracts;
using System.Threading.Tasks;
using System;

namespace magic.lambda.tests
{
    public class ForkTests
    {
        [Slot(Name = "fork-slot-1")]
        public class ForkSlot1 : ISlot
        {
            public static int ExecutionCount;

            public void Signal(ISignaler signaler, Node input)
            {
                Assert.Equal(0, ExecutionCount);
            }
        }

        [Slot(Name = "fork-slot-2")]
        public class ForkSlot2 : ISlot
        {
            public void Signal(ISignaler signaler, Node input)
            {
                ForkSlot1.ExecutionCount += 1;
            }
        }

        [Fact]
        public void ForkWithSleep()
        {
            ForkSlot1.ExecutionCount = 0;
            var lambda = Common.Evaluate(@"
fork
   fork-slot-1
sleep:100
fork-slot-2
");
        }

        [Fact]
        public void Semaphore_01()
        {
            ForkSlot1.ExecutionCount = 0;
            var lambda = Common.Evaluate(@"
fork
   semaphore:foo
      fork-slot-2
semaphore:foo
   fork-slot-2
");
            Assert.Equal(2, ForkSlot1.ExecutionCount);
        }

        [Fact]
        public void Semaphore_Throws()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
semaphore
"));
        }

        [Fact]
        public async Task Semaphore_01Async()
        {
            ForkSlot1.ExecutionCount = 0;
            var lambda = await Common.EvaluateAsync(@"
wait.fork
   wait.semaphore:foo2
      fork-slot-2
wait.semaphore:foo2
   fork-slot-2
");
            Assert.Equal(2, ForkSlot1.ExecutionCount);
        }

        [Fact]
        public async Task ForkWithSleepAsync()
        {
            ForkSlot1.ExecutionCount = 0;
            var lambda = await Common.EvaluateAsync(@"
wait.fork
   fork-slot-1
wait.sleep:100
fork-slot-2
");
        }
    }
}
