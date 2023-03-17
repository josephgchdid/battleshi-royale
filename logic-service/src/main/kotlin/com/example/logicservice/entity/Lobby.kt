package com.example.logicservice.entity

import org.springframework.data.annotation.Id
import org.springframework.data.redis.core.RedisHash

@RedisHash("lobby")
data class Lobby(
    @Id
    val gameId : String,
    val awaitingPlayers : Map<String, Boolean>
)