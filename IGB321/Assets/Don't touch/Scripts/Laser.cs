using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public int projType;

    public float speed = 10.0f;
    public float damage = 5.0f;

    public float lifeTime = 1.5f;

    //Effects
    public GameObject hitEffect;

    private bool damaging = true;


    private void Start()
    {
        Destroy(this.gameObject, lifeTime);
        damaging = true;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (projType == 0)
        {
            transform.position += Time.deltaTime * speed * transform.forward;
        }
    }

    public void OnTriggerEnter(Collider otherObject) {

        if (projType == 0) // Default shot
        {
            if (otherObject.tag == "Player")
            {
                otherObject.GetComponent<PlayerAvatar>().takeDamage(damage);
                //Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
            else if (otherObject.tag == "Environment")
            {
                //Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
        }

        if (projType == 1) // Melee
        {
            if (otherObject.tag == "Player" && damaging == true)
            {
                otherObject.GetComponent<PlayerAvatar>().takeDamage(damage);
                //Instantiate(hitEffect, transform.position, transform.rotation);
                damaging = false;
            }
            
        }
    }
}
