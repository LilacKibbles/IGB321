using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEffect : MonoBehaviour
{
    private float deathTimer;

    // Start is called before the first frame update
    void Start()
    {
        deathTimer = Time.time + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > deathTimer)
        {
            Destroy(this.gameObject);
        }
    }
}
