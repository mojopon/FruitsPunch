﻿using UnityEngine;
using System.Collections;
using UniRx;
using FruitsPunchInGameScripts;

public class Loader : MonoBehaviour
{
    public InputManager inputManagerPrefab;
    public ScoreManager scoreManagerPrefab;

    // in game manager prefabs(these will going to be instantiated when the game started) 
    public FruitsPunchManager fruitsManagerPrefab;
    public GameTimerManager gameTimerManagerPrefab;
    public CountdownManager countdownManagerPrefab;

    public GameObject[] effectManagersPrefab;

    private Transform managers;
    private Transform inGameManagers;
    private Transform effectManagers;
    void Start()
    {
        managers = new GameObject("Managers").transform;
        Instantiate(inputManagerPrefab).transform.SetParent(managers);
        Instantiate(scoreManagerPrefab).transform.SetParent(managers);


        GameState.Instance.GameStateReactiveProperty
                          .Where(x => x == GameStateEnum.BeforeGameStart)
                          .Subscribe(x => 
                          {
                              InstantiateInGameManagers();
                          })
                          .AddTo(gameObject);

        GameState.Instance.GameStateReactiveProperty
                  .Where(x => x == GameStateEnum.DiscardFruitsPunch)
                  .Subscribe(x => DestroyManagers())
                  .AddTo(gameObject);
    }

    void InstantiateInGameManagers()
    {
        if(inGameManagers == null)
            inGameManagers = new GameObject("InGameManagers").transform;

        Instantiate(fruitsManagerPrefab).transform.SetParent(inGameManagers);
        Instantiate(gameTimerManagerPrefab).transform.SetParent(inGameManagers);
        Instantiate(countdownManagerPrefab).transform.SetParent(inGameManagers);

        InsitantiateEffectManagers();
    }

    void InsitantiateEffectManagers()
    {
        if (effectManagers == null)
            effectManagers = new GameObject("EffectManagers").transform;

        effectManagers.SetParent(inGameManagers);

        foreach (var obj in effectManagersPrefab)
            Instantiate(obj).transform.SetParent(effectManagers);
        
    }

    void DestroyManagers()
    {
        if (inGameManagers != null)
        {
            Destroy(inGameManagers.gameObject);
            inGameManagers = null;
        }
    }
}
