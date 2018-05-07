using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Pathfinding;
using Game;

[RequireComponent(typeof(Worker))]
public class TestPathfind : MonoBehaviour {

	PathfindingAgent agent;
	Worker livingWorker;
	public TilePosition tileToGoTo;
	public bool start;

	// Use this for initialization
	void Start () {
		agent = new PathfindingAgent(new BasicRule(), WorldController.Instance.MainState.TileMap);
		livingWorker = GetComponent<Worker>();
		StartCoroutine(MoveTo());
	}
	
	// Update is called once per frame
	void Update () {
		if(start){
			agent.GeneratePath(TilePosition.FromWorldPosition(livingWorker.transform.position), tileToGoTo);
			start = false;
		}
	}

	IEnumerator MoveTo()
    {		 
        while(agent.HasTarget)
        {
            livingWorker.MoveTo(agent.GetNextTile().Position);
            Debug.Log(livingWorker.MoveToTarget.ToString());
            while(livingWorker.MoveToTarget != null)
            {
                yield return new WaitForEndOfFrame();
            }
        }
		yield return new WaitForEndOfFrame();
        yield return StartCoroutine(MoveTo());
    }
}
