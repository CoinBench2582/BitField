namespace BitField.Tests
{
    [TestClass]
    public sealed class TestBitField
    {
        private static readonly byte testByte = 0b1010101;
        private static readonly bool[] testBitArray = { true, false, true, false, true, false, true };

        /// <summary>
        /// This method is called once for the test class, before any tests of the class are run.
        /// </summary>
        /// <param name="context">Context of the test</param>
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            // context.WriteLine($"BitField struct tests");
        }

        [TestMethod]
        public void TestIndexThrows()
        {
            BitField bits = new();

            try
            {
                _ = bits[0];
                _ = bits[7];
                bits[0] = false;
                bits[7] = false;
            }
            catch 
            {
                throw new AssertFailedException();
            }
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[-1]);
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[8]);
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[-1] = true);
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[8] = true);
        }

        [TestMethod]
        public void TestConstructingThrows()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(static () => _ = new BitField(new bool[16]));
#pragma warning disable CS8625 // Literál null nejde převést na odkazový typ, který nemůže mít hodnotu null.
            _ = Assert.ThrowsException<ArgumentNullException>(static () => _ = new BitField(null));
#pragma warning restore CS8625 // Literál null nejde převést na odkazový typ, který nemůže mít hodnotu null.
        }

        [TestMethod]
        public void TestInitialisedValue()
        {
            BitField fromByte = new(testByte);
            BitField fromArray = new(testBitArray);

            IEnumerator<bool> first = fromByte.GetEnumerator();
            IEnumerator<bool> second = fromArray.GetEnumerator();

            while (first.MoveNext() && second.MoveNext())
                Assert.AreEqual(first.Current, second.Current);
        }
    }
}
