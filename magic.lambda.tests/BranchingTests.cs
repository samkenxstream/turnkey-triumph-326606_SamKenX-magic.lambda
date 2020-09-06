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
    public class BranchingTests
    {
        [Fact]
        public void If_01()
        {
            var lambda = Common.Evaluate(@"
.result
if
   .:bool:true
   .lambda
      set-value:x:../*/.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void If_Throws()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
if
   .:bool:true
   .:bool:true
   .lambda
      set-value:x:../*/.result
         .:OK"));
        }

        [Fact]
        public void Or_Throws_01()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
or
   .lambda
      set-value:x:../*/.result
         .:OK"));
        }

        [Fact]
        public void If_Throws_02()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
if
   .:bool:true
   .lambdaXX
      set-value:x:../*/.result
         .:OK"));
        }

        [Fact]
        public void IfWithOr()
        {
            var lambda = Common.Evaluate(@"
.result
if
   or
      .:bool:false
      .:bool:true
   .lambda
      set-value:x:../*/.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void IfWithOrSlot()
        {
            var lambda = Common.Evaluate(@"
.result
.value:bool:true
if
   or
      .:bool:false
      get-value:x:@.value
   .lambda
      set-value:x:../*/.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void IfWithOrSlotYieldingFalse()
        {
            var lambda = Common.Evaluate(@"
.result:OK
if
   or
      .:bool:false
      .:bool:false
   .lambda
      set-value:x:../*/.result
         .:error");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public async Task IfWithOrSlotYieldingFalseAsync_01()
        {
            var lambda = await Common.EvaluateAsync(@"
.result:OK
.false:bool:false
wait.if
   wait.or
      .:bool:false
      .:bool:false
      get-value:x:@.false
   .lambda
      set-value:x:../*/.result
         .:error");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public async Task IfWithOrSlotYieldingFalseAsync_02()
        {
            var lambda = await Common.EvaluateAsync(@"
.result:OK
wait.if
   wait.or
      .:bool:false
      .:bool:false
   .lambda
      set-value:x:../*/.result
         .:error");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public async Task IfWithOrAsync()
        {
            var lambda = await Common.EvaluateAsync(@"
.result
wait.if
   wait.or
      .:bool:false
      .:bool:true
   .lambda
      set-value:x:../*/.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public async Task If_01Async()
        {
            var lambda = await Common.EvaluateAsync(@"
.result
wait.if
   .:bool:true
   .lambda
      set-value:x:../*/.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void If_02()
        {
            var lambda = Common.Evaluate(@"
.result
if
   and
      .:bool:true
      .:bool:true
   .lambda
      set-value:x:../*/.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public async Task If_02Async()
        {
            var lambda = await Common.EvaluateAsync(@"
.result
wait.if
   wait.and
      .:bool:true
      .:bool:true
   .lambda
      set-value:x:../*/.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void IfThrows()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
if
   and
      .:bool:true
   .lambda
      set-value:x:../*/.result
         .:OK"));
        }

        [Fact]
        public void If_03()
        {
            var lambda = Common.Evaluate(@"
.result
if
   and
      .:bool:true
      .:bool:false
   .lambda
      set-value:x:../*/.result
         .:FAILURE");
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
        public void ElseThrows_01()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
else
   set-value:x:../*/.result
      .:OK"));
        }

        [Fact]
        public void ElseThrows_02()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate("else"));
        }

        [Fact]
        public async Task Else_01Async()
        {
            var lambda = await Common.EvaluateAsync(@"
.result
if
   .:bool:false
   .lambda
      set-value:x:../*/.result
         .:failure
wait.else
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

        [Fact]
        public void ElseIf_Throws_01()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
if
   .:bool:false
   .lambda
      set-value:x:../*/.result
         .:failure
else-if
   .:throws!
   eq
      get-name:x:../*/.result
      .:.result
   .lambda
      set-value:x:../*/.result
         .:OK"));
        }

        [Fact]
        public void ElseIf_Throws_02()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
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
   .lambdaXX
      set-value:x:../*/.result
         .:OK"));
        }

        [Fact]
        public void ElseIf_Throws_03()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
else-if
   eq
      get-name:x:../*/.result
      .:.result
   .lambdaXX
      set-value:x:../*/.result
         .:OK"));
        }

        [Fact]
        public async Task ElseIf_01Async()
        {
            var lambda = await Common.EvaluateAsync(@"
.result
if
   .:bool:false
   .lambda
      set-value:x:../*/.result
         .:failure
wait.else-if
   eq
      get-name:x:../*/.result
      .:.result
   .lambda
      set-value:x:../*/.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
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
