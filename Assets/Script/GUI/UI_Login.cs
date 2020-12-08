using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Login : UIViewTemplate
{
    [SerializeField] InputField Account;
    [SerializeField] InputField Password;
    [SerializeField] Button btnLogin;
    [SerializeField] UI_Controller controller;
    [SerializeField] SocketConnector socketConnector;

    private string account_value = "";

    private string password_value = "";

    void Start()
    {
        Account.onEndEdit.AddListener(End_Value_Account);
        Password.onEndEdit.AddListener(End_Value_Password);
        btnLogin.onClick.AddListener(setBtnLogin);
    }

    public override void OnShow()
    {
        base.OnShow();
        this.InitBG();
    }


    private void End_Value_Account(string inp)
    {
        account_value = inp;
    }

    private void End_Value_Password(string inp)
    {
        password_value = inp;
    }

    private void setBtnLogin()
    {
        //没输账号
        if (account_value.Equals(""))
        {
            UnityEditor.EditorUtility.DisplayDialog("登录错误", "用户名为空", "确认");
            return;
        }

        //没输密码
        if (password_value.Equals(""))
        {
            UnityEditor.EditorUtility.DisplayDialog("登录错误", "密码为空", "确认");
            return;
        }

        //用户名或密码错误
        //TODO

        //登陆成功
        //TODO等待来自服务器的认证,这里直接成功验证了
        Debug.Log("看看有没有走到这里");

        //根据服务器的返回值设置sceneData
        //if (socketConnector.Login(account_value, password_value))
        //{
            SceneData sd = GameObject.Find("SceneData").GetComponent<SceneData>();
            //没有返回玩家用户名和等级
            sd.Level = 233;
            sd.UserName = "testUser";
            //TODO加载该玩家拥有的角色
            //此时暂时直接添加用作测试
            List<int[]> canSetCharacter = new List<int[]>();
            canSetCharacter.Add(new int[2]{1, 50});
            canSetCharacter.Add(new int[2]{1, 50});
            canSetCharacter.Add(new int[2]{1, 50});
            canSetCharacter.Add(new int[2]{2, 50});
            canSetCharacter.Add(new int[2]{2, 50});
            canSetCharacter.Add(new int[2]{2, 50});
            canSetCharacter.Add(new int[2]{3, 50});
            canSetCharacter.Add(new int[2]{3, 50});
            canSetCharacter.Add(new int[2]{3, 50});
            //测试代码到此为止

            //直接调用方法读取list
            //List<int[]> canSetCharacter = socketConnector.GetAllChess();

            sd.CanSetCharacter = canSetCharacter;

            controller.UI_List[UIViewTemplate.StartPanel].OnHide();
            controller.UI_List[UIViewTemplate.MainMenu].OnShow();
            //controller.UI_List[UIViewTemplate.MainMenu].setArgument();
            //可能会加入参数

            //Debug.Log("点击登录");
            this.OnHide();
        //}
        /*else{
            Debug.Log("看看有没有走到这里1");
            UnityEditor.EditorUtility.DisplayDialog("登录失败", "用户名或密码错误", "确认");
            return;
        }*/
    }
}
