namespace BitField.Test
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
        public void TestNothing() { }
    }
}
