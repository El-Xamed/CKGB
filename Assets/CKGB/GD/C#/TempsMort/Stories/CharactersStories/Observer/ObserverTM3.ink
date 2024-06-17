->checkID
=== function Trigger(name)
EXTERNAL Trigger(name)

=== function RetourAuTMAfterRespirer(name)
EXTERNAL RetourAuTMAfterRespirer(name)
VAR IDobserver = 0
===checkID===
{ 
- IDobserver==1: ->observer0
- IDobserver==2: ->observer1
- IDobserver==3: ->observer2
- else: ->END
}
===observer0===

Sur une table, des gens jouent aux cartes.#Bulle:Narrateur
Il y a plus de personnes intéressées que de places,#Bulle:Narrateur
et les gens qui attendent leur tour forment un banc de poissons#Bulle:Narrateur
en arc de cercle autour de l’espace de jeu.#Bulle:Narrateur
~ Trigger("name")
Il y a quelque chose de fascinant#Bulle:Narrateur
dans le fait de regarder des gens jouer à un jeu auquel tu ne connais pas les règles.#Bulle:Narrateur
~ Trigger("name")
C'est comme regarder des gens parler dans une autre langue.#Bulle:Narrateur
Les cartes qui s’échangent tacitement,#Bulle:Narrateur
les gestes apparement anodins qui s’attirent les rires ou les insultes. #Bulle:Narrateur
~ Trigger("name")
La tension de ceux qui pensent gagner mais essaient de ne pas paraître présomptueux.#Bulle:Narrateur
L’attitude maussade de ceux qui pensent perdre, sans solution en vue.#Bulle:Narrateur
Ton oreille est attirée par de la musique un peu plus loin.#Bulle:Narrateur
~ Trigger("name")
Là, se balançant legèrement sur place,#Bulle:Narrateur
un joueur d’accordéon s’exerce à bas volume.#Bulle:Narrateur
Une famille l’observe en silence. #Bulle:Narrateur
L'enfant se penche, la bouche béante d’admiration.#Bulle:Narrateur
Ses parents savourent ce moment passé avec lui, une main quelque part sur le dos de leur pitchoun.#Bulle:Narrateur
Le grattement irrégulier du train parcourant les collines fournit un étrange roulement de tambour en tant que percussion. #Bulle:Narrateur
Peut-être que tu devrais faire un tour dans un des Wagons Calmes,#Bulle:Narrateur
juste pour prendre une pause du bruit.#Bulle:Narrateur
Un bêlement te fait encore rediriger ton regard.#Bulle:Narrateur
~ Trigger("name")
Dans un wagon plus loin, tu vois au travers de la porte un troupeau qui se fait transporter.#Bulle:Narrateur
Au milieu des moutons, des adolescents plaisantent,#Bulle:Narrateur
affalés contre la laine et gratouillant passivement la tête des animaux.#Bulle:Narrateur
Parfois, l’une des bêtes menace de mâcher leurs vêtements.#Bulle:Narrateur
La victime se lève alors brusquement avec un petit cri, et un rire commun agite tout le groupe.#Bulle:Narrateur
~ Trigger("name")
Les rails veinent la ville,#Bulle:Narrateur
ses quartiers comme une constellation de coeur qui y propulse la vie.#Bulle:Narrateur
Tu aimes bien prendre le train.#Bulle:Narrateur
Peu importe l’endroit où tu te rends,#Bulle:Narrateur
tu te sens à ta place pendant le trajet.#Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE
===observer1===
~ Trigger("name")
Le train vibre de manière irrégulière. #Bulle:Narrateur
Tu ne réussis jamais à identifier la vitesse à laquelle il va.#Bulle:Narrateur
Tu sais, en revanche, qu’elle est générée en grande partie par le poids du véhicule.#Bulle:Narrateur
Les stations de train sont souvent en hauteur, et les chemins entre elles dans des vallées.#Bulle:Narrateur
Ainsi, le train peut se laisser porter par son élan la plupart du temps.#Bulle:Narrateur
Dans ces phases, des dynamos changent l’énergie cynétique en électricité (complétée par des panneaux solaires sur le toit),#Bulle:Narrateur
qui est ensuite utilisée pour compenser quand l’élan ne suffit pas.#Bulle:Narrateur
Un ingénieux assemblage d’accéléromètres#Bulle:Narrateur
couplés aux données topologiques de la ville#Bulle:Narrateur
s’arrangent pour que la vitesse soit toujours constante.#Bulle:Narrateur
La quantité de travail qu’il a fallu pour réaliser cet engin te sidère parfois.#Bulle:Narrateur
Mais pas autant que la complexité du réseau que ce train entretient :#Bulle:Narrateur
des gens prennent quotidiennement ce chemin pour travailler aux quatre coins de la ville.#Bulle:Narrateur
Le poisson et le sel récoltés sur les côtes remontent dans les terres,#Bulle:Narrateur
tandis que le miel des plaines et le fromage des montagnes descendent vers le littoral.#Bulle:Narrateur
Sans le train, il faudrait parfois des jours de marche pour aller aux grandes bibliothèques.#Bulle:Narrateur
Et sans le train, chaque quartier devrait produire tous ses médicaments.#Bulle:Narrateur
Des pannes sur le réseau électrique mettraient plusieurs jours à être trouvées et résolues.#Bulle:Narrateur
La coordination des quartiers est assez semblable à la forme du train en soit.#Bulle:Narrateur
Au fil du temps, chaque quartier a contribué à ajouter et réparer des wagons.#Bulle:Narrateur
Ils sont dépareillés, différents et bigarrés… mais compatibles et complémentaires.#Bulle:Narrateur

