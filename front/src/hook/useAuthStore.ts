import type { LoginDtoResponse,AutenticacionType} from "../interface/LoginType";

import { create } from "zustand";

export const getInitialUser = (): LoginDtoResponse | null => {
    const user = localStorage.getItem("user");
    const token = localStorage.getItem("token");
    if(!user || !token) return null;

    try{
        return JSON.parse(user) as LoginDtoResponse;
    } catch{
        localStorage.removeItem("user");
        localStorage.removeItem("token");
        return null;
    } 
};

export const useAuthStore = create<AutenticacionType>((set) => ({
    user: getInitialUser(),
    token: localStorage.getItem("token"),

    login: (userData,token) => {
        localStorage.setItem("user", JSON.stringify(userData));
        localStorage.setItem("token", token);
        set({user: userData, token: token});
    },

    logout: () => {
        localStorage.removeItem("user");
        localStorage.removeItem("token");
        set({user: null, token: null});
    },
}));
 

