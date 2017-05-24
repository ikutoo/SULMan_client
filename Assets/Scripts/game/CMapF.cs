using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class CMapF : MonoBehaviour
{

    const int map_w = 100;
    const int map_h = 100;
    const int obj_num = map_h * map_w / 20;
    public List<GameObject> tileList;
    private List<GameObject> m_tiles;
    // Use this for initialization
    void Start()
    {
        FileStream file = new FileStream("game_Data/Resources/mapf.data", FileMode.Open);
        StreamReader reader = new StreamReader(file);
        m_tiles = new List<GameObject>();
        for (int i = 0; i < obj_num; i++)
        {
            string line = reader.ReadLine();
            string[] word = line.Split(',');
            GameObject tmp = MonoBehaviour.Instantiate(tileList[int.Parse(word[0])]);
            int x = int.Parse(word[1]);
            int y = int.Parse(word[2]);
            Debug.Log(word[0] + "-" + word[1] + "-" + word[2]);
            tmp.transform.position = new Vector3((x - map_w / 2) * 0.64f, (y - map_h / 2) * 0.64f, -1);
            m_tiles.Add(tmp);
        }
        reader.Close();
        file.Close();
    }
    // Update is called once per frame
    void Update()
    {


    }
}
