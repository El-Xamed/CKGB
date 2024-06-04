->Intro
=== function StartTM(name)
EXTERNAL StartTM(name)
=== function Trigger(name)
EXTERNAL Trigger(name)

===Intro===
//Bruit de porte, l’écran du temps mort apparait. Morgan est près d’une porte, Sa grand-mère à table, en train de petit-déjeuner.

’lut#Bulle:MorganBasGauche

Yo BG. Bien dormi ?#Bulle:NimuHautDroite

~ Trigger("name")

’uai. ’va ?#Bulle:MorganHautGauche

Super. #Bulle:NimuHautGauche
Juste, d’un POV pratique…#Bulle:NimuHautGauche
Est-ce que dire des moitiés de mots<br>c’est un plan à long terme pour toi ?#Bulle:NimuHautGauche #emot:question

’tetre.#Bulle:MorganHautGauche

Ok bah <i>gucchi</i>.#Bulle:NimuBasDroite

~ Trigger("name")

Tu as l’air pressé de partir ?#Bulle:NimuHautGauche

H-hm, j’vais prendre le train.#Bulle:MorganHautDroite
~ Trigger("name")
J’ai… <rainb>un truc</rainb>. Aux Marches.#Bulle:MorganHautDroite

Ah.#Bulle:NimuHautGauche
Elle marque une pause.#Bulle:Narrateur
<rainb>Un truc</rainb>. Je vois.#Bulle:NimuHautGauche

Tu espères sincèrement que ce ne soit pas le cas.#Bulle:Narrateur
Elle sourit.
Tu réalises que tu n'as absolument aucune chance de la mener en bateau.#Bulle:Narrateur

Alors je suppose<br> que je vais manger <br>toute seule ce matin…#Bulle:NimuHautGauche
dans cette grande maison #Bulle:NimuHautDroite
toute vide... #Bulle:NimuBasDroite
et froide. #Bulle:NimuBasGauche

Il fait genre. 22 degrés.#Bulle:Narrateur
Mais néanmoins, tu te sens doucement basculer... #Bulle:Narrateur
Puis tu <i>cèdes</i>, incapable de lui résister.#Bulle:Narrateur
~ Trigger("name")
<bounce>…</bounce> Nah mais j’peux prendre cinq minutes<br> si c’est important pour toi.#Bulle:MorganHautDroite #emot:Dots

Elle sourit d’un air satisfait.#Bulle:Narrateur
~ Trigger("name")
<i>Bueño</i>, tu me referas six tartines de confiture<br> avec du beurre s’il te plaît.#Bulle:NimuHautGauche

<i>ARG, LA TRAÎTRESSE.</i> #Bulle:MorganHautDroite 
Tu <fade>soupires</fade> lourdement#Bulle:Narrateur

Brillant.#Bulle:MorganHautDroite
’fais quoi ‘jourd’hui ?#Bulle:MorganHautGauche

Je pensais <i>chill</i> à la maison. <br>Et aller chercher les jeunes à l’école vers 15h.#Bulle:NimuHautGauche
Et toi ?#Bulle:NimuHautGauche

Un piège évident. Elle essaie de te faire parler de ton <rainb>rencard</rainb>. Vite. Contre-attaque avant qu'elle ne te tire les vers du nez.#Bulle:Narrateur

Est-c’que tu vas vraiment manger<br> <size=120%>SIX<size=/120%> tartines de plus ?#Bulle:MorganHautDroite

Hm quoi ? #Bulle:NimuHautGauche
Elle lève à peine les yeux de son livre.#Bulle:Narrateur
<bounce>Ah </bounce>!#Bulle:NimuHautGauche
Non pas du tout.#Bulle:NimuHautGauche
Il y en a trois qui sont pour toi.#Bulle:NimuHautGauche

<b>Échec et mat.</b>#Bulle:Narrateur
Tu fronces les sourcils et te prépare à répondre qu'un Coolkid est parfaitement capable de se nourrir seul, mais elle reprend la parole aussitôt.#Bulle:Narrateur

Après, j’ai assez la dalle<br> pour tout manger si tu préfères.#Bulle:NimuHautGauche

<i>(ARG, LA TRAITRESSE.)</i> #Bulle:MorganHautGauche

<bounce>…</bounce> Nah.#Bulle:MorganHautGauche

<wave>H-hm</wave> ?#Bulle:NimuHautGauche

Je vais avoir besoin de forces.#Bulle:MorganHautGauche

Tu prends une franche bouchée de pain recouvert de confiture.#Bulle:Narrateur

~ StartTM("TM")
->END