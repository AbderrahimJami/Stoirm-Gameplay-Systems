using CustomQuestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "QS_Interact", menuName = "CustomQuestSystem/QuestStepSO/QS_Interact")]
public class QS_InteractSO : QuestStepSO
{

    [Tooltip("List of interactables that player can interact with")]
    //[SerializeField] private string _targetInteractableName;
    [SerializeField] private List<string> _targetInteractableNames;




    public override void ResetProgress()
    {
        base.ResetProgress();
    }

    public override void StartListening()
    {
        Debug.Log("<color=#6ad86a>Interact Step </color> commencing now!!");

        GameEventsManager.Instance.MiscEvents.onInteractionRequested += EvaluateInteraction;
    }

    public override void StopListening()
    {
        GameEventsManager.Instance.MiscEvents.onInteractionRequested -= EvaluateInteraction;
    }


    private void EvaluateInteraction(IInteractable interactable)
    {
        //if (interactable.GetInteractableTransform().transform.gameObject.name == _targetInteractableName)
        //{
        //    //Broadcast to whoever's listening that the interaction witht the tutorial quest Happened 
        //    GameEventsManager.Instance.MiscEvents.InteractionEvaluated(true);
        //    Debug.Log($"Interacted with <b><color=#eeb33dff>{_targetInteractableName}</color></b> now!");
        //    FinishQuestStep();
        //}
        //else
        //{
        //    //Broadcast to whoever's listening that the interaction wasn't approved.
        //    GameEventsManager.Instance.MiscEvents.InteractionEvaluated(false);
        //}

        //Check if interactablename is in the list 
        string name = _targetInteractableNames.Find(o => o == interactable.GetInteractableTransform().transform.gameObject.name);
        if (name != null)
        {
            //Broadcast to whoever's listening that the interaction witht the tutorial quest Happened 
            GameEventsManager.Instance.MiscEvents.InteractionEvaluated(true);
            Debug.Log($"Interacted with <b><color=#eeb33dff>{name}</color></b> now!");
            FinishQuestStep();

        }
        else
        {
            //Broadcast to whoever's listening that the interaction wasn't approved.
            GameEventsManager.Instance.MiscEvents.InteractionEvaluated(false);

        }
    }


}
