using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
namespace Complete
{
    [Serializable]
    public class EnemyManager
    {     
        public Color m_EnemyColor;
        public Transform m_EnemySpawnPoint;
        [HideInInspector] public int m_EnemyNumber;
        [HideInInspector] public List<Transform> m_WayPointList;
        [HideInInspector] public GameObject m_Instance;
      //  [HideInInspector] public string m_ColoredEnemyText;
     
        private TankShooting m_Shooting;
        private StateController m_StateController;
        private GameObject m_CanvasGameObject;
      
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetupAI(List<Transform> wayPointList)
        {
            m_StateController = m_Instance.GetComponent<StateController>();
            m_StateController.SetupAI(true, wayPointList);

            m_Shooting = m_Instance.GetComponent<TankShooting>();
            m_Shooting.m_PlayerNumber = m_EnemyNumber;

            m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
            //m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

            // Get all of the renderers of the tank.
            MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

            // Go through all the renderers...
            for (int i = 0; i < renderers.Length; i++)
            {
                // ... set their material color to the color specific to this tank.
                renderers[i].material.color = m_EnemyColor;
            }
        }
    }
}