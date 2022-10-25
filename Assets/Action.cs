using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Action
{
	public List<KeyValuePair<string, bool>> prerequisites;
	public List<KeyValuePair<string, bool>> effects;

	public State state;
	public bool completed = false;
	public float cost;

	public Action() { 
		prerequisites = new List<KeyValuePair<string, bool>>(); 
		effects = new List<KeyValuePair<string, bool>>(); 
	}
	public abstract Action Clone();

	public bool MeetsPrerequisites(State s)
	{
		foreach (KeyValuePair<string, bool> prereq in prerequisites)
		{
			bool meetsPrereq = false;
			foreach (KeyValuePair<string, bool> met in s.prereqsMet)
			{

				if (met.Key == prereq.Key && met.Value == prereq.Value)
				{
					meetsPrereq = true;
					break;
				}
			}

			if (!meetsPrereq)
				return false;
		}

		return true;
	}
	
	public bool HasEffects(List<KeyValuePair<string, bool>> wantedEffects)
	{
		foreach (KeyValuePair<string, bool> wantEffect in wantedEffects)
		{
			bool hasEffect = false;
			foreach (KeyValuePair<string, bool> eff in effects)
			{

				if (eff.Key == wantEffect.Key && eff.Value == wantEffect.Value)
				{
					hasEffect = true;
					break;
				}
			}

			if (!hasEffect)
				return false;
		}

		return true;
	}
	
	public bool HasEffect(KeyValuePair<string, bool> wantedEffect)
	{
		bool hasEffect = false;
		foreach (KeyValuePair<string, bool> eff in effects)
		{

			if (eff.Key == wantedEffect.Key && eff.Value == wantedEffect.Value)
			{
				hasEffect = true;
				break;
			}
		}

		return hasEffect;
	}

	public virtual void OnCompletion()
	{
		effects.ForEach(e => { state.SetPrereq(e); });
		completed = true;
	}
	public abstract void OnStart();
	
	// Returns True if the action completed sucessfully or False if the action had to be cancelled
	public abstract bool OnTick();
}
