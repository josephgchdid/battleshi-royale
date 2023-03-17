package com.example.logicservice.service

import com.example.logicservice.entity.Lobby
import com.example.logicservice.repository.LobbyRepository
import org.springframework.beans.factory.annotation.Autowired

class LobbyService(
   @Autowired  private val lobbyRepository: LobbyRepository
) {


    fun createLobby(gameId : String, players : ArrayList<String>, initialPlayerId : String ) {

        val count = lobbyRepository.countByGameId(gameId)

        if(count.toInt() != 0){
            return // a game lobby has already been created
        }

        val awaitingPlayers = mutableMapOf<String, Boolean>()

        for (playerId : String in players){

            awaitingPlayers[playerId] = playerId == initialPlayerId
        }

        val gameLobby = Lobby(gameId, awaitingPlayers)

        lobbyRepository.save(gameLobby)
    }

    fun admitPlayerIntoLobby(gameId : String, playerId : String ) : Boolean {

        val lobby = lobbyRepository.findById(gameId)

        if(lobby.isEmpty){
            return false
        }
        return lobby.get().awaitingPlayers.containsKey(playerId)
    }
}