# How to do an unit test for an MRTK Unity project

- [How to do an unit test for an MRTK Unity project](#how-to-do-an-unit-test-for-an-mrtk-unity-project)
  - [Context](#context)
  - [Setup](#setup)
  - [Making a test](#making-a-test)
  - [Precisions For Play Mode Tests](#precisions-for-play-mode-tests)
    - [Creating a hand](#creating-a-hand)
    - [Pinching objects](#pinching-objects)
    - [Making reference to your monobehaviours/scripts](#making-reference-to-your-monobehavioursscripts)

## Context

Microsoft and the open source community have created a lot of unit test to check if MRTK is broken or not before deploying a new version.
To do that, they even have created a whole bunch of tools to simulate the hands and other bricks of MRTK.
If you want to uses thoses tools and have access to the +300 unit test of MRTK, you can read this document.

## Setup

1. Make sure that you have the [test utilities](https://github.com/microsoft/MixedRealityToolkit-Unity/releases) package installed  
2. It's better to also add the **EditModeTests** & **PlayModeTests** folders to your project (placed in the same place as in the github project)  that you can find [here](https://github.com/microsoft/MixedRealityToolkit-Unity/tree/mrtk_development/Assets/MRTK/Tests) to have examples and the Assembly Definition with the references to the mrtk already setup  

Now everything should be ready to make unit test in the project

## Making a test

Open either the **PlayModeTests** or the **EditModeTests** in case of what you need add a C# Test Script *(Right click > Create > Testing > C# Test Script)*.  
According to [MRTK documentation](https://microsoft.github.io/MixedRealityToolkit-Unity/Documentation/Contributing/UnitTests.html) you need to change all the content of the C# Test Script to either [this for Play Mode Test](https://pastebin.com/WrY1e4W2) or [this for Edit Mode Test](https://pastebin.com/CxqPk46g). *(don't forget to rename the class according to the filename !)*.
You can change and rename the default function *TestMyFeature* to make your first unit test or add another function. In this case make sure to add `[UnityTest]` above the function to make an unit test.

## Precisions For Play Mode Tests


### Creating a hand
To create a fake hand there is 2 way of doing it show in the examples :  
The second way seems easier to use since you don't have to declare an object for the hand. *The first way maybe the old way of simulating hands during tests*

_First way :_
```c#
// Create a hand this way
var rightHand = new TestHand(Handedness.Right);

//Manipulate the hand using
yield return rightHand.Show(new Vector3(0, 0, 0.5f));
yield return rightHand.Move(new Vector3(0, 0, 0.5f));
yield return rightHand.MoveTo(new Vector3(0, 0, 0.5f));
...
```

_Second way :_
```c#
//You don't need to explicitly create a hand but you need to get the input simulation service like this :
var inputSimulationService = PlayModeTestUtilities.GetInputSimulationService(); //That's black magic

//Then you can manipulate hands like this :
yield return PlayModeTestUtilities.ShowHand(Handedness.Right, inputSimulationService);
yield return PlayModeTestUtilities.MoveHand(startPos, startPos, ArticulatedHandPose.GestureId.Open, Handedness.Right, inputSimulationService, 30);
yield return PlayModeTestUtilities.MoveHand(startPos, endPos, ArticulatedHandPose.GestureId.Pinch, Handedness.Right, inputSimulationService, 30);
```

### Pinching objects
Make sure you are opening the hand before pinch like this :
```c#
yield return PlayModeTestUtilities.MoveHand(startPos, startPos, ArticulatedHandPose.GestureId.Open, Handedness.Right, inputSimulationService, 30);
yield return PlayModeTestUtilities.MoveHand(startPos, endPos, ArticulatedHandPose.GestureId.Pinch, Handedness.Right, inputSimulationService, 30);
```
Or your hand is gonna move in a pinch position without grabbing the object  
The last parameter is the number of frame. 60 = 1sec (if you are at 60FPS)
*Note : moving a object is sensitive to the number of steps. The number of steps is kinda like the speed at which the hand is moving, and if you are smoothing the movement of the object pinched when you are moving the hand, the object may have a big offset from the hand if the hand is moving very fast.*


### Making reference to your monobehaviours/scripts

**/!\ Warning /!\ : this is modifying the whole project structure and this solution may not be suited for a large scale project ! The purpose of this example is only to demonstrate what are the problems that are going to occur and how to handle them in a basic setup.**

If you have a custom script that you want to use for your unit test, you might encounter an issue where you can't make reference to your script with things like `GetComponent<YourCustomScript>()` etc... this is because you need to define Assembly Definition for your scripts. To be able to use them in your unit test you will need to :

1. Create an Assembly Definition where your script is *(Right Click > Create > Assembly Definition)*. It is recommended to name it the same name as the folder (for convention)
2. Doing so will make a lot of errors in the scripts in the folder where you made the Assembly Definition. Unity considère chaque script dans le dossier comme faisant parti de l'Assembly Definition là où elle est créée. To fix this, click on the Assembly Definition and in the inspector add references to the MRTK's assembly definition that are needed.
You will probably have to add at least those references for example ![example](https://i.imgur.com/O3Dl3RB.png)   
3. Now that your new Assembly Definition have all the references needed (you may need to add more when the project grows) you need to add a reference of this Assembly Definition to the MRTK's Test Unit Assembly Definition.
If you took the PlayModeTests folder from the MRTK github, then you will find it in /Assets/MRTK/Tests/PlayModeTests and you will need to add a reference like so :
![example](https://i.imgur.com/4NK3xAX.png)  

And now everything should be ready to use all the scripts in the same folder as the Assembly Definition you made in your unit tests.