Unity_Strategy
=====

A Strategy game made using `Unity 2018.2.12f1 (64-bit)` were the Player controls an Army of troops and you goal is to destroy the Enemy base while Defending yours.

# Project Ghoul
As a way to learn about video game AI, I made an RTS (Real-Time Strategy) game where the player controls a small army with the goal of destroying the enemy's main army building. The core of the game consists of individual units with their own AI, allowing them to make some decisions, and a larger army manager that organizes and sorts them as it passes down orders from the player or the AI. The enemy AI is a simple Minimax algorithm that translates the battlefield into a tree of possible choices, each assigned a  value. This system gives the AI the flexibility to plan ahead and respond to the player's behavior, all while being very performance efficient. To implement many of these systems, I relied on many features of .NET and C#, such as events and delegates, to allow units to subscribe and unsubscribe to receive orders from the manager as they are being selected.

# How To Play 

1. Use the `right mouse` button to click on the the trop you want to move and click with the `left mouse` button on were you want him to move 
2. Hold `left shifht` while clickin the `right mouse` button on tropps to controll multiple tropps at the same time.
3. Click on the base to buy more uints. 
4. Capture resoce points to secure more resoces for you army
5. Move the mouse cursor to the edges of the screen to move the camera 

  # Screen Shots
![Image_1](https://github.com/Sagiv440/Unity_Strategy/blob/main/Screenshot%20from%202024-05-14%2013-18-30.png?raw=true)
![Image_2](https://github.com/Sagiv440/Unity_Strategy/blob/main/SpaceMarines.png?raw=true)
