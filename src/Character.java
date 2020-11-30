import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;
import java.util.Random;

//所有角色的父类，是个抽象类
public abstract class Character
{
    //属性
    public double _maxHp; //最大生命值
    public double _hp; //当前生命
    public int _mp = 50; //蓝量，满蓝放技能
    public double _atk; //攻击力
    public double _def; //防御
    public double _critic; //暴击率，5表示5%暴击率
    private ArrayList<Buff> _buffs; //rt

    protected int _atkMp = 50; //普攻增加的怒气
    protected int _skillMp = 100; //大招所需怒气

    public double _speed; //速度
    public int _cost; //费用

    private ArrayList<Integer> _msg; //报文
    private int[] location; //在那个格子里
    protected int id; //用于实例化之后的单位，通过id获取对应对象
    private Character[][][] enemies;//所有角色列表，记得加一个set方法，在battle_data初始化最后加进来.

    //TODO在调用action的时候传入，可能加入set
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
        _buffs = new ArrayList<Buff>();
        _msg = new ArrayList<Integer>();
        _cost = cost;
    }
    
    public void setEnemies(Character[][][] enemies) {
    	this.enemies = enemies;
    }

    /*
     *    以下为能够被重载的方法
     */

    //行动，轮到角色行动时调用此方法
    public ArrayList<Integer> Action(battle_data battleData)
    {
        int[] location = Get_location();
        this.battleData = battleData;
        _msg = new ArrayList<Integer>();
        _msg.clear();
        _msg.add(location[0]);
        _msg.add(location[1]);
        _msg.add(location[2]);

        //检测沉默
        boolean silent = false;
        for(Buff buff : _buffs)
        {
            if (buff._buffKind == BuffKind.Silence)
            {
                silent = true;
                break;
            }
        }

        if (_mp < _skillMp || silent) //怒气小于100或沉默状态下普攻，否则skill
        {
            _msg.add(0);
            Attack(Count_critic());
        }
        else
        {
            _msg.add(1);
            Skill(Count_critic());
        }

        Check_buff_remain(); //减少buff持续时间1回合
        return _msg;
    }

    //用于通常攻击，在攻击完成之后返回状态值，让主程序继续运行
    //将返回状态值修改成返回伤害
    public double Attack(boolean isCritic)
    {
        Modify_mp(_atkMp);
        double atk = Count_atk();
        double damage = Count_damage(atk);
        
        //是否暴击
        if (isCritic)
        {
            damage *= 2;
            //Debug.Log("发生暴击");
        }

        Get_target(false).get(0).Defense(damage);
        return damage;
    }

    //受攻击，调用动画、效果等
    public void Defense(double damage)
    {
        _hp = Math.max(0, _hp - Count_Hurt(damage));
        //Debug.Log("血量：" + _hp + " 伤害：" + Count_Hurt(damage));
        if (_hp == 0)
        {
            Die();
        }
    }
    
    //获取攻击目标
    //一般来说isskill没什么用，但如果技能和平A攻击范围不同时有用
    //面对范围攻击的情况，此处返回值修改为list
    public ArrayList<Character> Get_target(boolean skill)
    {
        ArrayList<Character> list = new ArrayList<Character>();

        Character enemy;
        int enemyGroup = location[0] == 0 ? 1 : 0;

        //检测嘲讽
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!battleData.hasCharacterInGrid(enemyGroup, i, j))
                    continue;
                enemy = enemies[enemyGroup][i][j];
                if (enemy._hp > 0)
                {
                    //检测嘲讽
                    for(Buff buff : enemy._buffs)
                    {
                        if (buff._buffKind == BuffKind.Taunt)
                        {
                            list.add(enemy);
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
            if (!battleData.hasCharacterInGrid(enemyGroup, location[2], i))
                continue;
            enemy = enemies[enemyGroup][location[2]][i];
            if (enemy._hp > 0)
            {
                list.add(enemy);
                return list;
            }
        }

        //没有本列的，优先打近的那一列
        for (int i = (location[2] == 2 ? 2 : 0), step = (location[2] == 2 ? -1 : 1);
            i < 3 && i >= 0;
            i += step)
        {
            if (i != location[2])
                for (int j = 0; j < 3; j++)
                {
                    //检测当前位置有没有东西
                    if (!battleData.hasCharacterInGrid(enemyGroup, i, j))
                        continue;
                    enemy = enemies[enemyGroup][i][j];
                    if (enemy._hp > 0)
                    {
                        list.add(enemy);
                        return list;
                    }
                }
        }

        return null;
    }

    //治疗
    public double Heal(double amount)
    {
        return Buff_affect(amount, BuffKind.Heal);
    }

    /*
     *    以下为不会被重载的方法
     */

    public int[] Get_location()
    {
        return this.location;
    }

    //调整怒气值，参数不为0时怒气加上参数，参数为0时怒气归零
    public void Modify_mp(int amount)
    {
        if (amount != 0)
        {
            _mp += amount;
            _mp = Math.min(100, _mp); //怒气最高为100
            _mp = Math.max(0, _mp); //怒气最低为0
        }
        else
            _mp = 0;
    }

    //回合结束用于减少buff持续时间，移除时间结束的buff
    private void Check_buff_remain()
    {
        for (int i = _buffs.size() - 1; i >= 0; i--)
        {
            Buff buff = _buffs.get(i);
            buff._remain -= 1; //持续时间减一
            if (buff._remain <= 0) //时间为0则移除
            {
                _buffs.remove(buff);
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
    public boolean Count_critic()
    {
        if (new Random().nextDouble() <= _critic / 100)
        {
            _msg.add(1);
            return true;
        }

        _msg.add(0);
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
        def = Math.min(def, 75); //无论怎么加，防御最高为75 （即只受四分之一的伤害）
        return def;
    }

    //获得治疗
    public void Get_heal(double heal)
    {
        heal = Buff_affect(heal, BuffKind.GainHeal); //根据buff增减获得治疗量
        _hp = Math.min(_maxHp, _hp + heal); //当前hp不会超过maxHp
    }

    //得别人给的buff
    public void Get_buff(Buff buff)
    {
        _buffs.add(buff);
       // _buffs.Sort((Buff a, Buff b) //得到buff后将buff表进行排序
            //=> a._percent == b._percent ? 0 : (a._percent ? 1 : -1)); //将百分比buff排在后面，数值buff排在前面
        Collections.sort(_buffs, new Comparator<Buff>(){

			@Override
			public int compare(Buff a, Buff b) {
				int result = a._percent == b._percent ? 0 : (a._percent ? 1 : -1);
				return result;
			}
        	
        });
    }

    //计算数值受所有buff影响后的数值
    private double Buff_affect(double num, BuffKind buffKind)
    {
        for (int i = 0, len = _buffs.size(); i < len; i++)
        {
            if (_buffs.get(i)._buffKind == buffKind) //判断是否为增加获得治疗量的buff
            {
                num = _buffs.get(i).Count(num); //若是，则根据buff增减治疗量
            }
        } //由于排序时百分比buff在后面，数值buff在前面，所以遍历时会先加数值再乘百分比

        return num;
    }

    //释放技能，该部分根据各个单位的子类具体实现
    public int Skill(boolean isCritic)
    {
        Modify_mp(0); //skill后怒气归零
        return 1;
    }

    public int GetId()
    {
        return id;
    }

    public double GetHp()
    {
        return _hp;
    }
    
    public void SetLocation(int[] location)
    {
        this.location = location;
    }

    //退场动画，效果等
    protected void Die()
    {
    }
}