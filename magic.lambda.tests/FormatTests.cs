/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using Xunit;

namespace magic.lambda.tests
{
    public class FormatTests
    {
        [Fact]
        public void Format_01()
        {
            var lambda = Common.Evaluate(@"
.foo:int:57
format:x:-
   pattern:""{0:00000}""");
            Assert.Equal("00057", lambda.Children.Skip(1).First().Value);
            Assert.Empty(lambda.Children.Skip(1).First().Children);
        }
    }
}
