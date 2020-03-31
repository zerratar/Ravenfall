# Servers

All servers are single threaded except for the Front Server that will handle multiple connections

## Front Server / Connection Server
### The server that will hold all game connections, redirect data where needed
Port 8000
Accessibility Public

## Master Server
### The server that keeps track on active servers, this is the center of providing information about all running servers (Excluding ZoneServers)
Port 8001
Accessibility Internal

## Login Server
### The server that handles login requests sent from the Front Server. This has read access to the db
Port 8002
Accessibility Internal

## Game Server - Instanced
### A game server instance keeps track on Zone Servers and does all player persistance to the database. This also includes non realtime updates.
### Only one game server instance can be run per computer as the port is required for zone server connections
Port 8003
Accessibility Internal

## Zone Server - Instanced
### A zone server instance keeps track of players, AIs, and other realtime events. Nothing is saved here.
Client, connects to Game Server
Accessibility Internal


## Ideas
### Synchronizing player movements
Since we are using a pathfinding, click to move kind of system. We can interpolate between the start (previous destination), destination, (length, which is minimum dest-start) 
to get an estimate % of progression. We can then use that to determine where a player is when another player joins the game while the player is walking.

Example: Player A walks from 10,0,20 to 44,0,7. The distance is grabbed from the pathfinding algorithm, be it unitys built in navmeshagent when sending walkto destination, also provide a length
		 this can potentially be hacked by players. but only to make the character move as if they were walking through walls. basically. But it would appear as if their movement speed is crazy.

Another solution? Do the a-star algorithm on the server side. but this requires more CPU usage and also a copy of the world map.