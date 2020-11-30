
public class Buff
{
    public BuffKind _buffKind;
    private double _amount; //效果量：数值或者百分比
    public boolean _percent; //根据此boolean值确定buff是数值buff还是百分比buff
    public int _remain;

    //buff的计算方法
    public double Count(double num)
    {
        if (_percent) //判断是数值还是百分比buff
        {
            return num * (1 + _amount / 100); //若百分比buff的_amount为15，则表示数值+15%
        }
        return num + _amount; //若数值buff的_amount为15，则表示数值+15
    }

    public Buff(BuffKind buffKind, double amount, boolean percent, int remain)
    {
        _buffKind = buffKind;
        _amount = amount;
        _percent = percent;
        _remain = remain;
    }
}

//TODO： 更多的buff……