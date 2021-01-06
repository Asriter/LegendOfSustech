using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_FriendMessage : UIViewTemplate
{
    [SerializeField] Image _headImage;
    [SerializeField] Text _level;
    [SerializeField] Text _name;
    [SerializeField] Text _UID;
    [SerializeField] Button _battle;
    [SerializeField] Button _delete;

    //等级，名称，uid，头像id
    public override void setArgument(params object[] args)
    {
        _level.text = "Lv: " + args[2] as string;
        _name.text = args[1] as string;
        _UID.text = "UID: " + args[0] as string;

        //加载头像
        //TODO

        //偷个懒，直接把加载谢在这里
        _battle.onClick.AddListener(setBattleApply);
        _delete.onClick.AddListener(setDelete);
    }

    private void setBattleApply()
    {
        //TODO
        //surprise! there is no code!
    }

    private void setDelete()
    {
        //TODO
        //surprise! there is no code!
    }
}
