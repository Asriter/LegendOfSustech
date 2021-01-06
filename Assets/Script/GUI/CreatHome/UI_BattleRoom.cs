using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_BattleRoom : UIViewTemplate
{
    [SerializeField] Button btnReturn;
    [SerializeField] Button btnSetting;
    [SerializeField] Button btnReady;
    [SerializeField] Button btnEmBattle;
    [SerializeField] SceneData sceneData;
    [SerializeField] SocketConnector socketConnector;
    [SerializeField] UI_CreateRoom createRoom;

    //双方角色信息
    [SerializeField] Image myHeadPortrait;
    [SerializeField] Text myUserName;
    [SerializeField] Text myLevel;

    //对方信息
    [SerializeField] Image oppHeadPortrait;
    [SerializeField] Text oppUserName;
    [SerializeField] Text oppLevel;

    private bool isStart = false;
    private bool isOppAddRoom = false;

    private bool isEmBattle = false;
    private bool isReady = false;

    public override void OnShow()
    {
        base.OnShow();
        //加载我方信息
        //TODO加载头像
        //TODO没有我方userName
        //TODO甚至没有获得等级
        myUserName.text = sceneData.UserName;
        myLevel.text = sceneData.Level + "";

        //初始化class数据
        isStart = false;
        isOppAddRoom = false;

        isEmBattle = false;
        isReady = false;

        //判断我方用不用等人
        if (!socketConnector.isMy)
        {
            isOppAddRoom = true;
            loadOppData();
        }

        //加入监听器
        btnEmBattle.onClick.AddListener(setBtnEmbattle);
        btnReady.onClick.AddListener(setBtnReady);
        btnReturn.onClick.AddListener(setBtnReturn);
        //TODO设置setting

        //开始循环查找是否有人接入
        isStart = true;
    }

    //加载对手信息
    private void loadOppData()
    {
        oppUserName.text = socketConnector.opName;
        //TODO没有level
        oppLevel.text = "233";
    }

    //监听器
    private void setBtnReady()
    {
        if (isEmBattle && !isReady && isOppAddRoom)
        {
            if (sceneData.MyList == null)
            {
                UnityEditor.EditorUtility.DisplayDialog("连接错误", "阵容配置，请重试", "确认");
                return;
            }

            List<List<int>> battleDataList = socketConnector.StartBattle(sceneData.MyList);
            //判断谁是主视角
            if (!socketConnector.isMy)
            {
                foreach (List<int> list in battleDataList)
                {
                    list[0] *= -1;
                }
            }

            //TODO生成BattleData，但是由于没有对方的阵容该步骤暂时无法完成
            //TODO根据生成的BattleDATA调用Scene
            //SceneManager.LoadScene("BattleField");
        }
    }

    private void setBtnEmbattle()
    {
        if (isOppAddRoom && !isReady)
        {
            UI_Controller.Instance.UI_List[UIViewTemplate.Embattle].OnShow();
            isEmBattle = true;
        }
    }

    private void setBtnReturn()
    {
        sceneData.MyList = null;

    }

    //循环检测
    void Update()
    {
        if (isStart && !isOppAddRoom)
        {
            if (socketConnector.hasOpp())
            {
                isOppAddRoom = true;
                this.loadOppData();
            }
        }
    }

}
