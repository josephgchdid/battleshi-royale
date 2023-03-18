package com.example.logicservice.entity.game

import com.example.logicservice.entity.game.Coordinates
import org.springframework.data.annotation.Id
import org.springframework.data.redis.core.RedisHash

@RedisHash
data class Ship(

    @Id
    val shipId : String,

    val shipType : String,

    val shipSize : Int,

    var squaresLeft : Int,

    val coordinates: ArrayList<Coordinates>
)
