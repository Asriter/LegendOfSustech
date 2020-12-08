using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_CreateRoom : UIViewTemplate
{
    public int _roomName = -1;
    [SerializeField] UI_ChooseModel chooseModel;
    [SerializeField] UI_BattleRoom battleRoom;

    public override void OnShow()
    {
        base.OnShow();
        chooseModel.OnShow();
        battleRoom.OnHide();
        _roomName = -1;
    }
}
