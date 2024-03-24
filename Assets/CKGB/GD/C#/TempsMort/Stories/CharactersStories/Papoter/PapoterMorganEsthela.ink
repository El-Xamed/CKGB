-> checkID

=== function RetourAuTMAfterPapotage(name)
EXTERNAL RetourAuTMAfterPapotage(name)
VAR IdPapoter = 0

===checkID===
{
- IdPapoter==1:->Papoter0
- IdPapoter==2:->Papoter1
- IdPapoter==3:->Papoter2
- else: ->END
}
===Papoter0===
Hey, tu pense qu’il faut combien de fourmis pour déplacer un tram ?#Bulle:EsthelaBasDroite

Hhmmmm… 857 143.#Bulle:MorganHautGauche

Mais t’as répondus SUPER VITE ?!#Bulle:EsthelaBasDroite

A toi. Si j’ai un ch’val qui court à 50 hm/h.#Bulle:MorganHautGauche

Q-Quoi mais Morgan-#Bulle:EsthelaBasDroite

Qui fait la route Ruche-Terrier.#Bulle:MorganHautGauche

 C’était pas un concours !!#Bulle:EsthelaBasDroite

 Maintenant si. Et qu’j’ai un merle -qui va moins vite, mais n’a pas à prendre en compte la topographie- qui fait le même chemin.#Bulle:MorganHautGauche

Mais-#Bulle:EsthelaBasDroite

Avec un vent Nord-Ouest de 15 Km/h, qui arriv’rait en premier ?#Bulle:MorganHautGauche

 De quel couleur est le cheval ?#Bulle:EsthelaBasDroite

 Rose. A pois bleus. Et le merle porte un chapeau.#Bulle:MorganHautGauche

Quoi ? Mais il doit s’envoler tout le temps !!#Bulle:EsthelaBasDroite

 Non. Il l’a attaché avec un elastique. #Bulle:MorganHautGauche

…Tu avais aucune idée de la réponse à ma première question en fait ?#Bulle:EsthelaBasDroite

Un sourire en coin tranche les joues de Morgan.#Bulle:Narrateur

Tu m’as toujours posé aucune question sur le fait que je sois sapée en clown en fait.#Bulle:EsthelaBasDroite

Oh. J’aurai abhorré êtr’indiscret.#Bulle:MorganHautGauche

Sois indiscret. Immédiatement. Steuplait steuplait steuplait !!#Bulle:EsthelaBasDroite

Tiens, Esthéla ! J’adore ton nouveau style. C’est quoi tes inspis ?#Bulle:MorganHautGauche

Ma foi, je suis ravis que tu me pose la question spontanément. Laisse moi t’expliquer. #Bulle:EsthelaBasDroite

//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter1===
Je papote avec toi 1#Bulle:MorganHautGauche
Oui je vois ca #Bulle:EsthelaBasDroite
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter2===
Je papote avec toi 2#Bulle:MorganHautGauche
Oui je vois ca #Bulle:EsthelaBasDroite
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE

->END