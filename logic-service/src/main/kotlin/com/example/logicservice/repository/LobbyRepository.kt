package com.example.logicservice.repository

import com.example.logicservice.entity.lobby.Lobby
import org.springframework.data.repository.CrudRepository
import org.springframework.stereotype.Repository

@Repository
interface LobbyRepository : CrudRepository<Lobby, String>{

}