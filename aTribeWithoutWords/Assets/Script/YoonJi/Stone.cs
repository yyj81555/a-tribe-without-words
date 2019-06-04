using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Stone")
        {
            Debug.Log("충돌함");
            CreateManager.stone_hit_count++;
        }
    }


}
