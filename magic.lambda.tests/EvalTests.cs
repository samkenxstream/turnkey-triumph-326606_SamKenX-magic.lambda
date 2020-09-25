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
    }
}
