package com.example.logicservice.entity

import org.springframework.data.annotation.Id
import org.springframework.data.redis.core.RedisHash

@RedisHash
data class CachedObject(
    @Id
    val Id : String,

    val Body : String
)