Les silouettes floues qui composent ton petit monde défilent une à une à la fenêtre.#Bulle:Narrateur
Tu n’es pas sûr.e de réussir à comprendre tout cet écosystème.#Bulle:Narrateur
Mais tu trouves un confort notable à te dire que tu réussis à y contribuer.#Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE
===observer2===
~ Trigger("name")
Tes yeux sont mi-clos.#Bulle:Narrateur
Ta tête écrasée contre le repose-tête matelassé de ton siège. #Bulle:Narrateur
Le monde a pris fin.#Bulle:Narrateur
Tu es dans les coulisses maintenant.#Bulle:Narrateur
Et tout ce qu’il reste de la scène#Bulle:Narrateur
sont des échos rebondissants sur les rideaux#Bulle:Narrateur
et les taches floues des projecteurs.#Bulle:Narrateur
Tu es en train d’avancer.#Bulle:Narrateur
Vite.#Bulle:Narrateur
Plus vite, plus longtemps et plus régulièrement que n’importe quel humain peut avancer.#Bulle:Narrateur
Et pourtant, tu ne bouges pas.#Bulle:Narrateur
C’est un miracle. Un miracle du quotidien.#Bulle:Narrateur
Quand avons-nous trivialisé les miracles ?#Bulle:Narrateur
Quand est-ce que la prouesse devient un acquis ?#Bulle:Narrateur
N’est-ce pas cette question qui a criblé la civilisation précédente ?#Bulle:Narrateur
Habituée à pouvoir communiquer instantanément avec n’importe qui,#Bulle:Narrateur
n’importe où ?#Bulle:Narrateur
Qui avait accès à une quantité illimitée d’eau et d’électricité ?#Bulle:Narrateur
Qui pouvait manger n’importe quelle nourriture de n’importe quel pays à n’importe quel moment ? #Bulle:Narrateur
Qui lançait des humains dans l’espace ?#Bulle:Narrateur
Est-ce que tout n’est pas simplement en train de recommencer ?#Bulle:Narrateur
Des petites colonies de survivants,#Bulle:Narrateur
éclatées sur un bout de terre,#Bulle:Narrateur
qui se sont battues chaque jour pour ne plus avoir à s’inquiéter#Bulle:Narrateur
quotidiennement de la faim, de la soif, et avoir un minimum de confort.#Bulle:Narrateur
Mais aujourd’hui, ne nous intéressons-nous pas à plus que ça ?#Bulle:Narrateur
Nous avons fait de notre fierté#Bulle:Narrateur
de faire mieux que la civilisation précédente.#Bulle:Narrateur
Mais faisons-nous vraiment mieux ?#Bulle:Narrateur
Est-ce que les trains ne sont pas le premier pas dans cette fiévreuse course vers l’auto-destruction ? #Bulle:Narrateur
Quand demain, nos petits-enfants qui ont toujours connu le train#Bulle:Narrateur
commencerons à le prendre pour acquis, n’iront-ils pas chercher *plus* ?#Bulle:Narrateur

N’est-ce pas dans notre nature de regarder vers l’avant ? #Bulle:Narrateur
Si.#Bulle:Narrateur
Certainement. #Bulle:Narrateur
Mais c’est là l’intérêt de notre libre arbitre. #Bulle:Narrateur
Le pouvoir de changer notre nature.#Bulle:Narrateur
De décider de faire ce qui nous semble bien.#Bulle:Narrateur
Car cela fait aussi partie de notre nature, n’est-ce pas ?#Bulle:Narrateur
De la nature de toute chose.#Bulle:Narrateur
Chercher l’harmonie avec ce qui nous entoure.#Bulle:Narrateur
Et devenir des créatures de modération.#Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE

->END
