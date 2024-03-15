-> checkID

=== function RetourAuTMAfterPapotage(name)
EXTERNAL RetourAuTMAfterPapotage(name)
VAR IdPapoter = 0

===checkID===
{
- IdPapoter==0:->Papoter0
- IdPapoter==1:->Papoter1
- IdPapoter==2:->Papoter2
}
===Papoter0===
Je papote avec toi 0 #Bulle:MorganHautGauche
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