using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace Complete
{
    [Serializable]
    public class PlayerManager
    {

        public Color m_PlayerColor;                             // This is the color this tank will be tinted.
        public Transform m_PlayerSpawnPoint;
       
        [HideInInspector] public string m_ColoredPlayerText;    // A string that represents the player with their number colored to match their tank.
        [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
        [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.
        [HideInInspector] public List<Transform> m_WayPointList;

        private int m_PlayerNumber = 1;
        private TankMovement m_Movement;                        // Reference to tank's movement script, used to disable and enable control.
        private TankShooting m_Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
        private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.
        private StateController m_StateController;	

        // Reference to the StateController for AI tanks
        // Use this for initialization

        public void SetupPlayerTank()
        {
            // Get references to the components.

            m_Movement = m_Instance.GetComponent<TankMovement>();
            m_Shooting = m_Instance.GetComponent<TankShooting>();
            m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;
            m_CanvasGameObject.SetActive(false);
            // Set the player numbers to be consistent across the scripts.
            m_Movement.m_PlayerNumber = 1;
            m_Shooting.m_PlayerNumber = 1;

            // Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

            // Get all of the renderers of the tank.
            MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

            // Go through all the renderers...
            for (int i = 0; i < renderers.Length; i++)
            {
                // ... set their material color to the color specific to this tank.
                renderers[i].material.color = m_PlayerColor;
            }
        }
        public void DisablePlayerControl()
        {
            if (m_Movement != null)
                m_Movement.enabled = false;

           /* if (m_StateController != null)
                m_StateController.enabled = true;*/

            m_Shooting.enabled = false;

            m_CanvasGameObject.SetActive(true);
        }
        public void EnablePlayerControl()
        {
            if (m_Movement != null)
                m_Movement.enabled = true;

            /*if (m_StateController != null)
                m_StateController.enabled = true;*/

            m_Shooting.enabled = true;

            m_CanvasGameObject.SetActive(true);
        }
        public void ResetPlayer()
        {
            m_Instance.transform.position = m_PlayerSpawnPoint.position;
            m_Instance.transform.rotation = m_PlayerSpawnPoint.rotation;

            m_Instance.SetActive(false);
            m_Instance.SetActive(true);
        }
        
    }
}