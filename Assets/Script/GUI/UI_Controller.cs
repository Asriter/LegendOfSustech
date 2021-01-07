using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UI_Controller : MonoBehaviour
{
    [SerializeField] public List<UIViewTemplate> UI_List;
    //[SerializeField] UI_RoundPanel roundPanel;
    // Start is called before the first frame update
    public int depth = 1;
    void Start()
    {
        for (int i = 0; i < UI_List.Count; i++)
        {
            UI_List[i].initial(UI_List);
        }

        //根据状态开启界面
        if(GameObject.Find("SceneData").GetComponent<SceneData>().isReEmbattle)
        {
            GameObject.Find("SceneData").GetComponent<SceneData>().isReEmbattle = false;
            UI_List[UIViewTemplate.MainMenu].OnShow();
            UI_List[UIViewTemplate.Embattle].OnShow();
        }

        //roundPanel.initial(UI_List);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private static UI_Controller instance;
    public static UI_Controller Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        instance = this;
    }
}
