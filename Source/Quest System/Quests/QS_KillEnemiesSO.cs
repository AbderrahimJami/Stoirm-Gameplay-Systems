using CustomQuestSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomQuestSO", menuName = "CustomQuestSystem/QuestStepSO/QS_KillEnemies")]
public class QS_KillEnemiesSO : QuestStepSO
{

    [Tooltip("Numbers of max enemies to kills")]
    [SerializeField] private int _enemiesToKill = default;

    private int _killCount = 0;


    public override void StartListening()
    {
        Debug.Log("<color=#6cb5ff>Kill Enemy Quest Step </color> commencing now!!");
        GameEventsManager.Instance.MiscEvents.onEnemyKilled += OnEventReceived;
    }

    private void OnEventReceived(GameObject @object)
    {
        Debug.Log("<color=#6cb5ff>Enemie Killed Event received!!</color>");
        _killCount++;

        if (_killCount >= _enemiesToKill)
            FinishQuestStep();


    }

    public override void StopListening()
    {
        Debug.Log("Stop Listening now!");
        GameEventsManager.Instance.MiscEvents.onEnemyKilled -= OnEventReceived;

    }

    public override void ResetProgress()
    {
        base.ResetProgress();
        //Reset progress on the killcount
        _killCount = 0;
    }
}
