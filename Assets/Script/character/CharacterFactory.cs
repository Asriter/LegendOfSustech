//工厂方法，用于根据输入的id创建character
using UnityEngine;
class CharacterFactory
{
    public static Character CreateCharacter(int id)
    {
        Character c = null;
        switch(id)
        {
            case 1: c = new MvpTemp();    break;
            case 2: c = new TankTemp();   break;
            case 3: c = new WarriorTemp();break;
            case 4: c = new Taunter();    break; // 嘲讽盾
            case 5: c = new Paladin();    break; // 奶骑
            case 6:                       break; // 反甲
            case 7: c = new Explosive();  break; // 自爆卡车
            case 8: c = new Linkage();    break; // 联动
            case 9:                       break; // 打太极
            case 10:                      break; // 沉默
            case 11:                      break; // 群体治疗
            case 12:                      break; // 单体治疗
            case 13:                      break; // 操控怒气
            case 14:                      break; // 频繁减怒
            case 15:                      break; // 加buff
            case 16:                      break; // 单体输出
            case 17:                      break; // 溅射
            case 18:                      break; // 越打越痛
            case 19:                      break; // 变身
            case 20:                      break; // 内鬼
            case 21:                      break; // 激光豆
            case 22:                      break; // 人头狗
            case 23:                      break; // 投石机
            case 24:                      break; // 黑胡子

            default: Debug.Log("输入了错误的id，找不到对应单位"); break;
        }

        return c;
    }
}