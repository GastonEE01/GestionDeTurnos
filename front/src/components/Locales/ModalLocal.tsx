import React, { useState } from "react";
import { X, Building2, MapPin, Phone, Image } from "lucide-react";
import type { LocalesType } from "../../interface/LocalesType";
import type { DayOfWeek } from "../../interface/HorarioAtencionType";
import type { HorarioAtencionType } from "../../interface/HorarioAtencionType";
import type { LoginDtoResponse } from "../../interface/LoginType";
import toast from "react-hot-toast";
import { asociarLocalForUser } from "../../service/api";

import { updateLocal } from "../../service/api";

const CATEGORIAS_LOCALES = [
  "Peluquería y Barbería",
  "Estética y Spa",
  "Canchas y Deportes",
  "Salud y Odontología",
  "Gastronomía y Bares",
  "Indumentaria y Ropa",
  "Veterinaria y Mascotas",
  "Taller Mecánico",
  "Talleres y Clases",
  "Otros Servicios",
];

const DIAS_SEMANA = [
  { label: "Lunes", value: 1 },
  { label: "Martes", value: 2 },
  { label: "Miércoles", value: 3 },
  { label: "Jueves", value: 4 },
  { label: "Viernes", value: 5 },
  { label: "Sábado", value: 6 },
  { label: "Domingo", value: 0 },
];

export interface ModalLocalProps {
  cerrar: () => void;
  user: LoginDtoResponse;
  localToEdit?: LocalesType | null;
  onSuccess?: () => void;
}

