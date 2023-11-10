using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    public Character player;
    public Character enemy;

    public Character[] characterList = new Character[6];
    void Start()
    {
        player.transform.position = new Vector2(-4.5f, 2.5f);
        int rand = Random.Range(0, 6);
        enemy = characterList[rand];
        enemy.transform.position = new Vector2(5.5f, 1.2f);
        if (rand < 4)
        {
            enemy.transform.localScale *= new Vector2(-1, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
