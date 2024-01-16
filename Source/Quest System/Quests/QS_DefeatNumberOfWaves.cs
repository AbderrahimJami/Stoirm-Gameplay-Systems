using CustomQuestSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QS_DefeatWaves", menuName = "CustomQuestSystem/QuestStepSO/QS_DefeatWaves")]
public class QS_DefeatNumberOfWaves : QuestStepSO
{

    [Tooltip("Number of waves to defeat to complete the quest step")]
    [SerializeField] private int _targetWaves = 1;


    //private non serialized fields
    private int _wavesDefeated = 0;

    public override void StartListening()
    {
        GameEventsManager.Instance.MiscEvents.EnableQuestManager();
        GameEventsManager.Instance.MiscEvents.onWaveCompleted += ProgressQuest;
    }

    public override void StopListening()
    {

        GameEventsManager.Instance.MiscEvents.onWaveCompleted -= ProgressQuest;
    }

    private void EnableEnemies()
    {
        throw new NotImplementedException();
    }

    private void ProgressQuest(int level)
    {
        _wavesDefeated++;

        //Debug statement
        Debug.Log(_wavesDefeated.ToString());

        if (_wavesDefeated >= _targetWaves)
        {
            FinishQuestStep();
            Debug.Log("<color=green>Defeat Waves Quest Step COMPLETED!</color>");
        }
    }

    public override void ResetProgress()
    {
        base.ResetProgress();
        _wavesDefeated = 0;
    }


}
