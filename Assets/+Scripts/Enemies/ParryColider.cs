using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Theogony
{
    public class ParryColider : MonoBehaviour
    {

        Collider parryCollider;
        public GameObject sparkPrefab;
        public ParticleSystem sparkParticles;
        public AudioClip parryClip;

        private void Awake()
        {
            parryCollider = GetComponent<Collider>();
            parryCollider.gameObject.SetActive(true);
            parryCollider.isTrigger = true;
            parryCollider.enabled = false;
            sparkParticles = Instantiate(sparkPrefab, transform).GetComponent<ParticleSystem>();
        }

        public void EnableCollider()
        {
            parryCollider.enabled = true;
        }

        public void DisableCollider()
        {
            parryCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "EnemyWeapon")
            {
                // Debug.Log(transform.position);
                // Debug.Log("Parried");
                PlayerManager manager = GetComponentInParent<PlayerManager>();
                sparkParticles.transform.position = collision.transform.position;
                sparkParticles.Play();
                if(manager.wasHit){
                    manager.Damage(-collision.GetComponentInParent<EnemyController>().weapon.damageDealt);
                    manager.Knockback(collision.transform, -3 * collision.GetComponentInParent<EnemyController>().weapon.knockback);
                }
                collision.GetComponentInParent<EnemyController>().GotParried();
                manager.GetComponent<AudioSource>().PlayOneShot(parryClip);
                collision.enabled = false;
            }
        }
    }
    
}
