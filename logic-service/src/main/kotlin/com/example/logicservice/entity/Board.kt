package com.example.logicservice.entity

import org.springframework.data.annotation.Id
import org.springframework.data.annotation.Reference
import org.springframework.data.redis.core.RedisHash
import org.springframework.data.redis.core.index.Indexed

@RedisHash("Board")
data class Board(

    @Id
    val playerId : String,

    val id : String,

    val gameId : String,

    var squares : ArrayList<ArrayList<Coordinates>>,

    var totalShipsLeft : Int = 0,

    val ships : ArrayList<Ship>,

    )