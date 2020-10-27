using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于储存战场中的信息，包括单位，双方玩家的id，格子信息等，具体加载战场也是在这里
//TODO
public class battle_data
{
    //应该加载的所有角色
    //第一个参数是敌我，0是我1是敌
    //第二个是x坐标，从左到右递增0-2
    //第三个是y坐标，从里到外0-2
    private Character[,,] characterList;

    public List<int[]> battleData;
    
    //传来的参数为两个List，每个List中都是长度为7的int数组
    //id, x, y, hp, atk, def, cri
    public battle_data(List<int[]> myCharacterList, List<int[]> opponentCharacterList)
    {
        //初始化list
        characterList = new Character[2,3,3];
        
        //读取数据并加载到list中
        foreach(int[] property in myCharacterList)
        {
            Character c = CharacterFactory.CreateCharacter(property[0]);
            //暂时不写属性操控方法
            characterList[0, property[1], property[2]] = c;
        }
        foreach(int[] property in opponentCharacterList)
        {
            Character c = CharacterFactory.CreateCharacter(property[0]);
            //暂时不写属性操控方法
            characterList[1, property[1], property[2]] = c;
        }

        
    }

    private List<int[]> GetBattleData()
    {
        battleData = new List<int[]>();

        //TODO模拟战斗，得到battledata

        return battleData;
    }

    //将单位的Perfber添加到对应格子中并显示出来
    public void showCharacter()
    {
        //TODO
    }
}
