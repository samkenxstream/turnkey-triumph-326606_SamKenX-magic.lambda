/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using magic.node.extensions;

namespace magic.lambda.tests
{
    public class EvalTests
    {
        [Fact]
        public void InvokeCustomSignal()
        {
            var lambda = Common.Evaluate(@"foo");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void InvokeNonExistingSignal_Throws()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"foo_XXX"));
        }

        [Fact]
        public void InvokeEval()
        {
            var lambda = Common.Evaluate(@"
.src
eval
   """"
   .
   set-value:x:@.src
      .:OK
");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void InvokeEvalExpression()
        {
            var lambda = Common.Evaluate(@"
.src
try
   .eval
      throw:foo
   eval:x:@.eval
.catch
   set-value:x:@.src
      .:OK
");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void InvokeEvalThrows()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.src
eval:x:@.src
   set-value:x:@.src
      .:OK
"));
        }

        [Fact]
        public async Task InvokeEvalAsync()
        {
            var lambda = await Common.EvaluateAsync(@"
.src
eval
   set-value:x:@.src
      .:OK
");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void EvalWhitelist()
        {
            var lambda = Common.Evaluate(@"
.result
whitelist
   vocabulary
      add
      vocabulary
   .lambda
      add:x:@.result
         vocabulary
");
            Assert.Equal(2, lambda.Children.First().Children.Count());
            var first = lambda.Children.First().Children.First().Get<string>();
            var second = lambda.Children.First().Children.Skip(1).First().Get<string>();
            Assert.True(first == "add" || first == "vocabulary");
            Assert.True(second == "add" || second == "vocabulary");
        }

        [Fact]
        public async Task EvalWhitelistAsync()
        {
            var lambda = await Common.EvaluateAsync(@"
.result
whitelist
   vocabulary
      add
      vocabulary
   .lambda
      add:x:@.result
         vocabulary
");
            Assert.Equal(2, lambda.Children.First().Children.Count());
            var first = lambda.Children.First().Children.First().Get<string>();
            var second = lambda.Children.First().Children.Skip(1).First().Get<string>();
            Assert.True(first == "add" || first == "vocabulary");
            Assert.True(second == "add" || second == "vocabulary");
        }

        [Fact]
        public void EvalWhitelist_01_Throws()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
whitelist
   vocabulary
      add
      vocabulary
   .lambda
      add:x:@.result
         vocabulary
      set-value:x:@.result
         .:foo
"));
        }

        [Fact]
        public async Task EvalWhitelistAsync_01_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await Common.EvaluateAsync(@"
.result
whitelist
   vocabulary
      add
      vocabulary
   .lambda
      add:x:@.result
         vocabulary
      set-value:x:@.result
         .:foo
"));
        }

        [Fact]
        public void EvalWhitelist_02_Throws()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
whitelist
   .lambda
      add:x:@.result
         vocabulary
      set-value:x:@.result
         .:foo
"));
        }

        [Fact]
        public async Task EvalWhitelistAsync_02_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await Common.EvaluateAsync(@"
.result
whitelist
   .lambda
      add:x:@.result
         vocabulary
      set-value:x:@.result
         .:foo
"));
        }

        [Fact]
        public void EvalWhitelist_03_Throws()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
whitelist
   vocabulary
      add
      vocabulary
"));
        }

        [Fact]
        public async Task EvalWhitelistAsync_03_Throws()
        {
            await Assert.ThrowsAsync<ArgumentException>(async () => await Common.EvaluateAsync(@"
.result
whitelist
   vocabulary
      add
      vocabulary
"));
        }
    }
}
