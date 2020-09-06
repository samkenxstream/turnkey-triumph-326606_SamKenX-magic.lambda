/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using System.Linq;
using System.Threading.Tasks;
using Xunit;
using magic.node.extensions;
using magic.lambda.exceptions;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace magic.lambda.tests
{
    public class ExceptionsTests
    {
        [Fact]
        public void Throws_01()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
try
   throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
");
            Assert.Equal(true, lambda.Children.First().Value);
        }

        [Fact]
        public void Throws_02_Throws()
        {
            Assert.Throws<HyperlambdaException>(() => Common.Evaluate(@"
try
   throw:foo
"));
        }

        [Fact]
        public void NoThrow_01()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
try
   .throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
");
            Assert.Equal(false, lambda.Children.First().Value);
        }


        [Fact]
        public async Task Throws_02_ThrowsAsync()
        {
            await Assert.ThrowsAsync<HyperlambdaException>(async () => await Common.EvaluateAsync(@"
wait.try
   throw:foo
"));
        }

        [Fact]
        public async Task NoThrow_01Async()
        {
            var lambda = await Common.EvaluateAsync(@"
.throws:bool:false
wait.try
   .throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
");
            Assert.Equal(false, lambda.Children.First().Value);
        }

        [Fact]
        public void Throws_02()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
try
   throw:foo
.catch
.finally
   set-value:x:@.throws
      .:bool:true
");
            Assert.Equal(true, lambda.Children.First().Value);
        }

        [Fact]
        public void Throws_FinallyInvoked()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
try
   try
      throw:foo
   .finally
      set-value:x:@.throws
         .:bool:true
.catch
");
            Assert.Equal(true, lambda.Children.First().Value);
        }

        [Fact]
        public async Task Throws_FinallyInvokedAsync_01()
        {
            var lambda = await Common.EvaluateAsync(@"
.throws:bool:false
wait.try
   wait.try
      throw:foo
   .finally
      set-value:x:@.throws
         .:bool:true
.catch
");
            Assert.Equal(true, lambda.Children.First().Value);
        }

        [Fact]
        public async Task Throws_FinallyInvokedAsync_02()
        {
            var lambda = await Common.EvaluateAsync(@"
.throws:bool:false
wait.try
   wait.try
      throw:foo
   .catch
   .finally
      set-value:x:@.throws
         .:bool:true
.catch
");
            Assert.Equal(true, lambda.Children.First().Value);
        }

        [Fact]
        public void Throws_03Throws()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
.status
.public
.message
.type
try
   throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
   set-value:x:@.status
      get-value:x:@.arguments/*/status
   set-value:x:@.public
      get-value:x:@.arguments/*/public
   set-value:x:@.message
      get-value:x:@.arguments/*/message
   set-value:x:@.type
      get-value:x:@.arguments/*/type
");
            Assert.Equal(500, lambda.Children.Skip(1).First().Value);
            Assert.Equal(false, lambda.Children.Skip(2).First().Value);
            Assert.Equal("foo", lambda.Children.Skip(3).First().Value);
            Assert.Equal("magic.lambda.exceptions.HyperlambdaException", lambda.Children.Skip(4).First().Value);
        }

        [Fact]
        public async Task Throws_03ThrowsAsync()
        {
            var lambda = await Common.EvaluateAsync(@"
.throws:bool:false
.status
.public
.message
.type
wait.try
   throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
   set-value:x:@.status
      get-value:x:@.arguments/*/status
   set-value:x:@.public
      get-value:x:@.arguments/*/public
   set-value:x:@.message
      get-value:x:@.arguments/*/message
   set-value:x:@.type
      get-value:x:@.arguments/*/type
");
            Assert.Equal(500, lambda.Children.Skip(1).First().Value);
            Assert.Equal(false, lambda.Children.Skip(2).First().Value);
            Assert.Equal("foo", lambda.Children.Skip(3).First().Value);
            Assert.Equal("magic.lambda.exceptions.HyperlambdaException", lambda.Children.Skip(4).First().Value);
        }

        [Fact]
        public void Throws_04()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
try
   throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
");
            Assert.Equal(true, lambda.Children.First().Value);
        }

        [Fact]
        public void Throws_05()
        {
            var lambda = Common.Evaluate(@"
.throws:bool:false
.status
.public
.message
.type
try
   throw
      public:true
      status:123
.catch
   set-value:x:@.throws
      .:bool:true
   set-value:x:@.status
      get-value:x:@.arguments/*/status
   set-value:x:@.public
      get-value:x:@.arguments/*/public
   set-value:x:@.message
      get-value:x:@.arguments/*/message
   set-value:x:@.type
      get-value:x:@.arguments/*/type
");
            Assert.Equal(123, lambda.Children.Skip(1).First().Value);
            Assert.Equal(true, lambda.Children.Skip(2).First().Value);
            Assert.Equal("[no-message]", lambda.Children.Skip(3).First().Value);
            Assert.Equal("magic.lambda.exceptions.HyperlambdaException", lambda.Children.Skip(4).First().Value);
        }

        [Fact]
        public async Task Throws_01Async()
        {
            var lambda = await Common.EvaluateAsync(@"
.throws:bool:false
wait.try
   throw:foo
.catch
   set-value:x:@.throws
      .:bool:true
");
            Assert.True(lambda.Children.First().Get<bool>());
        }

        [Fact]
        public void ThrowsCSharp_01()
        {
            HyperlambdaException ex = null;
            try
            {
                throw new HyperlambdaException();
            }
            catch(HyperlambdaException ex2)
            {
                ex = ex2;
            }
            Assert.Equal(500, ex.Status);
            Assert.False(ex.IsPublic);
        }

        [Fact]
        public void ThrowsCSharp_02()
        {
            HyperlambdaException ex = null;
            try
            {
                throw new HyperlambdaException("foo");
            }
            catch(HyperlambdaException ex2)
            {
                ex = ex2;
            }
            Assert.Equal(500, ex.Status);
            Assert.False(ex.IsPublic);
            Assert.Equal("foo", ex.Message);
        }

        [Fact]
        public void ThrowsCSharp_03()
        {
            HyperlambdaException ex = null;
            try
            {
                throw new HyperlambdaException("foo", new ArgumentException());
            }
            catch(HyperlambdaException ex2)
            {
                ex = ex2;
            }
            Assert.Equal(500, ex.Status);
            Assert.False(ex.IsPublic);
            Assert.Equal("foo", ex.Message);
            Assert.Equal(typeof(ArgumentException), ex.InnerException.GetType());
        }

        [Fact]
        public void SerializeException()
        {
            HyperlambdaException ex = new HyperlambdaException("Test", true, 123);
            using (MemoryStream stream = new MemoryStream())
            {
               try
               {
                  var formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File));
                  formatter.Serialize(stream, ex);
                  stream.Position = 0;
                  var deserializedException = (HyperlambdaException)formatter.Deserialize(stream);
                  throw deserializedException;
               }
               catch (SerializationException)
               {
                  throw new Exception("Unable to serialize/deserialize the exception");
               }
               catch (HyperlambdaException error)
               {
                  Assert.Equal("Test", error.Message);
                  Assert.Equal(123, error.Status);
                  Assert.True(error.IsPublic);
               }
            }
         }
    }
}
