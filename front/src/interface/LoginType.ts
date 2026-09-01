
export interface LoginDtoRequest {
  email: string;
  password: string;
}

export interface LoginDtoResponse{
    id: string,
    email: string,
    name: string,
    rol: string;
    token: string;
} 

export interface AutenticacionType{
    user: LoginDtoResponse | null;
    login: (userData: LoginDtoResponse, token: string) => void;
    logout: () => void; 
}