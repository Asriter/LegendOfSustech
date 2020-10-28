//一次答辩用，战士/辅助
public class WarriorTemp : Character
{
    public WarriorTemp() : base(2500, 1750, 30, 5, 300)
    {
        this.id = 3;
    }

    public override int Skill(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(2 * atk);
        if (isCritic)
            damage *= 2;
        Get_target(true)[0].Defense(damage);
        return 1;
    }

    protected override void Die()
    {
    }

    /*public override void Attack_cartoon()
    {
    }

    public override void Defense_cartoon()
    {
    }

    public override void Die_cartoon()
    {
    }*/
}