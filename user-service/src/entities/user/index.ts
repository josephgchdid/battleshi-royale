import  { Schema, model } from 'mongoose';
import IUser from '../../interfaces/user';

const UserSchema = new Schema<IUser>({
    name : {
        type: String,
        required: true
    },

    joinedOn : {
        type : Date,
        required: true
    }
})


const User = model<IUser>("User", UserSchema)

export default User