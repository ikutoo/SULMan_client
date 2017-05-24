using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HUDlife_change : MonoBehaviour {

    private HUDText test;
    // Use this for initialization
    void Start()
    {

        test = this.GetComponent<HUDText>();

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            test.Add(-10, Color.red, 1);
        }

    }
}
