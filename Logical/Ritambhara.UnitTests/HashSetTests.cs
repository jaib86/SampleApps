using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ritambhara.UnitTests
{
    public class HashSetTests
    {
        [Fact]
        public void FindFirstDuplicateChar1()
        {
            char ch = HashSetExample.FindFirstDuplicateChar("Hello World");

            Assert.Equal('l', ch);
        }
    }
}
