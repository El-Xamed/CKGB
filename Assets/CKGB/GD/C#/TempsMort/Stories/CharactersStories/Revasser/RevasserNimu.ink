->checkID
=== function StartTM(name)
EXTERNAL StartTM(name)
=== function Trigger(name)
EXTERNAL Trigger(name)
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
Tu prends un moment pour fermer les yeux.#Bulle:Narrateur
C’est quelque chose que tu fais de plus en plus souvent. #Bulle:Narrateur
Tu ne ressens plus vraiment le besoin de bouger. #Bulle:Narrateur
Est-ce que c’est un signe ?#Bulle:Narrateur
Être là où tu es te suffit.#Bulle:Narrateur
Depuis plusieurs années tu te sens… accomplie.#Bulle:Narrateur
Fière. 

<i>(Bon, mon ptit gars a pas<br> toujours les idées bien alignées,)</i>#Bulle:NimuHautGauche
<i>(mais il est sur la bonne voie.)</i>#Bulle:NimuHautGauche
<i>(Ses adelphes et <br>ses cousins sont pas pires.)</i>#Bulle:NimuHautGauche

Tu as vraiment fait du bon travail.#Bulle:Narrateur

<i>(Enfin, pas que j'ai grand chose<br> à voir avec le fait qu’ils<br> soient géniaux.)</i>#Bulle:NimuHautDroite
<i>(Ils se débrouillent très bien<br> tous seuls pour ça.)</i>#Bulle:NimuHautDroite
À chaque fois que tu penses à eux, il y a une image qui te revient.#Bulle:Narrateur
Le visage de leur mère et de ses frères quand tu les as vus la première fois.#Bulle:Narrateur
Tu n’as pas choisi de devenir leur mère.#Bulle:Narrateur
Il y avait juste des enfants avec des yeux grands comme ceux d’un matou.#Bulle:Narrateur
Et comme ta détermination,#Bulle:Narrateur
alors que tu t’es jurée qu’ils ne grandiraient pas orphelins.#Bulle:Narrateur

<i>(Ouais.)</i>#Bulle:NimuHautDroite
<i>(J'ai vraiment fait<br> du bon travail.)</i> #Bulle:NimuHautDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser1===

//blabla

Tu t’écartes un peu des autres pour t’asseoir dans un coin.#Bulle:Narrateur
Bonne bouffe, prothèse, médocs, on peut faire ce qu’on veut, on peut pas éternellement lutter contre son âge.#Bulle:Narrateur
La plupart du temps, tu te sens totalement lucide et en pleine possession de tes moyens.#Bulle:Narrateur
Mais parfois, les douleurs qui constellent ton corps se font lancinantes, et tu sens que tu perds un peu pied.#Bulle:Narrateur
Sans doute le soleil.<br> Qu’est-ce que ça tape aujourd’hui.#Bulle:NimuHautDroite
Satanée couche d’ozone.<br> Chiffe molle.#Bulle:NimuHautDroite
Combien de temps est-ce qu’elle <br>va prendre à se remettre sur pied ?#Bulle:NimuHautDroite
Ça fait des années que tu t’es rétablie, toi.#Bulle:NimuHautDroite
Elle pense vraiment qu’elle<br> peut rester en morceaux, à se reposer à jamais ? #Bulle:NimuHautDroite
C’est pas une p'tite apocalypse<br> qui va la mettre au tapis, quand même. #Bulle:NimuHautDroite
Tu soupires en te massant l’arête du nez. #Bulle:Narrateur
Des fois, tu te demandes où est le vrai courage.#Bulle:Narrateur
Toujours persévérer est-il vraiment la solution ? #Bulle:Narrateur
Ce n’est pas que tu avais voulu te relever quand la Singularité t’avais mis au plus bas,#Bulle:Narrateur
quand vous étiez en proie aux famines,
ou encore avant, pendant la guerre civile.#Bulle:Narrateur
C’est ton instinct de survie qui ne t’a pas laissé le choix.
Il t’a dit “tu marcheras, ma grande”,#Bulle:Narrateur
et comme une conne t’as marché.#Bulle:Narrateur
Tu es fière de tout ce que tu as construit.#Bulle:Narrateur
Mais, avec du recul,#Bulle:Narrateur
tu vois comment tous ces efforts t’ont brisée en échange.#Bulle:Narrateur
Est-ce que tout cela en valait vraiment la peine ? #Bulle:Narrateur
Tu entends un vombrissement ténu, là, du fond de ton bras cybernétique. #Bulle:Narrateur

Peut-être que tu devrais enfin faire preuve du véritable courage.#Bulle:Narrateur
Peut-être que la couche d’ozone a raison. #Bulle:NimuBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser2===


//blabla

C’est quand même une sacrée matinée.
On a bien galopé.<br> C’est que c’est sportif de suivre ce gaillard.  #Bulle:NimuHautGauche
Qu’est-ce qu’il aurait fait si t’avais pas été là ?  #Bulle:NimuHautGauche
Pfff. De qui tu te moques ? #Bulle:Narrateur
Tu serres le poing.#Bulle:Narrateur
Tu ne t’étais pas amusée comme ça depuis des mois.#Bulle:Narrateur
Oh, ce n’était pas de tout repos. #Bulle:Narrateur
Mais c’est aussi agréable d’être épuisée. #Bulle:Narrateur
Vraiment épuisée.#Bulle:Narrateur
Pas léthargique, comme tu te sens tout le temps en restant à l’intérieur. #Bulle:Narrateur
Tu as passé ta vie à porter le monde sur tes épaules. #Bulle:Narrateur
À prier pour pouvoir passer des jours tranquilles. #Bulle:Narrateur
Et maintenant… Quoi ? #Bulle:Narrateur
Tu te rends compte que cette tranquilité n’a aucune valeur ?#Bulle:Narrateur
Non. #Bulle:Narrateur
Non, pas exactement.  #Bulle:Narrateur
Les responsabilités sont loin de te manquer... #Bulle:Narrateur
Devoir choisir qui aurait des médicaments pendant les pénuries, 
ou est-ce qu’il vaudrait mieux déménager la colonie #Bulle:Narrateur
à la recherche d’un point d’eau quand il n’avait pas plu pendant plusieurs mois.
Tu es bien contente de ne plus avoir à te soucier de tout ça.#Bulle:Narrateur
Mais, la vie à la maison, réparer ton vélo,#Bulle:Narrateur
te balader en forêt, aider tes petits-enfants…  #Bulle:Narrateur
Ça,#Bulle:Narrateur
c’est une charge que tu peux encore porter.#Bulle:Narrateur
Tout ce temps, tu t’es comportée comme si tu n’étais déjà plus là. #Bulle:Narrateur
Mais tu ne peux pas décréter qu’on n'a plus besoin de toi. #Bulle:Narrateur

Même si ça s’avère vrai, #Bulle:Narrateur
tant qu’on t’appelle à l’aide, tu veux répondre. #Bulle:Narrateur
C’est ça, les valeurs les plus importantes pour toi. #Bulle:Narrateur
Prendre soin de ce qui est autour de toi. Même si tu vieillis… #Bulle:Narrateur
C’est comme ça que tu veux vivre tes derniers jours. #Bulle:Narrateur
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE

->END
