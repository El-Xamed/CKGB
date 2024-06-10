-> checkID
=== function Trigger(name)
EXTERNAL Trigger(name)
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
~ Trigger("name")
Hey, tu penses qu’il faut<br> combien de fourmis pour déplacer un train ?#Bulle:EsthelaBasDroite

5 714 285.#Bulle:MorganHautGauche

Mais t’as répondu <shake>SUPER VITE</shake> ?!#Bulle:EsthelaBasDroite #emot:!

À toi.<br> Si un cheval court à 50 hm/h...#Bulle:MorganHautGauche

~ Trigger("name")

Q-Quoi mais Morgan-#Bulle:EsthelaBasDroite

...Prend la route Ruche-Terrier...#Bulle:MorganHautGauche

~ Trigger("name")

C’était pas un concours !!#Bulle:EsthelaBasDroite #emot:Drop

<i>Maintenant si.</i><br> Et qu’j’ai un merle, <br>qui va moins vite <br> mais n’a pas à prendre en compte la topographie<br>, qui fait le même chemin.#Bulle:MorganHautGauche

<wiggle>Mais-</wiggle>#Bulle:EsthelaBasDroite #emot:Deception

Avec un vent Nord-Ouest de 15 Km/h,<br> qui arriv’rait en premier ?#Bulle:MorganHautGauche

...#Bulle:ESthelaHautDroit #emot:Dots
De quelle couleur est le cheval ?#Bulle:EsthelaBasDroite

<i>Rose</i>.<br>À pois bleus.<br> Et le merle porte un chapeau.#Bulle:MorganHautGauche

~ Trigger("name")

Quoi ? Mais il doit s’envoler tout le temps !!#Bulle:EsthelaBasDroite #emot:!

Non. Il l’a attaché avec un élastique.#Bulle:MorganHautGauche

…T'as juste répondu <bounce><i>n'importe quoi</i></bounce> <br>à ma première question en fait, c'est ça ?#Bulle:EsthelaBasDroite

Un sourire en coin fend les joues de Morgan.#Bulle:Narrateur #emot:JoyRight

Tu m’as toujours posé<br> <incr>aucune question</incr> sur le<br> fait que je sois <br>sapée en clown, au fait.#Bulle:EsthelaBasDroite

Oh...#Bulle:MorganHautGauche #emot:Dots
J'avais peur d'être indiscret.#Bulle:MorganHautGauche
Mais c'est vrai que ça a éveillé ma curiosité.#Bulle:MorganHautGauche
Ça t'va vraiment bien, pour être honnête.#Bulle:MorganHautGauche 

Pff, <wave>charmeur</wave>. #Bulle:EsthelaHautDroite #emot:JoyLeft
Sois indiscret !! <br> Immédiatement !! #Bulle:EsthelaHautDroite
J'ai trop trop trop envie d'en parler depuis tout à l'heure !!!!!#Bulle:EsthelaHautDroite 

Tiens, Esthéla !<br> Quelle surprise ! <br>J’adore ton nouveau style.<br> C’est quoi tes inspis ?#Bulle:MorganHautGauche #emot:Sparkles

    <bounce<MORGAN, INCROYABLE, QUELLE COÏNCIDENCE,</bounce> je suis RA-VIE<br> que tu me poses la question <wave>SPONTANÉMENT</wave> !! #Bulle:EsthelaHautGauche

<wave>Laisse-moi t’expliquer en détail deux ou trois fois !!!</wave> #Bulle:EsthelaBasDroite #emot:Rainbow

Ainsi soit-il.#Bulle:Morgan

//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter1===
<shake>MORGAN !!</shake> #Bulle:EsthelaHautDroite
Est-ce que <br>tu me caches <i>quelque chose</i> ???#Bulle:EsthelaHautDroite

...#Bulle:MorganHautDroite #emot:Dots
'ffectivement, tu m'as percé à jour...#Bulle:MorganHautGauche emot:Deception
Mon caleçon est <wave>bleu</wave>.#Bulle:MorganHautGauche

