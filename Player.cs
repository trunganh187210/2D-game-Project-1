using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health;

    public int stage;
    public void savePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void loadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        //health = data.health;
        stage = data.stage;

        /*Vector2 position;
        position.x = data.position[0];
        position.y = data.position[1];
        transform.position = position;*/
    }
}
