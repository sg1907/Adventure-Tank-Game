using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using System.IO;
namespace Complete
{
    public class SurvivalManager : MonoBehaviour
    {
        public bool a = true;
        public static bool ManagerActive = false;
        public static SurvivalManager instance = null;
        [HideInInspector] public bool[] m_Collected = { false, false, false, false, false };
        // 0. index for chest, 1. index for key, 2. index for map, 3. index for flag, 4. index for medal
        public CameraControl m_CameraControl;
        public Text m_MessageText;
        public Text m_EndMessage;
        public Text m_TimeMessage;
        public Text m_ScoreMessage;
        public float StartDelay = 3f;
        public float EndDelay = 3f;
        private string score;
        public GameObject playerPrefab;
        public PlayerManager player;

        public GameObject[] m_EnemyTankPrefabs;
        public EnemyManager[] m_EnemyTank;
        public List<Transform> wayPointsForAI;

        [HideInInspector] public int m_numberOfAliveEnemy = 0;
        [HideInInspector] public int m_numberOfSpawnedEnemy = 0;
        [HideInInspector] public int m_numberOfDeathEnemy;
        private WaitForSeconds StartWait;
        private WaitForSeconds EndWait;

        private PlayerManager GameWinner;

        public float spawnWait;
        public float spawnMostWait;
        public float spawnLeastWait;
        public float timeLeft;
        private LinkedList<EnemyManager> m_EnemyList;


        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != null)
                Destroy(gameObject);
        }
        void Start()
        {
            ManagerActive = true;
            StartWait = new WaitForSeconds(StartDelay);
            EndWait = new WaitForSeconds(EndDelay);

            SpawnPlayer();
            // SpawnEnemy();
            //SpawnTurrets();

            StartCoroutine(GameLoop());
        }
        void Update()
        {
            
            timeLeft = timeLeft - Time.deltaTime;
            m_TimeMessage.text = "Remaining Time : " + System.Convert.ToInt32(timeLeft);
            //Debug.Log(System.Convert.ToInt32(timeLeft));
            if (timeLeft < 0)
            {
                m_EndMessage.text = "Time Up... Going Back To Menu...";
                Thread.Sleep(3000);
                SceneManager.LoadScene(0);
            }
        }

        private void SpawnPlayer()
        {

            player.m_Instance = Instantiate(playerPrefab, player.m_PlayerSpawnPoint.position, player.m_PlayerSpawnPoint.rotation) as GameObject;
            player.SetupPlayerTank();
        }

        private void SpawnEnemy()
        {
            if (a == true) { 
                m_EnemyTank[0].m_Instance = Instantiate(m_EnemyTankPrefabs[0], m_EnemyTank[0].m_EnemySpawnPoint.position, m_EnemyTank[0].m_EnemySpawnPoint.rotation) as GameObject;
                m_EnemyTank[0].m_EnemyNumber = m_numberOfSpawnedEnemy + 2;
                m_EnemyTank[0].SetupAI(wayPointsForAI);
                a = false;
            }
            else
            {
                m_EnemyTank[1].m_Instance = Instantiate(m_EnemyTankPrefabs[1], m_EnemyTank[1].m_EnemySpawnPoint.position, m_EnemyTank[1].m_EnemySpawnPoint.rotation) as GameObject;
                m_EnemyTank[1].m_EnemyNumber = m_numberOfSpawnedEnemy + 2;
                m_EnemyTank[1].SetupAI(wayPointsForAI);
                a = true;
            }

            m_numberOfSpawnedEnemy++;
            Debug.Log("Number Of Spawned Enemy  " + m_numberOfSpawnedEnemy);
            m_numberOfAliveEnemy++;
            Debug.Log("Number Of Alive Enemy  " + m_numberOfAliveEnemy);
            m_numberOfDeathEnemy = m_numberOfSpawnedEnemy - m_numberOfAliveEnemy;
            showScore();
            
        }
        public void showScore()
        {
            m_ScoreMessage.text = "Score " + m_numberOfDeathEnemy * 10;
            score = (m_numberOfDeathEnemy * 10).ToString();
            using (StreamWriter sw = new StreamWriter("HighScore.txt"))
            {              
                    sw.WriteLine(score);               
            }
        }
        private IEnumerator wait(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
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
                    //SceneManager.LoadScene(0);
                    Debug.Log("Player is Death");
                    break;
                }
                else
                {
                    
                    Debug.Log("else");
                    yield return new WaitForSeconds(spawnWait);
                    SpawnEnemy();
                    spawnWait = Random.Range(spawnLeastWait, spawnMostWait);
                    yield return null;

              
                }
               
            }

        }
        private IEnumerator GameEnding()
        {
            player.DisablePlayerControl();
            GameWinner = player;
            m_EndMessage.text = "You Are Death";
            yield return StartWait;
            SceneManager.LoadScene(0);

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
            return m_numberOfAliveEnemy; ;
        }

        public bool isPlayerAlive()
        {
            if (player.m_Instance.activeSelf)
                return true;
            else
                return false;
        }



























































        //private bool OneTankLeft()
        //{
        //    int numEnemyTanksLeft = 0;

        //    for (int j = 0; j < m_EnemyTanks.Length; j++)
        //    {
        //        if (m_EnemyTanks[j].m_Instance.activeSelf)
        //        {
        //            numEnemyTanksLeft++;
        //        }
        //    }
        //    if (numEnemyTanksLeft == 0)
        //    {
        //        return true;
        //    }
        //    else
        //        return false;
        //}
    }

}