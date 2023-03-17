package com.example.logicservice.entity

data class Message(
    val messageType : MessageType,
    val message : String,
    val game : String?,
)