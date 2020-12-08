using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//当前ui的显示状态
public enum UIViewState
{
    Nonvisible, //不可见的
    Visible,    //可见的
}

public class UIViewTemplate : MonoBehaviour
{
    //UI的层次对应编号，在list中的位置
    public const int StartPanel = 0;    //开始界面
    public const int MainMenu = 1;//菜单

    public const int Embattle = 2;//布阵

    public const int CreateBattleRoom = 3;  //建房

    public UIViewState ViewState = UIViewState.Visible;
    //储存所有的UI
    private List<UIViewTemplate> UI_List;
    //用于点击关闭的背景
    private GameObject bgObj;

    //初始化
    public virtual void initial(List<UIViewTemplate> list)
    {
        UI_List = list;
        OnHide();
    }
    //被显示时
    public virtual void OnShow()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (ViewState != UIViewState.Visible)
        {
            //将z坐标归0
            Vector3 pos = transform.localPosition;
            pos.z = 0;
            transform.localPosition = pos;

            ViewState = UIViewState.Visible;
            InitBG();
        }

        UpdateView();
    }

    //更新
    public virtual void UpdateView()
    {
    }

    //被隐藏
    public virtual void OnHide()
    {
        //if (ViewState == UIViewState.Visible)
        //{
        //从相机的视域体内推出
        Vector3 pos = transform.localPosition;
        pos.z = -99999;
        transform.localPosition = pos;

        ViewState = UIViewState.Nonvisible;
        this.gameObject.SetActive(false);
        //}
        //去掉背景
        Destroy(bgObj);
    }

    //初始化背景(点击背景自动关闭界面)
    //未设置状态转移机，也就是倒回上一步
    public void InitBG()
    {
        Transform bgTran = transform.Find("BG");
        //假定每次右键都会清除BG
        //if (bgTran == null)
        //{
        bgObj = new GameObject("BG", typeof(RectTransform));
        bgTran = bgObj.transform;
        bgTran.SetParent(transform);
        bgTran.SetAsFirstSibling();
        RectTransform rt = bgObj.GetComponent<RectTransform>();
        //rt.Normalize();
        //}
        //查看是否有图片
        Image img = bgTran.GetComponent<Image>();
        if (img == null)
        {
            img = bgTran.gameObject.AddComponent<Image>();
            img.color = new Color(0, 0, 0, 0f);
            CanvasRenderer cr = bgTran.GetComponent<CanvasRenderer>();
            cr.cullTransparentMesh = true;
        }
        img.raycastTarget = true;
        //是否有事件点击
        EventTrigger eventTrigger = bgTran.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = bgTran.gameObject.AddComponent<EventTrigger>();
        }
        //监听点击背景的事件
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        //此处新创建的BG没有关闭，可能出现bug，先做个记号
        entry.callback.AddListener(e => clossePanel());
        eventTrigger.triggers.Add(entry);

    }

    protected void canntClickBG()
    {
        bgObj = new GameObject("BG", typeof(RectTransform));
        Transform bgTran = bgObj.transform;
        bgTran.SetParent(transform);
        bgTran.SetAsFirstSibling();
        RectTransform rt = bgObj.GetComponent<RectTransform>();
    }

    protected void closeCanntClickBG()
    {
        Destroy(bgObj);
    }

    protected virtual void clossePanel()
    {
        OnHide();
        Destroy(bgObj);
        //Debug.Log("点击空白部分");
    }

    public virtual void setArgument(params object[] args)
    {
        //TODO
    }
}

