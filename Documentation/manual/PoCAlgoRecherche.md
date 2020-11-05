#PoC Algorithme de Recherche

## Algorithmes de sous-chaîne
de : https://www.wikizero.com/fr/Algorithme_de_recherche_de_sous-cha%C3%AEne  

Les algorithmes de recherche unique n'ont pas l'air de supporter les typos et les fautes, les rendants beaucoup moins intéressants pour notre cas d'utilisation

## Algorithmes de mesure de similarité ou Fuzzy Matching

Le fait de chercher par similarité a pour nom **Fuzzy Matching**.
  
Parmi les algorithmes de mesure de similarité, il semblerait que l'algorithme de [Distance de Levenshtein](https://www.wikizero.com/fr/Distance_de_Levenshtein) semble être le plus populaire, permettant de mesurer la différence entre 2 mots et ainsi autoriser un seuil de différence pour supporter les typos.

L'algorithme de distance de Levenshtein ne donne pas directement la liste des résultats qui correspondrait à la recherche mais va donc donner un score de correspondance qui permettra donc d'accepter les erreurs. 

## Fuzzy Sharp

[Une librairie pour faire des recherches en Fuzzy Matching](https://github.com/JakeBayer/FuzzySharp/tree/master/FuzzySharp) existe déjà, et semble être la meilleur solution pour faire des recherches qui accepte les typos.  
Elle est disponbile également sur [NuGet](https://www.nuget.org/packages/FuzzySharp) permettant de l'installer facilement dans le projet. 