using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AppNetWork;
public class CGameManager : MonoBehaviour
{

    public static CGameManager Instance = null;
    public List<CPlayer> playerList;
    public List<CCommonPlayer> commonPlayerList;
    private List<byte> m_controlList;
    private Dictionary<string, CCommonPlayer> m_players;
    private List<Player> m_init_players;
    private CPlayer m_player;
    // Use this for initialization

    void Awake()
    {
        //修改当前的FPS
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        m_players = new Dictionary<string, CCommonPlayer>();
        m_init_players = new List<Player>();
        m_controlList = new List<byte>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<byte> ControlList()
    {
        return m_controlList;
    }

    public void InitPlayer(Player p)
    {
        m_init_players.Add(p);
    }

    public void RemovePlayer(string name)
    {
        CCommonPlayer p = m_players[name];
        Destroy(p.gameObject);
        if (p != null)
            commonPlayerList.Remove(p);
        m_players.Remove(name);
    }

    public void Init()
    {
        foreach (Player p in m_init_players)
        {
            CCommonPlayer tmp = MonoBehaviour.Instantiate(commonPlayerList[p.ctype]);
            tmp.transform.position = new Vector3(p.pos_x, p.pos_y, 0);
            m_players.Add(p.name, tmp);
            Debug.Log("add player***");
        }
        m_init_players.Clear();

        Player mp = NetClient.Instance.Player();
        CPlayer player = MonoBehaviour.Instantiate(playerList[mp.ctype]);
        player.transform.position = new Vector3(mp.pos_x, mp.pos_y, 0);
        m_player = player;
    }

    public void AddPlayer(Player p)
    {
        CCommonPlayer tmp = MonoBehaviour.Instantiate(commonPlayerList[p.ctype]);
        tmp.transform.position = new Vector3(p.pos_x, p.pos_y, 0);
        m_players.Add(p.name, tmp);
        Debug.Log("add player***");
    }

    public void UpdateControl(string name, List<byte> list)
    {
        CCommonPlayer cp = m_players[name];
        //to do 根据list控制player
        foreach (byte b in list)
        {
            cp.CtlUpdate(b);
        }
    }
}
