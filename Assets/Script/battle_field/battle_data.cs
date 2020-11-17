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

    //处于不知道的什么原因，工厂方法返回的class显示也是null
    //因此额外引入一个list储存该位置有没有单位
    private int[,,] characterListRemember;

    //用于记录在寻找可动角色时是否有找到
    private bool flag;

    private List<List<int>> battleData;

    private bool isWin;

    //传来的参数为两个List，每个List中都是长度为7的int数组
    //id, x, y, hp, atk, def, cri
    public battle_data(List<int[]> myCharacterList, List<int[]> opponentCharacterList)
    {
        //初始化list
        characterList = new Character[2, 3, 3];
        characterListRemember = new int[2, 3, 3];

        //先定义两个list用来装双方的单位，方便遍历
        List<Character> myList = new List<Character>();
        List<Character> opList = new List<Character>();

        //读取数据并加载到list中
        foreach (int[] property in myCharacterList)
        {
            characterList[0, property[1], property[2]] = CharacterFactory.CreateCharacter(property[0]);
            //暂时不写属性操控方法

            //在remember中记录
            characterListRemember[0, property[1], property[2]] = 1;
            //在单位中加入所在的location
            characterList[0, property[1], property[2]].SetLocation(new Vector3Int(0, property[1], property[2]));

            //加入该方法的list
            myList.Add(characterList[0, property[1], property[2]]);

        }
        foreach (int[] property in opponentCharacterList)
        {
            characterList[1, property[1], property[2]] = CharacterFactory.CreateCharacter(property[0]);
            //暂时不写属性操控方法

            //在remember中记录
            characterListRemember[1, property[1], property[2]] = 1;
            //在单位中加入所在的location
            characterList[1, property[1], property[2]].SetLocation(new Vector3Int(1, property[1], property[2]));
            //加入该方法的list
            opList.Add(characterList[1, property[1], property[2]]);
        }

        //测试用
        //this.battleData = GenerateBattleData();
        this.battleData = GenerateBattleData(myList, opList);
    }

    //尝试另一种实现方式，生成battleData
    private List<List<int>> GenerateBattleData(List<Character> myList, List<Character> opList)
    {
        battleData = new List<List<int>>();

        //根据速度得到行动顺序，0是动，1是对方动
        //每次得到data之后都会取反
        int isAttack = GetSpeed();

        int[] myIndex = { -1 };
        int[] opIndex = { -1 };

        //模拟战斗，得到battledata
        while (true)
        {
            Character atkCharacter = null;
            //我方动
            if (isAttack == 0)
            {
                atkCharacter = FindActionCharacter(myList, myIndex);
                isAttack = 1;
            }
            //对方动
            else
            {
                atkCharacter = FindActionCharacter(opList, opIndex);
                isAttack = 0;
            }

            if (!flag)
            {
                //Debug.Log("ListLength: " + battleData.Count);
                return battleData;
            }

            //没打完
            List<int> list = atkCharacter.Action(this);
            //Debug.Log(list);
            battleData.Add(list);
        }
    }

    private List<List<int>> GenerateBattleData()
    {
        battleData = new List<List<int>>();

        //根据速度得到行动顺序，0是动，1是对方动
        //每次得到data之后都会取反
        int isAttack = GetSpeed();

        //下面两个变量记录双方当前行动单位的坐标
        int[] myActionNum = { -1, 0 };
        int[] opponentActionNum = { -1, 0 };

        //模拟战斗，得到battledata
        while (true)
        {
            int[] myIndex = { -1 };
            int[] opIndex = { -1 };
            Character atkCharacter = null;
            //我方动
            if (isAttack == 0)
            {
                atkCharacter = FindActionCharacter(isAttack, myActionNum);
                isAttack = 1;
            }
            //对方动
            else
            {
                atkCharacter = FindActionCharacter(isAttack, opponentActionNum);
                isAttack = 0;
            }

            if (!flag)
            {
                //Debug.Log("ListLength: " + battleData.Count);
                return battleData;
            }

            //没打完
            List<int> list = atkCharacter.Action(this);
            //Debug.Log(list);
            battleData.Add(list);
        }
    }

    //尝试另一种实现方式
    private Character FindActionCharacter(List<Character> list, int[] index)
    {
        flag = false;

        for (int i = index[0] + 1; i < list.Count; i++)
        {
            if (list[i].GetHp() <= 0)
                continue;

            flag = true;
            index[0] = i;
            return list[i];
        }

        for (int i = 0; i <= index[0]; i++)
        {
            if (list[i].GetHp() <= 0)
                continue;

            flag = true;
            index[0] = i;
            return list[i];
        }
        return null;
    }

    //获取双方速度来决定谁先动，return 0我先动，1对方先动
    private int GetSpeed()
    {
        int count1 = 0;
        int count2 = 0;
        double mySpeed = 0;
        double opSpeed = 0;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (this.hasCharacterInGrid(0, x, y))
                {
                    count1 += 1;
                    mySpeed += characterList[0, x, y]._speed;
                }
                if (this.hasCharacterInGrid(1, x, y))
                {
                    count2 += 1;
                    opSpeed += characterList[1, x, y]._speed;
                }
            }
        }
        mySpeed = mySpeed / count1;
        opSpeed = opSpeed / count2;

        return mySpeed >= opSpeed ? 0 : 1;
    }

    //获取下一个可以攻击的对象，如果为null则说明死完了
    private Character FindActionCharacter(int isAttack, int[] ActionNum)
    {
        Character attackCharacter = null;
        flag = false;
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (y * 3 + x <= ActionNum[0] + ActionNum[1] * 3)
                    continue;
                //找到在当前位置之后且不为空的对象
                if (this.hasCharacterInGrid(isAttack, x, y))
                {
                    if (characterList[isAttack, x, y].GetHp() <= 0)
                        continue;

                    flag = true;
                    ActionNum[0] = x;
                    ActionNum[1] = y;
                    return characterList[isAttack, x, y];
                }
            }
        }

        if (!flag)
        {
            //ActionNum[0] = -1;
            //ActionNum[1] = 0;

            //从头开始重跑一边
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (y * 3 + x > ActionNum[0] + ActionNum[1] * 3)
                        break;
                    //找到在当前位置之后且不为空的对象
                    if (this.hasCharacterInGrid(isAttack, x, y))
                    {
                        if (characterList[isAttack, x, y].GetHp() <= 0)
                            continue;

                        flag = true;
                        ActionNum[0] = x;
                        ActionNum[1] = y;
                        return characterList[isAttack, x, y];
                    }
                }
            }
        }

        flag = false;
        return attackCharacter;
    }

    public Character[,,] GetCharacterList()
    {
        return characterList;
    }

    public List<List<int>> GetBattleData()
    {
        return this.battleData;
    }

    public bool hasCharacterInGrid(int x, int y, int z)
    {
        return characterListRemember[x, y, z] == 1;
    }
}
