->checkID
=== function Trigger(name)
EXTERNAL Trigger(name)
=== function RetourAuTMAfterRespirer(name)
EXTERNAL RetourAuTMAfterRespirer(name)
VAR IDobserver = 0
===checkID===
{ 
- IDobserver==1: ->observer0
- IDobserver==2: ->observer1
- IDobserver==3: ->observer2
- else: ->END
}
===observer0===
~ Trigger("name")

Tu t’écarte un peu du groupe pour absorber le décors.#Bulle:Narrateur
~ Trigger("name")
Dans tes oreilles coulent les roulis des rivières et les gazouillis des oiseaux.#Bulle:Narrateur
Ce sont des sons que tu entends presque tous les jours.#Bulle:Narrateur
Étrangement, ils ne cessent jamais de te faire sourire pour autant.#Bulle:Narrateur
~ Trigger("name")
Il y a quelque chose de simple,#Bulle:Narrateur
d’instinctif dans le fait de te tenir ici,#Bulle:Narrateur
le visage constellé par les rayons de soleil qui s’infiltre au travers de la canopé.#Bulle:Narrateur

Tu prends une grande respiration,#Bulle:Narrateur
~ Trigger("name")
et tu croque dans une tomate que tu as arraché de son pied. #Bulle:Narrateur
Tu t’es un peu taché avec le jus, mais ça en valait la peine. #Bulle:Narrateur
Tu es prêt.e à repartir. #Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE
===observer1===

//blabla

Nerveusement, presque comme un automatisme, tu entretient les plantes devant lesquelles tu passes.#Bulle:Narrateur
Un coup d’arrosoirs ici, retirer des galets là,#Bulle:Narrateur
arracher quelques pousses qui risquent de gêner les autres. #Bulle:Narrateur
C’est un travail sans fin. #Bulle:Narrateur
Tes yeux se lève vers le ciel alors qu’une ombre fend les buissons.#Bulle:Narrateur
Elle se pose sur un arbre. Une pie des sables. #Bulle:Narrateur
Ses serres s’agrippe à une branche. Ta nuque conteste le mouvement que tu lui demande pour garder l’oiseau sous les yeux.#Bulle:Narrateur
Tu ne l’écoute pas, absorbé par le bruissement des plumes. #Bulle:Narrateur
La singularité a redistribué beaucoup de cartes. #Bulle:Narrateur
De ce que tu as entendu, la pie des sables étaient presque éteinte il y a un peu moins d’un siècle.#Bulle:Narrateur
Malgré un climat théoriquement de plus en plus favorable à sa prospérité, les brusques changements climatiques et l’urbanisation intenses ont contribué à désorienter et affamer l’espèce entière.#Bulle:Narrateur
Ces problèmes ont étés réglés par la soudaine pénuries d’humain #Bulle:Narrateur
et abondance de nature provoqué par la singularité. #Bulle:Narrateur

On ne peut pas se réjouir de catastrophes naturelles qui ont mis fins aux jours de milliards de personnes.#Bulle:Narrateur
Mais en voyant la manière dont la pie des sables bouge sa tête par à-coups#Bulle:Narrateur
et abrite ses petits sous son aile,#Bulle:Narrateur
tu ne peux t’empêcher de penser que,#Bulle:Narrateur
peut-être,#Bulle:Narrateur
le bilan n’est pas que négatif.#Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE
===observer2===


//blabla

Tu gravis une des buttes agricoles pour avoir tout le panorama sous les yeux.#Bulle:Narrateur
Des billes de terres sèches dégringolent alors que tu prends appuies sur les planches qui ont été enfoncée dans la terre pour former des marches sur la colline.#Bulle:Narrateur
Parfois, cette vue te fait mal à la tête. #Bulle:Narrateur
Enfin, non. #Bulle:Narrateur
Le paysage en lui-même est apaisant. #Bulle:Narrateur
Mais penser à ce qu’il y a derrière à tendance à te prendre la tête.#Bulle:Narrateur
La toile complexe de chaque plante,#Bulle:Narrateur
chaque animal,#Bulle:Narrateur
chaque nutriment de la terre,#Bulle:Narrateur
qui interagissent entre eux dans un concert de vie et de mort.#Bulle:Narrateur
Ça,#Bulle:Narrateur
ça te fais mal à la tête.#Bulle:Narrateur
La richesse des relations entre tous les phénomènes naturels#Bulle:Narrateur
au milieu desquels vous vivez.#Bulle:Narrateur
Comment être sûr d’être à sa place dans cette formule chimique ?#Bulle:Narrateur
Comment être sûr d’être le muscle qui aide à respirer, et pas l’organe cancéreux ?#Bulle:Narrateur

Tu redescend la colline en y réfléchissant. #Bulle:Narrateur
L’humanité n’en sera probablement jamais certaine.#Bulle:Narrateur
Elle peut seulement essayer de survivre#Bulle:Narrateur
en prenant ce dont elle a besoin et en réparant derrière elle.#Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE

->END