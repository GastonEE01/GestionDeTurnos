

export interface LocalesType{
    Id: number,
    Nombre: string,
    Descripcion: string,
    Categoria: string,
    ImagenURL: string,
    Direccion: string,
    Telefono: string
}

// Representa las propiedades (props) que va a recibir tu componente Tabla
export interface LocalesTableProps {
  data: LocalesType[];
  onEditar?: (id: number) => void;
  onEliminar?: (id: number) => void;
}