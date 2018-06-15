using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

	private Queue<string> sentences;
	
	void Start ()
	{
		sentences = new Queue<string>();
	}

	public void StartDialogue(Dialogue dialogue)
	{
		Debug.Log("Starting conversation with " + dialogue.name);
		
		sentences.Clear();

		foreach (string sentence in sentences)
		{
			sentences.Enqueue(sentence);
		}
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndConversation();
			return;
		}

		string sentence = sentences.Dequeue();
		
	}

	void EndConversation()
	{
		Debug.Log("End of conv");
	}
}
