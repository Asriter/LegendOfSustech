using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于储存战场中的信息，包括单位，双方玩家的id，格子信息等，具体加载战场也是在这里
//TODO
public class battle_data
{
    //应该加载的所有角色
    private List<Character> characterList;
    //对应角色在战场中的位置，Vector3的3个变量分别为敌我，x坐标，y坐标
    //x坐标从左到右
    //y坐标从中间到两边
    private List<Vector3Int> characterLocation;
    
    //未确定发来的信息的形式，故此处先没有传参
    public battle_data()
    {
        //初始化list
        characterList = new List<Character>();
        characterLocation = new List<Vector3Int>();
        
        //读取数据并将prefab加载到list中
        //TODO
    }

    //将单位的Perfber添加到对应格子中并显示出来
    public void showCharacter()
    {
        //TODO
    }
}
