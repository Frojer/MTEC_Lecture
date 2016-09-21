using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Vector3 fromVec = transform.position + transform.forward * 1.0f;
        Vector3 toVec = transform.position + transform.forward * -1.0f;

        Gizmos.DrawLine(fromVec, toVec);
    }
}
