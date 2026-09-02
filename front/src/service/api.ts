// component
import { type TurnosDtoRequest } from "../interface/TurnosType.ts";
import { type TurnoDtoResponse } from "../interface/TurnosType.ts";

// interface
import { type LocalesType } from "../interface/LocalesType.ts";
import { type UsuarioRegisterDto } from "../interface/UsuarioType.ts";
import { type HorarioAtencionType } from "../interface/HorarioAtencionType.ts";

import { type LoginDtoRequest } from "../interface/LoginType.ts";
import { type LoginDtoResponse } from "../interface/LoginType.ts";

//import { type ServicioType } from '../interface/ServicioType.ts'
import { type ServicioDtoRequest } from "../interface/ServicioType.ts";
import { type ServicioDtoResponse } from "../interface/ServicioType.ts";

import type { UpdateLocalDto } from "../interface/LocalesType.ts";
const API_URL = import.meta.env.VITE_API_URL;

export interface ApiResponse<T = void> {
  message: string;
  data?: T;
}

// LOCAL
export const getLocales = async (): Promise<LocalesType[]> => {
  const rest = await fetch(`${API_URL}/api/Local`, {
    method: "GET",
    headers: {
      "Content-type": "application/json",
    },
  });
  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    throw new Error(errorData.message || "Error al obtener los locales");
  }
  return rest.json();
};

export const deletedLocal = async (localId: string): Promise<ApiResponse> => {
  const rest = await fetch(`${API_URL}/api/Local/${localId}`, {
    method: "DELETE",
    headers: {
      "Content-type": "application/json",
    },
  });
  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    throw new Error(errorData.message || "Error al eliminar los locales");
  }
  return rest.json();
};

export const updateLocal = async (
  localId: string,
  localData: UpdateLocalDto,
): Promise<LocalesType & ApiResponse> => {
  const rest = await fetch(`${API_URL}/api/Local/${localId}`, {
    method: "PUT",
    headers: {
      "Content-type": "application/json",
    },
    body: JSON.stringify(localData),
  });
  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    throw new Error(errorData.message || "Error al actualizar el local");
  }
  return rest.json();
};

export const createLocal = async (
  localData: Omit<LocalesType, "id" | "servicios" | "horariosAtencion">,
  usuarioId: string,
  horarioData: HorarioAtencionType,
) => {
  const formatTime = (time: string) =>
    time.length === 5 ? `${time}:00` : time;
  const payloadLocal = {
    local: {
      usuarioId: usuarioId,
      name: localData.name,
      description: localData.description,
      category: localData.category,
      imageURL: localData.imageURL,
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
    body: JSON.stringify(payloadLocal),
  });

  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    const message =
      errorData.message ||
      errorData.Message ||
      errorData.detail ||
      errorData.title ||
      "Error al registrar el local";
      throw new Error(message);
  }

  return rest.json();
};

export const getLocalesByUser = async (
  usuarioId: string,
): Promise<LocalesType[]> => {
  const rest = await fetch(`${API_URL}/api/Local/usuario/${usuarioId}`, {
    method: "GET",
    headers: {
      "Content-type": "application/json",
    },
  });
  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    throw new Error(errorData.message || "Error al obtener los locales");
  }
  return rest.json();
};

// TURNO
export const addTurno = async (
  nuevoTurno: Omit<TurnosDtoRequest, "id">,
): Promise<TurnoDtoResponse & ApiResponse> => {
  const rest = await fetch(`${API_URL}/api/Turno`, {
    method: "POST",
    headers: {
      "Content-type": "application/json",
    },
    body: JSON.stringify(nuevoTurno),
  });

  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    const errorMessage =
      (typeof errorData === "string" ? errorData : null) ||
      errorData?.message ||
      errorData?.Message ||
      "Error al cancelar el turno ";
    throw new Error(errorMessage);
  }

  /*
       if(!rest.ok){
            const errorData = await rest.json().catch(() => ({}));
          const errorMessage = 
    (typeof errorData === 'string' ? errorData : null) ||
    errorData?.message || 
    errorData?.Message || 
    'Error al eliminar un servicio';

  throw new Error(errorMessage);
}

if (rest.status === 204) {
    return { message: "Servicio eliminado exitosamente" } as ApiResponse;
  }
    */
  return rest.json();
};

export const deleteTurno = async (
  turnoId: string,
  usuarioId: string,
): Promise<ApiResponse> => {
  const rest = await fetch(
    `${API_URL}/api/Turno/${turnoId}/cancelar?usuarioId=${usuarioId}`,
    {
      method: "DELETE",
      headers: {
        "Content-type": "application/json",
      },
    },
  );

  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    const errorMessage =
      errorData.message ||
      errorData.title ||
      errorData.detail ||
      "Error al cancelar el turno";

    throw new Error(errorMessage);
  }

  return rest.json();
};

