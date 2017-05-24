using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class CStart : MonoBehaviour {

    int m_counter;
	// Use this for initialization
	void Start () {
        m_counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
        m_counter++;
        if (m_counter > 120)
        {
            SceneManager.LoadScene("Scenes/gui/login");
        }
	}
}
