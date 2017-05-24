using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class CMap : MonoBehaviour
{
    public static CMap Instance = null;
    public List<GameObject> tileList;
    const int map_w = 100;
    const int map_h = 100;
    public int[,] map;


    // Use this for initialization
    void Start()
    {
        Instance = this;
        //System.Random random = new System.Random();
        map = new int[map_w, map_h];

        FileStream file = new FileStream("game_Data/Resources/map.data", FileMode.Open);

        StreamReader reader = new StreamReader(file);

        int n1 = 0, n2 = 0;
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] word = line.Split(',');
            foreach (string s in word)
            {
                map[n1, n2] = int.Parse(s);

                if (n2 < map_w - 1)
                {
                    n2++;
                }
                else
                {
                    n2 = 0;
                    n1++;
                    if (n1 >= map_h)
                    {
                        break;
                    }
                }
            }
        }
        reader.Close();
        file.Close();

        for (int i = 0; i < map_w; ++i)
        {
            for (int j = 0; j < map_h; ++j)
            {
                GameObject tmp = MonoBehaviour.Instantiate(tileList[map[i, j]]);
                tmp.transform.position = new Vector3((i - map_w / 2) * 0.64f, (j - map_h / 2) * 0.64f, 1);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    int GetTileType(int i, int j, int t)
    {
        int size = 2;
        if (i < size || i >= map_w - size || j < size || j >= map_h - size)
        {
            return t;
        }
        int[] num = new int[tileList.Count];
        for (int k = 0; k < tileList.Count; k++)
        {
            num[k] = 0;
        }
        for (int r = i - size; r <= i + size; r++)
        {
            for (int w = j - size; w <= j + size; w++)
            {
                num[map[r, w]]++;
            }
        }

        int max = -1;
        int type = 0;
        for (int k = 0; k < tileList.Count; k++)
        {
            if (num[k] > max)
            {
                max = num[k];
                type = k;
            }
        }
        return type;
    }
}
