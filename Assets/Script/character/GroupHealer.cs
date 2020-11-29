using System.Collections.Generic;

public class GroupHealer : Character //群体治疗 TODO 大招有bug，猜测是因为大招的目标只有友方没有敌方，缺少治疗动画
{
    public GroupHealer() : base(2500, 1500, 20, 5, 300, 1)
    {
        this.id = 11;
    }

    //被动：行动结束后给全体活着友方0.5倍攻击力的治疗量
    public override List<int> Action(battle_data battleData)
    {
        List<int> msg = base.Action(battleData);
        List<Character> friends = Get_target(true);
        foreach (Character friend in friends)
        {
            friend.Get_heal(Heal(0.5 * Count_atk()));
        }
        return msg;
    }

    //大招：给全体活着友方1倍攻击力的治疗量（被动会在大招后生效，也就是总共1.5倍攻击力的群奶）
    public override int Skill(bool isCritic)
    {
        List<Character> friends = Get_target(true);
        foreach (Character friend in friends)
        {
            friend.Get_heal(Heal(Count_atk()));
        }
        return 1;
    }

    public override List<Character> Get_target(bool skill)
    {
        if (!skill)
            return base.Get_target(skill);
        //技能目标为全体友方活着单位
        List<Character> list = new List<Character>();
        if (battleData == null)
            battleData = controller.Instance.battleData;
        Character[,,] enemies = battleData.GetCharacterList();
        Character friend;

        //往list里加所有活着的友方单位
        int f = Get_location()[0];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!battleData.hasCharacterInGrid(f, i, j))
                    continue;
                friend = enemies[f, i, j];
                if (friend._hp > 0)
                {
                    list.Add(friend);
                }
            }
        }

        return list;
    }
}