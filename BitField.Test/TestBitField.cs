namespace BitField.Tests
{
    [TestClass]
    public sealed class TestBitField
    {
        /// <summary>
        /// This method is called once for the test class, before any tests of the class are run.
        /// </summary>
        /// <param name="context">Context of the test</param>
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            context.WriteLine($"Testing BitField struct");
        }

        [TestMethod]
        public void TestIndexThrows()
        {
            BitField bits = new();

            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[-1]);
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[8]);
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[-1] = true);
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[8] = true);
        }
    }
}
