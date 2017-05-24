using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AppNetWork;

public class CPlayer : MonoBehaviour
{

    public float speed = 0.1f;
    public List<GameObject> moveList;
    Transform m_transform;
    SpriteRenderer m_renderer;
    CGameManager m_gameManager;
    int m_counter;
    // Use this for initialization
    void Start()
    {
        m_transform = this.transform;
        m_renderer = this.GetComponent<SpriteRenderer>();
        m_counter = 0;
        m_gameManager = CGameManager.Instance;

    }
    void FixedUpdate()
    {
        //this.rigidbody.velocity = new Vector3(0, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        mMove();
        m_counter++;

        Player p = NetClient.Instance.Player();
        p.pos_x = m_transform.position.x;
        p.pos_y = m_transform.position.y;
    }

    private void mMove()
    {
        int index;
        if (m_counter % 30 < 10)
        {
            index = 0;
        }
        else if (m_counter % 30 < 20)
        {
            index = 1;
        }
        else
        {
            index = 2;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            m_transform.Translate(new Vector3(-1 * speed, 0, 0));
            m_renderer.sprite = (Sprite)moveList[6 + index].GetComponent<SpriteRenderer>().sprite;
            m_gameManager.ControlList().Add(0);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            m_transform.Translate(new Vector3(speed, 0, 0));
            m_renderer.sprite = (Sprite)moveList[9 + index].GetComponent<SpriteRenderer>().sprite;
            m_gameManager.ControlList().Add(1);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            m_transform.Translate(new Vector3(0, speed, 0));
            m_renderer.sprite = (Sprite)moveList[0 + index].GetComponent<SpriteRenderer>().sprite;
            m_gameManager.ControlList().Add(2);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            m_transform.Translate(new Vector3(0, -1 * speed, 0));
            m_renderer.sprite = (Sprite)moveList[3 + index].GetComponent<SpriteRenderer>().sprite;
            m_gameManager.ControlList().Add(3);
        }
    }

    void OnTriggerEnter2D()
    {
        Debug.Log("Enter**************");
    }

    void OnTriggerExit2D()
    {
        Debug.Log("Exit**************");
    }
}
