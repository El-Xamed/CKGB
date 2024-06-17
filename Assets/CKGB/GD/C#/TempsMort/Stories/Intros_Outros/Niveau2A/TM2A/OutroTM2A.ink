->Outro
 === function Trigger(name)
EXTERNAL Trigger(name)
=== function StartChallenge(name)

EXTERNAL StartChallenge(name)

===Outro===

Ta grand-mère bâille à s'en décrocher la mâchoire.#Bulle:Narrateur
~ Trigger("name")
Bon !<br> Ça va êt' l'heure d'y aller la team.#Bulle:NimuHautGauche #emot:!
~ Trigger("name")
Tenez-vous bien <br>la main deux par deeeux !#Bulle:NimuHautGauche

Oui madaaaame !#Bulle:EsthelaHautGauche
~ Trigger("name")
C’est bon je crois <br>qu’on l’a définitivement perdue,<br> elle commence à dire n'importe quoi.#Bulle:MorganHautGauche

Hé !<br>
Mais tu m’as <br>quand même pris la main !#Bulle:EsthelaHautGauche

Ouais. Et ?#Bulle:MorganHautGauche
~ Trigger("name")
Ha !<br> Tu seras crevé avant<br> que j’devienne sénile mon gaillard.#Bulle:NimuHautGauche

Esthéla se penche vers toi pour te murmurer quelque chose.#Bulle:Narrateur

Morgan,<br> est-ce que ta grand-mère vient <br>de nous menacer de mort ?#Bulle:EsthelaHautGauche #emot:?
~ Trigger("name")
Yep,#Bulle:MorganHautGauche
Tu hausses les épaules.#Bulle:Narrateur
elle fait ça des fois.#Bulle:MorganHautGauche
~ StartChallenge("S_Challenge")
->END
