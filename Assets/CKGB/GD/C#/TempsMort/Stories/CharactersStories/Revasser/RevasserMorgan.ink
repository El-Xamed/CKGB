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
~ Trigger("name")

Les lumières vives des évènements - passés et à venir - de la journée flashent devant tes yeux.#Bulle:Narrateur
Elles submergent ton crâne comme les inondations de l'hiver dernier.#Bulle:Narrateur
Tu as besoin de temps. Pour digérer tes pensées. Pour te préparer.#Bulle:Narrateur
~ Trigger("name")
Tu fais quelques pas à l’écart, tes yeux rivés vers le ciel. #Bulle:Narrateur

Ta main vient caresser le carnet de poésie logé dans ta poche. Tu souris. Tu le trimballe toujours avec toi.#Bulle:Narrateur
Tu sors ton stylo et commence à le mâchouiller.#Bulle:Narrateur
En général, écrire t’aide à faire la paix avec tes émotions.#Bulle:Narrateur
Des gens ont déjà essayé de te guider vers la méditation ou l’exercice physique pour réussir à te sortir de ta tête.#Bulle:Narrateur
Mais rien n’a jamais aussi bien marché que l’art-thérapie.#Bulle:Narrateur
Tu commence à griffonner.#Bulle:Narrateur
De quoi tu as envie ? #Bulle:Narrateur
Tu te sens d’humeur haïku aujourd’hui.#Bulle:Narrateur

Depuis quelques semaines, c’est devenu ta marotte.#Bulle:Narrateur

<i>(5, 7, 5.)</i> #Bulle:MorganHautGauche 
La structure simple et stable des syllabes t’apporte du réconfort.#Bulle:Narrateur
<i>(5, 7, 5,)</i> #Bulle:MorganHautGauche 
comme les secondes que tu compte pour mesurer tes respirations.#Bulle:Narrateur
Tu te débarasse des rhymes.#Bulle:Narrateur
Tu cherche quelque choses qui frappe sans avoir recours à de la phonétique aussi simple.#Bulle:Narrateur
<i>(5, 7, 5,)</i> #Bulle:MorganHautGauche
Tu pense aux évènements de ce matin. A comment tu t’es réveillé à 4 heure. #Bulle:Narrateur
. Tes ongles brûlent encore, rongés nerveusement, les yeux enfoncé dans les planches plafond.#Bulle:Narrateur
<i>(5)</i>,#Bulle:MorganHautGauche
Pupilles fixes,#Bulle:Narrateur
<i>(7,)</i> #Bulle:MorganHautGauche
Insomnie amoureuse,#Bulle:Narrateur
<i>(5,)</i>, #Bulle:MorganHautGauche
Jour fébrile.#Bulle:Narrateur

<i>(Hm.)</i>#Bulle:MorganHautGauche
<i>(Peut mieux faire.)</i>#Bulle:MorganHautGauche

~ Trigger("name")
Ton stylo griffe le papier quelques minutes. La réalité glisse autour de toi.#Bulle:Narrateur
Quand, au bout d’un moment, tu te relève pour rejoindre les autres, tu as un peu oublié ce qui te rendais inquiet. #Bulle:Narrateur
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser1===

Tes yeux cligne.#Bulle:MorganBasGauche
Dans cet instant de relâche, tu prends conscience de la tension de tes muscles. #Bulle:MorganBasGauche
La contraction de ta machoire.#Bulle:MorganBasGauche
La foudre dans tes épaules.#Bulle:MorganBasGauche
La vitesse de ta respiration. #Bulle:MorganBasGauche
Tu décroche un des tes stylos d’une de tes poches, et commence à le faire tourner entre tes doigts.#Bulle:MorganBasGauche
Tu cherche quelque chose contre lequel t’adosser. Tes doigts frole le papier contenu dans tes poches, à la recherche de ton carnet dans un océan de brouillons, de notes et de penses-bête.#Bulle:MorganBasGauche
Quelque chose t’arrête cependant.#Bulle:MorganHautGauche
#Bulle:MorganBasGauche
La pointe de ton stylo plume touche la fibre végétale. N’es tu pas entrain d’entretenir un cercle vicieux ? #Bulle:MorganBasGauche
Tu t’es mit dans la tête que l’art thérapie était une bonne manière de gérer tes sentiments. Mais est-ce que c’est vraiment le cas ?#Bulle:MorganBasGauche
Est-ce que baigner dans les pensées que ton cerveau a machouillé comme des chewing gums,#Bulle:MorganBasGauche
jusqu’à pouvoir extraire leur saveur sur du papier est vraiment sain ?#Bulle:MorganBasGauche
Et quand est il de les exposer pour avoir de l’attention ?#Bulle:MorganBasGauche
Est-ce que c’est le genre de personne que tu veux être ?#Bulle:MorganBasGauche

Est-ce que la poésie a sa place dans l’identité que tu veux te constuire ? #Bulle:MorganBasGauche
Est-ce qu’en fait ce ne serait pas en vérité… La dernière chose qui te retiens d’avancer ?#Bulle:MorganBasGauche
La dernière chose qui te force à rester cette créature fébrile manipulée par ses émotions ? #Bulle:MorganBasGauche
Un lac d’encre a imprégné la page.#Bulle:MorganBasGauche
tu referme le stylo plume. #Bulle:MorganBasGauche
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser2===

Ca a commencé comme une musique dans ta tête.#Bulle:Narrateur
Comme une pensée, quelques mots, des phrases que tu étais incapable de te sortir de la tête.#Bulle:Narrateur
C’est plus comme une démangeaison maintenant. Cela DEMANDE ton attention. Tu saisis de cet instant pour t’en débarasser.#Bulle:Narrateur
Impulsivement, tu débouche ton stylo avec les dents, et commence a griffoner, assis sur ton siège de train. #Bulle:Narrateur

Tu ne sais peut-être pas grand chose sur toi-même.#Bulle:Narrateur
Ou sur quel genre de personne tu veux devenir. Mais dans cette océan d’incertitude, tu sens un prise à laquelle te raccrocher.#Bulle:Narrateur
Écrire n’est pas une manie.#Bulle:Narrateur
Ce n’est pas un simple système d’auto-défense.#Bulle:Narrateur
C’est ta vocation. #Bulle:Narrateur

<i>"Organes papier
Se faire un sang d’encre
Un contrat de chair"</i>#Bulle:MorganBasGauche

<i>"Tous les tatouages
peuvent s’effacer, mais pas 
les poème apprit."</i>#Bulle:MorganBasGauche

<i>"Aucune lentille
Ne peux me protéger du
Soleil en mon sein"</i>#Bulle:MorganBasGauche

C’est ridicule.#Bulle:Narrateur
Tu te sens tellement futile et vulnérable.#Bulle:Narrateur
Peut-être que ça t’empêchera toute ta vie d’enfin devenir le Coolkid que tu rêve d’être.#Bulle:Narrateur
Et tu n’es pas sûr de pouvoir faire la paix avec cela un jour.#Bulle:Narrateur
Mais si c’est le prix pour continuer de faire la chose la plus importe de ta vie...#Bulle:Narrateur
Alors tu es fier de le payer.#Bulle:Narrateur
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE

->END
