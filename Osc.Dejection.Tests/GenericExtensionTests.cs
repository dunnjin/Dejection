using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Osc.Dejection.Extensions;

namespace Osc.Dejection.Tests
{
    public class GenericExtensionTests
    { 
        public class ObjectStub
        {
            public ObjectStub Object { get; set; }
        }

        [Fact]
        public void GreaterThanEqualToItself()
        {
            Assert.True(2.GreatherThanOrEqual(2));
        }
        
        [Fact]
        public void LessThanEqualToItself()
        {
            Assert.True(2.LessThanOrEqual(2));
        }

        [Fact]
        public void TwoIsGreaterThanOne()
        {
            Assert.True(2.GreatherThan(1));
        }

        [Fact]
        public void OneIsLessThanTwo()
        {
            Assert.True(1.LessThan(2));
        }        

        [Fact]
        public void TwoIsNotLessThanOne()
        {
            Assert.False(2.LessThan(1));
        }

        [Fact]
        public void OneIsNotGreaterThanTwo()
        {
            Assert.False(1.GreatherThan(2));
        }

        [Fact]
        public void NegativeTwoIsLessThanNegativeOne()
        {
            Assert.True((-2).LessThan(-1));
        }

        [Fact]
        public void NegativeOneIsGreaterThanNegativeTwo()
        {
            Assert.True((-1).GreatherThan(-2));
        }

        [Fact]
        public void NegativeTwoIsNotGreaterThanNegativeOne()
        {
            Assert.False((-2).GreatherThan(-1));
        }

        [Fact]
        public void NegativeOneIsNotLessThanNegativeTwo()
        {
            Assert.False((-1).LessThan(-2));
        }

        [Fact]
        public void ClampReturnsSame()
        {
            Assert.Equal(1, 1.Clamp(0, 2));
        }

        [Fact]
        public void ClampReturnsMin()
        {
            Assert.Equal(0, (-1).Clamp(0, 2));
        }

        [Fact]
        public void ClampReturnsMax()
        {
            Assert.Equal(2, 3.Clamp(0, 2));
        }

        [Fact]
        public void IsNullReturnsTrueWhenObjectSetToNull()
        {
            object actual = null;

            Assert.True(actual.IsNull());
        }

        [Fact]
        public void IsNullReturnsFalseWhenObjectSetToInstance()
        {
            object actual = new object();

            Assert.False(actual.IsNull());
        }

        [Fact]
        public void IsNullReturnsFalseWhenNestedObjectSetToNull()
        {
            ObjectStub stub = new ObjectStub();

            Assert.True(stub.IsNull(obj => obj.Object));
        }

        [Fact]
        public void IsNullReturnsTrueWhenNestedObjectSetToInstance()
        {
            ObjectStub stub = new ObjectStub() { Object = new ObjectStub() };

            Assert.False(stub.IsNull(obj => obj.Object));
        }
    }
}
