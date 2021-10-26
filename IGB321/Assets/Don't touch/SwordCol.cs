using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCol : MonoBehaviour
{
    public float deathTimer;
    public float damage = 1;
    public GameObject bigAttack;
    private GameObject bigEffect;
    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        deathTimer = Time.time + 0.3f;
        //damage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTimer)
        {
            Destroy(this.gameObject);
        }

        if (first == true && damage > 1)
        {
            bigEffect = Instantiate(bigAttack, GameObject.FindGameObjectWithTag("Trail").GetComponent<Transform>());
            bigEffect.transform.localPosition = new Vector3(0, 0, 0);
            first = false;
        }

        Debug.Log(damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.transform.GetComponent<Enemy>().takeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
