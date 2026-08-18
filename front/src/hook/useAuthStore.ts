//  interface
import type {AutenticacionType} from '../interface/AutenticacionType';
import type { LoginResponse } from "../interface/LoginType";

import { create } from "zustand";


export const getInitialUser = (): LoginResponse | null => {
    const user = localStorage.getItem("user");
    const token = localStorage.getItem("token");
    if(!user || !token) return null;

    try{
        return JSON.parse(user) as LoginResponse;
    } catch{
        return JSON.parse(user) as LoginResponse;
    } 
};

export const useAuthStore = create<AutenticacionType>((set) => ({
    user: getInitialUser(),

    login: (userData,token) => {
        localStorage.setItem("user", JSON.stringify(userData));
        localStorage.setItem("token", token);
        set({user: userData});
    },

    logout: () => {
        localStorage.removeItem("user");
        localStorage.removeItem("token");
        set({user: null });
    },
}));
 

