using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_ChooseModel : UIViewTemplate
{
    [SerializeField] GameObject choosePanel;
    [SerializeField] GameObject AddPanel;
    [SerializeField] Button btnBuildRoom;
    [SerializeField] Button btnAddRoom;

    //下面是加入房间控件的内容
    [SerializeField] InputField RoomNumber;
    [SerializeField] Button btnCurrectAddRoom;
    [SerializeField] Button btnReturn;

    //连接
    [SerializeField] SocketConnector socketConnector;

    //父节点，即侧rateRoom
    [SerializeField] UI_CreateRoom createRoom;

    [SerializeField] UI_BattleRoom battleRoom;

    private string _roomNumber = "-1";

    public override void OnShow()
    {
        base.OnShow();
        choosePanel.SetActive(true);
        AddPanel.SetActive(false);
        _roomNumber = "-1";

        //输入数值
        RoomNumber.onEndEdit.AddListener(End_Value_RoomNumber);
        //加入监听器
        btnAddRoom.onClick.AddListener(setBtnAddRoom);
        //btnBuildRoom.onClick.AddListener(setBtnBuildRoom);
        btnBuildRoom.onClick.AddListener(setBtnBuildRoom);
        btnCurrectAddRoom.onClick.AddListener(setBtnCurrentAddRoom);
        btnReturn.onClick.AddListener(setBtnReturn);
    }

    private void End_Value_RoomNumber(string inp)
    {
        _roomNumber = inp;
    }
    private void setBtnBuildRoom()
    {
        Debug.Log(createRoom._roomName);
        //createRoom._roomName = socketConnector.BuildRoom();
        if (GameObject.Find("SceneData").GetComponent<SocketConnector>().BuildRoom())
        //if(socketConnector.ID == 1)
        {
            createRoom._roomName = socketConnector.Room;
            //打开房间窗口
            battleRoom.OnShow();
            this.OnHide();
        }
        else
        {
            UnityEditor.EditorUtility.DisplayDialog("连接错误", "网络连接错误，请重试", "确认");
            return;
        }
    }

    /*private void test()
    {
        socketConnector.BuildRoom();
        createRoom._roomName = socketConnector.Room;
            //打开房间窗口
        battleRoom.OnShow();
    }*/

    private void setBtnAddRoom()
    {
        choosePanel.SetActive(false);
        AddPanel.SetActive(true);
    }

    private void setBtnCurrentAddRoom()
    {
        if (_roomNumber.Equals("-1"))
        {
            UnityEditor.EditorUtility.DisplayDialog("连接错误", "请输入房间号", "确认");
            return;
        }

        string name = socketConnector.AttendRoom(_roomNumber);
        //连接失败
        if (name.Equals("fail"))
        {
            UnityEditor.EditorUtility.DisplayDialog("连接错误", "无法加入房间", "确认");
            return;
        }

        //标记房间号
        createRoom._roomName = int.Parse(name);
        this._roomNumber = name;

        //打开房间窗口
        battleRoom.OnShow();
        this.OnHide();
    }

    private void setBtnReturn()
    {
        choosePanel.SetActive(true);
        AddPanel.SetActive(false);
    }

    //重写点击空白关闭的方法，把父节点一起关闭
    protected override void clossePanel()
    {
        base.clossePanel();
        createRoom.OnHide();
    }

}