import { Request, Response } from "express"
import mongoose from "mongoose"
import User from "../entities/user"
import IUser from "../interfaces/user"
import Auth from "../util/auth"

export class UserService {
    
    auth:Auth 

    public constructor(){
        this.auth = new Auth()
    }

    public getUserProfile = async (req: Request, res: Response) => {
        
        var token = req.headers['authorization'] || "";
        
        var decodedToken = this.auth.verifyToken(token)
        
        if(decodedToken === null){
            return res.status(401)
        }
      
        var id = new mongoose.Types.ObjectId(decodedToken.userId || ""); 
       
        let user = await new User().collection.findOne({ _id : id})

        return res.json(user)
    }


    public createNewUser = async (req: Request, res: Response) => {

        let userReq = req.body as IUser

        let user = new User()
    
        user.name = userReq.name.trim()
   
        user.joinedOn = new Date()
   
        var totalUsers = await user.collection.countDocuments({ name : userReq.name.trim()})

        if(totalUsers > 0){
            return res.status(400).json( { message : "username taken", httpStatus : 400 })
        }

        await user.save()

        let payload = { 
            expiresIn : "30d",
            userId : user.id,
            userName : user.name
        }

        var token = this.auth.generateToken(payload)

        return res.status(200).json( { data  : {token : token}, httpStatus : 200})
    }
}
