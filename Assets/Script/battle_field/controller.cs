using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//主控程序,控制战场的进行
//TODO
public class controller : MonoBehaviour
{
    //通过control.Instance 直接调用这个自动实例化好了的class中的内容
    private static controller instance;
    public static controller Instance
    {
        get
        {
            if(instance == null)
                instance = GameObject.Find("Controller").GetComponent<controller>();
                
            return instance;
        }
    }

    //该战斗的数据，包括动作和伤害
    private battle_data battleData;

    /*场景中那九个格子
     *第一个变量为0和1，0为我方，1为对方
     *第二个变量为x坐标,从左到右
     *第三个坐标为y坐标，中间是0，两边是2
     */

    [SerializeField] List<GridUnit> gridUnits;
    //GridUnit[,,] gridUnits = new GridUnit[2,3,3];
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //初始化controller
    public void Initial()//battle_data battleData)
    {
        //清空gridunits中所有格子里的单位
        for(int i = 0; i < 2; i++)
        {
            for(int x = 0; x < 3; x++)
            {
                for(int y = 0; y < 3; y++)
                {
                    gridUnits[i*9 + x*3 + y].initial(new Vector3Int(i, x, y));
                }
            }
        }

        //this.battleData = battleData;

        //开始往格子里添加单位
        //TODO

        //开始执行动画,可能会用协程来完成
        //TODO
    }

    //通过坐标获取对应grid
    public GridUnit GetGridUnitByVector3(Vector3Int location)
    {
        int index = location.x*9 + location.y*3 + location.z;
        return gridUnits[index];
    }
}
