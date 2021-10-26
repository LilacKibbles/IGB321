using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    NavMeshAgent agent;

    public int enemyType;

    public GameObject player;

    public float health = 10.0f;

    public float agroRange = 10.0f;
    public float damage = 5.0f;

    //Rotation vars
    public float rotationSpeed;
    private float adjRotSpeed;
    public Quaternion targetRotation;

    //Laser Damage
    public GameObject attack;
    public GameObject attackMuzzle;
    private float attackTimer;
    public float attackTime = 1.0f;

    //Collision Damage
    private float damageTimer;
    private float damageTime = 0.5f;

    //private float takeDamageTimer;

    //public GameObject burning;
    //public GameObject explosion;

    private bool attacking = false;
    private int phase = 1;
    private bool colliding = false;
    private float delayTimer = 0;

    public GameObject fireDamage;
    private float FTFireTime = 0.2f;
    private float FTFireTimer;
    public GameObject flameStream;
    private bool firstRun = true;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {

        Behaviour();

        //Kill check - moved from takeDamage due to bug
        if (health <= 0) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAvatar>().accBlood += 1;
            //Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    void Behaviour() {

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");

        else if (player && !GameManager.instance.playerDead) {

            if (enemyType == 0)
            {
                //Raycast in direction of Player
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -(transform.position - player.transform.position).normalized, out hit, agroRange))
                {

                    //If Raycast hits player
                    if (hit.transform.tag == "Player")
                    {

                        Debug.DrawLine(transform.position, player.transform.position, Color.red);

                        //Rotate slowly towards player
                        targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                        adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

                        //Move towards player
                        if (Vector3.Distance(player.transform.position, transform.position) >= 5)
                        {
                            agent.SetDestination(player.transform.position);
                        }
                        //Stop if close to player
                        else if (Vector3.Distance(player.transform.position, transform.position) < 5)
                        {
                            agent.SetDestination(transform.position);
                        }

                        //Fire Laser
                        if (Time.time > attackTimer)
                        {
                            Instantiate(attack, attackMuzzle.transform.position, attackMuzzle.transform.rotation);
                            attackTimer = Time.time + attackTime;
                        }
                    }
                }
            }

            if (enemyType == 1)
            {
                //Raycast in direction of Player
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -(transform.position - player.transform.position).normalized, out hit, agroRange))
                {
                    //If Raycast hits player
                    if (hit.transform.tag == "Player")
                    {
                        
                        Debug.DrawLine(transform.position, player.transform.position, Color.red);

                        //Rotate slowly towards player
                        targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                        adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

                        //Move towards player
                        if (Vector3.Distance(player.transform.position, transform.position) >= 8)
                        {
                            agent.SetDestination(player.transform.position);
                        }
                        //Stop if close to player
                        else if (Vector3.Distance(player.transform.position, transform.position) < 8)
                        {
                            agent.SetDestination(transform.position);
                            attacking = true;
                        }

                        //Fire Laser
                        if (Time.time > attackTimer && attacking)
                        {
                            Instantiate(attack, attackMuzzle.transform.position, attackMuzzle.transform.rotation);
                            attackTimer = Time.time + attackTime;
                            attacking = false;
                        }
                    }
                }
            }

            if (enemyType == 2)
            {
                //Raycast in direction of Player
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -(transform.position - player.transform.position).normalized, out hit, agroRange))
                {

                    //If Raycast hits player
                    if (hit.transform.tag == "Player")
                    {
                        Debug.DrawLine(transform.position, player.transform.position, Color.red);

                        if (Time.time > attackTimer)
                        {
                            //Rotate slowly towards player
                            targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                            adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);
                        }

                        //Fire Laser
                        if (Time.time > attackTimer + 2)
                        {
                            Debug.Log("dadada");
                            Instantiate(attack, attackMuzzle.transform.position, attackMuzzle.transform.rotation);
                            attackTimer = Time.time + attackTime;
                        }
                    }
                }
            }

            if (enemyType == 3)
            {
                //Raycast in direction of Player
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -(transform.position - player.transform.position).normalized, out hit, agroRange))
                {

                    //If Raycast hits player
                    if (hit.transform.tag == "Player")
                    {

                        if (Time.time > attackTimer)
                        {
                            Debug.DrawLine(transform.position, player.transform.position, Color.red);

                            //Rotate slowly towards player
                            targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                            adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

                            //Move towards player
                            if (Vector3.Distance(player.transform.position, transform.position) >= 10)
                            {
                                agent.SetDestination(player.transform.position);
                            }
                            //Stop if close to player
                            else if (Vector3.Distance(player.transform.position, transform.position) < 10)
                            {
                                agent.SetDestination(transform.position);
                            }

                            //Fire Laser
                            if (Vector3.Distance(player.transform.position, transform.position) < 25)
                            {
                                attacking = true;
                                if (firstRun == true) { delayTimer = Time.time + 8; firstRun = false; }
                            }
                        }
                    }
                }

                if (Time.time > delayTimer && firstRun == false)
                {
                    attackTimer = Time.time + 3f;
                    firstRun = true;
                    attacking = false;
                }

                //Right Mouse Button
                if (attacking == true)
                {
                    if (flameStream.GetComponent<ParticleSystem>().isPlaying == false) 
                    { 
                        flameStream.GetComponent<ParticleSystem>().Play();
                        attackTimer = Time.time + attackTime;
                    }
                    
                    if (Time.time > FTFireTimer)
                    {
                        Instantiate(fireDamage, flameStream.transform.position, transform.rotation);
                        FTFireTimer = Time.time + FTFireTime;
                    }
                }
                else if (attacking == false)
                {
                    flameStream.GetComponent<ParticleSystem>().Stop();
                }
            }

            if (enemyType == 4)
            {
                //Raycast in direction of Player
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -(transform.position - player.transform.position).normalized, out hit, agroRange))
                {

                    //If Raycast hits player
                    if (hit.transform.tag == "Player")
                    {

                        Debug.DrawLine(transform.position, player.transform.position, Color.red);
                        Debug.Log(phase);

                        if (phase == 1)
                        {
                            attackTimer = Time.time + attackTime;
                            phase = 2;
                        }

                        else if (phase == 2)
                        {
                            //Rotate slowly towards player
                            targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                            adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

                            if (Time.time > attackTimer)
                            {
                                phase = 3;
                            }
                        }

                        else if (phase == 3)
                        {
                            //Rotate slowly towards player
                            targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                            adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);

                            //Move towards player
                            if (Vector3.Distance(player.transform.position, transform.position) >= 15)
                            {
                                agent.SetDestination(player.transform.position);
                                agent.isStopped = false;
                            }
                            //Stop if close to player
                            else if (Vector3.Distance(player.transform.position, transform.position) < 15)
                            {
                                agent.SetDestination(transform.position);
                                agent.isStopped = true;
                                delayTimer = Time.time + 0.3f;
                                phase = 4;
                            }

                        }

                        else if (phase == 4)
                        {
                            if (Time.time > delayTimer)
                            {
                                //Instantiate(attack, attackMuzzle.transform.position, attackMuzzle.transform.rotation);
                                attackTimer = Time.time + 0.6f;
                                phase = 5;
                            }
                        }

                        else if (phase == 5)
                        {
                            transform.position += Time.deltaTime * 50 * transform.forward;

                            if (Time.time > attackTimer)
                            {
                                phase = 1;
                            }
                        }

                       
                    }
                }
            }

            if (enemyType == 5)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, -(transform.position - player.transform.position).normalized, out hit, agroRange))
                {

                    //If Raycast hits player
                    if (hit.transform.tag == "Player")
                    {

                        Debug.DrawLine(transform.position, player.transform.position, Color.red);

                        if (Time.time > attackTimer)
                        {
                            //Rotate slowly towards player
                            targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
                            adjRotSpeed = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
                            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, adjRotSpeed);
                        }

                        //Fire Laser
                        if (Time.time > attackTimer + 2)
                        {
                            Instantiate(attack, attackMuzzle.transform.position, attackMuzzle.transform.rotation);
                            attackTimer = Time.time + attackTime;
                        }
                    }
                }
            }

        }
    }


    private void OnCollisionStay(Collision collision) {

        if (collision.transform.tag == "Player" && Time.time > damageTimer) {
            if (enemyType == 4) { collision.transform.GetComponent<PlayerAvatar>().takeDamage(2); }
            else { collision.transform.GetComponent<PlayerAvatar>().takeDamage(1); }
            damageTimer = Time.time + damageTime;
            //Debug.Log("shit");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliding = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }

    public void takeDamage(float thisDamage) {

        health -= thisDamage;
    }

}
