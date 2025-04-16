using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Light : MonoBehaviour
{
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, Player.transform.position.z - 1);
    }
}
