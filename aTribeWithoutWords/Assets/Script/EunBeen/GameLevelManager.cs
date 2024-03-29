﻿using System.Collections;
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

    // 현재 몇일이 흘렀는지 (DayNightCycle에서 계산)
    public int inGameDays;
    // 낮인지 밤인지 (DayNightCycle에서 계산)
    public bool isSunRise;

    public enum GameLevel
    {
        OldStoneAge, // 구석기
        NewStoneAge, // 신석기
        BronzeAge,   // 청동기
        GameClear    // 게임 클리어
    }

    // 현재 시대
    public GameLevel levelState;
    public bool isLevelClearFlag;

    [SerializeField] GameObject rabbitObj;
    [SerializeField] GameObject boarObj;
    //[SerializeField] GameObject soldierTribeObj;
    //[SerializeField] GameObject shamanTribeObj;

    // 맵상에 존재하는 적 리스트. 맵에 적이 생성되고 제거될때마다 최신화된다.
    public List<GameObject> emyAniListInMap; // 멧돼지
    public List<GameObject> emyTriListInMap; // 적 부족
    public List<GameObject> quryListInMap;   // 사냥감

    void Start () {
        Init();
	}

    void Init()
    {
        inGameDays = 0;
        isSunRise = true;
        levelState = GameLevel.OldStoneAge;
        isLevelClearFlag = false;
    }

    void Update () {

        // 밤에 적이 존재하지 않다면 적 출현
        if (!isSunRise && !IsEnemyAnimalAlive())
            CreateEnemyAnimal();

        if (!isSunRise && !IsEnemyTribeAlive())
            CreateEnemyTribe();

        // 밤에 사냥감이 존재하지 않는다면 사냥감 생성
        if (!isSunRise && !IsQuarryAlive())
            CreateQuarry();

        // 레벨 클리어시 다음 단계로 이동
        if (isLevelClearFlag == true)
        {
            GoNextLevel();
            isLevelClearFlag = false;
        }
    }

    /* 업적시스템에서 현재 시대에 맞는 업적 리스트를 만들고 업적을 달성한 경우 GameLevelManager의 isLevelClearFlag = true로 변경해주어야 한다. */
    /* void IsAchievementClear(){ GameLevelManager.instance.isLevelClearFlag = true }; */

    void GoNextLevel()
    {
        switch (levelState)
        {
            case GameLevel.OldStoneAge:
                levelState = GameLevel.NewStoneAge;
                break;
            case GameLevel.NewStoneAge:
                levelState = GameLevel.BronzeAge;
                break;
            case GameLevel.BronzeAge:
                levelState = GameLevel.GameClear;
                break;
        }
        Debug.Log("현재 시대 :" + levelState);
    }

    // 적 동물 생성
    void CreateEnemyAnimal()
    {
        switch (levelState)
        {
            case GameLevel.OldStoneAge: // 구석기 시대 : 멧돼지
                Instantiate(boarObj);
                Debug.Log("멧돼지 생성");
                break;
            case GameLevel.NewStoneAge: /* 신석기 시대 : 멧돼지 또는 호랑이? 혹시모르니 나누었음. */
                //Instantiate(boarObj);
                break;
            case GameLevel.BronzeAge:
                //Instantiate(boarObj);
                break;
        }
    }

    // 적 부족 생성 /* 적 부족은 부족 타입에 따라 달라지는 값처리가 필요함. 현재 생성 직후 초기화되기 때문에 별도의 설정방법 필요 */
    void CreateEnemyTribe()
    {
        switch (levelState)
        {
            case GameLevel.OldStoneAge:
                // 구석기엔 적 부족은 등장하지 않는다.
                break;
            case GameLevel.NewStoneAge:
                //Instantiate(soldierTribeObj);
                break;
            case GameLevel.BronzeAge:
                //Instantiate(shamanTribeObj);
                break;
        }
    }

    void CreateQuarry()
    {
        switch (levelState)
        {
            case GameLevel.OldStoneAge:
                Instantiate(rabbitObj);
                Instantiate(rabbitObj);
                Debug.Log("토끼 생성");
                break;
            case GameLevel.NewStoneAge:
                Instantiate(rabbitObj);
                Instantiate(rabbitObj);
                break;
            case GameLevel.BronzeAge:
                Instantiate(rabbitObj);
                Instantiate(rabbitObj);
                break;
        }
    }

    // 맵상에 적이 존재하는지 체크
    bool IsEnemyAnimalAlive()
    {
        if (emyAniListInMap.Count > 0)
            return true;
        else
            return false;
    }

    bool IsEnemyTribeAlive()
    {
        if (emyTriListInMap.Count > 0)
            return true;
        else
            return false;
    }

    bool IsQuarryAlive()
    {
        if (quryListInMap.Count > 0)
            return true;
        else
            return false;
    }
}
