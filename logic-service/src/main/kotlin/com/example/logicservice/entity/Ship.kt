package com.example.logicservice.entity

data class Ship(
    val shipType : String,

    val shipSize : Int,

    var squaresLeft : Int,

    val coordinates: ArrayList<Coordinates>
)
