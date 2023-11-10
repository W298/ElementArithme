using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public Character player;
    public GameObject enemyGameObj;
    
    public Character[] characterList = new Character[6];

    public Character newPlayer;
    void Start()
    {
        int rand = Random.Range(0, 6);

        newPlayer = Instantiate(player.gameObject,  new Vector2(-4.5f, 2.5f), Quaternion.identity).GetComponent<Character>();
        enemyGameObj = Instantiate(characterList[rand].gameObject, new Vector2(5.5f, 1.2f), Quaternion.identity);
        if (rand < 4)
        {
            enemyGameObj.transform.localScale *= new Vector2(-1, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
