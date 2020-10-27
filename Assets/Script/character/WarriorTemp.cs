//一次答辩用，战士/辅助
public class WarriorTemp : Character
{
    public WarriorTemp() : base(2500, 1750, 30, 5)
    {
    }

    protected override int Skill()
    {
        double atk = Count_atk();
        double damage = Count_damage(2 * atk);
        Get_target().Defense(damage);
        return 1;
    }

    protected override void Die()
    {
    }

    public override void Attack_cartoon()
    {
    }

    public override void Defense_cartoon()
    {
    }

    public override void Die_cartoon()
    {
    }
}