// import React from 'react'
import { useState } from "react";
import type { ServicioType } from '../../interface/ServicioType.ts';
import {addTurno} from '../../service/api.ts'

interface ModalTurnoProps{
    cerrar: () => void;
    localId: string; //  Agrego el idLocal para saber a donde pertenece el turno 
    servicios: ServicioType[];
    usuarioId: string;
}
export const ModalTurno = ({cerrar, usuarioId,localId, servicios} : ModalTurnoProps) => {   
        const [loading,setLoading] = useState(false);

        //const [turno,setTurno] = useState<TurnosProps[]>([]);
        /* const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault(); 
    setLoading(true);
        
        // 1 Leemos los datos del formulario
        const formData = new FormData(e.currentTarget);
        const nameTurno = formData.get('turno') as string;
        const selectedDate = formData.get('fecha') as string;

        // 2 Construimos el objeto respetando la estructura de TurnosType
        // Nota: Le mandamos los datos minimos que necestia el Backend.
        // Convertimos la fecha a un numero timestamp o string segun pida tu backend.
        const turno = {
          name: nameTurno,
          date : selectedDate, // Convierte la fecha a número si tu tipo dice 'number'
          servicioId: 1, // reemplazar por el id de servicio correspondiente
          locales: [{id:localId}] // se suele enviar vacio al crear o mapear segun el backend 
        };
             
        // 3 Hacemos la peticion
        try{
          await addTurno(turno);
          alert("Turno guardado con exito");
          cerrar();
        } catch (error) {
          console.error("Error al guardar el turno: ", error);
          alert("No se pude guardar el turno.");
        } finally {
          setLoading(false);
        }
      };*/

      //  AGREGAMOS 'async' AQUÍ:
      //  AGREGAMOS 'async' AQUÍ:
const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault(); 
    setLoading(true);

    const formData = new FormData(e.currentTarget);
    const selectedDate = formData.get('fecha') as string;
    const selectedServicioId = String(formData.get('servicio'));

    const turno = {
      date: selectedDate, 
      servicioId: selectedServicioId, 
      localId: localId,
      usuarioId: usuarioId
    };
         
    // En ModalTurno.tsx dentro de handleSubmit
try {
  const respuesta = await addTurno(turno);
  
  // respuesta ahora contiene { id, date, message } enviado por tu return Ok() de C#
  alert(respuesta.message || "¡Turno guardado con éxito!"); 
  cerrar();
} catch (error) {
  console.error("Error al guardar el turno: ", error);
  alert("No se pudo guardar el turno.");
}
    /*try {
      // Ahora sí, el await está permitido porque su función contenedora es async
      await addTurno(turno);
      alert("Turno guardado con éxito");
      cerrar();
    } catch (error) {
      console.error("Error al guardar el turno: ", error);
      alert("No se pudo guardar el turno.");
    } finally {
      setLoading(false);
    }*/
};
/*const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
      e.preventDefault(); 
    setLoading(true);

    const formData = new FormData(e.currentTarget);
    const nameTurno = formData.get('turno') as string;
    const selectedDate = formData.get('fecha') as string;

    const turno = {
      name: nameTurno,
      date: selectedDate, 
      servicioId: 1, 
      locales: [{ id: localId }] 
    };
         
    try {
      // Ahora sí, el await está permitido porque su función contenedora es async
      await addTurno(turno);
      alert("Turno guardado con éxito");
      cerrar();
    } catch (error) {
      console.error("Error al guardar el turno: ", error);
      alert("No se pudo guardar el turno.");
    } finally {
      setLoading(false);
    }
};*/
  return (
    <div>
        <form onSubmit={handleSubmit}>
        <button type="button" onClick={cerrar}>X</button>
        <label htmlFor="">Seleccione el servicio:</label>
        <select name="servicio" required>
          <option value="">Elija un servicio</option>
          {servicios.map((s) => (
            <option key={s.id} value={s.id}>
            {s.name}
            </option>
          ))}
        </select>
        <label >Fecha del turno:</label>
      <input type="date" name="fecha" required/>
      <button type="submit" disabled={loading}>{loading ? 'Enviando...' : 'Enviar'}</button>
</form>
    </div>
  )

}

/*
  TurnosType turno = () => {
             name = nombreTurno,
             date = fecha,
             locales = LocalesTableProps.id
          }
          const loading = async () => (
            try{
                     const data = await addTurno(turno);
                     setTurno(data);
                   } catch (error){
                     console.error("Error al traer locales: ", error);
                   }
                 }
                
               },[])
               */