using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Random = System.Random;

//一次答辩用，坦克
public class TankTemp : Character
{
    public TankTemp() : base(3600, 1000, 40, 5, 300, 1)
    {
        this.id = 2;
    }

    public override int Skill(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(2 * atk);
        //Debug.Log("使用技能");
        if (isCritic)
        {
            //Debug.Log("使用技能且暴击");
            damage *= 2;
        }

        Get_target(true)[0].Defense(damage);
        return 1;
    }

    /*public override void Attack_cartoon()
    {
    }

    public override void Defense_cartoon()
    {
    }

    public override void Die_cartoon()
    {
    }*/
}