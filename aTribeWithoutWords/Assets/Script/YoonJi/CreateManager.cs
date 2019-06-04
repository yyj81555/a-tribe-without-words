using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateManager : MonoBehaviour {
    public static int stone_hit_count = 0;
    public GameObject Weapon;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(stone_hit_count >= 6)
        {
            //CaveStorage.StoreItem(Weapon, CaveStorage.ItemType.WEAPON);
            Debug.Log("재작완료.");
            stone_hit_count = 0;
        }
	}
}
