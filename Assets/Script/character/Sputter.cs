using System.Collections.Generic;
using System.Linq;

public class Sputter : Character //溅射 TODO 被动未实现，容易出现数组越界
{
    public Sputter() : base(2600, 2500, 15, 5, 300, 1)
    {
        id = 17;
    }

    // //被动：普攻十字溅射，溅射造成0.25倍攻击力伤害
    // public override double Attack(bool isCritic)
    // {
    //     Modify_mp(_atkMp);
    //     double atk = Count_atk();
    //     double damage = Count_damage(atk);
    //     if (isCritic)
    //     {
    //         damage *= 2;
    //     }
    //
    //     double ratio = 1;
    //     foreach (Character enemy in Get_target(false))
    //     {
    //         enemy.Defense(damage * ratio);
    //         ratio = 0.25; //攻击第一个敌人后，其余都造成0.25倍伤害
    //     }
    //
    //     return damage;
    // }

    //大招：打全体敌人0.6倍攻击力伤害
    public override int Skill(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(0.6 * atk);
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

    //普攻十字溅射，大招打全体
    public override List<Character> Get_target(bool skill)
    {
        if (battleData == null)
            battleData = controller.Instance.battleData;
        Character[,,] enemies = battleData.GetCharacterList();
        Character enemy;
        List<Character> targets = new List<Character>();
        //非大招打十字
        if (!skill)
        {
            return base.Get_target(false); //如果要实现被动，注释此行
            // targets.Append(base.Get_target(skill)[0]);
            // int e = targets[0].Get_location()[0];
            // int x = targets[0].Get_location()[1];
            // int y = targets[0].Get_location()[2];
            // //bug 数组越界可能原因在下面
            // for (int cnt = 0; cnt < 4; cnt++)
            // {
            //     switch (cnt) //根据这个变化检测的坐标，检测顺序为左右下上
            //     {
            //         case 0:
            //             x--;
            //             break;
            //         case 1:
            //             x += 2;
            //             break;
            //         case 2:
            //             x--;
            //             y++;
            //             break;
            //         case 3:
            //             y -= 2;
            //             break;
            //     }
            //
            //     if (x >= 0 && x <= 2 && y >= 0 && y <= 2)
            //         continue;
            //     if (!battleData.hasCharacterInGrid(e, x, y))
            //         continue;
            //     enemy = enemies[e, x, y];
            //     if (enemy._hp > 0)
            //     {
            //         targets.Add(enemy);
            //     } //bug 数组越界可能原因在上面
            // }
            //
            // return targets;
        }

        //大招打全体
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
                    targets.Add(enemy);
                }
            }
        }

        return targets;
    }
}