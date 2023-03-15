package com.example.logicservice.service

import com.example.logicservice.entity.*
import com.example.logicservice.repository.BoardRepository
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.stereotype.Service

@Service
class LogicService(
   @Autowired  private val repository: BoardRepository
) {
       private final val mapper = jacksonObjectMapper()

       init {

        val board = Board(
            playerId = "2",
            id = "66efd05f-4c6d-4fa4-96bb-77c8b459c60f",
            gameId = "20aba1a2-16c8-47fe-b347-b53160887035",
            squares = arrayListOf(
                arrayListOf(
                    Coordinates(0, 0, false, false),
                    Coordinates(1, 0, false, false)
                ),
                arrayListOf(
                    Coordinates(0, 1, false, false),
                    Coordinates(1, 1, false, false)
                )
            ),

            ships = arrayListOf(
                Ship(
                    "some-uuid",
                    "destroyer",
                    2,
                    2,
                    arrayListOf(
                        Coordinates(0, 0, false, false),
                        Coordinates(1, 0, false, false)
                    )
                )
            )
        )

       val boardToFind  = repository.findById(board.playerId)

       if(boardToFind.isEmpty){
           save(board)
       }else {
           repository.deleteById(board.playerId)
           save(board)
       }
     }

     fun shootAtPlayer(missile: Missile) : Unit {

        val missileIsOutOfBounds = checkOutOfBoundsCoordinates(missile.coordinates.x, missile.coordinates.y);

        if(missileIsOutOfBounds){
            println("missile is out of bounds, did not hit anything")
            return;
        }

        //get the hit players board

        val cachedBoard = repository.findById(missile.destinationPlayer).get()

        val enemyBoard  = mapper.readValue(cachedBoard.Body, Board::class.java)


        //check if firing position has already been fired upon before, if so dont register shot
        val targetSquare = enemyBoard.squares[missile.coordinates.y][missile.coordinates.x]

         if(targetSquare.isBombed){
             println("this square is already bombed")
             return
         }

        //if it is, check if it has hit a ship
       val possibleTargetShip =  enemyBoard.ships.find { it.coordinates.any {
               coordinates ->  coordinates.x == missile.coordinates.x && coordinates.y == missile.coordinates.y
            }
       }
        //if it hasnt, mark that it has been shot and save the board
        // no target has been hit, mark it as a negative bombed space
        if(possibleTargetShip == null){

            enemyBoard.squares[missile.coordinates.y][missile.coordinates.x].apply {

                isBombed = true

                bombDidHit = false
            }

            save(enemyBoard)

            println("hit registered but it did not hit any valid targets")
            return
        }
        // if it has check if a player has sunk the ship
         enemyBoard.squares[missile.coordinates.y][missile.coordinates.x].apply {

             isBombed = true

             bombDidHit = true
         }

         println("you hit a ship! ")

         possibleTargetShip.squaresLeft--

         if(possibleTargetShip.squaresLeft == 0){

             print(" and you sank it! \n")
            enemyBoard.ships.remove(possibleTargetShip)
         }
        //if so check if they have any boats left, if not kick them out of the game for losing with a mean message

        save(enemyBoard)

         if(enemyBoard.ships.isNotEmpty()){
             return
         }

         println("the enemy has no ships left!")
        // cz
    }

    private fun checkOutOfBoundsCoordinates(x : Int , y : Int ) : Boolean {
        return  ( x < 0 || x >= 2 )  ||  ( y < 0 ||  y >= 2 )
    }

    private fun save(board:Board){

        val cached = CachedObject(
            board.playerId,
            mapper.writeValueAsString(board)

        )
        repository.save(cached)

    }
}