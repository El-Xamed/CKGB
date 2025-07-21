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

Ta main vient caresser le carnet de poésie logé dans ta poche. Tu souris. Tu le trimballes toujours avec toi.#Bulle:Narrateur
Tu sors ton stylo et commence à le mâchouiller.#Bulle:Narrateur
En général, écrire t’aide à faire la paix avec tes émotions.#Bulle:Narrateur
Des gens ont déjà essayé de te guider vers la méditation ou l’exercice physique pour réussir à te sortir de ta tête.#Bulle:Narrateur
Mais rien n’a jamais aussi bien marché que l’art-thérapie.#Bulle:Narrateur
Tu commences à griffonner.#Bulle:Narrateur
De quoi tu as envie ? #Bulle:Narrateur
Tu te sens d’humeur haïku aujourd’hui.#Bulle:Narrateur

Depuis quelques semaines, c’est devenu ta marotte.#Bulle:Narrateur

<i>(5, 7, 5.)</i> #Bulle:MorganHautGauche 
La structure simple et stable des syllabes t’apporte du réconfort.#Bulle:Narrateur
<i>(5, 7, 5,)</i> #Bulle:MorganHautGauche 
Comme les secondes que tu comptes pour mesurer tes respirations.#Bulle:Narrateur
Tu te débarasses des rimes.#Bulle:Narrateur
Tu cherches quelque choses qui frappe sans avoir recours à de la phonétique aussi simple.#Bulle:Narrateur
<i>(5, 7, 5,)</i> #Bulle:MorganHautGauche
Tu penses aux évènements de ce matin. À comment tu t’es réveillé à 4 heures. #Bulle:Narrateur
. Tes ongles brûlent encore, rongés nerveusement, le regard enfoncé dans les planches du plafond.#Bulle:Narrateur
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
Quand, au bout d’un moment, tu te relèves pour rejoindre les autres, tu as un peu oublié ce qui te rendait inquiet. #Bulle:Narrateur
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser1===

Tes yeux clignent.#Bulle:Narrateur
~ Trigger("name")
Dans cet instant de relâche, tu prends conscience de la tension de tes muscles. #Bulle:Narrateur
La contraction de ta mâchoire.#Bulle:Narrateur
La foudre dans tes épaules.#Bulle:Narrateur
La vitesse de ta respiration. #Bulle:Narrateur
Tu décroches ton stylo d’une de tes poches, et commences à le faire tourner entre tes doigts.#Bulle:Narrateur
~ Trigger("name")
Tu cherches quelque chose contre quoi t’adosser. Tes doigts frôlent le papier contenu dans tes poches,#Bulle:Narrateur
 à la recherche de ton carnet dans un océan de brouillons, de notes et de pense-bêtes.#Bulle:Narrateur
Quelque chose t’arrête cependant.#Bulle:Narrateur #emot:Dots

La pointe de ton stylo-plume touche la fibre végétale. N’es-tu pas en train d’entretenir un cercle vicieux ?#Bulle:Narrateur #emot:?
Tu t’es mis dans la tête que l’art-thérapie était une bonne manière de gérer tes sentiments. Mais est-ce que c’est vraiment le cas ? #emot:?
Est-ce que baigner dans les pensées que ton cerveau a mâchouillé comme des chewing-gums <br> jusqu’à pouvoir en extraire la saveur sur du papier est vraiment sain ? #emot:?
Et qu'en est-il de les exposer pour avoir de l’attention ? #emot:Deception
Est-ce que c’est le genre de personne que tu veux être ? #emot:?

Est-ce que la poésie a sa place dans l’identité que tu veux te constuire ?
Est-ce qu’en fait, ce ne serait pas en vérité… la dernière chose qui te retient d’avancer ? #Bulle:Narrateur #emot:Dots
La dernière chose qui te force à rester cette créature fébrile manipulée par ses émotions ? #Bulle:Narrateur
Un lac d’encre a imprégné la page. #Bulle:Narrateur
Tu refermes le stylo-plume. 
~ Trigger("name")
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE
===Revasser2===

Ça a commencé comme une musique dans ta tête.#Bulle:Narrateur
Comme une pensée, quelques mots, des phrases que tu étais incapable de te sortir de la tête.#Bulle:Narrateur
C’est plus comme une démangeaison maintenant. Cela DEMANDE ton attention. Tu saisis cet instant pour t’en débarasser.#Bulle:Narrateur
Impulsivement, tu débouches ton stylo avec les dents, et commences à griffoner, assis sur ton siège de train. #Bulle:Narrateur

Tu ne sais peut-être pas grand chose sur toi-même.#Bulle:Narrateur
Ou sur quel genre de personne tu veux devenir. Mais dans cet océan d’incertitude, tu sens un prise à laquelle te raccrocher.#Bulle:Narrateur
Écrire n’est pas une manie.#Bulle:Narrateur
Ce n’est pas un simple système d’auto-défense.#Bulle:Narrateur
C’est ta vocation. #Bulle:Narrateur

<i>"Organes papier
Se faire un sang d’encre
Un contrat de chair"</i>#Bulle:MorganBasGauche

<i>"Tous les tatouages
peuvent s’effacer, mais pas 
les poèmes appris."</i>#Bulle:MorganBasGauche

<i>"Aucune lentille
Ne peux me protéger du
Soleil en mon sein"</i>#Bulle:MorganBasGauche

C’est ridicule.#Bulle:Narrateur
Tu te sens tellement futile et vulnérable.#Bulle:Narrateur
Peut-être que ça t’empêchera toute ta vie d’enfin devenir le Coolkid que tu rêves d’être.#Bulle:Narrateur
Et tu n’es pas sûr de pouvoir faire la paix avec cela un jour.#Bulle:Narrateur
Mais si c’est le prix pour continuer de faire la chose la plus importante de ta vie...#Bulle:Narrateur
Alors tu es fier de le payer.#Bulle:Narrateur
~ IDrevasser++
~ RetourAuTMAfterRevasser("")
->DONE

->END
