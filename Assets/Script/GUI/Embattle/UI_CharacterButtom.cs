using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_CharacterButtom : MonoBehaviour
{
    [SerializeField] Button _character_buttom;
    [SerializeField] Image _character_image;
    private UI_Embattle embattle;
    public int id = -1;     //对应单位的id
    private int level = 50;  //应对后期可能会出现的等级系统

    //以下是这里用到的颜色
    private Color GRAY = new Color(93, 93, 93, 1.0f);
    private Color WHITE = new Color(255, 255, 255, 1.0f);

    private Color YELLOW = new Color(255, 255, 0, 1.0f);

    private bool isClick = false;

    DateTime nowTime;
    bool isDoubleClick = false;

    public void initial(int id, int level)
    {
        embattle = (UI_Embattle)UI_Controller.Instance.UI_List[UIViewTemplate.Embattle];
        this.id = id;
        this.level = level;
        _character_buttom.onClick.AddListener(setBtn);
        loadImage();
    }

    public void initial(int id)
    {
        this.id = id;
        loadImage();
    }

    private void loadImage()
    {
        string path = "Picture/Character/" + this.id;
        object obj = Resources.Load(path, typeof(Sprite));
        Sprite sp = obj as Sprite;
        _character_image.sprite = sp;
        _character_image.color = this.WHITE;
    }

    public void resetImage()
    {
        _character_image.sprite = null;
        _character_image.color = this.GRAY;
        this.id = -1;
    }


    private void setBtn()
    {
        //如果短时间内检测到多次点击则自动跳过
        if(isDoubleClick)
            return;
        
        if(id == -1)
            return;

         //Debug.Log("看看有没有走到这里1");
        if (!isClick)
        {
            // Debug.Log("看看有没有走到这里2");
            embattle.setNextCharacter(id, level);
            //其他单位全部变回原色
            embattle.resetAllColor();
            //该单位变成选中色
            clicked();

            //设置该单位为已选中
            isClick = true;
            //最后把该单位隐藏掉，表现上就是该单位消失，所有向上补全
            //TODO

            //记录当前时间
            nowTime = DateTime.Now;
            isDoubleClick = true;
        }
        //若已经选中，则再次点击是取消选中
        else
        {
            //Debug.Log("看看有没有走到这里");
            embattle.setNextCharacter(-1, -1);
            resetColor();

            //记录当前时间
            nowTime = DateTime.Now;
            isDoubleClick = true;
        }
    }

    public void resetColor()
    {
        isClick = false;
        if (id == -1){
            //_character_image.color = GRAY;
            //Debug.Log("gray");
        }
        else
        {
            _character_image.color = WHITE;
            //Debug.Log("white");
        }
    }

    public void clicked()
    {
        if (id != -1)
            _character_image.color = YELLOW;
    }

    //尝试弄个计时出来，多少秒内不能触发第二次
    void Update()
    {
        if(isDoubleClick)
        {
            DateTime newTime = DateTime.Now;
            TimeSpan timeSpan = newTime - nowTime;
            if(timeSpan.Milliseconds > 100)
                isDoubleClick = false;
            //Debug.Log(timeSpan.Milliseconds);
        }
    }
}
