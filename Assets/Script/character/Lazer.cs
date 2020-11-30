using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lazer : Character //激光豆 bug 说是打一列结果打了全体
{
    public Lazer() : base(2750, 1500, 10, 5, 300, 1)
    {
        id = 21;
    }

    //被动：普攻打一列
    public override double Attack(bool isCritic)
    {
        Modify_mp(_atkMp);
        double atk = Count_atk();
        double damage = Count_damage(atk);

        if (isCritic)
        {
            damage *= 2;
        }

        List<Character> targets = Get_target(false);
        foreach (Character target in targets)
        {
            target.Defense(damage);
        }

        return damage;
    }

    //大招：打一列，造成1.6倍攻击力伤害
    public override int Skill(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(1.6 * atk);
        if (isCritic)
        {
            damage *= 2;
        }

        List<Character> targets = Get_target(false);
        foreach (Character target in targets)
        {
            target.Defense(damage);
        }

        return base.Skill(isCritic);
    }

    //返回值为1列
    public override List<Character> Get_target(bool skill)
    {
        List<Character> list = new List<Character>();

        Vector3Int location = Get_location();
        //设置battleData
        if (battleData == null)
            battleData = controller.Instance.battleData;
        Character[,,] enemies = battleData.GetCharacterList();
        Character enemy;
        int enemyGroup = location[0] == 0 ? 1 : 0;

        //检测嘲讽
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!battleData.hasCharacterInGrid(enemyGroup, i, j))
                    continue;
                enemy = enemies[enemyGroup, i, j];
                if (enemy._hp > 0)
                {
                    //检测嘲讽
                    foreach (Buff buff in enemy._buffs)
                    {
                        if (buff._buffKind == BuffKind.Taunt)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                if (!battleData.hasCharacterInGrid(enemyGroup, i, k))
                                    continue;
                                enemy = enemies[enemyGroup, i, k];
                                if (enemy._hp > 0)
                                {
                                    list.Add(enemy);
                                }
                            }

                            return list;
                        }
                    }
                }
            }
        }

        //优先打本列的
        for (int i = 0; i < 3; i++)
        {
            //检测当前位置有没有东西
            for (int k = 0; k < 3; k++)
            {
                if (!battleData.hasCharacterInGrid(enemyGroup, i, k))
                    continue;
                enemy = enemies[enemyGroup, i, k];
                if (enemy._hp > 0)
                {
                    list.Add(enemy);
                }
            }
        }
        //列表里有单位就返回
        if (list.Count > 0) return list;

        //没有本列的，优先打近的那一列
        for (int i = (location.y == 2 ? 2 : 0), step = (location.y == 2 ? -1 : 1);
            i < 3 && i >= 0;
            i += step)
        {
            if (i != location.y)
                //检测当前位置有没有东西
                for (int k = 0; k < 3; k++)
                {
                    if (!battleData.hasCharacterInGrid(enemyGroup, i, k))
                        continue;
                    enemy = enemies[enemyGroup, i, k];
                    if (enemy._hp > 0)
                    {
                        list.Add(enemy);
                    }
                }
            //列表里有单位就返回
            if (list.Count > 0) return list;
        }

        return null;
    }
}