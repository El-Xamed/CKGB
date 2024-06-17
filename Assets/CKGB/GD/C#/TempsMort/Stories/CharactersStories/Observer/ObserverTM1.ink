->checkID
=== function Trigger(name)
EXTERNAL Trigger(name)

=== function RetourAuTMAfterRespirer(name)
EXTERNAL RetourAuTMAfterRespirer(name)
VAR IDobserver = 0
===checkID===
{ 
- IDobserver==1: ->Observer0
- IDobserver==2: ->Observer1
- IDobserver==3: ->Observer2
- else: ->END
}
===Observer0===
~ Trigger("name")
Tu tournes légèrement la tête.#Bulle:Narrateur
~ Trigger("name")
Clic.#Bulle:Narrateur
Comme la pellicule d’un appareil photo,<br> tu sens que ton cerveau capture l’instant.#Bulle:Narrateur
L’odeur de cannelle et de menthe laissée par le thé. L’amertume d’un café oublié qui s’y emmêle.#Bulle:Narrateur
~ Trigger("name")
La toile verte, débordante de taches multicolores au travers de la fenêtre.#Bulle:Narrateur
Les rayons de soleil cristallins qui transpercent l’espace par l’immense baie vitrée.#Bulle:Narrateur
~ Trigger("name")
La sensation du papier quand tu déplaces machinalement les cahiers de cours oubliés sur la table afin d’éviter qu’ils ne se fassent tacher.#Bulle:Narrateur
La chaleur des tasses. Le sucre de la confiture. Le petit choc sourd d’une cuillère en bois qui cogne contre l'un des récipients en céramique. #Bulle:Narrateur
Tu apprécies ces matins tranquilles. Les jours sans école, vous vous retrouvez souvent tous les deux, comme ça. #Bulle:Narrateur
Les autres se lèvent tôt. Ils vont explorer la ville, aider dans les champs, dessiner les insectes,#Bulle:Narrateur
travailler à l'atelier, jouer à la salle d’arcade, nourrir les animaux.#Bulle:Narrateur
Aux premières heures du jour, la maisonnée bourdonne d’une rimbambelle d’adelphes, de cousins, d’amis, de parents et de grands-parents en train de se préparer,#Bulle:Narrateur
avant de les relâcher dans la nature et de se retrouver silencieuse. #Bulle:Narrateur
Si l'on fait exception de vous,#Bulle:Narrateur
et de vos petits chocs sourds quand vos cuillères en bois se cognent à vos tasses en céramique. #Bulle:Narrateur
Vous aimez bien ces matinées ensemble.#Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE
===Observer1===
~ Trigger("name")
Tu te brûles les lèvres et fais une grimace. #Bulle:Narrateur
Ta tasse est trop chaude. Tu te précipites pour la reposer avant qu’elle ne brûle complètement tes doigts.#Bulle:Narrateur
~ Trigger("name")
Presque par réflexe, tu te diriges vers le lavabo pour verser de l’eau fraîche dans ta bouche.#Bulle:Narrateur
Tu n’es pas sûr.e que ça change grand chose. Mais ça t’a toujours semblé comme la marche à suivre. #Bulle:Narrateur
L’eau sur ton palais a un léger arrière-goût. #Bulle:Narrateur
Tu recules, et vérifie l’affichage du cabinet de la tuyauterie. #Bulle:Narrateur
~ Trigger("name")
Tes yeux habitués à ce geste décryptent sans mal les chiffres et les abréviations.#Bulle:Narrateur
La composition de l’eau est légèrement déviante du standard local, mais rien de dangeureux. #Bulle:Narrateur
Le liquide se meut de façon presque hypnotisante au travers des tuyaux translucides, coulant le long des murs depuis la citerne d’eau enfouie dans la colline.#Bulle:Narrateur
La dernière sécheresse remonte à bientôt un an.#Bulle:Narrateur
Alors que tu te rasseoies, ton esprit continue de suivre le courant. L’eau du lavabo, considérée comme grise à sa sortie, tombe directement dans un nouveau bac de filtres.#Bulle:Narrateur
Ceux-ci sont composés de certains minéraux, comme le gravier, mais aussi des racines de plantes des alentours.#Bulle:Narrateur
Ce qui reste après le filtrage sera utilisé pour l’eau des toilettes ou elle deviendra de l’eau noire, #Bulle:Narrateur
et suivra ensuite son propre cycle de traitement.#Bulle:Narrateur
~ Trigger("name")

Tu trouves cela beau.#Bulle:Narrateur
Élégant.#Bulle:Narrateur
Chaque conséquence de chaque action vient s’emboîter fluidement,#Bulle:Narrateur
et chaque production collatérale est utilisée pour résoudre un des besoins de la maison.#Bulle:Narrateur
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