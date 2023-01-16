# GOAP_AI
Quick-ish implementation of GOAP and a small game to go around it

# Player README

The AI are funny things.
The creatures: 
	*Eat
	*Drink
	*Poop
	*Wander
	*and Nap in available buildings
		
	Their poop will grow into trees over time.

The Humans:
	*Eat
	*Drink
	*Chop Trees
	*Build Buildings
	When many tents are built together, they can upgrade into a house.



## Controls:

WASD   - Camera movement
Space  - Camera Up
LShift - Camera Down
	
Up Arrow   - Pitch camera up
Down Arrow - Pitch camera down
	
UI Buttons - Select what to spawn
Left Mouse - Spawn the selected object
	
	
## Placeable Objects:

Water Canister - provides water to humans and creatures
Tree - Provides food to humans and creatures. Can be chopped to build 5 tents.
Human - Usual Joe Shmoe
Creature - Sideways-walking pooper
  

# Critical Questions
README.code


    Analyze your usage of the AI behavior selection algorithm
        How did you implement it?
            Would you do it differently? Justify your answer!
			
				I think given more time, I would try to implement GOAP with a faster algorithm. I used Dijkstra's, as I've implemented it
				before - but A* would be a faster choice. Also it would be nice to not clone a graph each time an action list is needed - 
				this could be done fairly easily by resetting nodes and removing the 'start node'.
			
        Was this the correct choice for your game?
            Yes: Why was this the best choice for you game
            No: What algorithm (or combo of algorithms) would you use and why?
				
				A* would be faster - also having a smaller state class (or maybe a more focussed state class) could speed things up as well.
				Lots of cloning happens that could be avoided.
				
				See Assets/ActionPlanner.cs for examples of exactly how it functions. I essentially have a base graph of potentially disconnected
				nodes, then add a new node in where it meets other node prerequisites. This added node is then my starting point for finding
				a desired goal.
			
    What other patterns did you implement?
        Which ones were they?
        Why did you use them?
			
			In order to avoid a singleton or some static registry of items, I utilized a service locator to spawn objects that I could not directly
			reference. This is the case for many of the Action (Assets/Action.cs) subclasses, as they are not Monobehaviors and could not
			have references assigned directly to them.
			
        Would you do something different?
		
			I think that a subclass sandbox would have worked well here - especially for implementing movement code. Movement could have also
			been its own state that the AI can enter, as opposed to having each State that requires a set distance from something having to
			control movement by accessing a common script (Assets/Chaser.cs).
		
    What is a pattern you considered but chose not to use?
        What pattern was it?
			
			The Singleton - as well as Observer.
		
        Why didn't you use it?
		
			The Singleton pattern is generally bad practice, and I have overrelied on it in the past. Nystrom wrote something along the lines of
			"excessive use of the Singleton pattern probably points to a poor understanding of OOP", which is a harsh criticism and leads me to
			stay far away.
			The Observer pattern has been a crutch for me in getting around Singletons - and I have also been using them in less than optimal ways.
			Nystrom states that Observer patterns should be used between systems, not within systems - which is something I'm trying to adhere to as
			well.
		
        What did you use instead?
		
			Instead of the Singleton, I used a static class and initialized it when needed. Observer pattern was avoided by direct references and 
			by use of the Service Locator pattern.
		
    Anything else you want to add, add here.
