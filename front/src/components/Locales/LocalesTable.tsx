import React, { useState } from "react";
import { ModalTurno } from "../Turnos/ModalTurno.tsx";
import { ModalLocal } from "./ModalLocal.tsx";
import { ModalService } from "../Servicio/ModalService.tsx";
import { 
  Store, 
  MapPin, 
  ArrowRight, 
  Plus, 
  Trash2, 
  Edit3, 
  Layers 
} from "lucide-react";

// interface
import type {
  LocalesTableProps,
  LocalesType,
} from "../../interface/LocalesType.ts";
/*import {
  DayOfWeek,
  type HorarioAtencionType,
} from "../../interface/HorarioAtencionType.ts";*/

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

// VISTA CLIENTE
  if (user.rol === "Cliente") {
    return (
      <div className="w-full">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {data.map((local) => (
            <div
              key={local.id}
              className="bg-white rounded-2xl border border-gray-100 p-6 shadow-sm hover:shadow-md transition-all flex flex-col justify-between"
            >
              <div>
                <div className="flex items-start justify-between mb-4">
                  <div className="w-12 h-12 bg-sky-900 rounded-xl flex items-center justify-center text-white overflow-hidden shrink-0">
                    {local.imageURL && local.imageURL.startsWith("http") ? (
                      <img
                        src={local.imageURL}
                        alt={local.name}
                        className="w-full h-full object-cover"
                      />
                    ) : (
                      <Store size={24} />
                    )}
                  </div>
                  <span className="px-3 py-1 rounded-full bg-amber-100 text-amber-800 text-xs font-semibold">
                    Abierto
                  </span>
                </div>

                <h3 className="text-lg font-bold text-gray-900 leading-tight">
                  {local.name}
                </h3>
                <p className="text-sm font-medium text-gray-500 mb-3">
                  {local.category}
                </p>

                <div className="space-y-2 text-sm text-gray-600 mb-6">
                  <div className="flex items-center gap-2">
                    <MapPin size={16} className="text-gray-400 shrink-0" />
                    <span className="truncate">{local.direction}</span>
                  </div>

                  <p className="text-xs text-gray-500 pt-1">
                    <span className="font-semibold text-gray-700">
                      {local.servicios.length} servicios
                    </span>{" "}
                    disponibles
                  </p>
                </div>
              </div>

              <button
                onClick={() => setSelectedLocal(local)}
                className="w-full bg-sky-900 hover:bg-sky-950 text-white font-semibold py-3 px-4 rounded-xl flex items-center justify-center gap-2 transition-colors shadow-sm"
              >
                <span>Ver disponibilidad</span>
                <ArrowRight size={18} />
              </button>
            </div>
          ))}
        </div>

        {selectedLocal !== null && (
          <ModalTurno
            cerrar={() => setSelectedLocal(null)}
            usuarioId={user.id}
            localId={selectedLocal.id}
            servicios={selectedLocal.servicios}
          />
        )}
      </div>
    );
  }

  // VISTA DUEÑO DE LOCAL / ADMIN
  return (
    <div className="w-full space-y-6">
      <div className="flex justify-between items-center">
        <h2 className="text-xl font-bold text-gray-800">Mis Locales</h2>
        <button
          onClick={handleAgregar}
          className="bg-sky-900 hover:bg-sky-950 text-white px-4 py-2 rounded-xl text-sm font-semibold flex items-center gap-2 shadow-sm transition-colors"
        >
          <Plus size={18} />
          Agregar local
        </button>
      </div>

      <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-gray-50 border-b border-gray-100 text-gray-600 text-xs font-bold uppercase tracking-wider">
                <th className="p-4">Local</th>
                <th className="p-4">Categoría</th>
                <th className="p-4">Dirección / Tel</th>
                <th className="p-4">Servicios</th>
                <th className="p-4 text-right">Acciones</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100 text-sm">
              {data.map((local) => (
                <tr key={local.id} className="hover:bg-gray-50/50 transition-colors">
                  <td className="p-4 font-semibold text-gray-900">
                    {local.name}
                  </td>
                  <td className="p-4 text-gray-600">{local.category}</td>
                  <td className="p-4 text-gray-600">
                    <div>{local.direction}</div>
                    <div className="text-xs text-gray-400">{local.phone}</div>
                  </td>
                  <td className="p-4 text-gray-600">
                    {local.servicios.length > 0
                      ? local.servicios.map((s) => s.name).join(", ")
                      : "Sin servicios"}
                  </td>
                  <td className="p-4 text-right">
                    <div className="flex items-center justify-end gap-2">
                      <button
                        onClick={() => handleAddDeleteUpdateService(local)}
                        className="p-2 text-indigo-600 hover:bg-indigo-50 rounded-lg transition-colors"
                        title="Gestionar Servicios"
                      >
                        <Layers size={18} />
                      </button>
                      <button
                        onClick={() => handleEditar(local)}
                        className="p-2 text-gray-600 hover:bg-gray-100 rounded-lg transition-colors"
                        title="Editar Local"
                      >
                        <Edit3 size={18} />
                      </button>
                      <button
                        onClick={() => handleDelete(local.id)}
                        className="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                        title="Eliminar Local"
                      >
                        <Trash2 size={18} />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {isModalOpen && (
        <ModalLocal
          cerrar={handleCerrarModal}
          usuarioId={user.id}
          localToEdit={selectedLocal}
          onSuccess={() => {}}
        />
      )}
      {isModalOpenService && selectedLocal && (
        <ModalService
          cerrar={handleCerrarModalService}
          local={selectedLocal}
          usuarioId={user.id}
          onSuccess={() => {}}
        />
      )}
    </div>
  );
};

/*

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
        </table>*/