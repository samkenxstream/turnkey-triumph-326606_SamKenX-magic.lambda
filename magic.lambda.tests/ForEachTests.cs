/*
 * Aista Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using Xunit;
using magic.node;
using magic.signals.contracts;

namespace magic.lambda.tests
{
    public class ForEachTests
    {
        [Slot(Name = "foo2")]
        public class Foo2Slot : ISlot
        {
            public static int ExecutionCount;

            public void Signal(ISignaler signaler, Node input)
            {
                ++ExecutionCount;
            }
        }

        [Fact]
        public void ForEach_01()
        {
            Foo2Slot.ExecutionCount = 0;
            Common.Evaluate(@".foo1
   bar1
   bar2
   bar3
for-each:x:../*/.foo1/*
   foo2");
            Assert.Equal(3, Foo2Slot.ExecutionCount);
        }

        [Fact]
        public void ForEach_02()
        {
            Foo2Slot.ExecutionCount = 0;
            Common.Evaluate(@".foo1
   bar1
   bar2
   bar3
for-each:x:../*/.foo1/*
   foo2
   return:done");
            Assert.Equal(1, Foo2Slot.ExecutionCount);
        }

        [Fact]
        public void ForEach_03()
        {
            Foo2Slot.ExecutionCount = 0;
            Common.Evaluate(@".foo1
   bar1
   bar2
   bar3
for-each:x:../*/.foo1/*
   foo2
   return
      result:done");
            Assert.Equal(1, Foo2Slot.ExecutionCount);
        }

        [Fact]
        public async Task ForEachAsync_01()
        {
            Foo2Slot.ExecutionCount = 0;
            await Common.EvaluateAsync(@".foo1
   bar1
   bar2
   bar3
for-each:x:../*/.foo1/*
   foo2");
            Assert.Equal(3, Foo2Slot.ExecutionCount);
        }

        [Fact]
        public async Task ForEachAsync_02()
        {
            Foo2Slot.ExecutionCount = 0;
            await Common.EvaluateAsync(@".foo1
   bar1
   bar2
   bar3
for-each:x:../*/.foo1/*
   foo2
   return:done");
            Assert.Equal(1, Foo2Slot.ExecutionCount);
        }

        [Fact]
        public async Task ForEachAsync_03()
        {
            Foo2Slot.ExecutionCount = 0;
            await Common.EvaluateAsync(@".foo1
   bar1
   bar2
   bar3
for-each:x:../*/.foo1/*
   foo2
   return
      result:done");
            Assert.Equal(1, Foo2Slot.ExecutionCount);
        }
    }
}
