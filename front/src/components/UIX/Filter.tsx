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
        <option value="Salud">Salud</option>
        <option value="Estética">Estética</option>
      </select>
    </div>
  )
}

