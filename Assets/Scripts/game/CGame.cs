using UnityEngine;
using System.Collections;

public class CGame : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        CGameManager.Instance.Init();
        UILabel label = GameObject.Find("label_username").GetComponent<UILabel>();
        label.text = NetClient.Instance.Player().name;

        int type = NetClient.Instance.Player().ctype;
        UISprite sprite = GameObject.Find("sprite_head").GetComponent<UISprite>();
        switch (type)
        {
            case 0:
                {
                    sprite.spriteName = "character1";
                }
                break;
            case 1:
                {
                    sprite.spriteName = "男_头像";
                }
                break;
            case 2:
                {
                    sprite.spriteName = "女_头像";
                }
                break;
        }
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
