using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyState
{
    Searching, 
    Trailing,
    Abandoning
}

public class EnemyBehaviour : MonoBehaviour {
    private IEnumerator activeCoroutine;
    private Pathfinding pathfinding;
    private EnemyState state;

	// Use this for initialization
	void Start () {
        pathfinding = GetComponent<Pathfinding>();
        activeCoroutine = pathfinding.SearchPlayer();
        StartCoroutine(activeCoroutine);
        state = EnemyState.Searching;
	}
	
	// Update is called once per frame
	void Update () {
		if(state == EnemyState.Searching)
        {
            // Check distance and change State to
            // Trailing if player is in Range
            
            // TODO
        }
        else if(state == EnemyState.Trailing)
        {
            // Check distance and change state to
            // Searching if player is out of range

            // TODO

            // Check if the player is hidden and change state to
            // Abandoning if so

            // TODO
        }
        else
        {
            // Check if the player suddenly is in range and visible
            // again and in this case change state to Trailing

            // TODO

            // Check if the enemy has been abandoning long enough and the
            // player didnt reappear

            if(pathfinding.TileDistToPLayer > 69)
            {
                state = EnemyState.Searching;
                StopCoroutine(activeCoroutine);
                activeCoroutine = pathfinding.SearchPlayer();
                StartCoroutine(activeCoroutine);
            }
        }
	}
}
