package com.example.logicservice.repository

import com.example.logicservice.entity.Lobby
import org.springframework.data.repository.CrudRepository
import org.springframework.stereotype.Repository

@Repository
interface LobbyRepository : CrudRepository<Lobby, String>{

    fun countByGameId(id:String) : Long;
}