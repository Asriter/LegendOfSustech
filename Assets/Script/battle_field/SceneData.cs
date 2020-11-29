using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
*该class只用于存储玩家所选择的阵容，以及从网络获得的对方阵容，还有服务器下发的战斗数据
*
*
*/
public class SceneData : MonoBehaviour
{
    private List<int[]> myList;
    private List<int[]> opList;

    private battle_data bd;

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

    //初始化bd
    public void initialBattleData(List<List<int>> battleMassage)
    {
        bd = new battle_data(myList, opList, battleMassage);
    }

    //标记不会在切换场景时销毁
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }
}
