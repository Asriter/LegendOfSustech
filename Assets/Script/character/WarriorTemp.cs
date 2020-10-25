//一次答辩用，战士/辅助
public class WarriorTemp : Character
{
    public WarriorTemp() : base(2500, 1750, 30)
    {
    }

    protected override int Skill()
    {
        double atk = base.Count_atk();
        double damage = base.Count_damage(2 * atk);
        Get_target()[0].Defense(damage);
        return 1;
    }

    protected override void Die()
    {
    }
}