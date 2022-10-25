using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
	public float hunger = 0;
	public float thirst = 0;

	public int woodCount = 0;

	public List<KeyValuePair<string, bool>> prereqsMet = new List<KeyValuePair<string, bool>>();

	protected List<Action> currentActions;

	// Hunger rate per second
	private readonly float HUNGER_RATE = 0.65f;
	private readonly float THIRST_RATE = 0.45f;

	protected virtual void Start()
	{
		ActionPlanner.Initialize();
		InvokeRepeating(nameof(UpdateGOAP), 0, 0.5f);
	}

	protected virtual void Update()
	{
		hunger += HUNGER_RATE * Time.deltaTime;
		thirst += THIRST_RATE * Time.deltaTime;

		if (currentActions != null)
		{
			// If we cancelled, we need to rethink
			if (!currentActions[0].OnTick())
			{
				currentActions = null;
				return;
			}

			if (currentActions[0].completed)
			{
				currentActions.RemoveAt(0);
				if (currentActions.Count == 0)
					currentActions = null;
				else
					currentActions[0].OnStart();
			}
		}

		if (hunger > 100 || thirst > 100)
			Destroy(gameObject);
	}

	protected virtual void UpdateGOAP()
	{

		currentActions.ForEach(a => { a.state = this; });
		currentActions[0].OnStart();
	}

	public void SetPrereq(KeyValuePair<string, bool> prereq)
	{
		RemovePrereqMetIfPresent(prereq, false);
		prereqsMet.Add(prereq);
	}
	public void RemovePrereqMetIfPresent(KeyValuePair<string, bool> prereq, bool matchVal)
	{
		KeyValuePair<string, bool> toRemove;
		foreach (KeyValuePair<string, bool> p in prereqsMet)
		{
			if (p.Key == prereq.Key && (!matchVal || p.Value == prereq.Value))
				toRemove = p;
		}

		if (prereqsMet.Contains(toRemove))
			prereqsMet.Remove(toRemove);
	}
}
