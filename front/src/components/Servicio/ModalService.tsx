import { useEffect, useState, useRef } from "react";
import type { LocalesType } from "../../interface/LocalesType.ts";
import type { ServicioDtoRequest } from "../../interface/ServicioType.ts";
import type { ServicioDtoResponse } from "../../interface/ServicioType.ts";
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
}

export const ModalService: React.FC<ModalServiceProps> = ({
  cerrar,
  local,
  usuarioId,
}) => {
  const [servicios, setServicios] = useState<ServicioDtoResponse[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>("");

  const [modalAddService, setModalAddSercice] = useState<boolean>(false);
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
      setModalAddSercice(false);
      formRef.current?.reset();
    } catch (error: any) {
      toast.error(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal">
      <h2>Servicios de {local.name}</h2>
      <button type="button" onClick={cerrar}>
        X
      </button>

      <button
        type="button"
        onClick={() => setModalAddSercice(!modalAddService)}
      >
        {modalAddService ? "Cancelar" : "+ Agregar servicio"}
      </button>

      <div>
        {modalAddService && (
          <form onSubmit={handleSubmit}>
            <label>Ingrese en nombre:</label>
            <input name="name" type="text" />

            <label>Ingrese la descripcion</label>
            <input name="description" type="text" />

            <label>Ingrese la duracion/min</label>
            <input name="durationInMinutes" type="text" />

            <label>Ingrese el precio</label>
            <input name="price" type="text" />
            <button type="submit">Agregar</button>
          </form>
        )}
      </div>

      {loading && <p>Cargando servicios...</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}

      {!loading && !error && (
        <table>
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Descripción</th>
              <th>Duración (min)</th>
              <th>Precio</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {servicios.map((service) => {
              const isEditing = editingId === service.id;

              return (
                <tr key={service.id}>
                  {/* NOMBRE */}
                  <td>
                    {isEditing ? (
                      <input
                        type="text"
                        value={editFormData.name}
                        onChange={(e) =>
                          setEditFormData({
                            ...editFormData,
                            name: e.target.value,
                          })
                        }
                      />
                    ) : (
                      service.name
                    )}
                  </td>

                  {/* DESCRIPCIÓN */}
                  <td>
                    {isEditing ? (
                      <input
                        type="text"
                        value={editFormData.description}
                        onChange={(e) =>
                          setEditFormData({
                            ...editFormData,
                            description: e.target.value,
                          })
                        }
                      />
                    ) : (
                      service.description || "-"
                    )}
                  </td>

                  {/* DURACIÓN */}
                  <td>
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
                      />
                    ) : (
                      `${service.durationInMinutes} min`
                    )}
                  </td>

                  {/* PRECIO */}
                  <td>
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
                      />
                    ) : (
                      `$${service.price}`
                    )}
                  </td>

                  {/* ACCIONES */}
                  <td>
                    {isEditing ? (
                      <>
                        <button onClick={() => handleSaveEdit(service.id)}>
                          Guardar
                        </button>
                        <button onClick={() => setEditingId(null)}>
                          Cancelar
                        </button>
                      </>
                    ) : (
                      <>
                        <button onClick={() => handleIniciarEdit(service)}>
                          Editar
                        </button>
                        <button onClick={() => handleDelete(service.id)}>
                          Eliminar
                        </button>
                      </>
                    )}
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      )}
    </div>
  );
};
