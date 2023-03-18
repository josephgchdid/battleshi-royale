package com.example.logicservice.entity.game

import com.example.logicservice.entity.game.Coordinates

data class Missile(
    val originPlayer : String,

    val destinationPlayer : String, 
    
    val coordinates: Coordinates
)
