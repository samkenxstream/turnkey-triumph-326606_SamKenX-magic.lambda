/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace magic.lambda.tests
{
    public class AsyncTests
    {
        [Fact]
        public async Task AddChildrenSrc()
        {
            var lambda = await Common.EvaluateAsync(@"
.dest
wait.add:x:../*/.dest
   .
      foo1:bar1
      foo2:bar2");
            Assert.Equal(2, lambda.Children.First().Children.Count());
            Assert.Equal("foo1", lambda.Children.First().Children.First().Name);
            Assert.Equal("bar1", lambda.Children.First().Children.First().Value);
            Assert.Equal("foo2", lambda.Children.First().Children.Skip(1).First().Name);
            Assert.Equal("bar2", lambda.Children.First().Children.Skip(1).First().Value);
        }
    }
}
