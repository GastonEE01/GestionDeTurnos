

export interface ServicioDtoResponse{
    id: string,
    usuarioId: string,
    name: string,
    description: string,
    durationInMinutes: number,
    price: number,
    localId: string;
}

export interface ServicioDtoRequest {
  usuarioId: string;
  name: string;
  description: string;
  durationInMinutes: number;
  price: number;
}

