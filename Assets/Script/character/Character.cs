//所有角色的父类，是个抽象类

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public abstract class Character : MonoBehaviour
{
    //属性
    private readonly double _maxHp; //最大生命值
    private double _hp; //当前生命
    private int _mp = 50; //蓝量，满蓝放技能
    private readonly double _atk; //攻击力
    private readonly double _def; //防御
    private readonly double _critic; //暴击率，5表示5%暴击率
    private List<Buff> _buffs; //rt
    private List<int> _msg; //报文
    private GridUnit _grid;//在那个格子里
    protected int id;//用于实例化之后的单位，通过id获取对应对象

    //待添加

    //构造器,里面不放参数，参数修改放到下面的initial里面,调用perfab的时候不知道会不会加载构造函数
    protected Character(double hp, double atk, double def, double critic)
    {
        _maxHp = hp;
        _hp = hp;
        _atk = atk;
        _def = def;
        _critic = critic;
        _buffs = new List<Buff>();
        _msg = new List<int>();
    }

    //行动，轮到角色行动时调用此方法
    public List<int> Action()
    {
        _msg.Clear();
        _msg.Append(Get_location().x);
        _msg.Append(Get_location().y);
        _msg.Append(Get_location().z);
        if (_mp < 100) //怒气小于100普攻，否则skill
        {
            _msg.Append(0);
            Attack();
            Modify_mp(25); //普攻怒气加25
        }
        else
        {
            _msg.Append(1);
            Skill();
            Modify_mp(0);
        }
        
        Check_buff_remain(); //skill后怒气归零
        return _msg;
    }

    public Vector3Int Get_location()
    {
        return _grid.GetLocation();
    }

    //调整怒气值，参数不为0时怒气加上参数，参数为0时怒气归零
    public void Modify_mp(int amount)
    {
        if (amount != 0)
        {
            _mp += amount;
            _mp = Math.Min(100, _mp); //怒气最高为100
        }
        else
            _mp = 0;
    }

    //回合结束用于减少buff持续时间，移除时间结束的buff
    private void Check_buff_remain()
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            Buff buff = _buffs[i];
            buff._remain -= 1; //持续时间减一
            if (buff._remain <= 0) //时间为0则移除
            {
                _buffs.Remove(buff);
            }
        }
    }

    //用于通常攻击，在攻击完成之后返回状态值，让主程序继续运行
    protected int Attack()
    {
        double atk = Count_atk();
        double damage = Count_damage(atk);
        Get_target().Defense(damage);
        return 1;
    }

    //计算实际攻击力
    protected double Count_atk()
    {
        double atk = _atk;
        atk = Buff_affect(atk, BuffKind.Atk); //根据buff增减攻击力
        return atk;
    }

    //计算理论伤害值
    protected double Count_damage(double damage)
    {
        damage = Buff_affect(damage, BuffKind.Damage); //根据buff增减伤害
        return damage;
    }

    //计算暴击
    protected double Count_critic(double damage)
    {
        if (new Random().NextDouble() <= _critic / 100)
        {
            _msg.Append(1);
            return damage * 2;
        }

        _msg.Append(0);
        return damage;
    }

    //获取攻击目标
    protected Character Get_target()
    {
        //TODO: 获取攻击目标
        return null;
    }

    //受攻击，调用动画、效果等
    public void Defense(double damage)
    {
        _hp = Math.Max(0, _hp - Count_Hurt(damage));
        if (_hp == 0)
        {
            Die();
        }
    }

    //计算实际伤害值
    protected double Count_Hurt(double damage)
    {
        double def = Count_def();
        damage *= (100 - def) / 100;
        damage = Buff_affect(damage, BuffKind.GainDamage); //根据buff增减实际受伤
        return damage;
    }

    //计算实际防御值
    protected double Count_def()
    {
        double def = _def;
        def = Buff_affect(def, BuffKind.Def); //根据buff增减实际防御
        def = Math.Min(def, 75); //无论怎么加，防御最高为75 （即只受四分之一的伤害）
        return def;
    }

    //获得治疗
    public void Get_heal(double heal)
    {
        heal = Buff_affect(heal, BuffKind.GainHeal); //根据buff增减获得治疗量
        _hp = Math.Min(_maxHp, _hp + heal); //当前hp不会超过maxHp
    }

    //得别人给的buff
    public void Get_buff(Buff buff)
    {
        _buffs.Add(buff);
        _buffs.Sort((Buff a, Buff b) //得到buff后将buff表进行排序
            => a._percent == b._percent ? 0 : (a._percent ? 1 : -1)); //将百分比buff排在后面，数值buff排在前面
    }

    //计算数值受所有buff影响后的数值
    private double Buff_affect(double num, BuffKind buffKind)
    {
        for (int i = 0, len = _buffs.Count; i < len; i++)
        {
            if (_buffs[i]._buffKind == buffKind) //判断是否为增加获得治疗量的buff
            {
                num = _buffs[i].Count(num); //若是，则根据buff增减治疗量
            }
        } //由于排序时百分比buff在后面，数值buff在前面，所以遍历时会先加数值再乘百分比

        return num;
    }

    //释放技能，该部分根据各个单位的子类具体实现
    protected abstract int Skill();

    //退场动画，效果等
    protected abstract void Die();

    public abstract void Attack_cartoon();

    public abstract void Defense_cartoon();

    public abstract void Die_cartoon();

    //在初始化battledata的时候用，把所在的格子加进来
    public void SetGrid(GridUnit grid)
    {
        this._grid = grid;
    }

    public int GetId()
    {
        return id;
    }
}