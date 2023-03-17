package com.example.logicservice.config

import com.corundumstudio.socketio.SocketIOServer
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.boot.CommandLineRunner
import org.springframework.stereotype.Component

@Component
class ServerRunner(@Autowired private val server : SocketIOServer)  : CommandLineRunner{

    override fun run(vararg args: String?) {
       server.start()
    }
}