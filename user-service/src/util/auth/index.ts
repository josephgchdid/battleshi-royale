import jwt from 'jsonwebtoken';
import fs from 'fs'
import path from 'path';
import IToken from '../../interfaces/token';

export default class Auth {

    key: jwt.Secret

    constructor(){
        this.key = fs.readFileSync(path.join(__dirname , 'ship.key'));
    }

    public generateToken = (payload : any) : string => {

        var token = jwt.sign(payload, this.key)
        
        return token;
    }   

    public verifyToken = (token: string) : IToken | null => {

      try{
    
        var decoded =  jwt.verify(token, this.key) as IToken
        
        return decoded
     
    }catch(_){
        return null
      }
    }

}