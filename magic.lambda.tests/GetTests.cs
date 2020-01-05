/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using Xunit;

namespace magic.lambda.tests
{
    public class GetTests
    {
        [Fact]
        public void Value()
        {
            var lambda = Common.Evaluate(".src:foo1\nget-value:x:../*/.src");
            Assert.Equal("foo1", lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void Name()
        {
            var lambda = Common.Evaluate(".foo1\nget-name:x:../*/.foo1");
            Assert.Equal(".foo1", lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void Count()
        {
            var lambda = Common.Evaluate(".foo1\n   bar1\n   bar2\nget-count:x:../*/.foo1/*");
            Assert.Equal(2, lambda.Children.Skip(1).First().Value);
        }
    }
}
