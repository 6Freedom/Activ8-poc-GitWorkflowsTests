# Doxygen VS DocFX

# Context

Which documentation can be used to make the documentation of Activ8

# Comparison

| | Doxygen| DocFX |
| --- | --- | --- |
| Installation | - | + |
| Documentation (on it) | = | = |
| Resources | - | + |
| Functionment | - | + |


# Doxygen

### Installation

Easy to install, explained [here](https://www.doxygen.nl/manual/starting.html).

### Documentation

Documentation is ok, but doesn't have a lot of recent resources, or resources at all.

### Resources

Not much resources, and nothing really recent.

### Functionment

Generates HTML files directly


# DocFX

### Installation

Easy installation with [this](https://github.com/NormandErwan/DocFxForUnity).

### Documentation

Well documented and a lot of resources   
[documentation](https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html)

### Resources

A lot of resources are made, templates, github workflows and examples

### Functionment

Operates a web server and generates the HTML Files

# Conclusion

DocFX seems easier to use and setup. A lot more resources are available for DocFX than for Doxygen. For DocFX doc templates are available and easy to install and not every resources you look for were made before 2016. 
Markdown are handled in a better way for DocFX.


# More about DocFX

## Setup a workflow to make the documentation update automatically

The goal is to make a branch that will be dedicated to being the documentation. It will contains nothing more than the website file to make the documentation.

Firstly, you need to setup a workflow by following [this](https://github.com/NormandErwan/DocFxForUnity#generate-automatically-your-documentation). Be sure to edit it so that it triggers with the pushes on the correct branch.
This workflow will create a branch `gh-pages` and rebuild the website everytime it is needed.  
Then, make sure that in the Settings repo, the `Source` of the github pages of your project is the correct branch (it should be `gh-pages`).  
Lastly, you need to edit the `.gitignore` so that you can have the `.sln` and the `.csproj` files of your project (DocFX builds its automatic documentation based on that).  
Everything should be ready now.

## Troubleshooting

If there is no `.csproj` file in your project folder you can generate them with Unity in your `Preferences > External Tools` check `Generate all .csproj files`. You are going to want to uncheck it after that or you won't be able to delete the uncesseray `.csproj` files. Then in your project's root you can delete the default's unity csproj or else DocFX with add them to the documentation : 

![example](https://i.imgur.com/hRBeLFh.png)


# Solution to have docFX for multiple projects 

#### Problème :  
Chaque repo a sa doc (= son site), comment faire pour avoir 1 seul doc pour tous les repos ?  

#### Solution proposée :    
Avoir des [liens absolus](https://github.com/dotnet/docfx/issues/2129) dans la navbar qui amène vers les autres docs + un site qui servira de home page   
chaque documentation des repos utiliseront le même thème (ou au moins la même base) pour qu'il y ait une cohérence dans la documentation