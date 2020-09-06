/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using Xunit;
using magic.node.extensions;
using magic.lambda.exceptions;

namespace magic.lambda.tests
{
    public class ExceptionsTests
    {
        [Fact]
        public void Throws_01()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
try
   throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
");
            Assert.Equal(true, lambda.Children.First().Value);
        }

        [Fact]
        public void Throws_02()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
try
   throw:foo
.catch
.finally
   set-value:x:@.throws
      .:bool:true
");
            Assert.Equal(true, lambda.Children.First().Value);
        }

        [Fact]
        public void Throws_03Throws()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
.status
.public
.message
.type
try
   throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
   set-value:x:@.status
      get-value:x:@.arguments/*/status
   set-value:x:@.public
      get-value:x:@.arguments/*/public
   set-value:x:@.message
      get-value:x:@.arguments/*/message
   set-value:x:@.type
      get-value:x:@.arguments/*/type
");
            Assert.Equal(500, lambda.Children.Skip(1).First().Value);
            Assert.Equal(false, lambda.Children.Skip(2).First().Value);
            Assert.Equal("foo", lambda.Children.Skip(3).First().Value);
            Assert.Equal("magic.lambda.exceptions.HyperlambdaException", lambda.Children.Skip(4).First().Value);
        }

        [Fact]
        public async Task Throws_03ThrowsAsync()
        {
            var lambda = await Common.EvaluateAsync(@"
.throws:bool:false
.status
.public
.message
.type
wait.try
   throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
   set-value:x:@.status
      get-value:x:@.arguments/*/status
   set-value:x:@.public
      get-value:x:@.arguments/*/public
   set-value:x:@.message
      get-value:x:@.arguments/*/message
   set-value:x:@.type
      get-value:x:@.arguments/*/type
");
            Assert.Equal(500, lambda.Children.Skip(1).First().Value);
            Assert.Equal(false, lambda.Children.Skip(2).First().Value);
            Assert.Equal("foo", lambda.Children.Skip(3).First().Value);
            Assert.Equal("magic.lambda.exceptions.HyperlambdaException", lambda.Children.Skip(4).First().Value);
        }

        [Fact]
        public void Throws_04()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
try
   throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
");
            Assert.Equal(true, lambda.Children.First().Value);
        }

        [Fact]
        public void Throws_05()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
.status
.public
.message
.type
try
   throw
      public:true
      status:123
.catch
   set-value:x:@.throws
      .:bool:true
   set-value:x:@.status
      get-value:x:@.arguments/*/status
   set-value:x:@.public
      get-value:x:@.arguments/*/public
   set-value:x:@.message
      get-value:x:@.arguments/*/message
   set-value:x:@.type
      get-value:x:@.arguments/*/type
");
            Assert.Equal(123, lambda.Children.Skip(1).First().Value);
            Assert.Equal(true, lambda.Children.Skip(2).First().Value);
            Assert.Equal("[no-message]", lambda.Children.Skip(3).First().Value);
            Assert.Equal("magic.lambda.exceptions.HyperlambdaException", lambda.Children.Skip(4).First().Value);
        }

        [Fact]
        public async Task Throws_01Async()
        {
            var lambda = await Common.EvaluateAsync(@"
.throws:bool:false
wait.try
   throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
");
            Assert.Equal(true, lambda.Children.First().Value);
        }
    }
}
