#How to do Continuous Integration with github actions and Unity  
Continuous integration is made through Github Actions (you can access them with the tab Actions of your github repo page) that lets you run actions automatically on certain events such as build on a push of a certain branch, run unit test...

##Making github actions
To make github actions you need to go to the **Actions** page of your project, and you can click on **New workflow** for your first action. by default actions are created in *.github/workflows*. Actions are **.yml** files made of multiple parts that are well detailed [here](https://docs.github.com/en/free-pro-team@latest/actions/learn-github-actions/introduction-to-github-actions). 

##Setting up the project
Follow meticulously [this to get a license in function of the type of license you have](https://unity-ci.com/docs/github/activation) *(I tried to use a license I made through the unity hub but it didn't worked so it is important to get it this way)*
 

##Use Case : Building your project on every push

The best way to explain how to do things is through examples, so here is a very simple use case on a script to make a build on every push

You need to checkout the project at the beginning of your actions or either your workflows won't be able to access the project

```yaml
#Name of the actions
name: Generate Build

#On every push
on: [push]

#Important for the license, the setup is different if you have a professional license
env:
  UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}

jobs:
  build:
    runs-on: ubuntu-latest
    
    #Checkout is very important at the start of every actions ! the container won't be able to access the project if you don't have a checkout
    steps:
    - name: Unity - Checkout
      uses: actions/checkout@v2
    
    #Make the action build your project
    - name: Unity - Builder
      uses: webbertakken/unity-builder@v1.4
      with:
        unityVersion: 2019.3.15f1
        targetPlatform: StandaloneWindows
    
    - name: Publish artifact
      uses: actions/upload-artifact@v1
      with:
        name: Build
```

Available `targetPlatforms` :  

| Code name | Description |
| ------ | ------ |  
| StandaloneOSX | Build a macOS standalone (Intel 64-bit). 
| StandaloneWindows     | Build a Windows standalone.
| StandaloneWindows64   | Build a Windows 64-bit standalone.
| StandaloneLinux64     | Build a Linux 64-bit standalone.
| iOS                   | Build an iOS player.
| Android               | Build an Android .apk standalone app.
| WebGL                 | WebGL.
| WSAPlayer             | Build an Windows Store Apps player.
| PS4                   | Build a PS4 Standalone.
| XboxOne               | Build a Xbox One Standalone.
| tvOS                  | Build to Apple's tvOS platform.
| Switch                | Build a Nintendo Switch player.

Error expected if you don't have a checkout step :
```
Aborting batchmode due to failure:
Couldn't set project path to: /github/workspace/github/workspace/.
```


##Resources
- [webbertaken's github repo that index most of the actions possible with unity](https://github.com/webbertakken/unity-actions)
- [Unity CI documentation](https://unity-ci.com/docs/github/getting-started)
- [why checkout is important](https://github.com/actions/checkout#checkout-v2)

#Building a hololens package automatically
It's not possible to build visual studio solution for hololens with Unity CI at the moment simply [because of this](https://github.com/webbertakken/unity-builder/issues/35)  

**Conclusion :** 
- Either no CI
- Either we make a docker image ourself and we configure it
- Either we will use a virtual machine with windows dedicacted with jenkins