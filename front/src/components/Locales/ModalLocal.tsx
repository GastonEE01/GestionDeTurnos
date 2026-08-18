import React, { useState } from 'react'
import type { ServicioType } from "../../interface/ServicioType";
import type { UpdateLocalDto } from '../../interface/LocalesType';
import type { LocalesType } from '../../interface/LocalesType';
import type {DayOfWeek} from '../../interface/HorarioAtencionType'
import type { HorarioAtencionType } from '../../interface/HorarioAtencionType';

import { updateLocal } from '../../service/api';
import { createLocal } from '../../service/api';

/*interface ModalLocalProps{
    cerrar: () => void;
    localId: string; //  Agrego el idLocal para saber a donde pertenece el turno 
    localToEdit?: LocalesType | null; // Si viene, es EDICIÓN; si es null/undefined, es CREACIÓN
    //servicios: ServicioType[];
    usuarioId: string;
    onSuccess?: () => void; // Para recargar la lista al terminar
}*/

export interface ModalLocalProps {
  cerrar: () => void;
  usuarioId: string;
  localToEdit?: LocalesType | null; // null = Crear nuevo | Objeto = Editar existente
  onSuccess?: () => void;
}

export const ModalLocal = ({cerrar,usuarioId,localToEdit,onSuccess} : ModalLocalProps) => {
    const [message, setMessage] = useState<string>("",);
    const [loading,setLoading] = useState(false)
    const isEditing = Boolean(localToEdit);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setMessage("");
    setLoading(true);

    const formData = new FormData(e.currentTarget);

     
    try{
        if(isEditing && localToEdit){
    const updateDto : UpdateLocalDto = {
        // editar
        name: formData.get("name") as string,
        description: formData.get("description") as string,
        category: formData.get("category") as string,
        imageURL: formData.get("imageURL") as string,
        direction: formData.get("direction") as string,
        phone: formData.get("phone") as string, 
    };
    await updateLocal(localToEdit.id, updateDto);
        setMessage("Local actualizado con éxito");
}else{
    // crear local
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

    const addHorario : HorarioAtencionType = {
        diaSemana: Number(formData.get("diaSemana")) as keyof typeof DayOfWeek,
        horaApertura: formData.get("horaApertura") as string,
        horaCierre: formData.get("horaCierre") as string,
        estaCerrado: false,

    };
    await createLocal(addLocal, usuarioId, addHorario);
        setMessage("Local creado con éxito");
}
if(onSuccess) onSuccess();
setTimeout(() => {
        cerrar();
      }, 1000);
    } catch (error: unknown) {
      const err = error as Error;
      console.error("Error en la operación:", err);
      setMessage(err.message || "Ocurrió un error inesperado");
    } finally {
      setLoading(false);
    }
  };

    return (
    <div>
      <h1>modal</h1>
      <h2>{localToEdit ? "Editar Local" : "Crear Nuevo Local"}</h2>
      <button type="button" onClick={cerrar}>X</button>
      <form onSubmit={handleSubmit}>
        <label>Nombre:</label>
        <input name="name" type="text" defaultValue={localToEdit?.name || ""} required />

        <label>Descripcion</label>
        <input name="description" type="text" defaultValue={localToEdit?.name || ""} required />

        <label>Categoria</label>
        <input name="category" type="text" defaultValue={localToEdit?.name || ""} required />

        <label>Imagen</label>
        <input name="imageURL" type="text" defaultValue={localToEdit?.name || ""} required/>

        <label>Direccion</label>
        <input name="direction" type="text" defaultValue={localToEdit?.name || ""} required/>

        <label>Telefono</label>
        <input name="phone" type="text" defaultValue={localToEdit?.name || ""} required/>

        {/* CAMPOS DE HORARIO SOLO PARA CREACIÓN */}
        {!isEditing && (
          <>
            <hr />
            <h3>Horario de Atención Inicial</h3>
            <label>Día de la Semana (0 = Domingo, 1 = Lunes, etc.):</label>
            <select name="diaSemana" defaultValue="1" required>
              <option value="1">Lunes</option>
              <option value="2">Martes</option>
              <option value="3">Miércoles</option>
              <option value="4">Jueves</option>
              <option value="5">Viernes</option>
              <option value="6">Sábado</option>
              <option value="0">Domingo</option>
            </select>

            <label>Hora Apertura:</label>
            <input name="horaApertura" type="time" defaultValue="08:00" required />

            <label>Hora Cierre:</label>
            <input name="horaCierre" type="time" defaultValue="18:00" required />
          </>
        )}
        <button type="submit" disabled={loading}>
            {loading ? "Guardando..." : "Guardar"}
          </button>

      </form>
      {message && <h2>{message}</h2>}
    </div>
  );
};