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

<i>(Bon, mon ptit gars a pas toujours les idées bien alignées,)</i>#Bulle:NimuHautGauche
<i>(mais il est sur la bonne voie.)</i>#Bulle:NimuHautGauche
<i>(Ses adelphes et ses cousins sont pas pires.)</i>#Bulle:NimuHautGauche

Tu as vraiment fait du bon travail.#Bulle:Narrateur

<i>(Enfin, pas que j'ai grand chose à voir avec le fait qu’ils soient géniaux.)</i>#Bulle:NimuHautDroite
<i>(Ils se débrouillent très bien tous seuls pour ça.)</i>#Bulle:NimuHautDroite
À chaque fois que tu penses à eux, il y a une image qui te revient.#Bulle:Narrateur
Le visage de leur mère et de ses frères quand tu les as vus la première fois.#Bulle:Narrateur
Tu n’as pas choisi de devenir leur mère.#Bulle:Narrateur
Il y avait juste des enfants avec des yeux grands comme ceux d’un matou.#Bulle:Narrateur
Et comme ta détermination,#Bulle:NimuHautDroite
alors que tu t’es jurée qu’ils ne grandiraient pas orphelins.#Bulle:NimuHautDroite

<i>(Ouais.)</i>#Bulle:NimuHautDroite
<i>(J'ai vraiment fait du bon travail.)</i> #Bulle:NimuHautDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser1===

//blabla

Tu t’écartes un peu des autres pour t’asseoir dans un coin.#Bulle:NimuHautDroite
Bonne bouffe, prothèse, médocs, on peut faire ce qu’on veut, on peut pas éternellement lutter contre son âge.#Bulle:NimuHautDroite
La plupart du temps, tu te sens totalement lucide et en pleine possession de tes moyens.#Bulle:NimuHautDroite
Mais parfois, les douleurs qui constellent ton corps se font lancinantes, et tu sens que tu perds un peu pied.#Bulle:NimuHautDroite
Sans doute le soleil. Qu’est-ce que ça tape aujourd’hui.#Bulle:NimuHautDroite
Satanée couche d’ozone. Chiffe molle.#Bulle:NimuHautDroite
Combien de temps est-ce qu’elle va prendre à se remettre sur pied ?#Bulle:NimuHautDroite
Ça fait des années que tu t’es rétablie, toi.#Bulle:NimuHautDroite
Elle pense vraiment qu’elle peut rester en morceaux, à se reposer à jamais ? #Bulle:NimuHautDroite
C’est pas une p'tite apocalypse qui va la mettre au tapis, quand même. #Bulle:NimuHautDroite
Tu soupires en te massant l’arête du nez. #Bulle:NimuHautDroite
Des fois, tu te demandes où est le vrai courage.#Bulle:NimuHautDroite
Toujours persévérer est-il vraiment la solution ? #Bulle:NimuHautDroite
Ce n’est pas que tu avais voulu te relever quand la Singularité t’avais mis au plus bas,#Bulle:NimuHautDroite
quand vous étiez en proie aux famines,
ou encore avant, pendant la guerre civile.#Bulle:NimuHautDroite
C’est ton instinct de survie qui ne t’a pas laissé le choix.
Il t’a dit “tu marcheras, ma grande”,#Bulle:NimuHautDroite
et comme une conne t’as marché.#Bulle:NimuHautDroite
Tu es fière de tout ce que tu as construit.#Bulle:NimuHautDroite
Mais, avec du recul,#Bulle:NimuHautDroite
tu vois comment tous ces efforts t’ont brisée en échange.#Bulle:NimuHautDroite
Est-ce que tout cela en valait vraiment la peine ? #Bulle:NimuHautDroite
Tu entends un vombrissement ténu, là, du fond de ton bras cybernétique. #Bulle:NimuHautDroite

Peut-être que tu devrais enfin faire preuve du véritable courage.#Bulle:NimuHautDroite
Peut-être que la couche d’ozone a raison. #Bulle:NimuBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser2===


//blabla

C’est quand même une sacrée matinée.
On a bien galopé. C’est que c’est sportif de suivre ce gaillard.  #Bulle:NimuBasDroite
Qu’est-ce qu’il aurait fait si t’avais pas été là ?  #Bulle:NimuBasDroite
Pfff. De qui tu te moques ? #Bulle:NimuBasDroite
Tu serres le poing. #Bulle:NimuBasDroite
Tu ne t’étais pas amusée comme ça depuis des mois. #Bulle:NimuBasDroite
Oh, ce n’était pas de tout repos. #Bulle:NimuBasDroite
Mais c’est aussi agréable d’être épuisée. #Bulle:NimuBasDroite
Vraiment épuisée.  #Bulle:NimuBasDroite
Pas léthargique, comme tu te sens tout le temps en restant à l’intérieur. #Bulle:NimuBasDroite
Tu as passé ta vie à porter le monde sur tes épaules.  #Bulle:NimuBasDroite
À prier pour pouvoir passer des jours tranquilles. #Bulle:NimuBasDroite
Et maintenant… Quoi ? #Bulle:NimuBasDroite
Tu te rends compte que cette tranquilité n’a aucune valeur ? #Bulle:NimuBasDroite
Non. #Bulle:NimuBasDroite
Non, pas exactement.  #Bulle:NimuBasDroite
Les responsabilités sont loin de te manquer...  #Bulle:NimuBasDroite
Devoir choisir qui aurait des médicaments pendant les pénuries, 
ou est-ce qu’il vaudrait mieux déménager la colonie #Bulle:NimuBasDroite
à la recherche d’un point d’eau quand il n’avait pas plu pendant plusieurs mois.
Tu es bien contente de ne plus avoir à te soucier de tout ça. #Bulle:NimuBasDroite
Mais, la vie à la maison, réparer ton vélo, #Bulle:NimuBasDroite
te balader en forêt, aider tes petits-enfants…  #Bulle:NimuBasDroite
Ça, #Bulle:NimuBasDroite
c’est une charge que tu peux encore porter. #Bulle:NimuBasDroite
Tout ce temps, tu t’es comportée comme si tu n’étais déjà plus là. #Bulle:NimuBasDroite
Mais tu ne peux pas décréter qu’on n'a plus besoin de toi.  #Bulle:NimuBasDroite

Même si ça s’avère vrai, #Bulle:NimuBasDroite
tant qu’on t’appelle à l’aide, tu veux répondre. #Bulle:NimuBasDroite
C’est ça, les valeurs les plus importantes pour toi. #Bulle:NimuBasDroite
Prendre soin de ce qui est autour de toi. Même si tu vieillis… #Bulle:NimuBasDroite
C’est comme ça que tu veux vivre tes derniers jours. #Bulle:NimuBasDroite
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE

->END
