using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SubSceneManager : MonoBehaviour
{
    [SerializeField] private NetworkClient networkClient;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SubScene[] subScenes;

    public int SubSceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        SetActiveSubScene(SubSceneIndex);
    }

    private void SetActiveSubScene(int subSceneIndex)
    {
        SubSceneIndex = subSceneIndex;
        for (var i = 0; i < subScenes.Length; ++i)
        {
            SetActiveFast(subScenes[i].gameObject, i == subSceneIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!networkClient.IsConnected || !networkClient.IsAuthenticated)
        {
            gameManager.ClearEntities();
            SetActiveSubScene(0);
            return;
        }

        if (!playerManager.Me)
        {
            SetActiveSubScene(1);
            return;
        }

        SetActiveSubScene(2);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetActiveFast(GameObject obj, bool state)
    {
        if (obj.activeSelf == state)
        {
            return;
        }

        obj.SetActive(state);
    }
}
