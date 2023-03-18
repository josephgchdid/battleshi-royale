package com.example.logicservice.service

import com.example.logicservice.entity.lobby.Lobby
import com.example.logicservice.repository.LobbyRepository
import com.fasterxml.jackson.core.type.TypeReference
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.stereotype.Service
import java.util.*

@Service
class LobbyService(
   @Autowired  private val lobbyRepository: LobbyRepository
) {

    private val mapper = jacksonObjectMapper()

    fun createLobby(gameId : String, players : ArrayList<String>, initialPlayerId : String ) {

       val count = lobbyRepository.findById(gameId)

       if(count.isPresent){
            return // a game lobby has already been created
        }

        val awaitingPlayers = mutableMapOf<String, Boolean>()

        for (playerId : String in players){

            awaitingPlayers[playerId] = playerId == initialPlayerId
        }


        val gameLobby = Lobby(gameId, awaitingPlayers)

        lobbyRepository.save(gameLobby)
    }

    fun admitPlayerIntoLobby(gameId : String, token : String ) : Boolean {


        val lobby = lobbyRepository.findById(gameId)

        if(lobby.isEmpty){
            return false
        }

        val decodedToken : Map<String, String>? = mapper.readValue(decodeToken(token), object : TypeReference<Map<String, String>>() {})

        val playerId = decodedToken?.get("userId") ?: ""

        return lobby.get().awaitingPlayers.containsKey(playerId)
    }

    private fun decodeToken(jwt : String) : String {

        val parts = jwt.split(".")

        return try {

            val charset = charset("UTF-8")

            String(Base64.getUrlDecoder().decode(parts[1].toByteArray(charset)), charset)

        } catch (e: Exception) {
            "{}"
        }
    }
}