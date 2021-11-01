﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //Singleton Setup
    public static GameManager instance = null;

    public bool playerDead = false;

    public bool levelComplete = false;

    public string thisLevel;
    public string nextLevel;

    public Slider blood;
    public Slider health;

    // Awake Checks - Singleton setup
    void Awake() {

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        thisLevel = SceneManager.GetActiveScene().name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator LoadLevel(string level) {

        yield return new WaitForSeconds(3);

        if (level == "End") { Application.Quit(); }
        else
        {
            SceneManager.LoadScene(level);
        }
    }


}
