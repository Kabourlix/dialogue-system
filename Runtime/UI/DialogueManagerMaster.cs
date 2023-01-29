using System;
using System.Collections;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Aurore.DialogSystem
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class DialogueManagerMaster : MonoBehaviour
    {
        protected DialogueNode current;
        protected bool canBeSkipped = false;

        [SerializeField] protected UnityEvent startDialogueEvent;
        [SerializeField] protected UnityEvent endDialogueEvent;
        private AudioSource _source;
        private event Action<DialogueNode> OnStartEnded;
        public void StartEnded() => OnStartEnded?.Invoke(current);

        protected virtual void Awake()
        {
            OnStartEnded += UpdateDialogue;
            _source = GetComponent<AudioSource>();
            _waitLetter = new WaitForSeconds(waitTimeLetter);
        }

        /// <summary>
        /// This method is called whenever a Component needs to start a sequence.
        /// </summary>
        /// <param name="root">The root node of a DialogGraph</param>
        /// <param name="voice"></param>
        /// <exception cref="NullReferenceException">Thrown if the given node is null</exception>
        public void TriggerDialogue(DialogueNode root, AudioClip voice)
        {
            // ReSharper disable once JoinNullCheckWithUsage
            if (root is null) throw new NullReferenceException("Root element of a graph must not be null !");
            if (root.GetInputPort("input").IsConnected) throw new MonoRootDialogGraphException($"The provided node in {current.graph.name} is not a Root !!!");

            _source.clip = voice;
            startDialogueEvent?.Invoke();
            current = root;
            StartDialogueLogic();
            //UpdateDialogue(root); for testing if the OnStartEvent is not called
        }

        public void TriggerDialogue(DialogueNode root) => TriggerDialogue(root, null);

        #region Start & End Logic

        /// <summary>
        /// This is called at the ed of start Dialogue to add your own logic. Don`t forget to start the first node after your logic.
        /// </summary>
        protected abstract void StartDialogueLogic();

        /// <summary>
        /// This method is called whenever a complete dialogue ends (not one line).
        /// This is seperate to make sure the end event is triggered even with user override.
        /// </summary>
        private void End()
        {
            EndDialogue();
            endDialogueEvent?.Invoke();
        }

        /// <summary>
        /// This is called when the all dialogue is ended and can be modified by inheritance
        /// </summary>
        protected abstract void EndDialogue();

        #endregion

        /// <summary>
        /// This method is in charge of updating the logic when we switch node in a dialog.
        /// This is the core method where the action unfolds.
        /// </summary>
        /// <param name="node">The new node to look at.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the type of the node is unknown.</exception>
        private void UpdateDialogue(DialogueNode node)
        {
            //Update the current Node and deal with the end of a sequence
            current = node;
            if (current is null)
            {
                End();
                return;
            }

            //Displaying content sequence
            switch (node.GetDialogueType())
            {
                case DialogueType.Simple:
                    UpdateDialogueSimple(node);
                    break;
                case DialogueType.Full:
                    UpdateDialogueFull(node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            //Deal with answers
            canBeSkipped = !node.HasAnswers();
            DealWithAnswers(node);
        }

        /// <summary>
        /// This method check if we need to print answer or not. If we need it, it calls the corresponding method.
        /// </summary>
        /// <param name="node">The current node</param>
        protected virtual void DealWithAnswers(DialogueNode node)
        {
            if(canBeSkipped)
            {
                HideAnswersUI(true);
                return;
            }
            HideAnswersUI(false);
            UpdateAnswers(node.answers);
        }

        #region UI Tighly linked abstract method

        /// <summary>
        /// Update the UI to display a "simple" dialogue, i.e. text-only dialogue.
        /// </summary>
        /// <param name="node">The node containing the data.</param>
        protected abstract void UpdateDialogueSimple(DialogueNode node);
        
        /// <summary>
        /// Update the UI to display a "full" dialogue, i.e. text+image+title dialogue.
        /// </summary>
        /// <param name="node">The node containing the data.</param>
        protected abstract void UpdateDialogueFull(DialogueNode node);
        /// <summary>
        /// This method hide or show the answers from the UI depending on the given boolean.
        /// </summary>
        /// <param name="b">true if you want to hide the UI.</param>
        protected abstract void HideAnswersUI(bool b);

        /// <summary>
        /// This method update the amount and the content of answers to be displayed.
        /// </summary>
        /// <param name="answers">a string array of the answers to show</param>
        protected abstract void UpdateAnswers(string[] answers);

        #endregion

        [Range(0.01f,0.1f)]
        [Tooltip("The time in seconds to wait between each letter display in a dialogue.")]
        [SerializeField] private float waitTimeLetter = 0.02f;
        private WaitForSeconds _waitLetter;
        
        /// <summary>
        /// Call this function to animate the letter in when displaying new text.
        /// Pay attention to situation when the user skip an unfinished animation.
        /// </summary>
        /// <param name="newSentence">The new sentence to display</param>
        /// <param name="uiText">The UI Text to write on (TMP_Text only)</param>
        /// <returns></returns>
        protected IEnumerator TypeSentence(string newSentence, TMP_Text uiText)
        {
            uiText.text = ""; //Reset it
            _source.Play();
            foreach (var c in newSentence.ToCharArray())
            {
                uiText.text += c; //Pas ultra opti mais je pense que j'ai pas le choix (pas possible de StringBuilder ici)
                //_source.Play();
                yield return _waitLetter;
            }
            _source.Stop();
        }
        
        #region Interactions
        
        /// <summary>
        /// This method must be called whenever an answer is chosen. Should be linked to your button callback method.
        /// </summary>
        /// <param name="index">The index of the answers has given in the original answers array.</param>
        public void OnAnswerClicked(int index)
        {
            var node = DialogGraph.GetNext(current, index);
            UpdateDialogue(node);
        }

        /// <summary>
        /// This method implement the behavior of the UI when the user hover the dialog box of the dialogue.
        /// </summary>
        /// <param name="b">true if hovered</param>
        public abstract void OnSkipHovered(bool b);
        /*{
            if (!canBeSkipped)
            {
                simpleDialog.transform.GetChild(1).gameObject.SetActive(false);
                return;
            }
            simpleDialog.transform.GetChild(1).gameObject.SetActive(b);
        }*/
        
        /// <summary>
        /// This method is called to skip the current dialogue and get to the next one in the graph.
        /// WARNING : This method does nothing on a multi-choice dialogue line.
        /// </summary>
        public void OnSkipDialog()
        {
            if (!canBeSkipped) return;

            UpdateDialogue(DialogGraph.GetNext(current, 0));
        }

        #endregion
        
    }
}