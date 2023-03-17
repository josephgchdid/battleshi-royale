package com.example.logicservice.service

import com.corundumstudio.socketio.SocketIOClient
import com.example.logicservice.entity.Message
import com.example.logicservice.entity.MessageType
import org.springframework.stereotype.Service


@Service
class SocketService {

    fun sendMessage(room: String?, eventName: String?, senderClient: SocketIOClient, message: String?) {
        for (client in senderClient.namespace.getRoomOperations(room).clients) {
            client.sendEvent(
                eventName,
                message?.let { Message(MessageType.SERVER, it, room) }
            )
        }
    }
}