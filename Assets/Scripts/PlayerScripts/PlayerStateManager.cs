﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum State
    {
        STATE_IDLE_SWORDMASTER,
        STATE_RUNNING_SWORDMASTER,
        STATE_JUMPING_SWORDMASTER,
        STATE_FALLING_SWORDMASTER,
        STATE_WALLSLIDING_SWORDMASTER,
        STATE_DAMAGED_SWORDMASTER,
        STATE_GROUND_DASHING_SWORDMASTER,
        STATE_BRAVE_STRIKE_SWORDMASTER,
        STATE_FIRST_ATTACK_SWORDMASTER,
        STATE_SECOND_ATTACK_SWORDMASTER,
        STATE_THIRD_ATTACK_SWORDMASTER,
        STATE_JUMPING_ATTACK_SWORDMASTER,
        STATE_UP_KICK_READY_SWORDMASTER,
        STATE_UP_KICK_ATTACK_SWORDMASTER,
        STATE_DOWN_KICK_READY_SWORDMASTER,
        STATE_DOWN_KICK_ATTACK_SWORDMASTER,
        STATE__TRICKSTER,
        STATE_IDLE_TRICKSTER,
        STATE_RUNNING_TRICKSTER,
        STATE_JUMPING_TRICKSTER,
        STATE_FALLING_TRICKSTER,
        STATE_WALLSLIDING_TRICKSTER,
        STATE_DAMAGED_TRICKSTER,
        STATE_GROUND_DASHING_TRICKSTER,
        STATE_AIR_DASHING_TRICKSTER,
        STATE_SHOOTING_IDLE_TRICKSTER,
        STATE_SHOOTING_RUNNING_TRICKSTER,
        STATE_SHOOTING_JUMPING_TRICKSTER,
        STATE_SHOOTING_DASHING_TRICKSTER,
    }
    public State PlayerState;
    void Start()
    {
        PlayerState = State.STATE_IDLE_SWORDMASTER;
    }
}
