/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using Xunit;

namespace magic.lambda.tests
{
    public class BranchingTests
    {
        [Fact]
        public void If_01()
        {
            var lambda = Common.Evaluate(".result\nif\n   .:bool:true\n   .lambda\n      set-value:x:../*/.result\n         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void If_02()
        {
            var lambda = Common.Evaluate(".result\nif\n   and\n      .:bool:true\n      .:bool:true\n   .lambda\n      set-value:x:../*/.result\n         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void If_03()
        {
            var lambda = Common.Evaluate(".result\nif\n   and\n      .:bool:true\n      .:bool:false\n   .lambda\n      set-value:x:../*/.result\n         .:FAILURE");
            Assert.Null(lambda.Children.First().Value);
        }

        [Fact]
        public void Else_01()
        {
            var lambda = Common.Evaluate(@"
.result
if
   .:bool:false
   .lambda
      set-value:x:../*/.result
         .:failure
else
   set-value:x:../*/.result
      .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void ElseIf_01()
        {
            var lambda = Common.Evaluate(@"
.result
if
   .:bool:false
   .lambda
      set-value:x:../*/.result
         .:failure
else-if
   eq
      get-name:x:../*/.result
      .:.result
   .lambda
      set-value:x:../*/.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }
    }

        [Fact]
        public void ElseIf_02()
        {
            var lambda = Common.Evaluate(@"
.src:int:1
.dest
if
   eq
      get-value:x:@.src
      .:int:1
   .lambda
      set-value:x:@.dest
         .:OK
else-if
   eq
      get-value:x:@.src
      .:int:2
   .lambda
      set-value:x:@.dest
         .:ERROR
else
   set-value:x:@.dest
      .:ERROR");
            Assert.Equal("OK", lambda.Children.Skip(1).First().Value);
        }
    }
}
