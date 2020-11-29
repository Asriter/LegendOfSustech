using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Linkage : Character //联动 TODO 有bug,引用错误
{
    public Linkage() : base(4000, 500, 40, 5, 300, 1)
    {
        this.id = 8;
    }

    //被动： 受伤25%概率让一个友方单位普攻（附带正常的普攻特效，不加Action的怒气）
    public override void Defense(double damage)
    {
        base.Defense(damage);
        if (_hp == 0) return; //死了不触发

        Random random = new Random();
        if (random.NextDouble() <= 0.25)
        {
            //先往list里加所有活着的友方单位
            List<Character> list = new List<Character>();
            if (battleData == null)
                battleData = controller.Instance.battleData;
            Character[,,] enemies = battleData.GetCharacterList();
            Character friend;

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

            //随机一个让他普攻，即调用Attack()方法
            if (list.Count > 0)
            {
                friend = list[random.Next() % list.Count];
                friend.Attack(friend.Count_critic());
            }
        }
    }

    //大招： 以自己为中心的3×3范围内加50%防御，持续1回合
    public override int Skill(bool isCritic)
    {
        List<Character> list = new List<Character>();
        if (battleData == null)
            battleData = controller.Instance.battleData;
        Character[,,] enemies = battleData.GetCharacterList();
        Character friend;

        //检测周围的己方活着单位
        Vector3Int location = Get_location();
        int f = location[0];
        for (int cnt1 = 0; cnt1 < 3; cnt1++)
        {
            int i = location[1] - 1 + cnt1;
            if (i >= 0 && i <= 2)
            {
                for (int cnt2 = 0; cnt2 < 3; cnt2++)
                {
                    int j = location[2] - 1 + cnt2;
                    if (j >= 0 && j <= 2)
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
            }
        }

        //加buff
        foreach (Character character in list)
        {
            character.Get_buff(new Buff(BuffKind.Def, 50, true, 1));
        }

        return base.Skill(isCritic);
    }
}