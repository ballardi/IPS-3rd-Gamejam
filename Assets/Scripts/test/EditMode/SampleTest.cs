using NUnit.Framework;

public class SampleTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void SampleTestSimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.Pass();
    }
}
