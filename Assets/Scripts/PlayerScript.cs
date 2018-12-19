using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour {

    public bool IsDead;
    protected int Hp;
    protected int MaxHp;
    protected Animator Anim;
    protected Rigidbody Rb;
    protected AudioSource AudioSource;
    protected readonly int HitHash = Animator.StringToHash("GetHit");
    protected AudioClip Hit;
    private SimpleHealthBar _healthBar;

    void Start () {
	    Anim = GetComponent<Animator>();
	    Rb = GetComponent<Rigidbody>();
	    AudioSource = GetComponent<AudioSource>();
	    _healthBar = GetComponentInChildren<SimpleHealthBar>();
	}

    // Update is called once per frame
	void Update () {
		
	}

    protected void OnTriggerEnter(Collider other) {
        if (!isLocalPlayer)
        {
            return;
        }
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
        if (!isLocalPlayer)
        {
            return;
        }
        if (other.gameObject.CompareTag ("FireSpell")) {
            DecreaseHp (2);
            Anim.SetTrigger (HitHash);
        }
    }

    protected void DecreaseHp(int count){
        if (Hp - count >= 0) {
            Hp -= count;
        }
        else {
            IsDead = true;
        }
        UpdateUI();
    }

    protected void IncreaseHp(int count){
        if (Hp > 0) {
            Hp += count;
        }
        UpdateUI();
    }

    protected void UpdateUI() {
        _healthBar.enabled = true;
        _healthBar.UpdateBar(Hp, MaxHp);
    }
}
