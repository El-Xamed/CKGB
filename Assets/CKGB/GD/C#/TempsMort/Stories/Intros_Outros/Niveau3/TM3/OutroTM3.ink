->Outro
 === function Trigger(name)
EXTERNAL Trigger(name)
=== function StartChallenge(name)
EXTERNAL StartChallenge(name)

===Outro===
Vous sentez le train ralentir. La voix du conducteur passe au travers des haut-parleurs grésillants.#Bulle:Narrateur
Tes oreilles habituées au filtre sonore déchiffrent ses paroles sans peine.#Bulle:Narrateur
Vous arrivez aux Marches.#Bulle:Narrateur

Vous vous pressez sur la banquette pour essayer de distinguer votre destination au travers de la fenêtre du train.#Bulle:Narrateur
~ Trigger("name")
Le quartier vertical composé de petits modules encastrés diagonalement les uns des autres projette une ombre sur toute la station.#Bulle:Narrateur

Esthéla a l'air de remarquer l'expression sur ton visage.
~ Trigger("name")
Ça va Morgan ?<br> T'as peur de la grimpette ?#Bulle:EsthelaHautGauche #emot:?

Je… Non.<br> Je suis juste…<br> <size=40%>nerveux pour <rainb>mon rencard</rainb>.</size=40%>#Bulle:MorganHautGauche 

De quoi ?#Bulle:EsthelaHautGauche

<size=40%>Coubeh.</size=40%>#Bulle:NimuHautDroite

J’ai dit : <size=60%>je suis nerveux pour <rainb>mon rencard</rainb></size=60%>.#Bulle:MorganHautGauche

Aaah ! <size=40%>T’es nerveux pour <rainb>ton rencard</rainb></size=40%>.#Bulle:EsthelaHautGauche
Barf, je suis sûre que ça passera !!<br> T’es le gars le plus <wave>cool</wave> de cette ville.#Bulle:EsthelaHautGauche #emot:Sparkles

Quoi ? Tu t’es regardée dans un miroir récemment ?#Bulle:MorganHautGauche

Ouais !! Et devines ce que j’ai vu ? <br>Ta fan numéro uno.#Bulle:EsthelaHautDroite
Maintenant, fonce.#Bulle:EsthelaHautGauche #emot:Rainbow
Enfin, attends que le train s'arrête quand même.#Bulle:EsthelaHautGauche
Mais après, FONCE.#Bulle:EsthelaHautGauche #emot:JoyRight

Et merci encore de m'avoir laissée <br>venir faire les courses avec toi.#Bulle:NimuHautDroite

...#Bulle:MorganHautGauche #emot:Dots
Tu peux arrêter de faire semblant.#Bulle:MorganHautGauche

Moi ?<br> Faire semblant ?<br> Mais <bounce>ja-mais</bounce> de la vie en fait.#Bulle:NimuHautDroite

Sa voix déborde d’un ironie malicieuse, mais j'avais probablement pas besoin de le préciser.#Bulle:Narrateur

Bon,<br> je suis déjà allé<br> une fois au café où j'ai <rainb>mon rencard</rainb>.#Bulle:MorganHautGauche

Et bien sûr,<br> c'est au rez-de-chaussée <br>et on aura aucun mal à le trouver ?#Bulle:NimuHautDroite

Et c'est genre, au quatrième étage.#Bulle:MorganHautDroite

<wave>Évidemment.</wave>#Bulle:NimuHautDroite

Donc il est temps de faire <br>la seule chose que des gens rationnels<br> peuvent envisager pour se préparer<br> à monter autant d'escaliers.#Bulle:MorganHautDroite
~ Trigger("name")
Installer un ascenseur ?#Bulle:EsthelaHautGauche

<b>S'étirer</b>.#Bulle:MorganHautDroite
Nos corps sont tous extrêmement<br> importants et nous avons l'obligation<br> morale d'en prendre soin<br> par respect pour nous-même.#Bulle:MorganHautDroite

<bounce>Wow</bounce>.#Bulle:EsthelaHautGauche
~ Trigger("name")
~ StartChallenge("S_Challenge")
->END
