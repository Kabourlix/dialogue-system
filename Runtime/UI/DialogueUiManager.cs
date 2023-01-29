using TMPro;
using UnityEngine;

namespace Aurore.DialogSystem
{
    public class DialogueUiManager : DialogueManagerMaster
    {
        #region Singleton declaration & Awake

        // ReSharper disable once InconsistentNaming
        public static DialogueUiManager Instance;
        protected override void Awake() 
        {
            base.Awake(); //needed to instantiate the event.
            if (Instance is not null && Instance != this) Destroy(gameObject);
            Instance = this;
            _IsOpenAnimHash = Animator.StringToHash("IsOpen");
        }

        #endregion
 
        [SerializeField] private GameObject simpleDialog;
        [SerializeField] private GameObject fullDialogue;
        [SerializeField] private GameObject answer;
        [SerializeField] private Animator animator;

        private int _IsOpenAnimHash;

        #region Display

        protected sealed override void StartDialogueLogic()
        {
            animator.SetBool(_IsOpenAnimHash,true);
        }
        protected sealed override void EndDialogue()
        {
            //simpleDialog.SetActive(false);
            //fullDialogue.SetActive(false);
            //answer.SetActive(false);
            animator.SetBool(_IsOpenAnimHash,false);
        }
        

        protected sealed override void UpdateDialogueSimple(DialogueNode node)
        {
            //Display the correct GameObject.
            simpleDialog.SetActive(true);
            fullDialogue.SetActive(false);
            //Text Modification
            var tmp = simpleDialog.transform.GetChild(0).GetComponent<TMP_Text>();
            StopCoroutine(TypeSentence(tmp.text, tmp));
            StartCoroutine(TypeSentence(node.content, tmp));
            //simpleDialog.transform.GetChild(0).GetComponent<TMP_Text>().text = node.content;
        }

        protected sealed override void UpdateDialogueFull(DialogueNode node)
        {
            //Display
            simpleDialog.SetActive(false);
            fullDialogue.SetActive(true);
            //Text, Image modification
            
            //TODO : Start playing audio if needed
        }

        protected override void HideAnswersUI(bool b) => answer.SetActive(!b);

        protected sealed override void UpdateAnswers(string[] answers)
        {
            answer.SetActive(true);
            //Display the correct amount of answers and adjust their text content.
            for (var i = 0; i < answer.transform.childCount; i++)
            {
                var child = answer.transform.GetChild(i);
                if (i < answers.Length)
                {
                    child.gameObject.SetActive(true);
                    child.GetComponentInChildren<TMP_Text>().text = answers[i];
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
                
            }
        }

        #endregion

        #region Interaction
        

        public override void OnSkipHovered(bool b)
        {
            if (!canBeSkipped)
            {
                simpleDialog.transform.GetChild(1).gameObject.SetActive(false);
                return;
            }
            simpleDialog.transform.GetChild(1).gameObject.SetActive(b);
        }

        #endregion
    }
}
