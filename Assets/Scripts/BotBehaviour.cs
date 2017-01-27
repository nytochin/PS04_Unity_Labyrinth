using UnityEngine;
using System.Collections;
using System;

public class BotBehaviour : MonoBehaviour {

    public GameObject sphere;
    // The bot moves back and forth from PositionA to PositionB
    public Vector3 PositionA; 
    public Vector3 PositionB;
    // The bot rotation changes depending on its destination
    public Vector3 RotationA;
    public Vector3 RotationB;
    public float speed;
    public float rotationSpeed;
    public float secondsPerSphere;

    private Vector3 startPosition;    
    private Vector3 endPosition;
    private Vector3 startRotation;
    private Vector3 endRotation;
    private float startTime;
    private float journeyLength;
    private float sphereDropTime;

    void Start () {
        startPosition = PositionA;
        endPosition = PositionB;
        startRotation = RotationA;
        endRotation= RotationA;
        startTime = Time.time;
        sphereDropTime = Time.time;
        journeyLength = Vector3.Distance(startPosition, endPosition);

    }
	
	void Update () {
        if (Time.time - sphereDropTime > secondsPerSphere)
        {          
            DropRandomColorSphere();         
            sphereDropTime = Time.time;
        }

        if (Vector3.Distance(endPosition, gameObject.transform.position) < 0.1)
        {
            SwitchPositionRotation();
            startTime = Time.time;
            journeyLength = Vector3.Distance(startPosition, endPosition);
        }
        else
        {
            Move();
            Rotate();          
        }
	}

    private void Move()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(startPosition, endPosition, fracJourney);
        // Debug.Log("endPosition " +  endPosition.ToString());
    }

    private void Rotate()
    {
        startRotation = new Vector3(
                 Mathf.LerpAngle(startRotation.x, endRotation.x, Time.deltaTime),
                 Mathf.LerpAngle(startRotation.y, endRotation.y, Time.deltaTime),
                 Mathf.LerpAngle(startRotation.z, endRotation.z, Time.deltaTime));
        transform.eulerAngles = startRotation;
    }

    private void DropRandomColorSphere()
    {
        GameObject magicPickup =
               (GameObject)Instantiate(
                   sphere,
                   new Vector3(gameObject.transform.position.x, 0.5f,
                   gameObject.transform.position.z),
                   Quaternion.identity);
        Color color = magicPickup.GetComponent<Renderer>().material.color;
        System.Random random = new System.Random();
        color.r = (float)random.NextDouble();
        color.g = (float)random.NextDouble();
        color.b = (float)random.NextDouble();
        Debug.Log(color.ToString());
        magicPickup.GetComponent<Renderer>().material.color = color;
    }

    private void SwitchPositionRotation()
    {
        if (endPosition == PositionA)
        {
            endPosition = PositionB;
            startPosition = PositionA;
            endRotation = RotationA;
            startRotation = RotationB;
        }
        else
        {
            endPosition = PositionA;
            startPosition = PositionB;
            endRotation = RotationB;
            startRotation = RotationA;
        }
    }
}
