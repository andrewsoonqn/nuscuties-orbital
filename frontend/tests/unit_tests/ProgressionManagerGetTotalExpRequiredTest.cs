using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public partial class ProgressionManagerGetTotalExpRequiredTest
{
    [TestCase(1, 0)]
    [TestCase(2, 300)]
    [TestCase(3, 660)]
    [TestCase(4, 1092)]
    [TestCase(5, 1610)]
    [TestCase(10, 6240)]
    public void TestGetTotalExpRequiredForLevel_KnownValues(int level, int expected)
    {
        AssertThat(ProgressionManager.GetTotalExpRequiredForLevel(level)).IsEqual(expected);
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-100)]
    public void TestGetTotalExpRequiredForLevel_InvalidLevel_ReturnsZero(int level)
    {
        AssertThat(ProgressionManager.GetTotalExpRequiredForLevel(level)).IsEqual(0);
    }
}