
//一次答辩用，输出
public class MvpTemp extends Character
{
    public MvpTemp()
    {
    	super(2600, 3000, 15, 5, 300, 1);
        this.id = 1;
    }

    public int Skill(boolean isCritic)
    {
        double atk = Count_atk();
        double damage = Count_damage(2 * atk);
        //Debug.Log("使用技能");
        if(isCritic){
            //Debug.Log("使用技能且暴击");
            damage *= 2;
        }
        Get_target(true).get(0).Defense(damage);
        return super.Skill(isCritic);
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