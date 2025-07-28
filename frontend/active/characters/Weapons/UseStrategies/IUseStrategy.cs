namespace nuscutiesapp.active.characters.Weapons.UseStrategies
{
    public interface IUseStrategy
    {
        public void Use(Weapon w);

        public IUseStrategy Copy();
    }
}