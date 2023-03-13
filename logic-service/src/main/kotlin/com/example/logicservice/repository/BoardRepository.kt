package com.example.logicservice.repository

import com.example.logicservice.entity.Board
import org.springframework.data.repository.CrudRepository
import org.springframework.stereotype.Repository

@Repository
interface BoardRepository : CrudRepository<Board, String> {
}