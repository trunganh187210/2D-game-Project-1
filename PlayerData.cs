using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int stage;

    public PlayerData(Player player)
    {
        //health = player.health;
        stage = player.stage;

        /*position = new float[2];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;*/
    }

}
