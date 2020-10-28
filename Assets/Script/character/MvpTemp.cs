//一次答辩用，输出
public class MvpTemp : Character
{
    public MvpTemp() : base(2600, 3000, 15, 5)
    {
        this.id = 1;
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