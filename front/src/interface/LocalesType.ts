import type { ServicioType } from './ServicioType';
import type { HorarioAtencionType } from './HorarioAtencionType';
import type { LoginResponse } from './LoginType';


export interface LocalesType{
    id: string,
    name: string,
    description: string,
    category: string,
    imageURL: string,
    //title: string,
    direction: string,
    phone: string,
    // Relacion de local con la lista de servicios
    servicios: ServicioType[];
    horariosAtencion: HorarioAtencionType[];
}

// Representa las propiedades (props) que va a recibir tu componente Tabla
export interface LocalesTableProps {
  data: LocalesType[];
  user: LoginResponse;
  onEditar?: (id: number) => void;
  onEliminar?: (id: number) => void;
  onAgregarServicio?: (localId: number) => void;
  onDeleteSuccess?: (id: string) => void;
}

// DTO para la modificación (PUT)
export interface UpdateLocalDto {
  name: string;
  description: string;
  category: string;
  imageURL: string;
  //title: string;
  direction: string;
  phone: string;
}