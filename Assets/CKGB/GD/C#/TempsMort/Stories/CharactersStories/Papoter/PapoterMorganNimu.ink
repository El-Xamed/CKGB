-> checkID
=== function StartTM(name)
EXTERNAL StartTM(name)
=== function Trigger(name)
EXTERNAL Trigger(name)
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
~ Trigger("name")
Tu te ronges les ongles. <br>Le bout de tes doigts te brûle #Bulle:Narrateur
Ta grand mère commences à fredonner une chanson #Bulle:Narrateur
…#Bulle:MorganHautGauche #emot:Dots
<wave>Alors...</wave>#Bulle:NimuBasDroite
Pas trop nerveux ?#Bulle:NimuBasDroite
~ Trigger("name")
Hm ?#Bulle:MorganHautGauche #emot:!
…‘Nah.#Bulle:MorganHautGauche
Bah tu vas dead ça alors.#Bulle:NimuBasDroite
…#Bulle:MorganHautGauche #emot:Dots
Je suis vraiment fière de toi,<br> tu sais ?#Bulle:NimuBasDroite
Tu es pris au dépourvu.<br> Tu ne le laisses pas paraitre.#Bulle:Narrateur
A-ah oui ?#Bulle:MorganHautGauche #emot:!
De fou !<br> ça demande vlà du courage <br>d'être aussi calme dans cette situation.#Bulle:NimuBasDroite
…#Bulle:MorganHautGauche #emot:Dots
Ca me rappelle mon premier<br> rendez-vous amoureux.<br> Haha, comment c’était claquée au sol.#Bulle:NimuBasGauche #emot:Drop
J’ai mis <incr>des années</incr> <br>avant de trouver des techniques<br> pour garder mon sang froid.#Bulle:NimuBasGauche 
Tu m’impressiones vraiment.#Bulle:NimuBasDroite #emot:Sparkles

Tu te mets nerveusement à jouer avec la fermeture éclaire de tes poches.#Bulle:Narrateur

Ta grand mère reprend son fredonnement.#Bulle:Narrateur

…#Bulle:MorganHautGauche #emot:Dots

Néanmoins.#Bulle:NimuHautDroite

Sache que je ne serai<br> pas moins fiere de toi <br>si jamais tu as besoin d'aide.#Bulle:NimuBasDroite
Si jamais, <br>à n’importe quel moment,<br> tu sens que tu as besoin de soutien,<br> tu as juste à me demander.#Bulle:NimuBasDroite
Ca ne demande pas moins de courage que d'avouer qu'on a un problème.<br> J'en sais quelque chose.#Bulle:NimuHautDroite
Elle a un regard pour son bras.#Bulle:Narrateur
On pourrait dire que peu importe ce que tu fais, je trouverai toujours une excuse pour être fiere de toi. #Bulle:NimuHautGauche
hm.#Bulle:MorganHautGauche

Wow. C'était un déclaration sacrément sentimentale et soudaine dans une journée déjà assez riche en rebondissements.#Bulle:Narrateur
Tu sais pas vraiment comment réagir, tu vas probablement avoir besoin d'un peu de temps pour digerer tout ça.#Bulle:Narrateur

Ta grand mère commence à chantonner au milieu de ses freudonnement.#Bulle:Narrateur
-ark tutudulutu..Baby sh-#Bulle:NimuBasDroite

<i>P’tetre.</i>#Bulle:MorganHautGauche

Hm ?#Bulle:NimuBasDroite emot:?

Peut-être que j’aprécierai beneficier de quelques conseils.#Bulle:MorganHautGauche

Elle a un petit sourire joueur. Et te montre qu'elle est prête à écouter tes questions. #Bulle:Narrateur 
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter1===
Mémé ?#Bulle:MorganHautDroite

Hm ?#Bulle:NimuHautGauche

J’me… J’me sens fébrile.#Bulle:MorganHautDroite

Bah viens on se pose.#Bulle:NimuHautGauche #emot:!

Tu sens une boule te monter <br>dans la gorge alors que <br>tu t'assois à coté de ta grand-mère.#BulleNarrateur

… J’pense que je vais faire demi-tour.#Bulle:MorganHautGauche #emot:Dots
J’ai… plus assez d'énergie pour continuer. 
J'crois qu'la pression emotionnelle <br>est trop pour moi.#Bulle:MorganHautGauche
Je vais retourner à la maison.
J'lui ferai passer un message pour m'excuser.#Bulle:MorganHautDroite

