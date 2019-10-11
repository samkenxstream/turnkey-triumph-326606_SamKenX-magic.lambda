/*
 * Magic, Copyright(c) Thomas Hansen 2019, thomas@gaiasoul.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using Xunit;

namespace magic.lambda.tests
{
    public class ApplyTests
    {
        [Fact]
        public void Apply_01()
        {
            var lambda = Common.Evaluate(@"
.applied
   foo1:{foo1}
   foo2:{foo2}
apply:x:-
   foo1:bar1
   foo2
      bar1
      bar2");
            Assert.Equal("bar1", lambda.Children.Skip(1).First().Children.First().Value);
            Assert.Equal(2, lambda.Children.Skip(1).First().Children.Skip(1).First().Children.Count());
            Assert.Equal("bar1", lambda.Children.Skip(1).First().Children.Skip(1).First().Children.First().Name);
            Assert.Equal("bar2", lambda.Children.Skip(1).First().Children.Skip(1).First().Children.Skip(1).First().Name);
        }
    }
}
