package com.example.logicservice.service

import com.example.logicservice.entity.Missile
import com.example.logicservice.repository.BoardRepository
import org.springframework.stereotype.Service

@Service
class LogicService(
    private val repository: BoardRepository
) {


     fun shootAtPlayer(missile: Missile) : Unit {

        val missileIsOutOfBounds = checkOutOfBoundsCoordinates(missile.coordinates.x, missile.coordinates.y);

        if(missileIsOutOfBounds){
            return;
        }

        //get the hit players board

        val enemyBoard = repository.findById(missile.destinationPlayer).get()


        //check if firing position has already been fired upon before, if so dont register shot
        val targetSquare = enemyBoard.squares[missile.coordinates.y][missile.coordinates.x]

         if(targetSquare.isBombed){
             return
         }

        //if it is, check if it has hit a ship
       val possibleTargetShip =  enemyBoard.ships.find {  it -> it.coordinates.any {
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

            repository.save(enemyBoard)

            return
        }
        // if it has check if a player has sunk the ship
         enemyBoard.squares[missile.coordinates.y][missile.coordinates.x].apply {

             isBombed = true

             bombDidHit = true
         }

         possibleTargetShip.squaresLeft--

         if(possibleTargetShip.squaresLeft == 0){
            enemyBoard.ships.remove(possibleTargetShip)
         }
        //if so check if they have any boats left, if not kick them out of the game for losing with a mean message

         repository.save(enemyBoard)

         if(enemyBoard.ships.isNotEmpty()){
             return
         }


        // cz
    }

    private fun checkOutOfBoundsCoordinates(x : Int , y : Int ) : Boolean {
        return  ( x < 0 || x >= 7 )  ||  ( y < 0 ||  y >= 7 )
    }
}