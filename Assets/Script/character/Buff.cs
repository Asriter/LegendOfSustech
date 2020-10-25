//Buff的class
public abstract class Buff
{
    public BuffKind _buffKind;
    public double _effect; //效果量：数值或者百分比
    public int _remain;

    public abstract double Count(double num);

    public Buff(BuffKind buffKind, double effect, int remain)
    {
        _buffKind = buffKind;
        _effect = effect;
        _remain = remain;
    }
}

public class AmountBuff : Buff //数值类buff
{
    public AmountBuff(BuffKind buffKind, double effect, int remain) : base(buffKind, effect, remain)
    {
    }

    public override double Count(double num)
    {
        //TODO: 计算数值
        return num;
    }
}

public class PercantageBuff : Buff //百分比buff
{
    public PercantageBuff(BuffKind buffKind, double effect, int remain) : base(buffKind, effect, remain)
    {
    }

    public override double Count(double num)
    {
        //TODO: 计算数值
        return num;
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
//TODO： 更多的buff……