using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {

    public GameObject avatar;
    public GameObject sword;
    public GameObject swordCol;
    public GameObject swordColLocal;

    private Vector3 swordPos;
    private Quaternion swordRot;

    public float health = 100.0f;
    private bool dead = false;

    //Movement
    public float moveSpeed = 5;
    private Vector3 playerPosition;
    private Rigidbody rb;

    //Weapons
    public GameObject bullet;
    private float MGFireTime = 0.3f;
    private float MGFireTimer;
    public int ammo = 500;

    public GameObject fireDamage;
    private float FTFireTime = 0.2f;
    private float FTFireTimer;
    public int fuel = 50;

    public float diagDodgeSpeed = 20f;
    public float fullDodgeSpeed = 27f;
    public float dodgeTime = 0.25f;
    private float dodgeUntil;
    public bool dodgeActive = false;
    private string dodgeDir1 = "";
    private string dodgeDir2 = "";
    public float dodgeDelay = 0.8f;
    private float dodgeDelayTimer;

    public float dodgeMissTimer;
    private Vector3 position;

    Animation swordSwipe;

    // Use this for initialization
    void Start () {
        //flameStream.GetComponent<ParticleSystem>().Stop();
        rb = GetComponent<Rigidbody>();

        swordSwipe = sword.GetComponent<Animation>();
        swordPos = sword.GetComponent<Transform>().localPosition;
        swordRot = sword.GetComponent<Transform>().localRotation;
    }
	
	// Update is called once per frame
	void Update () {

        if (!GameManager.instance.playerDead) {
            DodgeStart();
            if (dodgeActive == true)
            {
                position = transform.position;
                Dodge();
                transform.position = position;
                swordSwipe.Stop();
                sword.transform.localPosition = swordPos;
                sword.transform.localRotation = swordRot;
            }
            else { Shooting(); }   
        }

        if (Time.time > dodgeDelayTimer) { Movement(); }
    }

    public void Shooting() {
        //Left Mouse Button
        if (Input.GetMouseButton(0)) {

            if (Time.time > MGFireTimer && dodgeActive == false && Time.time > dodgeDelayTimer) {
                swordSwipe.Play();
                Instantiate(swordCol, swordColLocal.transform);
                //swordColSpawned.transform.position = swordColLocal.transform.position;
                MGFireTimer = Time.time + MGFireTime;
            }

        }
        else {
            if (Time.time > MGFireTimer)
            {
                sword.transform.localPosition = swordPos;
                sword.transform.localRotation = swordRot;
            }
        }

        //Right Mouse Button
        if (Input.GetMouseButtonDown(1) && fuel >= 1) {
            //flameStream.GetComponent<ParticleSystem>().Play();
        }
        else if (Input.GetMouseButtonUp(1) && fuel >= 1) {
            //flameStream.GetComponent<ParticleSystem>().Stop();
        }
        else if (fuel <= 0) {
            //flameStream.GetComponent<ParticleSystem>().Stop();
        }

        if (Input.GetMouseButton(1) && fuel >= 1) {
            if (Time.time > FTFireTimer) {
                //Instantiate(fireDamage, flameStream.transform.position, transform.rotation);
                fuel -= 1;
                FTFireTimer = Time.time + FTFireTime;
            }
        }
    }

    public void Movement() {

        playerPosition = transform.position;

        //Forwards and Back
        if (Input.GetKey("w")) {
            playerPosition.z = playerPosition.z + moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("s")) {
            playerPosition.z = playerPosition.z - moveSpeed * Time.deltaTime;
        }

        //Strafing 
        if (Input.GetKey("a")) {
            playerPosition.x = playerPosition.x - moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey("d")) {
            playerPosition.x = playerPosition.x + moveSpeed * Time.deltaTime;
        }

        //Animation Controls - Move Vector Dot Product
        Vector3 moveVector = (playerPosition - transform.position).normalized;
        float direction = Vector3.Dot(moveVector, transform.forward);

        transform.position = playerPosition;
        rb.velocity = new Vector3(0,0,0);   //Freeze velocity
    }


    public void takeDamage(float damage) {

        health -= damage;

        if (health <= 0) {
            //Disable irrelvant animation bools
            //muzzleFlash.SetActive(false);
            //flameStream.GetComponent<ParticleSystem>().Stop();
            GameManager.instance.playerDead = true;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    private void DodgeStart()
    {
        if (Input.GetKey("space") && Time.time > dodgeDelayTimer)
        {
            if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))
            {
                if (Input.GetKey("w"))
                {
                    dodgeDir1 = "w";
                    if (Input.GetKey("a")) { dodgeDir2 = "a"; }
                    if (Input.GetKey("d")) { dodgeDir2 = "d"; }
                }
                if (Input.GetKey("a"))
                {
                    dodgeDir1 = "a";
                    if (Input.GetKey("w")) { dodgeDir2 = "w"; }
                    if (Input.GetKey("s")) { dodgeDir2 = "s"; }
                }
                if (Input.GetKey("s"))
                {
                    dodgeDir1 = "s";
                    if (Input.GetKey("a")) { dodgeDir2 = "a"; }
                    if (Input.GetKey("d")) { dodgeDir2 = "d"; }
                }
                if (Input.GetKey("d"))
                {
                    dodgeDir1 = "d";
                    if (Input.GetKey("w")) { dodgeDir2 = "w"; }
                    if (Input.GetKey("s")) { dodgeDir2 = "s"; }
                }

                dodgeActive = true;
                dodgeUntil = Time.time + dodgeTime;
                dodgeDelayTimer = Time.time + dodgeDelay + dodgeTime;
                dodgeMissTimer = Time.time + dodgeDelay + dodgeTime - 0.01f;
            }
        }
    }

    private void Dodge()
    {
        if (dodgeDir1 == "w")
        {
            if (dodgeDir2 == "a")
            {
                position.z += diagDodgeSpeed * Time.deltaTime;
                position.x -= diagDodgeSpeed * Time.deltaTime;
            }
            else if (dodgeDir2 == "d")
            {
                position.z += diagDodgeSpeed * Time.deltaTime;
                position.x += diagDodgeSpeed * Time.deltaTime;
            }
            else { position.z += fullDodgeSpeed * Time.deltaTime; }
        }
        if (dodgeDir1 == "a")
        {
            if (dodgeDir2 == "w")
            {
                position.x -= diagDodgeSpeed * Time.deltaTime;
                position.z += diagDodgeSpeed * Time.deltaTime;
            }
            else if (dodgeDir2 == "s")
            {
                position.x -= diagDodgeSpeed * Time.deltaTime;
                position.z -= diagDodgeSpeed * Time.deltaTime;
            }
            else { position.x -= fullDodgeSpeed * Time.deltaTime; }
        }
        if (dodgeDir1 == "s")
        {
            if (dodgeDir2 == "a")
            {
                position.z -= diagDodgeSpeed * Time.deltaTime;
                position.x -= diagDodgeSpeed * Time.deltaTime;
            }
            else if (dodgeDir2 == "d")
            {
                position.z -= diagDodgeSpeed * Time.deltaTime;
                position.x += diagDodgeSpeed * Time.deltaTime;
            }
            else { position.z -= fullDodgeSpeed * Time.deltaTime; }
        }
        if (dodgeDir1 == "d")
        {
            if (dodgeDir2 == "w")
            {
                position.x += diagDodgeSpeed * Time.deltaTime;
                position.z += diagDodgeSpeed * Time.deltaTime;
            }
            else if (dodgeDir2 == "s")
            {
                position.x += diagDodgeSpeed * Time.deltaTime;
                position.z -= diagDodgeSpeed * Time.deltaTime;
            }
            else { position.x += fullDodgeSpeed * Time.deltaTime; }
        }

        if (Time.time > dodgeUntil)
        {
            dodgeActive = false;
            dodgeDir1 = "";
            dodgeDir2 = "";
        }
    }

    //End of Level Goal Interaction
    public void OnTriggerEnter(Collider other) {

        if (other.tag == "Goal") {
            GameManager.instance.levelComplete = true;
            StartCoroutine(GameManager.instance.LoadLevel(GameManager.instance.nextLevel));
        }
    }
}
