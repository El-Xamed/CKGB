->checkID

=== function RetourAuTMAfterRespirer(name)
EXTERNAL RetourAuTMAfterRespirer(name)
VAR IDrespirer = 0
===checkID===
{ 
- IDrespirer==0: ->Respirer0
- IDrespirer==1: ->Respirer1
- IDrespirer==2: ->Respirer2
- else: ->END
}
===Respirer0===

//blabla

Je respire 0 #Bulle:NimuHautDroite
~ IDrespirer++
~ RetourAuTMAfterRespirer("")
->DONE
===Respirer1===

//blabla

Je respire 1 #Bulle:NimuHautDroite
~ IDrespirer++
~ RetourAuTMAfterRespirer("")
->DONE
===Respirer2===


//blabla

Je respire 2 #Bulle:NimuHautDroite
~ IDrespirer++
~ RetourAuTMAfterRespirer("")
->DONE

->END