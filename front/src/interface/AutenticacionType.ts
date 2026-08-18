
import type { LoginResponse } from "../interface/LoginType";

export interface AutenticacionType{
    user: LoginResponse | null;
    login: (userData: LoginResponse, token: string) => void;
    logout: () => void;
    
}