using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Start : UIViewTemplate
{
    [SerializeField] Button btnSetting;
    [SerializeField] Button btnLogin;
    [SerializeField] Button btnRegister;
    [SerializeField] Button btnAbout;
    [SerializeField] UI_Login login;
    [SerializeField] UI_Register register;

    public override void initial(List<UIViewTemplate> list)
    {
        base.initial(list);
        //加入监听器
        btnSetting.onClick.AddListener(setBtnSetting);
        btnLogin.onClick.AddListener(setBtnLogin);
        btnRegister.onClick.AddListener(setBtnRegister);
        btnAbout.onClick.AddListener(setBtnAbout);

        //显示界面
        base.OnShow();
    }

    private void setBtnLogin()
    {
        //初始化网络
        //SocketHelper.GetInstance();
        login.OnShow();
        //Debug.Log("点击登录");
    }

    private void setBtnRegister()
    {

        //初始化网络
        //SocketHelper.GetInstance();
        register.OnShow();
        //Debug.Log("点击注册");
    }

    private void setBtnSetting()
    {
        //TODO
    }

     private void setBtnAbout()
    {
        //TODO
    }
}
