using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour {

    public Vector3 topPriority = new Vector2(115.2f, -28.65f);

    private void Update()
    {
        StartTask();
    }

    public void RemoveTask()
    {
        Destroy(gameObject);
    }

    public void StartTask()
    {
        // Check if the task is at the top of the tasks
        if(this.transform.position == topPriority)
        {
            // Start the task
            Debug.Log("Ay!");
        }
    }

}
