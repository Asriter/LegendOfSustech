//所有角色的父类，是个抽象类

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public abstract class Character : MonoBehaviour
{
    //属性
    public readonly double _maxHp; //最大生命值
    public double _hp; //当前生命
    public int _mp = 50; //蓝量，满蓝放技能
    protected readonly double _atk; //攻击力
    protected readonly double _def; //防御
    protected readonly double _critic; //暴击率，5表示5%暴击率
    public readonly double _speed;
    private List<Buff> _buffs; //rt
    private List<int> _msg; //报文
    private Vector3Int location; //在那个格子里
    protected int id; //用于实例化之后的单位，通过id获取对应对象
    public readonly int _cost; //费用

    //TODO 可否调用动画，该选项存疑暂不使用
    //public bool isAnimation = false;

    //TODO 在调用action的时候传入，可能加入set
    protected battle_data battleData;

    //待添加

    //构造器,里面不放参数，参数修改放到下面的initial里面,调用perfab的时候不知道会不会加载构造函数
    protected Character(double hp, double atk, double def, double critic, double speed, int cost)
    {
        _maxHp = hp;
        _hp = hp;
        _atk = atk;
        _def = def;
        _critic = critic;
        _speed = speed;
        _buffs = new List<Buff>();
        _msg = new List<int>();
        _cost = cost;
    }

    /*
     *    以下为能够被重载的方法
     */

    //行动，轮到角色行动时调用此方法
    public virtual List<int> Action(battle_data battleData)
    {
        Vector3Int location = Get_location();
        this.battleData = battleData;
        _msg = new List<int>();
        _msg.Clear();
        _msg.Add(location.x);
        _msg.Add(location.y);
        _msg.Add(location.z);
        if (_mp < 100) //怒气小于100普攻，否则skill
        {
            _msg.Add(0);
            Attack(Count_critic());
            //Modify_mp(50); //普攻怒气加50
        }
        else
        {
            _msg.Add(1);
            Skill(Count_critic());
            //Modify_mp(0);
        }

        //Check_buff_remain(); //skill后怒气归零
        //Debug.Log("攻击对象" + _msg[0] + " " + _msg[1] + " " + _msg[2] + " " + "技能：" + _msg[3] + " 暴击：" + _msg[4]);
        return _msg;
    }

    //用于通常攻击，在攻击完成之后返回状态值，让主程序继续运行
    //将返回状态值修改成返回伤害
    public virtual double Attack(bool isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(atk);
        //damage = this.Count_critic(damage);
        //是否暴击
        if (isCritic)
        {
            damage *= 2;
            //Debug.Log("发生暴击");
        }

        Get_target(false)[0].Defense(damage);
        Modify_mp(50); //普攻怒气加50
        Check_buff_remain();
        return damage;
    }

    //受攻击，调用动画、效果等
    public virtual void Defense(double damage)
    {
        _hp = Math.Max(0, _hp - Count_Hurt(damage));
        //Debug.Log("血量：" + _hp + " 伤害：" + Count_Hurt(damage));
        if (_hp == 0)
        {
            Die();
        }
    }

    //释放技能，该部分根据各个单位的子类具体实现
    public virtual int Skill(bool isCritic)
    {
        Modify_mp(0); //普攻怒气加50
        Check_buff_remain();
        return 1;
    }

    //退场动画，效果等
    protected virtual void Die()
    {
    }

    //获取攻击目标
    //一般来说isskill没什么用，但如果技能和平A攻击范围不同时有用
    //面对范围攻击的情况，此处返回值修改为list
    public virtual List<Character> Get_target(bool skill) //TODO
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
                            list.Add(enemy);
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
            if (!battleData.hasCharacterInGrid(enemyGroup, location.y, i))
                continue;
            enemy = enemies[enemyGroup, location.y, i];
            if (enemy._hp > 0)
            {
                list.Add(enemy);
                return list;
            }
        }

        //没有本列的，优先打近的那一列
        for (int i = (location.y == 2 ? 2 : 0), step = (location.y == 2 ? -1 : 1);
            i < 3 && i >= 0;
            i += step)
        {
            if (i != location.y)
                for (int j = 0; j < 3; j++)
                {
                    //检测当前位置有没有东西
                    if (!battleData.hasCharacterInGrid(enemyGroup, i, j))
                        continue;
                    enemy = enemies[enemyGroup, i, j];
                    if (enemy._hp > 0)
                    {
                        list.Add(enemy);
                        return list;
                    }
                }
        }

        return null;
    }

    //治疗
    public virtual void heal()
    {
    }

    /*
     *    以下为不会被重载的方法
     */

    public Vector3Int Get_location()
    {
        return this.location;
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
    public bool Count_critic()
    {
        if (new Random().NextDouble() <= _critic / 100)
        {
            _msg.Add(1);
            return true;
        }

        _msg.Add(0);
        return false;
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

    public int GetId()
    {
        return id;
    }

    public double GetHp()
    {
        return _hp;
    }

    public void SetLocation(Vector3Int location)
    {
        this.location = location;
    }

    //暂时用于演示demo，后续加细节
    //调用时传入攻击目标，用于后期实现更多动画
    IEnumerator Attack_cartoon(List<Character> target)
    {
        this.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        yield return new WaitForSeconds(0.3f);
        this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        yield return new WaitForSeconds(0.1f);
    }

    //调用
    public void Start_Attack_cartoon(List<Character> target)
    {
        StartCoroutine(Attack_cartoon(target));
    }

    //暂时用于演示demo，后续加细节
    //调用时传入攻击目标，用于后期实现更多动画
    IEnumerator Skill_cartoon(List<Character> target)
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255, 1f);
        this.gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        yield return new WaitForSeconds(0.3f);
        this.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
        yield return new WaitForSeconds(0.1f);
    }

    //调用
    public void Start_Skill_cartoon(List<Character> target)
    {
        StartCoroutine(Skill_cartoon(target));
    }

    IEnumerator Defense_cartoon()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1f);
        yield return new WaitForSeconds(0.3f);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1f);
        yield return new WaitForSeconds(0.1f);
    }

    //调用
    public void Start_Defense_cartoon()
    {
        StartCoroutine(Defense_cartoon());
    }

    IEnumerator Die_cartoon()
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }

    public void Start_Die_cartoon()
    {
        StartCoroutine(Die_cartoon());
    }
}