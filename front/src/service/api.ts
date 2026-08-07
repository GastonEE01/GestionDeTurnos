import  {type LocalesType} from '../components/Locales/LocalesType.ts'
import  {type TurnosType } from '../components/Turnos/TurnosType.ts'
import  {type UsuarioRegisterDto } from '../components/Pages/Registro.tsx'
import  {type LoginType} from '../components/Pages/Login.tsx'
const API_URL = import.meta.env.VITE_API_URL;

export const getLocales = async (): Promise<LocalesType[]>  => {
    const rest = await fetch(`${API_URL}/api/Local`,{
        method: 'GET',
        headers: {
            'Content-type' : 'application/json',
        },
    
    });
console.log(rest);
    if(!rest.ok){
            const errorData = await rest.json().catch(() => ({}));
            throw new Error(errorData.message || 'Error al obtener los locales');
    }
    return rest.json();
};

export const addTurno = async (nuevoTurno: Omit<TurnosType, 'id'>): Promise<{ id: string; date: string; message: string }> => {    const rest = await fetch(`${API_URL}/api/Turno`,{
        method: 'POST',
        headers: {
            'Content-type' : 'application/json',
        },
         body: JSON.stringify(nuevoTurno),
        });

    if(!rest.ok){
        const errorData = await rest.json().catch(() => ({}));
        throw new Error(errorData.message || 'Error al agregar un turno ')
    }
    return rest.json();
};


export const addUsuario = async (credentials: UsuarioRegisterDto ) : Promise<{ message: string }> => {    
    const rest = await fetch(`${API_URL}/api/Registro`,{
        method: 'POST',
        headers: {
            'Content-type' : 'application/json',
        },
        body: JSON.stringify(credentials),
    });
    console.log("credenciales" ,credentials)
    if(!rest.ok){
        const errorData = await rest.json().catch(() => ({}));
            throw new Error(errorData.message || 'Error al registrarse');
    }
    return rest.json();
}

export interface LoginResponse{
    id: string,
    email: string,
    name: number,
    rol: number;

} 
export const loginUser = async (credentials: LoginType) : Promise<Promise<{ token: string,response: LoginResponse }>>  => {
    const rest = await fetch(`${API_URL}/api/JWTService/Login`,{
        method: 'POST',
        headers: {
            'Content-type' : 'application/json',
        },
        body: JSON.stringify(credentials), 
    });
    console.log(rest);

    if(!rest.ok){
            const errorData = await rest.json().catch(() => ({}));
            throw new Error(errorData.message || 'Error al obtener los locales');
    }
    return rest.json();
};


