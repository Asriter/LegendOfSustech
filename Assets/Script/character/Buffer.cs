using System.Collections.Generic;

public class Buffer : Character //加buff
{
    public Buffer() : base(3000, 1250, 30, 5, 300, 1)
    {
        id = 15;
    }
    
    //被动:行动结束后友方攻击力最高的攻击力加40%，持续1回合
    public override List<int> Action(battle_data battleData)
    {
        List<int> msg = base.Action(battleData);
        
        if (battleData == null)
            battleData = controller.Instance.battleData;
        Character[,,] enemies = battleData.GetCharacterList();
        Character friend;
        Character target = this;

        //检测攻击力最高的
        int f = Get_location()[0];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!battleData.hasCharacterInGrid(f, i, j))
                    continue;
                friend = enemies[f, i, j];
                if (friend._hp > 0 && friend._atk > target._atk)
                {
                    target = friend;
                }
            }
        }
        
        //给攻击力最高的上buff
        target.Get_buff(new Buff(BuffKind.Atk, 40, true, 1));
        
        return msg;
    }

    //大招：对敌方全体造成0.5倍攻击力伤害，并减少20防御，持续1回合
    public override int Skill(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(0.5 * atk);
        if(isCritic){
            damage *= 2;
        }

        List<Character> targets = Get_target(true);
        foreach (Character target in targets)
        {
            target.Defense(damage);
            target.Get_buff(new Buff(BuffKind.Def, -20, false, 1));
        }

        return base.Skill(isCritic);
    }
    
    public override List<Character> Get_target(bool skill)
    {
        //不是技能就正常get target
        if (!skill)
            return base.Get_target(skill);
        //是技能就选择所有敌方活着的单位
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