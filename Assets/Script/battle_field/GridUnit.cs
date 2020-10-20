using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test_demo
{
    public class GridUnit : MonoBehaviour
    {
        //颜色设置
        [SerializeField] private SpriteRenderer tileRenderer;

        Color _color;

        public void Start()
        {
            initial();
        }

        public void initial()
        {
            tileRenderer.color = new Color(0, 0, 200, 0.3f);
        }

        public void Refresh(int objectType)
        {
            //根据格子类型切换颜色
            switch (objectType)
            {
                case 0:
                    tileRenderer.color = new Color(255, 0, 0, 0.3f);
                    //Debug.Log("blue");
                    break;

                case 1:
                    tileRenderer.color = new Color(0, 0, 255, 0.3f);
                    //Debug.Log("是否修改颜色0");
                    break;

                default:
                    tileRenderer.color = new Color(0, 0, 0, 0);
                    //Debug.Log("是否修改颜色1");
                    break;
            }
        }
    }
}
