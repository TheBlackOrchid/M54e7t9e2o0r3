﻿using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

    public ScoreController scoreController;
    public GameObject projectilePrefab; //ball prefab
    public InputHeandler inputHeandler;
    public bool spawnEnabled = true;
    [Header("Pools parameters")]
    public int poolSize = 2;
    public float spawnRate = 0.5f;
    public float offsetX = 1f;
    [Header("Projectiles settings")]
    public float minSpeed = 2f;
    public float maxSpeed = 10f;

    private GameObject ProjectilePool;
    private GameObject currProjectile;
    private Transform[] projectiles;
    private Transform myTransform;
    private Rigidbody2D currProjectileRb;
    private ProjectileController currProjectileController;
    private PlayerController playerController;
    private int currProjectileIndex;
    private bool canSpawn = true;
    private float randomX;
    private float extent;
    private float speed;

    // Use this for initialization
    void Start() {
        CreatePool("ProjectilePool");
        myTransform = transform;
        currProjectileIndex = 0;
        extent = Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x + offsetX;
    }

    // Update is called once per frame
    void Update() {
        if (!spawnEnabled) {
            if (inputHeandler.started) {
                spawnEnabled = true;
            }
        }
        if (spawnEnabled && canSpawn) { 
			StartCoroutine(SpawnCoroutine()); 
		} // running the coroutine
    }

    IEnumerator SpawnCoroutine() {
        // initialization
        canSpawn = false;
		yield return new WaitForSeconds(spawnRate);
        currProjectile = ProjectilePool.transform.GetChild(currProjectileIndex).gameObject;

        //if (playerController.GetComponent<Animation>() != dead ) {
            //if (!currProjectile.activeInHierarchy) {
            currProjectileRb = currProjectile.GetComponent<Rigidbody2D>();
            currProjectile.SetActive(true);

            // transformation
            randomX = Random.Range(-extent, extent);
            speed = Random.Range(minSpeed, maxSpeed);
            currProjectile.transform.position = new Vector3(randomX, myTransform.position.y, myTransform.position.z);
            currProjectile.transform.rotation = myTransform.rotation;
            currProjectileRb.angularVelocity = 0;
            currProjectileRb.velocity = Vector2.down * speed;

            // cooldown
            //}

            // ready to go
            if (currProjectileIndex < poolSize - 1) { currProjectileIndex++; } else { currProjectileIndex = 0; } // if ran out of balls - take the first
            canSpawn = true;
        //} 

    }

    void CreatePool(string name) {
        ProjectilePool = new GameObject(); // creating ball pool
        ProjectilePool.name = name; // naming it
        ProjectilePool.transform.position = Vector3.up * 10; // setting its position

        for (int i = 0; i < poolSize; i++) // populating pool
        {
            currProjectile = (GameObject)Instantiate(projectilePrefab); // new ball from prefab
            currProjectile.transform.SetParent(ProjectilePool.transform); // parenting to pool
            currProjectile.transform.localPosition = Vector3.zero; // teleporting ball to pool's position
            currProjectileController = currProjectile.GetComponent<ProjectileController>();
            currProjectileController.scoreController = scoreController;
            currProjectile.SetActive(false); // deactivating for now
        }
        currProjectile = null; // must be null for further actions (in case)
    }
}
