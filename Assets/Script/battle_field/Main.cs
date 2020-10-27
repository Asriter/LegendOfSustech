using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Main : MonoBehaviour
{
    [SerializeField] public SpriteRenderer backGround;

    //public item.equipment e;

    public battle_data battleData;
    // Start is called before the first frame update

    //在sense加载最开始调用
    private void Awake()
    {
        //获取双方对战数据，生成battleData
    }
    void Start()
    {

    }


    void Update()
    {

    }
}
