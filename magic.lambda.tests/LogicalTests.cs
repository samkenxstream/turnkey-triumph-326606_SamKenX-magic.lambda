/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using Xunit;
using magic.node.extensions;

namespace magic.lambda.tests
{
    public class LogicalTests
    {
        [Fact]
        public void And_01()
        {
            var lambda = Common.Evaluate(@"
.foo1:bool:true
and
   get-value:x:../*/.foo1
   .:bool:true");
            Assert.Equal(true, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void And_02()
        {
            var lambda = Common.Evaluate(@"
.foo1:bool:true
and
   get-value:x:../*/.foo1
   .:bool:true
   .:bool:false");
            Assert.Equal(false, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void AndWhitelist_Throws()
        {
            Assert.Throws<HyperlambdaException>(() => Common.Evaluate(@"
.foo1:bool:true
whitelist
   vocabulary
      set-value
   .lambda
      and
         get-value:x:../*/.foo1
         .:bool:true"));
        }

        [Fact]
        public async Task AndWhitelistAsync_Throws()
        {
            await Assert.ThrowsAsync<HyperlambdaException>(async () => await Common.EvaluateAsync(@"
.foo1:bool:true
whitelist
   vocabulary
      set-value
   .lambda
      and
         get-value:x:../*/.foo1
         .:bool:true"));
        }

        [Fact]
        public void Or_01()
        {
            var lambda = Common.Evaluate(@"
.foo1:bool:true
or
   get-value:x:../*/.foo1
   .:bool:false");
            Assert.Equal(true, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void Or_02()
        {
            var lambda = Common.Evaluate(@"
.foo1:bool:false
or
   get-value:x:../*/.foo1
   .:bool:true
   .:bool:false");
            Assert.Equal(true, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void Or_03()
        {
            var lambda = Common.Evaluate(@"
.foo1:bool:false
or
   get-value:x:../*/.foo1
   .:bool:false
   .:bool:false");
            Assert.Equal(false, lambda.Children.Skip(1).First().Value);
        }
    }
}
