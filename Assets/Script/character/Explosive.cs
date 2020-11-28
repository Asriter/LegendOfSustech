using System.Collections.Generic;

public class Explosive : Character
{
    //被动：起始无怒气（避免炸的太快）
    public Explosive() : base(3600, 750, 40, 0, 300, 1)
    {
        this.id = 7;
        Modify_mp(0);
    }

    //被动：受伤时减少5防御，加250攻击
    public override void Defense(double damage)
    {
        base.Defense(damage);
        Get_buff(new Buff(BuffKind.Def, 5, false, 999));
        Get_buff(new Buff(BuffKind.Atk, 250, false, 999));
    }

    //大招：直接死
    public override int Skill(bool isCritic)
    {
        _hp = 0;
        Die();
        return 1;
    }

    //死亡：炸全场1.5倍攻击力伤害，包括敌我（先计算敌方受伤）
    protected override void Die()
    {
        //检测所有活着的单位
        List<Character> list = new List<Character>();
        if (battleData == null)
            battleData = controller.Instance.battleData;
        Character[,,] enemies = battleData.GetCharacterList();
        Character enemy;

        //先把敌人加入list
        int enemyGroup = Get_location()[0] == 0 ? 1 : 0;
        for (int cnt = 0; cnt < 2; cnt++)
        {
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
            //然后轮到我方
            enemyGroup = enemyGroup == 0 ? 1 : 0;
        }

        double atk = Count_atk();
        double damage = Count_damage(1.5 * atk);
        if (Count_critic())
        {
            damage *= 2;
        }

        foreach (Character target in list)
        {
            target.Defense(damage);
        }
    }
}