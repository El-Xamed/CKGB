->checkID

=== function RetourAuTMAfterRevasser(name)
EXTERNAL RetourAuTMAfterRevasser(name)
VAR IDrevasser = 0
===checkID===
{ 
- IDrevasser==0: ->Revasser0
- IDrevasser==1: ->Revasser1
- IDrevasser==2: ->Revasser2
- else: ->END
}
===Revasser0===

//blabla

Je revasse 0 #Bulle:MorganBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser1===

//blabla

Je revasse 1 #Bulle:MorganBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser2===


//blabla

Je revasse 2 #Bulle:MorganBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE

->END