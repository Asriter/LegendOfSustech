using System.Collections.Generic;

public class Taunter : Character //嘲讽盾
{
    //被动：加10%防御
    public Taunter() : base(4000, 800, 40, 5, 300, 1)
    {
        this.id = 4;
        Get_buff(new Buff(BuffKind.Def, 10, true, 999));
    }

    //大招：对全体敌人造成0.2倍攻击力伤害，并获得嘲讽一回合
    public override int Skill(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(0.2 * atk);
        if(isCritic){
            damage *= 2;
        }

        List<Character> targets = Get_target(true);
        foreach (Character target in targets)
        {
            target.Defense(damage);
        }
        
        Get_buff(new Buff(BuffKind.Taunt, 0, false, 1));

        base.Skill(isCritic);
        return 1;
    }

    public override List<Character> Get_target(bool skill)
    {
        //不是技能就正常get target
        if (!skill)
            return base.Get_target(skill);
        //是技能就攻击所有敌方活着的单位
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