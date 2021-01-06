using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_BattleResult : UIViewTemplate
{
    [SerializeField] Text resultText;
    [SerializeField] Button btnReturn;

    public override void initial(List<UIViewTemplate> list)
    {
        base.initial(list);

        //显示界面
        base.OnShow();
    }

    public override void OnShow()
    {
        base.OnShow();
        if(controller.Instance.battleData.GetBattleData()[controller.Instance.battleData.GetBattleData().Count-1][0] == 0)
            resultText.text = "你赢了!";
        else
        {
            resultText.text = "你输了!";
        }

        btnReturn.onClick.AddListener(setBtnReturn);
    }

    private void setBtnReturn()
    {
        //GameObject.Find("SceneData").GetComponent<SceneData>().isReEmbattle = true;
        this.OnHide();
        SceneManager.LoadScene("Menu");
    }
}
