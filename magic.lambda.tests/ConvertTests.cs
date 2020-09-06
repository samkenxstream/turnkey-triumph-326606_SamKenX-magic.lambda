/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System;
using System.Linq;
using Xunit;
using magic.node;
using magic.node.extensions;
using magic.node.expressions;

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
            Assert.Equal(typeof(int), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal(5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToUInt()
        {
            var lambda = Common.Evaluate(@"
.src:5
convert:x:-
   type:uint");
            Assert.Equal(typeof(uint), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal((uint)5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToShort()
        {
            var lambda = Common.Evaluate(@"
.src:5
convert:x:-
   type:short");
            Assert.Equal(typeof(short), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal((short)5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToUShort()
        {
            var lambda = Common.Evaluate(@"
.src:5
convert:x:-
   type:ushort");
            Assert.Equal(typeof(ushort), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal((ushort)5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToLong()
        {
            var lambda = Common.Evaluate(@"
.src:5
convert:x:-
   type:long");
            Assert.Equal(typeof(long), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal((long)5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToULong()
        {
            var lambda = Common.Evaluate(@"
.src:5
convert:x:-
   type:ulong");
            Assert.Equal(typeof(ulong), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal((ulong)5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToDecimal()
        {
            var lambda = Common.Evaluate(@"
.src:5
convert:x:-
   type:decimal");
            Assert.Equal(typeof(decimal), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal((decimal)5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToDouble()
        {
            var lambda = Common.Evaluate(@"
.src:5
convert:x:-
   type:double");
            Assert.Equal(typeof(double), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal((double)5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToSingle_01()
        {
            var lambda = Common.Evaluate(@"
.src:5
convert:x:-
   type:single");
            Assert.Equal(typeof(float), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal((float)5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToSingle_02()
        {
            var lambda = Common.Evaluate(@"
.src:5
convert:x:-
   type:float");
            Assert.Equal(typeof(float), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal((float)5, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToBool()
        {
            var lambda = Common.Evaluate(@"
.src:true
convert:x:-
   type:bool");
            Assert.Equal(typeof(bool), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal(true, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToDate()
        {
            var lambda = Common.Evaluate(@"
.src:""2020-12-30T23:59:11""
convert:x:-
   type:date");
            Assert.Equal(typeof(DateTime), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal(new DateTime(2020, 12, 30, 23, 59, 11, DateTimeKind.Utc), lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToGuid()
        {
            var lambda = Common.Evaluate(@"
.src:""68ca1ec2-0776-4c37-a94a-108341fc372c""
convert:x:-
   type:guid");
            Assert.Equal(typeof(Guid), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal(new Guid("68ca1ec2-0776-4c37-a94a-108341fc372c"), lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToChar()
        {
            var lambda = Common.Evaluate(@"
.src:a
convert:x:-
   type:char");
            Assert.Equal(typeof(char), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal('a', lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToByte()
        {
            var lambda = Common.Evaluate(@"
.src:57
convert:x:-
   type:byte");
            Assert.Equal(typeof(byte), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal((byte)57, lambda.Children.Skip(1).First().Value);
        }

        [Fact]
        public void ConvertToExpression()
        {
            var lambda = Common.Evaluate(@"
.src:foo/bar
convert:x:-
   type:x");
            Assert.Equal(typeof(Expression), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal("foo/bar", lambda.Children.Skip(1).First().Get<Expression>().Value);
        }

        [Fact]
        public void ConvertToNode()
        {
            var lambda = Common.Evaluate(@"
.src:"".foo""
convert:x:-
   type:node");
            Assert.Equal(typeof(Node), lambda.Children.Skip(1).First().Value.GetType());
            Assert.Equal("", lambda.Children.Skip(1).First().Get<Node>().Name);
            Assert.Equal(".foo", lambda.Children.Skip(1).First().Get<Node>().Children.First().Name);
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


        [Fact]
        public void ConvertThrows()
        {
            Assert.Throws<ArgumentException>(() => Common.Evaluate(@"
.src:int:5
convert:x:-
   type:nont-existing-type"));
        }
    }
}
