/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using System.Linq;
using Xunit;

namespace magic.lambda.tests
{
    public class UnwrapTests
    {
        [Fact]
        public void UnwrapValue()
        {
            var lambda = Common.Evaluate(@".src
   foo1:bar1
   foo2:bar2
.dest
   dest1:x:../*/.src/0
   dest2:x:../*/.src/1
unwrap:x:../*/.dest/*");
            Assert.Equal("dest1", lambda.Children.Skip(1).First().Children.First().Name);
            Assert.Equal("bar1", lambda.Children.Skip(1).First().Children.First().Value);
            Assert.Equal("dest2", lambda.Children.Skip(1).First().Children.Skip(1).First().Name);
            Assert.Equal("bar2", lambda.Children.Skip(1).First().Children.Skip(1).First().Value);
        }

        [Fact]
        public void UnwrapToNull()
        {
            var lambda = Common.Evaluate(@"
.dest
   dest1:x:../*/.src/0
   dest2:x:../*/.src/1
unwrap:x:../*/.dest/*");
            Assert.Null(lambda.Children.First().Children.First().Value);
            Assert.Null(lambda.Children.First().Children.Skip(1).First().Value);
        }

        [Fact]
        public void UnwrapValue_Throws()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@".src
   foo1:bar1
   foo2:bar2
.dest
   dest1:x:../*/.src/*
   dest2:x:../*/.src/*
unwrap:x:../*/.dest/*"));
        }
    }
}
