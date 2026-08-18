import { useEffect, useState,useRef } from "react";
import type { LocalesType } from '../../interface/LocalesType.ts';
import type { ServicioType } from '../../interface/ServicioType.ts';
import { getServiceLocal } from '../../service/api.ts'
import { deleteService } from "../../service/api.ts";
import { updateService } from "../../service/api.ts";
import { addService } from "../../service/api.ts";
/*
export interface ModalLocalProps {
  cerrar: () => void;
  usuarioId: string;
  localToEdit?: LocalesType | null; // null = Crear nuevo | Objeto = Editar existente
  onSuccess?: () => void;
}

export const ModalLocal = ({cerrar,usuarioId,localToEdit,onSuccess} : ModalLocalProps) => {
    const [message, setMessage] = useState<string>("",);
    const [loading,setLoading] = useState(false)
    const isEditing = Boolean(localToEdit);*/

export interface ModalServiceProps {
    cerrar: () => void;
    onSuccess?: () => void;
    usuarioId: string,
    local: LocalesType;
    onDeleteSuccess?: (id: string) => void;
}

export const ModalService = ({cerrar,local,usuarioId,onDeleteSuccess} : ModalServiceProps) => {
  const [servicios, setServicios] = useState<ServicioType[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string>("");

  const [modalAddService,setModalAddSercice] = useState<boolean>(false);
  const formRef = useRef<HTMLFormElement>(null);

  // Guarda el ID del servicio que se está editando en este momento (o null si ninguno)
  const [editingId, setEditingId] = useState<string | null>(null);
  // Guarda los datos temporales que el usuario va escribiendo en los inputs
  const [editFormData, setEditFormData] = useState({ name: "", description: "", durationInMinutes: "", price: 0 });

  // cargar servicios
  useEffect(() => {
    const services = async () => {
        try{
            setLoading(true);
            const data = await getServiceLocal(local.id);
            setServicios(data);
        } catch(err: unknown){
            const errorObject = err as Error;
            setError(errorObject.message || "Error al cargar servicios");
        } finally{
            setLoading(false);
        }
        };
        services();
    },[local.id]);
  

     const handleDelete = async (servicioId: string) => {
        const confirmation = window.confirm("¿Estas seguro de que queres eliminar este local?")
        if(!confirmation) return;
    
        try{
          await deleteService(local.id,servicioId);
          if(onDeleteSuccess)
            onDeleteSuccess(servicioId); // no se para que se usa
        } catch(err: any){
            console.log(err.message || "Error al intentar eliminar el servicio");        }
      };
    

      const handleIniciarEdit = (servicio: ServicioType) => {
  setEditingId(servicio.id);
  setEditFormData({ 
      name: servicio.name,
      description: servicio.description || "",
      durationInMinutes: servicio.durationInMinutes ? String(servicio.durationInMinutes) : "",
      price: servicio.price 
  });
};

      const handleSaveEdit = async (servicioId: string) => {
  try {
    // Acá llamás a tu API: await updateServicio(id, editFormData);
    await updateService(local.id, servicioId, editFormData);
    
    // Actualizamos la lista local
      setServicios(servicios.map(s => 
        s.id === servicioId ? { ...s, ...editFormData, durationInMinutes: Number(editFormData.durationInMinutes) } : s
      ));

    // Salimos del modo edición
    setEditingId(null);
  } catch (err) {
    console.error("Error al actualizar servicio", err);
  }
};

const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
   e.preventDefault();
   setLoading(true);
    // Capturar los datos del formulario
   const formData = new FormData(e.currentTarget);

   // Armar el objeto
   const createServiceData: ServicioType = {
    id: "",
    usuarioId: usuarioId,
    localId: local.id,
    name: formData.get("name") as string,
    description: formData.get("description") as string,
    durationInMinutes: parseInt(formData.get("durationInMinutes") as string || "0", 10), 
    price: Number(formData.get("price") || 0),
   }

    // Opcional aca puedo hacer alguna validacion antes de mandarlo al back
    try{
        const serviceCreated = await addService(local.id,createServiceData);
        setServicios(prevServicios => [...prevServicios, serviceCreated]);
        console.log("Servicio agregado");
        setModalAddSercice(false);
        formRef.current?.reset();
        }catch(error){
        console.log("Error al agregar el servicio", error)
    }
    finally{
        setLoading(false);
    }
};
     /* {servicios.map((service) => {
    <button type="button" onClick={cerrar}>X</button>
  const isEditing = editingId === service.id;*/
return (
    <div className="modal">
      <h2>Servicios de {local.name}</h2>
      <button type="button" onClick={cerrar}>X</button>

      <button type="button" onClick={() => setModalAddSercice(!modalAddService)}>
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
      {error && <p style={{ color: 'red' }}>{error}</p>}

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
                        onChange={(e) => setEditFormData({ ...editFormData, name: e.target.value })}
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
                        onChange={(e) => setEditFormData({ ...editFormData, description: e.target.value })}
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
                        onChange={(e) => setEditFormData({ ...editFormData, durationInMinutes: e.target.value })}
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
                        onChange={(e) => setEditFormData({ ...editFormData, price: Number(e.target.value) })}
                      />
                    ) : (
                      `$${service.price}`
                    )}
                  </td>

                  {/* ACCIONES */}
                  <td>
                    {isEditing ? (
                      <>
                        <button onClick={() => handleSaveEdit(service.id)}>Guardar</button>
                        <button onClick={() => setEditingId(null)}>Cancelar</button>
                      </>
                    ) : (
                      <>
                        <button onClick={() => handleIniciarEdit(service)}>Editar</button>
                        <button onClick={() => handleDelete(service.id)}>Eliminar</button>
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




/*

  return (
    <div>
       
       <button type="button" onClick={cerrar}>X</button>

       {loading && <p>Cargando servicios...</p>}
       {error && <p style={{ color: 'red' }}>{error}</p>}
       
       {!loading && !error && (
        <>
        {servicios.length === 0 ? (
          <p>Este local no tiene servicios registrados</p>
        ): (
            <table>
                <thead>
                    <tr>
                        <th>Nombre</th>
                        <th>Descripcion</th>
                        <th>Duracion</th>
                        <th>Precio</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody>
                    {servicios.map((servicio) => (
                        
                        <tr key={servicio.id}>
                            <td>{servicio.name}</td>
                            <td>{servicio.description}</td>
                            <td>{servicio.durationInMinutes} min</td>
                            <td>${servicio.price}</td>
                            <td>
                      <button onClick={() => alert(`Editar ${servicio.name}`)}>Editar</button>
                      <button onClick={() => handleDelete(servicio.id)}>Eliminar</button>
                    </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        )}
        </>
       )}
    </div>
  );*/