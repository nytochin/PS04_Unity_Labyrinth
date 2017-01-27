using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float distance;
    public float heightOffset;
    public float sharpness;
    private Vector3 offset;

	void Start () {
        offset = transform.position;
        offset.z = 0;
	}
	
	void LateUpdate () {
        Vector3 velocityPlayer = player.GetComponent<Rigidbody>().velocity.normalized;
        if (velocityPlayer.Equals(Vector3.zero))
        {
            transform.position = player.transform.position + new Vector3(1.0f, 0.0f, 0.0f) * -distance + offset;
        }
        else
        {
            transform.position = player.transform.position + velocityPlayer * -distance + offset;
        }
        Vector3 toTarget = player.transform.position - transform.position;
        toTarget.y += heightOffset;

        // This constructs a rotation looking in the direction of our target
        Quaternion targetRotation = Quaternion.LookRotation(toTarget);

        // This blends the target rotation in gradually.
        // Keep sharpness between 0 and 1 - lower values are slower/softer.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, sharpness);           
      
        //Debug.Log("velocity : " + velocityPlayer.ToString());
        
    }
}