<shake>C’est pas ce que j’avais en tête</shake> !!#Bulle:EsthelaHautDroite #emot:!

Et bien maintenant, si.#Bulle:MorganHautGauche

<incr>MAIS STOOOOP !</incr>#Bulle:EsthelaHautDroite #emot:!
Pour de vrai. #BulleEsthelaHautGauche
Esthéla prend son visage (assez rare) des discussions sérieuses.#Bulle:Narrateur

J'aimerais vraiment avoir ton attention.#Bulle:EsthelaHautGauche
Je veux pas dire <i>“c’était mieux avant”</>.<br> Mais je vois très bien <br>que je suis pas la seule <br>qui a changé ces dernières semaines.#Bulle:EsthelaHautDroite

H-Hm ?#Bulle:MorganHautGauche

Elle attrape tes lunettes d'un geste sec. Les branches te fouettent le nez au passage.#Bulle:Narrateur
//Le sprite smol d’Esthela bump le sprite smol de Morgan. (Retirer les lunettes de Morgan si possible).

<incr>Hey !!</incr>#Bulle:MorganHautGauche emot:!

Le Morgan que je connaissais <br>était moins… <i>Placide</i> ?#Bulle:EsthelaHautDroite

J’aimais bien ça.#Bulle:EsthelaHautDroite

<wiggle>Rends</wiggle> !#Bulle:MorganHautGauche emot:anger

<wave>Wooohohoo</wave> mon frère, tes ceeernes c’est un truc de <wave>diiiingue</wave> !#Bulle:EsthelaHautDroite emot:JoyRight

//Le sprite de Morgan bump celui d’Esthéla et récupère ses lunettes.

Tu récupères tes lunettes des mains de la créature pour les remettre sur l'arête de ton nez.#Bulle:Narrateur

J’ai des soucis d’insomnie en c'moment.#Bulle:MorganHautGauche 

Est-ce queeee...<!>Tu veux m’en parler ?#Bulle:EsthelaHautDroite

Change pas de sujet. #Bulle:MorganHautGauche
J’ai pas fait d’scène<br> parce que t’es arrivée en créature de cirque.#Bulle:MorganHautGauche
J’pensais pouvoir m’attendre à ce que tu acceptes aussi comment j'm’exprime maintenant.#Bulle:MorganHautGauche

Hey, je te dois <b>rien</b><br> juste parce que tu agis<br> comme une personne décente.#Bulle:EsthelaHautDroite
Et j’ai pour devoir de rester honnête. <br>
Je suis pas contre ton nouveau style.<br>
Juste… Curieuse.#Bulle:EsthelaHautDroite

Pff, c’est quand ça t’arrange, hein ?#Bulle:MorganHautGauche

Non mais vraiement !<br> Est-ce que tu préférerais que j'me taise ?<br>Je vois bien que tu te fais du mal !#Bulle:EsthelaHautDroite emot:!
Est-ce que je dois faire comme<br> si de rien était<br> alors tu passses clairement la moitié<br> de ton temps à ruminer ?#Bulle:EsthelaHautDroite #emot:anger
Tu agis toujours<br> comme le Morgan que j’admire.
Mais t’as l’air de plus<br> te prendre la tête en le faisant.#Bulle:EsthelaHautDroite

Hm. Est-ce que tu veux qu’moi<br>aussi j’sois honnête avec toi ?#Bulle:MorganHautGauche

Oui ! #Bulle:EsthelaHautDroite

Un train fait environ 400 tonnes, soit 40 000 000 grammes. <br>La fourmie Acalanthe peut porter 7 grammes. <br>C’est une division assez simple.<br> Il faut 5 714 285 fourmis.#Bulle:MorganHautGauche

Je sais pas quoi <br>répondre à ça Morgan <br>tu me fais juste peur.#Bulle:EsthelaHautDroite 
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter2===
Hmm. Ce papotage ne devrait pas être accessible. Comment es-tu arrivé là ?#Bulle:Narrateur
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE

->END
