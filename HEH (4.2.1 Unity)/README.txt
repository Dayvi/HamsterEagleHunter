8/6/13
Added Player Rotation (just does exact angles for now, adding a smooth transition next)
Added Battery and Watch Asset
	Can pick up Watch and a GUI indicator appears in the top left corner of the screen
Angles in the top right now display actual angle amount
Delete and Insert now function properly
Gravity increased slightly
Respawn fixed so player doesn't continue moving from the inertia of being hit


8/4/13
Added Delete and Insert for camera zooming in and out
Added attack radius for the flying enemy (only triggers when in attack range and stops when out of range)
The camera now does not reset if the player dies
Default camera settings set to: 0.5, 0.1, 0.0, 0.9
Added basic item pick up (no inventory or gui indicator present currently)
Added small ledges to jump over
Added shadows (Though can take out, just tested aesthetics)


7/31/13

Added Damage Taken to player, displayed on top left of screen
Falling off the ledges kills player and restarts him
Flying Enemy looks at player then moves towards it.
	Knocks player backwards and deals damage to player on collision.
	The enemy sometimes stops moving right before hitting player. Looking into!

7/30/13

Added camera rotation (Home is rotate left, End is rotate right, Page Up is rotate up, and Page Down is rotate down)
Added Hadron model

7/29/13

Control character with WASD. 
Space is jump.
Added mouse wheel zoom in and out so you can see different distances. 