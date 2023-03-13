import jwt from 'jsonwebtoken';
import fs from 'fs'
import path from 'path';

export default class Auth {

    
    key: jwt.Secret


    constructor(){
        this.key = fs.readFileSync(path.join(__dirname , 'ship.key'));
    }

    public generateToken = (payload : any) : string => {

        var token = jwt.sign(payload, 'some-key')

        return token;
    }

}