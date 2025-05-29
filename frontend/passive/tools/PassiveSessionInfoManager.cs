using Godot;

public partial class PassiveSessionInfoManager : Node
{
    private double _totalTime;
    private double _timeSpent;
    private int _accumulatedExp;

    public void setTotalTime(double totalTime)
    {
        _totalTime = totalTime;
    }

    public double getTotalTime()
    {
        return _totalTime;
    }

    public void setTimeSpent(double timeSpent)
    {
        _timeSpent = timeSpent;
    }

    public double getTimeSpent()
    {
        return _timeSpent;
    }

    public void setAccumulatedExp(int accumulatedExp)
    {
        _accumulatedExp = accumulatedExp;
    }

    public int getAccumulatedExp()
    {
        return _accumulatedExp;
    }
}