// HORARIO ATENCION
export const getHorarioAtencionDisponible = async (
  localId: string,
  servicioId: string,
  fecha: string,
): Promise<string[]> => {
  const fechaISO = fecha.includes("T") ? fecha : `${fecha}T00:00:00Z`;

  const rest = await fetch(
    `${API_URL}/api/Turno/Disponibles?localId=${localId}&servicioId=${servicioId}&fecha=${encodeURIComponent(fechaISO)}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    },
  );

  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    console.error("Detalle del error 400 devuelto por C#:", errorData);
    const mensajeError =
      errorData.detail ||
      errorData.title ||
      errorData.message ||
      "Error al obtener los horarios de atención";
    throw new Error(mensajeError);
  }

  return rest.json();
};

export const getHorarioAtencionUsuario = async (
  usuarioId: string,
): Promise<TurnoDtoResponse[]> => {
  const rest = await fetch(`${API_URL}/api/Turno/usuario/${usuarioId}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  });

  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    throw new Error(errorData.message || "Error al obtener los turnos");
  }
  const data = await rest.json();
  return Array.isArray(data.turnos) ? data.turnos : [];
};

// Registro
export const addUsuarioLocal = async (
  userDto: UsuarioRegisterDto,
  localDto: Omit<LocalesType, "id">,
  horarioDto: HorarioAtencionType,
) => {
  const payload = {
    user: { ...userDto, rol: "Local" },
    local: {
      name: localDto.name,
      description: localDto.description,
      category: localDto.category,
      imageURL: localDto.imageURL,
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

  const res = await fetch(`${API_URL}/api/Registro/registro-local`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });

  if (!res.ok) {
    const errorData = await res.json().catch(() => ({}));

    const mensajeError =
      errorData.Message ||
      errorData.message ||
      (errorData.errors
        ? Object.values(errorData.errors).flat().join(", ")
        : null) ||
      "Error al registrar el local";

    throw new Error(mensajeError);
  }

  return res.json();
};

export const addUsuario = async (
  credentials: UsuarioRegisterDto,
): Promise<{ message: string }> => {
  const rest = await fetch(`${API_URL}/api/Registro`, {
    method: "POST",
    headers: {
      "Content-type": "application/json",
    },
    body: JSON.stringify(credentials),
  });
  console.log("credenciales", credentials);
  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    const messageError =
      errorData.Message || errorData.message || "Error al registro";
    throw new Error(messageError);
  }
  return rest.json();
};

// LOGIN
export const loginUser = async (
  credentials: LoginDtoRequest,
): Promise<LoginDtoResponse> => {
  const rest = await fetch(`${API_URL}/api/JWTService/Login`, {
    method: "POST",
    headers: {
      "Content-type": "application/json",
    },
    body: JSON.stringify(credentials),
  });
  console.log(rest);

  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    const errorMessage =
      (typeof errorData === "string" ? errorData : null) ||
      errorData?.message ||
      errorData?.Message ||
      "Error al iniciar sesión";

    throw new Error(errorMessage);
  }

  return rest.json();
};

// SERVICIO
export const getServiceLocal = async (
  localId: string,
): Promise<ServicioDtoResponse[]> => {
  const rest = await fetch(`${API_URL}/api/Servicio/${localId}/servicios`, {
    method: "GET",
    headers: {
      "Content-type": "application/json",
    },
  });
  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    throw new Error(errorData.message || "Error al obtener los locales");
  }
  return rest.json();
};

export const deleteService = async (
  localId: string,
  servicioId: string,
): Promise<ApiResponse> => {
  const rest = await fetch(
    `${API_URL}/api/Servicio/${localId}/servicios/${servicioId}`,
    {
      method: "DELETE",
      headers: {
        "Content-type": "application/json",
      },
    },
  );
  console.log(rest);

  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    const errorMessage =
      (typeof errorData === "string" ? errorData : null) ||
      errorData?.message ||
      errorData?.Message ||
      "Error al eliminar un servicio";

    throw new Error(errorMessage);
  }

  if (rest.status === 204) {
    return { message: "Servicio eliminado exitosamente" } as ApiResponse;
  }

  return rest.json();
};

export const updateService = async (
  localId: string,
  servicioId: string,
  servicioData: {
    name: string;
    description: string;
    durationInMinutes: string | number;
    price: number;
  },
): Promise<ServicioDtoRequest[]> => {
  const payload: ServicioDto = {
    name: servicioData.name,
    description: servicioData.description,
    durationInMinutes: Number(servicioData.durationInMinutes),
    price: Number(servicioData.price),
  };

  const rest = await fetch(
    `${API_URL}/api/Servicio/${localId}/servicios/${servicioId}`,
    {
      method: "PUT",
      headers: {
        "Content-type": "application/json",
      },
      body: JSON.stringify(payload),
    },
  );
  console.log(rest);

  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    throw new Error(errorData.message || "Error al obtener los locales");
  }
  return rest.json();
};

export const addService = async (
  localId: string,
  servicioData: ServicioDtoRequest,
): Promise<ServicioDtoResponse & ApiResponse> => {
  const rest = await fetch(`${API_URL}/api/Servicio/${localId}/servicios`, {
    method: "POST",
    headers: {
      "Content-type": "application/json",
    },
    body: JSON.stringify(servicioData),
  });
  if (!rest.ok) {
    const errorData = await rest.json().catch(() => ({}));
    const messageError =
      errorData.Message || errorData.message || "Error al registro";
    throw new Error(messageError);
  }
  return rest.json();
};

export interface ServicioDto {
  name: string;
  description: string;
  durationInMinutes: number;
  price: number;
}
