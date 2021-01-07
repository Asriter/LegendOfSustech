using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Embattle : UIViewTemplate
{
    //[SerializeField] Canvas canvas;
    [SerializeField] Button Return;
    [SerializeField] List<Button> tile;
    SceneData sceneData;
    [SerializeField] UI_CharacterButtom characterButtomModel;//下面菜单里prefab的本体
    [SerializeField] private Transform buttonRoot;//菜单里单位的父节点
    [SerializeField] Text nowCostText;
    [SerializeField] Text totalCostText;
    private UI_CharacterButtom[] characterButtoms = new UI_CharacterButtom[20];//下面菜单

    private List<int[]> myList;//我的阵营

    private int[,] location = new int[3, 3];    //用来存该单位放置的单位类型
    private int[,] level = new int[3, 3];    //用来存该单位放置的等级
    private int _cost;      //当前玩家等级拥有的cost，从SceneData中获得
    private int _totalCost;

    private bool isStart = false;

    private int[] nextCharacter = new int[2] { -1, -1 };   //0-》类型， 1-》等级

    //暂时还没加入翻页的功能，也不好分辨那个单位等级为多少

    //判断短时间内不会点击两次
    DateTime nowTime;
    bool isDoubleClick = false;

    public override void initial(List<UIViewTemplate> list)
    {
        base.initial(list);

        //添加监听器
        tile[0].onClick.AddListener(() => setBtnTile(0, 0));
        tile[1].onClick.AddListener(() => setBtnTile(0, 1));
        tile[2].onClick.AddListener(() => setBtnTile(0, 2));
        tile[3].onClick.AddListener(() => setBtnTile(1, 0));
        tile[4].onClick.AddListener(() => setBtnTile(1, 1));
        tile[5].onClick.AddListener(() => setBtnTile(1, 2));
        tile[6].onClick.AddListener(() => setBtnTile(2, 0));
        tile[7].onClick.AddListener(() => setBtnTile(2, 1));
        tile[8].onClick.AddListener(() => setBtnTile(2, 2));

        //返回添加监听器
        Return.onClick.AddListener(setBtnReturn);

        sceneData = GameObject.Find("SceneData").GetComponent<SceneData>();

        //test!!!!!
        //this.OnShow();
    }

    public override void OnShow()
    {
        base.OnShow();
        _cost = GameObject.Find("SceneData").GetComponent<SceneData>().COST;
        _totalCost = _cost;
        int length = GameObject.Find("SceneData").GetComponent<SceneData>().CanSetCharacter.Count;
        characterButtoms = new UI_CharacterButtom[20];

        //初始化下方的角色菜单
        for (int i = 0; i < 20; i++)
        {
            //读取prefab
            UI_CharacterButtom cb = Instantiate<UI_CharacterButtom>(characterButtomModel);
            cb.transform.SetParent(buttonRoot);
            cb.transform.localPosition = Vector3.zero;
            cb.transform.localScale = Vector3.one;
            cb.transform.localRotation = Quaternion.identity;
            //Debug.Log(i);
            characterButtoms[i] = cb;
            if (i < length)
            {
                cb.initial(sceneData.CanSetCharacter[i][0], sceneData.CanSetCharacter[i][1]);
                //cb.initial(sceneData.CanSetCharacter[i][0], sceneData.CanSetCharacter[i][1]);
            }
        }
        isStart = true;
    }

    //重置所有buttom的颜色
    public void resetAllColor()
    {
        foreach (UI_CharacterButtom cb in characterButtoms)
        {
            cb.resetColor();
        }
    }

    public override void OnHide()
    {
        RemoveAllChildren(buttonRoot.gameObject);
        base.OnHide();
    }

    private static void RemoveAllChildren(GameObject parent)
    {
        Transform transform;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            transform = parent.transform.GetChild(i);
            GameObject.Destroy(transform.gameObject);
        }
    }

    //监听器
    private void setBtnTile(int x, int y)
    {
        //判断是不是重复点击
        if (isDoubleClick)
            return;

        //先判断是不是第二次点击，在next为空的时候再一次点击该按钮，如果是则删掉该位置的单位
        if (this.nextCharacter[0] == -1 && location[x, y] != 0)
        {
            resetImage(x, y);
            _cost += CharacterFactory.CreateCharacter(location[x, y])._cost;
            location[x, y] = 0;
            level[x, y] = 0;

            //记录当前时间
            nowTime = DateTime.Now;
            isDoubleClick = true;
        }

        if (this.nextCharacter[0] != -1)
        {
            //调整cost
            int cost = CharacterFactory.CreateCharacter(nextCharacter[0])._cost;
            if (cost > _cost)
            {
                UnityEditor.EditorUtility.DisplayDialog("无法加入", "cost不足", "确认");
                return;
            }
            _cost -= cost;

            //在对应位置放入单位和等级
            location[x, y] = nextCharacter[0];
            level[x, y] = nextCharacter[1];

            //记录图标
            setImage(location[x, y], x, y);

            //清除状态
            nextCharacter[0] = -1;
            nextCharacter[1] = -1;
            resetAllColor();

            //记录当前时间
            nowTime = DateTime.Now;
            isDoubleClick = true;
        }
    }

    //返回的监听器
    private void setBtnReturn()
    {
        myList = new List<int[]>();
        //加载布阵资料到SceneData中
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (location[i, j] != 0)
                {
                    int[] ints = new int[7];
                    ints[0] = location[i, j];
                    ints[1] = j;
                    ints[2] = i;
                    myList.Add(ints);
                }
            }
        }

        if (myList.Count == 0)
        {
            UnityEditor.EditorUtility.DisplayDialog("", "阵容中没有character", "确认");
        }

        //UI_Controller.Instance.depth -= 1;
        //看看list里单位的位置和类型
        /*foreach(int[] i in myList)
        {
            Debug.Log(i[0] + " " + i[1] + " " + i[2]);
        }*/
        this.OnHide();
        isStart = false;
        sceneData.MyList = myList;

        //试试看在这里直接开打？
        /*sceneData.MyList = myList;
        SceneManager.LoadScene("BattleField");*/
    }

    //根据单位修改图片
    private void setImage(int id, int x, int y)
    {
        string path = "Picture/Character/" + id;
        object obj = Resources.Load(path, typeof(Sprite));
        Sprite sp = obj as Sprite;
        tile[x * 3 + y].gameObject.GetComponent<Image>().sprite = sp;
    }

    //取消该位置的单位
    private void resetImage(int x, int y)
    {
        tile[x * 3 + y].gameObject.GetComponent<Image>().sprite = null;
    }

    //get,set
    /*public int[] NextCharacter
    {
        get => nextCharacter;
        set => NextCharacter = value;
    }*/

    public void setNextCharacter(int x, int y)
    {
        nextCharacter[0] = x;
        nextCharacter[1] = y;
    }

    //尝试弄个计时出来，多少秒内不能触发第二次
    void Update()
    {
        if (isDoubleClick)
        {
            DateTime newTime = DateTime.Now;
            TimeSpan timeSpan = newTime - nowTime;
            if (timeSpan.Milliseconds > 100)
                isDoubleClick = false;
        }

        //检测，随时调整text的内容
        if (isStart)
        {
            nowCostText.text = _cost + "";
            totalCostText.text = _totalCost + "";
        }
    }
}
