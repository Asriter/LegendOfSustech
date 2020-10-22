using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有角色的父类
public class CharacterTemp : MonoBehaviour
{
    //属性
    private double _maxHp; //最大生命值
    private double _hp; //当前生命
    private int _mp = 50; //蓝量，满蓝放技能
    private double _atk; //攻击力
    private double _def; //防御
    private List<Buff> _buffs; //rt
    
    //待添加
    //TODO

    //构造器
    public CharacterTemp(double hp, double atk, double def)
    {
        _maxHp = hp;
        _hp = hp;
        _atk = atk;
        _def = def;
        _buffs = new List<Buff>();
    }

    //行动
    public void Action()
    {
        if (_mp < 100)
        {
            Attack();
            Modify_mp(25);
        }
        else
        {
            Skill();
            Modify_mp(0);
        }
        Check_buff_remain();
    }

    //调用受攻击动画
    public void Defense(double damage)
    {
        double def = _def;
        //TODO: 根据buff增减实际防御
        damage *= (100 - def) / 100;
        //TODO: 根据buff增减实际受伤
        _hp = Math.Max(0, _hp - damage);
        if (_hp == 0)
        {
            Die();
        }
    }

    //获得治疗
    public void Get_heal(double heal)
    {
        //TODO: 根据buff修改获得治疗值
        _hp = Math.Min(_maxHp, _hp + heal);
    }

    public void Get_buff(Buff buff)
    {
        _buffs.Add(buff);
    }

    //调整怒气值，参数不为0时怒气加上参数，参数为0时怒气归零
    public void Modify_mp(int amount)
    {
        if (amount != 0)
        {
            _mp += amount;
        }
        else
        {
            _mp = 0;
        }
    }

    //用于通常攻击，在攻击完成之后返回状态值，让主程序继续运行
    private int Attack()
    {
        double damage = _atk;
        //TODO: 根据buff增加伤害
        Get_target().Defense(_atk);
        return 1;
    }

    //获取攻击目标
    private CharacterTemp Get_target( /*敌阵列表*/)
    {
        //TODO
        return null;
    }

    //释放技能，该部分根据各个单位的子类具体实现
    private int Skill()
    {
        //TODO
        return 1;
    }


    private void Heal()
    {
        double heal = _atk;
        //TODO: 根据buff和公式获取奶量
        Get_heal_target().Get_heal(heal);
    }

    //获取治疗目标
    private CharacterTemp Get_heal_target()
    {
        //TODO
        return null;
    }
    
    //退场
    private void Die()
    {
        //TODO
    }
    
    //回合结束用于减少buff持续时间
    private void Check_buff_remain()
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            Buff buff = _buffs[i];
            buff._remain -= 1;
            if (buff._remain <= 0)
            {
                _buffs.Remove(buff);
            }
        }
    }
}

public class Buff
{
    public BuffKind _buffKind;
    public int _remain;

    public Buff(BuffKind buffKind, int remain)
    {
        _buffKind = buffKind;
        _remain = remain;
    }
}

public enum BuffKind
{
    Atk,
    Def,
    GainDamage,
    Heal,
    GainHeal
}