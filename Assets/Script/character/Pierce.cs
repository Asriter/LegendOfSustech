public class Pierce : Character //单体输出
{
    //被动：增加20%伤害
    public Pierce() : base(2800, 2500, 10, 10, 300, 1)
    {
        id = 16;
        Get_buff(new Buff(BuffKind.Damage, 20, true, 999));
    }
    
    //大招：攻击敌人造成2.0倍攻击力伤害并无视防御（实现方法是根据敌人防御将伤害乘一个倍数）
    public override int Skill(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(2 * atk);
        if(isCritic){
            damage *= 2;
        }
        
        Character target = Get_target(true)[0];
        //根据目标防御加伤害
        damage /= (100 - target.Count_def()) / 100;
        target.Defense(damage);
        return base.Skill(isCritic);
    }
}