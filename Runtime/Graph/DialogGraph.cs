using System;
using System.Linq;
using UnityEngine;
using XNode;

namespace Aurore.DialogSystem
{
	[CreateAssetMenu(fileName = "DGraph", menuName = "Aurore/DialogueGraph", order = 0)]
	public class DialogGraph : NodeGraph
	{

		private DialogueNode _root;
		
		/// <summary>
		/// Access the root of the current graph.
		/// </summary>
		/// <returns>The root node of the graph.</returns>
		/// <exception cref="NullReferenceException">Thrown if no root are found.</exception>
		/// <exception cref="MonoRootDialogGraphException">Thrown if there isn't a unique root in the graph.</exception>>
		public DialogueNode GetRoot()
		{
			_root = null;
			//Root node has no input
			foreach (var node in nodes.Where(node => !node.GetInputPort("input").IsConnected))
			{
				if (_root is not null)
					throw new MonoRootDialogGraphException($"Two or more roots are found in  {name}");
				_root = node as DialogueNode;
				
			}

			if(_root is null) throw new NullReferenceException($"There is no root node in the current graph : {name}");
			return _root;
		}

		/// <summary>
		/// Retrieve the next node in a graph according to the current one and the answer's index chosen.
		/// </summary>
		/// <param name="current">The current node we are on.</param>
		/// <param name="outputIndex">The index of the answer chosen.</param>
		/// <returns>Return a DialogNode corresponding to the next one.</returns>
		public static DialogueNode GetNext(DialogueNode current, int outputIndex) 
		{
			var port = current.GetPort($"answers {outputIndex}");
			return port is not {IsConnected: true} ? null : port.Connection.node as DialogueNode ;
		}

		
	}
}