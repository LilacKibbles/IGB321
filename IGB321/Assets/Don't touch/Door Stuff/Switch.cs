using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public int switchId;
    public GameObject plate;

    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.transform.tag == "Player" && activated == false)
        {
            foreach (GameObject x in GameObject.FindGameObjectsWithTag("Door"))
            {
                if (x.GetComponent<Door>().doorId == switchId)
                {
                    x.GetComponent<Door>().isUnlocked = true;
                }
            }

            plate.transform.position = new Vector3(plate.transform.position.x, plate.transform.position.y - 0.8f, plate.transform.position.z);

            activated = true;
        }

    }
}
