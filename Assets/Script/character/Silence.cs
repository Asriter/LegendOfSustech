using System.Collections.Generic;

public class Silence : Character //沉默 TODO 被动未实现
{
    public Silence() : base(2500, 1750, 20, 5, 350, 2)
    {
        id = 10;
    }

    //大招：给所有活着敌方一回合沉默
    public override int Skill(bool isCritic)
    {
        List<Character> list = Get_target(true);
        foreach (Character enemy in list)
        {
            enemy.Get_buff(new Buff(BuffKind.Silence, 0, false, 1));
        }
        return 1;
    }

    public override List<Character> Get_target(bool skill)
    {
        //不是技能就正常get target
        if (!skill)
            return base.Get_target(skill);
        
        //是技能就给敌方所有活着的单位1回合沉默
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