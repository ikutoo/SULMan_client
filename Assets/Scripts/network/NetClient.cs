using UnityEngine;
using System.Collections;
using AppNetWork;
using System.Diagnostics;
using System.Collections.Generic;

public class NetClient : MonoBehaviour
{

    public static NetClient Instance = null;

    //public CMenuCallback Menu = null;
    // 收到的聊在消息
    private string m_recvString = "";

    // NetManager
    private NetManager m_netManager;

    // 输入的聊天消息
    private string m_inputString = "";

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);

        Instance = this;
        m_netManager = new NetManager();
        m_netManager.Start();
    }

    // Update is called once per frame
    void Update()
    {
        // 处理队列中的数据
        m_netManager.Update();
        // 向服务器发送控制信息
        if (m_netManager.iGameState == 1)
            SendControl();
    }

    public void setUserName(string name)
    {
        m_netManager.strUsername = name;
        UnityEngine.Debug.Log("$$$1" + m_netManager.strUsername);
    }

    public Player Player()
    {
        return m_netManager.player;
    }

    public string RecvString
    {
        get { return m_recvString; }
        set { m_recvString = value; }
    }

    public bool First()
    {
        return m_netManager.player.first;
    }

    public bool Login(string username, string password, out string msg)
    {
        if (false == m_netManager.bConnected)
        {
            msg = "无法连接到服务器，请稍后重试！";
            return false;
        }
        m_netManager.iLoginState = 0;
        NetBitStream stream = new NetBitStream();
        stream.BeginWrite((ushort)MessageIdentifiers.ID.CLIENT_LOGIN);
        stream.WriteString(username + "&" + password);
        stream.EncodeHeader();
        m_netManager.Send(stream);
        msg = "";

        Stopwatch sw = new Stopwatch();
        sw.Start();
        while (0 == m_netManager.iLoginState)
        {
            m_netManager.Update();
            long time = sw.ElapsedMilliseconds;
            if (time > 5000)
            {
                break;
            }
        }
        sw.Stop();
        if (1 == m_netManager.iLoginState)
        {
            msg = m_netManager.strMessage;
            m_netManager.strUsername = username;
            return true; ;
        }
        msg = m_netManager.strMessage;
        return false;
    }

    public bool Register(string username, string password, out string msg)
    {
        if (false == m_netManager.bConnected)
        {
            msg = "无法连接到服务器，请稍后重试！";
            return false;
        }
        m_netManager.iRegisterState = 0;
        NetBitStream stream = new NetBitStream();
        stream.BeginWrite((ushort)MessageIdentifiers.ID.CLIENT_REGISTER);
        stream.WriteString(username + "&" + password);
        stream.EncodeHeader();
        m_netManager.Send(stream);
        msg = "";
        Stopwatch sw = new Stopwatch();
        sw.Start();
        while (0 == m_netManager.iRegisterState)
        {
            m_netManager.Update();
            long time = sw.ElapsedMilliseconds;
            if (time > 5000)
            {
                break;
            }
        }
        sw.Stop();
        if (1 == m_netManager.iRegisterState)
        {
            msg = m_netManager.strMessage;
            return true; ;
        }
        msg = m_netManager.strMessage;
        return false;
    }

    public bool LinkWorld(out string msg)
    {
        if (false == m_netManager.bConnected)
        {
            msg = "无法连接到服务器，请稍后重试！";
            return false;
        }
        m_netManager.iGameState = 0;
        NetBitStream stream = new NetBitStream();
        stream.BeginWrite((ushort)MessageIdentifiers.ID.CLIENT_LINK_WORLD);
        stream.WriteString(m_netManager.strUsername);
        stream.WriteInt(m_netManager.player.ctype);

        stream.EncodeHeader();
        m_netManager.Send(stream);
        msg = "";
        Stopwatch sw = new Stopwatch();
        sw.Start();
        while (0 == m_netManager.iGameState)
        {
            m_netManager.Update();
            long time = sw.ElapsedMilliseconds;
            if (time > 5000)
            {
                break;
            }
        }
        sw.Stop();
        if (1 == m_netManager.iGameState)
        {
            msg = m_netManager.strMessage;
            return true; ;
        }
        msg = m_netManager.strMessage;
        return false;
    }

    public void SendChat(string message)
    {
        NetBitStream stream = new NetBitStream();
        stream.BeginWrite((ushort)MessageIdentifiers.ID.ID_CHAT);
        stream.WriteString(message);
        stream.EncodeHeader();

        m_netManager.Send(stream);
    }


    // 发送聊天消息
    void SendChat()
    {
        // 将聊天消息写入NetBitStream对象
        NetBitStream stream = new NetBitStream();
        stream.BeginWrite((ushort)MessageIdentifiers.ID.ID_CHAT);
        stream.WriteString(m_inputString);
        stream.EncodeHeader();

        // 发送给服务器端
        m_netManager.Send(stream);

        //清空m_inputString
        m_inputString = "";
    }

    void SendChat2()
    {
        NetStructManager.TestStruct chatstr;
        chatstr.header = 0;
        chatstr.msgid = (ushort)MessageIdentifiers.ID.ID_CHAT2;
        chatstr.m = 0.1f;
        chatstr.n = 1000;
        chatstr.str = m_inputString;

        byte[] bytes = NetStructManager.getBytes(chatstr);

        NetBitStream stream = new NetBitStream();
        stream.CopyBytes(bytes);

        m_netManager.Send(stream);

        //清空m_inputString
        m_inputString = "";
    }

    private void SendControl()
    {

        NetBitStream stream = new NetBitStream();
        stream.BeginWrite((ushort)MessageIdentifiers.ID.PLAYER_ACTION);
        Player p = m_netManager.player;

        stream.WriteString(p.name);
        stream.WriteBool(p.first);
        stream.WriteFloat(p.pos_x);
        stream.WriteFloat(p.pos_y);
        stream.WriteFloat(p.hp);
        stream.WriteInt(p.ctype);
        int count = CGameManager.Instance.ControlList().Count;
        stream.WriteInt(count);

        List<byte> lc = CGameManager.Instance.ControlList();
        for (int i = 0; i < count; i++)
        {
            stream.WriteByte(lc[i]);
        }

        stream.EncodeHeader();

        m_netManager.Send(stream);
        CGameManager.Instance.ControlList().Clear();
    }
}
