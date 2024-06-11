->checkID
=== function Trigger(name)
EXTERNAL Trigger(name)
=== function RetourAuTMAfterRevasser(name)
EXTERNAL RetourAuTMAfterRevasser(name)
VAR IDrevasser = 0
===checkID===
{ 
- IDrevasser==1: ->Revasser0
- IDrevasser==2: ->Revasser1
- IDrevasser==3: ->Revasser2
- else: ->END
}
===Revasser0===

//blabla

Un papillon capte ton attention.#Bulle:Narrateur
Tu le suis des yeux.#Bulle:Narrateur
Il se plie en volant, et comme une feuille de papier, se fait parfois si fin qu’il échappe à ta vue.#Bulle:Narrateur
Puis, il déploie de nouveau ses ailes et réapparaît entre deux brises.#Bulle:Narrateur
De ce que tes cours t’ont appris, il y a une centaine d’années, la vaste majorité des espèces animales foulant la Terre s’étaient éteintes. #Bulle:Narrateur
Tu essaies de l’imaginer, cette vision que tu poursuis sans cesse, cette humanité folle et lucide dont tout le monde est issu. De vastes rivières de goudron scarifiant le sol jusqu’à l’horizon.#Bulle:Narrateur
Des colonnes de sable vitrifiées enchâssées dans d’immenses tours d’eau, de ciment et de gravier. #Bulle:Narrateur
Une sorte de ruche à humains, toute en pierre et en métal. Ses magasins débordant de produits transformés, alimentés depuis l’extérieur par des wagons individuels sans rails transportant plusieurs tonnes de marchandises depuis des campagnes sèches,#Bulle:Narrateur
drainées de leurs nutriments par des champs monochromes et des poisons.#Bulle:Narrateur
Tu exagères un peu ton ignorance. Tu sais ce qu’est une voiture. #Bulle:Narrateur
Il y a encore des engins à moteur à explosion dans Brekelantte.#Bulle:Narrateur
Tu as juste du mal à imaginer comment est-ce qu’il y a pu en avoir tellement, et comment est-ce que cet immense réseau interagissait. #Bulle:Narrateur
Tu sais qu’il y avait une espèce de code des conducteurs, un peu comme les pirates.#Bulle:Narrateur
C’est bizarre que les gens soient capables de se faire confiance les uns les autres pour conduire côte à côte des machines allant à plusieurs dizaines de kilomètres à l’heure, et pourtant témoignaient d’autant de méfiance envers les inconnus dans la vie de tous les jours.#Bulle:Narrateur
Qu’est-ce qui crée ce contraste entre l’extérieur et l’intérieur de la voiture ?#Bulle:Narrateur
Est-ce que les voitures rendaient magiquement les gens responsables et courtois ? #Bulle:Narrateur

Peut-être que la civilisation humaine aurait bien plus vite changé si tout le monde avait passé son temps dans la sécurité d’un habitacle de métal.#Bulle:Narrateur
Mais peut-être qu’à ce moment là il y aurait eu des problèmes de carburant ?#Bulle:Narrateur
Tu as vraiment du mal te représenter tout ça.#Bulle:Narrateur
La vie à cette époque a l’air définitivement bien compliquée.#Bulle:Narrateur
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser1===

//blabla
Les mains sous ta cape, tu caresses instinctivement une des balles de jonglage dans la poche de ta robe...#Bulle:Narrateur
Les gens sont toujours surpris quand ils apprennent que tu les portes tout le temps sur toi.#Bulle:Narrateur
À quoi est-ce qu’ils s’attendent ?#Bulle:Narrateur
Tu en sors une, et commences à la lancer et à la rattraper d’une main. Ça te fait te sentir bien.#Bulle:Narrateur
Confiante.#Bulle:Narrateur
Tu repenses à ces photos imprimées par le Musée.#Bulle:Narrateur
Ces gens adossés à un mur, l’oeil assuré et une cigarette à la bouche. C’est un peu pareil.#Bulle:Narrateur
Comme quand Morgan tient son stylo en parlant.#Bulle:Narrateur
Ça confère une contenance, une attitude.#Bulle:Narrateur
Des gens fument à Brekelantte. Du blémi, du chançoit, et même du tabac. Mais pas comme ils le faisaient à l’époque.#Bulle:Narrateur
Ça se sent, dans la manière dont les plus vieilles personnes en parlent.#Bulle:Narrateur
Tu ne comprends pas ce qui peut être si différent. Mais aussi, tu n’as jamais vu de cigarettes comme sur les photos, à Brekelantte. #Bulle:Narrateur
Tu fais tomber la balle remplie de graines.#Bulle:Narrateur
Oups.#Bulle:Narrateur
Tu la ramasses. Ton pouce poursuit la ligne tracée par la couture.#Bulle:Narrateur
Tu ne diras jamais assez merci à tes amis du cirque pour te l’avoir offerte.#Bulle:Narrateur
Pour t’avoir initiée à leur monde. Pour t’avoir faite l’une des leurs.#Bulle:Narrateur
Tu ne t’es jamais sentie différente là-bas. Pourtant, tu ne les connaissais pas il y a un mois.#Bulle:Narrateur
Ça avait été comme si… Comme si tu avais toujours été un poisson, et qu’on t’avait jeté dans l’eau pour la première fois.#Bulle:Narrateur
Tu n’étais plus seule, et pourtant tu n’as jamais été aussi libre...#Bulle:Narrateur
Tu sors deux autres balles...#Bulle:Narrateur
Ce n’est pas vraiment possible de jouer à deux balles. On peut le faire, plus ou moins,#Bulle:Narrateur
mais c’est bizarre, car tout le concept du jonglage repose sur le fait de ne pas avoir assez de mains mais de faire avec.#Bulle:Narrateur

Tu lances une balle.#Bulle:Narrateur
Le destin tient à si peu de choses. Elle retombe...#Bulle:Narrateur
Tu en lances une autre pour pouvoir l’accueillir. On est tous lancés dans des directions différentes. La deuxieme balle retombe.#Bulle:Narrateur
Tu dois en lancer une nouvelle pour avoir la place de la ramasser.#Bulle:Narrateur
Et il suffit d’un rien pour qu’on croise celle d’une autre personne. Elle entame sa cloche, et tu relances celle de l’autre côté.#Bulle:Narrateur
Elles se carambolent dans les airs.  Et que notre vie entière change de trajectoire.#Bulle:Narrateur
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser2===


//blabla

J'ai que deux temps morts à moi, comment vous êtes arrivés là ?? #Bulle:EsthelaBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE

->END
