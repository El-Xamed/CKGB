->Outro
=== function StartChallenge(name)

EXTERNAL StartChallenge(name)

===Outro===

Ta grand-mère baille à s'en décrocher la machoire.#Bulle:Narrateur
~ Trigger("name")
Bon ! 'va être l'heure d'y aller la team.#Bulle:NimuHautGauche #emot:!
~ Trigger("name")
Tenez vous bien la main deux par deeeux !#Bulle:NimuHautGauche

Oui madaaaame !#Bulle:EsthelaHautGauche
~ Trigger("name")
C’est bon je crois qu’on l’a définitement perdu, elle commence à dire n'importe quoi.#Bulle:MorganHautGauche

Tu m’as quand même pris la main !#Bulle:EsthelaHautGauche

Ouai, deal with it.#Bulle:MorganHautGauche
~ Trigger("name")
Ha! Tu seras crevé avant que j’devienne sénile mon gaillard.#Bulle:NimuHautGauche

Esthela se penche vers toi pour te murmurer quelque chose.Bulle:Narrateur

Morgan, est-ce que ta grand-mère viens de nous menacer de mort ?#Bulle:EsthelaHautGauche #emot:?
~ Trigger("name")
Yeah,#Bulle:MorganHautGauche
Tu hausse les épaules.#Bulle:Narrateur
elle fait ça des fois.#Bulle:MorganHautGauche
~ StartChallenge("S_Challenge")
->END
