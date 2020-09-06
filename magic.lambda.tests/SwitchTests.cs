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
    public class SwitchTests
    {
        [Fact]
        public void SwitchSimple()
        {
            var lambda = Common.Evaluate(@".result
.foo:bar
switch:x:@.foo
   case:bar
      set-value:x:@.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void SwitchFallthrough()
        {
            var lambda = Common.Evaluate(@".result
.foo:bar
switch:x:@.foo
   case:foo
   case:bar
      set-value:x:@.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void SwitchThrow_01()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@".result
.foo:bar
switch:x:@.foo
   caseX:foo
   case:bar
      set-value:x:@.result
         .:OK"));
        }

        [Fact]
        public void SwitchThrow_02()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@".result
.foo:bar
switch:x:@.foo"));
        }

        [Fact]
        public void SwitchThrow_03()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@".result
.foo:bar
switch:x:@.foo
   case
      set-value:x:@.result
         .:OK"));
        }

        [Fact]
        public void SwitchThrow_04()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@".result
.foo:bar
switch:x:@.foo
   case:xxx
   default:howdy
      set-value:x:@.result
         .:OK"));
        }

        [Fact]
        public void CaseThrows()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.result
.foo:bar
case:bar
   set-value:x:@.result
      .:OK"));
        }

        [Fact]
        public async Task SwitchSimpleAsync()
        {
            var lambda = await Common.EvaluateAsync(@".result
.foo:bar
wait.switch:x:@.foo
   wait.case:bar
      set-value:x:@.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void SwitchDefault()
        {
            var lambda = Common.Evaluate(@".result
.foo:barXX
switch:x:@.foo
   case:bar
      .do-nothing
   default
      set-value:x:@.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void DefaultThrows()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
default
   set-value:x:@.result
      .:OK"));
        }

        [Fact]
        public async Task SwitchDefaultAsync()
        {
            var lambda = await Common.EvaluateAsync(@".result
.foo:barXX
wait.switch:x:@.foo
   case:bar
      .do-nothing
   wait.default
      set-value:x:@.result
         .:OK");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void SwitchComplex()
        {
            var lambda = Common.Evaluate(@".result
.foo:bar2
switch:x:@.foo
   case:bar1
      set-value:x:@.result
         .:ERROR
   case:bar2
      set-value:x:@.result
         .:OK
   default
      set-value:x:@.result
         .:ERROR");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void NothingDone()
        {
            var lambda = Common.Evaluate(@".result
.foo:barXX
switch:x:@.foo
   case:bar1
      set-value:x:@.result
         .:ERROR
   case:bar2
      set-value:x:@.result
         .:ERROR");
            Assert.Null(lambda.Children.First().Value);
        }
    }
}
