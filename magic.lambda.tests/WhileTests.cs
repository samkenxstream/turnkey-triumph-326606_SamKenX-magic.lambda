/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace magic.lambda.tests
{
    public class WhileTests
    {
        [Fact]
        public void While_01()
        {
            var lambda = Common.Evaluate(@".src
   bar1
   bar2
.dest
while
   mt
      get-count:x:../*/.src/*
      .:int:0
   .lambda
      add:x:../*/.dest
         get-nodes:x:../*/.src/0
      remove-nodes:x:../*/.src/0");
            Assert.Equal(2, lambda.Children.Skip(1).First().Children.Count());
        }

        [Fact]
        public void While_02()
        {
            var lambda = Common.Evaluate(@".src
while
   .:bool:false
   .lambda
      set-value:x:@.src
         .:FAILURE");
            Assert.NotEqual("FAILURE", lambda.Children.First().Name);
        }

        [Fact]
        public async Task WhileAync()
        {
            var lambda = await Common.EvaluateAsync(@".src
   bar1
   bar2
.dest
wait.while
   mt
      get-count:x:../*/.src/*
      .:int:0
   .lambda
      add:x:../*/.dest
         get-nodes:x:../*/.src/0
      remove-nodes:x:../*/.src/0");
            Assert.Equal(2, lambda.Children.Skip(1).First().Children.Count());
        }

        [Fact]
        public void WhileTerminate()
        {
            var lambda = Common.Evaluate(@".src
   bar1
   bar2
.dest
while
   mt
      get-count:x:../*/.src/*
      .:int:0
   .lambda
      add:x:../*/.dest
         get-nodes:x:../*/.src/0
      remove-nodes:x:../*/.src/0
      return:done");
            Assert.Equal(1, lambda.Children.Skip(1).First().Children.Count());
        }

        [Fact]
        public async Task WhileTerminateAsync()
        {
            var lambda = await Common.EvaluateAsync(@".src
   bar1
   bar2
.dest
wait.while
   mt
      get-count:x:../*/.src/*
      .:int:0
   .lambda
      add:x:../*/.dest
         get-nodes:x:../*/.src/0
      remove-nodes:x:../*/.src/0
      return:done");
            Assert.Equal(1, lambda.Children.Skip(1).First().Children.Count());
        }

        [Fact]
        public void While_InfiniteLoop()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
while
   .:bool:true
   .lambda"));
        }

        [Fact]
        public async Task While_InfiniteLoopAsync()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await Common.EvaluateAsync(@"
wait.while
   .:bool:true
   .lambda"));
        }

        [Fact]
        public void WhileNoLambdaThrows()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
while
   .:bool:true"));
        }

        [Fact]
        public async Task WhileNoLambdaAsyncThrows()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await Common.EvaluateAsync(@"
wait.while
   .:bool:true"));
        }

        [Fact]
        public void While_InfiniteLoopStopsTooLate()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.no:int:0
while
   lt
      get-value:x:@.no
      .:int:5000
   .lambda
      math.increment:x:@.no"));
        }
    }
}
