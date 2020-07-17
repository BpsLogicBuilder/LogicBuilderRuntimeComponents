using LogicBuilder.RulesDirector;
using System;
using Xunit;

namespace RulesDirector.Tests
{
    public class AssignableTests
    {
        [Fact]
        public void Char_is_assignable_from_char()
        {
            //Assert
            Assert.True(typeof(char).AssignableFrom(typeof(char)));
        }

        [Fact]
        public void Object_is_assignable_from_int()
        {
            //Assert
            Assert.True(typeof(object).AssignableFrom(typeof(int)));
        }

        [Fact]
        public void Int_is_assignable_from_byte()
        {
            //Assert
            Assert.True(typeof(int).AssignableFrom(typeof(byte)));
        }

        [Fact]
        public void Int_is_assignable_from_sbyte()
        {
            //Assert
            Assert.True(typeof(int).AssignableFrom(typeof(sbyte)));
        }

        [Fact]
        public void Int_is_assignable_from_char()
        {
            //Assert
            Assert.True(typeof(int).AssignableFrom(typeof(char)));
        }

        [Fact]
        public void Int_is_assignable_from_ushort()
        {
            //Assert
            Assert.True(typeof(int).AssignableFrom(typeof(ushort)));
        }

        [Fact]
        public void Int_is_assignable_from_short()
        {
            //Assert
            Assert.True(typeof(int).AssignableFrom(typeof(short)));
        }

        [Fact]
        public void Int_is_assignable_from_int()
        {
            //Assert
            Assert.True(typeof(int).AssignableFrom(typeof(int)));
        }

        [Fact]
        public void Int_is_assignable_from_dateTime()
        {
            //Assert
            Assert.False(typeof(int).AssignableFrom(typeof(DateTime)));
        }

        [Fact]
        public void Int_is_assignable_from_long()
        {
            //Assert
            Assert.False(typeof(int).AssignableFrom(typeof(long)));
        }

        [Fact]
        public void Int_is_assignable_from_nullable_int()
        {
            //Assert
            Assert.False(typeof(int).AssignableFrom(typeof(int?)));
        }

        [Fact]
        public void Int_is_assignable_from_decimal()
        {
            //Assert
            Assert.False(typeof(int).AssignableFrom(typeof(decimal)));
        }

        [Fact]
        public void Int_is_assignable_from_float()
        {
            //Assert
            Assert.False(typeof(int).AssignableFrom(typeof(float)));
        }

        [Fact]
        public void Int_is_assignable_from_nullable_short()
        {
            //Assert
            Assert.False(typeof(int).AssignableFrom(typeof(short?)));
        }
    }
}
