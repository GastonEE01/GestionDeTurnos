import type { ServicioDtoResponse } from './ServicioType';
import type { HorarioAtencionType } from './HorarioAtencionType';
import type { LoginDtoResponse } from './LoginType';


export interface LocalesType{ 
    id: string,
    name: string,
    description: string,
    category: string,
    imageURL: string,
    direction: string,
    phone: string,
    servicios: ServicioDtoResponse[];
    horariosAtencion: HorarioAtencionType[];
}

export interface LocalesTableProps {
  data: LocalesType[];
  user: LoginDtoResponse;
  onEditar?: (id: number) => void;
  onEliminar?: (id: number) => void;
  onAgregarServicio?: (localId: number) => void;
  onDeleteSuccess?: (id: string) => void;
  onServiceSuccess?: () => void;
}

export interface UpdateLocalDto { 
  name: string;
  description: string;
  category: string;
  imageURL: string;
  direction: string;
  phone: string;
}