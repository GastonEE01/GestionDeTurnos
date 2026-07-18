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
          </tr>
        </thead>
        <tbody>
          {data.map((local) => (
            <tr key={local.Id}>
              <td>{local.Id}</td>
              <td>{local.Nombre}</td>
              <td>{local.Descripcion}</td>
              <td>{local.Categoria}</td>
              <td>{local.Direccion}</td>
              <td>{local.Telefono}</td>
              <td>{local.ImagenURL}</td>
              <td></td>
              <td></td>
            </tr>
            
          ))}
        </tbody>
      </table>
    </div>
  )
}

