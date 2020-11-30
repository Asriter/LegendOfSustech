using System.Collections.Generic;

public class Transformer : Character //变身 bug 显示错误：①变身没有伤害但显示打全体；②变身后普攻打了全体但是只有单个目标血条会变
{
    private bool _trans = false; //表示变身与否

    public Transformer() : base(2600, 2800, 15, 5, 300, 1)
    {
        id = 19;
    }

    //变身前打单体，变身后打全体（都是1.0倍攻击力伤害）
    public override double Attack(bool isCritic)
    {
        if (!_trans)
            return base.Attack(isCritic);

        double atk = Count_atk();
        double damage = Count_damage(atk);
        if (isCritic)
        {
            damage *= 2;
        }

        List<Character> targets = Get_target(_trans);
        foreach (Character target in targets)
        {
            target.Defense(damage);
        }

        return damage;
    }

    //大招：变身，没有其他花里胡哨的
    public override int Skill(bool isCritic)
    {
        _trans = true;
        _atkMp = 0; //大招全局只会用一次
        _skillMp = 10000;
        return base.Skill(isCritic);
    }

    //变身前打单体，变身后打全体
    public override List<Character> Get_target(bool skill)
    {
        if (!skill)
            return base.Get_target(skill);
        //变身后攻击所有敌方活着的单位
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