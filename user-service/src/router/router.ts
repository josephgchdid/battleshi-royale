import express from "express";
import { UserService } from "../service/userService";

export class Router {

    public routes = express.Router()

    private service : UserService;

    constructor(){

        this.service = new UserService();

        this.routes.get('/profile', this.service.getUserProfile)

        this.routes.post('/create', this.service.createNewUser)
    }

}