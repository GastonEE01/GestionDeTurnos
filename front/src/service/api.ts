// component
import  {type TurnosType } from '../components/Turnos/TurnosType.ts'
import  {type LoginType} from '../components/Pages/Login.tsx'

// interface
import  {type LocalesType} from '../interface/LocalesType.ts'
import  {type UsuarioRegisterDto } from '../interface/UsuarioType.ts'
import  {type HorarioAtencionType } from '../interface/HorarioAtencionType.ts'
import { type LoginResponse } from '../interface/LoginType.ts'
import { type ServicioType } from '../interface/ServicioType.ts'

import type {UpdateLocalDto} from '../interface/LocalesType.ts'
const API_URL = import.meta.env.VITE_API_URL;

export const getLocales = async (): Promise<LocalesType[]>  => {
    const rest = await fetch(`${API_URL}/api/Local`,{
        method: 'GET',
        headers: {
            'Content-type' : 'application/json',
        },    
    });
    if(!rest.ok){
            const errorData = await rest.json().catch(() => ({}));
            throw new Error(errorData.message || 'Error al obtener los locales');
    }
    return rest.json();
};

export const deletedLocal = async (localId : string): Promise<LocalesType[]> => {
  const rest = await fetch(`${API_URL}/api/Local/${localId}`,{
    method: 'DELETE',
    headers: {
            'Content-type' : 'application/json',
        }, 
  });
   if(!rest.ok){
            const errorData = await rest.json().catch(() => ({}));
            throw new Error(errorData.message || 'Error al eliminar los locales');
    }
    return rest.json();
}

export const updateLocal = async (localId: string,localData: UpdateLocalDto): Promise<LocalesType> => {
  const rest = await fetch(`${API_URL}/api/Local/${localId}`,{
        method: 'PUT',
        headers: {
            'Content-type' : 'application/json',
        },   
        body: JSON.stringify(localData), 
    });
    if(!rest.ok){
            const errorData = await rest.json().catch(() => ({}));
            throw new Error(errorData.message || 'Error al actualizar el local');
    }
    return rest.json();
}

export const createLocal = async (localData: Omit<LocalesType, "id" | "servicios" | "horariosAtencion">,usuarioId: string,horarioData: HorarioAtencionType) => {
  // Armamos el cuerpo según lo que espera C# en el POST /api/Local
  const formatTime = (time: string) => (time.length === 5 ? `${time}:00` : time);
  const payloadLocal = {
    local: {
      usuarioId: usuarioId,
      name: localData.name,
      description: localData.description,
      category: localData.category,
      imageURL: localData.imageURL,
      //title: localData.title,
      direction: localData.direction,
      phone: localData.phone,
    },
    horarios: [
      {
        diaSemana: Number(horarioData.diaSemana),
        horaApertura: formatTime(horarioData.horaApertura),
        horaCierre: formatTime(horarioData.horaCierre),
        estaCerrado: horarioData.estaCerrado ?? false,
      },
    ], 
  };

  const rest = await fetch(`${API_URL}/api/Local`, {
    method: "POST", 
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(payloadLocal), // 👈 Mandamos la estructura envuelta
  });

  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    throw new Error(errorData.message || "Error al crear el local");
  }

  return rest.json();
};

export const getLocalesByUser = async (usuarioId: string): Promise<LocalesType[]>  => {
    const rest = await fetch(`${API_URL}/api/Local/usuario/${usuarioId}`,{
        method: 'GET',
        headers: {
            'Content-type' : 'application/json',
        },    
    });
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

export const addUsuarioLocal = async (
  userDto: UsuarioRegisterDto,
  localDto: Omit<LocalesType, "id">,
  horarioDto: HorarioAtencionType
) => {
  // 1. Registrar el usuario en C#
  const userPayload = { ...userDto, rol: "Local" };
  const resUser = await fetch(`${API_URL}/api/Registro`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(userPayload),
  });

  if (!resUser.ok) {
    const errorData = await resUser.json().catch(() => ({}));
    throw new Error(errorData.message || "Error al registrar el usuario");
  }

  // Obtenemos el ID del usuario creado
  const userData = await resUser.json();
  const usuarioIdCreado = userData.id;

  // 2. Armar el JSON del Local con ese usuarioId y los horarios
  const payloadLocal = {
    local: {
      usuarioId: usuarioIdCreado,
      name: localDto.name,
      description: localDto.description,
      category: localDto.category,
      imageURL: localDto.imageURL,
      //title: localDto.title,
      direction: localDto.direction,
      phone: localDto.phone,
    },
    horarios: [
      {
        diaSemana: horarioDto.diaSemana,
        horaApertura:
          horarioDto.horaApertura.length === 5
            ? `${horarioDto.horaApertura}:00`
            : horarioDto.horaApertura,
        horaCierre:
          horarioDto.horaCierre.length === 5
            ? `${horarioDto.horaCierre}:00`
            : horarioDto.horaCierre,
        estaCerrado: horarioDto.estaCerrado,
      },
    ],
  };

  // 3. Crear el Local en C#
  const resLocal = await fetch(`${API_URL}/api/Local`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(payloadLocal),
  });

  if (!resLocal.ok) {
    const errorData = await resLocal.json().catch(() => ({}));
    throw new Error(errorData.message || "Error al registrar el local");
  }

  return resLocal.json();
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

/*export interface LoginResponse{
    id: string,
    email: string,
    name: number,
    rol: number;

} */

    
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


export const getServiceLocal = async (localId: string) : Promise<ServicioType[]>  => {
    const rest = await fetch(`${API_URL}/api/Servicio/${localId}/servicios`,{
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

export const deleteService = async (localId: string, servicioId: string) : Promise<ServicioType[]>  => {
    const rest = await fetch(`${API_URL}/api/Servicio/${localId}/servicios/${servicioId}`,{
        method: 'DELETE',
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


export const updateService = async (localId: string, servicioId: string, servicioData: {
    name: string;
    description: string;
    durationInMinutes: string | number;
    price: number;
  }) : Promise<ServicioType[]>  => {

    // 1. Armamos el objeto asegurando que los tipos numéricos sean correctos
  const payload: ServicioDto = {
    name: servicioData.name,
    description: servicioData.description,
    durationInMinutes: Number(servicioData.durationInMinutes),
    price: Number(servicioData.price),
  };

    const rest = await fetch(`${API_URL}/api/Servicio/${localId}/servicios/${servicioId}`,{
        method: 'PUT',
        headers: {
            'Content-type' : 'application/json',
        },
        body: JSON.stringify(payload),
    });
    console.log(rest);

    if(!rest.ok){
            const errorData = await rest.json().catch(() => ({}));
            throw new Error(errorData.message || 'Error al obtener los locales');
    }
    return rest.json();
};

export interface ServicioDto {
  name: string;
  description: string;
  durationInMinutes: number;
  price: number;
}


export interface CreateServicioDto {
  usuarioId: string;
  name: string;
  description: string;
  durationInMinutes: number;
  price: number;
}

export const addService = async (localId: string, servicioData: CreateServicioDto ) : Promise<ServicioType> => {    
    const rest = await fetch(`${API_URL}/api/Servicio/${localId}/servicios`,{
        method: 'POST',
        headers: {
            'Content-type' : 'application/json',
        },
        body: JSON.stringify(servicioData)
    });
    if(!rest.ok){
        const errorData = await rest.json().catch(() => ({}));
            throw new Error(errorData.message || 'Error al registrarse');
    }
    return rest.json();
}