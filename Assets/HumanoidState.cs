using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidState : State
{

	protected override void Start()
	{
		base.Start();
		SetPrereq(new KeyValuePair<string, bool>("isCreature", false));
	}

	override protected void Update()
	{
		base.Update();

		if (hunger > 50)
			SetPrereq(new KeyValuePair<string, bool>("needsFood", true));

		if (thirst > 50)
			SetPrereq(new KeyValuePair<string, bool>("needsWater", true));

		if (woodCount < 1)
			SetPrereq(new KeyValuePair<string, bool>("needsWood", true));
	}

	protected override void UpdateGOAP()
	{
		// Don't overwrite actions
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

		// Find something to do
		else
		{
			currentActions = ActionPlanner.FindActionsTowardsGoal(new KeyValuePair<string, bool>("houseBuilt", true), validActions);
		}

		base.UpdateGOAP();
	}
}
