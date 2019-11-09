/*
 * Magic, Copyright(c) Thomas Hansen 2019, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
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
      remove-node:x:../*/.src/0");
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
        public void While_04_InfiniteLoop()
        {
            Assert.Throws<ApplicationException>(() => Common.Evaluate(@"
while
   .:bool:true
   .lambda"));
        }

        [Fact]
        public void While_05_InfiniteLoopStopsTooLate()
        {
            Assert.Throws<ApplicationException>(() => Common.Evaluate(@"
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
