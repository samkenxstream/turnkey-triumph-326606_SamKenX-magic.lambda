/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using Xunit;

namespace magic.lambda.tests
{
    public class InsertTests
    {
        [Fact]
        public void InsertAfter_01()
        {
            var lambda = Common.Evaluate(@"
.dest
   dest
insert-after:x:-/*
   .
      .foo1
      .foo2
");
            Assert.Equal(".foo1", lambda.Children.First().Children.Skip(1).First().Name);
            Assert.Equal(".foo2", lambda.Children.First().Children.Skip(2).First().Name);
        }

        [Fact]
        public void InsertBefore_01()
        {
            var lambda = Common.Evaluate(@"
.dest
   dest
insert-before:x:-/*
   .
      .foo1
      .foo2
");
            Assert.Equal(".foo1", lambda.Children.First().Children.First().Name);
            Assert.Equal(".foo2", lambda.Children.First().Children.Skip(1).First().Name);
        }
    }
}
