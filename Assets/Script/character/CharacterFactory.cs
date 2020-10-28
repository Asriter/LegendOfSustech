//工厂方法，用于根据输入的id创建character
using UnityEngine;
class CharacterFactory
{
    public static Character CreateCharacter(int id)
    {
        Character c = null;
        switch(id)
        {
            case 1: c = new MvpTemp();break;
            case 2: c = new TankTemp();break;
            case 3: c = new WarriorTemp();break;
            default: Debug.Log("输入了错误的id，找不到对应单位"); break;
        }

        return c;
    }
}