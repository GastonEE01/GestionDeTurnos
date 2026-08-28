//import type { LocalesType } from "../Locales/LocalesType";

export interface TurnosType{
    id: string,
    date: string,
    servicioId: string,
    localId: string;
    usuarioId: string;
}

export interface TurnosProps {
  onAgregarServicio?: (turno: TurnosType) => void;
}

export interface TurnosTableProps{
  idUsuario: string;
}