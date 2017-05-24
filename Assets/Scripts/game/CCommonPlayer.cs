using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CCommonPlayer : MonoBehaviour
{

    public float speed = 0.1f;
    public List<GameObject> moveList;
    Transform m_transform;
    SpriteRenderer m_renderer;
    int m_counter;
    // Use this for initialization
    void Start()
    {
        m_transform = this.transform;
        m_renderer = this.GetComponent<SpriteRenderer>();
        m_counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_counter++;
    }

    public void CtlUpdate(byte b)
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
        switch (b)
        {
            case 0:
                {
                    m_transform.Translate(new Vector3(-1 * speed, 0, 0));
                    m_renderer.sprite = (Sprite)moveList[6 + index].GetComponent<SpriteRenderer>().sprite;
                }
                break;
            case 1:
                {
                    m_transform.Translate(new Vector3(speed, 0, 0));
                    m_renderer.sprite = (Sprite)moveList[9 + index].GetComponent<SpriteRenderer>().sprite;
                }
                break;
            case 2:
                {
                    m_transform.Translate(new Vector3(0, speed, 0));
                    m_renderer.sprite = (Sprite)moveList[0 + index].GetComponent<SpriteRenderer>().sprite;
                }
                break;
            case 3:
                {
                    m_transform.Translate(new Vector3(0, -1 * speed, 0));
                    m_renderer.sprite = (Sprite)moveList[3 + index].GetComponent<SpriteRenderer>().sprite;
                }
                break;
            default:
                break;
        }
    }
}
