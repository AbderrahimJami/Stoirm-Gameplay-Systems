using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CustomQuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        [Tooltip("List of Quests in the game")]
        [SerializeField] private List<CustomQuestSystem.QuestSO> _quests = default;

        [SerializeField] private Dictionary<string, CustomQuestSystem.QuestSO> _secondaryQuests;

        [SerializeField] private Text _currentQuestStepUIDescriptor; 



        private QuestSO _currentActiveMainlineQuest = default;
        private QuestStepSO _currentActiveMainlineQuestStep = default;


        // Start is called before the first frame update
        void Start()
        {
            StartMainlineQuest();


            //Broadcast the inital state of all quests at startup

            foreach (CustomQuestSystem.QuestSO quest in _secondaryQuests.Values)
            {
                GameEventsManager.Instance.QuestEvents.QuestStateChange(quest);
            }


        }

        private void Awake()
        {
            //Initializes the questStep Dictionary by calling the CreateQuestMap function
            _secondaryQuests = CreateQuestMap();
        }

        /// <summary>
        /// Returns a dictionary of quests by loading them from the assets folder inside the Quests folder
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, QuestSO> CreateQuestMap()
        {

            QuestSO[] allQuests = Resources.LoadAll<QuestSO>("Secondary Quests");
            //Create the questStep map
            Dictionary<string, QuestSO> idToQuestMap = new Dictionary<string, QuestSO>();

            foreach (QuestSO quest in allQuests)
            {
                if (idToQuestMap.ContainsKey(quest.Name))
                    Debug.LogWarning("Duplicate ID found when creating quest map " + quest.Name);

                //Set the parent quest for each quest step
                foreach (var questStep in quest.Steps)
                    questStep.ParentQuest = quest;

                //Add the quest to the new map
                idToQuestMap.Add(quest.Name, quest);
            }

            return idToQuestMap;
        }


        private void OnEnable()
        {
            GameEventsManager.Instance.QuestEvents.onStartQuestSO += StartQuest;
            GameEventsManager.Instance.QuestEvents.onAdvanceQuestSO += ProgressQuest;
            GameEventsManager.Instance.QuestEvents.onFinishQuestSO += FinishQuest;
        }

        private void OnDisable()
        {
            GameEventsManager.Instance.QuestEvents.onStartQuestSO -= StartQuest;
            GameEventsManager.Instance.QuestEvents.onAdvanceQuestSO -= ProgressQuest;
            GameEventsManager.Instance.QuestEvents.onFinishQuestSO -= FinishQuest;
        }

        private void StartMainlineQuest()
        {
            //Check if MainlineQuests is not null
            if (_quests != null)
            {
                //If so check if there's a questStep in questline that's not completed 
                _currentActiveMainlineQuest = _quests.Find(o => o.Status == QuestStatus.NOT_STARTED);

                if (_currentActiveMainlineQuest != null)
                {
                    //Call the ChangeQuestStatus and pass IN_PROGRESS as the new questStatus 
                    ChangeQuestStatus(_currentActiveMainlineQuest, QuestStatus.IN_PROGRESS);
                    //Call ProgressQuest to start questStep
                    ProgressQuest(_currentActiveMainlineQuest);
                }
                else
                {
                    //Finish the game
                }
            }

        }

        private void StartQuest(QuestSO quest)
        {
            if (quest.Type == QuestType.Secondary)
            {
                //Check if questStep is inside the secondary quests dictionary
                if (GetQuestByName(quest.Name))
                {
                    ////Check if we can actually start this quest
                    //if (!CheckRequirementsMet(quest))
                    //    return;


                    //If so, change the state of the questStep to be in progress
                    ChangeQuestStatus(quest, QuestStatus.IN_PROGRESS);
                    //Call progress questStep method passing in the questStep
                    ProgressQuest(quest);
                }

            }
            else
                Debug.LogError($"Quest {quest.Name} cannot be started cause neither a Mainline quest nor is it a Secondary quest");
        }

        /// <summary>
        /// Returns a questStep that has the provided questName, proveded there's one
        /// </summary>
        /// <param name="questName">questName of the questStep to search</param>
        /// <returns></returns>
        private bool GetQuestByName(string questName)
        {
            QuestSO quest = _secondaryQuests[questName];
            if (quest == null)
            {
                Debug.LogError("ID not found in the quest map: " + questName);
                return false;
            }

            return true;
        }





        private void ProgressQuest(QuestSO quest)
        {
            //Check if the current active questStep is not null
            if (quest != null)
            {
                //Check if there's steps in the quest
                if (quest.Steps != null)
                {
                    //If so, check if there's a step that's not completed in the questStep
                    QuestStepSO nextQuestStep = quest.Steps.Find(step => !step.IsCompleted);
                    if (nextQuestStep != null)
                    {
                        ChangeQuestStepUI(nextQuestStep);

                        if (quest.Type == QuestType.Mainline)
                            //If so, set the current active questStep step to be said queststep
                            _currentActiveMainlineQuestStep = nextQuestStep;

                        nextQuestStep.StartListening();
                        //Start listening to the queststep onCompleted event
                        nextQuestStep.onQuestStepCompleted += OnQuestStepCompleted;
                    }
                    else
                    {

                        if (quest.Type == QuestType.Mainline)
                        {
                            //Set questStep to finished HERE! (THIS IS CALLED TWICE!!!, LOOK INSIDE FINISHQUEST function)
                            ChangeQuestStatus(quest, QuestStatus.FINISHED);
                            //Start next mainline
                            StartMainlineQuest();
                            //Call the finish quest method passing in the quest that's finished
                            FinishQuest(quest);
                        }
                        else if (quest.Type == QuestType.Secondary)
                        {
                            //Set questStep to finished HERE!
                            ChangeQuestStatus(quest, QuestStatus.CAN_FINISH);
                        }


                    }
                }
                else
                    Debug.LogError($"The quest named {quest.Name} doesn't have any quest steps in it!");
            }
        }

        /// <summary>
        /// Sets the UI to show new QuestStep description
        /// </summary>
        /// <param name="nextQuestStep">Quest step to use description of</param>
        private void ChangeQuestStepUI(QuestStepSO nextQuestStep)
        {
            //Set UI for the currentQuestStepUIDescriptor
            _currentQuestStepUIDescriptor.text = nextQuestStep.QuestStepDescription;
        }

        private void FinishQuest(QuestSO quest)
        {
            //TODO
            Debug.Log("Hurray You completed a quest!!");
            //Call the change quest status function 
            ChangeQuestStatus(quest, QuestStatus.FINISHED);
            
        }

        private void ChangeQuestStatus(QuestSO quest, QuestStatus newStatus)
        {

            //Check if the questStep exists
            //I like using guard clauses, makes code cleaner
            if (quest == null)
                return;

            //If so, change the status to be newStatus
            quest.Status = newStatus;
            //Trigger GameEvent to notify change of QuestStatus to whomever's listening
            GameEventsManager.Instance.QuestEvents.QuestStateChange(quest);




        }


        private void OnQuestStepCompleted(QuestStepSO questStep)
        {
            //For now display Debug message on Console !!
            Debug.Log($"Event received from QuestStep <color=#6cb5ff>{questStep.Name}</color>! Quest step has been completed");

            //Check if questStep exists
            if (questStep.ParentQuest == null)
            {
                if (_currentActiveMainlineQuest != null)
                {
                    //Check if there's a step currently being tracked 
                    if (_currentActiveMainlineQuestStep != null)
                    {
                        //If so, make the questStep step stop listening to events
                        _currentActiveMainlineQuestStep.StopListening();
                        //Stop listening to the queststep onCompleted event
                        _currentActiveMainlineQuestStep.onQuestStepCompleted -= OnQuestStepCompleted;
                        //Progress questStep to next step (Might want to include logit )
                        ProgressQuest(_currentActiveMainlineQuest);
                    }
                }
            }
            else if (questStep.ParentQuest.Type == QuestType.Secondary)
            {
                if (questStep.ParentQuest != null)
                {
                    //Make the questStep step stop listening to events
                    questStep.StopListening();
                    //Stop listening to the queststep onCompleted event
                    questStep.onQuestStepCompleted -= OnQuestStepCompleted;
                    //Progress questStep to next step (Might want to include logit )
                    ProgressQuest(questStep.ParentQuest);
                }
                else
                    Debug.LogError($"Quest Step {questStep.Name} doesn't have a parent");
            }

        }



        private void OnApplicationQuit()
        {
            //Reset progress on Mainline Quests
            ResetQuestsProgress(_quests);
            //Reset progress on Secondary Quests
            ResetQuestsProgress(_secondaryQuests.Values);
        }

        private void ResetQuestsProgress<T>(T container) where T: ICollection<QuestSO>
        {
            if (container == null)
                return;

            foreach (var quest in container)
            {
                foreach (var questStep in quest.Steps)
                    questStep.ResetProgress();

                quest.Status = QuestStatus.NOT_STARTED;
            }
        }
    }




}