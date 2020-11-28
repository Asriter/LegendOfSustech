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
    void Update()
    {
        double dhp = character._hp / character._maxHp;
        double dmp = character._mp / max_mp;
        //Debug.Log(character._mp);

        hp.fillAmount = (float)dhp;
        mp.fillAmount = (float)dmp;
    }
}
