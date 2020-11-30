
    using System.Collections.Generic;

    public class Marshall: Character //黑胡子 bug 同Darius
    {
        public Marshall() : base(3000, 2000, 20, 0, 300, 1)
        {
            id = 24;
        }

        //大招：攻击目标并偷取目标50%攻击力2回合
        public override int Skill(bool isCritic)
        {
            double atk = Count_atk();
            double damage = Count_damage(atk);
            if (isCritic)
            {
                damage *= 2;
            }
            //偷取目标攻击力
            Character target = Get_target(true)[0];
            target.Defense(damage);
            Get_buff(new Buff(BuffKind.Atk, target._atk * 0.5, false, 2));

            return base.Skill(isCritic);
        }

        //打攻击力最高的
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
                    if (enemy._hp > 0 && (target == null || target._atk < enemy._atk))
                    {
                        target = enemy;
                    }
                }
            }

            list.Add(target);
            return list;
        }
    }
