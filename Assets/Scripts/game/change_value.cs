using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class change_value : MonoBehaviour {

    public UISlider slider;

	// Use this for initialization
	void Start () {
        slider = this.GetComponent<UISlider>();
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)){
            slider.value = slider.value - 0.1f;
            if(slider.value == 0){
                SceneManager.LoadScene("Scenes/gui/choose");
            }
        }
	
	}
}
