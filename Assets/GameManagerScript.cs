using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    int[,] map;
    GameObject[,] field;
    // Start is called before the first frame update
    void Start()
    {
       

        map = new int[,] {
            {1,0,0,0,0 },
            {0,0,0,0,0 },
            {0,0,0,0,0 }
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
                        new Vector3(x, map.GetLength(0) - y,0),
                        Quaternion.identity
                    );
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            int playerIndex = -1;
            playerIndex = GetPlayerIndex();
            MoveNumber(1, playerIndex, playerIndex + 1);
            PrintArray();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            int playerIndex = -1;
            playerIndex = GetPlayerIndex();
            MoveNumber(1, playerIndex, playerIndex - 1);
            PrintArray();
        }
        */
    }
    
   

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y,x].tag == null)
                {
                    continue;
                }
                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
                
            }
        }
        return new Vector2Int(-1,-1);
    }
    /*
    bool MoveNumber(string number, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }

        if (field[moveTo.y, moveTo.x])
        {
            int velocity = moveTo - moveFrom;
            if (!MoveNumber(2, moveTo, moveTo + velocity)) { return false; }
        }
        map[moveTo] = number;
        map[moveFrom] = 0;
        return true;
    }
    */
}
