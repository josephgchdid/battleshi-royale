package com.example.logicservice.config

import com.corundumstudio.socketio.SocketIOServer
import org.springframework.beans.factory.annotation.Value
import org.springframework.context.annotation.Bean
import org.springframework.context.annotation.Configuration
import java.io.Console

@Configuration
class SocketIoConfig {

    @Value("\${socket-server.host}")
    lateinit var host : String


    @Value("\${socket-server.port}")
    lateinit var serverPort : Integer

    @Bean
    fun socketIOServer() : SocketIOServer {

        val config : com.corundumstudio.socketio.Configuration = com.corundumstudio.socketio.Configuration()

        config.apply {
            hostname = host

            port = serverPort.toInt()
        }

        return SocketIOServer(config)
    }
}