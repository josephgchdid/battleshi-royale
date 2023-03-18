package com.example.logicservice.entity.lobby

import com.example.logicservice.entity.game.Ship
import com.example.logicservice.entity.game.Coordinates
import org.springframework.data.annotation.Id
import org.springframework.data.redis.core.RedisHash


@RedisHash("Player")
data class Player(

    @Id
    val playerId : String,

    val id : String,

    val gameId : String,

    var squares : ArrayList<ArrayList<Coordinates>>,

    var totalShipsLeft : Int = 0,

    val ships : ArrayList<Ship>,

    )