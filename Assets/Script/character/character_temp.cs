using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有角色的父类
public class character_temp : MonoBehaviour
{
    //属性
    private double _hp;    //生命
    private double _atk;    //攻击力
    private double _def;    //防御
    private double _mp;    //蓝量，满蓝放技能
    //buff用于记录状态，每一格代表了一个属性的改变，对应参数在constant中，但尚且未设定
    private double[] _buff;
    //待添加
    //TODO

    //方法
    //获取攻击目标
    public character_temp Get_target()
    {
        //TODO
        return null;
    }

    //用于通常攻击，在攻击完成之后返回状态值，让主程序继续运行
    public int Attack()
    {
        double damage = _atk;
        //TODO: 根据buff增加伤害
        Get_target().Defense(_atk);
        return 1;
    }

    //释放技能，该部分根据各个单位的子类具体实现
    public int Skill(){
        //TODO
        return 1;
    }

    //退场
    public void Die(){
        //TODO
    }

    //调用受攻击动画
    public void Defense(double damage){  //原为attacked
        //TODO: 根据防御，buff等属性减少受伤
        _hp = Math.Max(0, _hp - damage);
        if (_hp == 0)
        {
            Die();
        }
    }

}
