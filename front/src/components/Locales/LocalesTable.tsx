import React, { useState } from 'react'
import type {LocalesTableProps, LocalesType} from '../Locales/LocalesType.ts'
import {ModalTurno} from '../Turnos/ModalTurno.tsx'
export const LocalesTable: React.FC<LocalesTableProps> = ({data,usuarioId }) => {
   const [selectedLocal, setSelectedLocal] = useState<LocalesType | null>(null);
  return (
    <div>
      <table>
        <thead>
          <tr>
            <th>Id</th>
            <th>Nombre</th>
            <th>Descripcion</th>
            <th>Categoria</th>
            <th>ImagenURL</th>
            <th>Direccion</th>
            <th>Telefono</th>
            <th>Servicios</th>
            <th>Acciones</th>
            </tr>
           
        </thead>
        <tbody>
          {data.map((local) => (
            <tr key={local.id}>
              <td>{local.id}</td>
              <td>{local.name}</td>
              <td>{local.description}</td>
              <td>{local.category}</td>
              <td>{local.direction}</td>
              <td>{local.phone}</td>
              <td>{local.imageURL}</td>
              <td>{local.servicios.length > 0 
                ? local.servicios.map(s => s.name).join(", ") : "Sin servicios"}</td>
              <button onClick={() => setSelectedLocal(local)}>Pedir turno</button>
            </tr>
          ))}
        </tbody>
      </table>
      {selectedLocal !== null && (
     <ModalTurno cerrar={() => setSelectedLocal(null)} usuarioId={usuarioId} localId={selectedLocal.id} servicios={selectedLocal.servicios}/> 
  )}
    </div>
  )
}

