using System.Collections.Generic;

public class TaiChi : Character //打太极
{
    private bool _taiChi = false;

    //被动: 普攻获得怒气为100
    public TaiChi() : base(4000, 1200, 40, 10, 250, 1)
    {
        this.id = 9;
        _atkMp = 100;
    }

    //大招:进行一次攻击并获得状态“下次受伤的伤害量转为治疗量”
    public override int Skill(bool isCritic)
    {
        Attack(Count_critic());
        _taiChi = true;
        return 1;
    }
    
    //防御时检测是否在太极状态，否则正常受伤，是则获得治疗
    public override void Defense(double damage)
    {
        if (!_taiChi)
            base.Defense(damage);
        else
        {
            _taiChi = false;
            Get_heal(damage);
        }
    }
}