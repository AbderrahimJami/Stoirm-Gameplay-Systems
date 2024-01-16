using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

namespace CustomQuestSystem
{
    [CreateAssetMenu(fileName = "CustomQuestStepSO", menuName = "CustomQuestSystem/QuestStepSO")]
    abstract public class QuestStepSO : ScriptableObject
    {
        #region Private Fields


        /// <summary>
        /// Id of the questStep
        /// </summary>
        private int _questStepId = default;

        /// <summary>
        /// Parent Quest Step this Quest Belongs to
        /// </summary>
        private QuestSO _parentQuest = default;

        [Tooltip("Name of the quest step, this will have to be shown on the UI at some point")]
        [SerializeField] private string _questStepName = default;

        [Tooltip("Description of the quest step")]
        [TextArea]
        [SerializeField] private string _questStepDescription = default;


        [Tooltip("Whether the Quest Step is completed or not")]
        [SerializeField] private bool _isCompleted = default;





        #endregion


        #region Properties

        public bool IsCompleted { get => _isCompleted; }
        public QuestSO ParentQuest { get => _parentQuest; set => _parentQuest = value; }
        public string Name { get => _questStepName; }
        public string QuestStepDescription { get => _questStepDescription; }

        [Tooltip("Event to be triggerd on Quest Completion")]
        public UnityAction<QuestStepSO> onQuestStepCompleted = default;



        #endregion


        /// <summary>
        /// This function will start listening to the appropriate events in order to execute its own logic
        /// </summary>
        public abstract void StartListening();
        public abstract void StopListening();


        public virtual void ResetProgress()
        {
            _isCompleted = false;    
        }


        public void FinishQuestStep()
        {
            //Set the Step to be completed  
            _isCompleted = true;
            //Broadcast StepCompletion event to whoemever's listening
            if (onQuestStepCompleted != null)
                onQuestStepCompleted(this);
        }

    }





}
