using System.Collections.Generic;

public class DoubleAgent : Character
{
    public DoubleAgent() : base(2600, 2000, 15, 10, 300, 1)
    {
        id = 20;
    }

    //被动：普攻后给予自己20%攻击力加成2回合，若攻击目标存活，它也获得同样的buff
    public override double Attack(bool isCritic)
    {
        Modify_mp(_atkMp);
        double atk = Count_atk();
        double damage = Count_damage(atk);
        
        if (isCritic)
        {
            damage *= 2;
        }

        Character target = Get_target(false)[0];
        target.Defense(damage);
        Get_buff(new Buff(BuffKind.Atk, 20, true, 2));
        if(target._hp > 0)
            target.Get_buff(new Buff(BuffKind.Atk, 20, true, 2));
        return damage;
    }

    //大招：对敌方全体造成0.7倍攻击力伤害
    public override int Skill(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(0.7 * atk);
        if (isCritic)
        {
            damage *= 2;
        }

        List<Character> targets = Get_target(true);
        foreach (Character target in targets)
        {
            target.Defense(damage);
        }

        return base.Skill(isCritic);
    }

    public override List<Character> Get_target(bool skill)
    {
        if (!skill)
            return base.Get_target(skill);
        //大招攻击所有敌方活着的单位
        List<Character> list = new List<Character>();
        if (battleData == null)
            battleData = controller.Instance.battleData;
        Character[,,] enemies = battleData.GetCharacterList();
        Character enemy;
        int enemyGroup = Get_location()[0] == 0 ? 1 : 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!battleData.hasCharacterInGrid(enemyGroup, i, j))
                    continue;
                enemy = enemies[enemyGroup, i, j];
                if (enemy._hp > 0)
                {
                    list.Add(enemy);
                }
            }
        }

        return list;
    }
}