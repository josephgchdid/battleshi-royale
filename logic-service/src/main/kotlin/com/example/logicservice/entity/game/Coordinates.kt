package com.example.logicservice.entity.game


data class Coordinates(

    val x : Int,

    val y : Int,

    var isBombed : Boolean,

    var bombDidHit : Boolean
)
