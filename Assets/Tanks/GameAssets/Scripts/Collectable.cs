using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class Collectable : MonoBehaviour
    {
        MyGameManager mgm;
        public int value;
        public float rotateSpeed;
        void Start()
        {
          
        }

        // Update is called once per frame
        void Update()
        {
            RotateObject();
        }
        void RotateObject()
        {
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
       
        }

        public void OnTriggerEnter(Collider c)
        {
            if (c.gameObject.tag == "Player") {            
                MyGameManager.instance.Collect(gameObject);// 2 for map
                
            }
        }
    }
}