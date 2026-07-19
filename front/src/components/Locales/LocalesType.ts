

export interface LocalesType{
    id: number,
    name: string,
    description: string,
    category: string,
    imageURL: string,
    title: string,
    direction: string,
    phone: string
}

// Representa las propiedades (props) que va a recibir tu componente Tabla
export interface LocalesTableProps {
  data: LocalesType[];
  onEditar?: (id: number) => void;
  onEliminar?: (id: number) => void;
}