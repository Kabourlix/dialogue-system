using System;
using UnityEngine;
using XNode;

namespace Aurore.DialogSystem
{
	[Serializable]
	public struct Connection {}
	
	[CreateNodeMenu("Dialogue Node")]
	public class DialogueNode : Node
	{
		[Input] public Connection input;
		[TextArea] public string content;

		public string title;

		public Sprite img;
		public AudioClip audio;

		[TextArea][Output(dynamicPortList = true)] public string[] answers;

		public override object GetValue(NodePort port)
		{
			return null;
		}

		public DialogueType GetDialogueType()
		{
			return img == null ? DialogueType.Simple : DialogueType.Full;
		}

		public bool HasAnswers()
		{
			return answers.Length != 0 && (answers.Length != 1 || !answers[0].Equals(""));
		}
	}
}