export const ModalLocal = ({
  cerrar,
  user,
  localToEdit,
  onSuccess,
}: ModalLocalProps) => {
  const [loading, setLoading] = useState(false);
  const isEditing = Boolean(localToEdit);
  const [selectedDay, setSelectedDay] = useState<string>("");

  const [openingTime, setOpeningTime] = useState<string>("08:00");
  const [closingTime, setClosingTime] = useState<string>("18:00");
  const [horariosList, setHorariosList] = useState<HorarioAtencionType[]>(
    () => {
      return localToEdit && localToEdit.horariosAtencion
        ? localToEdit.horariosAtencion
        : [];
    },
  );

  const handleAddHorario = () => {
    if (selectedDay === "") {
      toast.error("Selecciona un día para agregar");
      return;
    }

    const nuevoHorario: HorarioAtencionType = {
      localId: "",
      diaSemana: Number(selectedDay) as keyof typeof DayOfWeek,
      horaApertura: openingTime,
      horaCierre: closingTime,
      estaCerrado: false,
    };

    setHorariosList([...horariosList, nuevoHorario]);
    setSelectedDay("");
  };

  const handleRemoveHorario = (index: number) => {
    setHorariosList(horariosList.filter((_, i) => i !== index));
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLoading(true);

    const formData = new FormData(e.currentTarget);

    try {
      if (isEditing && localToEdit) {
        // --- CASO EDITAR ---
        const updateDto = {
          name: formData.get("name") as string,
          description: formData.get("description") as string,
          category: formData.get("category") as string,
          imageURL: formData.get("imageURL") as string,
          direction: formData.get("direction") as string,
          phone: formData.get("phone") as string,
          horariosAtencion: horariosList,
        };

        const response = await updateLocal(
          localToEdit.id,
          updateDto,
          horariosList,
        );
        toast.success(response.message);
      } else {
        // --- CASO CREAR (el que acabamos de hacer) ---
        const local = {
          name: formData.get("name") as string,
          description: formData.get("description") as string,
          category: formData.get("category") as string,
          imageURL: formData.get("imageURL") as string,
          direction: formData.get("direction") as string,
          phone: formData.get("phone") as string,
          servicios: [],
          horariosAtencion: horariosList,
        };

        const rest = await asociarLocalForUser(user.id, local, horariosList);
        toast.success(rest.message || "Local asociado con éxito");
      }

      if (onSuccess) onSuccess();
      setTimeout(() => {
        cerrar();
      }, 1000);
    } catch (error: unknown) {
      const errorObject = error as Error;
      toast.error(errorObject.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="bg-white rounded-3xl shadow-2xl w-full max-w-3xl p-6 relative border border-gray-100 flex flex-col max-h-[90vh]">
        {/* Header */}
        <div className="flex justify-between items-center mb-4 pb-2 border-b border-gray-100">
          <div>
            <span className="text-[10px] font-bold uppercase tracking-wider text-sky-900">
              {isEditing ? "CONFIGURACIÓN" : "NUEVO ESTABLECIMIENTO"}
            </span>
            <h2 className="text-2xl font-black text-gray-900 leading-tight">
              {isEditing ? "Editar Local" : "Crear Nuevo Local"}
            </h2>
          </div>
          <button
            type="button"
            onClick={cerrar}
            className="text-gray-400 hover:text-gray-600 transition-colors p-1 rounded-full hover:bg-gray-100"
          >
            <X size={20} />
          </button>
        </div>

        {/* Formulario */}
        <form
          onSubmit={handleSubmit}
          className="flex flex-col flex-1 overflow-hidden"
        >
          {/* Contenedor de 2 Columnas */}
          <div className="overflow-y-auto pr-2 grid grid-cols-1 md:grid-cols-2 gap-4 flex-1">
            {/* COLUMNA IZQUIERDA: Inputs de texto */}
            <div className="space-y-3">
              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-1">
                  Nombre del Local
                </label>
                <div className="relative">
                  <input
                    name="name"
                    type="text"
                    defaultValue={localToEdit?.name || ""}
                    required
                    className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl pl-9 pr-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                  />
                  <Building2
                    className="absolute left-3 top-2.5 text-gray-400"
                    size={16}
                  />
                </div>
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-1">
                  Descripción
                </label>
                <input
                  name="description"
                  type="text"
                  defaultValue={localToEdit?.description || ""}
                  required
                  className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                />
              </div>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">
                  Categoría
                </label>
                <select
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black text-sm"
                  required
                  name="category"
                  defaultValue={localToEdit?.category || ""}
                >
                  <option value="" disabled>
                    Selecciona una categoría
                  </option>
                  {CATEGORIAS_LOCALES.map((cat) => (
                    <option key={cat} value={cat}>
                      {cat}
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-1">
                  Teléfono
                </label>
                <div className="relative">
                  <input
                    name="phone"
                    type="text"
                    defaultValue={localToEdit?.phone || ""}
                    placeholder="Ej: 1122334455"
                    required
                    className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl pl-9 pr-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                  />
                  <Phone
                    className="absolute left-3 top-2.5 text-gray-400"
                    size={16}
                  />
                </div>
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-1">
                  URL de Imagen
                </label>
                <div className="relative">
                  <input
                    name="imageURL"
                    type="text"
                    defaultValue={localToEdit?.imageURL || ""}
                    placeholder="https://..."
                    required
                    className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl pl-9 pr-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                  />
                  <Image
                    className="absolute left-3 top-2.5 text-gray-400"
                    size={16}
                  />
                </div>
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-1">
                  Dirección
                </label>
                <div className="relative">
                  <input
                    name="direction"
                    type="text"
                    defaultValue={localToEdit?.direction || ""}
                    placeholder="Ej: Av. San Martín 123"
                    required
                    className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl pl-9 pr-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500"
                  />
                  <MapPin
                    className="absolute left-3 top-2.5 text-gray-400"
                    size={16}
                  />
                </div>
              </div>
            </div>

            {/* COLUMNA DERECHA: Horarios */}
            <div className="rounded-2xl border border-gray-200 bg-gray-50 p-5 text-left flex flex-col justify-between">
              <div>
                <h3 className="text-base font-semibold text-gray-900 mb-1">
                  Horarios de Atención
                </h3>
                <p className="text-xs text-gray-500 mb-4">
                  Agregá los días y horarios en los que estará abierto tu local.
                </p>

                <div className="space-y-3 mb-4">
                  <div>
                    <label className="block text-xs font-semibold text-gray-700 mb-1">
                      Día
                    </label>
                    <select
                      value={selectedDay}
                      onChange={(e) => setSelectedDay(e.target.value)}
                      className="w-full rounded-xl border border-gray-300 bg-white px-2.5 py-1.5 text-xs text-gray-900 outline-none"
                    >
                      <option value="" disabled>
                        Elegí un día
                      </option>
                      {DIAS_SEMANA.map((d) => (
                        <option key={d.value} value={d.value}>
                          {d.label}
                        </option>
                      ))}
                    </select>
                  </div>

                  <div>
                    <label className="block text-xs font-semibold text-gray-700 mb-1">
                      Apertura
                    </label>
                    <input
                      type="time"
                      value={openingTime}
                      onChange={(e) => setOpeningTime(e.target.value)}
                      className="w-full rounded-xl border border-gray-300 bg-white px-1.5 py-1.5 text-xs text-gray-900 outline-none"
                    />
                  </div>

                  <div>
                    <label className="block text-xs font-semibold text-gray-700 mb-1">
                      Cierre
                    </label>
                    <input
                      type="time"
                      value={closingTime}
                      onChange={(e) => setClosingTime(e.target.value)}
                      className="w-full rounded-xl border border-gray-300 bg-white px-1.5 py-1.5 text-xs text-gray-900 outline-none"
                    />
                  </div>
                </div>

                <button
                  type="button"
                  onClick={handleAddHorario}
                  className="bg-sky-900 text-white text-xs font-semibold px-3 py-2 rounded-xl hover:bg-sky-800 transition-colors w-full"
                >
                  Agregar Horario
                </button>

                {/* Listado de agregados */}
                {horariosList.length > 0 && (
                  <div className="mt-3 space-y-2 border-t border-gray-200 pt-2 max-h-40 overflow-y-auto">
                    {horariosList.map((item, index) => (
                      <div
                        key={index}
                        className="flex justify-between items-center bg-white p-2 rounded-lg border text-xs"
                      >
                        <span>
                          Día {item.diaSemana}: {item.horaApertura} -{" "}
                          {item.horaCierre}
                        </span>
                        <button
                          type="button"
                          onClick={() => handleRemoveHorario(index)}
                          className="text-red-500 font-semibold hover:underline"
                        >
                          Eliminar
                        </button>
                      </div>
                    ))}
                  </div>
                )}
              </div>
            </div>
          </div>

          {/* Botones de Acción (Fuera del grid de 2 columnas, dentro del form) */}
          <div className="flex items-center gap-3 pt-4 mt-4 border-t border-gray-100 bg-white">
            <button
              type="button"
              onClick={cerrar}
              className="w-1/2 border border-gray-200 text-gray-700 font-semibold py-2.5 rounded-xl text-xs hover:bg-gray-50 transition-colors"
            >
              Cancelar
            </button>
            <button
              type="submit"
              disabled={loading}
              className="w-1/2 bg-amber-300 hover:bg-amber-400 text-amber-950 font-semibold py-2.5 rounded-xl text-xs transition-all shadow-sm disabled:opacity-50"
            >
              {loading ? "Guardando..." : "Guardar"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};
