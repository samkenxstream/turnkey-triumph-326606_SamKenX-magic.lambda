/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using Xunit;
using magic.node.extensions;

namespace magic.lambda.tests
{
    public class ExistsTests
    {
        [Fact]
        public void ExistsTrue()
        {
            var lambda = Common.Evaluate(@".dest
   foo
exists:x:-/*");
            Assert.True(lambda.Children.Skip(1).First().GetEx<bool>());
        }

        [Fact]
        public void ExistsFalse()
        {
            var lambda = Common.Evaluate(@".dest
exists:x:-/*");
            Assert.False(lambda.Children.Skip(1).First().GetEx<bool>());
        }
    }
}
