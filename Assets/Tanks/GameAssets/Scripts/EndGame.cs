using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Complete
{
    public class EndGame : MonoBehaviour {

        private GameObject m_CanvasGameObject;
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
                if (MyGameManager.instance.CheckCollectedObject()) { 
                    MyGameManager.instance.m_MessageText.text = "You Won";           
                    SceneManager.LoadScene(0);
                }
        }
    }
}