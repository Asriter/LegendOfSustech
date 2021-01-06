﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

public class SocketConnector : MonoBehaviour
{
    public SocketHelper s;
    public int ID = 0;
    public int Room = -1;
    public bool Quit = false;
    public int op_id = -1; //对手id
    public bool isMy; //true表示己方是mycharacterlist（其实是房主就是my）
    public bool opOffline = false;
    public List<int[]> opList = new List<int[]>();

    public string opName;
    public void Start()
    {
        //创建socket连接
        s = SocketHelper.GetInstance();
        //s = new SocketHelper();
    }

    //单例化测试
    /*public static SocketConnector instance;
    public static SocketConnector Instance
    {
        get
        {
            if(instance == null)
                instance = GameObject.Find("ScendData").GetComponent<SocketConnector>();
            return instance;
        }
    }*/

    private SocketConnector()
    {
        s = SocketHelper.GetInstance();
    }

    public List<List<int>> StartBattle(List<int[]> myList) //返回空并且opOffline为true则对手掉线
    {
        StringBuilder myChess = new StringBuilder("", 130);
        for (int i = 0; i < myList.Count; i++)
        {
            for (int j = 0; j < myList[i].Length; j++)
            {
                myChess.Append(myList[i][j]).Append(",");
            }
        }
        Send("startBattle");
        Send(myChess.ToString());
        string back = GetData();
        if (back.Equals("boom"))
        {
            opOffline = true;
            op_id = -1;
            return null;
        }
        string back1 = GetData();
        string[] data = back.Split(',');
        string[] data1 = back1.Split(',');
        List<List<int>> result = new List<List<int>>();
        for (int i = 0; i < data.Length / 5; i++)
        {
            List<int> tmp = new List<int>();
            for (int j = 0; j < 5; j++)
            {
                tmp.Add(int.Parse(data[i * 5 + j]));
            }
            result.Add(tmp);
        }
        for (int i = 0; i < data1.Length / 7; i++)
        {
            int[] tmp = { int.Parse(data1[i * 7]), int.Parse(data1[i * 7 + 1]), int.Parse(data1[i * 7 + 2]), int.Parse(data1[i * 7 + 3]), int.Parse(data1[i * 7 + 4]), int.Parse(data1[i * 7 + 5]), int.Parse(data1[i * 7 + 6]) };
            opList.Add(tmp);
        }
        return result;
    }

    public bool BuildRoom() //返回4位数字,然后调用WatchRoom
    {
        Quit = false;
        isMy = true;
        Send("buildRoom");
        string back = GetData();
        Room = int.Parse(back);
        return true;
    }

    public void WatchRoom()
    {
        string back = GetData();
        string[] data = back.Split(',');
        op_id = int.Parse(data[0]);
        opName = data[1];
    }

    public void QuitRoom()
    {
        Send("quitRoom");
        Quit = true;
    }

    public string AttendRoom(string code) //返回对手用户昵称
    {
        isMy = false;
        Send("attendRoom");
        Send(code);
        string back = GetData();
        if (back.Equals("fail"))
        {
            return back;
        }
        else
        {
            string[] data = back.Split(',');
            op_id = int.Parse(data[0]);
            opName = data[1];
            return data[1];
        }
    }

    public List<int[]> GetAllChess() //获得玩家所有棋子list[棋子，等级]
    {
        Send("getChess");
        string back = GetData();
        string[] data = back.Split(',');
        List<int[]> result = new List<int[]>();
        for (int i = 0; i < data.Length / 2; i++)
        {
            result.Add(new int[2] { int.Parse(data[i * 2]), int.Parse(data[i * 2 + 1]) });
        }
        return result;
    }

    private void Listener()
    {
        while (!s.isUpdate)
        {
            if (Quit) return;
        }
        s.isUpdate = false;
        string back = s.data.Trim();
        string[] data = back.Split(',');
        op_id = int.Parse(data[0]);
        //TODO:
        //怎么去通知你已经有人进来了，你看看能不能直接在这里执行你那边的相关函数来跑，或者你那边建立一个进程来监控，data[1]是对手名称
        if (op_id != -1)
        {
            opName = data[1];
        }
    }

    public bool hasOpp()
    {
        while (!s.isUpdate)
        {
            if (Quit) return false; ;
        }
        s.isUpdate = false;
        string back = s.data.Trim();
        string[] data = back.Split(',');
        op_id = int.Parse(data[0]);
        //TODO:
        //怎么去通知你已经有人进来了，你看看能不能直接在这里执行你那边的相关函数来跑，或者你那边建立一个进程来监控，data[1]是对手名称
        if (op_id != -1)
        {
            opName = data[1];
            return true;
        }
        return false;
    }


    public bool Login(string account, string password)
    {
        Send("login");
        Send(account + "," + password);
        string back = GetData();
        if (back.Equals("fail"))
        {
            return false;
        }
        else
        {
            ID = int.Parse(back);
            return true;
        }
    }
    public bool Register(string email, string phone, string password, string username)
    {
        Send("register");
        Send(email + "," + phone + "," + password + "," + username);
        string back = GetData();
        if (back.Equals("fail"))
        {
            return false;
        }
        else
        {
            ID = int.Parse(back);
            return true;
        }
    }

    public bool AddFriend(int uid) //添加好友，需要对方uid
    {
        Send("addFriend");
        Send(uid.ToString());
        string back = GetData();
        if (back.Equals("fail"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public List<string[]> GetFriend()
    {
        Send("getFriend");
        string back = GetData();
        string[] data = back.Split(',');
        List<string[]> result = new List<string[]>();
        for (int i = 0; i < data.Length / 3; i++)
        {
            string[] data1 = { data[i * 3 + 1], data[i * 3], data[i * 3 + 2] };
            result.Add(data1);
        }
        return result; //都是string类型，排序（id 昵称 等级）
    }

    public string[] GetSelf()
    {
        Send("getSelf");
        string back = GetData();
        string[] data = back.Split(',');
        string[] result = { data[1], data[0], data[2] };
        return result; //都是string类型，排序（id 昵称 等级）
    }

    private void Send(string str)
    {
        s.SendMessage(str + "\r\n");
    }

    private string GetData() //在读取后才结束循环
    {
        if(s == null)
            s = SocketHelper.GetInstance();
        while (!s.isUpdate)
        {
            continue;
        }
        s.isUpdate = false;
        return s.data.Trim();
    }


}