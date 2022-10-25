using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureState : State
{
	public float wanderThreshold = 25;
	public float chillValue = 0;

	protected override void Start()
	{
		base.Start();
		SetPrereq(new KeyValuePair<string, bool>("isCreature", true));
	}

	override protected void Update()
	{
		base.Update();

		if (hunger > 50)
			SetPrereq(new KeyValuePair<string, bool>("needsFood", true));

		if (thirst > 50)
			SetPrereq(new KeyValuePair<string, bool>("needsWater", true));


		SetPrereq(new KeyValuePair<string, bool>("wantsRelax", chillValue < wanderThreshold));
		SetPrereq(new KeyValuePair<string, bool>("wantsWander", ! (chillValue < wanderThreshold) ));

	}

	protected override void UpdateGOAP()
	{
		if (currentActions != null)
			return;

		// Get a list of actions that we currently meet the prerequisites for
		List<Action> validActions = ActionPlanner.GetValidActions(this);

		if (hunger > 50)
		{
			currentActions = ActionPlanner.FindActionsTowardsGoal(new KeyValuePair<string, bool>("needsFood", false), validActions);
		}
		else if (thirst > 50)
		{
			currentActions = ActionPlanner.FindActionsTowardsGoal(new KeyValuePair<string, bool>("needsWater", false), validActions);
		}

		// Find something to do if not doing anything else
		else
		{
			currentActions = ActionPlanner.FindActionsTowardsGoal(new KeyValuePair<string, bool>("chill", true), validActions);
		}

		base.UpdateGOAP();
	}
}
