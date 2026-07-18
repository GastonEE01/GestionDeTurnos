import React from 'react'

interface SearchTextProps {
    value: string;
    // Tipo exacto para el evento de cambio de un input en react
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

export const SearchText: React.FC<SearchTextProps> = ({value, onChange}) => {
  return (
    <div>
      <input type="text" placeholder='Escribe el nombre del local' value={value} onChange={onChange}/>
    </div>
  )
}

