using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UI_Register : UIViewTemplate
{
    [SerializeField] InputField mail;
    [SerializeField] InputField phone;
    [SerializeField] InputField userName;
    [SerializeField] InputField password;
    [SerializeField] InputField rePassword;

    [SerializeField] Button btnRegister;

    [SerializeField] SocketConnector socketConnector;

    private string _mail;
    private string _phone;
    private string _password;
    private string _rePassword;
    private string _userName;

    public override void OnShow()
    {
        base.OnShow();
        base.InitBG();
        //初始化
        _mail = "";
        _password = "";
        _rePassword = "";
        _phone = "";
        _userName = "";

        //绑定监听器
        mail.onEndEdit.AddListener(End_Value_mail);
        userName.onEndEdit.AddListener(End_Value_userName);
        phone.onEndEdit.AddListener(End_Value_phone);
        password.onEndEdit.AddListener(End_Value_password);
        rePassword.onEndEdit.AddListener(End_Value_rePassword);

        btnRegister.onClick.AddListener(setBtnRegister);
    }

    private void End_Value_mail(string inp)
    {
        _mail = inp;
    }

    private void End_Value_password(string inp)
    {
        _password = inp;
    }
    private void End_Value_rePassword(string inp)
    {
        _rePassword = inp;
    }
    private void End_Value_phone(string inp)
    {
        _phone = inp;
    }
    private void End_Value_userName(string inp)
    {
        _userName = inp;
    }

    private void setBtnRegister()
    {
        if (_mail.Equals(""))
        {
            UnityEditor.EditorUtility.DisplayDialog("注册错误", "邮箱为空", "确认");
            return;
        }

        if (_phone.Equals(""))
        {
            UnityEditor.EditorUtility.DisplayDialog("注册错误", "电话为空", "确认");
            return;
        }

        if (_userName.Equals(""))
        {
            UnityEditor.EditorUtility.DisplayDialog("注册错误", "用户名为空", "确认");
            return;
        }

        if (_password.Equals(""))
        {
            UnityEditor.EditorUtility.DisplayDialog("注册错误", "密码为空", "确认");
            return;
        }

        if (_rePassword.Equals(""))
        {
            UnityEditor.EditorUtility.DisplayDialog("注册错误", "请重复输入密码", "确认");
            return;
        }

        if (!_password.Equals(_rePassword))
        {
            UnityEditor.EditorUtility.DisplayDialog("注册错误", "两次密码不一致", "确认");
            return;
        }
        Debug.Log("看看有没有走到这里register");

        //注册
        if (socketConnector.Register(_mail, _phone, _password, _userName))
        {
            Debug.Log("看看有没有走到这里register1");
            if (socketConnector.Login(socketConnector.ID + "", _password))
            {
                SceneData sd = GameObject.Find("SceneData").GetComponent<SceneData>();
                //没有返回玩家用户名和等级
                sd.Level = 1;
                sd.UserName = _userName;

                //直接调用方法读取list
                List<int[]> canSetCharacter = socketConnector.GetAllChess();

                sd.CanSetCharacter = canSetCharacter;

                UI_Controller.Instance.UI_List[UIViewTemplate.StartPanel].OnHide();
                UI_Controller.Instance.UI_List[UIViewTemplate.MainMenu].OnShow();
                //controller.UI_List[UIViewTemplate.MainMenu].setArgument();
                //可能会加入参数

                //Debug.Log("点击登录");
                this.OnHide();
            }
            else
            {
                UnityEditor.EditorUtility.DisplayDialog("登录失败", "用户名或密码错误", "确认");
                return;
            }
        }
    }
}
