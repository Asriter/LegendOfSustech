using System.Collections.Generic;

public class Darius : Character //人头狗 bug 会导致Coroutine couldn't be started because the the game object 'Character' is inactive!，猜测是动画逻辑问题
{
    public Darius() : base(3000, 2000, 20, 0, 300, 1)
    {
        id = 22;
    }

    //大招：造成2.0倍攻击力伤害，击杀回满怒气
    public override int Skill(bool isCritic)
    {
        //先清空怒气
        int ret = base.Skill(isCritic);

        double atk = Count_atk();
        double damage = Count_damage(2 * atk);
        if (isCritic)
        {
            damage *= 2;
        }
        //击杀回满怒气
        Character target = Get_target(true)[0];
        target.Defense(damage);
        if (target._hp == 0) Modify_mp(_skillMp);

        return ret;
    }

    //选择生命值最低的
    public override List<Character> Get_target(bool skill)
    {
        List<Character> list = new List<Character>();
        if (battleData == null)
            battleData = controller.Instance.battleData;
        Character[,,] enemies = battleData.GetCharacterList();
        Character target = null;
        Character enemy;
        int enemyGroup = Get_location()[0] == 0 ? 1 : 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!battleData.hasCharacterInGrid(enemyGroup, i, j))
                    continue;
                enemy = enemies[enemyGroup, i, j];
                if (enemy._hp > 0 && (target == null || target._hp > enemy._hp))
                {
                    target = enemy;
                }
            }
        }

        list.Add(target);
        return list;
    }
}