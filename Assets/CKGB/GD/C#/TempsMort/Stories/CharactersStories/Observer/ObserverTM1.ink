->checkID

=== function RetourAuTMAfterRespirer(name)
EXTERNAL RetourAuTMAfterRespirer(name)
VAR IDobserver = 0
===checkID===
{ 
- IDobserver==0: ->Observer0
- IDobserver==1: ->Observer1
- IDobserver==2: ->Observer2
- else: ->END
}
===Observer0===

//blabla

Tu tourne légèrement là tête.#Bulle:Narrateur
Click.#Bulle:Narrateur
Comme la pellicule d’un appareil photo, tu sens que ton cerveau capture l’instant.#Bulle:Narrateur
L’odeur de cannelle et de menthe laissée par le thé. L’amertume d’un café oublié qui s’y emmèle.#Bulle:Narrateur
La toile verte, débordante de tâche multicolore au travers de la fenêtre.#Bulle:Narrateur
Les rayons de soleil cristallins qui transpercent l’espace par l’immense baie vitrée.#Bulle:Narrateur
La sensation du papier quand tu déplace machinalement les cahier de cours oubliés sur la table afin d’éviter qu’ils ne se fassent tacher.#Bulle:Narrateur
La chaleur des tasses. Le sucre de la confiture. Le petit choc sourd d’une cuillère en bois qui se cogne à une tasse en céramique. #Bulle:Narrateur
Tu aprécies ces matins tranquilles. Les jour sans école, vous vous retrouvez souvent tous les deux, comme ça. #Bulle:Narrateur
Les autres se lèvent tôt. Ils vont explorer la ville, aider dans les champs, dessiner des insectes, travailler dans un atelier, jouer à la salle d’arcade, nourrir des animaux.#Bulle:Narrateur
Au premières heures du jour, la maisonnée bourdonne d’une rimbambelle d’adelphes, de cousins, d’amis, parents et de grands-parents en train de se préparer,#Bulle:Narrateur
avant de les relâcher dans la nature et de se retrouver silencieuse. #Bulle:Narrateur
Si on fait exception de vous,#Bulle:Narrateur
et de vos petits chocs sourds quand vos cuillères en bois se cogne à vos tasses en céramique. #Bulle:Narrateur
Vous aimez bien ces matinées ensemble.#Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE
===Observer1===

//blabla

Tu te brûle les lèvres et faits une grimace. #Bulle:Narrateur
Ta tasse est trop chaude. Tu te précipite pour la reposer avant qu’elle ne brule complètement tes doigts.#Bulle:Narrateur
Presque par reflexe, tu te dirige vers le lavabo pour verser de l’eau fraiche dans ta bouche.#Bulle:Narrateur
Tu n’es pas sûr que ça change grand chose. Mais ça t’as toujours semblé comme la marche à suivre. #Bulle:Narrateur
L’eau sur ton palais a un léger arrière gout. #Bulle:Narrateur
Tu recule, et verifie l’affichage du cabinet de la tuyauterie. #Bulle:Narrateur
Tes yeux habitués à ce geste décryptent sans mal les chiffres et les abréviations.#Bulle:Narrateur
La composition de l’eau est légèrement déviante du standard local, mais rien de dangeureux. #Bulle:Narrateur
L’eau a un mouvement presque hypnotisant au travers des tuyaux translucides, coulant le long des murs depuis la citerne d’eau enfouie dans la colline.#Bulle:Narrateur
La dernière sécheresse remonte à bientôt un an.#Bulle:Narrateur
Alors que tu te rasseoies, ton esprit continue de suivre le courant. L’eau du lavabo, considérée comme grise à sa sortie, tombe directement dans un nouveau bac de filtres.#Bulle:Narrateur
Certains minéraux, comme le gravier, mais aussi les racines des plantes des alentours.#Bulle:Narrateur
Ce qui reste après le filtrage sera utilisé pour l’eau des toilettes ou elle deviendra de l’eau noire, #Bulle:Narrateur
et suivra ensuite son propre cycle de traitement.#Bulle:Narrateur

Tu trouve cela beau.#Bulle:Narrateur
Élégant.#Bulle:Narrateur
Chaque conséquence de chaque action vient s’emboiter fluidement,#Bulle:Narrateur
et chaque production collatéral est utilisé pour résoudre un des besoin de la maison.#Bulle:Narrateur
On prend souvent pour acquis ce qui nous entoure,#Bulle:Narrateur
mais à cet instant précis,#Bulle:Narrateur
tu es fier.e d’être dans une maison aussi ingénieuse.#Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE
===Observer2===


//blabla

On a que deux personnages dans ce temps mort  #Bulle:Narrateur
Comment est-ce que tu es arrivé là ??? #Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE

->END