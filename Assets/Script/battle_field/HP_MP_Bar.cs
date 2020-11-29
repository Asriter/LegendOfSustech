using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_MP_Bar : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private Image hp;
    [SerializeField] private Image mp;

    private static int max_mp = 100;

    //通过flag来控制啊是否调整蓝条红条
    private bool isChangeHp = true;

    private bool isChangeMp = true;
    void Update()
    {
        double dhp = character._hp / character._maxHp;
        double dmp = character._mp / max_mp;
        //Debug.Log(character._mp);

        if(isChangeHp)
        {
            hp.fillAmount = (float)dhp;
            isChangeHp = false;
        }
        if(isChangeMp)
        {
            mp.fillAmount = (float)dmp;
            isChangeMp = false;
        }
    }

    public void setHP()
    {
        this.isChangeHp = true;
    }

    public void setMP()
    {
        this.isChangeMp = true;
    }
}
