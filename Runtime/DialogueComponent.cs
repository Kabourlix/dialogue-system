using System;
using UnityEngine;

namespace Aurore.DialogSystem
{
    public class DialogueComponent : MonoBehaviour
    {
        [SerializeField] private DialogGraph graph;
        [SerializeField] private AudioClip audioMumblingVoice;
        
        /// <summary>
        /// Getter for the dialogue graph.
        /// </summary>
        public DialogGraph Graph => graph;
        
        /// <summary>
        /// Start the dialog sequence.
        /// </summary>
        public void StartSequence() => DialogueUiManager.Instance.TriggerDialogue(graph.GetRoot(), audioMumblingVoice);

        
        // FOR TESTING PURPOSE
        private void Start()
        {
            Debug.LogWarning($"StartSequence played from the Start function in {gameObject.name}");
            StartSequence();
        }
    }
}
