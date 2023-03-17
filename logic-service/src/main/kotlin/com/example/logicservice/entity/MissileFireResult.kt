package com.example.logicservice.entity

data class MissileFireResult(
    val result:Result,
    val firingPlayer : String,
    val firedUponPlayer : String,
    val outputMessage : String,
    val shipSunk : Boolean? = false,
    val playerDidLose : Boolean? = false
)