Ta grand mère lève sa main dans ta direction, attendant ton signal. Tu lui fais non de la tête, et elle la ramène dans sa poche.#Bulle:Narrateur
Elle fait toujours attention à ne pas te toucher sans t'avoir demandé avant. Cette attention t’as toujours bien plus réconforté que n’importe quel caresse.#Bulle:Narrateur

Si c’est vraiment ce que tu veux,<br> c’est ce qu’on fait.#Bulle:NimuHautGauche

Il n’y a rien de cringe<br> dans le fait de prendre soin de soi.#Bulle:NimuHautGauche
Pour l’instant, prends ton temps.<br> Tu as l’air de te sentir mal,<br> ce n’est peut-être pas forcément <br>le meilleur moment pour prendre des décisions."#Bulle:NimuHautGauche
Tu fronces les sourcils.<br> Tu n'es pas d'accord.#Bule:Narrateur
C’est parc’que j’suis<br> dans c’t’état que j’prends cette décision. <br>On rentre."#Bulle:MorganHautDroite

Tu te relèves.#Bulle:Narrateur

Tu es sûr.e ?#Bulle:NimuHautGauche

Est-ce que tu es sûr ?#Bulle:Narrateur
Tu fronces tes sourcils, la tête baissé vers le sol.<br> Tu observes la procession d’une coccinelle.#Bulle:Narrateur
Ta grand mère prend appuie sur quelque chose pour se relever à son tour. Tous ses os craques.#Bulle:Narrateur

Tiens, bois un peu d’eau."#Bulle:NimuHautGauche
Elle sort une petite gourde de sa veste et te la tends.#Bulle:Narrateur
J’ai confiance en toi.<br> Et puis, t’as fais le plus dur.#Bulle:NimuHautGauche

Je suis sorti de mon lit ?#Bulle:MorganHautDroite

Tu l’as invité à ce rencard.<br> Et iel a accepté en plus ! <br>Tu penses que si iel ne t'apréciait pas déjà iel aurait accepté ?#Bulle:NimuHautGauche #emot:Sparkles

Mais… Mais… c'est jamais trop tard<br> pour que je ruine tout.<br> Imagine qu’on trouve pas<br> d’quoi parler et qu’ia juste<br> un gros blanc pendant une heure.#Bulle:MorganHautDroite #emot:Deception
Imagines que le seul son qu’on entende<br> c’est le bruit bizarre que je fais<br> quand je bois du thé !#Bulle:MorganHautDroite #emot:Drop

Hey,#Bulle:NimuHautGauche
Regardes-moi. #Bulle:NimuHautGauche

Tu lèves les yeux vers elle sans réfléchir.#Bulle:Narrateur

Non, attends, c'est pas possible.#Bulle:Narrateur

Tu BAISSES les yeux vers elle sans réfléchir.#Bulle:Narrateur

Là, voilà,<br> ca fait plus de sens.#Bulle:Narrateur

Tu as connu ta grand-mère pendant toute ta vie. Une partie de ton malaise cède toujoursface au confort et à la familiarité de l’avoir à tes cotés.#Bulle:Narrateur

Quel intêret d'aller à ce rendez-vous<br> si tout ce que ca te fait<br> c'est du mal ?#Bulle:NimuHautDroite

Sa voix est calme.<br> Presque absente.<br> Tu sens la tienne s'emporter.#Bulle:Narrateur

Parce que dans les livres, <br>ils disent que l'amour<br> est la plus belle chose du monde !#Bulle:MorganHautGauche
Que tu sens un mélange de chatouilles<br> et de fourmis dans tout ton corps,#Bulle:MorganHautGauche
et qu'avec l'autre, <br>tu peux rigoler même quand personne<br> ne fait de blague.#Bulle:MorganHautGauche

Alors, pourquoi est-ce<br> que tu ne te sens pas comme ça ?#Bulle:NimuHautDroite

...#Bulle:MorganHautGauche #emot:Dots

L'amour est une fleur.<br> Son terreau est la confiance.<br> Tu ne pourras sûrement jamais ressentir ce que tu cherches<br> si tu persistes à avoir peur<br> de l'autre.#Bulle:NimuHautDroite
Maintenant, je veux que tu saches <br>que la peur est un adversaire <br>ancien et féroce."#Bulle:NimuHautDroite
Certains d'entre nous partent<br> avant d'avoir réussi<br>à faire la paix avec.#Bulle:NimuHautDroite
Même si c'est à toi<br> de faire le gros du travail,<br> je ne m'attends pas à<br> ce tu surmontes cette épreuve<br> du jour au lendemain.#Bulle:NimuHautDroite
Est-ce que tu veux toujours faire demi-tour ? <br>Je suis sûr que l'autre comprendra.#Bulle:NimuHautDroite

