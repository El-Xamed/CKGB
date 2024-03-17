->checkID

=== function RetourAuTMAfterRevasser(name)
EXTERNAL RetourAuTMAfterRevasser(name)
VAR IDrevasser = 0
===checkID===
{ 
- IDrevasser==1: ->Revasser0
- IDrevasser==2: ->Revasser1
- IDrevasser==3: ->Revasser2
- else: ->END
}
===Revasser0===

//blabla

Je revasse 0 #Bulle:NimuBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser1===

//blabla

Je revasse 1 #Bulle:NimuBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser2===


//blabla

Je revasse 2 #Bulle:NimuBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE

->END