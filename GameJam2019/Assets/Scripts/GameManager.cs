using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Completed
{
	using System.Collections.Generic;
	using UnityEngine.UI;
    public class GameManager : MonoBehaviour
    {
        public float levelStartDelay = 2f;
        public static GameManager instance = null;
        
        private GameObject levelImage;
        private BoardManager boardScript;
        // Use this for initialization
        private int level =1;
        private bool doingSetup = true;
        void Start()
        {
        }
        void Awake() {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            boardScript = GetComponent<BoardManager>();
            InitGame();
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            instance.level++;
            instance.InitGame();
        }
        void InitGame()
        {
            doingSetup = true;
            levelImage = GameObject.Find("LevelImage");
            levelImage.SetActive(true);
            Invoke("HideLevelImage", levelStartDelay);
            boardScript.SetupScene(level);
        }

        void HideLevelImage()
        {
            levelImage.SetActive(false);
            doingSetup=false;
        }

        public void GameOver()
		{			
			//Enable black background image gameObject.
			levelImage.SetActive(true);
			
			//Disable this GameManager.
			enabled = false;
		}

        
        // Update is called once per frame
        void Update()
        {

        }
    }
}