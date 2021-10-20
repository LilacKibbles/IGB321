using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCol : MonoBehaviour
{

    public float deathTimer;

    // Start is called before the first frame update
    void Start()
    {
        deathTimer = Time.time + 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTimer)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().takeDamage(10);
            Debug.Log("shit");
            Destroy(this.gameObject);
        }
    }
}
