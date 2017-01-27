using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

    public float speed;
    public GUIText winText;
    public GUIText loseText;
    public GUIText countText;
    public GUIText deathCountText;
    public GUIText numberOfPickupsLeft;
    public GameObject insideCube;
    public GameObject mainCamera;

    private int count;
    private static int deathCount = 0;
    private int numberOfPickups;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        numberOfPickups = GameObject.FindGameObjectsWithTag("Pickup").GetLength(0);
        count = 0;
        SetCountText();
        SetDeathCountText();
        winText.text = "";
        loseText.text = "";
        numberOfPickupsLeft.text = "";
        SetTransparency();
        
    }

    private void SetTransparency()
    {
        Color color = gameObject.GetComponent<Renderer>().material.color;
        color.a = 0.1f; // Transparency parameter
        gameObject.GetComponent<Renderer>().material.color = color;
    }

    public void Update()
    {
        if (Input.GetKey("r"))
        {
            ResetGame();
        }
    }

    void FixedUpdate()
    {
        // Movement according to the camera direction
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = mainCamera.transform.forward * moveVertical * speed;
        movement += mainCamera.transform.right * moveHorizontal * speed;
        rb.AddForce(movement);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickup")
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            StartCoroutine(DisplayNumberOfPickupsText(2));                            
        }
        if (other.gameObject.tag == "Hole" || other.gameObject.tag == "AttackBot")
        {
            deathCount = deathCount + 1;
            Debug.Log("dcs : " + deathCount);
            SetLoseText(); 
            StartCoroutine(ResetAfterSeconds(3));
            Time.timeScale = 0f;
        }
        if (other.gameObject.tag == "MagicPickup")
        {
            // Change the color
            insideCube.GetComponent<Renderer>().material.color = 
                other.gameObject.transform.GetComponent<Renderer>().material.color;
            other.gameObject.SetActive(false);
        }
    }

    private IEnumerator DisplayNumberOfPickupsText(float delay)
    {
        SetNumberPickupsLeft();
        yield return new WaitForSeconds(delay);
        ResetNumberPickupsLeft();
    }

    private void ResetNumberPickupsLeft()
    {
        numberOfPickupsLeft.text = "";
        
    }

    private void SetNumberPickupsLeft()
    {
        if (GameObject.FindGameObjectsWithTag("Pickup").GetLength(0) > 0)
        {
            numberOfPickupsLeft.text = "Only " + GameObject.FindGameObjectsWithTag("Pickup").GetLength(0).ToString() + " left!";
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BlueSphere")
        {
            ContactPoint contact = collision.contacts[0];
            rb.AddForce(contact.normal * 800);
        }      
    }

    private IEnumerator ResetAfterSeconds(int seconds)
    {
        float pauseEndTime = Time.realtimeSinceStartup + seconds;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null;
        }
        ResetGame();
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= numberOfPickups)
        {
            SetWinText();
            Debug.Log("count : " + count.ToString() + " numberPickups : " + numberOfPickups.ToString());
            StartCoroutine(ResetAfterSeconds(3));
            Time.timeScale = 0f;
        }
    }

    void SetDeathCountText()
    {
        deathCountText.text = "Death Count: " + deathCount.ToString();
    }

    private void SetWinText()
    {
        winText.text = "You win!";
    }

    private void SetLoseText()
    {
        loseText.text = "You lose!";
    }

    void ResetGame()
    {
        Application.LoadLevel("MiniGameDone");
        Time.timeScale = 1f;
    }
}
