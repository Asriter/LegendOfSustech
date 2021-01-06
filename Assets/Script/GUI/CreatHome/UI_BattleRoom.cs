using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Threading;
using System;
using UnityEngine.SceneManagement;

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
    //根据是否有人进来决定是显示id还是显示无人进入
    [SerializeField] GameObject maskPanel;

    private bool isStart = false;
    private bool isOppAddRoom = false;

    private bool isEmBattle = false;
    private bool isReady = false;

    //用于阶段性检测连接状态
    DateTime nowTime;

    public override void OnShow()
    {
        base.OnShow();
        //加载我方信息
        //TODO加载头像
        myUserName.text = sceneData.UserName;
        myLevel.text = sceneData.Level + "";

        //初始化class数据
        isStart = false;
        isOppAddRoom = false;

        isEmBattle = false;
        isReady = false;

        maskPanel.SetActive(true);

        Debug.Log("开始加载界面");

        //判断我方用不用等人
        if (!socketConnector.isThis)
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
        nowTime = DateTime.Now;
    }

    //加载对手信息
    private void loadOppData()
    {
        oppUserName.text = socketConnector.opName;
        //TODO没有level
        oppLevel.text = "233";

        maskPanel.SetActive(false);
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
            if (!socketConnector.isThis)
            {
                foreach (List<int> list in battleDataList)
                {
                    list[0] *= -1;
                }
            }

            //生成地方阵容数据
            List<int[]> OpList = socketConnector.opList;
            //TODO根据生成的BattleDATA调用Scene
            SceneManager.LoadScene("BattleField");
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
        socketConnector.QuitRoom();
        this.OnHide();
    }

    //循环检测
    void Update()
    {
        DateTime newTime = DateTime.Now;
        TimeSpan timeSpan = newTime - nowTime;
        if (timeSpan.Milliseconds < 1000)
        {
            return;
        }
        else
        {
            nowTime = DateTime.Now;
        }

        if (isStart && !isOppAddRoom)
        {
            Thread t = new Thread(new ThreadStart(socketConnector.hasOpp));
            t.Start();
            if (socketConnector.isOpEnter)
            {
                isOppAddRoom = true;
                this.loadOppData();
            }
        }
    }

}
