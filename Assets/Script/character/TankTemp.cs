//一次答辩用，坦克
public class TankTemp : Character
{
    public TankTemp() : base(3600, 1000, 40, 5, 300)
    {
        this.id = 2;
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