...#Bulle:MorganHautGauche #emot:Dots
Peut être plus tard.<br> Je pense que je peux encore <br>continuer un peu.#Bulle:MorganHautGauche

~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE
===Papoter2===
’Hey, mémé ?#Bulle:MorganHautGauche

H-hm ?

Est-ce que ça va ?#Bulle:MorganHautGauche

Bien sûr chouchou. <br>C’est gentil de demander.

Pour de vrai ?#Bulle:MorganHautGauche

Elle a l'air...<br> Perturbé par ton intêret.#Bulle:Narrateur

Est-ce que quelque chose t’inquiète ?#Bulle:NimuHautDroit

Tu mens#Bulle:MorganHautGauche

Morgan ?

J’ai pris des notes.#Bulle:MorganHautGauche

Ca sonne creepy.#Bulle:NimuHautDroit

Cathy. Elle t’a offert un collier.<br> Tu lui as dit que tu l’adorais.<br> Tu ne l'as jamais porté.<br> Elle t'en a parlé. <br>Et depuis tu l’porte.#Bulle:MorganHautGauche

H-hm oui et ? #Bulle:NimuHautDroit

Le pt’tit déjeuner.<br> Tu as jamais aimé<br> faire la grasse mat’.#Bulle:MorganHautGauche

C’est à cause de ça<br> que tout le monde <br>se lève tôt dans cette baraque.#Bulle:MorganHautGauche

Depuis six mois<br> tu te reveilles deux heures <br>après les autres.<br>Tu les évite.#Bulle:MorganHautGauche

…#Bulle:NimuHautDroit #emot:Dots

Tu peux nous dire quand t’aimes pas quelque chose.<br> Ou quand t’as besoin de temps.#Bulle:MorganHautGauche
T’as le droit de nous dire non.<br> En toute transparence, <br>c’plus vexant qu’tu prétendes le contraire.#Bulle:MorganHautGauche
Tu ne nous laisses même pas<br> l’occasion d’écouter ton avis.#Bulle:MorganHautGauche #emot:!

...#Bulle:NimuHautDroit #emot:Dots
C'est vrai.<br> Je suis dans une période un peu compliquée en ce moment.#Bulle:NimuHautDroit
Je suis... agée, Morgan.<br> Je vois tout sous une perspective très différente de la tienne...#Bulle:NimuHautDroit
Et je crois qu'elle me donne le vertige.#Bulle:NimuHautGauche
J'ai beaucoup de mal à accorder de l'importance aux choses. <br> Tout à l'air si éphèmere vu d'ici.#Bulle:NimuBasGauche
J'avoue que je ne me sens plus vraiment<br> concernée par ce qui se passe dans la maison. #Bulle:NimuBasDroite
Mais bon, vend pas la mèche aux autres.<br> Ils me lacheraient pas les basques sinon. #Bulle:NimuHautDroit
Et je ne me sens pas prête à en parler. #Bulle:NimuHautDroit

OK.#Bulle:MorganHautGauche
…#Bulle:NimuHautDroit

J’voulais juste qu’t’en parle. <br>J’peux voir pourquoi tu ressens ça.#Bulle:MorganHautGauche
Tes sourcils se froncent imperceptiblement#Bulle:Narrateur
Tu vies un truc vraiment pas facile. <br>Ah, mais t’as encore menti.#Bulle:MorganHautGauche

Ah ouai sherlock,<br> et j’peux savoir sur quoi maintenant ?#Bulle:NimuHautDroit

Si tu te sentais vraiment plus concernée,<br> tu s’rai pas venu avec moi.<br> C’tout.#Bulle:MorganHautGauche

Tu t'écartes. <br>Tu l'as assez dérangé.#Bulle:Narrateur
Derrière toi,<br> ta grand mère soupir <br>et marmonne dans sa barbe.#Bulle:Narrateur

J'ai pas élevé des andouilles moi.#Bulle:NimuHautDroit
//blabla
~ IdPapoter++
~ RetourAuTMAfterPapotage("name")
->DONE

->END
