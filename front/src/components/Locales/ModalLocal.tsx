import React, { useState } from "react";
import { X, Building2, MapPin, Phone, Image, Tag, Clock } from "lucide-react";
import type { UpdateLocalDto } from "../../interface/LocalesType";
import type { LocalesType } from "../../interface/LocalesType";
import type { DayOfWeek } from "../../interface/HorarioAtencionType";
import type { HorarioAtencionType } from "../../interface/HorarioAtencionType";
import toast from "react-hot-toast";

import { updateLocal } from "../../service/api";
import { createLocal } from "../../service/api";

export interface ModalLocalProps {
  cerrar: () => void;
  usuarioId: string;
  localToEdit?: LocalesType | null;
  onSuccess?: () => void;
}

export const ModalLocal = ({
  cerrar,
  usuarioId,
  localToEdit,
  onSuccess,
}: ModalLocalProps) => {
  const [message, setMessage] = useState<string>("");
  const [loading, setLoading] = useState(false);
  const isEditing = Boolean(localToEdit);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setMessage("");
    setLoading(true);

    const formData = new FormData(e.currentTarget);

    try {
      if (isEditing && localToEdit) {
        const updateDto: UpdateLocalDto = {
          name: formData.get("name") as string,
          description: formData.get("description") as string,
          category: formData.get("category") as string,
          imageURL: formData.get("imageURL") as string,
          direction: formData.get("direction") as string,
          phone: formData.get("phone") as string,

        };
        const response = await updateLocal(localToEdit.id, updateDto);
        toast.success(response.message);
      } else {
        const addLocal = {
          name: formData.get("name") as string,
          description: formData.get("description") as string,
          category: formData.get("category") as string,
          imageURL: formData.get("imageURL") as string,
          direction: formData.get("direction") as string,
          phone: formData.get("phone") as string,
          servicios: [],
          horariosAtencion: [],
        };

        const addHorario: Omit<HorarioAtencionType, "localId"> = {
          diaSemana: Number(
            formData.get("diaSemana"),
          ) as keyof typeof DayOfWeek,
          horaApertura: formData.get("horaApertura") as string,
          horaCierre: formData.get("horaCierre") as string,
          estaCerrado: false,
        };
        const response = await createLocal(addLocal, usuarioId, addHorario);
        toast.success(response.message);
      }
      if (onSuccess) onSuccess();
      setTimeout(() => {
        cerrar();
      }, 1000);
    } catch (error: unknown) {
      const err = error as Error;
      setMessage(err.message || "Ocurrió un error inesperado");
    } finally {
      setLoading(false);
    }
  };

 return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="bg-white rounded-3xl shadow-2xl w-full max-w-lg p-6 relative border border-gray-100 my-8">

        {/* Header */}
        <div className="flex justify-between items-start mb-6">
          <div>
            <span className="text-xs font-bold uppercase tracking-wider text-sky-900">
              {isEditing ? "CONFIGURACIÓN" : "NUEVO ESTABLECIMIENTO"}
            </span>
            <h2 className="text-2xl font-black text-gray-900 leading-tight">
              {isEditing ? "Editar Local" : "Crear Nuevo Local"}
            </h2>
          </div>
          <button type="button" onClick={cerrar} className="text-gray-400 hover:text-gray-600 transition-colors p-1 rounded-full hover:bg-gray-100">
            <X size={20} />
          </button>
        </div>

        {/* Formulario (Corregido: ahora abre con > en lugar de /> ) */}
        <form onSubmit={handleSubmit} className="space-y-4">
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="md:col-span-2">
              <label className="block text-xs font-semibold text-gray-700 mb-1">Nombre del Local</label>
              <div className="relative">
                <input
                  name="name"
                  type="text"
                  defaultValue={localToEdit?.name || ""}
                  required
                  className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl pl-9 pr-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                />
                <Building2 className="absolute left-3 top-2.5 text-gray-400" size={16} />
              </div>
            </div>

            <div className="md:col-span-2">
              <label className="block text-xs font-semibold text-gray-700 mb-1">Descripción</label>
              <input
                name="description"
                type="text"
                defaultValue={localToEdit?.description || ""}
                required
                className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
              />
            </div>

            <div>
              <label className="block text-xs font-semibold text-gray-700 mb-1">Categoría</label>
              <div className="relative">
                <input
                  name="category"
                  type="text"
                  defaultValue={localToEdit?.category || ""}
                  required
                  className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl pl-9 pr-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                />
                <Tag className="absolute left-3 top-2.5 text-gray-400" size={16} />
              </div>
            </div>

            <div>
              <label className="block text-xs font-semibold text-gray-700 mb-1">Teléfono</label>
              <div className="relative">
                <input
                  name="phone"
                  type="text"
                  defaultValue={localToEdit?.phone || ""}
                  placeholder="Ej: 1122334455"
                  required
                  className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl pl-9 pr-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                />
                <Phone className="absolute left-3 top-2.5 text-gray-400" size={16} />
              </div>
            </div>

            <div className="md:col-span-2">
              <label className="block text-xs font-semibold text-gray-700 mb-1">URL de Imagen</label>
              <div className="relative">
                <input
                  name="imageURL"
                  type="text"
                  defaultValue={localToEdit?.imageURL || ""}
                  placeholder="https://..."
                  required
                  className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl pl-9 pr-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                />
                <Image className="absolute left-3 top-2.5 text-gray-400" size={16} />
              </div>
            </div>

            <div className="md:col-span-2">
              <label className="block text-xs font-semibold text-gray-700 mb-1">Dirección</label>
              <div className="relative">
                <input
                  name="direction"
                  type="text"
                  defaultValue={localToEdit?.direction || ""}
                  placeholder="Ej: Av. San Martín 123"
                  required
                  className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl pl-9 pr-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                />
                <MapPin className="absolute left-3 top-2.5 text-gray-400" size={16} />
              </div>
            </div>
          </div>

          {/* Horario Inicial solo al crear (Corregido eliminando código duplicado previo) */}
          {!isEditing && (
            <div className="border-t border-gray-100 pt-4 mt-2">
              <div className="flex items-center gap-2 mb-3">
                <Clock size={16} className="text-sky-900" />
                <h3 className="text-xs font-bold uppercase tracking-wider text-sky-900">
                  Horario de Atención Inicial
                </h3>
              </div>

              <div className="grid grid-cols-1 md:grid-cols-3 gap-3">
                <div>
                  <label className="block text-xs font-medium text-gray-600 mb-1">Día</label>
                  <select
                    name="diaSemana"
                    defaultValue="1"
                    required
                    className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl p-2 text-xs focus:outline-none focus:ring-2 focus:ring-sky-500 cursor-pointer"
                  >
                    <option value="1">Lunes</option>
                    <option value="2">Martes</option>
                    <option value="3">Miércoles</option>
                    <option value="4">Jueves</option>
                    <option value="5">Viernes</option>
                    <option value="6">Sábado</option>
                    <option value="0">Domingo</option>
                  </select>
                </div>

                <div>
                  <label className="block text-xs font-medium text-gray-600 mb-1">Apertura</label>
                  <input
                    name="horaApertura"
                    type="time"
                    defaultValue="08:00"
                    required
                    className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl p-2 text-xs focus:outline-none focus:ring-2 focus:ring-sky-500"
                  />
                </div>

                <div>
                  <label className="block text-xs font-medium text-gray-600 mb-1">Cierre</label>
                  <input
                    name="horaCierre"
                    type="time"
                    defaultValue="18:00"
                    required
                    className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl p-2 text-xs focus:outline-none focus:ring-2 focus:ring-sky-500"
                  />
                </div>
              </div>
            </div>
          )}

          {/* Mensaje de error interno */}
          {message && (
            <p className="text-xs text-red-600 bg-red-50 p-2.5 rounded-xl text-center font-medium">
              {message}
            </p>
          )}

          {/* Acciones */}
          <div className="flex items-center gap-3 pt-4 border-t border-gray-100">
            <button
              type="button"
              onClick={cerrar}
              className="w-1/2 border border-sky-100 text-gray-700 font-semibold py-2.5 rounded-xl text-sm hover:bg-gray-50 transition-colors"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={loading}
              className="w-1/2 bg-amber-300 hover:bg-amber-400 text-amber-950 font-semibold py-2.5 rounded-xl text-sm transition-all shadow-sm disabled:opacity-50"
            >
              {loading ? "Guardando..." : "Guardar"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}