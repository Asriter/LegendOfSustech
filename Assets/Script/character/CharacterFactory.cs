//工厂方法，用于根据输入的id创建character
using UnityEngine;
class CharacterFactory
{
    public static Character CreateCharacter(int id)
    {
        switch(id)
        {
            case 1: return new MvpTemp();
            case 2: return new TankTemp();
            case 3: return new WarriorTemp();
            default: Debug.Log("输入了错误的id，找不到对应单位"); return null;
        }
    }
}