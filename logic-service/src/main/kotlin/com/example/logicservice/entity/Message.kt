package com.example.logicservice.entity

import com.example.logicservice.entity.MessageType

data class Message(
    val messageType : MessageType,
    val message : String,
    val game : String?,
)