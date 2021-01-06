using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_MainMenu : UIViewTemplate
{
    [SerializeField] Image _headPortrit;
    [SerializeField] Button _headPortritButton;
    [SerializeField] Text _userName;
    [SerializeField] Text _level;
    //TODO 经验条

    [SerializeField] Button _setting;
    [SerializeField] Button _package;
    [SerializeField] Button _shop;
    [SerializeField] Button _embattle;
    [SerializeField] Button _charge;
    [SerializeField] Button _closeContactMessage;
    [SerializeField] Button _friends;
    [SerializeField] Button _createBattleRoom;

    public override void initial(List<UIViewTemplate> list)
    {
        base.initial(list);

        //添加监听器
        _headPortritButton.onClick.AddListener(setBtnHeadPortrit);
        _setting.onClick.AddListener(setBtnSetting);
        _shop.onClick.AddListener(setBtnShop);
        _package.onClick.AddListener(setBtnPackage);
        _embattle.onClick.AddListener(setBtnEmbattle);
        _charge.onClick.AddListener(setBtnCharge);
        _closeContactMessage.onClick.AddListener(setBtnCloseContactMassage);
        _friends.onClick.AddListener(setBtnFriends);
        _createBattleRoom.onClick.AddListener(setBtnCreateBattleRoom);
    }

    public override void OnShow()
    {
        base.OnShow();
        SceneData sd = GameObject.Find("SceneData").GetComponent<SceneData>();
        _userName.text = sd.UserName;
        _level.text = sd.Level + "";

        //加载头像
        //TODO
    }

    //监听器
    private void setBtnHeadPortrit()
    {
        //TODO
    }

    private void setBtnShop()
    {
        //TODO
    }

    private void setBtnSetting()
    {
        //TODO
    }

    private void setBtnPackage()
    {
        //TODO
    }

    private void setBtnEmbattle()
    {
        //TODO
        UI_Controller.Instance.UI_List[UIViewTemplate.Embattle].OnShow();
    }

    private void setBtnCharge()
    {
        //TODO
    }

    private void setBtnCloseContactMassage()
    {
        //TODO
    }

    private void setBtnFriends()
    {
        //TODO
    }

    private void setBtnCreateBattleRoom()
    {
        UI_Controller.Instance.UI_List[UIViewTemplate.CreateBattleRoom].OnShow();
    }
}
