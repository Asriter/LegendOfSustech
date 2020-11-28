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
            if (instance == null)
                instance = GameObject.Find("Controller").GetComponent<controller>();

            return instance;
        }
    }

    //该战斗的数据，包括动作和伤害
    public battle_data battleData;

    /*场景中那九个格子
     *第一个变量为0和1，0为我方，1为对方
     *第二个变量为x坐标,从左到右
     *第三个坐标为y坐标，中间是0，两边是2
     */

    [SerializeField] List<GridUnit> gridUnits;

    //GridUnit[,,] gridUnits = new GridUnit[2,3,3];
    [SerializeField] Transform CharacterTransform;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //初始化controller
    public void Initial(battle_data battleData) //battle_data battleData)
    {
        //清空gridunits中所有格子里的单位
        for (int i = 0; i < 2; i++)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    gridUnits[i * 9 + x * 3 + y].initial(new Vector3Int(i, x, y));
                }
            }
        }

        this.battleData = battleData;

        //把单位放到格子里面
        loadPrefab();
        SetPrefabsActive();
    }

    //根据list中的单位，加载prefab同时加载到格子里面
    //最好直接把prefab中的脚本换掉
    private void loadPrefab()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    //该位置的单位不为空
                    if (battleData.hasCharacterInGrid(i, x, y))
                    {
                        int id = battleData.GetCharacterList()[i, x, y].GetId();
                        //加载prefab
                        GameObject obj = Resources.Load("Prefabs/character/" + id) as GameObject;
                        obj = Instantiate(obj);
                        //Debug.Log(obj.GetComponent<Character>().GetInstanceID());
                        obj.SetActive(false);
                        GridUnit grid = GetGridUnitByVector3(new Vector3Int(i, x, y)); //获取对应格子
                        obj.transform.SetParent(CharacterTransform); //设置位置
                        //设置位置测试
                        obj.transform.position = grid.gameObject.transform.position;

                        //重置list中的character
                        battleData.GetCharacterList()[i, x, y] = obj.GetComponent<Character>();
                        //往格子里加单位
                        grid.SetCharacter(battleData.GetCharacterList()[i, x, y]);
                        //设置character中自己的坐标
                        battleData.GetCharacterList()[i, x, y].SetLocation(new Vector3Int(i, x, y));
                    }
                }
            }
        }
    }

    //添加调用prefab
    private void SetPrefabsActive()
    {
        //显示
        for (int i = 0; i < 2; i++)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    //该位置的单位不为空
                    if (battleData.hasCharacterInGrid(i, x, y))
                    {
                        battleData.GetCharacterList()[i, x, y].gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void Start_Animation()
    {
        StartCoroutine(Animation());
    }

    //调用协程开始动画
    IEnumerator Animation()
    {
        Character[,,] characterList = battleData.GetCharacterList();
        yield return new WaitForSeconds(0.5f);
        Debug.Log(battleData.GetBattleData().Count);
        foreach (List<int> list in battleData.GetBattleData())
        {
            //Debug.Log("该条战斗信息:" + list[0] + " " + list[1] + " " + list[2] + " " + list[3] + " " + list[4]);

            //是否使用技能
            bool isSkill = list[3] == 1 ? true : false;

            //获取攻击目标
            List<Character> targets = characterList[list[0], list[1], list[2]].Get_target(isSkill);

            //播放攻击动画
            if (isSkill)
                characterList[list[0], list[1], list[2]].Start_Skill_cartoon(targets);
            else
                characterList[list[0], list[1], list[2]].Start_Attack_cartoon(targets);
            yield return new WaitForSeconds(0.5f);

            //播放防御动画
            foreach (Character c in targets)
            {
                c.Start_Defense_cartoon();
            }

            yield return new WaitForSeconds(0.5f);

            //攻击部分
            //是否暴击
            bool isCritic = list[4] == 1 ? true : false;
            //攻击类型，技能还是平A
            //血条动画在character的defence中实现
            if (list[3] == 0) //A
            {
                characterList[list[0], list[1], list[2]].Attack(isCritic);
            }
            else
            {
                //skill
                characterList[list[0], list[1], list[2]].Skill(isCritic);
            }

            //检测死亡，播放动画
            bool hasDie = false;
            for (int i = 0; i < 2; i++)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        //该位置的单位不为空
                        if (battleData.hasCharacterInGrid(i, x, y))
                        {
                            if (characterList[i, x, y].GetHp() <= 0 && characterList[i, x, y].gameObject.activeSelf)
                            {
                                characterList[i, x, y].Start_Die_cartoon();
                                hasDie = true;
                            }
                        }
                    }
                }
            }

            //有人死和没人死暂停的时间不同
            if (hasDie)
                yield return new WaitForSeconds(0.4f);
            else
                yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0f);
    }

    //通过坐标获取对应grid
    public GridUnit GetGridUnitByVector3(Vector3Int location)
    {
        int index = location.x * 9 + location.y * 3 + location.z;
        return gridUnits[index];
    }
}