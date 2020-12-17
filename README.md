# ai_smart_flocking

Blog post: https://adeltalhouk.wixsite.com/portfolio/post/ai-agents-smart-flocking

VERSION 1.0


DESCRIPTION:
	Boids will start flocking on their own. They will dynamically avoid the obstacles if they are too close to them, regardless of the obstacles' position(s).


INSTRUCTIONS:
	Press the Space Bar to spawn more boids. Click the Left Mouse Button to spawn an obstacle at the mouse location, press and hold to drag the obstacle to the mouse position. Click the Right Mouse Button to disable the obstacle. Press the 'W' key to switch between the circular obstacle and the squared one. Modify weights and radii in the Flock script, on the Flock GameObject in the Hierarchy. 


VALUE TWEAKING AND SAVING:
	"Steering Data" fields cannot be tweaked when the game is not running: doing so will undo the changes when you press play. Instead, modify the values when the project is running, and the next time you press play, the values will stay there (though they do not get updated in the inspector, so it will not look like it until you press play again). The same MIGHT be true for "Neighbourhood Information", though I am not sure, as changing those values will cause the framerate to drop significantly, therefore I have no tested it.


DISCLAIMER:
	This technique is not very efficient, and I will be iterating on it and updating this file and the blog post when I do so.