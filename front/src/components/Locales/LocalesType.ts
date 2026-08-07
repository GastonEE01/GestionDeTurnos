import type { ServicioType } from '../Servicio/ServicioType';


export const DayOfWeek = {
  0: 'Domingo',
  1: 'Lunes',
  2: 'Martes',
  3: 'Miércoles',
  4: 'Jueves',
  5: 'Viernes',
  6: 'Sábado'
} as const;

export interface HorarioAtencionType {
    localId: string;
    diaSemana: keyof typeof DayOfWeek;
    horaApertura: string; // Formato "HH:mm"
    horaCierre: string;  // Formato "HH:mm"
    estaCerrado: boolean;
}
export interface LocalesType{
    id: string,
    name: string,
    description: string,
    category: string,
    imageURL: string,
    title: string,
    direction: string,
    phone: string,
    // Relacion de local con la lista de servicios
    servicios: ServicioType[];
    horariosAtencion: HorarioAtencionType[];
}

// Representa las propiedades (props) que va a recibir tu componente Tabla
export interface LocalesTableProps {
  data: LocalesType[];
  usuarioId: string;
  onEditar?: (id: number) => void;
  onEliminar?: (id: number) => void;
  onAgregarServicio?: (localId: number) => void;
}