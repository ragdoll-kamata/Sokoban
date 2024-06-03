using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab; 
    public GameObject wallPrefab;
    public GameObject clearText;
    public GameObject particlePrefab;

    int[,] map;
    GameObject[,] field;
    // Start is called before the first frame update
    void Start()
    {
        clearText.SetActive(false);
        Screen.SetResolution(1280, 720, false);
        map = new int[,] {
            {1,0,0,0,0 },
            {0,2,4,3,0 },
            {0,2,0,0,3 },
            {0,2,4,3,0 },
            {0,0,4,0,0 },
            {0,0,4,0,0 }
        };
        field = new GameObject
        [
        map.GetLength(0),
        map.GetLength(1)
        ];

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y,x] = Instantiate(
                        playerPrefab,
                        new Vector3(x - map.GetLength(1) / 2.0f + 0.5f, map.GetLength(0) - y,0),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x - map.GetLength(1) / 2.0f + 0.5f, map.GetLength(0) - y, 0),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 3)
                {
                    Instantiate(
                        goalPrefab,
                        new Vector3(x - map.GetLength(1) / 2.0f + 0.5f, map.GetLength(0) - y, 0.01f),
                        Quaternion.identity
                    );
                }
                if (map[y, x] == 4)
                {
                    field[y, x] = Instantiate(
                        wallPrefab,
                        new Vector3(x - map.GetLength(1) / 2.0f + 0.5f, map.GetLength(0) - y, 0.0f),
                        Quaternion.identity
                    );
                }
              
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
           
            MoveNumber(playerIndex, playerIndex + new Vector2Int(1, 0));
            if (IsCleard())
            {
                clearText.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex - new Vector2Int(1, 0));
            if (IsCleard())
            {
                clearText.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex - new Vector2Int(0, 1));
            if (IsCleard())
            {
                clearText.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber(playerIndex, playerIndex + new Vector2Int(0, 1));
            if (IsCleard())
            {
                clearText.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            clearText.SetActive(false);
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Destroy(field[y, x]);
                    field[y, x] = null;
                    if (map[y, x] == 1)
                    {
                        field[y, x] = Instantiate(
                            playerPrefab,
                            new Vector3(x - map.GetLength(1) / 2.0f + 0.5f, map.GetLength(0) - y, 0),
                            Quaternion.identity
                        );
                    }
                    if (map[y, x] == 2)
                    {
                        field[y, x] = Instantiate(
                            boxPrefab,
                            new Vector3(x - map.GetLength(1) / 2.0f + 0.5f, map.GetLength(0) - y, 0),
                            Quaternion.identity
                        );
                    }
                    if (map[y, x] == 4)
                    {
                        field[y, x] = Instantiate(
                            wallPrefab,
                            new Vector3(x - map.GetLength(1) / 2.0f + 0.5f, map.GetLength(0) - y, 0.0f),
                            Quaternion.identity
                        );
                    }

                }
            }
        }
    }
    
   

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y,x] == null) { continue; }
                
                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
                
            }
        }
        return new Vector2Int(-1,-1);
    }
    
    bool MoveNumber(Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Wall") {  return false; };

        if(field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success= MoveNumber(moveTo, moveTo + velocity);
            if (!success) { return false; }
        }

        Vector3 moveToPosition = new Vector3(
            moveTo.x - map.GetLength(1) / 2.0f + 0.5f, 
            map.GetLength(0) - moveTo.y,
            0.0f
            );
        field[moveFrom.y, moveFrom.x].GetComponent<Move>().MoveTo(moveToPosition);
        
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        for(int i = 0; i < 5; i++)
        {
            Instantiate(
             particlePrefab,
             new Vector3(moveFrom.x - map.GetLength(1) / 2.0f + 0.5f, map.GetLength(0) - moveFrom.y, 0.0f),
             Quaternion.identity
            );
        }


        return true;
    }
    
    bool IsCleard()
    {
        List<Vector2Int> goals = new List<Vector2Int>();
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for(int x=0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for(int i=0;i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if(f == null || f.tag != "Box")
            {
                return false;
            }
        }

        return true;
    }
}
