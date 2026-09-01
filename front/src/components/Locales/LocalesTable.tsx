import React, { useState } from "react";
import { ModalTurno } from "../Turnos/ModalTurno.tsx";
import { ModalLocal } from "./ModalLocal.tsx";
import { ModalService } from "../Servicio/ModalService.tsx";
// interface
import type {
  LocalesTableProps,
  LocalesType,
} from "../../interface/LocalesType.ts";
import {
  DayOfWeek,
  type HorarioAtencionType,
} from "../../interface/HorarioAtencionType.ts";
import { deletedLocal } from "../../service/api.ts";
import toast from "react-hot-toast";

export const LocalesTable: React.FC<LocalesTableProps> = ({
  data,
  user,
  onDeleteSuccess,
}) => {
  const [selectedLocal, setSelectedLocal] = useState<LocalesType | null>(null);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isModalOpenService, setIsModalOpenService] = useState(false);

  const handleDelete = async (id: string) => {
    const confirmation = window.confirm(
      "¿Estas seguro de que queres eliminar este local?",
    );
    if (!confirmation) return;

    try {
      const response = await deletedLocal(id);
      toast.success(response.message);
      if (onDeleteSuccess) onDeleteSuccess(id);
    } catch (error: unknown) {
      const errorObject = error as Error;
      toast.error(errorObject.message);
    }
  };

  const handleAgregar = () => {
    setSelectedLocal(null);
    setIsModalOpen(true);
  };

  const handleEditar = (local: LocalesType) => {
    setSelectedLocal(local);
    setIsModalOpen(true);
  };

  const handleAddDeleteUpdateService = (local: LocalesType) => {
    setSelectedLocal(local);
    setIsModalOpenService(true);
  };

  const handleCerrarModalService = () => {
    setIsModalOpenService(false);
    setSelectedLocal(null);
  };

  const handleCerrarModal = () => {
    setIsModalOpen(false);
    setSelectedLocal(null);
  };

  if (user.rol === "Cliente")
    return (
      <div>
        <table>
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Descripcion</th>
              <th>Categoria</th>
              <th>ImagenURL</th>
              <th>Direccion</th>
              <th>Telefono</th>
              <th>Servicios</th>
              <th>Horarios de Atencion</th>
              <th>Horarios disponibles</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {data.map((local) => (
              <tr key={local.id}>
                <td>{local.name}</td>
                <td>{local.description}</td>
                <td>{local.category}</td>
                <td>{local.direction}</td>
                <td>{local.phone}</td>
                <td>{local.imageURL}</td>
                <td>
                  {local.servicios.length > 0
                    ? local.servicios.map((s) => s.name).join(", ")
                    : "Sin servicios"}
                </td>
                <td>
                  {local.horariosAtencion.length > 0
                    ? local.horariosAtencion
                        .map(
                          (h) =>
                            `${h.estaCerrado ? "Cerrado" : "Abierto"}: ${DayOfWeek[Number(h.diaSemana) as keyof typeof DayOfWeek]}: ${h.horaApertura.substring(0, 5)} - ${h.horaCierre.substring(0, 5)}`,
                        )
                        .join(", ")
                    : "Sin horarios"}
                </td>

                <td>
                  <button onClick={() => setSelectedLocal(local)}>
                    Pedir turno
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {selectedLocal !== null && (
          <ModalTurno
            cerrar={() => setSelectedLocal(null)}
            usuarioId={user.id}
            localId={selectedLocal.id}
            servicios={selectedLocal.servicios}
          />
        )}
      </div>
    ); // Local
  else
    return (
      <div>
        <button onClick={handleAgregar}>+ Agregar local</button>
        <table>
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Descripcion</th>
              <th>Categoria</th>
              <th>ImagenURL</th>
              <th>Direccion</th>
              <th>Telefono</th>
              <th>Servicios</th>
              <th>Horarios de Atencion</th>
            </tr>
          </thead>
          <tbody>
            {data.map((local) => (
              <tr key={local.id}>
                <td>{local.name}</td>
                <td>{local.description}</td>
                <td>{local.category}</td>
                <td>{local.direction}</td>
                <td>{local.phone}</td>
                <td>{local.imageURL}</td>
                <td>
                  {local.servicios.length > 0
                    ? local.servicios.map((s) => s.name).join(", ")
                    : "Sin servicios"}
                </td>
                <td>
                  {local.horariosAtencion.length > 0
                    ? local.horariosAtencion
                        .map(
                          (h) =>
                            `${h.estaCerrado ? "Cerrado" : "Abierto"}: ${DayOfWeek[Number(h.diaSemana) as keyof typeof DayOfWeek]}: ${h.horaApertura.substring(0, 5)} - ${h.horaCierre.substring(0, 5)}`,
                        )
                        .join(", ")
                    : "Sin horarios"}
                </td>
                <td>
                  <button onClick={() => handleDelete(local.id)}>
                    Eliminar local
                  </button>
                </td>
                <td>
                  <button onClick={() => handleEditar(local)}>
                    Modificar local
                  </button>
                </td>
                <td>
                  <button onClick={() => handleAddDeleteUpdateService(local)}>
                    Servicios
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {isModalOpen && (
          <ModalLocal
            cerrar={handleCerrarModal}
            usuarioId={user.id}
            localToEdit={selectedLocal} // Si es null, el modal sabe que es una CREACIÓN
            onSuccess={() => {
              // Acá podés refrescar la tabla si tenés la función
            }}
          />
        )}
        {isModalOpenService && selectedLocal && (
          <ModalService
            cerrar={handleCerrarModalService}
            local={selectedLocal} // Si es null, el modal sabe que es una CREACIÓN
            usuarioId={user.id}
            onSuccess={() => {
              // Acá podés refrescar la tabla si tenés la función
            }}
          />
        )}
      </div>
    );
};
