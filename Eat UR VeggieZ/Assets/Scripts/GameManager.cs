using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 
using UnityEngine.UI;                   //Allows us to use UI.
public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 1f;                      //Black screen Say next level (Boss battle)
    public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    private Text levelText;                                 //Text to display current level number or to say Boss Battle is coming
    private GameObject levelImage;                          //Image to block out level as levels are being set up, background for levelText. (for message)
    private BoardManager boardScript;                       //Store a reference to our BoardManager which will set up the level.
    private int level = 1;                                  //Use to make game harder, after x rounds the player go to next level. (Would change board)
    private int round = 1;                                  // Use to count rounds, for x rounds change board, summon boss etc.
    private bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.
    private int enemiesDead = 0;
    public bool firstScene = true;


    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        //Get a component reference to the attached BoardManager script
        boardScript = GetComponent<BoardManager>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    //this is called only once, and the paramter tell it to be called only after the scene was loaded
    //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static public void CallbackInitialization()
    {
        
        //register the callback to be called everytime the scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //This is called each time a scene is loaded.
    static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (instance.firstScene)
        {
            instance.level = 1;
            instance.firstScene = false;
        }
        else
        {
            instance.level++;
            instance.InitGame();
        }
        
    }


    //Initializes the game for each level.
    void InitGame()
    {
        //While doingSetup is true the player can't move, prevent player from moving while title card is up.
        doingSetup = true;

        //Get a reference to our image LevelImage by finding it by name.
        levelImage = GameObject.Find("LevelImage");

        //Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
        levelText = GameObject.Find("LevelText").GetComponent<Text>();

        //Set the text of levelText to the string "Day" and append the current level number.
        levelText.text = "Level " + level;

        //Set levelImage to active blocking player's view of the game board during setup.
        levelImage.SetActive(true);

        //Call the HideLevelImage function with a delay in seconds of levelStartDelay.
        Invoke("HideLevelImage", levelStartDelay);

        //Call the SetupScene function of the BoardManager script, pass it current level number.
        boardScript.SetupScene(level, round);

    }


    //Hides black image used between levels
    void HideLevelImage()
    {
        //Disable the levelImage gameObject.
        levelImage.SetActive(false);

        //Set doingSetup to false allowing player to move again.
        doingSetup = false;
    }

    //Update is called every frame.
    void Update()
    {
        //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
        if (doingSetup) // Maybe need to stop enemies move here if doingSetup

            //If any of these are true, return and do not start MoveEnemies.
            return;
    }

    //GameOver is called when the player reaches 0 food points
    public void GameOver()
    {
        //Set levelText to display number of levels passed and game over message
        levelText.text = "After " + round + " rounds, you died N00B.";

        //Enable black background image gameObject.
        levelImage.SetActive(true);

        //Disable this GameManager.
        enabled = false;
    }

    public int RoundUp()  // Call Next round on game and give round to Board Manager
    {
        return round++;
    }



}


