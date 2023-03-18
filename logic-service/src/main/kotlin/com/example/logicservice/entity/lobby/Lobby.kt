package com.example.logicservice.entity

import org.springframework.data.annotation.Id
import org.springframework.data.redis.core.RedisHash
import org.springframework.data.redis.core.index.Indexed


@RedisHash("lobby")
data class Lobby(
    @Id
    @Indexed
    val gameId : String,
    val awaitingPlayers : Map<String, Boolean>
)