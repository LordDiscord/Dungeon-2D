# Dungeon-2D
Este juego va a ser un juego de mazmorreo con el classico sistema de ir de sala en sala derrotando enemigos y looteando, pero estará basado en Dungeons & Dragons lo cual significará que el personaje podrá escoger diferentes clases al inicio con armas unicas, los combates serán por turnos y la propia estetica del juego sera pixel art Top-Downy ambientada en fantasia epica / medieval.

Este va a ser un juego con un sistema de combate por turrnos basado en Dungeons & Dragons, en el cual tanto los jugadores como los enemigos tienen una serie de estadisticas que representaran sus virtudes y tendrán que moverse por un tablero que de momento esta limitado el movimiento a 6 casillas para luego atacar al enemigo.

Los controles son los siguientes (Estan sujetos a cambio):
Movimiento por casillas(limitado a 6): W,A,S,D
Atacar: Click izquierdo del raton encima del enemigo (Condicion: Ha de estar a 1 casilla de distancia como mucho)
Pasar Turno: Pulsar la tecla "T"

Como funciona el combate:
Primero se mira cuanta iniciativa tiene el jugador y los enemigos, segun eso se determina quien empieza el turno de combate
Luego cuando se ataca, se tira un dado de 20 y a eso se le suma el modificador de destreza que se tenga, luego el numero se compara con la armadura del enemigo y si este la iguala o supero entonces le da y se realiza otra tirada para el daño(un dado d6 + destreza), luego de eso se le resta la vida pertinente.
Y asi hasta que cualquiera de los dos bandos acabe con el otro.
(De momento no esta implementado pero los magos irán por initeligencia en vez de destreza)
