using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUp : MonoBehaviour
{
    //Adicionar ao Enum o PowerUp para uma Lista
    public enum PowerUpType
    {
        SpeedBoost,      // multiplica a velocidade do player por 1.5x por 6s
        JumpBoost,       // multiplica o salto do player por 1.3x por 6s
        DamageBoost,     // multiplica o dano do player por 2x por 4s
        Invulnerability, // torna o player invulnerável por 4s
        MoreHealth,      // adiciona uma vida máxima ao player
        Heal             // back to full hp
    }
    // ao ser instanciado, vai colocar esta variavel com o tipo de efeito
    public PowerUpType type;

}