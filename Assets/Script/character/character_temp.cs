using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有角色的父类
public class character_temp : MonoBehaviour
{
    //属性
    private double _HP;
    private double _ATK;
    //buff用于记录状态，每一格代表了一个属性的改变，对应参数在constant中，但尚且未设定
    private double[] buff;
    //待添加
    //TODO

    //方法

    //用于通常攻击，在攻击完成之后返回状态值，让主程序继续运行
    public int attack(){
        //TODO
        return 1;
    }

    //释放技能，该部分根据各个单位的子类具体实现
    public int skill(){
        //TODO
        return 1;
    }

    //退场
    public void die(){
        //TODO
    }

    //调用受攻击动画
    public void attacked(){
        //TODO
    }

}
