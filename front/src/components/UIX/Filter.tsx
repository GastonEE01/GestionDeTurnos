import React from 'react'
import { SlidersHorizontal } from 'lucide-react';

interface FilterProps {
    value: string;
    onChange: (e: React.ChangeEvent<HTMLSelectElement>) => void;
}

export const Filter : React.FC<FilterProps> = ({value, onChange}) => {
  return (
    <div className="relative w-full">
      <span className="absolute inset-y-0 left-0 flex items-center pl-3.5 pointer-events-none text-gray-400">
        <SlidersHorizontal size={16} />
      </span>

      <select name="FilterCategoria" className="w-full appearance-none bg-gray-50/50 border border-gray-200 text-gray-800 rounded-xl pl-10 pr-10 py-2.5 text-sm focus:outline-none focus:ring-2 focus:ring-slate-900 focus:bg-white transition-all shadow-sm cursor-pointer" id="" value={value} onChange={onChange}>
        <option value="">Elija una opcion</option>
        <option value="Peluquería y Barbería">Peluquería y Barbería</option>
        <option value="Estética y Spa">Estética y Spa</option>
        <option value="Canchas y Deportes">Canchas y Deportes</option>
        <option value="Salud y Odontología">Salud y Odontología</option>
        <option value="Gastronomía y Bares">Gastronomía y Bares</option>
        <option value="Indumentaria y Ropa">Indumentaria y Ropa</option>
        <option value="Veterinaria y Mascotas">Veterinaria y Mascotas</option>
        <option value="Taller Mecánico">Taller Mecánico</option>
        <option value="Talleres y Clases">Talleres y Clases</option>
      </select>
      <span className="absolute inset-y-0 right-0 flex items-center pr-3.5 pointer-events-none text-gray-400">
        ▼
      </span>
    </div>
  )
}

