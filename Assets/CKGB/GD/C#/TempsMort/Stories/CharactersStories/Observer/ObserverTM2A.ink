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

Tu t’écartes un peu du groupe pour absorber le décor.#Bulle:Narrateur
~ Trigger("name")
Dans tes oreilles se déversent les roulis des rivières et les gazouillis des oiseaux.#Bulle:Narrateur
Des sons que tu entends presque tous les jours.#Bulle:Narrateur
Étrangement, ils ne cessent jamais de te faire sourire pour autant.#Bulle:Narrateur
~ Trigger("name")
Il y a quelque chose de simple,#Bulle:Narrateur
d’instinctif dans le fait de te tenir ici,#Bulle:Narrateur
le visage constellé par les rayons de soleil qui s’infiltrent au travers de la canopée.#Bulle:Narrateur

Tu prends une grande inspiration,#Bulle:Narrateur
~ Trigger("name")
et tu croques dans une tomate que tu as arrachée de son pied. #Bulle:Narrateur
Tu t’es un peu taché.e avec le jus, mais ça en valait la peine. #Bulle:Narrateur
Tu es prêt.e à repartir. #Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE
===observer1===

//blabla

~ Trigger("name")
Nerveusement, presque comme un automatisme, tu entretiens les plantes devant lesquelles tu passes.#Bulle:Narrateur
Un coup d’arrosoir ici, retirer des galets là,#Bulle:Narrateur
~ Trigger("name")
arracher quelques pousses qui risquent de gêner les autres. #Bulle:Narrateur
C’est un travail sans fin. #Bulle:Narrateur
~ Trigger("name")
Tes yeux se lèvent vers le ciel alors qu’une ombre fend les buissons.#Bulle:Narrateur
Elle se pose sur un arbre. Une pie des sables. #Bulle:Narrateur
Ses serres s’agrippent à une branche. Ta nuque contestes le mouvement que tu lui demandes pour garder l’oiseau sous les yeux.#Bulle:Narrateur
Tu ne l’écoutes pas, absorbé par le bruissement des plumes. #Bulle:Narrateur
La Singularité a redistribué beaucoup de cartes. #Bulle:Narrateur
De ce que tu as entendu, la pie des sables était presque éteinte il y a un peu moins d’un siècle.#Bulle:Narrateur
Malgré un climat théoriquement de plus en plus favorable à sa prospérité, les brusques changements climatiques et l’urbanisation intenses avaient contribué à désorienter et affamer l’espèce entière.#Bulle:Narrateur
Ces problèmes ont été réglés par la soudaine pénurie d’humains, #Bulle:Narrateur
et par l'abondance de nature provoquée par la Singularité. #Bulle:Narrateur

~ Trigger("name")
On ne peut pas se réjouir de catastrophes naturelles qui ont mis fin aux jours de milliards de personnes.#Bulle:Narrateur
Mais en voyant la manière dont la pie des sables bouge sa tête par à-coups#Bulle:Narrateur
et abrite ses petits sous son aile,#Bulle:Narrateur
tu ne peux t’empêcher de penser que,#Bulle:Narrateur
peut-être,#Bulle:Narrateur
de belles choses en sont aussi nées.#Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE
===observer2===


//blabla

Tu gravis une des buttes agricoles pour avoir tout le panorama sous les yeux.#Bulle:Narrateur
Des billes de terre sèche dégringolent alors que tu prends appui sur les planches qui ont été enfoncées dans la terre pour former des marches sur la colline.#Bulle:Narrateur
Parfois, cette vue te fait mal à la tête. #Bulle:Narrateur
Enfin, non. #Bulle:Narrateur
Le paysage en lui-même est apaisant. #Bulle:Narrateur
Mais penser à ce qu’il y a derrière a tendance à te prendre la tête.#Bulle:Narrateur
La toile complexe de chaque plante,#Bulle:Narrateur
chaque animal,#Bulle:Narrateur
chaque nutriment de la terre,#Bulle:Narrateur
qui interagissent entre eux dans un concert de vie et de mort.#Bulle:Narrateur
Ça,#Bulle:Narrateur
ça te fait mal à la tête.#Bulle:Narrateur
La richesse des relations entre tous les phénomènes naturels#Bulle:Narrateur
au milieu desquels vous vivez.#Bulle:Narrateur
Comment être sûr d’être à sa place dans cette formule chimique ?#Bulle:Narrateur
Comment être sûr d’être le muscle qui aide à respirer, et pas l’organe cancéreux ?#Bulle:Narrateur

Tu redescends la colline en y réfléchissant. #Bulle:Narrateur
L’humanité n’en sera probablement jamais certaine.#Bulle:Narrateur
Elle peut seulement essayer de survivre#Bulle:Narrateur
en prenant ce dont elle a besoin et en réparant derrière elle.#Bulle:Narrateur
~ IDobserver++
~ RetourAuTMAfterRespirer("")
->DONE

->END