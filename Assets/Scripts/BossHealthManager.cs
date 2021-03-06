﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthManager : MonoBehaviour {

    public int enemyHealth;
    public GameObject deathEffect;
    public int pointsOnDeath;

    public AudioClip acHurt;

    public GameObject bossPrefab;

    public float smallestSize;
    private Transform _tr;
    private Transform _trParent;
    public GameObject Wall;



    // Use this for initialization
    void Start() {
       _tr = GetComponent <Transform>();
        _trParent = _tr.parent;
       
    }

    void CheckLive () {
        if (enemyHealth <= 0) {

            Messenger.Broadcast("CheckChildren");

            deathEffect.Spawn(_tr.position, _tr.rotation);
            
            if (_tr.localScale.y > smallestSize) {
                GameObject clone1 = Instantiate(bossPrefab,
                    new Vector3(_tr.position.x + 0.5f, _tr.position.y - _tr.position.y * 0.5f, _tr.position.z),
                    _tr.rotation, _trParent);


                GameObject clone2 = Instantiate(bossPrefab,
                    new Vector3(_tr.position.x - 0.5f, _tr.position.y - _tr.position.y * 0.5f, _tr.position.z),
                    _tr.rotation,_trParent);



                clone1.GetComponent <Transform>().localScale = _tr.localScale * 0.5f;
                clone1.GetComponent <BossHealthManager>().enemyHealth = (int) (10.0f * clone1.GetComponent <Transform>().localScale.y);

                clone2.GetComponent <Transform>().localScale = _tr.localScale * 0.5f;
                clone2.GetComponent <BossHealthManager>().enemyHealth = (int) (10.0 * clone2.GetComponent <Transform>().localScale.y);
            } else {
                Messenger.Broadcast("AddPoints", pointsOnDeath);
            }
            gameObject.SetActive(false);
        }
    }

    
   

    public void GiveDamage(int damageToGive) {
        enemyHealth -= damageToGive;
        VoiceManager.me.PlayNoiseSound(acHurt);
        CheckLive();
    }
}
