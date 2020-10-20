using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test_demo
{
    public class GridUnit : MonoBehaviour
    {
        //颜色设置
        [SerializeField] private GameObject tileRenderer;

        Color _color;

        public void start()
        {
            tileRenderer.GetComponent<Renderer>().material.color = new Color(0, 0, 255, 0.3f);
        }

        public void initial()
        {
            tileRenderer.GetComponent<Renderer>().material.color = new Color(0, 0, 255, 0.3f);
        }

        public void Refresh(int objectType)
        {
            //根据格子类型切换颜色
            switch (objectType)
            {
                case 0:
                    tileRenderer.GetComponent<Renderer>().material.color = new Color(255, 0, 0, 0.3f);
                    //Debug.Log("blue");
                    break;

                case 1:
                    tileRenderer.GetComponent<Renderer>().material.color = new Color(0, 0, 255, 0.3f);
                    //Debug.Log("是否修改颜色0");
                    break;

                default:
                    tileRenderer.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 0);
                    //Debug.Log("是否修改颜色1");
                    break;
            }
        }
    }
}
