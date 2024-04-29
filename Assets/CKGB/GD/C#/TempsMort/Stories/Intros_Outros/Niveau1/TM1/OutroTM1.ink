->Outro
=== function StartChallenge(name)

EXTERNAL StartChallenge(name)

===Outro===
//Après avoir validé la fin du temps mort, swipe noir soft vers la droite de l'écran Morgan commence à faire les cents pas. Bruit de porte qui se ferme et la grand mère qui s'écarte d'une des portes en debut de scène. Morgan est en train de faire les 100 pas.
~ Trigger("name")

Tiens, t’es pas encore parti ?#Bulle:NimuHautDroite

Mes cheveux.#Bulle:MorganHautDroite
~ Trigger("name")
Ils sont sur ta tête,<br> à ma connaissance.#Bulle:NimuHautGauche

Ils sont bien comme ça ?#Bulle:MorganHautDroite
~ Trigger("name")
Sur ta tête ?#Bulle:NimuHautGauche
~ Trigger("name")
Tu te retournes partiellement vers elle et fais ta fameuse moue inexpressive et sincèrement ennuyée jusqu'à ce qu'elle concède d'arrêter ses bêtises. #Bulle:Narrateur

Ils sont parfaits tes cheveux.#Bulle:NimuHautDroite

Ok.<br>et j’ai pas de tâche de confiture dans mon dos ?#Bulle:MorganBasGauche
~ Trigger("name")
Tu te contortionnes de nouveau pour lui faire observer ta chemise.#Bulle:Narrateur

Mais<bounce>…</bounce> pourquoi t’aurai<br> de la confiture dans le<bounce>…</bounce><br> <shake>Non<shake> ! #Bulle:NimuHautDroite

Tu pousses un soupir de soulagement et te frottes les mains un instant.#Bulle:Narrateur

’kay. Brillant.#Bulle:MorganHautDroite
~ Trigger("name")
Prêt à partir, tu tapes successivement tes poches pour t'assurer que tout est bien à sa place (pour la 21e fois).#Bulle:Narrateur
Tu fronces les sourcils.#Bulle:Narrateur
~ Trigger("name")
Ah, et t’aurais pas vu mes <wave>clés</wave> ?#Bulle:MorganHautauche
~ Trigger("name")
Mais tu vas décamper à la fin ?#Bule:NimuHautDroite #emot:!

Non, mais pour de vrai!#Bulle:MorganHautDroite
~ Trigger("name")
Je mets pas la main dessus ! #Bulle:MorganHautGauche

Bah attends je vais te prêter les miennes.#Bulle:NimuHautGauche

Fluidement, elle s'approche du petit bol dans lequel elle laisse toujours ses clés.#Bulle:Narrateur
~ Trigger("name")
Hm#Bulle:NimuHautDroite

<bounce>…</bounce> Je les retrouve pas non plus.#Bulle:NimuHautDroite
~ Trigger("name")
Ah. Cata. P'tetre dans l'canapé ? #Bulle:MorganBasGauche
~ StartChallenge("S_Challenge")
//Start challenge
->END
