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
    [SerializeField] private Transform _parent;//所有好友信息的父节点

    private int uid = -1;
    [SerializeField] SocketConnector socketConnector;

    //第三个界面
    //TODO
    //Surprise! There is no code!

    public override void OnShow()
    {
        _addFriend.onClick.AddListener(btnAddFriendPanel);
        _checkFriend.onClick.AddListener(btnCheckPanel);
        _friendApply.onClick.AddListener(btnFriendApplyPanel);
        _return.onClick.AddListener(btnReturn);

        //打开添加界面
        this.showAddPanel();

        base.OnShow();
    }

    //打开添加好友界面
    private void showAddPanel()
    {
        _addFriendPanel.SetActive(true);
        _friendApplyPanel.SetActive(false);
        _checkFriendPanel.SetActive(false);

        //设置button
        _UIDInput.onEndEdit.AddListener(End_Value_Account);
        _add.onClick.AddListener(btnAddFriend);
    }

    private void End_Value_Account(string inp)
    {
        uid = int.Parse(inp);
    }

    //添加对应uid的好友
    private void btnAddFriend()
    {
        if(uid == -1)
        {
            UnityEditor.EditorUtility.DisplayDialog("添加失败", "uid为空", "确认");
            return;
        }

        if (socketConnector.AddFriend(uid))
        {
            UnityEditor.EditorUtility.DisplayDialog("添加失败", "uid为空", "确认");
        }
    }

    //返回上一层
    private void btnReturn()
    {

    }

    private void btnAddFriendPanel()
    {

    }

    private void btnCheckPanel()
    {

    }

    private void btnFriendApplyPanel()
    {

    }
}
