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

Sois indiscret. Immédiatement. Steuplait steuplait steuplait !!#Bulle:EsthelaHautDroite

Tiens, Esthéla ! J’adore ton nouveau style. C’est quoi tes inspis ?#Bulle:MorganHautGauche

Ma foi, je suis ravis que tu me pose la question spontanément. Laisse moi t’expliquer. #Bulle:EsthelaBasDroite

//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter1===
"Tu me mens par omission."

"Mon caleçon est bleu."#Bulle:MorganHautGauche

"C’est pas ce que j’avais en tête !!"#Bulle:EsthelaHautDroite

"Et bien maintenant, si."#Bulle:MorganHautGauche

"MAIS STOP !"#Bulle:EsthelaHautDroite
Elle prend sa tête (assez rare) des discussions sérieuse.#Bulle:Narrateur

"Je veux pas dire “c’était mieux avant”. Mais je vois très bien que je suis pas la seule qui a changée ces dernières semaines."#Bulle:EsthelaHautDroite

"J… J’essaies un nouveau truc."#Bulle:MorganHautGauche

Elle attrape tes lunettes d'un geste SECRETS Les branches te fouettes le nez au passage.#Bulle:Narrateur
//Le sprite smol d’Esthela bump le sprite smol de Morgan. (Retirer les lunettes de Morgan si possible).

"Hey !!"#Bulle:MorganHautGauche

"Le Morgan que je connaissais était moins… Placide. J’aimais bien ça.'#Bulle:EsthelaHautDroite"

" Rends !"#Bulle:MorganHautGauche

" Woooh mon frère tes ceeernes c’est dingue !"#Bulle:EsthelaHautDroite

Le sprite de Morgan bump celui d’Esthéla et récupère ses lunettes.

"J’ai des soucis d’insomnie."

"Tu veux m’en parler ?"#Bulle:EsthelaHautDroite

"Change pas de sujet. J’ai pas fait d’scène parc’que t’es arrivé en créature de cirque."#Bulle:MorganHautGauche
"J’pensais pouvoir m’attendre à ce que tu accepte aussi comment je m’exprime maintenant."#Bulle:MorganHautGauche

" Hey, je te dois *rien* juste parce que tu agis comme une personne décente. Et j’ai pour devoir de rester honnête. Je suis pas **forcément** contre ton nouveau style. Juste… Curieuse."#Bulle:EsthelaHautDroite

CK : Pff, c’est quand ça t’arrange, hein ?

"Non mais vraiement ! Est-ce que je devrai juste me taire alors que je vois que tu te fais du mal ?"#Bulle:EsthelaHautDroite
"Est-ce que je faire comme si de rien était alors tu passe clairement la moitié de ton temps à ruminer ?"#Bulle:EsthelaHautDroite
"Tu agis toujours comme le Morgan que j’admire. Mais t’as l’air de plus te prendre la tête en le faisant."#Bulle:EsthelaHautDroite

" Hm. Est-ce que tu veux qu’moi aussi j’sois honnête avec toi ?"#Bulle:MorganHautGauche

"OUI !!"#Bulle:EsthelaHautDroite

"Un tram fait environ 60 tonnes, soit 6 000 000 grammes. La fourmie Acalanthe peut porter 7 grammes. C’est une division assez simple. Il faut 857 143 fourmis."#Bulle:MorganHautGauche

"Je sais pas quoi répondre à ça Morgan tu me fais juste peur."#Bulle:EsthelaHautDroite
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter2===
Hmm. Ce papotage ne devrai pas être accessible. Comment es-tu arrivé là ?#Bulle:Narrateur
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE

->END