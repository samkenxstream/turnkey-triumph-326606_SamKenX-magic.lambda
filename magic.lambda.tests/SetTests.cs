/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using Xunit;
using magic.node.extensions;
using magic.node.expressions;
using System.Threading.Tasks;
using System;

namespace magic.lambda.tests
{
    public class SetTests
    {
        [Fact]
        public void SetWithNull()
        {
            var lambda = Common.Evaluate(@"
.foo1
   foo2
remove-nodes:x:../*/.foo1/*
");
            Assert.Empty(lambda.Children.First().Children);
        }

        [Fact]
        public void SetNameWithStatic()
        {
            var lambda = Common.Evaluate(@"
.foo1
set-name:x:../*/.foo1
   .:.foo2
");
            Assert.Equal(".foo2", lambda.Children.First().Name);
        }

        [Fact]
        public void SetNameWithoutSource()
        {
            var lambda = Common.Evaluate(@"
.foo1
set-name:x:../*/.foo1
");
            Assert.Equal("", lambda.Children.First().Name);
        }

        [Fact]
        public void SetNameThrows_01()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.foo1
set-name:x:../*/.foo1
   .:.foo2
   .:.foo2
"));
        }

        [Fact]
        public async Task SetNameWithStaticAsync()
        {
            var lambda = await Common.EvaluateAsync(@"
.foo1
wait.set-name:x:../*/.foo1
   .:.foo2
");
            Assert.Equal(".foo2", lambda.Children.First().Name);
        }

        [Fact]
        public void SetNameWithExpression()
        {
            var lambda = Common.Evaluate(@"
.foo1:.bar1
set-name:x:../*/.foo1
   get-value:x:../*/.foo1
");
            Assert.Equal(".bar1", lambda.Children.First().Name);
        }

        [Fact]
        public void SetNameWithExpressionReturningNull()
        {
            var lambda = Common.Evaluate(@"
.foo1:error
.foo2
set-value:x:../*/.foo1
   get-value:x:../*/.foo2
");
            Assert.Null(lambda.Children.First().Value);
        }

        [Fact]
        public void SetNameWithExpression_Throws()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.foo1
   bar1
   bar2
set-name:x:../*/.foo1
   get-value:x:../*/.foo1/*
"));
        }

        [Fact]
        public void SetValueWithStatic()
        {
            var lambda = Common.Evaluate(@"
.foo1
set-value:x:../*/.foo1
   .:OK
");
            Assert.Equal("OK", lambda.Children.First().Value);
        }

        [Fact]
        public void SetValueWithExpression()
        {
            var lambda = Common.Evaluate(@"
.foo1:.bar1
set-value:x:../*/.foo1
   get-name:x:../*/.foo1
");
            Assert.Equal(".foo1", lambda.Children.First().Value);
        }

        [Fact]
        public void SetValueWithExpressionEvaluate()
        {
            var lambda = Common.Evaluate(@"
.foo1:.bar1
.foo2:howdy
set-value:x:../*/.foo1
   .:x:../*/.foo2");
            Assert.Equal(typeof(string), lambda.Children.First().Value.GetType());
            Assert.Equal("howdy", lambda.Children.First().Value);
        }

        [Fact]
        public void SetValueWithExpressionNotEvaluate()
        {
            var lambda = Common.Evaluate(@"
.foo1:.bar1
set-x:x:../*/.foo1
   .:x:../*/.foo1");
            Assert.Equal(typeof(Expression), lambda.Children.First().Value.GetType());
            Assert.Equal("../*/.foo1", lambda.Children.First().Get<Expression>().Value);
        }
    }
}
