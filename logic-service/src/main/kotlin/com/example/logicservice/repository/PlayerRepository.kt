package com.example.logicservice.repository

import com.example.logicservice.entity.game.CachedObject
import org.springframework.data.repository.CrudRepository
import org.springframework.stereotype.Repository

@Repository
interface PlayerRepository : CrudRepository<CachedObject, String> {

}