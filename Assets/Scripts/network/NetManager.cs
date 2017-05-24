using UnityEngine;
using System.Collections;
using AppNetWork;
using System.Collections.Generic;
public class NetManager : NetworkManager
{

    NetTCPClient client = null;
    public int iLoginState = 0;
    public int iRegisterState = 0;
    public int iGameState = 0;
    public string strMessage = "";
    public bool bConnected = false;
    public string strUsername = "";
    public Player player;

    // 世界中玩家列表
    public System.Collections.ArrayList playerList;

    NetPacket packet = null;

    // Use this for initialization
    public void Start()
    {
        client = new NetTCPClient();
        client.Connect("101.201.235.106", 10001);
        //client.Connect("220.167.41.5", 10001);
        //client.Connect("127.0.0.1", 10001);

        player = new Player();
        playerList = new System.Collections.ArrayList();
    }


    public void Send(NetBitStream stream)
    {
        client.Send(stream);
    }

    public override void Update()
    {
        for (packet = GetPacket(); packet != null;)
        {
            ushort msgid = 0;
            packet.TOID(out msgid);

            switch (msgid)
            {
                case (ushort)MessageIdentifiers.ID.CONNECTION_REQUEST_ACCEPTED:
                    {
                        MSG_CONNECTION_REQUEST_ACCEPTED();
                        break;
                    }
                case (ushort)MessageIdentifiers.ID.CONNECTION_ATTEMPT_FAILED:
                    {
                        MSG_CONNECTION_ATTEMPT_FAILED();
                        break;
                    }
                case (ushort)MessageIdentifiers.ID.CONNECTION_LOST:
                    {
                        MSG_CONNECTION_LOST();
                        break;
                    }
                case (ushort)MessageIdentifiers.ID.CLIENT_LOGIN_RESPONSE:
                    {
                        MSG_CLIENT_LOGIN_RESPONSE();
                        break;
                    }
                case (ushort)MessageIdentifiers.ID.CLIENT_REGISTER_RESPONSE:
                    {
                        MSG_CLIENT_REGISTER_RESPONSE();
                        break;
                    }
                case (ushort)MessageIdentifiers.ID.ID_CHAT:
                    {
                        MSG_ID_CHAT();
                        break;
                    }
                case (ushort)MessageIdentifiers.ID.ID_CHAT2:
                    {
                        MSG_ID_CHAT2();
                        break;
                    }
                case (ushort)MessageIdentifiers.ID.CLIENT_LINK_WORLD_RESPONSE:
                    {
                        MSG_CLIENT_LINK_WORLD_RESPONSE();
                        break;
                    }
                case (ushort)MessageIdentifiers.ID.CLIENT_LINK_WORLD:
                    {
                        MSG_CLIENT_LINK_WORLD();
                        break;
                    }
                case (ushort)MessageIdentifiers.ID.PLAYER_ACTION:
                    {
                        MSG_PLAYER_ACTION();
                        break;
                    }
                case (ushort)MessageIdentifiers.ID.CLIENT_EXIT_WORLD_RESPONSE:
                    {
                        MSG_CLIENT_EXIT_WORLD_RESPONSE();
                        break;
                    }
                default:
                    {
                        // 错误
                        break;
                    }
            }// end switch

            // 销毁数据包
            packet = null;

        }// end for


    } // end Update

