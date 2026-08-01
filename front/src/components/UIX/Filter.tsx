import React from 'react'

interface FilterProps {
    value: string;
    // Tipo exacto para el evento de cambio de un input en react
    onChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
}

export const Filter : React.FC<FilterProps> = ({value, onChange}) => {
  return (
    <div>
      <select name="FilterCategoria" id="" value={value} onChange={onChange}>
        <option value="">Elija una opcion</option>
        <option value="Salud y Medicina">Salud y Medicina</option>
        <option value="Gastronomía">Gastronomía</option>
        <option value="Mascotas">Mascotas</option>
      </select>
    </div>
  )
}

