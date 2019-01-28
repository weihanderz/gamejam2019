using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public static GameManager instance = null;
    private Text levelText;
    private GameObject levelImage;
    private BoardManager boardScript;
    // Use this for initialization
    private int level = 1;
    private bool doingSetup = true;

    private Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Awake()
    {
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
        Debug.Log("LEVEL++");
        instance.level++;
        instance.InitGame();
    }
    void InitGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Stage " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        boardScript.SetupScene(level);
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "YOU ARE DEAD";
        levelImage.SetActive(true);
        //Disable this GameManager.
        enabled = false;
        Invoke("Reset", 5f);
    }

    public void Reset()
    {
        //Enable this GameManager.
        enabled = true;
        this.player.health = 50;
        this.level = 1;
        this.player.gameObject.SetActive(true);
        this.InitGame();
    }


    // Update is called once per frame
    void Update()
    {

    }
}