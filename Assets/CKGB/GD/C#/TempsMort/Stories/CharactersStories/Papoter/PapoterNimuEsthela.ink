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
NIMU !!!!!#Bulle:EsthelaBasDroite

Ah, tu as entendue ça ?#Bulle:NimuBasGauche

CW : ??#Bulle:EsthelaBasDroite

C’était le bruit de mon coeur qui s’arrête. Tu viens de tuer une pauvre grand-mère.#Bulle:NimuBasGauche

Noooooooon ! Pardon je promets promets je vais être toooute calme et silencieuse à partir de maintenant !!#Bulle:EsthelaBasDroite

Ptdr. Tu tiendra pas cinq minutes.#Bulle:NimuBasGauche

Peut-être éventuellement que je pourrais être motivée par une petite histoire d’avant la singularité ?#Bulle:EsthelaBasDroite

Nimu soupir lourdement. #Bulle:Narrateur

Roh la forceuse. T’en as pas eut assez à l’école ? Tu m’adresse jamais la parole pour autre chose.#Bulle:NimuBasGauche

Mais parce que t’as toujours refusé de m’en donner !!#Bulle:EsthelaBasDroite

Esthélaaa..?#Bulle:NimuBasGauche

Oups. Je voulais dire “mais parce que t’as toujours refusé de m’en donner”. ← en petit#Bulle:EsthelaBasDroite

 Si t’as autant le seum, pourquoi ne pas plutot aller poser tes questions à d’autres zoomers qui ne font rien de leur journée ?#Bulle:NimuBasGauche

Parce que la plupart sont morts. Ou sénile. Ou les deux. Et ce qui restent et bien… je suis mal à l’aise avec les inconnus.#Bulle:EsthelaBasDroite

Je comprends, Esthéla. Mais j’ai bien peur que ce combat t’appartienne. Maintenant, yeet. Morgan à besoin du soutient de ses amis.#Bulle:NimuBasGauche

J’ai aussi besoin de soutient d’une amie. Comment je pourrais soutenarisationner Morgan si j’ai moi même tu m’as pas soutenurisationnarisée alors que tu es mon amie ?#Bulle:EsthelaBasDroite

 Esthéla, je suis profondément touchée. Mais je pense que ce n’est pas que à toi de décider du statu de notre relation.#Bulle:NimuBasGauche

Oh. Oh… Hrm. D’accord. Bah. Je vais soutenir Morgan alors. Navré de t’avoir dérangé.#Bulle:EsthelaBasDroite

Mais l’amité est quelque chose qui peut évoluer avec le temps. Et tu sais quoi ? Je suis plutôt touchée que tu aies essayer de tenir ta parole. Ca a été agréable discuter #Bulle:NimuBasGauche

QUOI ?! YES TROP BIEN !! HAHA, MERCI MERCI !! RAAH TU VAS VOIR ON VA ÊTRE TELLEMENT AMIS ! ET BIENTOT, TOUS LES SECRETS DE L’ANCIEN MONDE SERONT MIEN !#Bulle:EsthelaBasDroite

GM: …#Bulle:NimuBasGauche


//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter1===
Je papote avec toi 1#Bulle:NimuBasGauche
Oui je vois ca #Bulle:EsthelaBasDroite
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter2===
Je papote avec toi 2#Bulle:NimuBasGauche
Oui je vois ca #Bulle:EsthelaBasDroite
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE

->END