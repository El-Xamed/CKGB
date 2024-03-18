-> checkID

=== function RetourAuTMAfterPapotage(name)
EXTERNAL RetourAuTMAfterPapotage(name)
VAR IdPapoter = 0

===checkID===
{
- IdPapoter==1:->Papoter0
- IdPapoter==2:->Papoter1
- IdPapoter==3:->Papoter2
- else:
~ RetourAuTMAfterPapotage("name") 
->END
}
===Papoter0===
 …#Bulle:NimuBasDroite
Morgan se ronge visiblement les ongles #Bulle:Narrateur
Nimu, elle commence à fredonner Despacito #Bulle:Narrateur
 … #Bulle:MorganHautGauche
Alors, nerveux ?#Bulle:NimuBasDroite
Hm ?…‘Nah. #Bulle:MorganHautGauche
Bah tu vas dead ça alors.#Bulle:NimuBasDroite
 … #Bulle:MorganHautGauche
Je suis vraiment fière que de toi, tu sais ? 
A-ah oui ? #Bulle:MorganHautGauche
 Mais ouai, ça demande beaucoup de courage d’être maitre de soi-même comme tu l’es.#Bulle:NimuBasDroite
… #Bulle:MorganHautGauche
Ca me rappelle mon premier rendez-vous amoureux. Haha, comment c’était claquée au sol. J’ai mit vlà longtemps avant de trouver des techniques pour m’apaiser. Tu m’impressione.#Bulle:NimuBasDroite

joue nerveusement avec la fermeture éclaire de ses poches #Bulle:MorganHautGauche

Elle reprend despacito #Bulle:Narrateur

… #Bulle:MorganHautGauche 

En tout cas, sache que je suis de tout coeur avec toi.#Bulle:NimuBasDroite

‘oui ? #Bulle:MorganHautGauche

Si jamais, à n’importe quel moment, tu sens que tu as besoin de soutient, tu as juste à me demander.#Bulle:NimuBasDroite

-hm. #Bulle:MorganHautGauche

Nimu commence à chantonner#Bulle:Narrateur
ark tutudulutu..Baby sh-#Bulle:NimuBasDroite

P’tetre. #Bulle:MorganHautGauche

Hm ?#Bulle:NimuBasDroite

Peut-être que j’aprécierai beneficier de quelques conseils avisés. #Bulle:MorganHautGauche

Nimu a un petit sourire joueur alors qu’elle répond à tes questions. 0 #Bulle:MorganHautGauche
Oui je vois ca #Bulle:NimuBasDroite
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter1===
Je papote avec toi 1#Bulle:MorganHautGauche
Oui je vois ca #Bulle:NimuBasDroite
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter2===
Je papote avec toi 2#Bulle:MorganHautGauche
Oui je vois ca #Bulle:NimuBasDroite
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE

->END