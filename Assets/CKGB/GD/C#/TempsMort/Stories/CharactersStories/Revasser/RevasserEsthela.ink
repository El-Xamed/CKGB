->checkID

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

Un papillon capte ton attention.#Bulle:EsthelaHautGauche
Tu le suis des yeux.#Bulle:EsthelaHautGauche
Il se plie en volant, et comme une feuille de papier, se fait si fin parfois qu’il échappe à ta vue.#Bulle:EsthelaHautGauche
Puis, il déploie de nouveau ses ailes et réapparait entre deux brises.#Bulle:EsthelaHautGauche
De ce que tes cours t’ont apprit, il y a une centaine d’année, la vaste majorité des espèces animales foulant la Terre s’étaient éteintes. #Bulle:EsthelaHautGauche
Tu essaies de l’imaginer, cette vision que tu poursuis sans cesse, cette humanité folle et lucide dont tout le monde est issu. De vaste rivières de goudron scarifiant le sol jusqu’à l’horizon.#Bulle:EsthelaHautGauche
Des colonnes de sable vitrifiées enchâssées dans d’immenses tours d’eau, de ciment et de gravier. #Bulle:EsthelaHautGauche
Une sorte de ruche à humains, toute en pierre et en métal. Ses magasins débordant de produits transformés, alimentés depuis l’extérieur par des wagons individuels sans rails transportant plusieurs tonnes de marchandise depuis des campagnes sèches,#Bulle:EsthelaHautGauche
drainées de leur nutriment par des champs monochrome et des poisons.#Bulle:EsthelaHautGauche
Tu exagère un peu ton ignorance. Tu sais ce qu’est une voiture. #Bulle:EsthelaHautGauche
Il y a encore des engins à moteurs à explosion dans Brekelantte.#Bulle:EsthelaHautGauche
Tu as juste du mal à imaginer comment est-ce qu’il y a pu en avoir tellement, et comment est-ce que cette immense réseau interagissait. #Bulle:EsthelaHautGauche
Tu sais qu’il y avait une espèce de code des conducteurs, un peu comme les pirates. #Bulle:EsthelaHautGauche
C’est bizarre que les gens soient capables de se faire confiance les uns les autres pour conduire cote à cote des machines allant à plusieurs kilomètres à l’heure, et pourtant témoignait d’autant de méfiance envers les inconnus dans la vie de tous les jours.#Bulle:EsthelaHautGauche
Qu’est-ce qui créer ce contraste entre l’extérieur et l’intérieur de la voiture ?#Bulle:EsthelaHautGauche
Est-ce que les voitures rendaient magiquement les gens polies et courtois ? #Bulle:EsthelaHautGauche

Peut-être que la civilisation humaine aurait bien plus vite changé si tout le monde avait passé son temps dans la sécurité d’un habitacle de métal.#Bulle:EsthelaHautGauche
Mais peut-être qu’à ce moment là il y aurait eut des problèmes de carburant ?#Bulle:EsthelaHautGauche
Tu as vraiment du mal te représenter tout ça. #Bulle:EsthelaHautGauche
La vie à cette époque à l’air définitivement bien compliquée. #Bulle:EsthelaHautGauche
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser1===

//blabla

Je revasse 1 #Bulle:EsthelaBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser2===


//blabla

Je revasse 2 #Bulle:EsthelaBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE

->END