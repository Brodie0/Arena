using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerScript : NetworkBehaviour {

    public bool IsDead;
    protected int Hp;
    protected int MaxHp;
    protected Animator Anim;
    protected Rigidbody Rb;
    protected AudioSource AudioSource;
    protected readonly int HitHash = Animator.StringToHash("GetHit");
    protected int attack01Hash = Animator.StringToHash("Attack01");
    protected int attack02Hash = Animator.StringToHash("Attack02");
    protected AudioClip Hit;
    private Text _health;

    protected void Start () {

	    Anim = GetComponent<Animator>();
        AudioSource = GetComponent<AudioSource>();
        if (!isLocalPlayer)
        {
            return;
        }
	    Rb = GetComponent<Rigidbody>();
	    _health = GetComponentInChildren<Text>();
        UpdateUI();
	}

    protected void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag ("LifePotion")) {
            other.gameObject.SetActive (false);
            IncreaseHp (1);
        } 
        else if (other.gameObject.CompareTag ("Weapon")) {
            DecreaseHp (1);
            Anim.SetTrigger (HitHash);
            AudioSource.PlayOneShot (Hit, 1f);
        } 
    }

    void OnParticleCollision(GameObject other){
        if (other.gameObject.CompareTag ("FireSpell")) {
            DecreaseHp (2);
            Anim.SetTrigger (HitHash);
        }
    }

    protected void DecreaseHp(int count){
        if (!isLocalPlayer)
        {
            return;
        }
        if (Hp - count >= 0) {
            Hp -= count;
        }
        else {
            IsDead = true;
        }
        UpdateUI();
    }

    protected void IncreaseHp(int count){
        if (!isLocalPlayer)
        {
            return;
        }
        if (Hp > 0) {
            Hp += count;
        }
        UpdateUI();
    }

    protected void UpdateUI() {
        _health.text = "Health: " + Hp;
    }
}
