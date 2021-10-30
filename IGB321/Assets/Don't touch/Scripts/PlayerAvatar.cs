using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAvatar : MonoBehaviour {

    public GameObject avatar;
    public GameObject sword;
    public GameObject swordCol;
    public GameObject swordColLocal;
    public GameObject swordColSpawned;

    private Vector3 swordPos;
    private Quaternion swordRot;

    public float health = 10f;
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

    public int accBlood = 0;
    public int maxBlood = 10;

    public GameObject burning;

    // Use this for initialization
    void Start () {
        //flameStream.GetComponent<ParticleSystem>().Stop();
        rb = GetComponent<Rigidbody>();

        swordSwipe = sword.GetComponent<Animation>();
        swordPos = sword.GetComponent<Transform>().localPosition;
        swordRot = sword.GetComponent<Transform>().localRotation;
        health = 10;
        accBlood = 0;
        maxBlood = 10;
    }
	
	// Update is called once per frame
	void Update () {

        if (!GameManager.instance.playerDead) {
            DodgeStart();

            GameManager.instance.health.value = health;
            if (accBlood > maxBlood) { accBlood = maxBlood; }
            GameManager.instance.blood.value = accBlood;

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
                swordColSpawned = Instantiate(swordCol, swordColLocal.transform);
                swordColSpawned.GetComponent<SwordCol>().damage = 1;
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

        if (Input.GetKeyDown(KeyCode.Q) && accBlood > 0)
        {
            if (Time.time > MGFireTimer && dodgeActive == false && Time.time > dodgeDelayTimer)
            {
                swordSwipe.Play();
                swordColSpawned = Instantiate(swordCol, swordColLocal.transform);
                swordColSpawned.GetComponent<SwordCol>().damage = 1 + accBlood;
                MGFireTimer = Time.time + MGFireTime;

                if (accBlood == 1 || accBlood == 2 || accBlood == 3) 
                { 
                    maxBlood -= 1;
                    GameManager.instance.blood.maxValue = maxBlood;
                    GameManager.instance.blood.transform.localScale = new Vector3(GameManager.instance.blood.transform.localScale.x - 0.1f, 1f, 1f);
                    GameManager.instance.blood.transform.position = new Vector3(GameManager.instance.blood.transform.position.x - 7f, GameManager.instance.blood.transform.position.y, 0f);
                }
                else if (accBlood == 4 || accBlood == 5 || accBlood == 6) 
                { 
                    maxBlood -= 2;
                    GameManager.instance.blood.maxValue = maxBlood;
                    GameManager.instance.blood.transform.localScale = new Vector3(GameManager.instance.blood.transform.localScale.x - 0.2f, 1f, 1f);
                    GameManager.instance.blood.transform.position = new Vector3(GameManager.instance.blood.transform.position.x - 14f, GameManager.instance.blood.transform.position.y, 0f);
                }
                else if (accBlood >= 7) 
                { 
                    maxBlood -= 3;
                    GameManager.instance.blood.maxValue = maxBlood;
                    GameManager.instance.blood.transform.localScale = new Vector3(GameManager.instance.blood.transform.localScale.x - 0.3f, 1f, 1f);
                    GameManager.instance.blood.transform.position = new Vector3(GameManager.instance.blood.transform.position.x - 21f, GameManager.instance.blood.transform.position.y, 0f);
                }

                accBlood = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && accBlood > 0)
        {
            health += accBlood;
            if (health > 10) { health = 10; }

            if (accBlood == 1 || accBlood == 2 || accBlood == 3)
            {
                maxBlood -= 1;
                GameManager.instance.blood.maxValue = maxBlood;
                GameManager.instance.blood.transform.localScale = new Vector3(GameManager.instance.blood.transform.localScale.x - 0.1f, 1f, 1f);
                GameManager.instance.blood.transform.position = new Vector3(GameManager.instance.blood.transform.position.x - 7f, GameManager.instance.blood.transform.position.y, 0f);
            }
            else if (accBlood == 4 || accBlood == 5 || accBlood == 6)
            {
                maxBlood -= 2;
                GameManager.instance.blood.maxValue = maxBlood;
                GameManager.instance.blood.transform.localScale = new Vector3(GameManager.instance.blood.transform.localScale.x - 0.2f, 1f, 1f);
                GameManager.instance.blood.transform.position = new Vector3(GameManager.instance.blood.transform.position.x - 14f, GameManager.instance.blood.transform.position.y, 0f);
            }
            else if (accBlood >= 7)
            {
                maxBlood -= 3;
                GameManager.instance.blood.maxValue = maxBlood;
                GameManager.instance.blood.transform.localScale = new Vector3(GameManager.instance.blood.transform.localScale.x - 0.3f, 1f, 1f);
                GameManager.instance.blood.transform.position = new Vector3(GameManager.instance.blood.transform.position.x - 21f, GameManager.instance.blood.transform.position.y, 0f);
            }

            accBlood = 0;
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
            GameManager.instance.health.value = 0;
            Destroy(this.gameObject);
            StartCoroutine(GameManager.instance.LoadLevel(GameManager.instance.thisLevel));
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
            Debug.Log("yes");
            Destroy(other.gameObject);
            StartCoroutine(GameManager.instance.LoadLevel(GameManager.instance.nextLevel));
        }
    }
}
