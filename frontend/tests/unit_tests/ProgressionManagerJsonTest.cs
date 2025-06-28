using GdUnit4;
using System.IO;
using System.Text.Json;
using static GdUnit4.Assertions;

[TestSuite]
public partial class ProgressionManagerJsonTest
{
    private ProgressionManager _progressionManager;
    private string _testSaveFilePath = "user://test_progression.json";
    
    // Helper method to create or write to test json file
    private void CreateTestJsonFile(ProgressionManager.ProgressionData data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(data, options);
        
        var dirPath = Path.GetDirectoryName(_testSaveFilePath);
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
            
        File.WriteAllText(_testSaveFilePath, jsonString);
    }
    
    // Helper method to load test json file
    private ProgressionManager.ProgressionData LoadTestJsonFile()
    {
        if (!File.Exists(_testSaveFilePath))
            return new ProgressionManager.ProgressionData();
            
        string jsonString = File.ReadAllText(_testSaveFilePath);
        return JsonSerializer.Deserialize<ProgressionManager.ProgressionData>(jsonString) 
               ?? new ProgressionManager.ProgressionData();
    }
    
    [BeforeTest]
    public void SetUp()
    {
        if (File.Exists(_testSaveFilePath))
        {
            File.Delete(_testSaveFilePath);
        }
        
        _progressionManager = new ProgressionManager();
        _progressionManager.SetSaveFilePath(_testSaveFilePath);
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
    public void TestInitialization_UsesDefaults()
    {
        _progressionManager._Ready();
        AssertThat(_progressionManager.GetExp()).IsEqual(0);
        AssertThat(_progressionManager.GetLevel()).IsEqual(1);
        AssertThat(ProgressionManager.BaseExp).IsEqual(300);
        AssertThat(ProgressionManager.ScaleFactor).IsEqual(1.2);
    }
    
    [TestCase]
    public void TestInitialization_WithDefaultJsonFile()
    {
        var testData = new ProgressionManager.ProgressionData
        {
            Exp = 0,
            Level = 1,
        };
        
        CreateTestJsonFile(testData);
        
        _progressionManager._Ready();
        
        AssertThat(_progressionManager.GetExp()).IsEqual(0);
        AssertThat(_progressionManager.GetLevel()).IsEqual(1);
        AssertThat(ProgressionManager.BaseExp).IsEqual(300);
        AssertThat(ProgressionManager.ScaleFactor).IsEqual(1.2);
    }
    
    [TestCase]
    public void TestInitialization_WithJsonFile_LoadsSavedData()
    {
        var testData = new ProgressionManager.ProgressionData
        {
            Exp = 1000,
            Level = 4, // Should be recalculated based on exp
        };
        
        CreateTestJsonFile(testData);
        
        _progressionManager._Ready();
        
        AssertThat(_progressionManager.GetExp()).IsEqual(1000);
        // Level should be recalculated based on 1000 exp
        AssertThat(_progressionManager.GetLevel()).IsEqual(3);
    }
    
    [TestCase]
    public void TestInitialization_WithMismatchedLevelInJson_RecalculatesLevel()
    {
        var testData = new ProgressionManager.ProgressionData
        {
            Exp = 660, // This should be level 3
            Level = 10,
        };
        
        CreateTestJsonFile(testData);
        
        _progressionManager._Ready();
        
        AssertThat(_progressionManager.GetExp()).IsEqual(660);
        AssertThat(_progressionManager.GetLevel()).IsEqual(3);
    }
}
