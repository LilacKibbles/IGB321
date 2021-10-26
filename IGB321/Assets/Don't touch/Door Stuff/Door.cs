using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;

    private Quaternion leftPos;
    private Quaternion rightPos;

    private Animation left;
    private Animation right;

    public int doorId;
    public bool isUnlocked = true;

    // Start is called before the first frame update
    void Start()
    {
        left = leftDoor.GetComponent<Animation>();
        right = rightDoor.GetComponent<Animation>();

        leftPos = leftDoor.transform.rotation;
        rightPos = rightDoor.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider otherObject)
    { 
        if (otherObject.transform.tag == "Player" && isUnlocked == true)
        {
            left.Play();
            right.Play();
        }
    }

    
    public void OnTriggerExit(Collider otherObject)
    {
        if (otherObject.transform.tag == "Player")
        {
            left.Stop();
            right.Stop();
            leftDoor.transform.rotation = leftPos;
            rightDoor.transform.rotation = rightPos;
        }
    }
    

}
