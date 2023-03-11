import { Request, Response } from "express"

export class UserService {
 
    public constructor(){

    }

    public getUserById = (req: Request, res: Response) => {
        res.json( { message : 'hello world'})
    }
}