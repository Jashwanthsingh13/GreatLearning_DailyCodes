namespace Demo.tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestMethod1()
    {
        //Creating an object of the class 

        var Calc = new Calcualtor();

        //Passing value

        int result = Calc.Multiply(2, 6);

        //Checking for the expected value

        Assert.AreEqual(12, result);
    }
}
