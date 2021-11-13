/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Linq;
using Xunit;

namespace magic.lambda.tests
{
    public class LogicalTests
    {
        [Fact]
        public void And_01()
        {
            var lambda = Common.Evaluate(".foo1:bool:true\nand\n   get-value:x:../*/.foo1\n   .:bool:true");
            Assert.Equal(true, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void And_02()
        {
            var lambda = Common.Evaluate(".foo1:bool:true\nand\n   get-value:x:../*/.foo1\n   .:bool:true\n   .:bool:false");
            Assert.Equal(false, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void Or_01()
        {
            var lambda = Common.Evaluate(".foo1:bool:true\nor\n   get-value:x:../*/.foo1\n   .:bool:false");
            Assert.Equal(true, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void Or_02()
        {
            var lambda = Common.Evaluate(".foo1:bool:false\nor\n   get-value:x:../*/.foo1\n   .:bool:true\n   .:bool:false");
            Assert.Equal(true, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void Or_03()
        {
            var lambda = Common.Evaluate(".foo1:bool:false\nor\n   get-value:x:../*/.foo1\n   .:bool:false\n   .:bool:false");
            Assert.Equal(false, lambda.Children.Skip(1).First().Value);
        }
    }
}
