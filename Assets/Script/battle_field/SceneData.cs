using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
/*
*该class只用于存储玩家所选择的阵容，以及从网络获得的对方阵容，还有服务器下发的战斗数据
*同时用于储存玩家信息，包括用户名，可能出现的等级，经验值，头像等
*
*/
public class SceneData : MonoBehaviour
{
    private List<int[]> myList;
    private List<int[]> opList;

    private battle_data bd;

    private string _user_name;

    private int _level;

    private List<int[]> _canSetCharacter;   //该玩家拥有的角色，里面的int【】为长度为2的数组。分别为id和等级

    private int _totalCost = 20;            //该玩家在该等级拥有的cost

    public Socket socket;

    public bool isReEmbattle = false;

    //getset
    public List<int[]> MyList
    {
        get => myList;
        set => myList = value;
    }
    public List<int[]> OpList
    {
        get => opList;
        set => opList = value;
    }
    public battle_data BattleData => bd;

    public string UserName
    {
        get => _user_name;
        set => _user_name = value;
    }

    public int Level
    {
        get => _level;
        set => _level = value;
    }

    public List<int[]> CanSetCharacter
    {
        get => _canSetCharacter;
        set => _canSetCharacter = value;
    }

    public int COST => _totalCost;

    //初始化bd
    public void initialBattleData(List<List<int>> battleMassage)
    {
        bd = new battle_data(myList, opList, battleMassage);
    }

    //标记不会在切换场景时销毁
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        this.isReEmbattle = false;
    }
}
