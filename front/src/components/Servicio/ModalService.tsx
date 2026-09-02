import { useEffect, useState, useRef } from "react";
import { X, Plus, Trash2, Edit2, Check, Clock } from "lucide-react";
import type { LocalesType } from "../../interface/LocalesType.ts";
import type { ServicioDtoRequest,ServicioDtoResponse} from "../../interface/ServicioType.ts";
import {
  getServiceLocal,
  deleteService,
  updateService,
  addService,
} from "../../service/api.ts";
import toast from "react-hot-toast";

export interface ModalServiceProps {
  cerrar: () => void;
  usuarioId: string;
  local: LocalesType;
  onSuccess?: () => void; 
}

export const ModalService: React.FC<ModalServiceProps> = ({
  cerrar,
  local,
  usuarioId,
  onSuccess,
}) => {
  const [servicios, setServicios] = useState<ServicioDtoResponse[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>("");

  const [modalAddService, setModalAddService] = useState<boolean>(false);
  const formRef = useRef<HTMLFormElement>(null);

  const [editingId, setEditingId] = useState<string | null>(null);
  const [editFormData, setEditFormData] = useState({
    name: "",
    description: "",
    durationInMinutes: "",
    price: 0,
  });

  useEffect(() => {
    const services = async () => {
      try {
        setLoading(true);
        const data = await getServiceLocal(local.id);
        setServicios(data);
      } catch (err: unknown) {
        const errorObject = err as Error;
        setError(errorObject.message || "Error al cargar servicios");
      } finally {
        setLoading(false);
      }
    };
    services();
  }, [local.id]);

  const handleDelete = async (servicioId: string) => {
    const confirmation = window.confirm(
      "¿Estas seguro de que queres eliminar este local?",
    );
    if (!confirmation) return;

    try {
      const response = await deleteService(local.id, servicioId);
      setServicios((prev) => prev.filter((s) => s.id !== servicioId));
      toast.success(response.message);
    } catch (error: unknown) {
      const errorObject = error as Error;
      toast.error(errorObject.message);
    }
  };

  const handleIniciarEdit = (servicio: ServicioDtoResponse) => {
    setEditingId(servicio.id);
    setEditFormData({
      name: servicio.name,
      description: servicio.description || "",
      durationInMinutes: servicio.durationInMinutes
        ? String(servicio.durationInMinutes)
        : "",
      price: servicio.price,
    });
  };

  const handleSaveEdit = async (servicioId: string) => {
    try {
      await updateService(local.id, servicioId, editFormData);

      setServicios(
        servicios.map((s) =>
          s.id === servicioId
            ? {
                ...s,
                ...editFormData,
                durationInMinutes: Number(editFormData.durationInMinutes),
              }
            : s,
        ),
      );
      toast.success("Servicio actualizado");
      setEditingId(null);
    } catch (err) {
      const errorObject = err as Error;
      toast.error(errorObject.message || "Error al actualizar servicio");
    }
  };

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLoading(true);

    const formData = new FormData(e.currentTarget);

    const createServiceData: ServicioDtoRequest = {
      usuarioId: usuarioId,
      name: formData.get("name") as string,
      description: formData.get("description") as string,
      durationInMinutes: parseInt(
        (formData.get("durationInMinutes") as string) || "0",
        10,
      ),
      price: Number(formData.get("price") || 0),
    };

    try {
      const response = await addService(local.id, createServiceData);
      setServicios((prevServicios) => [...prevServicios, response]);
      toast.success(response.message);
      setModalAddService(false);
      formRef.current?.reset();
      if (onSuccess) onSuccess();
    } catch (error: unknown) {
      const errorObject = error as Error;
      toast.error(errorObject.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm p-4 overflow-y-auto">
      <div className="bg-white rounded-3xl shadow-2xl w-full max-w-4xl p-6 relative border border-gray-100 my-8">
        
        {/* Header */}
        <div className="flex justify-between items-start mb-5">
          <div>
            <span className="text-xs font-bold uppercase tracking-wider text-sky-900">
              GESTIÓN DE SERVICIOS
            </span>
            <h2 className="text-2xl font-black text-gray-900 leading-tight">
              Servicios de {local.name}
            </h2>
          </div>
          <button
            onClick={cerrar}
            className="text-gray-400 hover:text-gray-600 transition-colors p-1 rounded-full hover:bg-gray-100"
          >
            <X size={20} />
          </button>
        </div>

        {/* Botón Agregar Servicio */}
        <div className="flex justify-end mb-4">
          <button
            type="button"
            onClick={() => setModalAddService(!modalAddService)}
            className="flex items-center gap-1.5 bg-sky-900 hover:bg-sky-950 text-white text-xs font-bold py-2 px-3.5 rounded-xl transition-all shadow-sm"
          >
            {modalAddService ? <X size={14} /> : <Plus size={14} />}
            {modalAddService ? "Cancelar" : "Agregar servicio"}
          </button>
        </div>

        {/* Formulario Crear Servicio */}
        {modalAddService && (
          <form
            ref={formRef}
            onSubmit={handleSubmit}
            className="bg-sky-50/60 border border-sky-100 p-4 rounded-2xl mb-5 space-y-3"
          >
            <h3 className="text-xs font-bold text-sky-900 uppercase tracking-wider">
              Nuevo Servicio
            </h3>
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-1">Nombre</label>
                <input
                  name="name"
                  type="text"
                  required
                  className="w-full bg-white border border-sky-100 rounded-xl px-3 py-1.5 text-xs focus:outline-none focus:ring-2 focus:ring-sky-500"
                />
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-1">Descripción</label>
                <input
                  name="description"
                  type="text"
                  required
                  className="w-full bg-white border border-sky-100 rounded-xl px-3 py-1.5 text-xs focus:outline-none focus:ring-2 focus:ring-sky-500"
                />
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-1">Duración (min)</label>
                <input
                  name="durationInMinutes"
                  type="number"
                  required
                  className="w-full bg-white border border-sky-100 rounded-xl px-3 py-1.5 text-xs focus:outline-none focus:ring-2 focus:ring-sky-500"
                />
              </div>

              <div>
                <label className="block text-xs font-semibold text-gray-700 mb-1">Precio ($)</label>
                <input
                  name="price"
                  type="number"
                  required
                  className="w-full bg-white border border-sky-100 rounded-xl px-3 py-1.5 text-xs focus:outline-none focus:ring-2 focus:ring-sky-500"
                />
              </div>
            </div>

            <div className="flex justify-end pt-2">
              <button
                type="submit"
                disabled={loading}
                className="bg-amber-300 hover:bg-amber-400 text-amber-950 font-semibold px-4 py-2 rounded-xl text-xs transition-all shadow-sm"
              >
                {loading ? "Guardando..." : "Agregar Servicio"}
              </button>
            </div>
          </form>
        )}

        {/* Estados de Carga / Error */}
        {loading && !modalAddService && (
          <p className="text-xs text-sky-800 bg-sky-50 p-3 rounded-xl flex items-center gap-2">
            <Clock size={16} className="animate-spin" /> Cargando servicios...
          </p>
        )}

        {error && <p className="text-xs text-red-600 bg-red-50 p-3 rounded-xl mb-4">{error}</p>}

        {/* Tabla de Servicios */}
        {!loading && !error && (
          <div className="border border-sky-100 rounded-2xl overflow-x-auto shadow-sm">
            <table className="w-full text-left text-xs text-gray-700 min-w-[550px]">
              <thead className="bg-sky-50/50 text-gray-400 font-bold uppercase border-b border-sky-100">
                <tr>
                  <th className="py-3 px-4">Nombre</th>
                  <th className="py-3 px-4">Descripción</th>
                  <th className="py-3 px-4">Duración</th>
                  <th className="py-3 px-4">Precio</th>
                  <th className="py-3 px-4 text-center">Acciones</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-sky-50">
                {servicios.length === 0 ? (
                  <tr>
                    <td colSpan={5} className="py-6 text-center text-gray-400 italic">
                      No hay servicios registrados para este local.
                    </td>
                  </tr>
                ) : (
                  servicios.map((service) => {
                    const isEditing = editingId === service.id;

                    return (
                      <tr key={service.id} className="hover:bg-sky-50/30 transition-colors">
                        <td className="py-3 px-4 font-semibold text-gray-900 whitespace-nowrap">
                          {isEditing ? (
                            <input
                              type="text"
                              value={editFormData.name}
                              onChange={(e) =>
                                setEditFormData({ ...editFormData, name: e.target.value })
                              }
                              className="bg-white border rounded px-2 py-1 text-xs w-full"
                            />
                          ) : (
                            service.name
                          )}
                        </td>

                        <td className="py-3 px-4 text-gray-500 max-w-[150px] truncate">
                          {isEditing ? (
                            <input
                              type="text"
                              value={editFormData.description}
                              onChange={(e) =>
                                setEditFormData({ ...editFormData, description: e.target.value })
                              }
                              className="bg-white border rounded px-2 py-1 text-xs w-full"
                            />
                          ) : (
                            service.description || "-"
                          )}
                        </td>

                        <td className="py-3 px-4 whitespace-nowrap">
                          {isEditing ? (
                            <input
                              type="text"
                              value={editFormData.durationInMinutes}
                              onChange={(e) =>
                                setEditFormData({
                                  ...editFormData,
                                  durationInMinutes: e.target.value,
                                })
                              }
                              className="bg-white border rounded px-2 py-1 text-xs w-16"
                            />
                          ) : (
                            `${service.durationInMinutes} min`
                          )}
                        </td>

                        <td className="py-3 px-4 font-medium text-gray-800 whitespace-nowrap">
                          {isEditing ? (
                            <input
                              type="number"
                              value={editFormData.price}
                              onChange={(e) =>
                                setEditFormData({
                                  ...editFormData,
                                  price: Number(e.target.value),
                                })
                              }
                              className="bg-white border rounded px-2 py-1 text-xs w-20"
                            />
                          ) : (
                            `$${service.price}`
                          )}
                        </td>

                        <td className="py-3 px-4 text-center whitespace-nowrap">
                          {isEditing ? (
                            <div className="flex justify-center gap-1">
                              <button
                                onClick={() => handleSaveEdit(service.id)}
                                className="p-1 text-green-600 hover:bg-green-50 rounded transition-colors"
                              >
                                <Check size={16} />
                              </button>
                              <button
                                onClick={() => setEditingId(null)}
                                className="p-1 text-green-600 hover:bg-green-50 rounded transition-colors"
                              >
                                <X size={16} />
                              </button>
                            </div>
                          ) : (
                            <div className="flex justify-center gap-2">
                              <button
                                onClick={() => handleIniciarEdit(service)}
                                className="text-sky-700 hover:text-sky-900 transition-colors p-1"
                              >
                                <Edit2 size={15} />
                              </button>
                              <button
                                onClick={() => handleDelete(service.id)}
                                className="text-sky-700 hover:text-sky-900 transition-colors p-1"
                              >
                                <Trash2 size={15} />
                              </button>
                            </div>
                          )}
                        </td>
                      </tr>
                    );
                  })
                )}
              </tbody>
            </table>
          </div>
        )}

      </div>
    </div>
  );
};




