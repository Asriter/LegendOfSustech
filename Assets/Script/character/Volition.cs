public class Volition : Character //越打越痛 bug 大招如果杀一个打另一个，第二个的血条不会有变动
{
    public Volition() : base(2600, 2000, 10, 15, 300, 1)
    {
        id = 18;
    }

    //被动：普攻后获得10%攻击力加成，持续整局
    public override double Attack(bool isCritic)
    {
        double damage = base.Attack(isCritic);
        Get_buff(new Buff(BuffKind.Atk, 10, true, 999));
        return damage;
    }

    //大招：进行两次普攻（完成后才执行skill的清怒气）
    public override int Skill(bool isCritic)
    {
        Attack(isCritic);
        Attack(isCritic);
        return base.Skill(isCritic);
    }
}