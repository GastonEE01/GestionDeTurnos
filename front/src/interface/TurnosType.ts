
export interface TurnosDtoRequest{
    date: string,
    servicioId: string,
    localId: string;
    usuarioId: string;
}

export interface TurnoDtoResponse{
  id: string,
  usuarioId: string; 
  localId: string; 
  servicioId: string; 
  date: string; 
}

export interface TurnosProps {
  onAgregarServicio?: (turno: TurnosDtoRequest) => void;
}

export interface TurnosTableProps{
  idUsuario: string;
}