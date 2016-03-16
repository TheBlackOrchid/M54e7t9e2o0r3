﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float speed = 5f;
   
    private Rigidbody2D rb;
    private Animator anim;
    private Touch currTouch;
    private bool started = false;
    private int direction = 0;


    void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}


    void Update() {
        if (!started) {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) {
                started = true;
                direction = 1;
                anim.SetBool("started", true);
            }
        } else {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) {
#if UNITY_EDITOR
                direction *= -1;
#elif UNITY_ANDROID
                if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    direction *= -1;
                }
#endif
            }
        }
    }


    void FixedUpdate () {

        rb.MovePosition(transform.position + Vector3.right * speed * direction * Time.deltaTime);
        Vector3 theScale = transform.localScale;
        if (direction != 0) {
            theScale.x = direction;
        }
        transform.localScale = theScale;

    }

  
}