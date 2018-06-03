using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
namespace Complete
{
    [Serializable]
    public class EnemyTurretManager
    {
        public Color m_EnemyTurretColor;
        public Transform m_EnemyTurretSpawnPoint;

        [HideInInspector] public int m_EnemyTurretNumber;
        [HideInInspector] public GameObject m_Instance;
        private TankShooting m_Shooting;
        private StateController m_StateController;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void SetupAITurret()
        {
            m_StateController = m_Instance.GetComponent<StateController>();
            m_StateController.SetupAITurret(true);

            m_Shooting = m_Instance.GetComponent<TankShooting>();
            m_Shooting.m_PlayerNumber = m_EnemyTurretNumber;

            MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();
            // Go through all the renderers...
            for (int i = 0; i < renderers.Length; i++)
            {
                // ... set their material color to the color specific to this tank.
                renderers[i].material.color = m_EnemyTurretColor;
            }
        }
    }
}