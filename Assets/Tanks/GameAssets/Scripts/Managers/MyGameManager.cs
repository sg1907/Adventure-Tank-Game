using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Complete
{
    public class MyGameManager : MonoBehaviour
    {
        public bool EndPoint = false;
        public static MyGameManager instance = null;
        [HideInInspector]public bool[] m_Collected = { false, false, false, false, false };
        // 0. index for chest, 1. index for key, 2. index for map, 3. index for flag, 4. index for medal
        public CameraControl m_CameraControl; 
        public Text m_MessageText;
        public Text m_EndMessage;
        public float StartDelay = 3f;            
        public float EndDelay = 3f;

        public GameObject playerPrefab;
        public PlayerManager player;    
                                         
        public GameObject[] m_EnemyTankPrefabs;
        public EnemyManager[] m_EnemyTanks; 
        public List<Transform> wayPointsForAI;
    
        public GameObject[] m_EnemyTurretPrefabs;
        public EnemyTurretManager[] m_EnemyTurrets;
        
        private WaitForSeconds StartWait;      
        private WaitForSeconds EndWait;

        private PlayerManager GameWinner;

        public void Collect(GameObject passedObject)
        {
            
            if (passedObject.tag == "Chest")
                m_Collected[0] = true;
            if (passedObject.tag == "Key")
                m_Collected[1] = true;
            if (passedObject.tag == "Harita")
                m_Collected[2] = true;
            if (passedObject.tag == "Flag")
                m_Collected[3] = true;
            if (passedObject.tag == "Medal")
                m_Collected[4] = true;
            CheckCollectedObject();
            Destroy(passedObject);
        }
        public bool CheckCollectedObject()
        {
            bool b = true;
            string str = "You need to collect";
            if (m_Collected[0] == false)
            {
                str += " the Chest";
                b = false;
            }
            if (m_Collected[1] == false)
            {
                str += ", the Key";
                b = false;
            }
            if (m_Collected[2] == false)
            {
                str += ", the Map";
                b = false;
            }
            if (m_Collected[3] == false)
            {
                str += ", the Flag";
                b = false;
            }
            if (m_Collected[4] == false)
            {
                str += ", the Medal";
                b = false;

               
            }
            
            m_MessageText.text = str;

            return b;

        }
        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);
        }
        void Start()
        {
            
            StartWait = new WaitForSeconds(StartDelay);
            EndWait = new WaitForSeconds(EndDelay);
            
            SpawnPlayer();
            SpawnEnemies();
            //SpawnTurrets();

            StartCoroutine(GameLoop());
        }


        private void SpawnPlayer()
        {

            player.m_Instance = Instantiate(playerPrefab, player.m_PlayerSpawnPoint.position, player.m_PlayerSpawnPoint.rotation) as GameObject;
            player.SetupPlayerTank();
        }

        private void SpawnEnemies()
        {
            for (int i = 0; i < m_EnemyTanks.Length; i++)
            {
                m_EnemyTanks[i].m_Instance =
                    Instantiate(m_EnemyTankPrefabs[i], m_EnemyTanks[i].m_EnemySpawnPoint.position, m_EnemyTanks[i].m_EnemySpawnPoint.rotation) as GameObject;
                m_EnemyTanks[i].m_EnemyNumber = i+2;
                m_EnemyTanks[i].SetupAI(wayPointsForAI);
            }
        }
        
        private void SetCameraTargets()
        {

            Transform[] targets = new Transform[m_EnemyTanks.Length + 1]; 
          
            targets[0] = player.m_Instance.transform;
       
            for (int i = 1; i < targets.Length; i++)
            {
                targets[i] = m_EnemyTanks[i-1].m_Instance.transform;              
            }

            m_CameraControl.m_Targets = targets;
        }
        private IEnumerator GameLoop()
        {
           
            yield return StartCoroutine(GameStarting());
           
            yield return StartCoroutine(GamePlaying());
           
            yield return StartCoroutine(GameEnding());

           
            if (GameWinner != null)
            {
               // m_MessageText.text = "End";
               
                //SceneManager.LoadScene(0);

            }
            else
            {
                StartCoroutine(GameLoop());
                
            }
        }
        private IEnumerator GameStarting()
        {
     
            ResetPlayerTank();
            DisablePlayerTankControl();

           // m_CameraControl.SetStartPositionAndSize();

            m_MessageText.text = "Starting...";
  
            yield return StartWait;
        }
        private IEnumerator GamePlaying()
        {
            EnablePlayerTankControl();

            m_MessageText.text = string.Empty;

            while (true)
            {
                if (!isPlayerAlive())
                {
                    // m_MessageText.text = "You Are Death";
                    break;
                }
                else
                {

                    if (numberOfAliveEnemy() > 0)
                    {
                        Debug.Log("enemy alive");
                        yield return null;
                    }
                    else
                    {
                        if (CheckCollectedObject())
                        {
                            yield return null;
                        }
                           else
                            break;
                    }
                }
            }
            
        }
        private IEnumerator GameEnding()
        {
            GameWinner = player;
            if (!isPlayerAlive())
            {

                m_EndMessage.text = "You Are Death";
                yield return StartWait;
                SceneManager.LoadScene(0);
            }
            else
            {

            }
            yield return null;
        }
        private void EnablePlayerTankControl()
        {

            player.EnablePlayerControl();
        }
        private void DisablePlayerTankControl()
        {

            player.DisablePlayerControl();
        }
        private void ResetPlayerTank()
        {
            player.ResetPlayer();
        }

        private int numberOfAliveEnemy()
        {
            int number = 0;

            for (int j = 0; j < m_EnemyTanks.Length; j++)
            {
                if (m_EnemyTanks[j].m_Instance.activeSelf)
                {
                    number++;
                }            
            }
            Debug.Log("Kalan Enemy Sayısı:"+number);
            return number;
        }
       
        public bool isPlayerAlive()
        {
            if (player.m_Instance.activeSelf)
                return true;
            else
                return false;
        }



























































        private bool OneTankLeft()
        {
            int numEnemyTanksLeft = 0;

            for (int j = 0; j < m_EnemyTanks.Length; j++)
            {
                if (m_EnemyTanks[j].m_Instance.activeSelf)
                {
                    numEnemyTanksLeft++;
                }
            }
            if (numEnemyTanksLeft == 0)
            {
                return true;
            }
            else
                return false;
        }
    }

}