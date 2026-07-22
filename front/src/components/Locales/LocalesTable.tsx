import React from 'react'
import type {LocalesTableProps} from '../Locales/LocalesType.ts'

export const LocalesTable: React.FC<LocalesTableProps> = ({data }) => {
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
              <td></td>
            </tr>
            
          ))}
        </tbody>
      </table>
    </div>
  )
}