    private void MSG_CONNECTION_REQUEST_ACCEPTED()
    {
        Debug.Log("连接到服务器");
        bConnected = true;
    }
    private void MSG_CONNECTION_ATTEMPT_FAILED()
    {
        Debug.Log("连接服务器失败,请退出");
        bConnected = false;
    }
    private void MSG_CONNECTION_LOST()
    {
        Debug.Log("失与服务器的连接,请按任意键退出");
        bConnected = false;
    }
    private void MSG_CLIENT_LOGIN_RESPONSE()
    {
        bool success;
        string msg = "";
        bool first;
        float pos_x;
        float pos_y;
        float hp;
        int ctype;
        NetBitStream stream = new NetBitStream();
        stream.BeginRead2(packet);
        stream.ReadBool(out success);
        stream.ReadString(out msg);

        //读取用户基本状态信息，并更新信息
        stream.ReadBool(out first);
        stream.ReadFloat(out pos_x);
        stream.ReadFloat(out pos_y);
        stream.ReadFloat(out hp);
        stream.ReadInt(out ctype);
        //更新用户基本信息
        player.name = strUsername;
        player.first = first;
        player.pos_x = pos_x;
        player.pos_y = pos_y;
        player.hp = hp;
        player.ctype = ctype;

        Debug.Log("$$$" + strUsername);
        Debug.Log(first);
        Debug.Log(pos_x);
        Debug.Log(pos_y);
        Debug.Log(hp);
        Debug.Log(ctype);

        if (success)
        {
            Debug.Log("登录成功！");
            iLoginState = 1;
        }
        else
        {
            Debug.Log("登录失败！");
            iLoginState = 2;
        }
        strMessage = msg;
    }
    private void MSG_CLIENT_REGISTER_RESPONSE()
    {
        bool success;
        string msg = "";
        NetBitStream stream = new NetBitStream();
        stream.BeginRead2(packet);
        stream.ReadBool(out success);
        stream.ReadString(out msg);
        if (success)
        {
            Debug.Log("注册成功！");
            iRegisterState = 1;
        }
        else
        {
            Debug.Log("注册失败！");
            iRegisterState = 2;
        }
        strMessage = msg;
    }
    private void MSG_ID_CHAT()
    {
        NetBitStream stream = new NetBitStream();
        string recvStr = "";
        stream.BeginRead2(packet);
        stream.ReadString(out recvStr);
        NetClient.Instance.RecvString = recvStr;
        //string _name = "";
        //foreach (Player p in playerList)
        //{
        //    if (p.peer.RemoteEndPoint.ToString().Equals(packet.peer.RemoteEndPoint.ToString()))
        //    {
        //        _name = p.name;
        //    }
        //}
        CMenuCallback.Instance.textlist.Add(packet.peer.RemoteEndPoint.ToString() + ": " + recvStr);
    }
    private void MSG_ID_CHAT2()
    {
        NetStructManager.TestStruct chatstr;
        chatstr = (NetStructManager.TestStruct)NetStructManager.fromBytes(packet.bytes, typeof(NetStructManager.TestStruct));

        NetClient.Instance.RecvString = chatstr.str;

        Debug.Log(chatstr.header);
        Debug.Log(chatstr.msgid);
        Debug.Log(chatstr.m);
        Debug.Log(chatstr.n);
        Debug.Log(chatstr.str);
    }
    private void MSG_CLIENT_LINK_WORLD_RESPONSE()
    {
        bool success;
        string msg = "";
        NetBitStream stream = new NetBitStream();
        stream.BeginRead2(packet);
        stream.ReadBool(out success);
        stream.ReadString(out msg);
        if (success)
        {
            Debug.Log("登入游戏世界成功**");
            iGameState = 1;
            // 从流中读取世界状态信息， 并写入
            int count;
            string name;
            bool first;
            float pos_x;
            float pos_y;
            float hp;
            int ctype;
            stream.ReadInt(out count);
            for (int i = 0; i < count; i++)
            {
                stream.ReadString(out name);
                stream.ReadBool(out first);
                stream.ReadFloat(out pos_x);
                stream.ReadFloat(out pos_y);
                stream.ReadFloat(out hp);
                stream.ReadInt(out ctype);

                Player p = new Player(name, first, pos_x, pos_y, hp, ctype);
                playerList.Add(p);
                CGameManager.Instance.InitPlayer(p);
            }
        }
        else
        {
            Debug.Log("登录游戏世界失**");
            iGameState = 2;
        }
        strMessage = msg;
    }

    private void MSG_CLIENT_LINK_WORLD()
    {
        NetBitStream stream = new NetBitStream();
        stream.BeginRead2(packet);

        // 从流中读取新登陆玩家状态信息， 并写入
        string name;
        bool first;
        float pos_x;
        float pos_y;
        float hp;
        int ctype;

        stream.ReadString(out name);
        stream.ReadBool(out first);
        stream.ReadFloat(out pos_x);
        stream.ReadFloat(out pos_y);
        stream.ReadFloat(out hp);
        stream.ReadInt(out ctype);

        Player p = new Player(name, first, pos_x, pos_y, hp, ctype);
        playerList.Add(p);
        CGameManager.Instance.AddPlayer(p);
    }

    private void MSG_PLAYER_ACTION()
    {
        //读取流信息，更新玩家状态
        NetBitStream stream = new NetBitStream();
        stream.BeginRead2(packet);

        string name;
        bool first;
        float pos_x;
        float pos_y;
        float hp;
        int ctype;

        stream.ReadString(out name);
        stream.ReadBool(out first);
        stream.ReadFloat(out pos_x);
        stream.ReadFloat(out pos_y);
        stream.ReadFloat(out hp);
        stream.ReadInt(out ctype);

        Player player = null;
        foreach (Player p in playerList)
        {
            if (p.name.Equals(name))
            {
                player = p;
            }
        }

        player.name = name;
        player.first = first;
        player.pos_x = pos_x;
        player.pos_y = pos_y;
        player.hp = hp;
        player.ctype = ctype;

        List<byte> lc = new List<byte>();
        int count;
        stream.ReadInt(out count);
        for (int i = 0; i < count; i++)
        {
            byte b;
            stream.ReadByte(out b);
            lc.Add(b);
        }
        //传递控制信息
        CGameManager.Instance.UpdateControl(name, lc);
        Debug.Log("recv control ****");
        Debug.Log(name);
        Debug.Log(first);
        Debug.Log(pos_x);
        Debug.Log(pos_y);
        Debug.Log(hp);
        Debug.Log(ctype);
        string str = "";
        foreach (byte b in lc)
        {
            str += b + "-";
        }
        Debug.Log(str);
    }

    private void MSG_CLIENT_EXIT_WORLD_RESPONSE()
    {
        //to do ,读取流信息， 跟新玩家列表与信息
        NetBitStream stream = new NetBitStream();
        stream.BeginRead2(packet);
        string name = "";
        stream.ReadString(out name);
        foreach (Player p in playerList)
        {
            if (p.name.Equals(name))
            {
                playerList.Remove(p);
                CGameManager.Instance.RemovePlayer(name);
                break;
            }
        }
    }
}// end file

