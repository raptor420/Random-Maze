using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Maze MazePrefab;
    private Maze MazeInstance;
    public Player playerPrefab;
    private Player playerInstance;
   
    void Awake()
    {
       StartCoroutine( BeginGame());
       // MazeInstance.Generate();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();

        }
    }

    private IEnumerator BeginGame()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f,1f);
        MazeInstance = Instantiate(MazePrefab) as Maze;
        //  StartCoroutine(MazeInstance.Generate());
        yield return StartCoroutine(MazeInstance.Generate());
        playerInstance = Instantiate(playerPrefab) as Player;
        playerInstance.SetLocation(MazeInstance.GetCell(MazeInstance.RandomCoordinates));
        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, .5f, .5f);

    } 

    private void RestartGame()
    {
        StopAllCoroutines();
        Destroy(MazeInstance.gameObject);
        if (playerInstance != null)
        {

            Destroy(playerInstance.gameObject);
        }
        StartCoroutine(BeginGame());


    }
}
