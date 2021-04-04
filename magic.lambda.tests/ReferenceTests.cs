/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2021, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using Xunit;
using magic.node;
using magic.node.extensions;
using System;

namespace magic.lambda.tests
{
    public class ReferenceTests
    {
        [Fact]
        public void ReferenceCheck()
        {
            var lambda = Common.Evaluate(@".foo
   bar
reference:x:-");
            Assert.True(lambda.Children.Skip(1).First().Value is Node);
            Assert.Equal(".foo", lambda.Children.Skip(1).First().GetEx<Node>().Name);
            Assert.Equal("bar", lambda.Children.Skip(1).First().GetEx<Node>().Children.First().Name);
        }

        [Fact]
        public void ReferenceThrows_01()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@".foo
   bar
reference"));
        }

        [Fact]
        public void ReferenceThrows_02()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@".foo
   bar1
   bar2
reference:x:-/*"));
        }
    }
}
