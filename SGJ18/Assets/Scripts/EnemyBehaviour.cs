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
    private EnemyState state;

    private Pathfinding pathfinding;
    public PlayerMovement playerMovement;

    public int enemyViewDist = 50;

	// Use this for initialization
	void Start () {
        pathfinding = GetComponent<Pathfinding>();
        activeCoroutine = pathfinding.SearchPlayer();
        StartCoroutine(activeCoroutine);
        state = EnemyState.Searching;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(state);
		if(state == EnemyState.Searching)
        {
            // Check distance and change State to
            // Trailing if player is in Range
            
            if(pathfinding.TileDistToPLayer < enemyViewDist)
            {
                StateTransition(pathfinding.TrailPlayer(), EnemyState.Trailing);
            }
        }
        else if(state == EnemyState.Trailing)
        {
            // Check distance and change state to
            // Searching if player is out of range

            if(pathfinding.TileDistToPLayer > enemyViewDist)
            {
                StateTransition(pathfinding.SearchPlayer(), EnemyState.Searching);
            }

            // Check if the player is hidden and change state to
            // Abandoning if so

            if (playerMovement.isHidden || Input.GetKey(KeyCode.LeftControl))
            {
                StateTransition(pathfinding.AbandonPlayer(), EnemyState.Abandoning);
            }
        }
        else
        {
            // Check if the player suddenly is in range and visible
            // again and in this case change state to Trailing

            if(pathfinding.TileDistToPLayer < enemyViewDist && !playerMovement.isHidden || Input.GetKey(KeyCode.LeftControl))
            {
                StateTransition(pathfinding.TrailPlayer(), EnemyState.Trailing);
            }

            // Check if the enemy has been abandoning long enough and the
            // player didnt reappear

            if(pathfinding.TileDistToPLayer > 69)
            {
                StateTransition(pathfinding.SearchPlayer(), EnemyState.Searching);
            }
        }
	}

    private void StateTransition(IEnumerator newCoroutine, EnemyState newState)
    {
        // Maybe stopping the inner coroutine is also mandatory
        state = newState;
        StopCoroutine(activeCoroutine);
        activeCoroutine = newCoroutine;
        StartCoroutine(activeCoroutine);
    }
}
