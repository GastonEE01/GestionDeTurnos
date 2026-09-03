import React from 'react'
import { Search } from 'lucide-react';

interface SearchTextProps {
    value: string;
    onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
}

export const SearchText: React.FC<SearchTextProps> = ({value, onChange}) => {
  return (
    <div className="relative w-full">
      <span className="absolute inset-y-0 left-0 flex items-center pl-3.5 pointer-events-none text-gray-400">
      <Search size={18} />
      </span>
      <input type="text" className="w-full bg-gray-50/50 border border-gray-200 text-gray-800 rounded-xl pl-10 pr-4 py-2.5 text-sm focus:outline-none focus:ring-2 focus:ring-slate-900 focus:bg-white transition-all shadow-sm"placeholder='Escribe el nombre del local' value={value} onChange={onChange}/>
    </div>
  )
}

