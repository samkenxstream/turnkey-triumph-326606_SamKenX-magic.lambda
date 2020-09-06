/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using Xunit;
using magic.node.extensions;

namespace magic.lambda.tests
{
    public class VocabularyTests
    {
        [Fact]
        public void VocabularyWithoutFilter()
        {
            var lambda = Common.Evaluate(@"vocabulary");
            Assert.True(lambda.Children.First().Children.Count() > 20);
        }

        [Fact]
        public void VocabularyWithFilter()
        {
            var lambda = Common.Evaluate(@"vocabulary:wait.");
            Assert.True(lambda.Children.First().Children.Count() > 10);
            foreach (var idx in lambda.Children.First().Children.Select(x => x.GetEx<string>()))
            {
                Assert.StartsWith("wait.", idx);
            }
        }
    }
}
