using System;

public class Calmer : Character //频繁减怒
{
    //被动：开大需要怒气为50（开局就能开大）
    public Calmer() : base(3000, 2000, 25, 10, 300, 1)
    {
        this.id = 14;
        _skillMp = 50;
    }

    //被动：普攻25%概率减被攻击者25怒气
    public override double Attack(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(atk);
        if (isCritic)
        {
            damage *= 2;
        }

        Character target = Get_target(false)[0];
        target.Defense(damage);
        if (new Random().NextDouble() <= 0.25)
        {
            target.Modify_mp(-25);
        }

        return damage;
    }

    //大招：攻击敌人并将其怒气归0
    public override int Skill(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(atk);
        if (isCritic)
        {
            damage *= 2;
        }

        Character target = Get_target(false)[0];
        target.Defense(damage);
        target.Modify_mp(0);

        return base.Skill(isCritic);
    }
}