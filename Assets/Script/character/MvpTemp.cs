//一次答辩用，输出
public class MvpTemp : Character
{
    public MvpTemp() : base(2600, 3000, 15)
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