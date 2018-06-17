using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockControl : MonoBehaviour {

    public int unlocked = 0;

    public SpriteRenderer first;
    public SpriteRenderer second;
    public SpriteRenderer third;

    public Sprite unlock;
	// Use this for initialization
	public void increment() {
        unlocked++;
        first.sprite = unlock;
        if (unlocked > 1) second.sprite = unlock;
        if (unlocked > 2) third.sprite = unlock;

    }
}
