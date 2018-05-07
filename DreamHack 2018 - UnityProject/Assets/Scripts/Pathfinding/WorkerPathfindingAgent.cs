using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Game.Pathfinding;

[RequireComponent(typeof(Worker))]
public class WorkerPathfindingAgent : MonoBehaviour {

	public TilePosition endPos;
	public bool startMoving;

	void Update(){
		if(startMoving == true){
			StartCoroutine(moveTo());
			startMoving = false;
		}
	}

	IEnumerator moveTo(){
		Worker livingWorker = GetComponent<Worker>();
		Queue<TilePosition> posList = livingWorker.FindPath(WorldController.Instance.MainState.TileMap, TilePosition.FromWorldPosition(livingWorker.transform.position), endPos);
		Debug.Log("Player is currently at: " + TilePosition.FromWorldPosition(livingWorker.transform.position).ToString());
		while(posList.Count > 0){
			livingWorker.MoveTo(posList.Dequeue());
			Debug.Log(livingWorker.MoveToTarget.ToString());
			while(livingWorker.MoveToTarget != null){
				yield return new WaitForEndOfFrame();
			}
		}
		yield return null;
	}
}
