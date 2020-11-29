using Random = System.Random;

public class Paladin : Character //奶骑
{
    public Paladin() : base(4500, 1200, 30, 5, 300, 1)
    {
        this.id = 5;
    }

    //被动：普攻50%几率回复0.5倍攻击力的血
    public override double Attack(bool isCritic)
    {
        double atk = Count_atk();
        if(new Random().NextDouble() <= 0.5)
        {
            Get_heal(0.5 * atk);
        }
        return base.Attack(isCritic);
    }

    //大招：对目标造成2倍攻击力的伤害，并回复2倍攻击力的血
    public override int Skill(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(2 * atk);
        
        if(isCritic){
            damage *= 2;
        }
        Get_target(true)[0].Defense(damage);
        
        Get_heal(2 * atk);
        return base.Skill(isCritic);
    }
}