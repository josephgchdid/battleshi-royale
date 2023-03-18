package com.example.logicservice.service

import com.example.logicservice.entity.game.*
import com.example.logicservice.entity.lobby.Player
import com.example.logicservice.repository.PlayerRepository
import com.fasterxml.jackson.module.kotlin.jacksonObjectMapper
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.stereotype.Service

@Service
class LogicService(
   @Autowired  private val repository: PlayerRepository
) {
       private final val mapper = jacksonObjectMapper()

       init {

        val player = Player(
            playerId = "6415b95807906b41da9aafc0",
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

       val playerToFind  = repository.findById(player.playerId)

       if(playerToFind.isEmpty){
           save(player)
       }else {
           repository.deleteById(player.playerId)
           save(player)
       }
     }

     fun shootAtPlayer(missileJson: String) : String {

        val missile: Missile = mapper.readValue(missileJson, Missile::class.java)

        val missileIsOutOfBounds = checkOutOfBoundsCoordinates(missile.coordinates.x, missile.coordinates.y);

        if(missileIsOutOfBounds){
            return serializeOutput(
                MissileFireResult(
                    result = Result.NO_HIT,
                    missile.originPlayer,
                    missile.destinationPlayer,
                    "Is the player blind ? missile is out of bounds, did not hit anything",
                )
            )
        }

        //get the hit players board

        val cachedBoard = repository.findById(missile.destinationPlayer).get()

        val enemyPlayer  = mapper.readValue(cachedBoard.Body, Player::class.java)


        //check if firing position has already been fired upon before, if so dont register shot
        val targetSquare = enemyPlayer.squares[missile.coordinates.y][missile.coordinates.x]

         if(targetSquare.isBombed){

             return serializeOutput(
                 MissileFireResult(
                     result = Result.ALREADY_BOMBED,
                     missile.originPlayer,
                     missile.destinationPlayer,
                     "The targeted square is already bombed, no hit",
                 )
             )
         }

        //if it is, check if it has hit a ship
       val possibleTargetShip =  enemyPlayer.ships.find { it.coordinates.any {
               coordinates ->  coordinates.x == missile.coordinates.x && coordinates.y == missile.coordinates.y
            }
       }
        //if it hasnt, mark that it has been shot and save the board
        // no target has been hit, mark it as a negative bombed space
        if(possibleTargetShip == null){

            enemyPlayer.squares[missile.coordinates.y][missile.coordinates.x].apply {

                isBombed = true

                bombDidHit = false
            }

            save(enemyPlayer)

            return serializeOutput(
                MissileFireResult(
                    result = Result.ALREADY_BOMBED,
                    missile.originPlayer,
                    missile.destinationPlayer,
                    "Hit registered but it did not hit any valid targets ( LOL )",
                )
            )
        }
        // if it has check if a player has sunk the ship
         enemyPlayer.squares[missile.coordinates.y][missile.coordinates.x].apply {

             isBombed = true

             bombDidHit = true
         }

         var message = "A ${possibleTargetShip.shipType.uppercase()} was hit! ( finally )"

         var shipDidSink : Boolean = false

         var playerDidLose = false

         var result = Result.SHIP_HIT

         possibleTargetShip.squaresLeft--

         if(possibleTargetShip.squaresLeft == 0){

             enemyPlayer.ships.remove(possibleTargetShip)

             playerDidLose = enemyPlayer.ships.isEmpty()

             shipDidSink = true

             result = if(playerDidLose) Result.PLAYER_ELIMINATED else Result.SHIP_SUNK

             message += "and was sank!\n ${if(playerDidLose) "Hit player took an L and has no ships left, they are out of the game LMAO" else ""}"

         }

        //if so check if they have any boats left, if not kick them out of the game for losing with a mean message

        save(enemyPlayer)

         return serializeOutput(
             MissileFireResult(
                 result = result,
                 missile.originPlayer,
                 missile.destinationPlayer,
                 message,
                 shipSunk = shipDidSink,
                 playerDidLose = playerDidLose
             )
         )
    }

    private fun checkOutOfBoundsCoordinates(x : Int , y : Int ) : Boolean {
        return  ( x < 0 || x >= 2 )  ||  ( y < 0 ||  y >= 2 )
    }

    private fun save(player: Player){

        val cached = CachedObject(
            player.playerId,
            mapper.writeValueAsString(player)

        )
        repository.save(cached)

    }

    private fun serializeOutput(result : MissileFireResult) : String {
        return mapper.writeValueAsString(result)
    }


}