using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour {
    private Joycon j;
    private List<Joycon> joycons;
    private int jc_ind = 0;

    public int scene1Index = 1;
    public GameObject story;
	// Use this for initialization
	void Start () {

        joycons = JoyconManager.Instance.j;
        j = joycons[jc_ind];
    }
	
	// Update is called once per frame
	void Update () {
        if (story.activeSelf)
        {
            if (j.GetButtonDown(Joycon.Button.DPAD_DOWN)
                         || j.GetButtonDown(Joycon.Button.DPAD_UP)
                         || j.GetButtonDown(Joycon.Button.DPAD_RIGHT)
                         || j.GetButtonDown(Joycon.Button.DPAD_LEFT))
            {
                SceneManager.LoadScene(scene1Index);

            }
        }
        else
        {
            if (j.GetButtonDown(Joycon.Button.DPAD_DOWN)
                         || j.GetButtonDown(Joycon.Button.DPAD_UP)
                         || j.GetButtonDown(Joycon.Button.DPAD_RIGHT)
                         || j.GetButtonDown(Joycon.Button.DPAD_LEFT))
            {
                story.SetActive(true);

            }
        }
    }
}
