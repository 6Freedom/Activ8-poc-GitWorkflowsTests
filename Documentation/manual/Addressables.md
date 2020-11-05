## Table of content
- [Contexte](#contexte)
  - [Nota Bene](#nota-bene)
- [Réalisation d'un proof of concept addressables](#réalisation-dun-proof-of-concept-addressables)
  - [Création de la scène](#création-de-la-scène)
  - [Test de la scène](#test-de-la-scène)
  - [Instanciation et nouveaux scripts](#instanciation-et-nouveaux-scripts)
- [Paramétrage des Addressables](#paramétrage-des-addressables)
- [Conclusions](#conclusions)
- [Liens utiles](#liens-utiles)

# Contexte

Le but des addressables est de pouvoir lié des assets d'un projet unity à une adresse.  
Exemple :

* Vous faite un projet Unity
* Au démarrage votre scène ne contient qu'une camera et une light
* Vous faite un prefab qui contient un cube _cube.prefab_
* Ensuite vous écrivez un script _CubeGenerator.cs_
* Vous posez le script sur un gameobject
* Ce script contient une variable de type **GameObject** _myPrefab_ qui est visible dans l'inspector
* Le script instancie le cube au _Start()_ au centre du monde
* Vous revenez dans l'éditeur, drag&droppez le prefab dans l'inspector pour donner la référence au script
* Vous buildez

Pour ce projet, votre build va contenir (pour simplifier) 3 assets : la scène, le script et le prefab.

Avec les addressables voici ce qui se passe :

* Vous faite un projet Unity
* Au démarrage votre scène ne contient qu'une camera et une light
* Vous faite un prefab qui contient un cube _cube.prefab_
* Ensuite vous écrivez un script _CubeGenerator.cs_
* Vous posez le script sur un gameobject
* Ce script contient une variable de type AddressableAsset _myPrefabAddressable_ qui est visible dans l'inspector
* Ce script load l'asset via _myPrefabAddressable_ 
* Une fois l'objet loadé, le script converti l'asset en prefab puis instancie le prefab au centre du monde
* Vous revenez dans l'éditeur, vous aller voir l'inspector du script et vous aller pouvoir choisir l'addressable du prefab
* Ensuite vous spécifiez dans les paramètre du projet où seront stocké les addressables (par exemple https://6freedom.studio/addressables/projetTest)
* Ensuite vous faite une build des addressable dans la fenêtre associé
* Ensuite vous faite une build de votre projet Unity

Pour ce projet, votre build va contenir (pour simplifier) que 2 assets : la scène et le script. Le prefab sera téléchargé au moment où le script va le load.  

## Nota Bene

* Si l'utilisateur n'est pas connecté à internet, il faut prévoir un message d'erreur
* Si l'utilisateur a téléchargé un addressable une première fois, il est stocké dans le disque dur et n'a plus besoin d'internet
* Il est possible de load tous les assets au démarrage de l'application et rajouter une barre de chargement. C'est potentiellement plus pratique pour les utilisateurs
* Les addressables ont une date. Ce qui permet de vérifier si il existe une mise à jour de ces assets
* A chaque fois que l'équipe de développement veux mettre à jour les assets, il faut refaire une build des addressables puis les déposer sur le stockage
* Il est possible de choisir entre packager tous les assets dans un seul gros fichier, ou d'avoir plusieurs petits fichiers

# Réalisation d'un proof of concept addressables

* Utilisation d'un serveur Azure Blob storage [fichier d'exemple stocker sur ce serveur](https://6freedomstorage.blob.core.windows.net/addressables/logo-6freedom-RVB.png)
* Objectif : 
  * Réussir à mettre à jour des scripts, des scènes, des prefabs, des textures, des matérials, des sons
  * Réussir à créer de nouveau comportement qui n'était pas prévu par le logiciel de base
  * Tester la différence entre "un chargement complet au démarrage" et "charger les fichiers un par un"


D'abord il faut installer le package Addressables. Voici le lien vers la documentation : https://docs.unity3d.com/Packages/com.unity.addressables@1.8/manual/index.html

![Package](https://i.imgur.com/QAgsyvm.png)

## Création de la scène

Premièrement j'ai créer une scene vide nommé _Loader_ qui contient un seul script _Loader.cs_. Le but est que ce script charge la scene _MainMenu_ qui elle est un Addressable.

![MainMenuAddressable](https://i.imgur.com/c1DLVvw.png)

 Première remarque : une scène Addressable ne peux pas être dans le build settings. Donc on ne peux pas loader la scène par son _buildIndex_. Secondement, pour charger un asset il faut faire 

``Addressables.LoadAssetAsync<GameObject>(monPrefab).Completed += OnLoadDone;``

mais pour les scènes il faut faire

``Addressables.LoadSceneAsync(mainMenuScene).Completed += Loader_Completed;``

Donc mon Loader ressemble à ça :

```csharp
public class Loader : MonoBehaviour
{
    public AssetReference mainMenuScene;

    // Start is called before the first frame update
    void Start()
    {
        Addressables.LoadSceneAsync(mainMenuScene).Completed += OnLoadDone;
    }

    private void OnLoadDone(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log($"Scene{obj.Result.Scene.name} successfully loaded");
        }
        else
        {
            Debug.LogError("You don't have access to the internet or something");
        }
    }
}
```

Dans la scène _MainMenu_ j'ai rajouté un bouton dans un canvas et un objet 3D. Ni le bouton, ni l'objet 3D ne sont taggé comme Addressables. Ils sont déjà dans une scène Addressable, donc pas besoin de cocher Addressables pour ces assets.
![Vue Scene](https://i.imgur.com/e7oSYJy.png)

---

## Test de la scène

Pour tester cette scène, j'ai fait une _New Build_ des Addressables. J'ai mis seulement la scène Loader dans les build settings puis j'ai fait une build Windows Standalone.

Je me retrouve donc, dans un dossier _ServerData/_ avec les addressables buildé et dans un dossier _/Build_ la build WindowsStandalone.

![Builds Folder](https://i.imgur.com/fmJ0qLf.png)

Ensuite je copie ce qui se trouve dans le dossier ServerData sur le serveur de stockage.

![Copy to server](https://i.imgur.com/2lLTVB3.png)

Ensuite je lance l'application et je vois bien l'objet 3D et le bouton.

Ensuite j'ai rouvert Unity, j'ai changé le bouton de couleur et j'ai mis un autre asset 3D. Ensuite j'ai fait une mise à jour des Addressables (_Update Previous Build_). Puis j'ai recopier sur le serveur de stockage

Puis j'ai relancer l'application **(sans nouvelle build Unity)** et j'ai le nouveau bouton et le nouvel objet 3D.

![Updated scene](https://i.imgur.com/DVeQL8O.png)

## Instanciation et nouveaux scripts

J'ai rajouté un script qui permet, lors de l'appui sur le bouton _Spawn some snow_ d'instancier un prefab avec un _particle system_. Ce bouton fonctionne très bien dans l'éditeur mais ne fonctionne pas du tout dans la build. 

![Spawner](https://i.imgur.com/DmD0g54.png)

**Pourquoi ?**

En ouvrant les logs de l'application, voici ce que l'on peux voir :

![Error](https://i.imgur.com/BTXld3V.png)

Car selon la [documentation officiel](https://docs.unity3d.com/Packages/com.unity.addressables@1.16/manual/ContentUpdateWorkflow.html#project-structure) :
> Addressables can only distribute content, not code. As such, a code change generally necessitates a fresh player build, and often a fresh build of content

La raison c'est que les scripts doivent être compilés. Or il n'y a pas de compilateur dans l'application en elle même et les scripts ne sont pas compilés au moment de buildé les Addressables.

# Paramétrage des Addressables

Dans les paramètres de profiles des addressables j'ai rajouté un profil nommé 6freedom (1) puis j'ai rajouté deux variables (4) et (5). Ensuite j'ai choisi que les addressables seraient buildé dans un dossier sur mon disque dur (2). Et que le logiciel ira les loader depuis le serveur azure.

![Profiles Parameters](https://i.imgur.com/fcpGSNn.png)

Dans le _Default Local Group_ j'ai mis _Build Path_ en _RemoteBuildPath_ et le _Load Path_ en _RemoteLoadPath_ pour que, par défaut, ça charge les assets depuis le serveur. Le paramètre _Bundle Mode_ permet de choisir si on construit un seul gros fichier _.bundle_ ou un fichier par asset ou un fichier par label prédéfinis à l'avance.

![Default Local Group](https://i.imgur.com/yvkbmUB.png)

Dans _AddressableAssetSettings_ j'ai aussi mis les _Build Path_ et les _Load Path_ en _Remote_. En plus j'ai coché _Build Remote Path_

![AdressableAssetSettings](https://i.imgur.com/OVQf4qj.png)

Dans la fenêtre _Groups_ je n'ai ajouté que la scene _MainMenu_

Lors de la build des addressables il faut **faire attention** car il y a deux façons.
* **(1) Faire une nouvelle version** : créer un catalogue avec une date précise. A faire avant de faire un nouveau build du logiciel avec Unity
![New Build](https://i.imgur.com/URMjCJO.png)

* **(2) Mettre à jour une version** : modifie le dernier catalogue en date. A faire quand on veux mettre à jour les fichiers sur le serveur de stockage.
![Update Previous Build](https://i.imgur.com/fQGSFTo.png)

# Conclusions

1. Il est possible de à mettre à jour des assets mais pas des scripts.
2. Si on veux mettre à jour des scripts il faut refaire une build et livrer cette build aux utilisateurs.
3. Il est donc impossible de créer de nouveau comportement qui n'était pas prévu par le logiciel de base par une simple mise à jour des Addressables sur le serveur.
4. Il n'y a pas besoin de mettre en Addressables les assets qui sont référencé par les gameobjects d'une scène si celle ci est en Addressable.


# Liens utiles
* [Blog post about Addressables](https://thegamedev.guru/unity-addressables/benefits-for-your-game/)
* [Official Documentation](https://docs.unity3d.com/Packages/com.unity.addressables@1.16/manual/)
* [Tutorial how to build from remote path](https://www.youtube.com/watch?v=KJbNsaj1c1o)
* [Load scene with addressables tutorial](https://learn.unity.com/tutorial/addressables-scene-loading)
* [More tutorials](https://learn.unity.com/course/asset-management-with-addressables-toolkit-2019-3)