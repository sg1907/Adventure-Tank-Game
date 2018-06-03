using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    public class EndDoor : MonoBehaviour
    {
        public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.


        private AudioSource m_ExplosionTankAudio;               // The audio source to play when the tank explodes.
        private ParticleSystem m_ExplosionTankParticles;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Shell")
            {
                // other.gameObject.SetActive(false);
               
                Debug.Log("Door is Opened");
                m_ExplosionTankParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
                Debug.Log(transform.position);
                Debug.Log(m_ExplosionTankParticles.transform.position);
                m_ExplosionTankParticles.transform.position = other.transform.position;
                Debug.Log(m_ExplosionTankParticles.transform.position);
                // Get a reference to the audio source on the instantiated prefab.
                m_ExplosionTankAudio = m_ExplosionTankParticles.GetComponent<AudioSource>();
                
                // Disable the prefab so it can be activated when it's required.
                m_ExplosionTankParticles.gameObject.SetActive(true);
                m_ExplosionTankParticles.Play();

                // Play the tank explosion sound effect.
                m_ExplosionTankAudio.Play();
                Destroy(gameObject);
            }
        }
    }
}