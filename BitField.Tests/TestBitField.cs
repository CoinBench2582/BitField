namespace BitField.Tests
{
    [TestClass]
    public class TestBitField
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            context.WriteLine($"Testing BitField struct functionality");
            // This method is called once for the test class, before any tests of the class are run.
        }

        [TestInitialize]
        public void TestInit()
        {
            // This method is called before each test method.
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // This method is called after each test method.
        }

        [TestMethod]
        public void TestNothing() { }
    }
}
