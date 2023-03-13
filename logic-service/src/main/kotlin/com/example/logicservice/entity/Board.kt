package com.example.logicservice.entity

import org.springframework.data.annotation.Id
import org.springframework.data.redis.core.RedisHash

@RedisHash("Board")
data class Board(

    @Id
    val boardId : String,

    val ownerId : String,

    val gameId : String,

    var squares : ArrayList<ArrayList<Coordinates>>,

    var totalShipsLeft : Int,

    val ships : ArrayList<Ship>,

)