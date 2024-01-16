using System.Collections.Generic;
using UnityEngine;

namespace CustomQuestSystem
{

    [CreateAssetMenu(fileName = "CustomQuestSO", menuName = "CustomQuestSystem/QuestSO")]
    public class QuestSO : ScriptableObject
    {
        private int _questId;

        [Tooltip("Name of the quest, this will be displayed in the UI at some point")]
        [SerializeField] private string _questName = default;



        [Tooltip("Description of the quest, this will be displayed in the UI at some point")]
        [TextArea]
        [SerializeField] private string _questDescription = default;

        [Tooltip("Whether this is a Mainline quest or Secondary quest")]
        [SerializeField] private QuestType _questType = default;


        [Tooltip("Lists of QuestSteps that make up a Quest")]
        [SerializeField] private List<CustomQuestSystem.QuestStepSO> _steps = default;

        [Tooltip("Set at runtime, Sets the quest to Completed or not")]
        [SerializeField] private QuestStatus _status;


        //List of Quests Prerequisits 
        [SerializeField] private List<QuestSO> _prerequisiteQuests = default;


        public string Name { get => _questName; }
        public string QuestDescription { get => _questDescription; }
        public QuestType Type { get => _questType; }
        public List<CustomQuestSystem.QuestStepSO> Steps { get => _steps; }
        public QuestStatus Status { get => _status; set => _status = value; }
        public int QuestId { get => _questId; }
        public List<QuestSO> PrerequisiteQuests { get => _prerequisiteQuests; }
    }   
}