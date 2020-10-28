using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridUnit : MonoBehaviour
{
    //颜色设置
    [SerializeField] private SpriteRenderer tileRenderer;

    Color _color;

    private Character character;

    private Vector3Int location;

    public void initial()
    {
        //调颜色，测试用，可删
        tileRenderer.color = new Color(0, 0, 200, 0.3f);
        //清空character
        character = null;
    }

    public void initial(Vector3Int location)
    {
        //调颜色，测试用，可删
        tileRenderer.color = new Color(0, 0, 200, 0.3f);
        //清空character
        character = null;

        this.location = location;
    }

    public Vector3Int GetLocation()
    {
        return location;
    }

    public bool IsCharacterEmpty()
    {
        return character != null;
    }

    public Character GetCharacter()
    {
        if(character != null)
            return character;
        else
        {
            Debug.Log("该格子里没有任何对象");
            return null;
        }
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

