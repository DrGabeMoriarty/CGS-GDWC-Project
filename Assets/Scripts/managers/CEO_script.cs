using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class CeoScript : MonoBehaviour 
{
    public static CeoScript Instance;
    public static GameObject[] Powerups;
    public static int[] PowerupSpawned;
    public static GameObject ActivePowerUp;
    public AudioMixer mixer;
    public static int[] EnemiesKilled = new int[3];
    public static int TotalKillScore=0;
    public static int DangerLevel=0, LastDangerLevel=0, AnxietyLevel=0;
    public static int PostProcessing;
    public static bool OgDifficulty=false;
    public enum GameState
    {
        Menu,
        PreForestLevel,ForestLevel, ForestLevelCleared,WithDaPidgon,
        PreHotelLevel,HotelLevel, HotelLevelCleared,
        BossBattle,BossBattle2, BossBattleCleared, GameOver
    }
    public static GameState CurrentGameState;
    public static int LastLevel=0;
    public static int Money = 0,StartMoney=0;
    public static int Health = 100, HealthLimit = 320, HealthCost=40;
    public static float Speed = 1.0f, SpeedLimit = 1.6f;
    public static int SpeedCost=40;
    public static int Ammocapacity = 300, AmmoMax = 900, AmmoCost=40;
    public static int Staminacap = 600, StaminaMax = 1200, StaminaCost=40;
    public static int FirstLoad,FirstTimeInSession=1;
    public static int Highscore;
    public static float MusicLevel, SFxLevel;
    public static string NextLevel="Main_menu";
    private void Awake()
    { 
        if (Instance != null)
        { 
            Destroy(gameObject); 
        }
        else
        { 
            Instance = this; 
            DontDestroyOnLoad(gameObject); 
        } 
    }

    private void Start() 
    { 
        PowerupSpawned = new int[4];
        Money = PlayerPrefs.GetInt("money", 0);
        FirstLoad = PlayerPrefs.GetInt("firstLoad",1);
        Highscore = PlayerPrefs.GetInt("Highscore",0);
        MusicLevel = PlayerPrefs.GetFloat("musicLevel", 0.75f);
        SFxLevel = PlayerPrefs.GetFloat("sfxLevel", 0.75f);
        PostProcessing = PlayerPrefs.GetInt("postProcessing",1);
        PlayerPrefs.GetInt("OGD",0);

        if(PlayerPrefs.GetInt("OGD")==0)    
            OgDifficulty=false;
        else if(PlayerPrefs.GetInt("OGD")==1)
            OgDifficulty=true;
        
        LastDangerLevel=0;
        if(FirstLoad==1)
            Highscore=0;
        mixer.SetFloat("musicLevel",Mathf.Log10(MusicLevel)*20);
        mixer.SetFloat("SFxLevel",Mathf.Log10(SFxLevel)*20);
    }

    

    public static void Transition(int newhealth)
    {
        Health = newhealth;
    }

    public static void GameOver()
    {
        PlayerPrefs.SetInt("money",Money);

        if(TotalKillScore+((Money-StartMoney>0)? Money-StartMoney:0)>Highscore)
            Highscore=TotalKillScore+((Money-StartMoney>0)? Money-StartMoney:0);
        PlayerPrefs.SetInt("Highscore",Highscore);

        PowerupSpawned = new int[4];

        UnityEngine.SceneManagement.SceneManager.LoadScene("game_over");
        //other things.
    }
    
    public static void LoadLevel(string levelName)
    {
        NextLevel = levelName;
        UnityEngine.SceneManagement.SceneManager.LoadScene("loading_scene");
    }

    public static void QuitGame()
    {
        PlayerPrefs.SetInt("Highscore",Highscore);
        PlayerPrefs.SetInt("money",Money);
        PlayerPrefs.SetFloat("musicLevel", MusicLevel);
        PlayerPrefs.SetFloat("sfxLevel", SFxLevel);
        PlayerPrefs.SetInt("postProcessing",PostProcessing);
    }
}