using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager singleton;
	
	private GroundPiece[] allGroundPiece;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void SetUpNewLevel()
	{
		allGroundPiece = FindObjectsOfType<GroundPiece>();
	}
	
	//Transition from one level to another on completion
	
	private void Awake()
	{
		if(singleton == null)
		 	singleton = this;
	      else if(singleton != this)
			Destroy(gameObject);
		
			DontDestroyOnLoad(gameObject);
	}
	
	//Call OnLevelFinishedLoading each time a new scene is loaded
	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}
	
	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		SetUpNewLevel();
	}  
	
	//Check to see if game is complete or not
	public void CheckComplete()
	{
		bool isFinished = true;
		
		for(int i = 0; i < allGroundPiece.Length; i++)
		{
			if(allGroundPiece[i].isColored == false)
			{
				isFinished = false;
				break;
			}
		}
		
		if(isFinished)
			NextLevel();
	}
	
	 private void NextLevel()
	{ 
	  if(SceneManager.GetActiveScene().buildIndex == 3) 
	    {
			SceneManager.LoadScene(0);
		}
	  else 
	  {
		  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	  }
	}

}
