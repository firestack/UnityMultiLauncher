# Unity Multi Launcher
[![GitHub release](https://img.shields.io/github/release/firestack/UnityMultiLauncher.svg)](https://github.com/firestack/UnityMultiLauncher/releases/latest) [![GitHub issues](https://img.shields.io/github/issues/firestack/UnityMultiLauncher.svg)](https://github.com/firestack/UnityMultiLauncher/issues)

Managing multiple unity installs can be a pain, so here's a tool to hopefully make that easier.
![Demo Image](https://raw.githubusercontent.com/firestack/UnityMultiLauncher/data/UMLDemo.PNG)]

## Awesome Things
 - Unity Multi Launcher finds your latest projects that have already been opened automatically!
   (Via registry, how unity does it)
 - We don't send your data anywhere! all of your secret projects stay secret!
 - You can launch both the Unity Launcher From UML or a project with the right editor version!

## Info Things
This Executable stores all your settings in a file called

`unitymultilauncher.<yourusernamehere>.cfg.json`

in it's local directory, feel free to mess with it.

## Current restrictions
 - Windows Only
	 - Windows Registry Dependency ( for Unity Project Lookup)
	 - WPF GUI Dependency
 - Can't find all instances of unity autonomously
 	- Unity does a weird thing in Windows when you install multiple versions of unity
	  it only puts the latest one into your Registry and
	  program files

---------------------------------------------------------
If you do use my code or this program please let me know!
I'd like to know how I can improve it or hear about any
bugs people may encounter.
Bugs and Issues can be submitted [here](https://github.com/firestack/UnityMultiLauncher/issues)


