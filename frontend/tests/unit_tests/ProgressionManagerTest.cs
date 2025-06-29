using GdUnit4;
using System.IO;
using System.Text.Json;
using static GdUnit4.Assertions;

[TestSuite]
public partial class ProgressionManagerTest
{
    private ProgressionManager _progressionManager;
    private string _testSaveFilePath = "user://test_progression.json";

    [BeforeTest]
    public void SetUp()
    {
        // Clean up any existing test file
        if (File.Exists(_testSaveFilePath))
        {
            File.Delete(_testSaveFilePath);
        }

        _progressionManager = new ProgressionManager();
        _progressionManager.SetSaveFilePath(_testSaveFilePath);
        _progressionManager._Ready();
    }

    [AfterTest]
    public void TearDown()
    {
        _progressionManager?.QueueFree();

        if (File.Exists(_testSaveFilePath))
        {
            File.Delete(_testSaveFilePath);
        }
    }

    [TestCase]
    public void TestCalculateLevel_WithZeroExp_ReturnsLevelOne()
    {
        AssertThat(_progressionManager.GetLevel()).IsEqual(1);
    }

    [TestCase]
    public void TestCalculateLevel_WithExpBelowFirstThreshold_ReturnsLevelOne()
    {
        _progressionManager.AddExp(299);
        AssertThat(_progressionManager.GetLevel()).IsEqual(1);
        AssertThat(_progressionManager.GetExp()).IsEqual(299);
    }

    [TestCase]
    public void TestCalculateLevel_WithExactFirstThreshold_ReturnsLevelTwo()
    {
        _progressionManager.AddExp(300);
        AssertThat(_progressionManager.GetLevel()).IsEqual(2);
        AssertThat(_progressionManager.GetExp()).IsEqual(300);
    }

    [TestCase]
    public void TestCalculateLevel_NegativeValues()
    {
        // Test known calculation points
        _progressionManager.AddExp(660);
        AssertThat(_progressionManager.GetLevel()).IsEqual(3);
        AssertThat(_progressionManager.GetExp()).IsEqual(660);

        _progressionManager.AddExp(1092 - 660); // Add remaining to reach level 4
        AssertThat(_progressionManager.GetLevel()).IsEqual(4);
        AssertThat(_progressionManager.GetExp()).IsEqual(1092);

        _progressionManager.AddExp(660 - 1092); // Deduct previous amount
        AssertThat(_progressionManager.GetLevel()).IsEqual(3);
        AssertThat(_progressionManager.GetExp()).IsEqual(660);
    }

    [TestCase]
    public void TestCalculateLevel_KnownValues()
    {
        // Test known calculation points
        _progressionManager.AddExp(660);
        AssertThat(_progressionManager.GetLevel()).IsEqual(3);

        _progressionManager.AddExp(1092 - 660); // Add remaining to reach level 4
        AssertThat(_progressionManager.GetLevel()).IsEqual(4);

        _progressionManager.AddExp(1610 - 1092); // Add remaining to reach level 5
        AssertThat(_progressionManager.GetLevel()).IsEqual(5);
    }

    [TestCase(1, 0)]
    [TestCase(2, 300)]
    [TestCase(3, 660)]
    [TestCase(4, 1092)]
    [TestCase(5, 1610)]
    [TestCase(10, 6240)]
    public void TestProgressionTable_KnownValues(int level, int expectedTotalExp)
    {
        if (expectedTotalExp > 0)
            _progressionManager.AddExp(expectedTotalExp);

        AssertThat(_progressionManager.GetLevel()).IsEqual(level);
        AssertThat(_progressionManager.GetExp()).IsEqual(expectedTotalExp);
    }

    [TestCase]
    public void TestLargeExpValues_DoesNotOverflow()
    {
        _progressionManager.AddExp(1000000);

        AssertThat(_progressionManager.GetLevel()).IsGreater(1);
        AssertThat(_progressionManager.GetExp()).IsEqual(1000000);
    }

    [TestCase]
    public void TestConsistency_MultipleSmallAdds_vs_OneLargeAdd()
    {
        var manager1 = new ProgressionManager();
        manager1._Ready();

        var manager2 = new ProgressionManager();
        manager2._Ready();

        for (int i = 0; i < 100; i++)
        {
            manager1.AddExp(10);
        }

        manager2.AddExp(1000);

        // Both should end up at the same level
        AssertThat(manager1.GetLevel()).IsEqual(manager2.GetLevel());
        AssertThat(manager1.GetExp()).IsEqual(manager2.GetExp());

        manager1.QueueFree();
        manager2.QueueFree();
    }
}
