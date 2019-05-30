using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : MonoBehaviour {

    // 싱글턴 변수
    private static GameLevelManager _instance;

    public static GameLevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameLevelManager>() as GameLevelManager;

                if (_instance == null)
                {
                    Debug.Log("GameLevelManager 스크립트가 부착된 오브젝트가 없습니다.");
                    return null;
                }
            }

            return _instance;
        }
    }

    // 현재 몇일이 흘렀는지(DayNightCycle에서 계산)
    public int inGameDays = 0;

    void Start () {
		
	}
	
	void Update () {
		
	}
}
