using nuscutiesapp.tools;

public partial class PlayerHitbox : Hitbox
{
    protected override float CalcDamage()
    {
        return _derivedStatCalculator.CalcTotalDamage();
    }
}