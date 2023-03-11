import * as dotenv from 'dotenv';
import mongoose, { ConnectOptions } from 'mongoose';
import path from 'path';

export default class Connection {

    private connectionString : string

    constructor(){

        dotenv.config({ path : path.join(__dirname , '.env')})

        this.connectionString = process.env.DATABASE_URI || "";

        console.log(this.connectionString)
    }


    connectToDataBase =  () => {
       mongoose.connect(this.connectionString, {
            useNewUrlParser: true,
            useUnifiedTopology: true,
        
        } as ConnectOptions, 
        
        ).then((a) => console.log('connected to the db'))
        .catch((e) => console.error(e.message))
    }
}