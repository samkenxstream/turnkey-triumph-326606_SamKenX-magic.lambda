/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using Xunit;

namespace magic.lambda.tests
{
    public class ConvertTests
    {
        [Fact]
        public void ConvertToInt()
        {
            var lambda = Common.Evaluate(@"
.src:5
convert:x:-
   type:int");
            Assert.Equal(5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToString()
        {
            var lambda = Common.Evaluate(@"
.src:int:5
convert:x:-
   type:string");
            Assert.Equal("5", lambda.Children.Skip(1).First().Value);
        }
    }
}
