import type { ServicioType } from '../Servicio/ServicioType';
export interface LocalesType{
    id: number,
    name: string,
    description: string,
    category: string,
    imageURL: string,
    title: string,
    direction: string,
    phone: string,
    // Relacion de local con la lista de servicios
    servicios: ServicioType[];
}

// Representa las propiedades (props) que va a recibir tu componente Tabla
export interface LocalesTableProps {
  data: LocalesType[];
  onEditar?: (id: number) => void;
  onEliminar?: (id: number) => void;
  onAgregarServicio?: (localId: number) => void;
}