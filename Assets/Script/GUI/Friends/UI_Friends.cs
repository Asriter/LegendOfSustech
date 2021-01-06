using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UI_Friends : UIViewTemplate
{
    [SerializeField] Button _return;

    [SerializeField] Button _addFriend;

    [SerializeField] Button _checkFriend;
    [SerializeField] Button _friendApply;


    //三个界面的gameobjeck
    [SerializeField] GameObject _addFriendPanel;
    [SerializeField] GameObject _checkFriendPanel;
    [SerializeField] GameObject _friendApplyPanel;

    //第一个界面中的控件
    [SerializeField] InputField _UIDInput;
    [SerializeField] Button _add;
    [SerializeField] Text _myUID;

    //第二个界面
    [SerializeField] private Transform _friendMessageRoot;//所有好友信息的父节点

    private int uid = -1;
    [SerializeField] SocketConnector socketConnector;

    //好友信息的prefab
    [SerializeField] UI_FriendMessage friendMessageModel;

    //第三个界面
    //TODO
    //Surprise! There is no code!

    public override void OnShow()
    {
        this.uid = -1;

        _addFriend.onClick.AddListener(btnAddFriendPanel);
        _checkFriend.onClick.AddListener(btnCheckPanel);
        _friendApply.onClick.AddListener(btnFriendApplyPanel);
        _return.onClick.AddListener(btnReturn);

        //打开添加界面
        this.btnAddFriendPanel();

        base.OnShow();
    }

    private void End_Value_Account(string inp)
    {
        uid = int.Parse(inp);
    }

    //添加对应uid的好友
    private void btnAddFriend()
    {
        if (uid == -1)
        {
            UnityEditor.EditorUtility.DisplayDialog("添加失败", "uid为空", "确认");
            return;
        }

        if (socketConnector.AddFriend(uid))
        {
            UnityEditor.EditorUtility.DisplayDialog("添加成功", "添加成功，等待对方确认！", "确认");
        }
        else
        {
            UnityEditor.EditorUtility.DisplayDialog("添加失败", "好友添加失败，请检查对方UID", "确认");
            return;
        }
    }

    //返回上一层
    private void btnReturn()
    {
        this.uid = -1;
        this.OnHide();
    }

    private void btnAddFriendPanel()
    {
        _addFriendPanel.SetActive(true);
        _friendApplyPanel.SetActive(false);
        _checkFriendPanel.SetActive(false);

        //设置button
        _UIDInput.onEndEdit.AddListener(End_Value_Account);
        _add.onClick.AddListener(btnAddFriend);
    }

    private void btnCheckPanel()
    {
        _addFriendPanel.SetActive(false);
        _friendApplyPanel.SetActive(false);
        _checkFriendPanel.SetActive(true);

        //加载好友信息
        List<string[]> friendList = socketConnector.GetFriend();

        //把好友信息放到格子里
        for (int i = 0; i < friendList.Count; i++)
        {
            UI_FriendMessage fm = Instantiate<UI_FriendMessage>(friendMessageModel);
            fm.transform.SetParent(_friendMessageRoot);
            fm.transform.localPosition = Vector3.zero;
            fm.transform.localScale = Vector3.one;
            fm.transform.localRotation = Quaternion.identity;

            fm.setArgument(friendList[i][0], friendList[i][1], friendList[i][2]);
        }
    }

    private void btnFriendApplyPanel()
    {
        _addFriendPanel.SetActive(false);
        _friendApplyPanel.SetActive(true);
        _checkFriendPanel.SetActive(false);
    }
}
