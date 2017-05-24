using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CMenuCallback : MonoBehaviour
{
    public UITextList textlist;

    public static CMenuCallback Instance = null;

    void Start()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    public void On_Button_Register_Clicked()
    {
        SceneManager.LoadScene("Scenes/gui/register");
    }

    public void On_Button_Login_Clicked()
    {
        string username = GameObject.Find("SpriteUsername").GetComponent<UIInput>().value;
        string password = GameObject.Find("SpritePassword").GetComponent<UIInput>().value;
        if (username == "" || password == "")
        {
            Debug.Log("用户名或密码不能为空！");
            return;
        }
        if (username.Length > 24 || password.Length > 24)
        {
            Debug.Log("用户名或密码过长");
            return;
        }
        Debug.Log(username);
        Debug.Log(password);
        string msg;
        if (true == NetClient.Instance.Login(username, password, out msg))
        {
            Debug.Log("login success!");
            NetClient.Instance.setUserName(username);
            NetClient.Instance.Player().name = username;
            SceneManager.LoadScene("Scenes/gui/interface");
        }
        else
        {
            Debug.Log("login failed!");
        }
        Debug.Log(msg);
    }

    public void On_Label_Return_Login_Clicked()
    {
        SceneManager.LoadScene("Scenes/gui/login");
    }

    public void On_Button_Final_Register_Clicked()
    {
        string username = GameObject.Find("SpriteUsername").GetComponent<UIInput>().value;
        string password = GameObject.Find("SpritePassword").GetComponent<UIInput>().value;
        string ack_password = GameObject.Find("SpriteAckPassword").GetComponent<UIInput>().value;

        Debug.Log(username);
        Debug.Log(password);
        Debug.Log(ack_password);
        if (password != ack_password)
        {
            Debug.Log("两次密码不一致，请重新输入！");
            return;
        }
        if (username == "" || password == "")
        {
            Debug.Log("用户名或密码不能为空！");
            return;
        }
        if (username.Length > 24 || password.Length > 24)
        {
            Debug.Log("用户名或密码过长");
            return;
        }

        string msg;
        if (true == NetClient.Instance.Register(username, password, out msg))
        {
            Debug.Log("register success!");
            SceneManager.LoadScene("Scenes/gui/login");
        }
        else
        {
            Debug.Log("register failed!");
        }
        Debug.Log(msg);
    }

    public void On_Button_Choose0_Clicked()
    {
        int type = 0;
        NetClient.Instance.Player().ctype = type;
        NetClient.Instance.Player().first = false;

        On_Button_Play_Clicked();
    }
    public void On_Button_Choose1_Clicked()
    {
        int type = 1;
        NetClient.Instance.Player().ctype = type;
        NetClient.Instance.Player().first = false;

        On_Button_Play_Clicked();
    }
    public void On_Button_Choose2_Clicked()
    {
        int type = 2;
        NetClient.Instance.Player().ctype = type;
        NetClient.Instance.Player().first = false;

        On_Button_Play_Clicked();
    }

    public void On_Button_Play_Clicked()
    {
        //根据用户基本信息判断是否是首次登入
        if (NetClient.Instance.First())
        {
            SceneManager.LoadScene("Scenes/gui/choose");
        }
        else
        {
            string msg;
            if (true == NetClient.Instance.LinkWorld(out msg))
            {
                Debug.Log("登入游戏世界成功!");

                SceneManager.LoadScene("Scenes/game");
            }
            else
            {
                Debug.Log("登入游戏世界失败!");
            }
            Debug.Log(msg);
        }
    }

    public void On_Control_Input_Submit()
    {
        UIInput input = this.GetComponent<UIInput>();
        string chatmessage = input.value;
        NetClient.Instance.SendChat(chatmessage);
        textlist.Add(NetClient.Instance.Player().name + ": " + chatmessage);
        input.value = "";
    }
    public void On_Button_KeepPlay_Clicked()
    {
        SceneManager.LoadScene("Scenes/gui/interface");
    }
}
