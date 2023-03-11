import express, { Express } from 'express';
import { Router } from './router/router';
import * as dotenv from 'dotenv';
import path from 'path';
import { Constants } from './util/constants';
import Connection from './database/connection';
class Server {


    app: Express  = express();

    router: Router; 

    connection:Connection

    constructor(){

        this.connection = new Connection()

        this.connection.connectToDataBase()
        
        dotenv.config({ path : path.join(__dirname , '.env')})

        this.router = new Router();

        this.app.use(express.json())

        this.app.use(Constants.BASE_URL,this.router.routes)

        this.app.listen(
            process.env.PORT, 
            () => console.log(`⚡️ server is running on port ${process.env.PORT}⚡️`)
        )
    }
}


let srv:Server = new Server()